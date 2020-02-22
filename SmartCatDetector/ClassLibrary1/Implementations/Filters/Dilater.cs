using ClassLibrary1.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ClassLibrary1.Implementations
{
    public class Dilater : IMatOperation
    {
        private Mat frame;
        public Dilater(Mat mat)
        {
            frame = mat;
        }
        public void Execute()
        {
            Mat element = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1));
            CvInvoke.Dilate(frame, frame, element, new Point(-1, -1), 6, BorderType.Constant, new MCvScalar(0, 0, 0));

            element.Dispose();
        }
    }
}
