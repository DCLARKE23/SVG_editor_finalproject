using Newtonsoft.Json;

namespace SVG_editor_finalproject
{
    public class ElementModel
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public ElementModel()
        {
            Id = new object().GetHashCode().ToString();
            Name = GetType().Name;
        }
    }
    public class DocumentModel
    {
        public List<SimpleShapeModel> Shapes { get; set; } = new();
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
        }

        public static DocumentModel? FromJson(string json)
        {
            return JsonConvert.DeserializeObject<DocumentModel>(json,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
        }
        public DocumentModel Clone()
        {
            return FromJson(ToJson());
        }
    }
    public class PointModel
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class SizeModel
    {
        public double Width { get; set; }
        public double Height { get; set; }
    }
    public class ShapeModel : ElementModel
    {
        public ColorModel StrokeColor { get; set; } = ColorModel.Black;
        public double StrokeWidth { get; set; } = 3;
        public ColorModel FillColor { get; set; } = ColorModel.White;
    }

    public class SimpleShapeModel : ShapeModel
    {
        public double Left => Math.Min(Position.X, Position.X + Size.Width);
        public double Right => Math.Max(Position.X, Position.X + Size.Width);
        public double Top => Math.Min(Position.Y, Position.Y + Size.Height);
        public double Bottom => Math.Max(Position.Y, Position.Y + Size.Height);

        public PointModel Position { get; set; } = new();
        public SizeModel Size { get; set; } = new();

        public double Width => Right - Left;
        public double Height => Bottom - Top;
        public double CenterX => Left + Width / 2;
        public double CenterY => Top + Height / 2;
        public double Radius => Math.Min(Width, Height) / 2;
        public double BorderRadius { get; set; } = 0;
        public bool Filled { get; set; } = false;
    }

    public class EllipseModel : SimpleShapeModel
    {
    }

    public class RectangleModel : SimpleShapeModel
    {
    }

    public class CircleModel : SimpleShapeModel
    {
    }

    public class SquareModel : SimpleShapeModel
    {
    }
    public class PolyLineModel : ShapeModel // may not need
    {
        public List<PointModel> Points { get; set; } = new();
    }

    public class ColorModel
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public static ColorModel Black => new() { R = 0, G = 0, B = 0 };
        public static ColorModel Red => new() { R = 255, G = 0, B = 0 };
        public static ColorModel Green => new() { R = 0, G = 255, B = 0 };
        public static ColorModel Blue => new() { R = 0, G = 0, B = 255 };
        public static ColorModel White => new() { R = 255, G = 255, B = 255 };
    }
}
