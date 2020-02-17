using ClassLibrary1.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1.Implementations
{
    public class GaussianBlur : IMatOperation
    {
        private Mat frame;
        public GaussianBlur(Mat mat)
        {
            frame = mat;
        }
        public void Execute()
        {
            CvInvoke.GaussianBlur(frame, frame, new System.Drawing.Size(5,5), 0.0);
        }
    }
}
