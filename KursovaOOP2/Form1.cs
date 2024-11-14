using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace KursovaOOP2
{
    public partial class Form1 : Form
    {
        
        
        private Stack<List<Shape>> undoStack = new Stack<List<Shape>>();
        private Stack<List<Shape>> redoStack = new Stack<List<Shape>>();
        private FigureMover figureMover = new FigureMover();
        private List<Shape> shapes = new List<Shape>();
        private Shape selectedShape;
        private Point lastMousePosition;
        private Color selectedColor = Color.Red;
        private bool isResizing = false;
        private bool isDrawingLine = false;
        private Point lineStartPoint;
        private Point lineEndPoint;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (selectedShape != null && e.KeyCode == Keys.D)
            {
                shapes.Remove(selectedShape);
                selectedShape = null;
                Invalidate();
                AddToUndoStack();
                UpdateCommandListBox("Shape deleted.");
            }

        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (selectedShape != null)
            {
                double factor = e.Delta > 0 ? 1.1 : 0.9;
                selectedShape.Resize(factor);
                AddToUndoStack();
                UpdateCommandListBox("Shape resized.");
                Invalidate();

            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            foreach (Shape shape in shapes)
            {
                shape.Draw(e.Graphics);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                foreach (Shape shape in shapes)
                {
                    if (shape.ContainsPoint(e.Location))
                    {
                        selectedShape = shape;
                        lastMousePosition = e.Location;
                        isResizing = true;
                        AddToUndoStack();
                        figureMover.StartMove(selectedShape);
                        return;
                    }
                }
            }

            base.OnMouseDown(e);

            if (isDrawingLine && e.Button == MouseButtons.Left)
            {
                if (lineStartPoint.IsEmpty)
                {
                    lineStartPoint = e.Location;
                }
                else
                {
                    lineEndPoint = e.Location;

                    shapes.Add(new Line { X1 = lineStartPoint.X, Y1 = lineStartPoint.Y, X2 = lineEndPoint.X, Y2 = lineEndPoint.Y, Color = selectedColor });

                    lineStartPoint = Point.Empty;
                    lineEndPoint = Point.Empty;
                    isDrawingLine = false;
                    button6.Enabled = true;
                    Invalidate();
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (isResizing && selectedShape != null)
            {

            }
            if (selectedShape != null && selectedShape.ContainsPoint(e.Location))
            {
                Cursor = Cursors.SizeAll;
            }
            else
            {
                Cursor = Cursors.Default;
            }

            if (figureMover != null && selectedShape != null)
            {
                Point delta = new Point(e.Location.X - lastMousePosition.X, e.Location.Y - lastMousePosition.Y);
                figureMover.MoveShapes(delta);
                lastMousePosition = e.Location;
                Invalidate();

            }
        }


        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Left)
            {
                isResizing = false;
                figureMover.EndMove();
                UpdateCommandListBox("Shape moved.");
            }

        }
        private void UpdateCommandListBox(string command)
        {
            listBox1.Items.Add(command);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void AddToUndoStack()
        {

            List<Shape> initialState = new List<Shape>();
            foreach (Shape shape in shapes)
            {
                Shape copiedShape = null;
                if (shape is Circle)
                {
                    Circle circle = shape as Circle;
                    copiedShape = new Circle { X = circle.X, Y = circle.Y, Radius = circle.Radius, Color = circle.Color };
                }
                else if (shape is Square)
                {
                    Square square = shape as Square;
                    copiedShape = new Square { X = square.X, Y = square.Y, Size = square.Size, Color = square.Color };
                }
                else if (shape is Triangle)
                {
                    Triangle triangle = shape as Triangle;
                    copiedShape = new Triangle { X = triangle.X, Y = triangle.Y, Base = triangle.Base, Height = triangle.Height, Color = triangle.Color };
                }
                else if (shape is Line)
                {
                    Line line = shape as Line;
                    copiedShape = new Line { X1 = line.X1, Y1 = line.Y1, X2 = line.X2, Y2 = line.Y2, Color = line.Color };
                }

                if (copiedShape != null)
                {
                    initialState.Add(copiedShape);
                }
            }

            undoStack.Push(initialState);
            redoStack.Clear();
        }

        private void SaveToFile(string filePath)
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };
                string json = JsonConvert.SerializeObject(shapes, Formatting.Indented, settings);

                File.WriteAllText(filePath, json);

                MessageBox.Show("Shapes saved successfully.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving shapes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UpdateCommandListBox("File Saved.");
        }

        private void LoadFromFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    };
                    string json = File.ReadAllText(filePath);


                    List<Shape> loadedShapes = JsonConvert.DeserializeObject<List<Shape>>(json, settings);
                    if (loadedShapes != null)
                    {

                        shapes.Clear();
                        shapes.AddRange(loadedShapes);
                        Invalidate();
                        MessageBox.Show("Shapes loaded successfully.", "Load", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("File not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading shapes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UpdateCommandListBox("File loaded.");

        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            shapes.Add(new Circle { X = 300, Y = 100, Radius = 50, Color = selectedColor });
            Invalidate();
            AddToUndoStack();
            UpdateCommandListBox("Added circle.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            shapes.Add(new Triangle { X = 300, Y = 300, Base = 100, Height = 80, Color = selectedColor });

            Invalidate();
            AddToUndoStack();
            UpdateCommandListBox("Added triangle.");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                selectedColor = colorDialog.Color;
            }
            UpdateCommandListBox("Changed color.");
            AddToUndoStack();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            shapes.Clear();
            Invalidate();
            AddToUndoStack();
            UpdateCommandListBox("Stage cleared.");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            shapes.Add(new Square { X = 300, Y = 200, Size = 100, Color = selectedColor });
            Invalidate();
            AddToUndoStack();
            UpdateCommandListBox("Added square.");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!isDrawingLine)
            {
                isDrawingLine = true;
                button6.Enabled = false;
                lineStartPoint = Point.Empty;
                lineEndPoint = Point.Empty;
            }

            AddToUndoStack();
            UpdateCommandListBox("Added Line.");
            Invalidate();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                redoStack.Push(new List<Shape>(shapes));
                shapes.Clear();
                foreach (Shape shape in undoStack.Pop())
                {
                    shapes.Add(shape);

                }
                Invalidate();
                UpdateCommandListBox("Undo last command.");

            }

        }

        private void button8_Click(object sender, EventArgs e)
        {

            if (redoStack.Count > 0)
            {
                undoStack.Push(new List<Shape>(shapes));
                shapes.Clear();
                foreach (Shape shape in redoStack.Pop())
                {
                    shapes.Add(shape);
                }
                Invalidate();
                UpdateCommandListBox("Redo last command.");

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JSON Files (*.json)|*.json|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveToFile(saveFileDialog.FileName);
                }
            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "JSON Files (*.json)|*.json|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadFromFile(openFileDialog.FileName);
                }

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                var circles = shapes.OfType<Circle>().ToList();

                textBox1.Text = $"{circles.Count}";
                UpdateCommandListBox("Found number of circles.");
            }
            else
            {
                textBox1.Text = "";
            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                var circlesCount = shapes.OfType<Circle>().Count();
                var squaresCount = shapes.OfType<Square>().Count();
                var trianglesCount = shapes.OfType<Triangle>().Count();
                var linesCount = shapes.OfType<Line>().Count();

                string mostAppearedFigure = "None";
                int maxCount = 0;

                if (circlesCount > maxCount)
                {
                    mostAppearedFigure = "Circle";
                    maxCount = circlesCount;
                }
                if (squaresCount > maxCount)
                {
                    mostAppearedFigure = "Square";
                    maxCount = squaresCount;
                }
                if (trianglesCount > maxCount)
                {
                    mostAppearedFigure = "Triangle";
                    maxCount = trianglesCount;
                }
                if (linesCount > maxCount)
                {
                    mostAppearedFigure = "Line";
                    maxCount = linesCount;
                }

                textBox2.Text = mostAppearedFigure;
                UpdateCommandListBox("Found most appeared figure.");
            }
            else
            {
                textBox2.Text = "";
            }

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {

                var colorCounts = shapes
                    .GroupBy(shape => shape.Color)
                    .Select(group => new { Color = group.Key.Name, Count = group.Count() })
                    .ToList();


                string mostAppearedColor = "None";
                int maxCount = 0;

                foreach (var colorCount in colorCounts)
                {
                    if (colorCount.Count > maxCount)
                    {
                        mostAppearedColor = colorCount.Color;
                        maxCount = colorCount.Count;
                    }
                }


                textBox3.Text = mostAppearedColor;
                UpdateCommandListBox("Found most appeared color.");

            }
            else
            {

                textBox3.Text = "";
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {

                int totalShapeCount = shapes.Count();

                textBox4.Text = totalShapeCount.ToString();
                UpdateCommandListBox("Found total shapes.");

            }
            else
            {

                textBox4.Text = "";
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            var checkboxes = Controls.OfType<CheckBox>().Where(cb => cb != checkBox5);

            foreach (var checkbox in checkboxes)
            {
                checkbox.Checked = checkBox5.Checked;
            }
            UpdateCommandListBox("Found everything.");
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                var triangles = shapes.OfType<Triangle>().ToList();

                textBox7.Text = $"{triangles.Count}";
                UpdateCommandListBox("Found number of triangles.");

            }
            else
            {
                textBox7.Text = "";
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                var squares = shapes.OfType<Square>().ToList();

                textBox5.Text = $"{squares.Count}";
                UpdateCommandListBox("Found number of squares.");

            }
            else
            {
                textBox5.Text = "";
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
            {
                var lines = shapes.OfType<Line>().ToList();

                textBox6.Text = $"{lines.Count}";
                UpdateCommandListBox("Found number of lines.");

            }
            else
            {
                textBox6.Text = "";
            }
        }


        private void MacroButton_Click_1(object sender, EventArgs e)
        {
            shapes.Add(new Square { X = 400, Y = 100, Size = 100, Color = Color.Red });
            shapes.Add(new Square { X = 550, Y = 100, Size = 120, Color = Color.Blue });
            shapes.Add(new Square { X = 700, Y = 100, Size = 130, Color = Color.Green });

            Invalidate();
            AddToUndoStack();
            UpdateCommandListBox("Printed 3 squares.");
        }

        
    }
}

