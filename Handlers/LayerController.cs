using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVG_editor_finalproject.Handlers
{
    public class LayerController
    {
        public List<LayerControl> Layers { get; set; }
        public string LayerName { get; set; }
        public List<SimpleShapeModel> ShapeModels { get; set; }
        public LayerControl LayerControl { get; set; }
        public int LayerIndex { get; set; }
        public LayerControl CurrentLayer { get; set; }
       
        public LayerController(LayerControl Layer)
        {
            Layers = new List<LayerControl>();
            LayerIndex = 0;
            LayerName = string.Empty;
            ShapeModels = new List<SimpleShapeModel>();
            LayerControl = new LayerControl();
            CurrentLayer = Layer;
            Layers.Add(Layer);
            CurrentLayer = Layers[0];
        }

        public void AddLayer(LayerControl Layer)
        {
            Layers.Add(Layer);
            LayerName = "Layer" + LayerIndex;
            LayerIndex++;
            Layer.RenameLayer(LayerName);
        }
    }

}
