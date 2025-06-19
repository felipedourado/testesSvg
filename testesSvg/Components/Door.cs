using System.Globalization;
using System.Xml.Linq;

namespace testesSvg.Components;

public static class Door
{
    //DobradicaMin DOIS FUROS-- ok
    //var payload = new SvgRequest
    //{
    //    Width = "20000",
    //    Height = "30000",
    //    JoinSystemType = "door",
    //    HingeSku = "hinge"
    //};

    //DobradicaMax DOIS FUROS-- ok
    //var payload = new SvgRequest
    //{
    //    Width = "50000",
    //    Height = "90000",
    //    JoinSystemType = "door",
    //    HingeSku = "hinge"
    //};

    //DobradicaMin TRES FUROS-- ok
    //var payload = new SvgRequest
    //{
    //    Width = "20000",
    //    Height = "90001",
    //    JoinSystemType = "door",
    //    HingeSku = "hinge"
    //};

    //DobradicaMax TRES FUROS-- ok
    //var payload = new SvgRequest
    //{
    //    Width = "90000",
    //    Height = "160000",
    //    JoinSystemType = "door",
    //    HingeSku = "hinge"
    //};

    //DobradicaMin QUATRO FUROS-- ok
    //var payload = new SvgRequest
    //{
    //    Width = "20000",
    //    Height = "160001",
    //    JoinSystemType = "door",
    //    HingeSku = "hinge"
    //};

    //DobradicaMax QUATRO FUROS-- ok
    //var payload = new SvgRequest
    //{
    //    Width = "100000",
    //    Height = "200000",
    //    JoinSystemType = "door",
    //    HingeSku = "hinge"
    //};

    //DobradicaMin CINCO FUROS-- ok
    //var payload = new SvgRequest
    //{
    //    Width = "20000",
    //    Height = "200001",
    //    JoinSystemType = "door",
    //    HingeSku = "hinge"
    //};

    //DobradicaMax CINCO FUROS-- ok
    //var payload = new SvgRequest
    //{
    //    Width = "100000",
    //    Height = "270000",
    //    JoinSystemType = "door",
    //    HingeSku = "hinge"
    //};


    public static IEnumerable<XElement> Generate(int width, int height, string doorItens, bool isLandscape)
    {
        if (height <= 9000 && string.IsNullOrEmpty(doorItens))
            return Double(width, height, isLandscape);

        if (doorItens.Equals("triple"))
            return Triple(width, height, isLandscape);

        if (doorItens.Equals("quad"))
            return Quad(width, height, isLandscape);


        var bigX1 = CalculateBigCircleX1Quin(width);
        var bigY1 = CalculateBigCircleY1(height);
        var bigY2 = CalculateBigCircleY2Quin(height);
        var bigY3 = CalculateBigCircleY3Quin();
        var bigY4 = CalculateBigCircleY4Quin(height);
        var bigY5 = CalculateBigCircleY5Quin(height);

        var x = CalculateSmallCircleX(width);
        var y1 = CalculateSmallCircleY1(height);
        var y2 = CalculateSmallCircleY2(height);
        var y3 = CalculateSmallCircleY7Quin(height);
        var y4 = CalculateSmallCircleY8Quin(height);
        var y1Quin = CalculateSmallCircleY3Triple();
        var y2Quin = CalculateSmallCircleY4Triple();
        var y3Quin = CalculateSmallCircleY10Quin(height);
        var y4Quin = CalculateSmallCircleY9Quin(height);
        var y5Quin = CalculateSmallCircleY11Quin(height);
        var y6Quin = CalculateSmallCircleY12Quin(height);

        return
            [
               Side([.. x], [.. y1], "door-small-circle-1", isLandscape),
               Side([.. x], [.. y2], "door-small-circle-2", isLandscape),
               Side([.. x], [.. y3], "door-small-circle-3", isLandscape),
               Side([.. x], [.. y4], "door-small-circle-4", isLandscape),
               Side([.. bigX1], [.. bigY1], "door-big-circle-1", isLandscape),
               Side([.. bigX1], [.. bigY2], "door-big-circle-2", isLandscape),
               BaseQuinPath([.. bigX1], [.. bigY3], "door-big-circle-3", isLandscape),
               Side([.. bigX1], [.. bigY4], "door-big-circle-4", isLandscape),
               Side([.. x], [.. y1Quin], "door-small-circle-4", isLandscape),
               Side([.. x], [.. y2Quin], "door-small-circle-4", isLandscape),
               Side([.. x], [.. y3Quin], "door-small-circle-4", isLandscape),
               Side([.. x], [.. y4Quin], "door-small-circle-4", isLandscape),
               Side([.. bigX1], [.. bigY5], "door-big-circle-5", isLandscape),
               Side([.. x], [.. y5Quin], "door-small-circle-5", isLandscape),
               Side([.. x], [.. y6Quin], "door-small-circle-6", isLandscape),
            ];
    }

