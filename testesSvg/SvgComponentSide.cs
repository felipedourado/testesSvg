using System.Globalization;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using testesSvg.Components;

namespace testesSvg;

public static class SvgComponentSide
{
    private const double VIEWBOX_SCALE_FACTOR = 0.1; // 1/10
    private const double VIEWBOX_OFFSET_FACTOR = 0.05; // 1/20
    private const double EXPAND_FACTOR = 2.0;

    public static string GenerateComponent()
    {
        ////DobradicaMin DOIS FUROS-- 
        //var payload = new SvgRequest
        //{
        //    Width = "20000",
        //    Height = "30000",
        //    JoinSystemType = "door",
        //    HingeSku = "hinge"
        //};

        //PecaRebaixoMinifixMinParafuso --OK
        //var payload = new SvgRequest
        //{
        //    Width = "12000",
        //    Height = "20000",
        //    Thickness = "700",
        //    Offset = "0",
        //    JoinSystemType = "minifix",
        //    Type = "Side",
        //    IsLandscape = true,
        //    IsBorderBottom = true,
        //};

        var payload = new SvgRequest
        {
            Width = "1000",
            Height = "2000",
            IsLandscape = true,
            IsBorderBottom = true
        };

        string svgXml = GenerateSvgFromJson(payload);
        return svgXml;
    }

