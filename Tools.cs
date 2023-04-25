namespace SVG_editor_finalproject
{
    public enum Tool
    {
        Initial,
        Select,
        Rectangle,
        Circle,
        Ellipse,
        Square
    }
    public class ToolController
    {
        private Tool tool = Tool.Initial;
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