    static List<double> CalculateSmallCircleX(int width)
    {
        return new List<double>
            {
              -0.5 * width + 260.5,
              -0.5 * width + 257.0836,
              -0.5 * width + 247.75,
              -0.5 * width + 235,
              -0.5 * width + 222.25,
              -0.5 * width + 212.9163,
              -0.5 * width + 209.5
            };
    }

    static List<double> CalculateSmallCircleY1(int height)
    {
        return new List<double>
            {
              -0.5 * height + 720,
              -0.5 * height + 707.25,
              -0.5 * height + 697.9163,
              -0.5 * height + 694.5,
              -0.5 * height + 732.75,
              -0.5 * height + 742.0836,
              -0.5 * height + 745.5
            };
    }

    static List<double> CalculateSmallCircleY2(int height)
    {
        return new List<double>
            {
              -0.5 * height + 1200,
              -0.5 * height + 1187.25,
              -0.5 * height + 1177.91635,
              -0.5 * height + 1174.5,
              -0.5 * height + 1212.75,
              -0.5 * height + 1222.08365,
              -0.5 * height + 1225.5
            };
    }

    static List<double> CalculateSmallCircleY3(int height)
    {
        return new List<double>
            {
              0.5 * height - 1200,
              0.5 * height - 1212.75,
              0.5 * height - 1222.08365,
              0.5 * height - 1225.5,
              0.5 * height - 1187.25,
              0.5 * height - 1177.91635,
              0.5 * height - 1174.5
            };
    }

    static List<double> CalculateSmallCircleY4(int height)
    {
        return new List<double>
            {
              0.5 * height - 720,
              0.5 * height - 732.75,
              0.5 * height - 742.08365,
              0.5 * height - 745.5,
              0.5 * height - 707.25,
              0.5 * height - 697.9163,
              0.5 * height - 694.5
            };
    }

    static List<double> CalculateSmallCircleY3Triple()
    {
        return new List<double>
            {
              -240,
              -252.75,
              -262.08365,
              -265.5,
              -227.25,
              -217.91637,
              -214.5
            };
    }

    static List<double> CalculateSmallCircleY4Triple()
    {
        return new List<double>
            {
              240,
              227.25,
              217.91637,
              214.5,
              252.75,
              262.08365,
              265.5
            };
    }

    static List<double> CalculateSmallCircleY3Quad(int height)
    {
        return new List<double>
            {
              -0.166675 * height - 239.9,
              -0.166675 * height - 252.65,
              -0.166675 * height - 261.9837,
              -0.166675 * height - 265.4,
              -0.166675 * height - 227.15,
              -0.166675 * height - 217.8165,
              -0.166675 * height - 214.4
            };
    }

    static List<double> CalculateSmallCircleY4Quad(int height)
    {
        return new List<double>
            {
              -0.166675 * height + 240.1,
              -0.166675 * height + 227.35,
              -0.166675 * height + 218.0163,
              -0.166675 * height + 214.6,
              -0.166675 * height + 252.85,
              -0.166675 * height + 262.1835,
              -0.166675 * height + 265.6
            };
    }

    static List<double> CalculateSmallCircleY5Quad(int height)
    {
        return new List<double>
            {
              0.16665 * height - 239.7,
              0.16665 * height - 252.45,
              0.16665 * height - 261.7835,
              0.16665 * height - 265.2,
              0.16665 * height - 226.95,
              0.16665 * height - 217.6163,
              0.16665 * height - 214.2
            };
    }

