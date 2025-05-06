using System.Globalization;
using System.Xml.Linq;

namespace testesSvg.Components
{
    public static class VbTwo
    {

        #region testes
        ////PecaRebaixoVbTwoMinParafuso -- ok
        //var payload = new SvgRequest
        //{
        //    Width = "12000",
        //    Height = "20000",
        //    Thickness = "700",
        //    Offset = "0",
        //    JoinSystemType = "VbTwo",
        //    Type = "Side"
        //};

        ////PecaRebaixoVbTwoMaxParafuso -- ok
        //var payload = new SvgRequest
        //{
        //    Width = "150000",
        //    Height = "100000",
        //    Thickness = "700",
        //    Offset = "0",
        //    JoinSystemType = "VbTwo",
        //    Type = "Side"
        //};

        ////PecaRebaixoVbTwoMinTambor -- ok
        //var payload = new SvgRequest
        //{
        //    Width = "12000",
        //    Height = "20000",
        //    Thickness = "700",
        //    Offset = "0",
        //    JoinSystemType = "VbTwo",
        //    Type = "Base"
        //};

        ////PecaRebaixoVbTwoMaxTambor -- ok
        //var payload = new SvgRequest
        //{
        //    Width = "150000",
        //    Height = "100000",
        //    Thickness = "700",
        //    Offset = "0",
        //    JoinSystemType = "VbTwo",
        //    Type = "Base"
        //};
        #endregion 


        #region Base (Tambor)
        public static IEnumerable<XElement> GenerateBase(int width, int height)
        {
            // ----- Formula -----
            double[] x1 =
            [
                0.5 * width + 5.0,
                0.5 * width - 8.39746,
                0.5 * width - 45,
                0.5 * width - 95,
                0.5 * width - 145,
                0.5 * width - 181.60254,
                0.5 * width - 195
            ];

            if (width <=1500)
            {
                //0.5x−44.99994
            }

            double[] x2 =
            [
                -0.5 * width + 195,
                -0.5 * width + 181.60254,
                -0.5 * width + 145,
                -0.5 * width + 95,
                -0.5 * width + 45,
                -0.5 * width + 8.39746,
                -0.5 * width + 5.0
            ];

            if (width <= 1500)
            {
                //-0.5x+145.00003
            }

            double[] y1 =
           [
            -0.5 * height + 280,
            -0.5 * height + 230,
            -0.5 * height + 193.39746,
            -0.5 * height + 180,
            -0.5 * height + 330,
            -0.5 * height + 366.60254,
            -0.5 * height + 380,
            -0.5 * height + 329.99994

           ];

            //if (width <= 1500) (SE FOR MENOR)
            //{
            //    // -0.5 * height + 329.99994,
            //   // -0.5 * height + 366.602,
            //}

            if (width >= 2000) //SE FOR MAIOR QUE TANTO)
            {
                //−0.5x+366.602
            }

            double[] y2 =
           [
            0.5 * height - 460,
            0.5 * height - 510,  // y2
            0.5 * height - 546.60254,   // y3
            0.5 * height - 560,   // y4
            0.5 * height - 410,    // y5
            0.5 * height - 373.39746,   // y6
            0.5 * height - 360,  // y7
            0.5 * height - 410.00006  // y8
           
           ];

            if (width > 20000)
            {
                //0.5 * height - 373.3975  // y9
            }

            #region // ----- Formula Small Circle -----
            var circleX1 = new List<double>
            {
                0.5 * width - 365,
                0.5 * width - 371.69873,
                0.5 * width - 390,
                0.5 * width - 415,
                0.5 * width - 440,
                0.5 * width - 458.30127,
                0.5 * width - 465
            };

            if (width < 15998)
                circleX1.Add(0.5 * width - 389.99997);


            var circleX2 = new List<double>
            {
                -0.5 * width + 465,
                -0.5 * width + 458.30127,
                -0.5 * width + 440,
                -0.5 * width + 415,
                -0.5 * width + 390,
                -0.5 * width + 371.69873,
                -0.5 * width + 365
            };

            if (width <= 15992)
            {
                circleX2.Add(-0.5 * width - 759.99998);
                circleX2.Add(0.5 * width - 741.69872);
            }
                

            double[] circleY1 =
           [
            -0.5 * height + 280,
            -0.5 * height + 255,
            -0.5 * height + 236.6987,
            -0.5 * height + 230,
            -0.5 * height + 305,
            -0.5 * height + 323.3013,
            -0.5 * height + 330
           ];

            double[] circleY2 =
           [
            0.5 * height - 460,
            0.5 * height - 485,  // y2
            0.5 * height - 503.30127,   // y3
            0.5 * height - 510,   // y4
            0.5 * height - 435,    // y5
            0.5 * height - 416.6987,   // y6
            0.5 * height - 410  // y7
           ];

            #endregion

            return
              [
                 BaseCircleUpper(x1, y1, "vbone-base-circle-up-right"),
                 BaseCircle(x1, y2, "vbone-base-circle-down-right"),
                 BaseSmallCircle([.. circleX1], circleY1, "vbtwo-base-circle-small-up-right"),
                 BaseCircleUpper(x2, y1, "vbone-base-circle-up-left"),
                 BaseCircle(x2, y2, "vbone-base-circle-down-left"),
                BaseSmallCircle([.. circleX1], circleY2, "vbtwo-base-circle-small-down-right"),
                BaseSmallCircle([.. circleX2], circleY1, "vbtwo-base-circle-small-down-left"),
                BaseSmallCircle([.. circleX2], circleY2, "vbtwo-base-circle-small-down-right"),

              ];
        }

