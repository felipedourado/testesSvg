using System.Globalization;
using System.Xml.Linq;

namespace testesSvg.Components
{
    public static class Minifix
    {
        #region Base (Tambor)
        public static IEnumerable<XElement> GenerateBase(int width, int height)
        {
            // ----- Formula -----
            double[] x1 =
            [
                0.5 * width - 239.5,     // x1
                0.5 * width + 0.5,  // x2
            ];

            double[] x2 =
            [
                -(0.5 * width + 0.5),     // x1
                -(0.5 * width - 239.5),  // x2
            ];

            double[] y1 =
           [
            -(0.5 * height - 320),// y1
            -(0.5 * height - 314.641),  // y2
            -(0.5 * height - 300),   // y3
            -(0.5 * height - 280),   // y4
            -(0.5 * height - 260),    // y5
            -(0.5 * height - 245.359),   // y6
            -(0.5 * height - 240)  // y7
           ];

            double[] y2 =
           [
            0.5 * height - 420,// y1
            0.5 * height - 425.359,  // y2
            0.5 * height - 425.358,   // y3
            0.5 * height - 440,   // y4
            0.5 * height - 460,    // y5
            0.5 * height - 480,   // y6
            0.5 * height - 494.642,  // y7
            0.5 * height - 494.641,  // y8
            0.5 * height - 500  // y9
           ];


            #region Formula Circle
            // ----- Formula Circle-----
            double[] circleX1 =
            [
                0.5 * width - 165,     // x1
                0.5 * width - 175.0481,  // x2
                0.5 * width - 202.5,  // x3
                0.5 * width - 240,  // x4
                0.5 * width - 277.5,  // x5
                0.5 * width - 304.9517,  // x6
                0.5 * width - 315  // x7
            ];

            double[] circleX2 =
           [
                -(0.5 * width - 315),     // x1
                -(0.5 * width - 304.9517),  // x2
                -(0.5 * width - 277.5),  // x3
                -(0.5 * width - 240),  // x4
                -(0.5 * width - 202.5),  // x5
                -(0.5 * width - 175.0483),  // x6
                -(0.5 * width - 165)  // x7
           ];

            double[] circleY1 =
            [
                -(0.5 * height - 280), //y1
                -(0.5 * height - 242.5), //y2
                -(0.5 * height - 215.048), //y3
                -(0.5 * height - 205), //y4
                -(0.5 * height - 318), //y5
                -(0.5 * height - 344.951), //y6
                -(0.5 * height - 355)//y7
            ];

            double[] circleY2 =
            [
                0.5 * height - 460, //y1
                0.5 * height - 497.5, //y2
                0.5 * height - 524.951, //y3
                0.5 * height - 535, //y4
                0.5 * height - 422.5, //y5
                0.5 * height - 395.048, //y6
                0.5 * height - 385//y7
            ];

            #endregion

            return
              [
                 Base(x1, y1, "minifix-base-up-right"),
                 BaseCircle(circleX1, circleY1, "minifix-base-circle-up-right"),
                 BaseDown(x1, y2, "minifix-base-down-right"),
                 BaseCircle(circleX1, circleY2, "minifix-base-circle-down-right"),
                 Base(x2, y1, "minifix-base-up-left"),
                 BaseCircle(circleX2, circleY1, "minifix-base-circle-up-left"),
                 BaseDown(x2, y2, "minifix-base-down-left"),
                 BaseCircle(circleX2, circleY2, "minifix-base-circle-down-left"),

              ];
        }

