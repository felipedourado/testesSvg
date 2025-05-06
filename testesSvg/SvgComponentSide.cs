using System.Globalization;
using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;
using testesSvg.Components;

namespace testesSvg;

public static class SvgComponentSide
{
    public static string GenerateComponent()
    {
        ////PecaCanalCavilhaMinParafuso-- 
        //var payload = new SvgRequest
        //{
        //    Width = "30000",
        //    Height = "30000",
        //    Thickness = "700",
        //    Offset = "1500",
        //    JoinSystemType = "dowels",
        //    Type = "Side"
        //};

        ////PecaCanalCavilhaMinUmFuroParafuso -- Errado a proporcao do centro do furo
        //var payload = new SvgRequest
        //{
        //    Width = "12000",
        //    Height = "12000",
        //    Thickness = "700",
        //    Offset = "1500",
        //    JoinSystemType = "dowels"
        //};

        //PecaCanalCavilhaMaxParafuso --
        var payload = new SvgRequest
        {
            Width = "219000",
            Height = "159000",
            Thickness = "700",
            Offset = "1500",
            JoinSystemType = "dowels",
            Type = "Side"
        };



        string svgXml = GenerateSvgFromJson(payload);
        return svgXml;
    }

    static string GenerateSvgFromJson(SvgRequest json)
    {
        int width = Convert.ToInt32(json.Width);
        int height = Convert.ToInt32(json.Height);

        int? dadoOffsetFromEnd = !string.IsNullOrEmpty(json.Offset) ? Convert.ToInt32(json.Offset) : null;
        int? dadoThickness = !string.IsNullOrEmpty(json.Thickness) ? Convert.ToInt32(json.Thickness) : null;

        int viewBoxX = -width / 20;
        int viewBoxY = -height / 20; //regra para BOARD 15
        int viewBoxWidth = width / 10;
        int viewBoxHeight = height / 10;

        double expandFactor = 1.3; // 30% maior
        int viewBoxX1 = (int)(viewBoxX * expandFactor);
        int viewBoxY1 = (int)(viewBoxY * expandFactor);
        int viewBoxWidth1 = (int)(viewBoxWidth * expandFactor);
        int viewBoxHeight1 = (int)(viewBoxHeight * expandFactor);


        var svg = new XElement("svg",
            new XAttribute("viewBox", $"{viewBoxX1} {viewBoxY1} {viewBoxWidth1} {viewBoxHeight1}"),
            new XAttribute("width", viewBoxWidth),
            new XAttribute("height", viewBoxHeight)
            , Label.CreateLabelGroup(width, height)
            , Label.CreateTopLabelGroup(width, height)
            , CreateBackgroundPath(viewBoxX, viewBoxY)
        //  ,CreateBorderGroup(viewBoxWidth, viewBoxHeight, viewBoxX, viewBoxY, true, false, false, true)
        );

        if (dadoThickness is not null)
        {
            svg.Add(Thickness.CreateThickness(viewBoxWidth, height, viewBoxX, dadoOffsetFromEnd, (int)dadoThickness));
        }

        if (!string.IsNullOrEmpty(json.JoinSystemType))
        {
            if (json.JoinSystemType.Contains("VbOne"))
            {
                if (json.Type.Equals("Base"))
                    svg.Add(VbOne.GenerateBase(viewBoxWidth, viewBoxHeight));
                else
                    svg.Add(VbOne.GenerateSide(viewBoxWidth, viewBoxHeight));
            }
            if (json.JoinSystemType.Contains("VbTwo"))
            {
                if (json.Type.Equals("Base"))
                    svg.Add(VbTwo.GenerateBase(viewBoxWidth, viewBoxHeight));
                else
                    svg.Add(VbTwo.GenerateSide(viewBoxWidth, viewBoxHeight));
            }
            if (json.JoinSystemType.Contains("dowels"))
            {
                svg.Add(Dowels.GenerateSide(viewBoxWidth, viewBoxHeight));
            }

            if (json.JoinSystemType.Contains("minifix"))
            {
                if (json.Type.Equals("Base"))
                    svg.Add(Minifix.GenerateBase(viewBoxWidth, viewBoxHeight));

                if (json.Type.Equals("Side"))
                {
                    svg.Add(Minifix.GenerateSide(viewBoxWidth, viewBoxHeight));
                    //svg.Add(CreateMinifixPaths(viewBoxWidth, viewBoxHeight));
                }

            }
        }

        //CRIAR VALIDACAO DE SIDE, BASE E DOOR
        //if (!string.IsNullOrEmpty(json.JoinSystemType))
        //{
        //  svg.Add(CreateMinifixPaths(viewBoxWidth, viewBoxHeight));
        //}

        //if (!string.IsNullOrEmpty(json.JoinSystemType))
        //{
        //    svg.Add(CreateMinifixBase(viewBoxWidth, viewBoxHeight));
        //}

        //pensar melhor sobre essa regra
        //quando giro preciso redimensionar todos os groups
        //if (request.IsLandscape)
        //{
        //   svg.Add(new XAttribute("transform", "rotate(-90)"));
        //}

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
        //testar peças com rebaixo + cavilha - pulei por causa dos 3 furos
        //testar peças com rebaixo + vb 1 furo -
        //testar peças com rebaixo + vb 2 furos -
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
        //pendente component type base (salvei um exemplo top)
        //avaliar rotação das peças e se afeta furação e usinagem
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

    static IEnumerable<XElement> CreateDowelsPath(int w, int h)
    {
        return new List<XElement>
        {

            CreateDowels(w, h),
         //   MinifixSideRightDown(w, h),
         //   MinifixSideLeftUp(w, h),
            CreateLeftDowel(w, h),
        //    MinifixSideLeftDown(w, h)
        };
    }

    static XElement CreateDowels(double width, double height)
    {
        var group = new XElement("g", new XAttribute("name", "dowels"));

        // Base de posição do Dowel
        double baseX = (width / 2.0) - 35;
        double baseY = 0.0841444417 / height;

        // ----- Primeiro Path -----
        double x1 = baseX;
        double y1 = baseY;

        double x2 = baseX - 5.359;
        double y2 = baseY - 20;

        string d = $"M {x1.ToString("0.####", CultureInfo.InvariantCulture)} {y1.ToString("0.########", CultureInfo.InvariantCulture)} " +
                   $"L {x2.ToString("0.####", CultureInfo.InvariantCulture)} {y2.ToString("0.########", CultureInfo.InvariantCulture)} " +
                   $"L {x2.ToString("0.####", CultureInfo.InvariantCulture)} {y2.ToString("0.########", CultureInfo.InvariantCulture)} " +
                   $"L {x1.ToString("0.####", CultureInfo.InvariantCulture)} {y1.ToString("0.########", CultureInfo.InvariantCulture)} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Segundo Path -----
        double x3 = x2;
        double y3 = y2;

        double x4 = x1 - 20;
        double y4 = y1 - 35;

        string d2 = $"M {x3.ToString("0.####", CultureInfo.InvariantCulture)} {y3.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x4.ToString("0.####", CultureInfo.InvariantCulture)} {y4.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x4.ToString("0.####", CultureInfo.InvariantCulture)} {y4.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x3.ToString("0.####", CultureInfo.InvariantCulture)} {y3.ToString("0.########", CultureInfo.InvariantCulture)} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d2),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Terceiro Path -----
        double x5 = x4;
        double y5 = y4;

        double x6 = x5 - 20;
        double y6 = baseY - 40;

        string d3 = $"M {x5.ToString("0.####", CultureInfo.InvariantCulture)} {y5.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x6.ToString("0.####", CultureInfo.InvariantCulture)} {y6.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x6.ToString("0.####", CultureInfo.InvariantCulture)} {y6.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x5.ToString("0.####", CultureInfo.InvariantCulture)} {y5.ToString("0.########", CultureInfo.InvariantCulture)} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d3),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Quarto Path -----
        double x7 = baseX - 40;          // 3225 - 3205 = 20 de diferença
        double y7 = baseY - 40;          // -40.000004
        double x8 = baseX - 60;          // 3205
        double y8 = baseY - 34.641018;   // -34.641018

        string d4 = $"M {x7.ToString("0.####", CultureInfo.InvariantCulture)} {y7.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x8.ToString("0.####", CultureInfo.InvariantCulture)} {y8.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x8.ToString("0.####", CultureInfo.InvariantCulture)} {y8.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x7.ToString("0.####", CultureInfo.InvariantCulture)} {y7.ToString("0.########", CultureInfo.InvariantCulture)} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d4),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Quinto Path -----
        double x9 = baseX - 60;            // 3205
        double y9 = baseY - 34.641018;

        double x10 = baseX - 74.6409;      // 3190.3591
        double y10 = baseY - 20;           // -19.999996

        string d5 = $"M {x9.ToString("0.####", CultureInfo.InvariantCulture)} {y9.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x10.ToString("0.####", CultureInfo.InvariantCulture)} {y10.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x10.ToString("0.####", CultureInfo.InvariantCulture)} {y10.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x9.ToString("0.####", CultureInfo.InvariantCulture)} {y9.ToString("0.########", CultureInfo.InvariantCulture)} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d5),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Sexto Path -----
        double x11 = baseX - 74.6409;       // 3190.3591
        double y11 = baseY - 20;

        double x12 = baseX - 80;           // 3185
        double y12 = baseY + 0.0000010927848;

        string d6 = $"M {x11.ToString("0.####", CultureInfo.InvariantCulture)} {y11.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x12.ToString("0.####", CultureInfo.InvariantCulture)} {y12.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x12.ToString("0.####", CultureInfo.InvariantCulture)} {y12.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x11.ToString("0.####", CultureInfo.InvariantCulture)} {y11.ToString("0.########", CultureInfo.InvariantCulture)} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d6),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Sétimo Path -----
        double x13 = baseX - 80;             // 3185
        double y13a = baseY * 0.0454522; // 1.0927848E-06
        double y13b = baseY * 0.2454505; // 5.9010376E-06

        double x14 = baseX - 74.6409;        // 3190.3591
        double y14 = baseY + 20.000006;

        string d7 = $"M {x13.ToString("0.####", CultureInfo.InvariantCulture)} {y13a.ToString().Replace(',', '.')} " +
                    $"L {x14.ToString("0.####", CultureInfo.InvariantCulture)} {y14.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x14.ToString("0.####", CultureInfo.InvariantCulture)} {y14.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x13.ToString("0.####", CultureInfo.InvariantCulture)} {y13b.ToString().Replace(',', '.')} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d7),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Oitavo Path -----
        double x15 = baseX - 74.6409;         // 3190.3591
        double y15a = baseY + 20.000006;
        double y15b = baseY + 20.000011;

        double x16 = baseX - 60;              // 3205
        double y16 = baseY + 34.641018;

        string d8 = $"M {x15.ToString("0.####", CultureInfo.InvariantCulture)} {y15a.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x16.ToString("0.####", CultureInfo.InvariantCulture)} {y16.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x16.ToString("0.####", CultureInfo.InvariantCulture)} {y16.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x15.ToString("0.####", CultureInfo.InvariantCulture)} {y15b.ToString("0.########", CultureInfo.InvariantCulture)} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d8),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Nono Path -----
        double x17 = baseX - 60;              // 3205
        double y17a = baseY + 34.641018;
        double y17b = baseY + 34.641020;

        double x18 = baseX - 40;              // 3225
        double y18a = baseY + 39.999996;
        double y18b = baseY + 40.000004;

        string d9 = $"M {x17.ToString("0.####", CultureInfo.InvariantCulture)} {y17a.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x18.ToString("0.####", CultureInfo.InvariantCulture)} {y18a.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x18.ToString("0.####", CultureInfo.InvariantCulture)} {y18b.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x17.ToString("0.####", CultureInfo.InvariantCulture)} {y17b.ToString("0.########", CultureInfo.InvariantCulture)} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d9),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Decimo Path -----
        double x19 = baseX - 40;              // 3225
        double y19a = baseY + 39.999996;
        double y19b = baseY + 40.000004;

        double x20 = baseX - 20;              // 3245
        double y20a = baseY + 34.641006;
        double y20b = baseY + 34.641014;

        string d10 = $"M {x19.ToString("0.####", CultureInfo.InvariantCulture)} {y19a.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x20.ToString("0.####", CultureInfo.InvariantCulture)} {y20a.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x20.ToString("0.####", CultureInfo.InvariantCulture)} {y20b.ToString("0.########", CultureInfo.InvariantCulture)} " +
                    $"L {x19.ToString("0.####", CultureInfo.InvariantCulture)} {y19b.ToString("0.########", CultureInfo.InvariantCulture)} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d10),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Décimo Primeiro Path -----
        double x21 = baseX - 20;              // 3245
        double y21a = baseY + 34.641006;
        double y21b = baseY + 34.641014;

        double x22 = baseX - 5.359;           // 3259.641
        double y22a = baseY * 1.44084E6;
        double y22b = baseY + 19.999994;

        string d11 = $"M {x21.ToString("0.####", CultureInfo.InvariantCulture)} {y21a.ToString("0.########", CultureInfo.InvariantCulture)} " +
                     $"L {x22.ToString("0.####", CultureInfo.InvariantCulture)} {y22b.ToString("0.########", CultureInfo.InvariantCulture)} " +
                     $"L {x22.ToString("0.####", CultureInfo.InvariantCulture)} {y22b.ToString("0.########", CultureInfo.InvariantCulture)} " +
                     $"L {x21.ToString("0.####", CultureInfo.InvariantCulture)} {y22a.ToString("0.########", CultureInfo.InvariantCulture)} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d11),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Décimo Segundo Path -----
        double x23 = baseX - 5.359;             // 3259.641
        double y23a = baseY + 19.999989;
        double y23b = baseY + 19.999994;

        double x24 = baseX;                     // 3265
        double y24a = -(baseY * 0.09997067);

        string d12 = $"M {x23.ToString("0.####", CultureInfo.InvariantCulture)} {y23a.ToString("0.########", CultureInfo.InvariantCulture)} " +
                     $"L {x24.ToString("0.####", CultureInfo.InvariantCulture)} {y24a.ToString().Replace(',', '.')} " +
                     $"L {x24.ToString("0.####", CultureInfo.InvariantCulture)} {y24a.ToString().Replace(',', '.')} " +
                     $"L {x23.ToString("0.####", CultureInfo.InvariantCulture)} {y23b.ToString("0.########", CultureInfo.InvariantCulture)} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d12),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Décimo Segundo Path -----
        var points = new (double X, double Y)[]
{
    (baseX, -0.0000024041262),                    // 3265, -2.4041262E-06
    (baseX - 5.359, -20.000004),                  // 3259.641, -20.000004
    (baseX - 20, -34.64102),                      // 3245, -34.64102
    (baseX - 40, -40.000004),                     // 3225, -40.000004
    (baseX - 60, -34.641018),                     // 3205, -34.641018
    (baseX - 74.6409, -19.999996),                // 3190.3591, -19.999996
    (baseX - 80, 0.0000010927848),                // 3185, 1.0927848E-06
    (baseX - 74.6409, 20.000006),                 // 3190.3591, 20.000006
    (baseX - 60, 34.641018),                      // 3205, 34.641018
    (baseX - 40, 39.999996),                      // 3225, 39.999996
    (baseX - 20, 34.641006),                      // 3245, 34.641006
    (baseX - 5.359, 19.999989)                    // 3259.641, 19.999989
};

        string d13 = "M " + string.Join(" L ", points.Select(p =>
            $"{p.X.ToString("0.####", CultureInfo.InvariantCulture)} {p.Y.ToString("0.########", CultureInfo.InvariantCulture)}"
        )) + " Z";

        group.Add(new XElement("path",
            new XAttribute("d", d13),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));


        return group;
    }

    static XElement CreateLeftDowel(double width, double height)
    {
        var group = new XElement("g", new XAttribute("name", "dowels-left"));
        var ci = CultureInfo.InvariantCulture;

        // Base de posição invertida (lado esquerdo)
        double baseX = -(width * 0.482576);
        double baseY = 0.0000024041262 / height;

        double x1 = baseX;                   // -3185
        //double y1 = -(height * 6.8689332e-10);
        double y1 = baseY;

        double x2 = baseX - 5.3591;          // -3190.3591
        double y2 = baseY - 20.000004;

        double y3 = -19.999998;

        string d = $"M {x1.ToString("0.####", ci)} {y1.ToString().Replace(',', '.')} " +
                   $"L {x2.ToString("0.####", ci)} {y2.ToString("0.########", ci)} " +
                   $"L {x2.ToString("0.####", ci)} {y3.ToString("0.########", ci)} " +
                   $"L {x1.ToString("0.####", ci)} {y1.ToString().Replace(',', '.')} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Segundo Path -----
        double x3 = x2;
        double y4 = y2;

        double deltaX = x2 - x1;
        double x4 = x3 - (14.6409 / Math.Abs(deltaX)) * Math.Abs(deltaX);
        double y5 = y4 - 14.641016;
        double y6 = y5 + 0.000002;
        double y7 = y3;

        string d2 = $"M {x3.ToString("0.####", ci)} {y4.ToString("0.########", ci)} " +
                    $"L {x4.ToString("0.####", ci)} {y5.ToString("0.########", ci)} " +
                    $"L {x4.ToString("0.####", ci)} {y6.ToString("0.########", ci)} " +
                    $"L {x3.ToString("0.####", ci)} {y7.ToString("0.########", ci)} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d2),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Terceiro Path -----
        double x5 = x4;                         // continua do segundo path
        double y8 = y5;                         // continua do segundo path
        double x6 = x4 - 20;                    // movimento para a esquerda
        double y9 = y5 - 4.641018;             // descida vertical similar ao original

        string d3 = $"M {x5.ToString("0.####", ci)} {y8.ToString("0.########", ci)} " +
                    $"L {x6.ToString("0.####", ci)} {y9.ToString("0.########", ci)} " +
                    $"L {x6.ToString("0.####", ci)} {y9.ToString("0.########", ci)} " + // fechamento da linha
                    $"L {x5.ToString("0.####", ci)} {y8.ToString("0.########", ci)} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d3),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Quarto Path -----
        double x7 = x6;                   // -3225
        double x8 = x7 - 20;              // -3245

        double y10 = baseY - 40;                // baseY é um valor próximo a 0, então y10 ≈ -40
        double y11 = y10 + 5.359;               // desloca para cima, aproximando de -34.641...

        string d4 = $"M {x7.ToString("0.####", ci)} {y10.ToString("0.########", ci)} " +
                    $"L {x8.ToString("0.####", ci)} {y11.ToString("0.########", ci)} " +
                    $"L {x8.ToString("0.####", ci)} {y11.ToString("0.########", ci).Replace("41018", "41014")} " +
                    $"L {x7.ToString("0.####", ci)} {y10.ToString("0.########", ci).Replace("000004", "999996")} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d4),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));



        // ----- Quinto Path -----
        double x9 = x8 - 14.6409;            // x8 é -3245 → -3259.641
        double y12 = baseY - 20;             // baseY próximo a zero → -19.999996
        double y13 = y12 + 0.000006;         // -19.99999

        string d5 = $"M {x8.ToString("0.####", ci)} {y11.ToString("0.########", ci)} " +
                    $"L {x9.ToString("0.####", ci)} {y12.ToString("0.########", ci)} " +
                    $"L {x9.ToString("0.####", ci)} {y13.ToString("0.########", ci)} " +
                    $"L {x8.ToString("0.####", ci)} {y11.ToString("0.########", ci).Replace("41018", "41014")} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d5),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Sexto Path -----
        double x10 = x9 - 5.3591;            // -3265
        double y14 = baseY + 0.0000010927848; // 1.0927848E-06
        double y15 = baseY * 0.2454505;       // 5.9010376E-06

        string d6 = $"M {x9.ToString("0.####", ci)} {y12.ToString("0.########", ci)} " +
                    $"L {x10.ToString("0.####", ci)} {y14.ToString().Replace(',', '.')} " +
                    $"L {x10.ToString("0.####", ci)} {y15.ToString().Replace(',', '.')} " +
                    $"L {x9.ToString("0.####", ci)} {y12.ToString("0.########", ci)} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d6),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new
            XAttribute("stroke", "black")
        ));