    static string GenerateSvgFromJson(SvgRequest json)
    {
        int width = Convert.ToInt32(json.Width);
        int height = Convert.ToInt32(json.Height);

        if (string.IsNullOrEmpty(json.JoinSystemType))
            return GenerateRectangleSvg(json);

        //int width = Convert.ToInt32(json.Width);
        //int height = Convert.ToInt32(json.Height);

        int viewBoxX = -width / 20;
        int viewBoxY = -height / 20;
        int viewBoxWidth = width / 10;
        int viewBoxHeight = height / 10;

        // double expandFactor = 1.3; // 30% maior
        //int expandedviewBoxX1 = (int)(viewBoxX * expandFactor);
        //int expandedviewBoxY1 = (int)(viewBoxY * expandFactor);

        //int expandedviewBoxWidth1 = (int)(viewBoxWidth * expandFactor);
        //int expandedviewBoxHeight1 = (int)(viewBoxHeight * expandFactor);

        //var svg = new XElement("svg",
        //    new XAttribute("viewBox", $"{expandedviewBoxX1} {expandedviewBoxY1} {expandedviewBoxWidth1} {expandedviewBoxHeight1}"),
        //    new XAttribute("width", width),
        //    new XAttribute("height", height)
        //    , Label.CreateLabelGroup(width, height)
        //    , Label.CreateTopLabelGroup(width, height)
        //    , CreateBackgroundPath(viewBoxX, viewBoxY)
        ////  ,CreateBorderGroup(viewBoxWidth, viewBoxHeight, viewBoxX, viewBoxY, true, false, false, true)
        //);

        var viewBox = CalculateViewBox(Convert.ToInt32(json.Width), Convert.ToInt32(json.Height), json.IsLandscape);
        var expandedViewBox = ExpandedViewBox(viewBox);

        var w = json.IsLandscape ? height : width;
        var h = json.IsLandscape ? width : height;

        var svg = new XElement("svg",
           new XAttribute("viewBox", FormatViewBox(expandedViewBox)),
           new XAttribute("width", w),
           new XAttribute("height", h)
           , Label.Height(w, h)
           , Label.Width(w, h)
           , CreateBackgroundPath(viewBox.X, viewBox.Y, json.IsLandscape)
       );

        if (json.IsAnyBorderValid())
            svg.Add(CreateBorderGroup(viewBox.Width, viewBox.Height, viewBox.X, viewBox.Y, json));

        int? dadoThickness = !string.IsNullOrEmpty(json.Thickness) ? Convert.ToInt32(json.Thickness) : null;

        if (dadoThickness is not null)
        {
            int? dadoOffsetFromEnd = !string.IsNullOrEmpty(json.Offset) ? Convert.ToInt32(json.Offset) : null;
            svg.Add(Thickness.CreateThickness(viewBoxWidth, height, viewBoxX, dadoOffsetFromEnd, (int)dadoThickness, json.IsLandscape));
        }

        if (!string.IsNullOrEmpty(json.JoinSystemType))
        {
            if (json.JoinSystemType.Equals("VbOne"))
            {
                if (json.Type.Equals("Base"))
                    svg.Add(VbOne.GenerateBase(viewBoxWidth, viewBoxHeight, json.IsLandscape));
                else
                    svg.Add(VbOne.GenerateSide(viewBoxWidth, viewBoxHeight, json.IsLandscape));
            }
            if (json.JoinSystemType.Equals("VbTwo"))
            {
                if (json.Type.Equals("Base"))
                    svg.Add(VbTwo.GenerateBase(viewBoxWidth, viewBoxHeight, json.IsLandscape));
                else
                    svg.Add(VbTwo.GenerateSide(viewBoxWidth, viewBoxHeight, json.IsLandscape));
            }
            if (json.JoinSystemType.Equals("dowels"))
            {
                if (json.Type.Equals("Base"))
                    svg.Add(Dowels.GenerateBase(viewBoxWidth, viewBoxHeight, json.IsLandscape));
                else
                    svg.Add(Dowels.GenerateSide(viewBoxWidth, viewBoxHeight, json.IsLandscape));
            }
            if (json.JoinSystemType.Equals("minifix"))
            {
                if (json.Type.Equals("Base"))
                    svg.Add(Minifix.GenerateBase(viewBoxWidth, viewBoxHeight, json.IsLandscape));

                if (json.Type.Equals("Side"))
                {
                    //add regra um furo, dois furos
                    svg.Add(Minifix.GenerateSide(viewBoxWidth, viewBoxHeight, json.IsLandscape));
                }

            }
            if (json.JoinSystemType.Equals("minifixanddowels"))
            {
                if (height <= 24999)
                {
                    if (json.Type.Equals("Side"))
                        svg.Add(Minifix.GenerateSide(viewBoxWidth, viewBoxHeight, json.IsLandscape));
                    if (json.Type.Equals("Base"))
                        svg.Add(Minifix.GenerateBase(viewBoxWidth, viewBoxHeight, json.IsLandscape));
                }
                else
                {
                    if (json.Type.Equals("Side"))
                        svg.Add(Minifix.GenerateMinifixDowelsSide(viewBoxWidth, viewBoxHeight, json.IsLandscape));
                    if (json.Type.Equals("Base"))
                        svg.Add(Minifix.GenerateMinifixDowelsBase(viewBoxWidth, viewBoxHeight, json.IsLandscape));
                }
            }
            if (json.JoinSystemType.Equals("door"))
            {
                svg.Add(Door.Generate(viewBoxWidth, viewBoxHeight));
            }

        }

        var ok = ResizeSvg(svg.ToString(), 48, 37.312);

        svg.Add(Label.Footer(h));

        return svg.ToString();

        #region testes
        //rebaixo linha vermelha colada no canto da peça, canal um pouco acima


        //testes
        //testar peças com rebaixo, testar 2 tamanhos de chapas (board) e conferir --feito
        //testar peças com canal, testar 2 tamanhos de chapas (board) e conferir --feito

        //testar peças com minifix
        //se receber rebaixo e minifix (por si so ja é parafuso ou seja component type side)

        //criar component type door
        //testar peças com dobradiças (door), testar 3 tamanhos de chapas (board) e conferir
        //fazer teste criando uma peça com dobradiça com parafuso e uma com tambor e analisar


        //---------------------------
        //testes usinagem

        //side (parafuso)
        //testar peças com rebaixo + minifix - ok
        //testar peças com rebaixo + cavilha - ok
        //testar peças com rebaixo + vb 1 furo - ok
        //testar peças com rebaixo + vb 2 furos - ok
        //testar peças com rebaixo + minifix + cavilha -

        //side (parafuso)
        //testar peças com canal + minifix - ok
        //testar peças com canal + cavilha -  (PecaCanalCavilhaMinParafuso final do arquivo exemplo de um furo para logica dobradiça)
        //testar peças com canal + vb 1 furo -
        //testar peças com canal + vb 2 furos - 
        //testar peças com canal + minifix + cavilha -

        //base (tambor)
        //testar peças com rebaixo + minifix - ok
        //testar peças com rebaixo + cavilha -
        //testar peças com rebaixo + vb 1 furo -
        //testar peças com rebaixo + vb 2 furos -
        //testar peças com rebaixo + minifix + cavilha -

        //side (tambor)
        //testar peças com canal + minifix - ok
        //testar peças com canal + cavilha -
        //testar peças com canal + vb 1 furo -
        //testar peças com canal + vb 2 furos -
        //testar peças com canal + minifix + cavilha -

        //se receber dowels (cavilha), tem que criar 3 dowels, top, center e down

        //pendente component type door
        //criar um mapeamento das peças que gerei no exporter, gerei no meu codigo e validei que estão iguais, se a config da peça já estiver no mapeamento gerar pelo meu servico e dar update na flag do banco
        //se estiver salvar uma flag no banco GInter

        #endregion
    }