    static List<double> CalculateSmallCircleY6Quad(int height)
    {
        return new List<double>
            {
              0.16665 * height + 240.3,
              0.16665 * height + 227.55,
              0.16665 * height + 218.2165,
              0.16665 * height + 214.8,
              0.16665 * height + 253.05,
              0.16665 * height + 262.3837,
              0.16665 * height + 265.8
            };
    }

    static List<double> CalculateSmallCircleY7Quin(int height)
    {
        return new List<double>
            {
              -0.25 * height - 240,
              -0.25 * height - 252.75,
              -0.25 * height - 262.0835,
              -0.25 * height - 265.5,
              -0.25 * height - 227.25,
              -0.25 * height - 217.9165,
              -0.25 * height - 214.5
            };
    }

    static List<double> CalculateSmallCircleY8Quin(int height)
    {
        return new List<double>
            {
              -0.25 * height + 240,
              -0.25 * height + 227.25,
              -0.25 * height + 217.9165,
              -0.25 * height + 214.5,
              -0.25 * height + 252.75,
              -0.25 * height + 262.0835,
              -0.25 * height + 265.5
            };
    }

    static List<double> CalculateSmallCircleY9Quin(int height)
    {
        return new List<double>
            {
              0.25 * height + 240,
              0.25 * height + 227.25,
              0.25 * height + 217.9165,
              0.25 * height + 214.5,
              0.25 * height + 252.75,
              0.25 * height + 262.0835,
              0.25 * height + 265.5
            };
    }

    static List<double> CalculateSmallCircleY10Quin(int height)
    {
        return new List<double>
            {
              0.25* height - 240,
              0.25* height - 252.75,
              0.25* height - 262.0835,
              0.25* height - 265.5,
              0.25* height - 227.25,
              0.25* height - 217.9165,
              0.25* height - 214.5
            };
    }

    static List<double> CalculateSmallCircleY11Quin(int height)
    {
        return new List<double>
            {
              0.5 * height - 1199.611,
              0.5 * height - 1212.361,
              0.5 * height - 1221.697,
              0.5 * height - 1225.111,
              0.5 * height - 1186.861,
              0.5 * height - 1177.53,
              0.5 * height - 1174.111
            };
    }

    static List<double> CalculateSmallCircleY12Quin(int height)
    {
        return new List<double>
            {
              0.5 * height - 719.611,
              0.5 * height - 732.361,
              0.5 * height - 741.697,
              0.5 * height - 745.111,
              0.5 * height - 706.861,
              0.5 * height - 697.53,
              0.5 * height - 694.111
            };
    }


    static List<double> CalculateBigCircleX1(int width)
    {
        var ok = new List<double>
            {
              -0.5 * width + 350,
              -0.5 * width + 326.55444,
              -0.5 * width + 262.5,
              -0.5 * width + 175,
              -0.5 * width + 87.5,
              -0.5 * width + 23.4455,
              -0.5 * width
            };
        return ok;
    }

    static List<double> CalculateBigCircleY1(int height)
    {
        return new List<double>
            {
              -0.5 * height + 960, //-3540
              -0.5 * height + 872.5, //3627.5
              -0.5 * height + 808.44556, //3691.5544
              -0.5 * height + 785,//3715
              -0.5 * height + 1047.50003, //3452.5
              -0.5 * height + 1111.55444, //3388
              -0.5 * height + 1135 //3365
            };
    }

    static List<double> CalculateBigCircleY2(int height)
    {
        return new List<double>
            {
              0.5 * height - 960, //-3540
              0.5 * height - 1047.5, //3627.5
              0.5 * height - 1111.55444, //3691.5544
              0.5 * height - 1135,//3715
              0.5 * height - 872.49994, //3452.5
              0.5 * height - 808.4456, //3388
              0.5 * height - 785 //3365
            };
    }

    static List<double> CalculateBigCircleTriple()
    {
        return new List<double>
            {
              -2.8412403E-06,
              -87.5,
              -151.55446,
              -175,
              -151.55444,
              -87.49997,
              1.2457746E-05,
              1.8140227E-05,
              87.50003,
              151.55446,
              151.55441,
              87.49996,
              2.8412403E-06,
              175
            };
    }