        static XElement Base(double[] x, double[] y, string name)
        {
            var group = new XElement("g", new XAttribute("name", name));

            // ----- Primeiro Path -----
            AddMinifixBaseQuadPath(group, x[0], y[0], x[0], y[1], x[1], y[1], x[1], y[0]);

            // ----- Segundo Path -----
            AddMinifixBaseQuadPath(group, x[0], y[1], x[0], y[2], x[1], y[2], x[1], y[1]);

            // ----- Terceiro Path -----
            AddMinifixBaseQuadPath(group, x[0], y[2], x[0], y[3], x[1], y[3], x[1], y[2]);

            // ----- Quarto Path -----
            AddMinifixBaseQuadPath(group, x[0], y[3], x[0], y[4], x[1], y[4], x[1], y[3]);

            // ----- Quinto Path -----
            AddMinifixBaseQuadPath(group, x[0], y[4], x[0], y[5], x[1], y[5], x[1], y[4]);

            // ----- Sexto Path -----
            AddMinifixBaseQuadPath(group, x[0], y[5], x[0], y[6], x[1], y[6], x[1], y[5]);

            // ----- Sétimo Path -----
            AddMinifixBaseQuadPath(group, x[0], y[6], x[0], y[5], x[1], y[5], x[1], y[6]);

            // ----- Oitavo Path -----
            AddMinifixBaseQuadPath(group, x[0], y[5], x[0], y[4], x[1], y[4], x[1], y[5]);

            // ----- Nono Path -----
            AddMinifixBaseQuadPath(group, x[0], y[4], x[0], y[3], x[1], y[3], x[1], y[4]);

            // ----- Décimo Path -----
            AddMinifixBaseQuadPath(group, x[0], y[3], x[0], y[2], x[1], y[2], x[1], y[3]);

            // ----- Décimo Primeiro Path -----
            AddMinifixBaseQuadPath(group, x[0], y[2], x[0], y[1], x[1], y[1], x[1], y[2]);

            // ----- Décimo Segundo Path -----
            AddMinifixBaseQuadPath(group, x[0], y[1], x[0], y[0], x[1], y[0], x[1], y[1]);

            // ----- Décimo Terceiro Path -----
            var points = new[] { y[0], y[1], y[2], y[3], y[4], y[5], y[6], y[5], y[4], y[3], y[2], y[1] };
            AddDrawColor(group, points, x[1]);

            // ----- Décimo Quarto Path -----
            AddDrawColor(group, points, x[1]);

            return group;
        }

        static XElement BaseCircle(double[] x, double[] y, string name)
        {
            var group = new XElement("g", new XAttribute("name", name));

            // ----- Primeiro Path -----
            AddMinifixBaseQuadPath(group, x[0], y[0], x[1], y[1], x[1], y[1], x[0], y[0]);

            // ----- Segundo Path -----
            AddMinifixBaseQuadPath(group, x[1], y[1], x[2], y[2], x[2], y[2], x[1], y[1]);

            // ----- Terceiro Path -----
            AddMinifixBaseQuadPath(group, x[2], y[2], x[3], y[3], x[3], y[3], x[2], y[2]);

            // ----- Quarto Path -----
            AddMinifixBaseQuadPath(group, x[3], y[3], x[4], y[2], x[4], y[2], x[3], y[3]);

            // ----- Quinto Path -----
            AddMinifixBaseQuadPath(group, x[4], y[2], x[5], y[1], x[5], y[1], x[4], y[2]);

            // ----- Sexto Path -----
            AddMinifixBaseQuadPath(group, x[5], y[1], x[6], y[0], x[6], y[0], x[5], y[1]);

            // ----- Sétimo Path -----
            AddMinifixBaseQuadPath(group, x[6], y[0], x[5], y[4], x[5], y[4], x[6], y[0]);

            // ----- Oitavo Path -----
            AddMinifixBaseQuadPath(group, x[5], y[4], x[4], y[5], x[4], y[5], x[5], y[4]);

            // ----- Nono Path -----
            AddMinifixBaseQuadPath(group, x[4], y[5], x[3], y[6], x[3], y[6], x[4], y[5]);

            // ----- Décimo Path -----
            AddMinifixBaseQuadPath(group, x[3], y[6], x[2], y[5], x[2], y[5], x[3], y[6]);

            // ----- Décimo Primeiro Path -----
            AddMinifixBaseQuadPath(group, x[2], y[5], x[1], y[4], x[1], y[4], x[2], y[5]);

            // ----- Décimo Segundo Path -----
            AddMinifixBaseQuadPath(group, x[1], y[4], x[0], y[0], x[0], y[0], x[1], y[4]);

            // ----- Decimo Terceiro background Path -----
            var points = new (double X, double Y)[]
            {
                (x[0], y[0]),
                (x[1], y[1]),
                (x[2], y[2]),
                (x[3], y[3]),
                (x[4], y[2]),
                (x[5], y[1]),
                (x[6], y[0]),
                (x[5], y[4]),
                (x[4], y[5]),
                (x[3], y[6]),
                (x[2], y[5]),
                (x[1], y[4])
            };

            AddDrawColorCircle(group, points);
            AddDrawColorCircle(group, points);

            return group;
        }