    static XElement CreateOuterBackgroundPath(int viewBoxX, int viewBoxY)
    {
        var group = new XElement("g");

        // Calcula 30% a mais em cada direção
        double expandFactor = 1.8; // 30% maior
        int x1 = (int)(viewBoxX * expandFactor);
        int y1 = (int)(viewBoxY * expandFactor);
        int x2 = (int)(-viewBoxX * expandFactor);
        int y2 = (int)(viewBoxY * expandFactor);
        int x3 = (int)(-viewBoxX * expandFactor);
        int y3 = (int)(-viewBoxY * expandFactor);
        int x4 = (int)(viewBoxX * expandFactor);
        int y4 = (int)(-viewBoxY * expandFactor);

        // Monta o atributo 'd' do path
        string pathData = $"M {x1} {y1} L {x2} {y2} L {x3} {y3} L {x4} {y4} Z";

        group.Add(new XElement("path",
            new XAttribute("d", pathData),
            new XAttribute("style", "fill:white;stroke:none;"),
            new XAttribute("stroke", "none")
        ));

        return group;
    }

    static XElement CreateBackgroundPath(int viewBoxX, int viewBoxY, bool isLandscape)
    {
        var group = new XElement("g", new XAttribute("name", "background"), isLandscape ? new XAttribute("transform", "rotate(-90)") : null);

        int x1 = viewBoxX;
        int y1 = viewBoxY;
        int x2 = -viewBoxX;
        int y2 = viewBoxY;
        int x3 = -viewBoxX;
        int y3 = -viewBoxY;
        int x4 = viewBoxX;
        int y4 = -viewBoxY;

        string pathData = $"M {x1} {y1} L {x2} {y2} L {x3} {y3} L {x4} {y4} Z";

        group.Add(new XElement("path",
             new XAttribute("d", pathData),
             new XAttribute("style", "fill:white;stroke-linejoin:round;stroke-width:10;"),
             new XAttribute("stroke", "black")
         ));

        return group;
    }