    static List<double> CalculateBigCircleY3Quad(int height)
    {
        return new List<double>
            {
              -0.166675 * height + 0.1,
              -0.166675 * height - 87.4,
              -0.166675 * height - 151.4546,
              -0.166675 * height - 174.9,
              -0.166675 * height + 87.6,
              -0.166675 * height + 151.6545,
              -0.166675 * height + 175.1
            };
    }

    static List<double> CalculateBigCircleY4Quad(int height)
    {
        return new List<double>
            {
              0.16665 * height + 0.3,
              0.16665 * height - 87.2,
              0.16665 * height - 151.2545,
              0.16665 * height - 174.7,
              0.16665 * height + 87.8,
              0.16665 * height + 151.8546,
              0.16665 * height + 175.3
            };
    }

    static List<double> CalculateBigCircleX1Quin(int width)
    {
        return new List<double>
            {
              -0.494375 * width + 338.75,
              -0.494375 * width + 315.3044,
              -0.494375 * width + 251.25,
              -0.494375 * width + 163.75,
              -0.494375 * width + 76.25,
              -0.494375 * width + 12.1955,
              -0.494375 * width - 11.25
            };
    }

    static List<double> CalculateBigCircleY2Quin(int height)
    {
        return new List<double>
            {
              -0.25 * height,
              -0.25 * height - 87.5,
              -0.25 * height - 151.554,
              -0.25 * height - 175,
              -0.25 * height + 87.5,
              -0.25 * height + 151.554,
              -0.25 * height + 175
            };
    }

    static List<double> CalculateBigCircleY3Quin()
    {
        return new List<double>
            {
              -2.8412403E-06,
              -87.5,
              -151.55446,
              -175,
              -151.55444,
              -87.49997,
              1.2457746E-05,
              1.8140227E-05,
              87.50003,
              151.55446,
              175,
              151.55441,
              87.49996,
              2.8412403E-06
            };
    }

    static List<double> CalculateBigCircleY4Quin(int height)
    {
        return new List<double>
            {
              0.25 * height,
              0.25 * height - 87.5,
              0.25 * height - 151.554,
              0.25 * height - 175,
              0.25 * height + 87.5,
              0.25 * height + 151.554,
              0.25 * height + 175
            };
    }

    static List<double> CalculateBigCircleY5Quin(int height)
    {
        return new List<double>
            {
              0.5 * height - 959.611,
              0.5 * height - 1047.111,
              0.5 * height - 1111.166,
              0.5 * height - 1134.611,
              0.5 * height - 872.111,
              0.5 * height - 808.059,
              0.5 * height - 784.611
            };
    }


    static IEnumerable<XElement> Double(int width, int height, bool isLandscape)
    {
        var x = CalculateSmallCircleX(width);
        var y1 = CalculateSmallCircleY1(height);
        var y2 = CalculateSmallCircleY2(height);
        var y3 = CalculateSmallCircleY3(height);
        var y4 = CalculateSmallCircleY4(height);

        var bigX1 = CalculateBigCircleX1(width);
        var bigY1 = CalculateBigCircleY1(height);
        var bigY2 = CalculateBigCircleY2(height);

        return
            [
               Side([.. x], [.. y1], "door-small-circle-1", isLandscape),
               Side([.. x], [.. y2], "door-small-circle-2", isLandscape),
               Side([.. x], [.. y3], "door-small-circle-3", isLandscape),
               Side([.. x], [.. y4], "door-small-circle-4", isLandscape),
               Side([.. bigX1], [.. bigY1], "door-big-circle-1", isLandscape),
               Side([.. bigX1], [.. bigY2], "door-big-circle-2", isLandscape),
            ];
    }

