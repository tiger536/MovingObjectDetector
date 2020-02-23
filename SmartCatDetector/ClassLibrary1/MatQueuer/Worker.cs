using ObjectDetection.Extensions;
using ObjectDetection.Implementations;
using ObjectDetection.Implementations.Filters;
using ObjectDetection.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectDetection.MatQueuer
{
    public class Worker : IWorker
    {
        private FixedSizeQueue<Mat> matQueuer { get; set; }
        private int MaxSize { get; set; }
        private int RotateImageClockwise { get; set; }
        private Task Processor { get; set; }
        private IDnnProvider _provider;
        private CancellationTokenSource TaskCancellationTokenSource { get; set; }

        public event EventHandler<WorkerResultsReadyArgs> DnnResultsReady;


        public Worker(IDnnProvider provider)
        {
            _provider = provider;
            MaxSize = Convert.ToInt32(ConfigurationManager.AppSettings["FrameQueueSize"]);
            RotateImageClockwise = Convert.ToInt32(ConfigurationManager.AppSettings["RotateImageClockwise"]);
            matQueuer = new FixedSizeQueue<Mat>(MaxSize);
            TaskCancellationTokenSource = new CancellationTokenSource();
            Processor = Task.Run(async () => await Start(TaskCancellationTokenSource.Token));           
        }

        private async Task Start(CancellationToken token)
        {
            while(true)
            {
                try
                {
                    if (matQueuer.Count < this.MaxSize)
                    {
                        await Task.Delay(60);
                        continue;
                    }

                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    var matList = matQueuer.DequeueAll(); // index 0 is oldest Mat
                    var oldestFrame = matList[0];
                    var newestFrame = matList[MaxSize - 1];                  

                    if (RotateImageClockwise > 0)
                    {
                        CvInvoke.Rotate(newestFrame, newestFrame, GetRotateFlags());
                        CvInvoke.Rotate(oldestFrame, oldestFrame, GetRotateFlags());
                    }
                    var mat = new Mat();

                    var oldestFrameGray = new Mat();
                    var newestFrameGray = new Mat();
                    CvInvoke.CvtColor(oldestFrame, oldestFrameGray, ColorConversion.Bgr2Gray);
                    CvInvoke.CvtColor(newestFrame, newestFrameGray, ColorConversion.Bgr2Gray);

                    List<IMatOperation> filterList = new List<IMatOperation>();
                    filterList.Add(new GaussianBlur(oldestFrameGray));
                    filterList.Add(new GaussianBlur(newestFrameGray));
                    filterList.Add(new AbsDiff(oldestFrameGray, newestFrameGray, mat));
                    filterList.Add(new Thresolder(mat));
                    filterList.Add(new Dilater(mat));

                    foreach (var filter in filterList)
                    {
                        filter.Execute();
                    }

                    VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                    var diz2 = new List<Rectangle>();
                    var puntiFinali = new VectorOfPoint();
                    CvInvoke.FindContours(mat, contours, new Mat(), RetrType.External, ChainApproxMethod.ChainApproxSimple);

                    for (int i = 0; i < contours.Size; i++)
                    {
                        var contourApprox = new VectorOfPoint();
                        CvInvoke.ApproxPolyDP(contours[i], contourApprox, CvInvoke.ArcLength(contours[i], true) * 0.02, true);

                        var rect = CvInvoke.BoundingRectangle(contourApprox);

                        if (puntiFinali.Size == 0)
                        {
                            puntiFinali.Push(new Point[] { rect.Location, rect.BottomLocation() });
                            diz2.Add(rect);
                            continue;
                        }

                        bool toBeAdded = false;
                        foreach (var rettangolo in diz2)
                        {
                            if (rettangolo.CanIIncludeThisRectangleInAnother(rect, 0.5))
                            {
                                puntiFinali.Push(new Point[] { rect.Location, rect.BottomLocation() });
                                toBeAdded = true;
                                continue;
                            }
                        }
                        if (toBeAdded) diz2.Add(rect);
                    }

                    Mat testMat = null;
                    if (puntiFinali.Size > 0)
                    {
                        var finalRect = CvInvoke.BoundingRectangle(puntiFinali);
                        finalRect.Inflate((int)(finalRect.Width * 0.21),(int) (finalRect.Height * 0.21));
                        finalRect = finalRect.ResizeToFitMat(newestFrame.Width, newestFrame.Height);
#if DEBUG
                        testMat = new Mat(newestFrame, finalRect).Clone();
#endif
                        _provider.SetInputMat(new Mat(newestFrame, finalRect));
                        var results = _provider.Forward();
#if DEBUG
                        if (results.ClassesList.Any())
                        {
                            CvInvoke.Rectangle(newestFrame, finalRect, new MCvScalar(0, 0, 255), 5);
                            CvInvoke.PutText(newestFrame, results.bestIndex >= 0 ? $"{(results.ClassesList[results.bestIndex]).ToUpper()} {Math.Round(results.ConfidenceList[results.bestIndex] * 100)/100}" : String.Empty,
                                new Point(finalRect.X - 10, finalRect.Y - 10), FontFace.HersheySimplex, 1.5, new MCvScalar(0, 0, 255), 3);
                        }
#endif
                        watch.Stop();
                        NotifyNow(new WorkerResultsReadyArgs()
                        {
                            NewestMat = newestFrame,
                            TestMat = testMat,
                            BestMatch = results.bestIndex >=0 ? results.ClassesList[results.bestIndex]: String.Empty,
                            BestConfidence = results.bestIndex >= 0 ? results.ConfidenceList[results.bestIndex] : 0.0f,
                            ProcessingTime = watch.ElapsedMilliseconds,
                            DectectionOut = results.ClassesList
                        }) ;


                    }
                    oldestFrameGray.Dispose();
                    newestFrameGray.Dispose();
                    oldestFrame.Dispose();
                    mat.Dispose();
                    contours.Dispose();
#if RELEASE
                    newestFrame.Dispose();
#endif
                }
                catch (Exception e)
                {
                    //DO SOMETHING
                }
            }

        }

        private void NotifyNow(WorkerResultsReadyArgs args)
        {
            var handler = DnnResultsReady;
            if (handler == null)
                return;

            handler(this, args);
        }

        public void AddWork(Mat mat)
        {
            matQueuer.Enqueue(mat);
        }

        private RotateFlags GetRotateFlags()
        {
            switch(RotateImageClockwise)
            {
                case 90:
                default:
                    return RotateFlags.Rotate90Clockwise;
                case 180:
                    return RotateFlags.Rotate180;
                case 270:
                    return RotateFlags.Rotate90CounterClockwise;
            }
        }
    }
}