    //QUANDO AUMENTA MT A BORDA COME UM PEDAÇO DA PEÇA
    //DIMINUI O ESPAÇO DO FURO COM A MARGEM DA PEÇA
    static XElement CreateBorderGroup(int w, int h, int x, int y, SvgRequest svg)
    {
        var group = new XElement("g", new XAttribute("name", "border"), svg.IsLandscape ? new XAttribute("transform", "rotate(-90)") : null);

        // Estilo padrão
        string defaultStyle = "fill:none;stroke:#666;stroke-width:30;stroke-linejoin:round;stroke-dasharray:none;";
        // Estilo em destaque
        string highlightStyle = "fill:none;stroke:#000000;stroke-width:70;stroke-linejoin:round;";

        var borders = new[]
        {
            new { Name = "top",     X1 = x,     Y1 = y,     X2 = x + w, Y2 = y,     Highlight = svg.IsBorderTop },
            new { Name = "right",   X1 = x + w, Y1 = y,     X2 = x + w, Y2 = y + h, Highlight = svg.IsBorderRight },
            new { Name = "bottom",  X1 = x + w, Y1 = y + h, X2 = x,     Y2 = y + h, Highlight = svg.IsBorderBottom },
            new { Name = "left",    X1 = x,     Y1 = y + h, X2 = x,     Y2 = y,     Highlight = svg.IsBorderLeft }
        };

        //VALIDAR NO SOFTWARE DE ETIQUETA
        foreach (var border in borders)
        {
            if (border.Highlight)
                group.Add(new XElement("line",
                    new XAttribute("x1", border.X1),
                    new XAttribute("y1", border.Y1),
                    new XAttribute("x2", border.X2),
                    new XAttribute("y2", border.Y2),
                    new XAttribute("style", border.Highlight ? highlightStyle : defaultStyle)
                ));
        }

        return group;
    }

    static string ResizeSvg(string svgContent, double targetWidthMm, double targetHeightMm)
    {
        XDocument svgDoc = XDocument.Parse(svgContent);
        var svgElement = svgDoc.Root;

        // Atualiza dimensões físicas
        svgElement.SetAttributeValue("width", $"{targetWidthMm.ToString(CultureInfo.InvariantCulture)}mm");
        svgElement.SetAttributeValue("height", $"{targetHeightMm.ToString(CultureInfo.InvariantCulture)}mm");

        // Garante que viewBox existe (se já existe, mantém)
        var viewBoxAttr = svgElement.Attribute("viewBox");
        if (viewBoxAttr == null)
        {
            // Se não tiver viewBox, calcula a partir de width/height originais (em px)
            var widthAttr = svgElement.Attribute("width")?.Value.Replace("mm", "");
            var heightAttr = svgElement.Attribute("height")?.Value.Replace("mm", "");
            double width = double.Parse(widthAttr, CultureInfo.InvariantCulture);
            double height = double.Parse(heightAttr, CultureInfo.InvariantCulture);
            svgElement.SetAttributeValue("viewBox", $"0 0 {width} {height}");
        }

        // Define o modo de ajuste e centralização
        svgElement.SetAttributeValue("preserveAspectRatio", "xMidYMid meet");

        // Aplica rotate(-90) em todos os elementos<g>
        var gElements = svgElement.Descendants().Where(el => el.Name.LocalName == "g");
        foreach (var g in gElements)
        {
            var existingTransform = g.Attribute("transform")?.Value;
            string newTransform = "rotate(-90)";

            if (!string.IsNullOrWhiteSpace(existingTransform))
            {
                newTransform = existingTransform + " " + newTransform;
            }

            g.SetAttributeValue("transform", newTransform);
        }
        // Remove qualquer transform herdado (opcional)
        svgElement.SetAttributeValue("transform", null);

        return svgDoc.Declaration + svgDoc.ToString();
    }




    static ViewBoxData CalculateViewBox(int width, int height, bool isLandscape)
    {
        return new ViewBoxData
        {
            X = (int)(-width * VIEWBOX_OFFSET_FACTOR),
            Y = (int)(-height * VIEWBOX_OFFSET_FACTOR),
            Width = (int)(width * VIEWBOX_SCALE_FACTOR),
            Height = (int)(height * VIEWBOX_SCALE_FACTOR),
            IsLandscape = isLandscape
        };
    }