        // ----- Sétimo Path -----
        double y16 = baseY + 20.000006;     // ~20.000006
        double y17 = y16 + 0.000005;         // ~20.000011

        string d7 = $"M {x10.ToString("0.####", ci)} {y14.ToString().Replace(',', '.')} " +
                    $"L {x9.ToString("0.####", ci)} {y16.ToString("0.########", ci)} " +
                    $"L {x9.ToString("0.####", ci)} {y17.ToString("0.########", ci)} " +
                    $"L {x10.ToString("0.####", ci)} {y15.ToString().Replace(',', '.')} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d7),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Oitavo Path -----
        double x11 = x9 + 14.6409;           // -3245
        double y18 = baseY + 34.641018;
        double y19 = y18 + 0.000002;

        string d8 = $"M {x9.ToString("0.####", ci)} {y16.ToString("0.########", ci)} " +
                    $"L {x11.ToString("0.####", ci)} {y18.ToString("0.########", ci)} " +
                    $"L {x11.ToString("0.####", ci)} {y19.ToString("0.########", ci)} " +
                    $"L {x9.ToString("0.####", ci)} {y17.ToString("0.########", ci)} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d8),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Nono Path -----
        double x12 = x11 + 20;              // -3225
        double y20 = baseY + 39.999996;
        double y21 = y20 + 0.000008;

