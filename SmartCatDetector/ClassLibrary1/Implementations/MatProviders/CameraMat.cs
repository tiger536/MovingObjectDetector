using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1.Implementations
{
    public class CameraMat : IMatReady
    {
       
        private VideoCapture _capture = null;
        private Mat _frame =  new Mat();

        public event EventHandler<MatReadyCompletedArgs> MatReady;

        public CameraMat()
        {
            _capture = new VideoCapture();
            _capture.Start();
            _capture.ImageGrabbed += ProcessFrame;
        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                _capture.Retrieve(_frame, 0);
                NotifyNow();
            }
        }

        private void NotifyNow()
        {
            var handler = MatReady;
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
