using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVG_editor_finalproject.Handlers
{
    public class SelectHandler
    {
        public List<SimpleShapeModel> SelectedShapes { get; private set; } = new();

        public void ClearSelected()
        {
            SelectedShapes.Clear();
        }
        public void AddToSelection(SimpleShapeModel shape)
        {
            shape.StrokeColor = ColorModel.Red;
            SelectedShapes.Add(shape);
        }
    }
}