        static XElement BaseDown(double[] x, double[] y, string name)
        {
            var group = new XElement("g", new XAttribute("name", name));

            // ----- Primeiro Path -----
            AddMinifixBaseQuadPath(group, x[0], y[0], x[0], y[1], x[1], y[3], x[1], y[0]);

            // ----- Segundo Path -----
            AddMinifixBaseQuadPath(group, x[0], y[1], x[0], y[3], x[1], y[3], x[1], y[3]);

            // ----- Terceiro Path -----
            AddMinifixBaseQuadPath(group, x[0], y[3], x[0], y[4], x[1], y[4], x[1], y[3]);

            // ----- Quarto Path -----
            AddMinifixBaseQuadPath(group, x[0], y[4], x[0], y[5], x[1], y[5], x[1], y[4]);

            // ----- Quinto Path -----
            AddMinifixBaseQuadPath(group, x[0], y[5], x[0], y[6], x[1], y[7], x[1], y[5]);

            // ----- Sexto Path -----
            AddMinifixBaseQuadPath(group, x[0], y[6], x[0], y[8], x[1], y[8], x[1], y[7]);

            // ----- Sétimo Path -----
            AddMinifixBaseQuadPath(group, x[0], y[8], x[0], y[7], x[1], y[7], x[1], y[8]);

            // ----- Oitavo Path -----
            AddMinifixBaseQuadPath(group, x[0], y[7], x[0], y[5], x[1], y[5], x[1], y[7]);

            // ----- Nono Path -----
            AddMinifixBaseQuadPath(group, x[0], y[5], x[0], y[4], x[1], y[4], x[1], y[5]);

            // ----- Décimo Path -----
            AddMinifixBaseQuadPath(group, x[0], y[4], x[0], y[3], x[1], y[3], x[1], y[4]);

            // ----- Décimo Primeiro Path -----
            AddMinifixBaseQuadPath(group, x[0], y[3], x[0], y[1], x[1], y[2], x[1], y[3]);

            // ----- Décimo Segundo Path -----
            AddMinifixBaseQuadPath(group, x[0], y[1], x[0], y[0], x[1], y[0], x[1], y[2]);

            // ----- Décimo Terceiro Path -----

            var pointsLeft = new (double X, double Y)[]
            {
                (x[0], y[0]),
                (x[0], y[1]),
                (x[0], y[3]),
                (x[0], y[4]),
                (x[0], y[5]),
                (x[0], y[6]),
                (x[0], y[8]),
                (x[0], y[7]),
                (x[0], y[5]),
                (x[0], y[4]),
                (x[0], y[3]),
                (x[0], y[1])
            };

            AddDrawColorCircle(group, pointsLeft);

            // ----- Décimo Quarto Path -----
            var pointsRight = new[]
            {
                (x[1], y[0]),
                (x[1], y[2]),
                (x[1], y[3]),
                (x[1], y[4]),
                (x[1], y[5]),
                (x[1], y[7]),
                (x[1], y[8]),
                (x[1], y[7]),
                (x[1], y[5]),
                (x[1], y[4]),
                (x[1], y[3]),
                (x[1], y[2])
            };

            AddDrawColorCircle(group, pointsLeft);

            return group;
        }

        #endregion


        #region Side (Parafuso)
        public static IEnumerable<XElement> GenerateSide(int width, int height)
        {

            // ----- Formula -----
            double[] x1 = 
            [
                0.5 * width - 50,
                0.5 * width - 53.34937, 
                0.5 * width - 62.5, 
                0.5 * width - 75, 
                0.5 * width - 87.5,
                0.5 * width - 96.65063, 
                0.5 * width - 100
             ];

            double[] x2 =
            [
                -0.5 * width + 100,    
                -0.5 * width + 96.65063,  
                -0.5 * width + 87.5, 
                -0.5 * width + 75,  
                -0.5 * width + 62.5,  
                -0.5 * width + 53.34937,  
                -0.5 * width + 50, 
            ];

            double[] y1 =
           [
            -(0.5 * height - 280),
            -(0.5 * height - 267.5),  
            -(0.5 * height - 258.34937),   
            -(0.5 * height - 255),   
            -(0.5 * height - 292.5),   
            -(0.5 * height - 301.65063),   
            -(0.5 * height - 305)  
           ];

            double[] y2 =
           [
            0.5 * height - 420,
            0.5 * height - 472.5,  
            0.5 * height - 481.65063,   
            0.5 * height - 485,   
            0.5 * height - 447.5,   
            0.5 * height - 438.34937,   
            0.5 * height - 435,   
           
           ];
     

            return
              [
                Side(x1, y1, "minifix-side-up-right"),

                //VALIDAR OS AddMinifixBaseQuadPath dos 3 comparando com o svg no vscode
                //finalizado testar o min e max
                Side(x1, y2, "minifix-side-down-right"),
                Side(x2, y1, "minifix-side-up-left"),
                Side(x2, y2, "minifix-side-up-left")

              ];
        }

