using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaOOP2
{
    public class Line : Shape
    {
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }

        public override void Draw(Graphics graphics)
        {
            using (Pen pen = new Pen(Color, 2))
            {
                graphics.DrawLine(pen, X1, Y1, X2, Y2);
            }
        }

        public override double CalculateArea()
        {

            return 0;
        }

        public override bool ContainsPoint(Point point)
        {

            Rectangle lineRect = new Rectangle(Math.Min(X1, X2), Math.Min(Y1, Y2), Math.Abs(X1 - X2), Math.Abs(Y1 - Y2));
            return lineRect.Contains(point);
        }

        public override void Resize(double factor)
        {

        }
    }
}
