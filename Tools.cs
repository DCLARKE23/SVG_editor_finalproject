namespace SVG_editor_finalproject
{
    public enum Tool
    {
        Select,
        Rectangle,
        Circle,
        Ellipse,
        Square
    }
    public class ToolController
    {
        private Tool tool = Tool.Rectangle;
        public Tool Tool
        {
            get => tool;
            set
            {
                tool = value;
            }
        }
    }
}
