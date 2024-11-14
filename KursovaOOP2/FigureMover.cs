using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaOOP2
{
    public class FigureMover
    {
        
            private readonly List<Shape> movedShapes = new List<Shape>();

            public void StartMove(Shape shape)
            {
                if (!movedShapes.Contains(shape))
                    movedShapes.Add(shape);
            }

            public void MoveShapes(Point delta)
            {
                foreach (Shape shape in movedShapes)
                {
                    shape.X += delta.X;
                    shape.Y += delta.Y;
                }
            }

            public void EndMove()
            {
                movedShapes.Clear();
            }
    
    }
    
}
