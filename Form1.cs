using Svg;
using System.Reflection;

namespace SVG_editor_finalproject
{   
    public partial class Form1 : Form
    {
        public ShapeDrawingController ShapeDrawingController { get; set; }
        public DocumentModel Shapes { get; private set; }
        public FileHandler FileHandler { get; }
        public UndoRedoHandler UndoRedoHandler { get; }
        public SelectHandler SelectHandler { get; set; }
        public ToolController ToolController { get; set; } = new();

        public Form1()
        {
            InitializeComponent();
            FileHandler = new("json", OnSave, OnOpen);
            Shapes = new DocumentModel();
            UndoRedoHandler = new UndoRedoHandler(Shapes.Clone());
            SelectHandler = new SelectHandler();    // may change
            lineColorDialog.Color = Color.Black;
            fillColorDialog.Color = Color.Black;
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.BringToFront();
            UpdatePanels();
            UpdateHexBoxes();
            ShapeDrawingController = new ShapeDrawingController(pictureBox1);
            ToolController.Tool = Tool.Initial;
        }
        public void DocumentChanged()
        {
            pictureBox1.Invalidate();
            FileHandler.Modified = true;
        }
// ================================= FILE HANDLING =========================================
        public void OnSave(string fileName)
        {
            var doc = Shapes.ToJson();
            File.WriteAllText(fileName, doc);
        }
        public void OnOpen(string fileName)
        {
            var text = File.ReadAllText(fileName);
            var doc = DocumentModel.FromJson(text);
            Shapes = doc;
            DocumentChanged();
        }
        private void saveasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileHandler.SaveWithDialog();
        }
        private void saveCtrlSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileHandler.SaveWithCurrentName();
        }
        private void openCtrlOToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FileHandler.Open();
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!FileHandler.NewWithCheck())
                return;
            Shapes = new DocumentModel();
            DocumentChanged();
            // UndoController.Reset(model);
        }
        public SvgDocument GetSvgDocument() // make the next 3 functions cleaner
        {
            return Shapes.Shapes.ToSvg();
        }
        public string GetXml()
        {
            return GetSvgDocument().GetXML();
        }
        private void exportSVGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "SVG files (*.svg)|*.svg|All files (*.*)|*.*";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            File.WriteAllText(saveFileDialog1.FileName, GetXml());
        }
// ================================== COLOUR CONTROLS ========================================
        private void UpdatePanels()
        {
            panel1.BackColor = fillColorDialog.Color;
            panel3.BackColor = lineColorDialog.Color;
        }
        private void UpdateHexPanels()
        {
            fillColorDialog.Color = HexToRGB(textBox1.Text);
            lineColorDialog.Color = HexToRGB(textBox2.Text);
        }
        private void UpdateHexBoxes()
        {
            textBox1.Text = RGBToHex(lineColorDialog.Color.R, lineColorDialog.Color.G, lineColorDialog.Color.B);
            textBox2.Text = RGBToHex(fillColorDialog.Color.R, fillColorDialog.Color.G, fillColorDialog.Color.B);
        }
        public static string RGBToHex(int r, int g, int b)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}", r, g, b);
        }
        public static Color HexToRGB(string hexValue)
        {
            // Strip any '#' characters from the beginning of the string
            hexValue = hexValue.TrimStart('#');

            // Convert the hex string to an integer
            int hex = int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);

            // Extract the red, green, and blue components from the integer
            int red = (hex >> 16) & 0xFF;
            int green = (hex >> 8) & 0xFF;
            int blue = hex & 0xFF;

            // Create and return a Color object representing the RGB color
            Color color = Color.FromArgb(red, green, blue);
            return color;
        }       
        private void button1_Click(object sender, EventArgs e)  // colour dialog button (line)
        {
            lineColorDialog.ShowDialog();
            UpdatePanels();
            UpdateHexBoxes();
        }
        private void button2_Click(object sender, EventArgs e)  // colour dialog button (fill)
        {
            fillColorDialog.ShowDialog();
            UpdatePanels();
            UpdateHexBoxes();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            /*
            if (textBox1.Text.StartsWith('#') && textBox1.Text.Length == 7)
            {
                UpdateHexPanels();
            } */
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            /*
            UpdateHexBoxes(); */
        }
// ================================= SHAPE DRAWING ==============================================
        public void StartDrawingShape(SimpleShapeModel shape)
        {
            Shapes.Shapes.Add(shape);   // change name to something better
            ShapeDrawingController.StartDrawing(shape, MousePosition);
            pictureBox1.Capture = true;
        }
        public void CompleteDrawingShape()
        {
            if (!ShapeDrawingController.IsDrawing())
                return;
            ShapeDrawingController.StopDrawing();
            UndoRedoHandler.NewItem(Shapes.Clone());
            DocumentChanged();
        }
        public void CancelDrawingShape()
        {
            if (!ShapeDrawingController.IsDrawing())
                return;
            Shapes.Shapes.Remove(ShapeDrawingController.Shape);
            ShapeDrawingController.StopDrawing();
            DocumentChanged();
        }
        public void UpdateShape()
        {
            if (!ShapeDrawingController.IsDrawing())
                return;
            ShapeDrawingController.Update(MousePosition);
            DocumentChanged();
        }
        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rectangleToolStripMenuItem.Checked)
            {
                rectangleToolStripMenuItem.Checked = false;
            }
            rectangleToolStripMenuItem.Checked = true;
            ToolController.Tool = Tool.Rectangle;
            textBox3.Text = "Rectangle";
        }
        private void circleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolController.Tool = Tool.Circle;
            textBox3.Text = "Circle";
        }

        private void squareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolController.Tool = Tool.Square;
            textBox3.Text = "Square";
        }

        private void ellipseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolController.Tool = Tool.Ellipse;
            textBox3.Text = "Ellipse";
        }
