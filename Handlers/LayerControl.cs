namespace SVG_editor_finalproject.Handlers
{
    public partial class LayerControl : UserControl
    {
        public bool Lock { get; set; }
        public bool Visible { get; set; }
        public LayerModel Model { get; set; }

        public LayerControl()
        {
            InitializeComponent();
            Lock = false;
            Visible = true;
            Model = new LayerModel();
        }
        private void button1_Click(object sender, EventArgs e)  // Locked
        {
            Lock = !Lock;
            Model.Locked = Lock;
            // lockButtonClick.Invoke(this, e);
        }
        public event EventHandler lockButtonClick;

        private void button2_Click(object sender, EventArgs e)  // Visible
        {
            Visible = !Visible;
            Model.Visible = Visible;
            visibleButtonClick.Invoke(this, e);
        }
        public event EventHandler visibleButtonClick;
        private void button3_Click(object sender, EventArgs e) // Front
        {
            if(Lock)
            {
                return;
            }
            frontButtonClick.Invoke(this, e);
        }
        public event EventHandler frontButtonClick;

        private void button4_Click(object sender, EventArgs e) // Back
        {
            if (Lock) 
            { 
                return; 
            }
        }
        public event EventHandler backButtonClick;

        public void RenameLayer(string newName)
        {
            layername.Text = newName;
        }

        
        private void LayerControl_OnClick(object sender, EventArgs e)
        {
            
            layerSelect.Invoke(this, e);
        }
        public event EventHandler layerSelect;
    }
}