    static IEnumerable<XElement> Triple(int width, int height, bool isLandscape)
    {
        var x = CalculateSmallCircleX(width);
        var y1 = CalculateSmallCircleY1(height);
        var y2 = CalculateSmallCircleY2(height);
        var y3 = CalculateSmallCircleY3(height);
        var y4 = CalculateSmallCircleY4(height);

        var bigX1 = CalculateBigCircleX1(width);
        var bigY1 = CalculateBigCircleY1(height);
        var bigY2 = CalculateBigCircleY2(height);
        var bigY3 = CalculateBigCircleTriple();

        var y1Triple = CalculateSmallCircleY3Triple();
        var y2Triple = CalculateSmallCircleY4Triple();

        return
            [
               Side([.. x], [.. y1], "door-small-circle-1", isLandscape),
               Side([.. x], [.. y2], "door-small-circle-2", isLandscape),
               Side([.. x], [.. y3], "door-small-circle-3", isLandscape),
               Side([.. x], [.. y4], "door-small-circle-4", isLandscape),
               Side([.. bigX1], [.. bigY1], "door-big-circle-1", isLandscape),
               Side([.. bigX1], [.. bigY2], "door-big-circle-2", isLandscape),
               BaseSinglePath([.. bigX1], [.. bigY3], "door-big-circle-3", isLandscape),
               Side([.. x], [.. y1Triple], "door-small-circle-4", isLandscape),
               Side([.. x], [.. y2Triple], "door-small-circle-4", isLandscape),
            ];
    }

    static IEnumerable<XElement> Quad(int width, int height, bool isLandscape)
    {
        var x = CalculateSmallCircleX(width);
        var y1 = CalculateSmallCircleY1(height);
        var y2 = CalculateSmallCircleY2(height);
        var y3 = CalculateSmallCircleY3(height);
        var y4 = CalculateSmallCircleY4(height);

        var bigX1 = CalculateBigCircleX1(width);
        var bigY1 = CalculateBigCircleY1(height);
        var bigY2 = CalculateBigCircleY2(height);
        var bigY3 = CalculateBigCircleY3Quad(height);
        var bigY4 = CalculateBigCircleY4Quad(height);

        var y1Quad = CalculateSmallCircleY3Quad(height);
        var y2Quad = CalculateSmallCircleY4Quad(height);
        var y3Quad = CalculateSmallCircleY5Quad(height);
        var y4Quad = CalculateSmallCircleY6Quad(height);

        return
            [
               Side([.. x], [.. y1], "door-small-circle-1", isLandscape),
               Side([.. x], [.. y2], "door-small-circle-2", isLandscape),
               Side([.. x], [.. y3], "door-small-circle-3", isLandscape),
               Side([.. x], [.. y4], "door-small-circle-4", isLandscape),
               Side([.. bigX1], [.. bigY1], "door-big-circle-1", isLandscape),
               Side([.. bigX1], [.. bigY2], "door-big-circle-2", isLandscape),
               Side([.. bigX1], [.. bigY3], "door-big-circle-3", isLandscape),
               Side([.. bigX1], [.. bigY4], "door-big-circle-4", isLandscape),
               Side([.. x], [.. y1Quad], "door-small-circle-4", isLandscape),
               Side([.. x], [.. y2Quad], "door-small-circle-4", isLandscape),
               Side([.. x], [.. y3Quad], "door-small-circle-4", isLandscape),
               Side([.. x], [.. y4Quad], "door-small-circle-4", isLandscape),
            ];
    }



    static XElement Side(double[] x1, double[] y1, string name, bool isLandscape)
    {

        var group = new XElement("g", new XAttribute("name", name), isLandscape ? new XAttribute("transform", "rotate(-90)") : null);

        AddMinifixBaseQuadPath(group, x1[0], y1[0], x1[1], y1[1], x1[1], y1[1], x1[0], y1[0]);
        AddMinifixBaseQuadPath(group, x1[1], y1[1], x1[2], y1[2], x1[2], y1[2], x1[1], y1[1]);
        AddMinifixBaseQuadPath(group, x1[2], y1[2], x1[3], y1[3], x1[3], y1[3], x1[2], y1[2]);
        AddMinifixBaseQuadPath(group, x1[3], y1[3], x1[4], y1[2], x1[4], y1[2], x1[3], y1[3]);
        AddMinifixBaseQuadPath(group, x1[4], y1[2], x1[5], y1[1], x1[5], y1[1], x1[4], y1[2]);
        AddMinifixBaseQuadPath(group, x1[5], y1[1], x1[6], y1[0], x1[6], y1[0], x1[5], y1[1]);

        AddMinifixBaseQuadPath(group, x1[6], y1[0], x1[5], y1[4], x1[5], y1[4], x1[6], y1[0]);
        AddMinifixBaseQuadPath(group, x1[5], y1[4], x1[4], y1[5], x1[4], y1[5], x1[5], y1[4]);
        AddMinifixBaseQuadPath(group, x1[4], y1[5], x1[3], y1[6], x1[3], y1[6], x1[4], y1[5]);
        AddMinifixBaseQuadPath(group, x1[3], y1[6], x1[2], y1[5], x1[2], y1[5], x1[3], y1[6]);
        AddMinifixBaseQuadPath(group, x1[2], y1[5], x1[1], y1[4], x1[1], y1[4], x1[2], y1[5]);
        AddMinifixBaseQuadPath(group, x1[1], y1[4], x1[0], y1[0], x1[0], y1[0], x1[1], y1[4]);


        // ----- Décimo Terceiro Path -----
        var points = new[]
        {
                (x1[0], y1[0]),
                (x1[1], y1[1]),
                (x1[2], y1[2]),
                (x1[3], y1[3]),
                (x1[4], y1[2]),
                (x1[5], y1[1]),
                (x1[6], y1[0]),
                (x1[5], y1[4]),
                (x1[4], y1[5]),
                (x1[3], y1[6]),
                (x1[2], y1[5]),
                (x1[1], y1[4])
            };

        AddCircleColor(group, points);

        // ----- Décimo Quarto Path -----
        AddCircleColor(group, points);
        return group;
    }

