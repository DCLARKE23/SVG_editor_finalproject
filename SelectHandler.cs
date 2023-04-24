using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVG_editor_finalproject
{
    public class SelectHandler
    {
        public List<SimpleShapeModel> SelectedShapes { get; set; }

        public void ClearSelected() 
        {
            SelectedShapes.Clear();
            // DocumentChanged in form
        }

        public void AddToSelection(SimpleShapeModel shape)
        {
            shape.StrokeColor = ColorModel.Red;
            SelectedShapes.Add(shape);
            
        }
    }
}