// ============================================ PICTUREBOX CONTROLS ======================================
        private void pictureBox1_OnMouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                switch(ToolController.Tool)
                {
                    case Tool.Rectangle:
                        StartDrawingShape(new RectangleModel()
                        {BorderRadius = trackBar2.Value,
                        StrokeColor = lineColorDialog.Color.ToColorModel(),
                        StrokeWidth = trackBar1.Value,
                        FillColor = fillColorDialog.Color.ToColorModel(),
                        Filled = checkBox1.Checked,
                        });
                        break;
                    case Tool.Square:
                        StartDrawingShape(new SquareModel()
                        {BorderRadius = trackBar2.Value,
                        StrokeWidth = trackBar1.Value,
                        StrokeColor = lineColorDialog.Color.ToColorModel(),
                        FillColor = fillColorDialog.Color.ToColorModel(),
                        Filled = checkBox1.Checked,
                        });   
                        break;
                    case Tool.Circle: 
                        StartDrawingShape(new CircleModel()
                        {StrokeColor = lineColorDialog.Color.ToColorModel(),
                         StrokeWidth = trackBar1.Value,
                         FillColor = fillColorDialog.Color.ToColorModel(),
                        Filled = checkBox1.Checked,                        
                        }); 
                        break;
                    case Tool.Ellipse:
                        StartDrawingShape(new EllipseModel()
                        {StrokeColor = lineColorDialog.Color.ToColorModel(),
                        StrokeWidth = trackBar1.Value,
                        FillColor = fillColorDialog.Color.ToColorModel(),
                        Filled = checkBox1.Checked,
                        });
                        break;                    
                    case Tool.Select:
                        SelectWithMouse();
                        break;
                }
            }
            if(e.Button == MouseButtons.Right) 
            { 
                CancelDrawingShape();
            }
        }
        private void pictureBox1_OnMouseMove(object sender, MouseEventArgs e)
        {
            UpdateShape();
        }
        private void pictureBox1_OnMouseUp(object sender, MouseEventArgs e)
        {
            pictureBox1.Capture = false;
            CompleteDrawingShape();
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int numOfCells = 9;
            int cellSize = 125;
            Pen p = new Pen(Color.LightGray);
            if (gridControl.Checked)
            {
                for (int y = 0; y < numOfCells; ++y)
                {
                    g.DrawLine(p, 0, y * cellSize, numOfCells * cellSize, y * cellSize);
                }

                for (int x = 0; x < numOfCells; ++x)
                {
                    g.DrawLine(p, x * cellSize, 0, x * cellSize, numOfCells * cellSize);
                }
            }
            var svgDoc = Shapes.Shapes.ToSvg();
            svgDoc.Draw(e.Graphics);
        }
        private void gridControl_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }        
// ======================================== UNDO REDO HANDLING =======================================
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var doc = UndoRedoHandler.Undo();
            if (doc is not null)
            {
                Shapes = doc;
                DocumentChanged();
            }  
        }
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var doc = UndoRedoHandler.Redo();
            if (doc is not null)
            {
                Shapes = doc;
                DocumentChanged();
            }    
        }

// ===================================== SELECTION HANDLING ========================================
        private void selectToolStripMenuItem_Click(object sender, EventArgs e)  // edit to be more basic
        {
            if (!selectToolStripMenuItem.Checked)
            {
                selectToolStripMenuItem.Checked = true;
                ToolController.Tool = Tool.Select;
                textBox3.Text = "Select";
                // DocumentChanged();
            }
            selectToolStripMenuItem.Checked = false;
            DocumentChanged();    
        }
        private void deleteDelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(var s in SelectHandler.SelectedShapes)
                Shapes.Shapes.Remove(s);
            SelectHandler.ClearSelected();
            DocumentChanged();
        }
        public bool HitTest(SimpleShapeModel e, Point pt)
        {
            if(e is SimpleShapeModel shape)
            {
                return (pt.X >= shape.Left && pt.X <= shape.Right) && (pt.Y >= shape.Top && pt.Y <= shape.Bottom);
            }
            return false;
        }
        public SimpleShapeModel? HitTest(Point pt)
        {
            foreach (var x in Shapes.Shapes)
            {
                if (HitTest(x, pt))
                    return x;
            }

            return null;
        }
        public void SelectWithMouse()
        {
            var e = HitTest(MousePositionRelativeToPicture());
            if (e != null)
            {
                SelectHandler.AddToSelection(e);
            }
            DocumentChanged();
        }
        public Point MousePositionRelativeToPicture()
        {
            var pt = pictureBox1.PointToScreen(Point.Empty);
            return new Point(MousePosition.X - pt.X, MousePosition.Y - pt.Y);
        }
        private void clearCtrlLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Shapes.Shapes.Clear();
            DocumentChanged();
        }   
    }
}