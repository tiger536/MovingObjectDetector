using ObjectDetection.Models;
using Emgu.CV;

namespace ObjectDetection.Implementations
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
            CvInvoke.GaussianBlur(frame, frame, new System.Drawing.Size(7,7), 0.0);
        }
    }
}
