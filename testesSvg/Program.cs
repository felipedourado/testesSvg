// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using testesSvg;

Console.WriteLine("Hello, World!");

Console.WriteLine(SvgComponentSide.GenerateComponent());


static string GenerateRetancle(int width, int height, bool isTopThick, bool isRightThick, bool isLeftThick, bool isBottomThick)
{
    const int thinStrokenWidth = 1;
    const int thickStrokeWidth = 4;
    const string thinStrokeColor = "#999";
    const string thickStrokeColor = "#000";
    const int padding = 10;
    const int textPadding = 15; //extra padding for text labels

    int svgWidth = width + (2 * padding) + thickStrokeWidth + textPadding + 30;
    int svgHeight = height + (2 * padding) + thickStrokeWidth + textPadding + 20;

    int rectX = padding + (thickStrokeWidth / 2);
    int rectY = padding + (thickStrokeWidth / 2);

    StringBuilder svgBuilder = new StringBuilder();

    //Header
    svgBuilder.AppendLine($"<svg xmlns=\"https://www.w3.org/2000/svg\" viewbox=\"0 0 {svgWidth} {svgHeight}\" width=\"{svgWidth}px\" height=\"{svgHeight}px\">");

    //Top Edge
    int topStrokeWidth = isTopThick ? thickStrokeWidth : thinStrokenWidth;
    string topColor = isTopThick ? thickStrokeColor : thinStrokeColor;
    svgBuilder.AppendLine($"  <line x1=\"{rectX}\" y1=\"{rectY}\" x2=\"{rectX + width}\" y2=\"{rectY}\" stroke=\"{topColor}\" stroke-width=\"{topStrokeWidth}\" />");

    //Right Edge
    int rightStrokeWidth = isRightThick ? thickStrokeWidth : thinStrokenWidth;
    string rightColor = isRightThick ? thickStrokeColor : thinStrokeColor;
    svgBuilder.AppendLine($"  <line x1=\"{rectX + width}\" y1=\"{rectY}\" x2=\"{rectX + width}\" y2=\"{rectY + height}\" stroke=\"{rightColor}\" stroke-width=\"{rightStrokeWidth}\" />");

    //Bottom Edge
    int bottomStrokeWidth = isBottomThick ? thickStrokeWidth : thinStrokenWidth;
    string bottomColor = isBottomThick ? thickStrokeColor : thinStrokeColor;
    svgBuilder.AppendLine($"  <line x1=\"{rectX + width}\" y1=\"{rectY + height}\" x2=\"{rectX}\" y2=\"{rectY + height}\" stroke=\"{bottomColor}\" stroke-width=\"{bottomStrokeWidth}\" />");

    //Left Edge
    int leftStrokeWidth = isLeftThick ? thickStrokeWidth : thinStrokenWidth;
    string leftColor = isLeftThick ? thickStrokeColor : thinStrokeColor;
    svgBuilder.AppendLine($"  <line x1=\"{rectX}\" y1=\"{rectY + height}\" x2=\"{rectX}\" y2=\"{rectY}\" stroke=\"{leftColor}\" stroke-width=\"{leftStrokeWidth}\" />");

    //Add width text below the bottom edge
    int widthTextX = rectX + (width / 2);
    int widthTextY = rectY + height + textPadding;
    svgBuilder.AppendLine($"  <text x=\"{widthTextX}\" y=\"{widthTextY}\" text-anchor=\"middle\" font-family=\"Arial\" font-size=\"12\"> {width}</text>");

    //Add Height text to the right of the right edge
    int heightTextX = rectX + width + textPadding;
    int heightTextY = rectY + (height / 2);
    svgBuilder.AppendLine($"  <text x=\"{heightTextX}\" y=\"{heightTextY}\" text-anchor=\"middle\" dominant-baseline=\"middle\" font-family=\"Arial\" font-size=\"12\" transform=\"rotate(90, {heightTextX}, {heightTextY})\"> {height}</text>");

    svgBuilder.AppendLine("</svg>");
    return svgBuilder.ToString();
}