    public static XElement BaseSinglePath(double[] x1, double[] y1, string name, bool isLandscape)
    {

        var group = new XElement("g", new XAttribute("name", name), isLandscape ? new XAttribute("transform", "rotate(-90)") : null);

        AddSinglePath(group, x1[0], y1[0], x1[1], y1[1], x1[1], y1[1], x1[0], y1[12]);
        AddSinglePath(group, x1[1], y1[1], x1[2], y1[2], x1[2], y1[2], x1[1], y1[1]);
        AddSinglePath(group, x1[2], y1[2], x1[3], y1[3], x1[3], y1[3], x1[2], y1[2]);
        AddSinglePath(group, x1[3], y1[3], x1[4], y1[4], x1[4], y1[4], x1[3], y1[3]);
        AddSinglePath(group, x1[4], y1[4], x1[5], y1[5], x1[5], y1[5], x1[4], y1[4]);
        AddSinglePath(group, x1[5], y1[5], x1[6], y1[6], x1[6], y1[7], x1[5], y1[5]);
        AddSinglePath(group, x1[6], y1[6], x1[5], y1[8], x1[5], y1[8], x1[6], y1[7]);
        AddSinglePath(group, x1[5], y1[8], x1[4], y1[9], x1[4], y1[9], x1[5], y1[8]);
        AddSinglePath(group, x1[4], y1[9], x1[3], y1[13], x1[3], y1[13], x1[4], y1[9]);
        AddSinglePath(group, x1[3], y1[13], x1[2], y1[10], x1[2], y1[10], x1[3], y1[13]);
        AddSinglePath(group, x1[2], y1[10], x1[1], y1[11], x1[1], y1[11], x1[2], y1[10]);
        AddSinglePath(group, x1[1], y1[11], x1[0], y1[0], x1[0], y1[12], x1[1], y1[11]);

        var points = new[]
        {
            (x1[0], y1[0]),
            (x1[1], y1[1]),
            (x1[2], y1[2]),
            (x1[3], y1[3]),
            (x1[4], y1[4]),
            (x1[5], y1[5]),
            (x1[6], y1[6]),
            (x1[5], y1[8]),
            (x1[4], y1[9]),
            (x1[3], y1[13]),
            (x1[2], y1[10]),
            (x1[1], y1[11])
        };

        AddCircleColor(group, points);
        AddCircleColor(group, points);

        return group;
    }

