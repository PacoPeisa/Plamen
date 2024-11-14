using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaOOP2
{
    public class Circle : Shape
    {
        public int Radius { get; set; }

        public override void Draw(Graphics graphics)
        {
            using (Brush brush = new SolidBrush(Color))
            {
                graphics.FillEllipse(brush, X - Radius, Y - Radius, Radius * 2, Radius * 2);
            }
        }

        public override double CalculateArea()
        {
            return Math.PI * Radius * Radius;
            
        }

        public override bool ContainsPoint(Point point)
        {
            double distance = Math.Sqrt(Math.Pow(point.X - X, 2) + Math.Pow(point.Y - Y, 2));
            return distance <= Radius;
        }

        public override void Resize(double factor)
        {
            Radius = (int)(Radius * factor);
        }
    }
}
