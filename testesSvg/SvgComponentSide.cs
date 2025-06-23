using System.Globalization;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using testesSvg.Components;

namespace testesSvg;

public static class SvgComponentSide
{
    private const double VIEWBOX_SCALE_FACTOR = 0.1; // 1/10
    private const double VIEWBOX_OFFSET_FACTOR = 0.05; // 1/20
    private const double EXPAND_FACTOR = 1.5;

    public static string GenerateComponent()
    {
        //Minifix-Rebaixo-Max - SEM LANDSCAPE
        //var payload = new SvgRequest
        //{
        //    Width = "219000",
        //    Height = "159000",
        //    Thickness = "700",
        //    Offset = "1500",
        //    JoinSystemType = "minifix",
        //    Type = "Side",
        //    IsBorderRight = true,
        //    IsBorderLeft = true,
        //    IsBorderTop = true,
        //    IsBorderBottom = true,
        //    BorderBottomLabel = "123456",
        //    BorderLeftLabel = "123456",
        //    BorderRightLabel = "123456",
        //    BorderTopLabel = "123456",
        //    IsTicket = true
        //};

        var payload = new SvgRequest
        {
            Width = "159000",
            Height = "219000",
            Thickness = "700",
            Offset = "1500",
            JoinSystemType = "minifix",
            Type = "Side",
            IsBorderRight = true,
            IsBorderLeft = true,
            IsBorderTop = true,
            IsBorderBottom = true,
            BorderBottomLabel = "123456",
            BorderLeftLabel = "123456",
            BorderRightLabel = "123456",
            BorderTopLabel = "123456",
            IsTicket = true,
            IsLandscape = true
        };

        //Minifix-Rebaixo-Min - SEM LANDSCAPE
        //var payload = new SvgRequest
        //{
        //    Width = "20000",
        //    Height = "14000",
        //    Thickness = "700",
        //    Offset = "1500",
        //    JoinSystemType = "minifix",
        //    Type = "Side",
        //    IsBorderRight = true,
        //    IsBorderLeft = true,
        //    IsBorderTop = true,
        //    IsBorderBottom = true,
        //    BorderBottomLabel = "123456",
        //    BorderLeftLabel = "123456",
        //    BorderRightLabel = "123456",
        //    BorderTopLabel = "123456",
        //    IsTicket = true,
        //    HeightLabel = "14000",
        //    WidthLabel = "20000"
        //};


        //PecaRebaixoMinifixMinParafuso - COM LANDSCAPE
        //var payload = new SvgRequest
        //{
        //    Width = "14000",
        //    Height = "20000",
        //    Thickness = "700",
        //    Offset = "1500",
        //    JoinSystemType = "minifix",
        //    Type = "Side",
        //    IsBorderRight = true,
        //    IsBorderLeft = true,
        //    IsBorderTop = true,
        //    IsBorderBottom = true,
        //    BorderBottomLabel = "123456",
        //    BorderLeftLabel = "123456",
        //    BorderRightLabel = "123456",
        //    BorderTopLabel = "123456",
        //    IsLandscape = true
        //};

        //Retangulo Minifix - Preset TicketPrint
        //var payload = new SvgRequest
        //{
        //    Width = "5469",
        //    Height = "3308",
        //    Thickness = "290",
        //    Offset = "412",
        //    JoinSystemType = "minifix",
        //    Type = "Side",
        //    IsTicket = true
        //};

        string svgXml = GenerateSvgFromJson(payload);
        return svgXml;
    }

    //aguardando validação dos tamanhos pre-setados para quadrado e retangulo para corte e borda
    //trazer a logica do landscape para o isticket true para saber onde vai bordear corretamente
    //iniciar pre-set para vbone
    //criar regra para quadrado e retangulo (todas as peças deverão ter essa regras, criar um template de quadrado e retangulo fixo)
    //validar logica da flag etiqueta com landscape para não perder a referencia
    //criar legenda para aparecer o codigo e nome da borda (alterar L1, L2 para B1, B2)


