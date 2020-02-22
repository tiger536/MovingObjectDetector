﻿using ClassLibrary1.Models;
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
        private Mat output;

        public GrayScaleConverter(Mat mat, Mat output)
        {
            frame = mat;
            this.output = output;
        }
        public void Execute()
        {
            CvInvoke.CvtColor(frame, output, ColorConversion.Bgr2Gray);
        }
    }
}
