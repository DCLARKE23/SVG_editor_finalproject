using Svg;
using SVG_editor_finalproject.Handlers;

namespace SVG_editor_finalproject
{
    public partial class Form1 : Form
    {
        public ShapeDrawingController ShapeDrawingController { get; set; }
        public DocumentModel DocumentModel { get; private set; }
        public FileHandler FileHandler { get; }
        public UndoRedoHandler UndoRedoHandler { get; }
        public SelectHandler SelectHandler { get; set; }
        public ToolController ToolController { get; set; } = new();
        public LayerController LayerController { get; set; }

        public Form1()
        {
            InitializeComponent();
            FileHandler = new("json", OnSave, OnOpen);
            DocumentModel = new DocumentModel();
            UndoRedoHandler = new UndoRedoHandler(DocumentModel.Clone());
            SelectHandler = new SelectHandler();
            lineColorDialog.Color = Color.Black;
            fillColorDialog.Color = Color.Blue;
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.BringToFront(); // grid can still be seen but shapes may also be drawn
            UpdatePanels();
            UpdateHexBoxes();
            ShapeDrawingController = new ShapeDrawingController(pictureBox1);
            ToolController.Tool = Tool.Rectangle;
            var Layer = new LayerControl();
            Layer.layerSelect += LayerSelect;
            Layer.lockButtonClick += LockButton;
            Layer.visibleButtonClick += VisibleButton;
            Layer.frontButtonClick += FrontButton;
            Layer.backButtonClick += BackButton;
            LayerController = new LayerController(Layer);
            flowLayoutPanel2.Controls.Add(Layer);
            DocumentModel.LayerModels.Add(Layer.Model);
        }
        public void DocumentChanged()
        {
            pictureBox1.Invalidate();
            FileHandler.Modified = true;
        }
// ================================= FILE HANDLING =========================================
        public void OnSave(string fileName)
        {
            var doc = DocumentModel.ToJson();
            File.WriteAllText(fileName, doc);
        }
        public void OnOpen(string fileName)
        {
            var text = File.ReadAllText(fileName);
            var doc = DocumentModel.FromJson(text);
            DocumentModel = doc;
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
            DocumentModel = new DocumentModel();
            DocumentChanged();
        }
        public SvgDocument GetSvgDocument()
        {
            return DocumentModel.LayerModels.ToSvg();
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
        private void UpdateHexBoxes()
        {
            textBox1.Text = RGBToHex(lineColorDialog.Color.R, lineColorDialog.Color.G, lineColorDialog.Color.B);
            textBox2.Text = RGBToHex(fillColorDialog.Color.R, fillColorDialog.Color.G, fillColorDialog.Color.B);
        }
        public static string RGBToHex(int r, int g, int b)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}", r, g, b);
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
// ================================= SHAPE DRAWING ==============================================
        public void StartDrawingShape(SimpleShapeModel shape)
        {
            if (LayerController.CurrentLayer.Lock) return;
            LayerController.CurrentLayer.Model.Shapes.Add(shape);
            ShapeDrawingController.StartDrawing(shape, MousePosition);
            pictureBox1.Capture = true;
        }
        public void CompleteDrawingShape()
        {
            if (!ShapeDrawingController.IsDrawing())
                return;
            ShapeDrawingController.StopDrawing();
            UndoRedoHandler.NewItem(DocumentModel.Clone());
            DocumentChanged();
        }
        public void CancelDrawingShape()
        {
            if (!ShapeDrawingController.IsDrawing())
                return;
            LayerController.CurrentLayer.Model.Shapes.Remove(ShapeDrawingController.Shape);
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
            var svgDoc = DocumentModel.LayerModels.ToSvg();
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
                DocumentModel = doc;
                DocumentChanged();
            }  
        }
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var doc = UndoRedoHandler.Redo();
            if (doc is not null)
            {
                DocumentModel = doc;
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
            }
            selectToolStripMenuItem.Checked = false;
            DocumentChanged();    
        }
        private void deleteDelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(var s in SelectHandler.SelectedShapes)
                LayerController.CurrentLayer.Model.Shapes.Remove(s);
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
            foreach (var x in LayerController.CurrentLayer.Model.Shapes)
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
            LayerController.CurrentLayer.Model.Shapes.Clear();
            DocumentChanged();
        }
// ============================= LAYER CONTROLS ========================================
        private void button3_Click(object sender, EventArgs e)  // add new layer
        {
            var Layer = new LayerControl();
            Layer.layerSelect += LayerSelect;
            Layer.lockButtonClick += LockButton;
            Layer.visibleButtonClick += VisibleButton;
            Layer.frontButtonClick += FrontButton;
            Layer.backButtonClick += BackButton;
            flowLayoutPanel2.Controls.Add(Layer);
            LayerController.AddLayer(Layer);
            DocumentModel.LayerModels.Add(Layer.Model); // may not need to be here
        }

        public void LayerSelect(object? sender, EventArgs e)
        {
            LayerController.CurrentLayer = (LayerControl)sender;
        }

        public void LockButton(object? sender, EventArgs e)
        {

        }
        public void VisibleButton(object? sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }
        public void FrontButton(object? sender, EventArgs e)
        {
            LayerController.MoveFront((LayerControl)sender, DocumentModel.LayerModels);
            pictureBox1.Invalidate();
        }
        public void BackButton(object? sender, EventArgs e)
        {
            LayerController.MoveBack((LayerControl)sender, DocumentModel.LayerModels);
            pictureBox1.Invalidate();
        }
    }
}