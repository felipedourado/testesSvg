using System.Globalization;
using System.Xml.Linq;
using testesSvg.Components;

namespace testesSvg;

public static class SvgComponentSide
{
    private const double VIEWBOX_SCALE_FACTOR = 0.1; // 1/10
    private const double VIEWBOX_OFFSET_FACTOR = 0.05; // 1/20
    private const double EXPAND_FACTOR = 1.3; // 30% de expansão

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
        var payload = new SvgRequest
        {
            Width = "12000",
            Height = "20000",
            Thickness = "700",
            Offset = "0",
            JoinSystemType = "minifix",
            Type = "Side",
            IsLandscape = true
        };

        string svgXml = GenerateSvgFromJson(payload);
        return svgXml;
    }

    static string GenerateSvgFromJson(SvgRequest json)
    {
        int width = Convert.ToInt32(json.Width);
        int height = Convert.ToInt32(json.Height);

        int viewBoxX = -width / 20;
        int viewBoxY = -height / 20; //regra para BOARD 15
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
           , Label.CreateLabelGroup(w, h)
           , Label.CreateTopLabelGroup(w, h)
           , CreateBackgroundPath(viewBox.X, viewBox.Y)
       //TESTAR A BORDA SE TA FORMATANDO CERTO
          ,CreateBorderGroup(viewBoxWidth, viewBoxHeight, viewBoxX, viewBoxY, true, false, false, true)
       );

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

    static XElement CreateBackgroundPath(int viewBoxX, int viewBoxY)
    {
        var group = new XElement("g", new XAttribute("name", "background"));

        // Define os quatro pontos do retângulo
        int x1 = viewBoxX;
        int y1 = viewBoxY;
        int x2 = -viewBoxX;
        int y2 = viewBoxY;
        int x3 = -viewBoxX;
        int y3 = -viewBoxY;
        int x4 = viewBoxX;
        int y4 = -viewBoxY;

        // Monta o atributo 'd' do path
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
    static XElement CreateBorderGroup(int w, int h, int x, int y, bool top, bool right, bool bottom, bool left)
    {
        var group = new XElement("g", new XAttribute("name", "border"));

        // Estilo padrão
        string defaultStyle = "fill:none;stroke:#666;stroke-width:30;stroke-linejoin:round;stroke-dasharray:none;";
        // Estilo em destaque
        string highlightStyle = "fill:none;stroke:#000000;stroke-width:70;stroke-linejoin:round;";

        var borders = new[]
        {
            new { Name = "top",     X1 = x,     Y1 = y,     X2 = x + w, Y2 = y,     Highlight = top },
            new { Name = "right",   X1 = x + w, Y1 = y,     X2 = x + w, Y2 = y + h, Highlight = right },
            new { Name = "bottom",  X1 = x + w, Y1 = y + h, X2 = x,     Y2 = y + h, Highlight = bottom },
            new { Name = "left",    X1 = x,     Y1 = y + h, X2 = x,     Y2 = y,     Highlight = left }
        };

        foreach (var border in borders)
        {
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
        var (effectiveWidth, effectiveHeight) = isLandscape ? (height, width) : (width, height);

        return new ViewBoxData
        {
            X = (int)(-effectiveWidth * VIEWBOX_OFFSET_FACTOR),
            Y = (int)(-effectiveHeight * VIEWBOX_OFFSET_FACTOR),
            Width = (int)(effectiveWidth * VIEWBOX_SCALE_FACTOR),
            Height = (int)(effectiveHeight * VIEWBOX_SCALE_FACTOR),
            IsLandscape = isLandscape
        };
    }

    static ViewBoxData ExpandedViewBox(ViewBoxData viewBox)
    {
        double expandFactor = EXPAND_FACTOR;
        return new ViewBoxData
        {
            X = (int)(viewBox.X * expandFactor),
            Y = (int)(viewBox.Y * expandFactor),
            Width = (int)(viewBox.Width * expandFactor),
            Height = (int)(viewBox.Height * expandFactor),
            IsLandscape = viewBox.IsLandscape
        };
    }

    static string FormatViewBox(ViewBoxData viewBox)
    {
        return $"{viewBox.X} {viewBox.Y} {viewBox.Width} {viewBox.Height}";
    }
}

