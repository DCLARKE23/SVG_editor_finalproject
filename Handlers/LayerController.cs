namespace SVG_editor_finalproject.Handlers
{
    public class LayerController
    {
        public List<LayerControl> Layers { get; set; } = new();
        public string LayerName { get; set; }
        public LayerControl LayerControl { get; set; } = new();
        public int LayerIndex { get; set; }
        public LayerControl CurrentLayer { get; set; }
       
        public LayerController(LayerControl Layer)
        {
            LayerIndex = 2;
            LayerName = string.Empty;
            CurrentLayer = Layer;
            Layers.Add(CurrentLayer);
            // CurrentLayer = Layers[0];
        }

        public void AddLayer(LayerControl Layer)    // broken
        {
            Layers.Add(Layer);
            LayerName = "Layer" + LayerIndex;
            LayerIndex++;
            Layer.RenameLayer(LayerName);
        }

        public void MoveFront(LayerControl Layer, List<LayerModel> layerModels)
        {
            var i = 0;
            var count = 0;
            if(Layer == Layers[0])
            {
                return;
            }
            foreach (var l in Layers) 
            { 
                if (l == Layer)
                {
                    i = count; break;
                }
                count++;
            }
            var temp = Layers[i];
            Layers[i] = Layers[i - 1];
            Layers[i-1] = temp;

            var temp2 = layerModels[i];
            layerModels[i] = layerModels[i - 1];
            layerModels[i-1] = temp2;
        }

        public void MoveBack(LayerControl Layer, List<LayerModel> layerModels)
        {
            var i = 0;
            var count = 0;
            if (Layer == Layers[Layers.Count-1])
            {
                return;
            }
            foreach (var l in Layers)
            {
                if (l == Layer)
                {
                    i = count; break;
                }
                count++;
            }
            var temp = Layers[i];
            Layers[i] = Layers[i + 1];
            Layers[i+1] = temp;

            var temp2 = layerModels[i];
            layerModels[i] = layerModels[i + 1];
            layerModels[i+1] = temp2;
        }
    }
}