        static XElement Side(double[] x, double[] y, string name)
        {
            var group = new XElement("g", new XAttribute("name", "minifix-right-up"));

            // ----- Primeiro Path -----
            AddMinifixBaseQuadPath(group, x[0], y[0], x[1], y[1], x[1], y[1], x[0], y[0]);

            // ----- Segundo Path -----
            AddMinifixBaseQuadPath(group, x[1], y[1], x[2], y[2], x[2], y[2], x[1], y[1]);

            // ----- Terceiro Path -----
            AddMinifixBaseQuadPath(group, x[2], y[2], x[3], y[3], x[3], y[3], x[2], y[2]);

            // ---- Quarto Path -----
            AddMinifixBaseQuadPath(group, x[3], y[3], x[4], y[2], x[4], y[2], x[3], y[3]);

            // ----- Quinto Path -----
            AddMinifixBaseQuadPath(group, x[4], y[2], x[5], y[1], x[5], y[1], x[4], y[2]);

            // ----- Sexto Path -----
            AddMinifixBaseQuadPath(group, x[5], y[1], x[6], y[0], x[6], y[0], x[5], y[1]);

            // ----- Setimo Path -----
            AddMinifixBaseQuadPath(group, x[6], y[0], x[5], y[4], x[5], y[4], x[6], y[0]);

            // ----- Oitavo Path -----
            AddMinifixBaseQuadPath(group, x[5], y[4], x[4], y[5], x[4], y[5], x[5], y[4]);

            // ----- Nono Path -----
            AddMinifixBaseQuadPath(group, x[4], y[5], x[3], y[6], x[3], y[6], x[4], y[5]);

            // ----- Decimo Path -----
            AddMinifixBaseQuadPath(group, x[3], y[6], x[2], y[5], x[2], y[5], x[3], y[6]);

            // ----- Decimo Primeiro Path -----
            AddMinifixBaseQuadPath(group, x[2], y[5], x[1], y[4], x[1], y[4], x[2], y[5]);

            // ----- Decimo Segundo Path -----
            AddMinifixBaseQuadPath(group, x[1], y[4], x[0], y[0], x[0], y[0], x[1], y[4]);

            // ----- Decimo Terceiro background Path -----
            var points = new (double X, double Y)[]
           {
                (x[0], y[0]),
                (x[1], y[1]),
                (x[2], y[2]),
                (x[3], y[3]),
                (x[4], y[2]),
                (x[5], y[1]),
                (x[6], y[0]),
                (x[5], y[4]),
                (x[4], y[5]),
                (x[3], y[6]),
                (x[2], y[5]),
                (x[1], y[4])
           };

            AddDrawColorCircle(group, points);
            AddDrawColorCircle(group, points);

            return group;
        }

        #endregion

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

        static void AddDrawColor(XElement group, double[] points, double x)
        {
            var ci = CultureInfo.InvariantCulture;
            string d14 = "M " + string.Join(" L ", points.Select(y =>
                 $"{x.ToString("0.####", ci)} {y.ToString("0.####", ci)}"
             )) + " Z";

            group.Add(new XElement("path",
                new XAttribute("d", d14),
                new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
                new XAttribute("stroke", "black")
            ));
        }

        static void AddDrawColorCircle(XElement group, (double X, double Y)[] points)
        {
            var ci = CultureInfo.InvariantCulture;
            var d15 = "M " + string.Join(" L ", points.Select(p =>
              $"{p.X.ToString("0.####", ci)} {p.Y.ToString("0.####", ci)}"
          )) + " Z";

            group.Add(new XElement("path",
                new XAttribute("d", d15),
                new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
                new XAttribute("stroke", "black")
            ));

        }
       
    }
}