#region n usado
static string GenerateXml()
{
    string json = @"{
            'projection': 'orthographic',
            'components': [
                {
                    'componentType': 'door',
                    'name': '19692883',
                    'props': {
                        'Board': 'board15',
                        'Width': '90000',
                        'Height': '50000',
                        'HingeSku': 'hinge'
                    }
                },
                {
                    'componentType': 'side',
                    'name': '19692884',
                    'props': {
                        'Board': 'board15',
                        'Width': '56900',
                        'Height': '54800',
                        'DadoThickness': '700'
                    }
                },
                {
                    'componentType': 'side',
                    'name': '19692885',
                    'props': {
                        'Board': 'board15',
                        'Width': '45800',
                        'Height': '96000',
                        'DadoThickness': '700'
                    }
                }
            ]
        }";

    JObject jsonObj = JObject.Parse(json);
    XElement svgExport = new XElement("SvgExport");

    foreach (var component in jsonObj["components"])
    {
        string name = component["name"]!.ToString();
        string componentType = component["componentType"]!.ToString();
        int width = int.Parse(component["props"]!["Width"]!.ToString());
        int height = int.Parse(component["props"]!["Height"]!.ToString());

        int hw = width / 2;
        int hh = height / 2;

        XElement svg = new XElement("svg",
            new XAttribute("viewBox", $"-{hw} -{hh} {width} {height}"),
            new XAttribute("width", width),
            new XAttribute("height", height)
        );

        XElement gBase = new XElement("g");
        foreach (var path in CreateBasePaths(width, height))
        {
            gBase.Add(new XElement("path",
                new XAttribute("d", path),
                new XAttribute("style", "fill:#00004d;fill-opacity:0.2;stroke-linejoin:round;stroke-width:10;"),
                new XAttribute("stroke", "black")
            ));
        }
        svg.Add(gBase);

        if (componentType == "side" && component["props"]!["DadoThickness"] != null)
        {
            int dadoThickness = int.Parse(component["props"]!["DadoThickness"]!.ToString());
            XElement gDado = new XElement("g");
            foreach (var path in CreateDadoPaths(width, height, dadoThickness))
            {
                gDado.Add(new XElement("path",
                    new XAttribute("d", path),
                    new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
                    new XAttribute("stroke", "black")
                ));
            }
            svg.Add(gDado);
        }

        XElement compElem = new XElement("Component", new XAttribute("Name", name), svg);
        svgExport.Add(compElem);
    }

    Console.WriteLine(svgExport);
    return svgExport.ToString();
}

static List<string> CreateBasePaths(int width, int height)
{
    int hw = width / 2;
    int hh = height / 2;

    string basePath = $"M {-hw} {-hh} L {hw} {-hh} L {hw} {hh} L {-hw} {hh} Z";
    string top = $"M {-hw} {-hh} L {hw} {-hh} L {hw} {-hh} L {-hw} {-hh} Z";
    string bottom = $"M {-hw} {hh} L {hw} {hh} L {hw} {hh} L {-hw} {hh} Z";
    string right = $"M {hw} {-hh} L {hw} {hh} L {hw} {hh} L {hw} {-hh} Z";
    string left = $"M {-hw} {-hh} L {-hw} {hh} L {-hw} {hh} L {-hw} {-hh} Z";

    return new List<string> { basePath, basePath, top, bottom, right, left };
}

static List<string> CreateDadoPaths(int width, int height, int thickness)
{
    int x1 = -width / 2;
    int x2 = width / 2;
    int y1 = height / 2 - 270;
    int y2 = y1 + thickness;

    string main = $"M {x1} {y1} L {x2} {y1} L {x2} {y2} L {x1} {y2} Z";
    string top = $"M {x1} {y1} L {x2} {y1} L {x2} {y1} L {x1} {y1} Z";
    string bottom = $"M {x1} {y2} L {x2} {y2} L {x2} {y2} L {x1} {y2} Z";
    string right = $"M {x2} {y1} L {x2} {y2} L {x2} {y2} L {x2} {y1} Z";
    string left = $"M {x1} {y1} L {x1} {y2} L {x1} {y2} L {x1} {y1} Z";

    return new List<string> { main, main, top, bottom, right, left };
}

#endregion



static string GenerateComponent()
{
    string json = @"{
            ""projection"": ""orthographic"",
            ""components"": [
                {
                    ""componentType"": ""door"",
                    ""name"": ""19693994"",
                    ""props"": {
                        ""Board"": ""board15"",
                        ""Width"": ""39600"",
                        ""Height"": ""65600"",
                        ""HingeSku"": ""hinge""
                    }
                }
            ]
        }";

    string svgXml = GenerateSvgFromJson(json);
    return svgXml;
}

