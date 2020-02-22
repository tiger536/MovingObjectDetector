﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ClassLibrary1.Extensions
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
