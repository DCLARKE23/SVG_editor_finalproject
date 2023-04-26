using Svg;

namespace SVG_editor_finalproject
{
    public static class Utils
    {
        public static System.Drawing.Color ToDrawingColor(this ColorModel color)
        {
            return System.Drawing.Color.FromArgb(color.R, color.G, color.B);
        }
        public static ColorModel ToColorModel(this System.Drawing.Color color)
        {
            return new ColorModel()
            {
                R = color.R,
                G = color.G,
                B = color.B,
            };
        }
        public static SvgPaintServer ToSvg(this ColorModel color)
        {
            return new SvgColourServer(color.ToDrawingColor());
        }

        public static SvgElement? ToSvg(this SimpleShapeModel Shape)
        {
            switch (Shape)
            {
                case RectangleModel rectangle:
                    return new SvgRectangle()
                    {
                        X = (float)rectangle.Left,
                        Y = (float)rectangle.Top,
                        Width = (float)rectangle.Width,
                        Height = (float)rectangle.Height,
                        Stroke = rectangle.StrokeColor.ToSvg(),
                        StrokeWidth = (float)rectangle.StrokeWidth,
                        Fill = rectangle.Filled ? rectangle.FillColor.ToSvg() : SvgPaintServer.None,
                        CornerRadiusX = (float)rectangle.BorderRadius,
                        CornerRadiusY = (float)rectangle.BorderRadius,
                    };
                case CircleModel circle:
                    return new SvgCircle()
                    {
                        CenterX = (float)circle.CenterX,
                        CenterY = (float)circle.CenterY,
                        Radius = (float)circle.Radius,
                        Stroke = circle.StrokeColor.ToSvg(),
                        StrokeWidth = (float)circle.StrokeWidth,
                        Fill = circle.Filled ? circle.FillColor.ToSvg() : SvgPaintServer.None,
                    };
                case SquareModel square:
                    return new SvgRectangle()
                    {
                        X = (float)square.Left,
                        Y = (float)square.Top,
                        Width = (float)square.Radius * 2,
                        Height = (float)square.Radius * 2,
                        Stroke = square.StrokeColor.ToSvg(),
                        StrokeWidth = (float)square.StrokeWidth,
                        Fill = square.Filled ? square.FillColor.ToSvg() : SvgPaintServer.None,
                        CornerRadiusX = (float)square.BorderRadius,
                        CornerRadiusY = (float)square.BorderRadius,
                    };
                case EllipseModel ellipse:
                    return new SvgEllipse()
                    {
                        CenterX = (float)ellipse.CenterX,
                        CenterY = (float)ellipse.CenterY,
                        RadiusX = (float)ellipse.Width / 2,
                        RadiusY = (float)ellipse.Height / 2,
                        Stroke = ellipse.StrokeColor.ToSvg(),
                        StrokeWidth = (float)ellipse.StrokeWidth,
                        Fill = ellipse.Filled ? ellipse.FillColor.ToSvg() : SvgPaintServer.None,
                    };
            }
            return null;              
        }
        public static SvgDocument ToSvg(this List<LayerModel> Layers)
        {
            var r = new SvgDocument();
            
            foreach (var layer in Layers)
            {
                if (!layer.Visible)
                {
                    foreach (var shape in layer.Shapes)
                    {
                        var tmp = shape.ToSvg();
                        if (tmp != null)
                            r.Children.Add(tmp);
                    }
                }
            }
            return r;
        }
    }
}

