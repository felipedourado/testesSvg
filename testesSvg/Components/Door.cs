using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace testesSvg.Components;

public static class Door
{
    public static IEnumerable<XElement> Generate(int width, int height)
    {
        //if (height <= 1399)
        //    return SideSingle(width, height);

        //if (height <= 4999)
        //    return SideDouble(width, height);

        //// ----- Formula -----
        //var x1 = CalculateSideX1(width);

        //var y1 = CalculateY1(height);

        //var x2 = CalculateSideX2(width);

        //var y2 = new List<double>
        //    {
        //        -2.4041262E-06,
        //        -20.000004,
        //        -19.999998,
        //        -34.64102,
        //        -34.641018,
        //        -40.000004,
        //        -39.999996,
        //        -34.641014,
        //        -19.999996,
        //        -19.99999,
        //        1.0927848E-06,
        //        5.9010376E-06,
        //        20.000006,
        //        20.000011,
        //        34.641018,
        //        34.64102,
        //        39.999996,
        //        40.000004,
        //        34.641006,
        //        34.641014,
        //        19.999989,
        //        19.999994,
        //        2.4041262E-06,
        //    };

        //var y3 = CalculateY3(height);

        var x = CalculateSmallCircleX1(width);
        var y1 = CalculateSmallCircleY1(height);
        var y2 = CalculateSmallCircleY2(height);
        var bigX1 = CalculateBigCircleX1(width);
        var bigY1 = CalculateBigCircleY1(height);
        var bigY2 = CalculateBigCircleY2(height);

        //---------------------------------------------------------------------------------
        var referenceArea = 1200 * 1200;
        var currentArea = width * height;

        // Determina quantos grupos criar baseado na proporção de áreas
        int groupCount = (int)Math.Floor(Math.Sqrt(currentArea / referenceArea));

        // Garante no mínimo 1 grupo e no máximo 3 (como nos exemplos)
        groupCount = Math.Min(Math.Max(groupCount, 1), 3);

        //if (groupCount == 1)
        //{
        //    return
        //        [
        //            Side(xRight, yRight, "dowel-side-up-right"),
        //            Side(xLeft, yRight, "dowel-side-up-left")
        //        ];
        //}
        //else if (groupCount == 2)
        //{
        return
            [
               Side([.. x], [.. y1], "door-small-circle-1"),
               Side([.. x], [.. y2], "door-small-circle-2"),
               Side([.. bigX1], [.. bigY1], "door-big-circle-1"),
               Side([.. bigX1], [.. bigY2], "door-big-circle-2"),
                   //SideSinglePath([.. x1], y2.ToArray(), "dowel-side-middle-right"),
                   //Side([.. x1], y3.ToArray(), "dowel-side-down-right"),

                   //Side([.. x2], y1.ToArray(), "dowel-side-up-left"),
                   //SideSinglePath([.. x2], [..y2], "dowel-side-middle-left"),
                   //Side([.. x2], [..y3], "dowel-side-down-left")
            ];
        //}
    }

    public static List<double> CalculateSideX1(int width)
    {
        return new List<double>
            {
                0.5 * width - 50,
                0.5 * width - 55.35895,
                0.5 * width - 70,
                0.5 * width - 90,
                0.5 * width - 110,
                0.5 * width - 124.64102,
                0.5 * width - 130,
            };
    }

    public static List<double> CalculateSideX2(int width)
    {
        return new List<double>
            {
                -0.5 * width + 130,
                -0.5 * width + 124.64102,
                -0.5 * width + 110,
                -0.5 * width + 90,
                -0.5 * width + 70,
                -0.5 * width + 55.35895,
                -0.5 * width + 50,
            };
    }

    static List<double> CalculateSmallCircleX1(int width)
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

    static List<double> CalculateY1(int height)
    {
        return new List<double>
            {
              -0.5 * height + 320,
              -0.5 * height + 300,
              -0.5 * height + 285.359,
              -0.5 * height + 280,
              -0.5 * height + 340,
              -0.5 * height + 354.6409,
              -0.5 * height + 360
            };
    }

    static List<double> CalculateY3(int height)
    {
        return new List<double>
            {
                 0.5 * height - 480,
                 0.5 * height - 500,
                 0.5 * height - 514.641,
                 0.5 * height - 520,
                 0.5 * height - 460,
                 0.5 * height - 445.359,
                 0.5 * height - 440
            };
    }

    //static IEnumerable<XElement> SideSingle(int width, int height)
    //{
    //    // ----- Formula -----
    //    var x1 = new List<double>
    //        {
    //            0.5 * width - 35,
    //            0.5 * width - 40.35895,
    //            0.5 * width - 55,
    //            0.5 * width - 75,
    //            0.5 * width - 95,
    //            0.5 * width - 109.64102,
    //            0.5 * width -115,
    //        };

