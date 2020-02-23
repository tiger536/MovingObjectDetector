using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace ObjectDetection.Implementations
{
    public class ImageMat : IMatReady
    {
       
        private VideoCapture _capture = null;
        private Mat _frame =  new Mat();

        public event EventHandler<MatReadyCompletedArgs> MatReady;

        public ImageMat()
        {
            _frame = CvInvoke.Imread(ConfigurationManager.AppSettings["ExampleImage"]);
            NotifyNow();
        }

        private async void NotifyNow()
        {
            var handler = MatReady;
            while(handler == null)
            {
                await Task.Run(() => Task.Delay(500));
                handler = MatReady;
            }
            if (handler == null)
                return;

            handler(this, new MatReadyCompletedArgs(_frame));
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
