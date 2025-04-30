using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace testesSvg;

public class SvgComponentBase
{
    static string GenerateComponent()
    {
        string json = @"{
            ""projection"": ""orthographic"",
            ""components"": [
                {
                    ""componentType"": ""base"",
                    ""name"": ""19693995"",
                    ""props"": {
                        ""Board"": ""board15"",
                        ""Width"": ""66000"",
                        ""Height"": ""35000"",
                        ""DadoThickness"": ""700""
                    }
                }
            ]
        }";

        return GenerateSvgFromJson(json);
    }

    static string GenerateSvgFromJson(string json)
    {
        var doc = JsonDocument.Parse(json);
        var props = doc.RootElement
                       .GetProperty("components")[0]
                       .GetProperty("props");

        int width = int.Parse(props.GetProperty("Width").GetString());
        int height = int.Parse(props.GetProperty("Height").GetString());

        int viewBoxX = -width / 20;
        int viewBoxY = -height / 20;
        int viewBoxWidth = width / 10;
        int viewBoxHeight = height / 10;

        var svg = new XElement("svg",
            new XAttribute("viewBox", $"{viewBoxX} {viewBoxY} {viewBoxWidth} {viewBoxHeight}"),
            new XAttribute("width", viewBoxWidth),
            new XAttribute("height", viewBoxHeight),
            //CreateBackgroundGroup(viewBoxWidth, viewBoxHeight, viewBoxX, viewBoxY),
            CreateHighlightGroups(viewBoxWidth, viewBoxHeight, viewBoxX, viewBoxY)
        );

        return svg.ToString();
    }


    static XElement CreateHighlightGroups(int w, int h, int x, int y)
    {
        var group = new XElement("g");

        int highlightHeight = 70;
        int highlightY = y + h - 230;

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
}