        string d9 = $"M {x11.ToString("0.####", ci)} {y18.ToString("0.########", ci)} " +
                    $"L {x12.ToString("0.####", ci)} {y20.ToString("0.########", ci)} " +
                    $"L {x12.ToString("0.####", ci)} {y21.ToString("0.########", ci)} " +
                    $"L {x11.ToString("0.####", ci)} {y19.ToString("0.########", ci)} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d9),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Décimo Path -----
        double x13 = x12 + 20;              // -3205
        double y22 = baseY + 34.641006;
        double y23 = y22 + 0.000008;

        string d10 = $"M {x12.ToString("0.####", ci)} {y20.ToString("0.########", ci)} " +
                     $"L {x13.ToString("0.####", ci)} {y22.ToString("0.########", ci)} " +
                     $"L {x13.ToString("0.####", ci)} {y23.ToString("0.########", ci)} " +
                     $"L {x12.ToString("0.####", ci)} {y21.ToString("0.########", ci)} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d10),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Décimo Primeiro Path -----
        double x14 = x13 + 14.6409;         // -3190.3591
        double y24 = baseY + 19.999989;
        double y25 = y24 + 0.000005;

        string d11 = $"M {x13.ToString("0.####", ci)} {y22.ToString("0.########", ci)} " +
                     $"L {x14.ToString("0.####", ci)} {y24.ToString("0.########", ci)} " +
                     $"L {x14.ToString("0.####", ci)} {y25.ToString("0.########", ci)} " +
                     $"L {x13.ToString("0.####", ci)} {y23.ToString("0.########", ci)} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d11),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Décimo Segundo Path -----
        double x15 = x14 + 5.3591;          // -3185
        double y26 = -baseY;                 // -2.4041262E-06
        double y27 = baseY;                  //  2.4041262E-06

