using ClassLibrary1.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1.Implementations
{
    public class Thresolder : IMatOperation
    {
        private Mat frame;
        public Thresolder(Mat mat)
        {
            frame = mat;
        }
        public void Execute()
        {
            CvInvoke.Threshold(frame, frame, 25, 255, ThresholdType.Binary);
        }
    }
}