    static ViewBoxData ExpandedViewBox(ViewBoxData viewBox)
    {
        var (viewBoxWidth, viewBoxHeight) = viewBox.IsLandscape ? (viewBox.Height, viewBox.Width) : (viewBox.Width, viewBox.Height);
        var (viewBoxY, viewBoxX) = viewBox.IsLandscape ? (viewBox.X, viewBox.Y) : (viewBox.Y, viewBox.X);

        return new ViewBoxData
        {
            X = (int)(viewBoxX * EXPAND_FACTOR),
            Y = (int)(viewBoxY * EXPAND_FACTOR),
            Width = (int)(viewBoxWidth * EXPAND_FACTOR),
            Height = (int)(viewBoxHeight * EXPAND_FACTOR),
            IsLandscape = viewBox.IsLandscape
        };
    }

    static string FormatViewBox(ViewBoxData viewBox)
    {
        return $"{viewBox.X} {viewBox.Y} {viewBox.Width} {viewBox.Height}";
    }

    static string GenerateRectangleSvg(SvgRequest svg)
    {
        int width = Convert.ToInt32(svg.Width);
        int height = Convert.ToInt32(svg.Height);
        const int thickStrokeWidth = 4;
        const int padding = 10;
        const int textPadding = 15; // Extra padding for text labels

        // Calculate SVG dimensions to accommodate the rectangle, strokes, and text
        int originalSvgWidth = width + (2 * padding) + thickStrokeWidth + textPadding + 30; // Extra space for width/height text
        int originalSvgHeight = height + (2 * padding) + thickStrokeWidth + textPadding + 20; // Extra space for width/height text

        // Calculate viewBox as a square - use the larger dimension
        int maxDimension = Math.Max(originalSvgWidth, originalSvgHeight);
        int squareViewBoxSize = (int)(maxDimension * 1.5); // 50% larger square

        // Both width and height are the same for a square viewBox
        int viewBoxWidth = squareViewBoxSize;
        int viewBoxHeight = squareViewBoxSize;

        // Calculate offset to center the background element
        int offsetX = (viewBoxWidth - originalSvgWidth) / 2;
        int offsetY = (viewBoxHeight - originalSvgHeight) / 2;

        // Calculate rectangle position
        int rectX = padding + (thickStrokeWidth / 2) + offsetX;
        int rectY = padding + (thickStrokeWidth / 2) + offsetY;

        var svgTeste = new XElement("svg",
          new XAttribute("viewBox", $"0 0 {viewBoxWidth} {viewBoxHeight}"),
          new XAttribute("width", viewBoxWidth),
          new XAttribute("height", viewBoxHeight));

        svgTeste.Add(BackgroundCut(rectX, rectY, width, height, svg.IsLandscape, squareViewBoxSize));
        svgTeste.Add(BorderCut(rectX, rectY, rectX + width, rectY + height, svg, squareViewBoxSize));

        int wH = svg.IsLandscape ? height : width;
        int hW = svg.IsLandscape ? width : height;

        svgTeste.Add(CreateTopWidthText(rectX, rectY, wH, viewBoxHeight, svg.IsLandscape ? height.ToString() : width.ToString()));
        svgTeste.Add(CreateLeftHeightText(rectX, rectY, width, height, viewBoxHeight, svg.IsLandscape ? width.ToString() : height.ToString()));


        if (svg.IsLandscape)
        {
            if (svg.IsBorderRight)
                svgTeste.Add(CreateTopL1Label(rectX, rectY, width, height, viewBoxHeight));
            if (svg.IsBorderBottom)
                svgTeste.Add(CreateRightC2Label(rectX, rectY, width, height, viewBoxHeight));
            if (svg.IsBorderLeft)
                svgTeste.Add(CreateBottomL2Label(rectX, rectY, width, height, viewBoxHeight));
            if (svg.IsBorderTop)
                svgTeste.Add(CreateLeftC1Label(rectX, rectY, width, height, viewBoxHeight));
        }
        else
        {
            if (svg.IsBorderRight)
                svgTeste.Add(CreateRightC2Label(rectX, rectY, width, height, viewBoxHeight));
            if (svg.IsBorderBottom)
                svgTeste.Add(CreateBottomL2Label(rectX, rectY, width, height, viewBoxHeight));
            if (svg.IsBorderLeft)
                svgTeste.Add(CreateLeftC1Label(rectX, rectY, width, height, viewBoxHeight));
            if (svg.IsBorderTop)
                svgTeste.Add(CreateTopL1Label(rectX, rectY, width, height, viewBoxHeight));
        }



        svgTeste.Add(CreateFooterText(viewBoxWidth, viewBoxHeight));

        return svgTeste.ToString();
    }