    public static XElement BaseQuinPath(double[] x1, double[] y1, string name, bool isLandscape)
    {

        var group = new XElement("g", new XAttribute("name", name), isLandscape ? new XAttribute("transform", "rotate(-90)") : null);

        AddSinglePath(group, x1[0], y1[0], x1[1], y1[1], x1[1], y1[1], x1[0], y1[13]);
        AddSinglePath(group, x1[1], y1[1], x1[2], y1[2], x1[2], y1[2], x1[1], y1[1]);
        AddSinglePath(group, x1[2], y1[2], x1[3], y1[3], x1[3], y1[3], x1[2], y1[2]);
        AddSinglePath(group, x1[3], y1[3], x1[4], y1[4], x1[4], y1[4], x1[3], y1[3]);
        AddSinglePath(group, x1[4], y1[4], x1[5], y1[5], x1[5], y1[5], x1[4], y1[4]);
        AddSinglePath(group, x1[5], y1[5], x1[6], y1[6], x1[6], y1[7], x1[5], y1[5]);
        AddSinglePath(group, x1[6], y1[6], x1[5], y1[8], x1[5], y1[8], x1[6], y1[7]);
        AddSinglePath(group, x1[5], y1[8], x1[4], y1[9], x1[4], y1[9], x1[5], y1[8]);

        AddSinglePath(group, x1[4], y1[9], x1[3], y1[10], x1[3], y1[10], x1[4], y1[9]);

        AddSinglePath(group, x1[3], y1[10], x1[2], y1[11], x1[2], y1[11], x1[3], y1[10]);
        AddSinglePath(group, x1[2], y1[11], x1[1], y1[12], x1[1], y1[12], x1[2], y1[11]);
        AddSinglePath(group, x1[1], y1[12], x1[0], y1[0], x1[0], y1[13], x1[1], y1[12]);

        var points = new[]
        {
            (x1[0], y1[0]),
            (x1[1], y1[1]),
            (x1[2], y1[2]),
            (x1[3], y1[3]),
            (x1[4], y1[4]),
            (x1[5], y1[5]),
            (x1[6], y1[6]),
            (x1[5], y1[8]),
            (x1[4], y1[9]),
            (x1[3], y1[10]),
            (x1[2], y1[11]),
            (x1[1], y1[12])
        };

        var points1 = new[]
        {
            (x1[0], y1[13]),
            (x1[1], y1[1]),
            (x1[2], y1[2]),
            (x1[3], y1[3]),
            (x1[4], y1[4]),
            (x1[5], y1[5]),
            (x1[6], y1[7]),
            (x1[5], y1[8]),
            (x1[4], y1[9]),
            (x1[3], y1[10]),
            (x1[2], y1[11]),
            (x1[1], y1[12])
        };

        AddCircleColorQuin(group, points);
        AddCircleColorQuin(group, points1);

        return group;
    }

    static void AddMinifixBaseQuadPath(XElement group, double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
    {
        var ci = CultureInfo.InvariantCulture;
        string d = $"M {x1.ToString("0.####", ci)} {y1.ToString("0.####", ci)} " +
                  $"L {x2.ToString("0.####", ci)} {y2.ToString("0.####", ci)} " +
                  $"L {x3.ToString("0.####", ci)} {y3.ToString("0.####", ci)} " +
                  $"L {x4.ToString("0.####", ci)} {y4.ToString("0.####", ci)} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));
    }

    static void AddSinglePath(XElement group, double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
    {
        var ci = CultureInfo.InvariantCulture;
        string d = $"M {x1.ToString("0.####", ci)} {y1.ToString().Replace(',', '.')} " +
                  $"L {x2.ToString("0.####", ci)} {y2.ToString().Replace(',', '.')} " +
                  $"L {x3.ToString("0.####", ci)} {y3.ToString().Replace(',', '.')} " +
                  $"L {x4.ToString("0.####", ci)} {y4.ToString().Replace(',', '.')} Z";

        group.Add(new XElement("path",
            new XAttribute("d", d),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));
    }

    static void AddCircleColor(XElement group, (double, double)[] points)
    {
        var ci = CultureInfo.InvariantCulture;
        string d13 = "M " + string.Join(" L ", points.Select(p =>
          $"{p.Item1.ToString("0.####", ci)} {p.Item2.ToString("0.####", ci)}")) + " Z";

        group.Add(new XElement("path",
            new XAttribute("d", d13),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));
    }

    static void AddCircleColorQuin(XElement group, (double, double)[] points)
    {
        var ci = CultureInfo.InvariantCulture;
        string d13 = "M " + string.Join(" L ", points.Select(p =>
          $"{p.Item1.ToString("0.####", ci)} {p.Item2.ToString().Replace(',', '.')}")) + " Z";

        group.Add(new XElement("path",
            new XAttribute("d", d13),
            new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
            new XAttribute("stroke", "black")
        ));
    }
}