    static string GenerateSvgFromJson(SvgRequest json)
    {
        if (json.IsTicket && string.IsNullOrEmpty(json.JoinSystemType))
            TicketView(json);

        if (string.IsNullOrEmpty(json.JoinSystemType))
            return GenerateRectangleSvg(json);

        int width = Convert.ToInt32(json.Width);
        int height = Convert.ToInt32(json.Height);

        string doorItens = string.Empty;

        if (!string.IsNullOrEmpty(json.JoinSystemType) && json.JoinSystemType.Equals("door"))
        {
            doorItens = "double";

            var heightDouble = height / 10.0;

            if (heightDouble >= 9000.1 && heightDouble < 1600.1)
                doorItens = "triple";

            if (heightDouble >= 1600.1 && heightDouble < 2000.1)
                doorItens = "quad";

            if (heightDouble > 2000.1)
                doorItens = "quin";
        }

        int viewBoxX = -width / 20;
        int viewBoxY = -height / 20;
        int viewBoxWidth = width / 10;
        int viewBoxHeight = height / 10;

        var viewBox = CalculateViewBox(Convert.ToInt32(json.Width), Convert.ToInt32(json.Height), json.IsLandscape);
        var expandedViewBox = ExpandedViewBox(viewBox);

        var w = json.IsLandscape ? height : width;
        var h = json.IsLandscape ? width : height;

        var svg = new XElement("svg",
           new XAttribute("viewBox", FormatViewBox(expandedViewBox)),
           new XAttribute("width", w),
           new XAttribute("height", h)
           , Label.HeightSystemType(w, h, expandedViewBox.Height, json.HeightLabel)
           , Label.WidthSystemType(w, h, expandedViewBox.Height, json.WidthLabel)
           , CreateBackgroundPath(viewBox.X, viewBox.Y, json.IsLandscape)
       );

        if (json.IsAnyBorderValid())
            svg.Add(CreateBorderGroup(viewBox.Width, viewBox.Height, viewBox.X, viewBox.Y, json));

        if (json.IsLandscape)
        {
            if (json.IsBorderRight)
                svg.Add(BorderTopLabel(expandedViewBox.X, expandedViewBox.Y, expandedViewBox.Width, expandedViewBox.Height, json.BorderTopLabel));
            if (json.IsBorderBottom)
                svg.Add(BorderRightLabel(expandedViewBox.X, expandedViewBox.Y, expandedViewBox.Width, expandedViewBox.Height, json.BorderRightLabel));
            if (json.IsBorderLeft)
                svg.Add(BorderBottomLabel(expandedViewBox.X, expandedViewBox.Y, expandedViewBox.Width, expandedViewBox.Height, json.BorderBottomLabel));
            if (json.IsBorderTop)
                svg.Add(BorderLeftLabel(expandedViewBox.X, expandedViewBox.Y, expandedViewBox.Width, expandedViewBox.Height, json.BorderLeftLabel));
        }
        else
        {
            if (json.IsBorderRight)
                svg.Add(BorderRightLabel(expandedViewBox.X, expandedViewBox.Y, expandedViewBox.Width, expandedViewBox.Height, json.BorderRightLabel));
            if (json.IsBorderBottom)
                svg.Add(BorderBottomLabel(expandedViewBox.X, expandedViewBox.Y, expandedViewBox.Width, expandedViewBox.Height, json.BorderBottomLabel));
            if (json.IsBorderLeft)
                svg.Add(BorderLeftLabel(expandedViewBox.X, expandedViewBox.Y, expandedViewBox.Width, expandedViewBox.Height, json.BorderLeftLabel));
            if (json.IsBorderTop)
                svg.Add(BorderTopLabel(expandedViewBox.X, expandedViewBox.Y, expandedViewBox.Width, expandedViewBox.Height, json.BorderTopLabel));
        }

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
                svg.Add(Door.Generate(viewBoxWidth, viewBoxHeight, doorItens, json.IsLandscape));
            }

        }

        svg.Add(Label.Footer(expandedViewBox.Height, expandedViewBox.Y));

        if (json.IsTicket)
        {
            var ok = ResizeSvg(svg.ToString(), 48, 37);
            return ok;
        }

        return svg.ToString();
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
        //var gElements = svgElement.Descendants().Where(el => el.Name.LocalName == "g");
        //foreach (var g in gElements)
        //{
        //    var existingTransform = g.Attribute("transform")?.Value;
        //    string newTransform = "rotate(-90)";

        //    if (!string.IsNullOrWhiteSpace(existingTransform))
        //    {
        //        newTransform = existingTransform + " " + newTransform;
        //    }

        //    g.SetAttributeValue("transform", newTransform);
        //}
        // Remove qualquer transform herdado (opcional)
        svgElement.SetAttributeValue("transform", null);

        return svgDoc.Declaration + svgDoc.ToString();
    }

    static string RedimensionarSvg(string svgContent, double novoX, double novoY, double novaLargura, double novaAltura, string unidade = "mm")
    {
        try
        {
            // Carregar o SVG como XML
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(svgContent);

            XmlElement svgElement = doc.DocumentElement;

            // Obter viewBox original
            string viewBoxOriginal = svgElement.GetAttribute("viewBox");
            if (string.IsNullOrEmpty(viewBoxOriginal))
            {
                throw new ArgumentException("SVG deve ter um atributo viewBox");
            }

            // Parse do viewBox: "minX minY width height"
            string[] viewBoxValues = viewBoxOriginal.Split(' ');
            if (viewBoxValues.Length != 4)
            {
                throw new ArgumentException("viewBox deve ter 4 valores");
            }

            double minX = double.Parse(viewBoxValues[0]);
            double minY = double.Parse(viewBoxValues[1]);
            double larguraViewBox = double.Parse(viewBoxValues[2]);
            double alturaViewBox = double.Parse(viewBoxValues[3]);

            // Definir as novas dimensões e posição
            svgElement.SetAttribute("x", $"{novoX}{unidade}");
            svgElement.SetAttribute("y", $"{novoY}{unidade}");
            svgElement.SetAttribute("width", $"{novaLargura}{unidade}");
            svgElement.SetAttribute("height", $"{novaAltura}{unidade}");

            // Manter o viewBox original para preservar as proporções do conteúdo
            svgElement.SetAttribute("viewBox", viewBoxOriginal);

            // Aplicar preserveAspectRatio para manter proporções e centralizar
            // "xMidYMid meet" mantém as proporções e centraliza o conteúdo
            svgElement.SetAttribute("preserveAspectRatio", "xMidYMid meet");

            return doc.OuterXml;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao redimensionar SVG: {ex.Message}", ex);
        }
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

    #region LegendasAntigas
    static XElement CreateRightC2LabelDrill(int x, int y, int viewBoxWidth, int viewBoxHeight)
    {
        // Calculate positioning at 20% of viewBox width from right edge
        int marginDistance = (int)(viewBoxWidth * 0.02); // 80% from left = 20% from right
        // Position vertically centered in the viewBox
        //int textY = viewBoxHeight / 2;
        int textX = (x + viewBoxWidth) - marginDistance;

        int textY = y + (viewBoxHeight / 2);
        // Calculate font size based on viewBox height
        int fontSize = Math.Max(8, viewBoxHeight / 30); // Minimum 8px, scales with height

        var rightC2Group = new XElement("g",
            new XAttribute("name", "right-c2-label")
        );

        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "end"), // Align text to the right
            new XAttribute("dominant-baseline", "middle"), // Center the text vertically
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", fontSize),
            new XAttribute("fill", "#333333"), // Dark gray color
            new XAttribute("font-weight", "bold"), // Make label bold
            "C2"
        );

        rightC2Group.Add(textElement);
        return rightC2Group;
    }

    static XElement CreateLeftC1LabelDrill(int x, int y, int viewBoxWidth, int viewBoxHeight)
    {
        // Valores padrão
        // Calculate font size based on viewBox height
        int fontSize = Math.Max(8, viewBoxHeight / 30); // Minimum 8px, scales with height

        // Calcular posição X: 20% de distância da margem esquerda
        // margem esquerda do viewBox = viewBoxX
        // 20% da largura do viewBox = viewBoxWidth * 0.2
        int marginDistance = (int)(viewBoxWidth * 0.02);
        int textX = x + marginDistance;

        // Calcular posição Y: meio do eixo Y
        int textY = y + (viewBoxHeight / 2);

        // Criar o elemento text usando XElement
        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "start"),
            new XAttribute("dominant-baseline", "middle"),
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", fontSize),
            new XAttribute("fill", "#333333"),
            new XAttribute("font-weight", "bold"),
            "C1"
        );

        // Criar o elemento group contendo o text
        var groupElement = new XElement("g",
            new XAttribute("name", "left-c1-label"),
            textElement
        );

        return groupElement;
    }

    static XElement CreateTopL1LabelDrill(int viewBoxX, int viewBoxY, int viewBoxWidth, int viewBoxHeight)
    {
        int fontSize = Math.Max(8, viewBoxHeight / 30);

        int textX = viewBoxX + (viewBoxWidth / 2);

        int marginDistance = (int)(viewBoxHeight * 0.05);
        int textY = viewBoxY + marginDistance;

        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "middle"),
            new XAttribute("dominant-baseline", "middle"),
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", fontSize),
            new XAttribute("fill", "#333333"),
            new XAttribute("font-weight", "bold"),
            "L1"
        );

        var groupElement = new XElement("g",
            new XAttribute("name", "l1-label"),
            textElement
        );

        return groupElement;
    }

    static XElement CreateBottomL2LabelDrill(int viewBoxX, int viewBoxY, int viewBoxWidth, int viewBoxHeight)
    {
        // Calcular posição X: meio do eixo X
        int fontSize = Math.Max(8, viewBoxHeight / 30);

        int textX = viewBoxX + (viewBoxWidth / 2);

        int marginDistance = (int)(viewBoxHeight * 0.08);
        int textY = (viewBoxY + viewBoxHeight) - marginDistance;

        // Criar o elemento text usando XElement
        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "middle"),
            new XAttribute("dominant-baseline", "middle"),
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", fontSize),
            new XAttribute("fill", "#333333"),
            new XAttribute("font-weight", "bold"),
            "L2"
        );

        // Criar o elemento group contendo o text
        var groupElement = new XElement("g",
            new XAttribute("name", "l2-label"),
            textElement
        );

        return groupElement;
    }

    #endregion

    #region LegendasNovas

    static XElement BorderRightLabel(int x, int y, int viewBoxWidth, int viewBoxHeight, string label, int xOffset = 11)
    {
        // Calculate offset to center the background element
        int offsetX = (viewBoxWidth - viewBoxWidth) / 2;
        int offsetY = (viewBoxHeight - viewBoxWidth) / 2;

        int textX = x + offsetY; // ligeiramente à direita da borda

        double rotateY = y + (viewBoxWidth / 2.0);

        int marginDistance = (int)(viewBoxWidth * 0.02);
        //int textX = (x + viewBoxWidth) - marginDistance;

        int textY = y + (viewBoxHeight / 2);
        int fontSize = Math.Max(8, viewBoxHeight / 30);

        var rightC2Group = new XElement("g", new XAttribute("name", "right-c2-label"));

        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "end"),
            new XAttribute("dominant-baseline", "middle"),
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", fontSize),
            new XAttribute("fill", "#333333"),
            new XAttribute("font-weight", "bold"),
            new XAttribute("transform", $"rotate(-90 {textX} {rotateY.ToString().Replace(',', '.')} )"),
            label
        );

        rightC2Group.Add(textElement);
        return rightC2Group;
    }

    static XElement BorderLeftLabel(int x, int y, int viewBoxWidth, int viewBoxHeight, string label, int xOffset = 13)
    {
        // Valores padrão
        // Calculate font size based on viewBox height
        int fontSize = Math.Max(8, viewBoxHeight / 30);

        // Calcular posição X: 20% de distância da margem esquerda
        // margem esquerda do viewBox = viewBoxX
        // 20% da largura do viewBox = viewBoxWidth * 0.2
        int marginDistance = (int)(viewBoxWidth * 0.02);

        int textX = x - xOffset;

        // Calcular posição Y: meio do eixo Y
        int textY = y + (viewBoxHeight / 2);

        double rotateY = y + (viewBoxHeight / 2.0);

        // Criar o elemento text usando XElement
        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "start"),
            new XAttribute("dominant-baseline", "middle"),
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", fontSize),
            new XAttribute("fill", "#333333"),
            new XAttribute("font-weight", "bold"),
            new XAttribute("transform", $"rotate(-90 {textX} {rotateY.ToString().Replace(',', '.')} )"),
            label
        );

        var groupElement = new XElement("g", new XAttribute("name", "left-c1-label"), textElement);

        return groupElement;
    }

    static XElement BorderTopLabel(int viewBoxX, int viewBoxY, int viewBoxWidth, int viewBoxHeight, string label)
    {
        int fontSize = Math.Max(8, viewBoxHeight / 20);

        int textX = viewBoxX + (viewBoxWidth / 2);

        int marginDistance = (int)(viewBoxHeight * 0.05);
        int textY = viewBoxY + marginDistance;

        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "middle"),
            new XAttribute("dominant-baseline", "middle"),
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", fontSize),
            new XAttribute("fill", "#333333"),
            new XAttribute("font-weight", "bold"),
            label
        );

        var groupElement = new XElement("g",
            new XAttribute("name", "l1-label"),
            textElement
        );

        return groupElement;
    }

    static XElement BorderBottomLabel(int viewBoxX, int viewBoxY, int viewBoxWidth, int viewBoxHeight, string label)
    {
        // Calcular posição X: meio do eixo X
        int fontSize = Math.Max(8, viewBoxHeight / 20);

        int textX = viewBoxX + (viewBoxWidth / 2);

        int marginDistance = (int)(viewBoxHeight * 0.08);
        int textY = (viewBoxY + viewBoxHeight) - marginDistance;

        // Criar o elemento text usando XElement
        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "middle"),
            new XAttribute("dominant-baseline", "middle"),
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", fontSize),
            new XAttribute("fill", "#333333"),
            new XAttribute("font-weight", "bold"),
            label
        );

        // Criar o elemento group contendo o text
        var groupElement = new XElement("g",
            new XAttribute("name", "l2-label"),
            textElement
        );

        return groupElement;
    }

    #endregion 


    //-------------------------------------------------
    static string GenerateRectangleSvg(SvgRequest svg)
    {
        if (svg.IsTicket)
            TicketView(svg);

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

        var rectBox = new ViewBoxData
        {
            Height = height,
            Width = width,
            X = rectX,
            Y = rectY,
            IsLandscape = svg.IsLandscape
        };

        svgTeste.Add(BackgroundCut(rectBox, squareViewBoxSize));
        svgTeste.Add(BorderCut(rectX, rectY, rectX + width, rectY + height, svg, squareViewBoxSize));

        svgTeste.Add(CreateTopWidthText(rectX, rectY, width, height, viewBoxWidth, viewBoxHeight, svg.IsLandscape));
        svgTeste.Add(CreateLeftHeightText(rectX, rectY, width, height, viewBoxWidth, viewBoxHeight, svg.IsLandscape));


        if (svg.IsLandscape)
        {
            if (svg.IsBorderRight)
                svgTeste.Add(CreateTopL1Label(rectX, rectY, width, height, viewBoxHeight));
            if (svg.IsBorderBottom)
                //svgTeste.Add(CreateRightC2Label(rectX, rectY, width, height, viewBoxWidth, viewBoxHeight, svg.IsLandscape));
                svgTeste.Add(CreateRightC2Label(viewBoxWidth, viewBoxHeight));
            if (svg.IsBorderLeft)
                svgTeste.Add(CreateBottomL2Label(rectX, rectY, width, height, viewBoxHeight));
            if (svg.IsBorderTop)
                //svgTeste.Add(CreateLeftC1Label(rectX, rectY, width, height, viewBoxHeight));
                svgTeste.Add(CreateLeftC1Label(viewBoxWidth, viewBoxHeight));
        }
        else
        {
            if (svg.IsBorderRight)
                //svgTeste.Add(CreateRightC2Label(rectX, rectY, width, height, viewBoxWidth, viewBoxHeight, svg.IsLandscape));
                svgTeste.Add(CreateRightC2Label(viewBoxWidth, viewBoxHeight));
            if (svg.IsBorderBottom)
                svgTeste.Add(CreateBottomL2Label(rectX, rectY, width, height, viewBoxHeight));
            if (svg.IsBorderLeft)
                //svgTeste.Add(CreateLeftC1Label(rectX, rectY, width, height, viewBoxHeight));
                svgTeste.Add(CreateLeftC1Label(viewBoxWidth, viewBoxHeight));
            if (svg.IsBorderTop)
                svgTeste.Add(CreateTopL1Label(rectX, rectY, width, height, viewBoxHeight));
        }



        svgTeste.Add(CreateFooterText(viewBoxWidth, viewBoxHeight));

        return svgTeste.ToString();
    }

    static XElement BackgroundCut(ViewBoxData rectBox, int squareViewBoxSize, bool? isTicket = null)
    {
        XAttribute transformAttribute = null;

        if (rectBox.IsLandscape)
        {
            // Calculate the center of the square viewBox
            double viewBoxCenter = squareViewBoxSize / 2.0;
            string centerFormat = viewBoxCenter.ToString().Replace(',', '.');

            // Create rotation transform around the center
            transformAttribute = new XAttribute("transform", $"rotate(-90 {centerFormat} {centerFormat})");
        }

        var group = new XElement("g", new XAttribute("name", "background"), transformAttribute);

        int x1 = rectBox.X;
        int y1 = rectBox.Y;
        int x2 = rectBox.X + rectBox.Width;
        int y2 = rectBox.Y;
        int x3 = rectBox.X + rectBox.Width;
        int y3 = rectBox.Y + rectBox.Height;
        int x4 = rectBox.X;
        int y4 = rectBox.Y + rectBox.Height;

        string pathData = $"M {x1} {y1} L {x2} {y2} L {x3} {y3} L {x4} {y4} Z";

        var valueStroke = isTicket.HasValue && isTicket.Value ? "0.3" : "10";

        group.Add(new XElement("path",
             new XAttribute("d", pathData),
             new XAttribute("style", $"fill:none;stroke-width:{valueStroke};"),
             new XAttribute("stroke", "black")
         ));

        return group;
    }

    static XElement BorderCut(int x1, int y1, int x2, int y2, SvgRequest svg, int squareViewBoxSize, bool? isTicket = null)
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
        var defaultStroke = isTicket.HasValue && isTicket.Value ? "0.3" : "10";
        var highlightStroke = isTicket.HasValue && isTicket.Value ? "1" : "30";

        string defaultStyle = $"fill:none;stroke:#666;stroke-width:{defaultStroke};stroke-linejoin:round;stroke-dasharray:none;";
        // Estilo em destaque
        string highlightStyle = $"fill:none;stroke:#000000;stroke-width:{highlightStroke};";

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

    static XElement CreateTopWidthText(int rectX, int rectY, int width, int height, int viewBoxWidth, int viewBoxHeight, bool isLandscape)
    {
        // Calculate the center point for rotation
        int centerX = viewBoxWidth / 2;
        int centerY = viewBoxHeight / 2;
        string text = isLandscape ? height.ToString() : width.ToString();

        // Calculate font size based on viewBox height (responsive sizing)
        int fontSize = Math.Max(10, viewBoxHeight / 30); // Minimum 10px, scales with height
        int textMargin = (int)(viewBoxHeight * 0.02); // 2% of viewBox height as margin from rectangle

        int textX, textY;

        if (isLandscape) // -90 degrees rotation
        {
            // Com rotação -90°, o retângulo gira em torno do centro do viewBox
            // O centro do retângulo rotacionado fica no centro de rotação
            textX = centerX; // Centro de rotação = centro do retângulo rotacionado
            textY = centerY - (width / 2) - textMargin; // Acima do retângulo rotacionado
        }
        else // No rotation (0 degrees)
        {
            // Standard positioning for non-rotated rectangle
            textX = rectX + (width / 2); // Center horizontally over the rectangle
            textY = rectY - textMargin; // Position above the rectangle
        }

        var topWidthGroup = new XElement("g",
            new XAttribute("name", "top-width-label")
        );

        // Adjust text alignment based on rotation
        string textAnchor = "middle"; // Sempre centralizado horizontalmente
        string dominantBaseline = "bottom"; // Sempre alinhado na parte inferior (acima do retângulo)

        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", textAnchor),
            new XAttribute("dominant-baseline", dominantBaseline),
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", fontSize),
            new XAttribute("fill", "#666666"),
            text
        );

        topWidthGroup.Add(textElement);
        return topWidthGroup;
    }

    static XElement CreateLeftHeightText(int rectX, int rectY, int width, int height, int viewBoxWidth, int viewBoxHeight, bool isLandscape)
    {
        // Calculate the center point for rotation
        int centerX = viewBoxWidth / 2;
        int centerY = viewBoxHeight / 2;
        string text = isLandscape ? width.ToString() : height.ToString();

        // Calculate font size based on viewBox height (responsive sizing)
        int fontSize = Math.Max(10, viewBoxHeight / 30); // Minimum 10px, scales with height
        int textMargin = (int)(viewBoxHeight * 0.02); // 2% of viewBox height as margin from rectangle

        int textX, textY;
        string transform = "";

        if (isLandscape) // -90 degrees rotation
        {
            // Com rotação -90°, o lado esquerdo do retângulo original fica na parte de baixo
            // O texto deve ficar à esquerda do retângulo visualmente rotacionado
            // Posicionar à esquerda do centro de rotação menos metade da altura original
            textX = centerX - (height / 2) - textMargin; // À esquerda do retângulo rotacionado
            textY = centerY; // Centralizado verticalmente no viewBox

            // Adicionar rotação de 90 graus ao texto para que fique legível na vertical
            transform = $"rotate(-90 {textX} {textY})";
        }
        else // No rotation (0 degrees)
        {
            // Standard positioning for non-rotated rectangle
            textX = rectX - textMargin; // Position to the left of the rectangle
            textY = rectY + (height / 2); // Center vertically along the rectangle height

            // Adicionar rotação de 90 graus ao texto para que fique na vertical
            transform = $"rotate(-90 {textX} {textY})";
        }

        var leftHeightGroup = new XElement("g",
            new XAttribute("name", "left-height-label")
        );

        // Adjust text alignment based on rotation
        string textAnchor = "middle"; // Centralizado para texto rotacionado
        string dominantBaseline = "middle"; // Sempre centralizado verticalmente

        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", textAnchor),
            new XAttribute("dominant-baseline", dominantBaseline),
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", fontSize),
            new XAttribute("fill", "#666666"),
            new XAttribute("transform", transform),
            text
        );

        leftHeightGroup.Add(textElement);
        return leftHeightGroup;
    }

    static XElement CreateLeftC1Label(int viewBoxWidth, int viewBoxHeight)
    {
        // Calculate positioning at 10% of viewBox width from left edge
        int textX = (int)(viewBoxWidth * 0.05);

        // Position vertically centered in the viewBox
        int textY = viewBoxHeight / 2;

        // Calculate font size based on viewBox height
        int fontSize = Math.Max(8, viewBoxHeight / 30); // Minimum 8px, scales with height

        var leftC1Group = new XElement("g",
            new XAttribute("name", "left-c1-label")
        );

        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "start"), // Align text to the left
            new XAttribute("dominant-baseline", "middle"), // Center the text vertically
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", fontSize),
            new XAttribute("fill", "#333333"), // Dark gray color
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

    static XElement CreateRightC2Label(int viewBoxWidth, int viewBoxHeight)
    {
        // Calculate positioning at 80% of viewBox width (20% margin from right)
        int textX = (int)(viewBoxWidth * 0.90);

        // Position vertically centered in the viewBox
        int textY = viewBoxHeight / 2;

        // Calculate font size based on viewBox height
        int fontSize = Math.Max(8, viewBoxHeight / 30); // Minimum 8px, scales with height

        var rightC2Group = new XElement("g",
            new XAttribute("name", "right-c2-label")
        );

        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "middle"), // Center the text horizontally
            new XAttribute("dominant-baseline", "middle"), // Center the text vertically
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", fontSize),
            new XAttribute("fill", "#333333"), // Dark gray color
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

    #region Ticket Print

    static XElement TicketView(SvgRequest svg)
    {
        //quadrado 300 x 300
        //x 54, y 32, w 41, h 20
        int viewBoxX = 49;
        int viewBoxY = 31;
        int viewBoxWidth = 48;
        int viewBoxHeight = 37;

        int originalWidth = Convert.ToInt32(svg.Width);
        int originalHeight = Convert.ToInt32(svg.Height);
        const int originalThickStrokeWidth = 4;
        const int originalPadding = 10;
        const int originalTextPadding = 15;

        // Calculate original SVG dimensions
        int originalSvgWidth = originalWidth + (2 * originalPadding) + originalThickStrokeWidth + originalTextPadding + 30;
        int originalSvgHeight = originalHeight + (2 * originalPadding) + originalThickStrokeWidth + originalTextPadding + 20;

        // Calcular escala para caber no viewBox (mantendo proporção)
        double scaleX = (double)viewBoxWidth / originalSvgWidth;
        double scaleY = (double)viewBoxHeight / originalSvgHeight;
        double baseScale = Math.Min(scaleX, scaleY); // Usar a menor escala para manter proporção
        double scale = baseScale * 0.7; // Diminuir 30% da proporção

        // Aplicar escala em todas as dimensões
        int scaledWidth = (int)(originalWidth * scale);
        int scaledHeight = (int)(originalHeight * scale);

        // Calcular posição para centralizar no viewBox
        // Centralizar apenas o retângulo principal (width x height) no viewBox
        int centerX = viewBoxX + viewBoxWidth / 2;
        int centerY = viewBoxY + viewBoxHeight / 2;

        // Posição do retângulo centralizado
        int rectX = centerX - scaledWidth / 2;
        int rectY = centerY - scaledHeight / 2;

        var rectBox = new ViewBoxData
        {
            Height = scaledHeight,
            Width = scaledWidth,
            X = rectX,
            Y = rectY,
            IsLandscape = svg.IsLandscape
        };

        var svgTeste = new XElement("svg",
            new XAttribute("viewBox", $"{viewBoxX} {viewBoxY} {viewBoxWidth} {viewBoxHeight}"),
            new XAttribute("width", viewBoxWidth),
            new XAttribute("height", viewBoxHeight));

        // Adicionar elementos usando as dimensões e posições escaladas
        svgTeste.Add(BackgroundCut(rectBox, Convert.ToInt32(scale), true));

        #region border

        if (svg.IsAnyBorderValid())
            svgTeste.Add(BorderCut(rectX, rectY, rectX + scaledWidth, rectY + scaledHeight, svg, Convert.ToInt32(scale), true));

        svgTeste.Add(LabelTopWidthTicket(rectX, rectY, rectBox.Width, svg.WidthLabel));
        svgTeste.Add(LabelLeftHeightTicket(rectX, rectY, rectBox.Height, svg.HeightLabel));

        if (svg.IsBorderRight)
            svgTeste.Add(LabelBorderRightTicket(rectX, rectY, rectBox.Height, svg.BorderRightLabel));
        if (svg.IsBorderBottom)
            svgTeste.Add(LabelBorderBottomTicket(rectX, rectY, rectBox.Width, rectBox.Height, viewBoxHeight, svg.BorderBottomLabel));
        if (svg.IsBorderLeft)
            svgTeste.Add(LabelBorderLeftTicket(rectX, rectY, rectBox.Height, svg.BorderLeftLabel));
        if (svg.IsBorderTop)
            svgTeste.Add(LabelBorderTopTicket(rectX, rectY, rectBox.Width, rectBox.Height, viewBoxHeight, svg.BorderTopLabel));

        #endregion 

        //metodo para validar o systemType
        int? dadoThickness = !string.IsNullOrEmpty(svg.Thickness) ? Convert.ToInt32(svg.Thickness) : null;

        if (dadoThickness is not null)
        {
            int? dadoOffsetFromEnd = !string.IsNullOrEmpty(svg.Offset) ? Convert.ToInt32(svg.Offset) : null;
            svgTeste.Add(CreateTicketThickness(scale, scaledWidth, scaledHeight, rectX, rectY, (int)dadoThickness, dadoOffsetFromEnd));
        }





        if (!string.IsNullOrEmpty(svg.JoinSystemType))
        {
            if (svg.JoinSystemType.Equals("minifix"))
            {
                if (svg.Type.Equals("Base"))
                    svgTeste.Add(Minifix.GenerateBase(viewBoxWidth, viewBoxHeight, svg.IsLandscape));

                if (svg.Type.Equals("Side"))
                {
                    //add regra um furo, dois furos
                    svgTeste.Add(Minifix.GenerateSideForTicketView(rectBox, scale));
                }
            }

        }

        svgTeste.Add(LabelFooterTicket(rectX, rectY, rectBox.Width, rectBox.Height, viewBoxHeight));

        return svgTeste;
    }

    #region TicketLegends

    static XElement LabelTopWidthTicket(int rectX, int rectY, int width, string widthLabel)
    {
        int textX, textY;

        // Modo retrato: sem rotação
        textX = rectX + (width / 2); // Centro horizontal do retângulo
        textY = rectY + 3;  // Acima do retângulo com margem

        // Criar o grupo SVG
        var topWidthGroup = new XElement("g", new XAttribute("name", "width-label"));

        // Criar o elemento de texto
        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "middle"),
            new XAttribute("dominant-baseline", "bottom"),
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", "2"),
            new XAttribute("fill", "#666666"),
            widthLabel
        );

        topWidthGroup.Add(textElement);
        return topWidthGroup;
    }

    static XElement LabelLeftHeightTicket(int rectX, int rectY, int height, string heightLabel)
    {
        int textX = rectX - 1;
        double margin = rectY + (height / 2.0);
        string rotate = margin.ToString().Replace(',', '.');

        double viewY = rectY + (height / 2.0) + 3.5;
        string textY = viewY.ToString().Replace(',', '.');

        var leftHeightGroup = new XElement("g", new XAttribute("name", "height-label"));

        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "middle"),
            new XAttribute("dominant-baseline", "middle"),
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", "2"),
            new XAttribute("fill", "#666666"),
            new XAttribute("transform", $"rotate(-90 {textX} {rotate})"),
            heightLabel
        );

        leftHeightGroup.Add(textElement);
        return leftHeightGroup;
    }

    static XElement LabelBorderBottomTicket(int rectX, int rectY, int width, int height, int viewBoxHeight, string borderLabel)
    {
        int textMargin = (int)(viewBoxHeight * 0.02);
        double labelSpacing = viewBoxHeight * 0.068;
        int textX = rectX + (width / 2) + 1;
        int rectBottomY = rectY + height;
        double textY = rectBottomY + textMargin + labelSpacing;

        var bottomL2Group = new XElement("g", new XAttribute("name", "border-bottom-label"));

        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY.ToString().Replace(',', '.')),
            new XAttribute("text-anchor", "middle"),
            new XAttribute("dominant-baseline", "top"),
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", "2"),
            new XAttribute("fill", "#333333"),
            new XAttribute("font-weight", "bold"),
            borderLabel
        );

        bottomL2Group.Add(textElement);
        return bottomL2Group;
    }

    static XElement LabelBorderLeftTicket(int rectX, int rectY, int height, string labelText, int xOffset = 13)
    {
        int textX = rectX - xOffset;

        // Adapta o Y dinamicamente conforme a altura
        int textY = rectY + height + (height < 15 ? 7 : 1);

        double rotateY = rectY + (height / 2.0);

        var leftBorderLabelGroup = new XElement("g", new XAttribute("name", "border-left-label"));

        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "middle"),
            new XAttribute("dominant-baseline", "middle"),
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", "2"),
            new XAttribute("fill", "#333333"),
            new XAttribute("font-weight", "bold"),
            new XAttribute("transform", $"rotate(-90 {textX} {rotateY.ToString().Replace(',', '.')} )"),
            labelText
        );

        leftBorderLabelGroup.Add(textElement);
        return leftBorderLabelGroup;
    }

    static XElement LabelBorderRightTicket(int rectX, int rectY, int height, string labelText, int xOffset = 11)
    {
        int textX = rectX + xOffset; // ligeiramente à direita da borda
        int textY = rectY - (height < 15 ? 13 : 1); // adaptativo

        double rotateY = rectY + (height / 2.0);

        var rightBorderLabelGroup = new XElement("g", new XAttribute("name", "border-right-label"));

        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "middle"),
            new XAttribute("dominant-baseline", "middle"),
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", "2"),
            new XAttribute("fill", "#333333"),
            new XAttribute("font-weight", "bold"),
            new XAttribute("transform", $"rotate(90 {textX} {rotateY.ToString().Replace(',', '.')} )"),
            labelText
        );

        rightBorderLabelGroup.Add(textElement);
        return rightBorderLabelGroup;
    }

    static XElement LabelBorderTopTicket(int rectX, int rectY, int width, int height, int viewBoxHeight, string labelText)
    {
        int textMargin = (int)(viewBoxHeight * 0.02);
        int labelSpacing = (int)(viewBoxHeight * 0.035);

        int textX = rectX + (width / 2) + 1;
        int widthTextY = rectY - textMargin;
        int textY = widthTextY - labelSpacing;

        var topL1Group = new XElement("g", new XAttribute("name", "border-top-label"));

        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY),
            new XAttribute("text-anchor", "middle"),
            new XAttribute("dominant-baseline", "bottom"),
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", "2"),
            new XAttribute("fill", "#333333"),
            new XAttribute("font-weight", "bold"),
            labelText
        );

        topL1Group.Add(textElement);

        return topL1Group;
    }

    static XElement LabelFooterTicket(int rectX, int rectY, int width, int height, int viewBoxHeight)
    {
        // Calculate footer positioning
        int textX = rectX + (width / 2) + 1;

        double labelSpacing = height < 15 ? viewBoxHeight * 0.30 : viewBoxHeight * 0.19;
        int rectBottomY = rectY + height;
        double textY = rectBottomY + labelSpacing;

        var footerGroup = new XElement("g", new XAttribute("name", "footer"));

        var textElement = new XElement("text",
            new XAttribute("x", textX),
            new XAttribute("y", textY.ToString().Replace(',', '.')),
            new XAttribute("text-anchor", "middle"),
            new XAttribute("dominant-baseline", "bottom"),
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", "2"),
            new XAttribute("fill", "#333333"),
            "Face de Alinhamento"
        );

        footerGroup.Add(textElement);

        return footerGroup;
    }

    #endregion

    public static XElement CreateTicketThickness(double scale, double scaledWidth, double scaledHeight,
    int rectX, int rectY, int dadoThickness, int? dadoOffset = 0)
    {
        double scaledThickness = dadoThickness * scale;
        double highlightY;

        // Coordenadas do destaque de espessura
        if (dadoOffset != null)
        {
            double offset = (dadoOffset ?? 0) * scale;
            highlightY = rectY + scaledHeight - scaledThickness - offset;
        }
        else
            highlightY = rectY + scaledHeight - scaledThickness;


        double highlightHeight = scaledThickness;

        string left = (rectX - 2 * scale).ToString().Replace(',', '.');
        string right = (rectX + scaledWidth + 2 * scale).ToString().Replace(',', '.');
        string top = (highlightY).ToString().Replace(',', '.');
        string bottom = (highlightY + highlightHeight).ToString().Replace(',', '.');

        var group = new XElement("g", new XAttribute("name", "thickness"));

        // Adiciona o retângulo principal (parte vermelha)
        group.Add(new XElement("path",
            new XAttribute("d", $"M {left} {top} " +
                               $"L {right} {top} " +
                               $"L {right} {bottom} " +
                               $"L {left} {bottom} Z"),
            new XAttribute("style", "fill:red;fill-opacity:0.5;stroke-linejoin:round;stroke-width:0.1;"),
            new XAttribute("stroke", "black")
        ));

        return group;
    }


    #endregion

}

