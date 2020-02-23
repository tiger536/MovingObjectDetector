using System.Drawing;

namespace ObjectDetection.Extensions
{
    public static class RectangleExtension
    {
        public static Point BottomLocation (this Rectangle rec)
        {
            return new Point(rec.X + rec.Width,rec.Y+rec.Height);
        }

        public static bool CanIIncludeThisRectangleInAnother(this Rectangle rect, Rectangle anotherRect, double ratio)
        {
            var r1 = Rectangle.Inflate(rect, (int)(rect.Width * ratio), (int)(rect.Height * ratio));
            var r2 = Rectangle.Inflate(anotherRect, (int)(anotherRect.Width * ratio), (int)(anotherRect.Height * ratio));

            return r1.IntersectsWith(r2);
        }

        public static Rectangle ResizeToFitMat(this Rectangle rect, int matWidth, int matHeight)
        {
            if (rect.X < 0) rect.X = 0;
            if (rect.Y < 0) rect.Y = 0;

            if(rect.X + rect.Width >= matWidth)
            {
                rect.Width = matWidth - rect.X - 1;
            }

            if (rect.Y + rect.Height >= matHeight)
            {               
                rect.Height = matHeight - rect.Y - 1;
            }
            return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);

        }
    }
}