    //    var y1 = new List<double>
    //        {
    //          0.5 * height - 600.0000024041262,
    //          0.5 * height - 620.000004,
    //          0.5 * height - 619.999998,
    //          0.5 * height - 634.64102,
    //          0.5 * height - 634.641018,
    //          0.5 * height - 640.000004,
    //          0.5 * height - 639.999996,
    //          0.5 * height - 634.641014,
    //          0.5 * height - 619.999996,
    //          0.5 * height - 619.99999,
    //          0.5 * height - 599.9999989,
    //          0.5 * height - 599.9999940989624,
    //          0.5 * height - 579.999994,
    //          0.5 * height - 579.999989,
    //          0.5 * height - 565.358994,
    //          0.5 * height - 565.35898,
    //          0.5 * height - 560.000004,
    //          0.5 * height - 559.999996,
    //          0.5 * height - 565.358994,
    //          0.5 * height - 565.358986,
    //          0.5 * height - 580.000011,
    //          0.5 * height - 580.000006,
    //        };

    //    var x2 = new List<double>
    //        {
    //            -0.5 * width + 115,
    //            -0.5 * width + 109.64102,
    //            -0.5 * width + 95,
    //            -0.5 * width + 75,
    //            -0.5 * width + 55,
    //            -0.5 * width + 40.35895,
    //            -0.5 * width + 35,
    //        };


    //    return
    //      [
    //         SideSinglePath([.. x1], [.. y1], "dowel-right"),
    //               SideSinglePath([.. x2], [..y1], "dowel-left"),
    //          ];
    //}

    static IEnumerable<XElement> SideDouble(int width, int height)
    {
        // ----- Formula -----
        var x1 = new List<double>
            {
              0.5 * width - 50,
              0.5 * width - 55.35895,
              0.5 * width - 70,
              0.5 * width - 90,
              0.5 * width - 110,
              0.5 * width - 124.64102,
              0.5 * width - 130,

            };

        var y1 = new List<double>
            {
                -0.5 * height + 280,
                -0.5 * height + 260,
                -0.5 * height + 245.35898,
                -0.5 * height + 240,
                -0.5 * height + 300,
                -0.5 * height + 314.64102,
                -0.5 * height + 320,
            };

        var x2 = new List<double>
            {
                -0.5 * width + 130,
                -0.5 * width + 124.64102,
                -0.5 * width + 110,
                -0.5 * width + 90,
                -0.5 * width + 70,
                -0.5 * width + 55.35895,
                -0.5 * width + 50,
            };

        var y2 = new List<double>
            {
               0.5 * height - 460,
               0.5 * height - 480,
               0.5 * height - 494.641,
               0.5 * height - 500,
               0.5 * height - 440,
               0.5 * height - 425.35898,
               0.5 * height - 420,
            };

        return
          [
               Side([.. x1], [.. y1], "dowel-up-right"),
                   Side([.. x1], [..y2], "dowel-down-right"),
                   Side([.. x2], [..y1], "dowel-up-left"),
                   Side([.. x2], [..y2], "dowel-down-left"),
              ];
    }


    static XElement Side(double[] x1, double[] y1, string name)
    {

        var group = new XElement("g", new XAttribute("name", name));

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

    static XElement SideBig(double[] x1, double[] y1, string name)
    {

        var group = new XElement("g", new XAttribute("name", name));

        AddMinifixBaseQuadPath(group, x1[0], y1[0], x1[1], y1[1], x1[1], y1[1], x1[0], y1[0]);
        AddMinifixBaseQuadPath(group, x1[1], y1[1], x1[2], y1[2], x1[2], y1[2], x1[1], y1[1]);
        AddMinifixBaseQuadPath(group, x1[2], y1[2], x1[3], y1[3], x1[3], y1[3], x1[2], y1[2]);
       
        AddMinifixBaseQuadPath(group, x1[3], y1[3], x1[4], y1[2], x1[4], y1[2], x1[3], y1[3]);
        AddMinifixBaseQuadPath(group, x1[4], y1[2], x1[5], y1[1], x1[5], y1[1], x1[4], y1[2]);
        AddMinifixBaseQuadPath(group, x1[5], y1[1], x1[6], y1[0], x1[6], y1[0], x1[5], y1[1]);

        AddMinifixBaseQuadPath(group, x1[6], y1[0], x1[5], y1[5], x1[5], y1[5], x1[6], y1[0]);
        AddMinifixBaseQuadPath(group, x1[5], y1[5], x1[4], y1[6], x1[4], y1[6], x1[5], y1[5]);
        AddMinifixBaseQuadPath(group, x1[4], y1[6], x1[3], y1[6], x1[3], y1[6], x1[4], y1[6]);
        
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
}
