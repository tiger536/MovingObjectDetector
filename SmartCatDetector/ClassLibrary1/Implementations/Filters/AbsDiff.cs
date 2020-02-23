using ObjectDetection.Models;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectDetection.Implementations.Filters
{
    public class AbsDiff : IMatOperation
    {
        private Mat mat1, mat2, result;
        public AbsDiff(Mat mat1, Mat mat2, Mat result)
        {
            this.mat1 = mat1;
            this.mat2 = mat2;
            this.result = result;
        }
        public void Execute()
        {
            CvInvoke.AbsDiff(mat1, mat2, result);
        }
    }
}