static string GenerateSvgFromJson(string json)
{
    var doc = JsonDocument.Parse(json);
    var props = doc.RootElement
                   .GetProperty("components")[0]
                   .GetProperty("props");

    var component = doc.RootElement.GetProperty("components")[0];

    int width = int.Parse(props.GetProperty("Width").GetString());
    int height = int.Parse(props.GetProperty("Height").GetString());
    string componentType = component.GetProperty("componentType").GetString();

    int viewBoxX = -width / 20;
    int viewBoxY = -height / 20; //regra para BOARD 15
    int viewBoxWidth = width / 10;
    int viewBoxHeight = height / 10;

    // Criando o SVG com os elementos fixos
    var svg = new XElement("svg",
        new XAttribute("viewBox", $"{viewBoxX} {viewBoxY} {viewBoxWidth} {viewBoxHeight}"),
        new XAttribute("width", viewBoxWidth),
        new XAttribute("height", viewBoxHeight),
        CreateBorderGroup(viewBoxWidth, viewBoxHeight, viewBoxX, viewBoxY, true, true, true, true),
        //CreateBackgroundGroup(viewBoxWidth, viewBoxHeight, viewBoxX, viewBoxY),
        CreateHighlightGroup(viewBoxWidth, viewBoxHeight, viewBoxX, viewBoxY)
    );

    if (componentType.Equals("Door", StringComparison.OrdinalIgnoreCase))
    {
        svg.Add(CreateGearShape(viewBoxX + 20, viewBoxY + 20, 15, 8));
        svg.Add(CreateGearShape(viewBoxX + 20, viewBoxY + viewBoxHeight - 20, 15, 8));
    }

    return svg.ToString();
}

static XElement CreateHighlightGroup(int w, int h, int x, int y)
{
    var group = new XElement("g");

    int highlightHeight = 70;
    //int highlightY = y + h / 2 - highlightHeight / 2;
    int highlightY = y + h - 230;
    //formula para altura dinamica
    //int offsetFromBottom = h / 15;
    //int highlightY = y+h - offsetFromBottom;


    var coords = new[]
    {
            new[] { x - 2, highlightY, x + w + 2, highlightY, x + w + 2, highlightY + highlightHeight, x - 2, highlightY + highlightHeight },
            new[] { x - 2, highlightY, x + w + 2, highlightY, x + w + 2, highlightY + highlightHeight, x - 2, highlightY + highlightHeight },
            new[] { x - 2, highlightY, x + w + 2, highlightY, x + w + 2, highlightY, x - 2, highlightY },
            new[] { x - 2, highlightY + highlightHeight, x + w + 2, highlightY + highlightHeight, x + w + 2, highlightY + highlightHeight, x - 2, highlightY + highlightHeight },
            new[] { x + w + 2, highlightY, x + w + 2, highlightY + highlightHeight, x + w + 2, highlightY + highlightHeight, x + w + 2, highlightY },
            new[] { x - 2, highlightY, x - 2, highlightY + highlightHeight, x - 2, highlightY + highlightHeight, x - 2, highlightY }
        };

    foreach (var path in coords)
    {
        group.Add(new XElement("path",
            new XAttribute("d", $"M {path[0]} {path[1]} L {path[2]} {path[3]} L {path[4]} {path[5]} L {path[6]} {path[7]} Z"),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));
    }

    return group;
}

static XElement CreateBorderGroup(int w, int h, int x, int y, bool top, bool right, bool bottom, bool left)
{
    var group = new XElement("g");

    // Estilo padrão
    string defaultStyle = "fill:none;stroke:#666;stroke-width:10;stroke-linejoin:round;stroke-dasharray:none;";
    // Estilo em destaque
    string highlightStyle = "fill:none;stroke:#000000;stroke-width:30;stroke-linejoin:round;";

    // Top border
    group.Add(new XElement("line",
        new XAttribute("x1", x),
        new XAttribute("y1", y),
        new XAttribute("x2", x + w),
        new XAttribute("y2", y),
        new XAttribute("style", top ? highlightStyle : defaultStyle)
    ));

    // Right border
    group.Add(new XElement("line",
        new XAttribute("x1", x + w),
        new XAttribute("y1", y),
        new XAttribute("x2", x + w),
        new XAttribute("y2", y + h),
        new XAttribute("style", right ? highlightStyle : defaultStyle)
    ));

    // Bottom border
    group.Add(new XElement("line",
        new XAttribute("x1", x + w),
        new XAttribute("y1", y + h),
        new XAttribute("x2", x),
        new XAttribute("y2", y + h),
        new XAttribute("style", bottom ? highlightStyle : defaultStyle)
    ));

    // Left border
    group.Add(new XElement("line",
        new XAttribute("x1", x),
        new XAttribute("y1", y + h),
        new XAttribute("x2", x),
        new XAttribute("y2", y),
        new XAttribute("style", left ? highlightStyle : defaultStyle)
    ));

    return group;
}


