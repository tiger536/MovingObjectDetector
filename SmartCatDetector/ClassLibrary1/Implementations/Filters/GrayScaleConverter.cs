using ClassLibrary1.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1.Implementations
{
    public class GrayScaleConverter : IMatOperation
    {
        private Mat frame;
        public GrayScaleConverter(Mat mat)
        {
            frame = mat;
        }
        public void Execute()
        {
            CvInvoke.CvtColor(frame, frame, ColorConversion.Bgr2Gray);
        }
    }
}