    static XElement BackgroundCut(int rectX, int rectY, int width, int height, bool isLandscape, int squareViewBoxSize)
    {
        XAttribute transformAttribute = null;

        if (isLandscape)
        {
            // Calculate the center of the square viewBox
            double viewBoxCenter = squareViewBoxSize / 2.0;
            string centerFormat = viewBoxCenter.ToString().Replace(',', '.');

            // Create rotation transform around the center
            transformAttribute = new XAttribute("transform", $"rotate(-90 {centerFormat} {centerFormat})");
        }

        var group = new XElement("g", new XAttribute("name", "background"), transformAttribute);

        int x1 = rectX;
        int y1 = rectY;
        int x2 = rectX + width;
        int y2 = rectY;
        int x3 = rectX + width;
        int y3 = rectY + height;
        int x4 = rectX;
        int y4 = rectY + height;

        string pathData = $"M {x1} {y1} L {x2} {y2} L {x3} {y3} L {x4} {y4} Z";

        group.Add(new XElement("path",
             new XAttribute("d", pathData),
             new XAttribute("style", "fill:none;stroke-width:10;"),
             new XAttribute("stroke", "black")
         ));

        return group;
    }

    static XElement BorderCut(int x1, int y1, int x2, int y2, SvgRequest svg, int squareViewBoxSize)
    {
        XAttribute transformAttribute = null;

        if (svg.IsLandscape)
        {
            // Calculate the center of the square viewBox
            double viewBoxCenter = squareViewBoxSize / 2.0;
            string centerFormat = viewBoxCenter.ToString().Replace(',', '.');

            // Create rotation transform around the center
            transformAttribute = new XAttribute("transform", $"rotate(-90 {centerFormat} {centerFormat})");
        }

        var bordergroup = new XElement("g", new XAttribute("name", "border"), transformAttribute);

        // Estilo padrão
        string defaultStyle = "fill:none;stroke:#666;stroke-width:30;stroke-linejoin:round;stroke-dasharray:none;";
        // Estilo em destaque
        string highlightStyle = "fill:none;stroke:#000000;stroke-width:30;";

        var borders = new[]
        {
            new { Name = "top",     X1 = x1,     Y1 = y1,     X2 = x2, Y2 = y1,     Highlight = svg.IsBorderTop },
            new { Name = "right",   X1 = x2, Y1 = y1,     X2 = x2, Y2 = y2, Highlight = svg.IsBorderRight },
            new { Name = "bottom",  X1 = x2, Y1 = y2, X2 = x1,     Y2 = y2, Highlight = svg.IsBorderBottom },
            new { Name = "left",    X1 = x1,     Y1 = y2, X2 = x1,     Y2 = y1,     Highlight = svg.IsBorderLeft }
        };

        //VALIDAR NO SOFTWARE DE ETIQUETA
        foreach (var border in borders)
        {
            if (border.Highlight)
                bordergroup.Add(new XElement("line",
                    new XAttribute("x1", border.X1),
                    new XAttribute("y1", border.Y1),
                    new XAttribute("x2", border.X2),
                    new XAttribute("y2", border.Y2),
                    new XAttribute("style", border.Highlight ? highlightStyle : defaultStyle)
                ));
        }

        return bordergroup;
    }

