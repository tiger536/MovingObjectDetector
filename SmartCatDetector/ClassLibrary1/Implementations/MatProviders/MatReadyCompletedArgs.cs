using Emgu.CV;
using System;

namespace ObjectDetection.Implementations
{
    public class MatReadyCompletedArgs : EventArgs
    {
        public MatReadyCompletedArgs(Mat frame)
        {
            Frame = frame;
        }

        public MatReadyCompletedArgs(Mat frame, double processingTime)
        {
            Frame = frame;
            ProcessingTime = processingTime;
        }

        public Mat Frame { get; }
        public double ProcessingTime { get; } = -1;
    }
}