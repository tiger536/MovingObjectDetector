using ClassLibrary1.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1.Implementations
{
    public class CannyFilter : IMatOperation
    {
        private Mat frame;
        public CannyFilter(Mat mat)
        {
            frame = mat;
        }
        public void Execute()
        {
            CvInvoke.Canny(frame, frame, 100, 200);
        }
    }
}