    static XElement CreateFooterText(int viewBoxWidth, int viewBoxHeight, string text = "Face de Alinhamento")
    {
        // Calculate footer positioning
        int footerMargin = (int)(viewBoxHeight * 0.05);
        int textX = viewBoxWidth / 2; // Center horizontally
        int textY = viewBoxHeight - footerMargin; // Position from bottom

        // Calculate font size based on viewBox height (responsive sizing)
        int fontSize = Math.Max(12, viewBoxHeight / 20); // Minimum 12px, scales with height

        var footerGroup = new XElement("g",
            new XAttribute("name", "footer")
        );

        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "middle"), // Center text horizontally
            new XAttribute("dominant-baseline", "bottom"), // Align text to bottom
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", fontSize),
            new XAttribute("fill", "#333333"),
            text
        );

        footerGroup.Add(textElement);

        return footerGroup;
    }

    static XElement CreateTopWidthText(int rectX, int rectY, int width, int viewBoxHeight, string text)
    {
        // Calculate positioning above the top line of the background
        int textMargin = (int)(viewBoxHeight * 0.02); // 2% of viewBox height as margin above rectangle
        int textX = rectX + (width / 2); // Center horizontally over the rectangle
        int textY = rectY - textMargin; // Position above the rectangle

        // Calculate font size based on viewBox height (responsive sizing)
        int fontSize = Math.Max(10, viewBoxHeight / 30); // Minimum 10px, scales with height

        var topWidthGroup = new XElement("g",
            new XAttribute("name", "top-width-label")
        );

        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "middle"), // Center text horizontally
            new XAttribute("dominant-baseline", "bottom"), // Align text to bottom (sits above rectangle)
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", fontSize),
            new XAttribute("fill", "#666666"),
            text // Display the width value
        );

        topWidthGroup.Add(textElement);

        return topWidthGroup;
    }

    static XElement CreateLeftHeightText(int rectX, int rectY, int width, int height, int viewBoxHeight, string text)
    {
        // Calculate positioning to the left of the background rectangle
        int textMargin = (int)(viewBoxHeight * 0.02); // 2% of viewBox height as margin from rectangle
        int textX = rectX - textMargin; // Position to the left of the rectangle
        int textY = rectY + (height / 2); // Center vertically along the rectangle height

        // Calculate font size based on viewBox height (responsive sizing)
        int fontSize = Math.Max(10, viewBoxHeight / 30); // Minimum 10px, scales with height

        var leftHeightGroup = new XElement("g",
            new XAttribute("name", "left-height-label")
        );

        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "end"), // Align text to the right (away from rectangle)
            new XAttribute("dominant-baseline", "middle"), // Center text vertically
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", fontSize),
            new XAttribute("fill", "#666666"),
            text // Display the height value
        );

        leftHeightGroup.Add(textElement);

        return leftHeightGroup;
    }

    static XElement CreateLeftC1Label(int rectX, int rectY, int width, int height, int viewBoxHeight)
    {
        // Calculate positioning above the height text (to the left of the rectangle)
        int textMargin = (int)(viewBoxHeight * 0.02); // Same margin as height text
        int labelSpacing = (int)(viewBoxHeight * 0.035); // Additional spacing above height text

        int textX = rectX - textMargin; // Same X position as height text
        int heightTextY = rectY + (height / 2); // Y position of height text
        int textY = heightTextY - labelSpacing; // Position above the height text

        // Calculate font size based on viewBox height (slightly smaller than height text)
        int fontSize = Math.Max(8, viewBoxHeight / 30); // Minimum 8px, scales with height

        var leftC1Group = new XElement("g",
            new XAttribute("name", "left-c1-label")
        );

        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "end"), // Align text to the right (same as height text)
            new XAttribute("dominant-baseline", "bottom"), // Align to bottom so it sits above height text
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", fontSize),
            new XAttribute("fill", "#333333"), // Slightly darker than height text
            new XAttribute("font-weight", "bold"), // Make label bold
            "C1"
        );

        leftC1Group.Add(textElement);

        return leftC1Group;
    }

    static XElement CreateTopL1Label(int rectX, int rectY, int width, int height, int viewBoxHeight)
    {
        // Calculate positioning above the width text (above the rectangle)
        int textMargin = (int)(viewBoxHeight * 0.02); // Same margin as width text
        int labelSpacing = (int)(viewBoxHeight * 0.035); // Additional spacing above width text

        int textX = rectX + (width / 2); // Same X position as width text (centered)
        int widthTextY = rectY - textMargin; // Y position of width text
        int textY = widthTextY - labelSpacing; // Position above the width text

        // Calculate font size based on viewBox height (slightly smaller than width text)
        int fontSize = Math.Max(8, viewBoxHeight / 30); // Minimum 8px, scales with height

        var topL1Group = new XElement("g",
            new XAttribute("name", "top-l1-label")
        );

        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "middle"), // Center text horizontally (same as width text)
            new XAttribute("dominant-baseline", "bottom"), // Align to bottom so it sits above width text
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", fontSize),
            new XAttribute("fill", "#333333"), // Slightly darker than width text
            new XAttribute("font-weight", "bold"), // Make label bold
            "L1"
        );

        topL1Group.Add(textElement);

        return topL1Group;
    }

    static XElement CreateRightC2Label(int rectX, int rectY, int width, int height, int viewBoxHeight)
    {
        // Calculate positioning above the height text (to the right of the rectangle)
        int textMargin = (int)(viewBoxHeight * 0.02); // Same margin as height text
        int labelSpacing = (int)(viewBoxHeight * 0.035); // Additional spacing above height text
        int textX = rectX + width + textMargin; // Position to the right of the rectangle
        int heightTextY = rectY + (height / 2); // Y position of height text
        int textY = heightTextY - labelSpacing; // Position above the height text (same height as C1)

        // Calculate font size based on viewBox height (slightly smaller than height text)
        int fontSize = Math.Max(8, viewBoxHeight / 30); // Minimum 8px, scales with height

        var rightC2Group = new XElement("g",
            new XAttribute("name", "right-c2-label")
        );

        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "start"), // Align text to the left (opposite of C1)
            new XAttribute("dominant-baseline", "bottom"), // Align to bottom so it sits above height text
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", fontSize),
            new XAttribute("fill", "#333333"), // Slightly darker than height text
            new XAttribute("font-weight", "bold"), // Make label bold
            "C2"
        );

        rightC2Group.Add(textElement);
        return rightC2Group;
    }

    static XElement CreateBottomL2Label(int rectX, int rectY, int width, int height, int viewBoxHeight)
    {
        // Calculate positioning below the width text (below the rectangle)
        int textMargin = (int)(viewBoxHeight * 0.02); // Same margin as width text
        int labelSpacing = (int)(viewBoxHeight * 0.035); // Additional spacing below width text
        int textX = rectX + (width / 2); // Same X position as width text (centered)
        int rectBottomY = rectY + height; // Bottom Y position of rectangle
        int textY = rectBottomY + textMargin + labelSpacing; // Position below the rectangle

        // Calculate font size based on viewBox height (slightly smaller than width text)
        int fontSize = Math.Max(8, viewBoxHeight / 30); // Minimum 8px, scales with height

        var bottomL2Group = new XElement("g",
            new XAttribute("name", "bottom-l2-label")
        );

        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "middle"), // Center text horizontally (same as width text)
            new XAttribute("dominant-baseline", "top"), // Align to top so it sits below the rectangle
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", fontSize),
            new XAttribute("fill", "#333333"), // Slightly darker than width text
            new XAttribute("font-weight", "bold"), // Make label bold
            "L2"
        );

        bottomL2Group.Add(textElement);
        return bottomL2Group;
    }
}

