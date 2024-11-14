using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaOOP2
{
    public abstract class Shape
    {
        
        public void HandleMouseWheel(MouseEventArgs e)
        {
            double factor = e.Delta > 0 ? 1.1 : 0.9;
            Resize(factor);
        }
        public int X { get; set; }
        public int Y { get; set; }
        public Color Color { get; set; }

        public abstract void Draw(Graphics graphics);
        public abstract double CalculateArea();
        public abstract bool ContainsPoint(Point point);
        public abstract void Resize(double factor);
        
        
    }
        
}
