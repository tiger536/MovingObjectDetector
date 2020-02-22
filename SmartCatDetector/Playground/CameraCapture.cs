//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ClassLibrary1;
using ClassLibrary1.Implementations;
using ClassLibrary1.Models;
using ClassLibrary1.Extensions;
using Emgu.CV;
using Emgu.CV.BgSegm;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Collections.Concurrent;
using ClassLibrary1.MatQueuer;

namespace Playground
{
    public partial class CameraCapture : Form
    {
        private VideoCapture _capture = null;
        private IMatReady _matGenerator;

        private IWorker _worker;


        public CameraCapture(IMatReady matGenerator, IWorker iworker)
        {
            InitializeComponent();
            
            _matGenerator = matGenerator;
            _worker = iworker;
            _matGenerator.MatReady += matReady;
            _worker.DnnResultsReady += displayResults;
        }

        public float[] GetRow(float[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
        }


        private void displayResults(object sender, WorkerResultsReadyArgs arg)
        {
            CvInvoke.Resize(arg.NewestMat, arg.NewestMat, new Size(captureImageBox.Width, captureImageBox.Height));
            captureImageBox.Image = arg.NewestMat;
            if (arg.TestMat != null && !arg.TestMat.IsEmpty)
            {
                out2.Image = arg.TestMat;
            }
        }

     /*   private void singleMatReady(object sender, MatReadyCompletedArgs arg)
        {
            Mat myNewMat = arg.Frame;
   //          Mat blob = Emgu.CV.Dnn.DnnInvoke.BlobFromImage(image:myNewMat,size:new Size(416,416));
            Mat blob = Emgu.CV.Dnn.DnnInvoke.BlobFromImage(myNewMat, 0.00392, new Size(416, 416), new MCvScalar(0, 0, 0), true, false);

            var net = _iDnnProvider.GetNet();
            net.SetInput(blob);


            var tensors = new VectorOfMat();
            net.Forward(tensors, GetOutputLayers(net));
           

            var listaConfidence = new List<float>();
            var listaClassiOutput = new List<string>();

            using (Mat boxes = tensors[0])
            using (Mat masks = tensors[1])
            {
                float[,] boxesData = boxes.GetData(true) as float[,];
                for (int i = 0; i < boxes.Rows; i++)
                {
                    var boxData = GetRow(boxesData, i);
                    var confidence = boxData.Skip(5).Max();
                    var maxIndex = boxData.ToList().IndexOf(confidence);

                    if(confidence > 0.5)
                    {
                        listaConfidence.Add(confidence);
                        listaClassiOutput.Add(listaClassi[boxData.ToList().IndexOf(confidence)-5]);
                    }
                }
            }

            CvInvoke.Resize(arg.Frame, arg.Frame, new Size(captureImageBox.Width, captureImageBox.Height));
            captureImageBox.Image = arg.Frame;
        }
        */
        private void matReady(object sender, MatReadyCompletedArgs arg)
        {
            if (arg.Frame != null && !arg.Frame.IsEmpty)
            {
                _worker.AddWork(arg.Frame);
            }


         //   if (first == null)
         //   {
         //       first = cloned;
         //       return;
         //   }
         //   Mat ahaha = new Mat();
 

         //   //_forgroundDetector.Apply(cloned, ahaha);
         //   //_motionHistory.Update(ahaha);

         //   CvInvoke.AbsDiff(first, cloned,ahaha);        

         //   Mat element = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1));

         //   CvInvoke.Threshold(ahaha, ahaha, 24, 255, ThresholdType.Binary);
         //   Mat test = new Mat(ahaha.Size, ahaha.Depth, 3);

         //   CvInvoke.Dilate(ahaha, ahaha, element
         //, new Point(-1, -1)
         //, 6
         //, BorderType.Constant
         //, new MCvScalar(0, 0, 0));


         //   VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
         //   VectorOfVectorOfPoint contoursOut = new VectorOfVectorOfPoint();
         //   var diz2 = new List<Rectangle>();
         //   var puntiFinali = new VectorOfPoint();
         //   CvInvoke.FindContours(ahaha, contours, new Mat(), RetrType.External, ChainApproxMethod.ChainApproxSimple);

         //   for (int i = 0; i < contours.Size; i++)
         //   {
         //       var contourApprox = new VectorOfPoint();
         //       CvInvoke.ApproxPolyDP(contours[i], contourApprox, CvInvoke.ArcLength(contours[i], true) * 0.02, true);
         //       contoursOut.Push(contourApprox);

         //       var rect = CvInvoke.BoundingRectangle(contourApprox);

         //       if (puntiFinali.Size == 0)
         //       {
         //           puntiFinali.Push(new Point[] { rect.Location, rect.BottomLocation() });
         //           diz2.Add(rect);
         //           continue;
         //       }

         //       bool toBeAdded = false;
         //       foreach (var rettangolo in diz2)
         //       {
         //           if (rettangolo.CanIIncludeThisRectangleInAnother(rect, 0.5))
         //           {
         //               puntiFinali.Push(new Point[] { rect.Location, rect.BottomLocation() });
         //               toBeAdded = true;
         //               continue;
         //           }
         //       }
         //       if (toBeAdded) diz2.Add(rect);
         //   }

         //   var finalRect = CvInvoke.BoundingRectangle(puntiFinali);
         ////   CvInvoke.Rectangle(arg.Frame, finalRect, new MCvScalar(0, 255, 0), 5);

         //   VectorOfVectorOfPoint conaaatours = new VectorOfVectorOfPoint();

         //   Mat myNewMat = new Mat(arg.Frame, finalRect);
         //   CvInvoke.Rotate(myNewMat, myNewMat, RotateFlags.Rotate90Clockwise);
         // Mat blob = Emgu.CV.Dnn.DnnInvoke.BlobFromImage(image:myNewMat,size:new Size(416,416));
        //    Mat blob = Emgu.CV.Dnn.DnnInvoke.BlobFromImage(myNewMat, 0.00392f, new Size(416, 416), new MCvScalar(0,0,0), true, false);

        //    var net = _iDnnProvider.GetNet();
        //    net.SetInput(blob);

      //      var tensors = new VectorOfMat();
      ////      net.Forward(tensors, GetOutputLayers(net));

      //      var listaConfidence = new List<float>();
      //      var listaClassiOutput = new List<string>();

            //using (Mat boxes = tensors[0])
            //using (Mat masks = tensors[1])
            //{
            //    float[,] boxesData = boxes.GetData(true) as float[,];
            ////  float boxData = boxesData[0,0..^84] as float[];
            //    for(int i = 0; i< boxes.Rows;i++)
            //    {
            //        var boxData = GetRow(boxesData, i);
            //        var confidence = boxData[4];
            //        if(confidence > 0.3)
            //        {
            //            listaConfidence.Add(confidence);
            //            var maxI = boxData.Skip(4).Max();
            //            listaClassiOutput.Add(listaClassi[boxData.ToList().IndexOf(maxI)]);
            //        }
            //    }
            // }
            //int classId = 0;
            //Mat outblob = ciao[0];
            //double classProb = 0;

            //getMaxClass(ref outblob, ref classId, ref classProb);

            // var bestClass = listaClassi[classId];
            //  var ciao = net.Forward();

            //for (int i = 0; i < ciao.Rows; i++)
            //{
            //    var cazzo = (ciao.Row(i));
            //    var cazzoImmagine = cazzo.ToImage<Bgr, float>();
            //    var confidence = cazzoImmagine[0, 4].Green;
            //    int maxI = 0;
            //    double max = 0.0;


            //    if (confidence > 0.3)
            //    {
            //        for (int e = 5; e < cazzoImmagine.Cols; e++)
            //        {
            //            if ((cazzoImmagine[0, e].Green) > max)
            //            {
            //                max = (cazzoImmagine[0, e].Green);
            //                maxI = e - 5;
            //            }
            //        }
            //        var classe = listaClassi[maxI];
            //        //  CvInvoke.MinMaxLoc(cazzo,);
            //        // Cv2.MinMaxLoc(prob.Row[i].ColRange(prefix, prob.Cols), out _, out Point max);
            //        //CvInvoke.MinMaxLoc(;
            //    }
            //}

            /*
            for (int i = 0; i < contoursOut.Size; i++)
            {
                CvInvoke.DrawContours(test, contoursOut, i, new MCvScalar(255, 0, 0), 10);
            }


            first = cloned.Clone();
            CvInvoke.Resize(arg.Frame, arg.Frame, new Size(captureImageBox.Width, captureImageBox.Height));
            CvInvoke.Resize(cloned, cloned, new Size(captureImageBox.Width, captureImageBox.Height));
            CvInvoke.Resize(ahaha, ahaha, new Size(captureImageBox.Width, captureImageBox.Height));
            CvInvoke.Resize(test, test, new Size(captureImageBox.Width, captureImageBox.Height));


            captureImageBox.Image = arg.Frame;
            outputImageBox.Image = cloned;
            if (!myNewMat.IsEmpty) out2.Image = myNewMat;

            base.Invoke((Action)delegate { processingTimeBox.Text = arg.ProcessingTime.ToString(); });
            */
        }

        private void captureButtonClick(object sender, EventArgs e)
        {
            _matGenerator.Start();
        }

        private void ReleaseData()
        {
            if (_capture != null)
                _capture.Dispose();
        }

        private void PauseCapureButtonClick(object sender, EventArgs e)
        {
            _matGenerator.Pause();
        }


        private string[] GetOutputLayers(Emgu.CV.Dnn.Net net )
        {
            var layersName = net.LayerNames;
            var lista = new List<string>();
            foreach(int layerID in net.UnconnectedOutLayers)
            {
                lista.Add(net.LayerNames[layerID -1]);
            }
            return lista.ToArray();
            
        }

        void getMaxClass(ref Mat probBlob, ref int classId, ref double classProb)
        {
            Mat probMat = probBlob.Reshape(1, 1); //reshape the blob to 1x1000 matrix
            Point classNumber = new Point();

            var tmp = new Point();
            double tmpdouble = 0;
            CvInvoke.MinMaxLoc(probMat, ref tmpdouble, ref classProb, ref tmp, ref classNumber);

            classId = classNumber.X;
        }
    }
}