static XElement CreateGearShape(int cx, int cy, int radius, int teeth)
{
    var pathData = new StringBuilder();
    double angleStep = 2 * Math.PI / (teeth * 2);
    bool outer = true;

    for (int i = 0; i < teeth * 2; i++)
    {
        double angle = i * angleStep;
        double r = outer ? radius : radius * 0.75;
        double x = cx + r * Math.Cos(angle);
        double y = cy + r * Math.Sin(angle);

        if (i == 0)
            pathData.Append($"M {x:F2} {y:F2} ");
        else
            pathData.Append($"L {x:F2} {y:F2} ");

        outer = !outer;
    }

    pathData.Append("Z");

    return new XElement("path",
        new XAttribute("d", pathData.ToString()),
        new XAttribute("fill", "red"),
        new XAttribute("stroke", "black"),
        new XAttribute("stroke-width", "1")
    );
}










//static string GenerateComponent()
//{
//    string json = @"{
//            ""projection"": ""orthographic"",
//            ""components"": [
//                {
//                    ""componentType"": ""door"",
//                    ""name"": ""19693998"",
//                    ""props"": {
//                        ""Board"": ""board15"",
//                        ""Width"": ""39600"",
//                        ""Height"": ""65600"",
//                        ""HingeSku"": ""hinge""
//                    }
//                }
//            ]
//        }";

//    string svgXml = GenerateSvgFromJson(json);
//    return svgXml;
//}

//static string GenerateSvgFromJson(string json)
//{
//    var parsed = JObject.Parse(json);
//    var props = parsed["components"]?[0]?["props"];
//    if (props == null) return "<svg></svg>";

//    double width = double.Parse(props["Width"]!.ToString()) / 10.0;
//    double height = double.Parse(props["Height"]!.ToString()) / 10.0;
//    double dado = double.Parse(props["DadoThickness"]!.ToString()) / 10.0;

//    double halfWidth = width / 2;
//    double halfHeight = height / 2;

//    string viewBox = $"{-halfWidth} {-halfHeight} {width} {height}";

//    string outerPath = $"M {-halfWidth} {-halfHeight} L {halfWidth} {-halfHeight} L {halfWidth} {halfHeight} L {-halfWidth} {halfHeight} Z";

//    string redPathsTop = GenerateCirclePolygon(-halfHeight + dado, dado);
//    string redPathsBottom = GenerateCirclePolygon(halfHeight - dado, -dado);

//    string svg = $@"<svg viewBox=""{viewBox}"" width=""{width}"" height=""{height}"">
//  <g>
//    <path d=""{outerPath}"" style=""fill:#00004d;fill-opacity:0.2;stroke:black;stroke-width:10;stroke-linejoin:round;"" />
//  </g>
//  <g>
//    {redPathsTop}
//  </g>
//  <g>
//    {redPathsBottom}
//  </g>
//</svg>";

//    return svg;
//}

//static string GenerateCirclePolygon(double centerY, double offsetY)
//{
//    var sb = new StringBuilder();
//    int count = 12;
//    double radius = 350;
//    double centerX = 0;
//    for (int i = 0; i < count; i++)
//    {
//        double angle = 2 * Math.PI * i / count;
//        double x = centerX + Math.Cos(angle) * radius;
//        double y = centerY + Math.Sin(angle) * radius;

//        double x1 = x + 23 * Math.Cos(angle - 0.2);
//        double y1 = y + 23 * Math.Sin(angle - 0.2);

//        double x2 = x + 23 * Math.Cos(angle + 0.2);
//        double y2 = y + 23 * Math.Sin(angle + 0.2);

//        sb.AppendLine($@"<path d=""M {x:F1} {y:F1} L {x1:F1} {y1:F1} L {x2:F1} {y2:F1} Z"" style=""fill:red;fill-opacity:0.4;stroke:black;stroke-width:4;stroke-linejoin:round;"" />");
//    }
//    return sb.ToString();
//}





