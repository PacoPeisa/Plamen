using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaOOP2
{
    public class Square : Shape
    {
        public int Size { get; set; }

        public override void Draw(Graphics graphics)
        {
            using (Brush brush = new SolidBrush(Color))
            {
                graphics.FillRectangle(brush, X - Size / 2, Y - Size / 2, Size, Size);
            }
        }

        public override double CalculateArea()
        {
            return Size * Size;
        }

        public override bool ContainsPoint(Point point)
        {
            return (point.X >= X - Size / 2) && (point.X <= X + Size / 2) && (point.Y >= Y - Size / 2) && (point.Y <= Y + Size / 2);
        }

        public override void Resize(double factor)
        {
            Size = (int)(Size * factor);
        }
    }
}
