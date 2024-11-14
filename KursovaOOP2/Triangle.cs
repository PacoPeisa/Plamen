using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaOOP2
{
    public class Triangle : Shape
    {
        public int Base { get; set; }
        public int Height { get; set; }

        public override void Draw(Graphics graphics)
        {
            Point[] points =
            {
            new Point(X, Y - Height / 2),
            new Point(X + Base / 2, Y + Height / 2),
            new Point(X - Base / 2, Y + Height / 2)
        };

            using (Brush brush = new SolidBrush(Color))
            {
                graphics.FillPolygon(brush, points);
            }
        }

        public override double CalculateArea()
        {
            return 0.5 * Base * Height;
        }

        public override bool ContainsPoint(Point point)
        {

            int halfBase = Base / 2;
            int halfHeight = Height / 2;
            return (point.X >= X - halfBase) && (point.X <= X + halfBase) && (point.Y >= Y - halfHeight) && (point.Y <= Y + halfHeight);
        }

        public override void Resize(double factor)
        {
            Base = (int)(Base * factor);
            Height = (int)(Height * factor);
        }
    }
}

