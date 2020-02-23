using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectDetection.Implementations
{
    public class VideoMat : IMatReady
    {
       
        private VideoCapture _capture = null;
        private double processingTime;
        private int frameRate = 1;
        private DateTimeOffset last = new DateTimeOffset(DateTime.Now);
        public event EventHandler<MatReadyCompletedArgs> MatReady;

        public VideoMat()
        {
            _capture = new VideoCapture(ConfigurationManager.AppSettings["ExampleVideo"]);
            _capture.Start();
            frameRate = (int)_capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);
            _capture.ImageGrabbed += ProcessFrame;

        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                Mat mat = new Mat();
                _capture.Retrieve(mat);

                if (mat == null || mat.IsEmpty)
                {
                    _capture.Stop();
                    return;
                }
                NotifyNow(mat);
            }
            processingTime = ((new DateTimeOffset(DateTime.Now)) - last).TotalMilliseconds;
            var waitTime = (1000 / frameRate) - (int)processingTime;
            if (waitTime < 0) waitTime = 0;
            Thread.Sleep(waitTime);
            last = new DateTimeOffset(DateTime.Now);
        }

        private void NotifyNow(Mat mat)
        {
            var handler = MatReady;
            if (handler == null)
                return;

            handler(this, new MatReadyCompletedArgs(mat, processingTime));
        }

        public void Pause()
        {
            _capture.Pause();
        }

        public void Start()
        {
            _capture.Start();
        }
    }
}