        string d12 = $"M {x14.ToString("0.####", ci)} {y24.ToString("0.########", ci)} " +
                     $"L {x15.ToString("0.####", ci)} {y26.ToString("0.########", ci)} " +
                     $"L {x15.ToString("0.####", ci)} {y27.ToString("0.########", ci)} " +
                     $"L {x14.ToString("0.####", ci)} {y25.ToString("0.########", ci)} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d12),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        // ----- Décimo Terceiro Path -----
        var points = new (double X, double Y)[]
        {
    (x1, 2.4041262E-06),          // -3185, 2.4041262E-06
    (x2, -19.999998),             // -3190.3591, -19.999998
   (x4, -34.641018),             // -3205, -34.641018
    (x6, -39.999996),             // -3225, -39.999996
    (x8, -34.641014),             // -3245, -34.641014
    (x9, -19.99999),              // -3259.641, -19.99999
    (x10, 5.9010376E-06),          // -3265, 5.9010376E-06
    (x9, 20.000011),              // -3259.641, 20.000011
    (x8, 34.64102),               // -3245, 34.64102
    (x6, 40.000004),             // -3225, 40.000004
    (x4, 34.641014),             // -3205, 34.641014
    (x2, 19.999994)              // -3190.3591, 19.999994
        };

        string d13 = "M " + string.Join(" L ", points.Select(p =>
            $"{p.X.ToString("0.####", CultureInfo.InvariantCulture)} {p.Y.ToString(CultureInfo.InvariantCulture)}"
        )) + " Z";

        group.Add(new XElement("path",
            new XAttribute("d", d13),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));

        return group;
    }


    static XElement GenericDouble(double x1, double x2, double y1, double y2)
    {
        string d12 = $"M {x1.ToString("0.####", CultureInfo.InvariantCulture)} {y1.ToString("0.####", CultureInfo.InvariantCulture)} " +
                   $"L {x2.ToString("0.####", CultureInfo.InvariantCulture)} {y2.ToString("0.####", CultureInfo.InvariantCulture)} " +
                   $"L {x2.ToString("0.####", CultureInfo.InvariantCulture)} {y2.ToString("0.####", CultureInfo.InvariantCulture)} " +
                   $"L {x1.ToString("0.####", CultureInfo.InvariantCulture)} {y1.ToString("0.####", CultureInfo.InvariantCulture)} Z";

        return (new XElement("path",
            new XAttribute("d", d12),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));
    }


}