        static XElement BaseCircleUpper(double[] x, double[] y, string name)
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

        static XElement BaseSmallCircle(double[] x, double[] y, string name)
        {
            var group = new XElement("g", new XAttribute("name", name));

            var posValid = x.Length == 8 ? x[7] : x[2];


            AddMinifixBaseQuadPath(group, x[0], y[0], x[1], y[1], x[1], y[1], x[0], y[0]); 

            AddMinifixBaseQuadPath(group, x[1], y[1], x[2], y[2], x[2], y[2], x[1], y[1]); 

            AddMinifixBaseQuadPath(group, x[2], y[2], x[3], y[3], x[3], y[3], x[2], y[2]); 

            AddMinifixBaseQuadPath(group, x[3], y[3], x[4], y[2], x[4], y[2], x[3], y[3]); 

            AddMinifixBaseQuadPath(group, x[4], y[2], x[5], y[1], x[5], y[1], x[4], y[2]); 

            AddMinifixBaseQuadPath(group, x[5], y[1], x[6], y[0], x[6], y[0], x[5], y[1]); 

            AddMinifixBaseQuadPath(group, x[6], y[0], x[5], y[4], x[5], y[4], x[6], y[0]); 

            AddMinifixBaseQuadPath(group, x[5], y[4], x[4], y[5], x[4], y[5], x[5], y[4]); 

            AddMinifixBaseQuadPath(group, x[4], y[5], x[3], y[6], x[3], y[6], x[4], y[5]); 

            AddMinifixBaseQuadPath(group, x[3], y[6], posValid, y[5], posValid, y[5], x[3], y[6]); 

            AddMinifixBaseQuadPath(group, posValid, y[5], x[1], y[4], x[1], y[4], posValid, y[5]); 

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


        #region Side (Parafuso)
        public static IEnumerable<XElement> GenerateSide(int width, int height)
        {
            // ----- Formula -----
            double[] x1 =
            [
                0.5 * width - 45,
                0.5 * width - 48.34937,
                0.5 * width - 57.5,
                0.5 * width - 70,
                0.5 * width - 82.5,
                0.5 * width - 91.65063,
                0.5 * width - 95
             ];

            double[] x2 =
            [
                -0.5 * width + 95,
                -0.5 * width + 91.65063,
                -0.5 * width + 82.5,
                -0.5 * width + 70,
                -0.5 * width + 57.5,
                -0.5 * width + 48.34937,
                -0.5 * width + 45,
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
            0.5 * height - 460,
            0.5 * height - 472.5,
            0.5 * height - 481.65063,
            0.5 * height - 485,
            0.5 * height - 447.5,
            0.5 * height - 438.34937,
            0.5 * height - 435,

           ];


            return
              [
                Side(x1, y1, "vbone-side-up-right"),

                //VALIDAR OS AddMinifixBaseQuadPath dos 3 comparando com o svg no vscode
                Side(x1, y2, "vbone-side-down-right"),
                Side(x2, y1, "vbone-side-up-left"),
                Side(x2, y2, "vbone-side-up-left")

              ];
        }

        static XElement Side(double[] x, double[] y, string name)
        {
            var group = new XElement("g", new XAttribute("name", name));

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
            string d = $"M {x1.ToString("0.#####", ci)} {y1.ToString("0.#####", ci)} " +
                      $"L {x2.ToString("0.#####", ci)} {y2.ToString("0.#####", ci)} " +
                      $"L {x3.ToString("0.#####", ci)} {y3.ToString("0.#####", ci)} " +
                      $"L {x4.ToString("0.#####", ci)} {y4.ToString("0.#####", ci)} Z";

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
              $"{p.X.ToString("0.#####", ci)} {p.Y.ToString("0.#####", ci)}"
          )) + " Z";

            group.Add(new XElement("path",
                new XAttribute("d", d15),
                new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
                new XAttribute("stroke", "black")
            ));

        }
    }
}
