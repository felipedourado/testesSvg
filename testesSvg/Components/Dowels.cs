using System.Globalization;
using System.Xml.Linq;

namespace testesSvg.Components
{
    public static class Dowels
    {
        #region parafuso
        //PecaCanalCavilhaMinParafuso UM FURO-- 
        //var payload = new SvgRequest
        //{
        //    Width = "12000",
        //    Height = "13999",
        //    Thickness = "700",
        //    Offset = "0",
        //    JoinSystemType = "dowels",
        //    Type = "Side"
        //};
        //PecaCanalCavilhaMinParafuso UM FURO-- 
        //var payload = new SvgRequest
        //{
        //    Width = "12000",
        //    Height = "12000",
        //    Thickness = "700",
        //    Offset = "0",
        //    JoinSystemType = "dowels",
        //    Type = "Side"
        //};
        //PecaCanalCavilhaMinParafuso-- DOIS FUROS
        //var payload = new SvgRequest
        //{
        //    Width = "12000",
        //    Height = "14000",
        //    Thickness = "700",
        //    Offset = "0",
        //    JoinSystemType = "dowels",
        //    Type = "Side"
        //};
        //PecaCanalCavilhaMinParafuso-- DOIS FUROS 
        //var payload = new SvgRequest 
        //{
        //    Width = "32000",
        //    Height = "49999",
        //    Thickness = "700",
        //    Offset = "0",
        //    JoinSystemType = "dowels",
        //    Type = "Side"
        //};
        //PecaCanalCavilhaMinParafuso-- TRES FUROS
        //var payload = new SvgRequest
        //{
        //    Width = "12000",
        //    Height = "50000",
        //    Thickness = "700",
        //    Offset = "0",
        //    JoinSystemType = "dowels",
        //    Type = "Side"
        //};
        //PecaCanalCavilhaMinParafuso-- TRES FUROS
        //var payload = new SvgRequest
        //{
        //    Width = "102000",
        //    Height = "90000",
        //    Thickness = "700",
        //    Offset = "0",
        //    JoinSystemType = "dowels",
        //    Type = "Side"
        //};

        #endregion

        #region base (tambor)
        //PecaCanalCavilhaMinTambor-- DOIS FUROS
        //var payload = new SvgRequest
        //{
        //    Width = "12000",
        //    Height = "20000",
        //    Thickness = "700",
        //    Offset = "0",
        //    JoinSystemType = "dowels",
        //    Type = "Base"
        //};
        //PecaCanalCavilhaMAXTambor-- DOIS FUROS
        //var payload = new SvgRequest
        //{
        //    Width = "52000",
        //    Height = "49999",
        //    Thickness = "700",
        //    Offset = "0",
        //    JoinSystemType = "dowels",
        //    Type = "Base"
        //};
        //PecaCanalCavilhaMinTambor-- TRES FUROS
        //var payload = new SvgRequest
        //{
        //    Width = "12000",
        //    Height = "50000",
        //    Thickness = "700",
        //    Offset = "0",
        //    JoinSystemType = "dowels",
        //    Type = "Base"
        //};
        //PecaCanalCavilhaMaxTambor-- TRES FUROS 
        //var payload = new SvgRequest
        //{
        //    Width = "100000",
        //    Height = "100000",
        //    Thickness = "700",
        //    Offset = "0",
        //    JoinSystemType = "dowels",
        //    Type = "Base"
        //};

        #endregion

        #region Base (Tambor)
        public static IEnumerable<XElement> GenerateBase(int width, int height, bool isLandscape)
        {
            if (height <= 4999)
                return BaseDouble(width, height, isLandscape);

            // ----- Formula -----
            var x1 = CalculateBaseX1(width);

            var y1 = new List<double>
            {
              -0.5 * height + 360,
              -0.5 * height + 354.6409,
              -0.5 * height + 340,
              -0.5 * height + 320,
              -0.5 * height + 300,
              -0.5 * height + 285.359,
              -0.5 * height + 280
            };

            var x2 = CalculateBaseX2(width);

            var y2 = new List<double>
            {
               39.999996,
               34.64101,
               34.64102,
               40.000008,
               19.999994,
               20.000004,
               -6.5567083E-06,
               3.059797E-06,
               -20.000008,
               -19.999998,
               -34.64103,
               -34.641018,
               -40.000008,
               -39.999996,
               -34.641003,
               -20.000002,
               -19.999992,
               -4.3312575E-06,
               5.285248E-06,
               20.00001,
               20.00002,
               34.641018,
               34.64103
            };

            var y3 = new List<double>
            {
                 0.5 * height - 440,
                 0.5 * height - 445.359,
                 0.5 * height - 460,
                 0.5 * height - 480,
                 0.5 * height - 500,
                 0.5 * height - 514.641,
                 0.5 * height - 520
            };

            return
                [
                   Base([.. x1], [.. y1], "dowel-side-up-right", isLandscape),
                   BaseSinglePath([.. x1], y2.ToArray(), "dowel-side-middle-right", isLandscape),
                   Base([.. x1], y3.ToArray(), "dowel-side-down-right", isLandscape),

                   Base([.. x2], y1.ToArray(), "dowel-side-up-left", isLandscape),
                   BaseSinglePath([.. x2], [..y2], "dowel-side-middle-left", isLandscape),
                   Base([.. x2], [..y3], "dowel-side-down-left", isLandscape)
                ];
        }

        public static List<double> CalculateBaseX1(int width)
        {
            return new List<double>
            {
                0.5 * width - 219.5,
                0.5 * width + 0.5
            };
        }

        public static List<double> CalculateBaseX2(int width)
        {
            return new List<double>
            {
                -0.5 * width - 0.5,
                -0.5 * width + 219.5,
            };
        }

        static IEnumerable<XElement> BaseDouble(int width, int height, bool isLandscape)
        {
            // ----- Formula -----
            var x1 = new List<double>
            {
              0.5 * width - 219.5,
              0.5 * width - 0.5
            };

            var y1 = new List<double>
            {
                -0.5 * height + 320,
                -0.5 * height + 314.641,
                -0.5 * height + 300,
                -0.5 * height + 280,
                -0.5 * height + 260,
                -0.5 * height + 245.35895,
                -0.5 * height + 240,
            };

            var x2 = new List<double>
            {
                -0.5 * width - 0.5,
                -0.5 * width + 219.5
            };

            var y2 = new List<double>
            {
               0.5 * height - 420,
               0.5 * height - 425.35895,
               0.5 * height - 440,
               0.5 * height - 460,
               0.5 * height - 480,
               0.5 * height - 494.64102,
               0.5 * height - 500,
            };

            return
              [
                   Base([.. x1], [.. y1], "dowel-base-up-right", isLandscape),
                   Base([.. x1], [.. y2], "dowel-base-down-right", isLandscape),
                   Base([.. x2], [.. y1], "dowel-base-up-right", isLandscape),
                   Base([.. x2], [..y2], "dowel-base-down-left", isLandscape ),
              ];
        }

        static XElement Base(double[] x, double[] y, string name, bool isLandscape)
        {
            var group = new XElement("g", new XAttribute("name", name), isLandscape ? new XAttribute("transform", "rotate(-90)") : null);

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

        public static XElement BaseSinglePath(double[] x1, double[] y1, string name, bool isLandscape)
        {

            var group = new XElement("g", new XAttribute("name", name), isLandscape ? new XAttribute("transform", "rotate(-90)") : null);

            AddSinglePath(group, x1[0], y1[0], x1[0], y1[2], x1[1], y1[2], x1[1], y1[0]); // 39.999996 → 34.64102
            AddSinglePath(group, x1[0], y1[1], x1[0], y1[5], x1[1], y1[5], x1[1], y1[1]); // 34.64101 → 20.000004
            AddSinglePath(group, x1[0], y1[4], x1[0], y1[7], x1[1], y1[7], x1[1], y1[4]); // 19.999994 → 3.059797E-06
            AddSinglePath(group, x1[0], y1[6], x1[0], y1[9], x1[1], y1[9], x1[1], y1[6]); // -6.5567083E-06 → -19.999998
            AddSinglePath(group, x1[0], y1[8], x1[0], y1[11], x1[1], y1[11], x1[1], y1[8]); // -20.000008 → -34.641018
            AddSinglePath(group, x1[0], y1[10], x1[0], y1[13], x1[1], y1[13], x1[1], y1[10]); // -34.64103 → -39.999996
            AddSinglePath(group, x1[0], y1[12], x1[0], y1[14], x1[1], y1[14], x1[1], y1[12]); // -40.000008 → -34.641003
            AddSinglePath(group, x1[0], y1[11], x1[0], y1[16], x1[1], y1[16], x1[1], y1[11]); // -34.641018 → -19.999992
            AddSinglePath(group, x1[0], y1[15], x1[0], y1[18], x1[1], y1[18], x1[1], y1[15]); // -20.000002 → 5.285248E-06
            AddSinglePath(group, x1[0], y1[17], x1[0], y1[20], x1[1], y1[20], x1[1], y1[17]); // -4.3312575E-06 → 20.00002
            AddSinglePath(group, x1[0], y1[19], x1[0], y1[22], x1[1], y1[22], x1[1], y1[19]); // 20.00001 → 34.64103
            AddSinglePath(group, x1[0], y1[21], x1[0], y1[3], x1[1], y1[3], x1[1], y1[21]);  // 34.641018 → 40.000008

            // ----- Décimo Terceiro Path -----
            //var points = new[]
            //{
            //    (x[0], y[0]),
            //    (x[1], y[1]),
            //    (x[2], y[3]),
            //    (x[3], y[5]),
            //    (x[4], y[3]),
            //    (x[5], y[8]),
            //    (x[6], y[10]),
            //    (x[5], y[12]),
            //    (x[4], y[14]),
            //    (x[3], y[16]),
            //    (x[2], y[14]),
            //    (x[1], y[20])
            //};

            //AddSingleCircle(group, points);

            //// ----- Décimo Quarto Path -----
            //AddSingleCircle(group, points);
            return group;
        }

        static XElement BaseCircle(double[] x1, double[] y1, string name, bool isLandscape)
        {
            var group = new XElement("g", new XAttribute("name", name));

            AddMinifixBaseQuadPath(group, x1[0], y1[0], x1[1], y1[1], x1[1], y1[1], x1[0], y1[0]);
            AddMinifixBaseQuadPath(group, x1[0], y1[1], x1[1], y1[2], x1[1], y1[2], x1[0], y1[1]);
            AddMinifixBaseQuadPath(group, x1[0], y1[2], x1[1], y1[3], x1[1], y1[3], x1[0], y1[2]);
            AddMinifixBaseQuadPath(group, x1[0], y1[3], x1[1], y1[4], x1[1], y1[4], x1[0], y1[3]);
            AddMinifixBaseQuadPath(group, x1[0], y1[4], x1[1], y1[5], x1[1], y1[5], x1[0], y1[4]);
            AddMinifixBaseQuadPath(group, x1[0], y1[5], x1[1], y1[6], x1[1], y1[6], x1[0], y1[5]);
            AddMinifixBaseQuadPath(group, x1[0], y1[6], x1[1], y1[5], x1[1], y1[5], x1[0], y1[6]);
            AddMinifixBaseQuadPath(group, x1[0], y1[5], x1[1], y1[4], x1[1], y1[4], x1[0], y1[5]);
            AddMinifixBaseQuadPath(group, x1[0], y1[4], x1[1], y1[3], x1[1], y1[3], x1[0], y1[4]);
            AddMinifixBaseQuadPath(group, x1[0], y1[3], x1[1], y1[2], x1[1], y1[2], x1[0], y1[3]);
            AddMinifixBaseQuadPath(group, x1[0], y1[2], x1[1], y1[1], x1[1], y1[1], x1[0], y1[2]);
            AddMinifixBaseQuadPath(group, x1[0], y1[1], x1[1], y1[0], x1[1], y1[0], x1[0], y1[1]);


            // ----- Decimo Terceiro background Path -----
            //var points = new (double X, double Y)[]
            //{
            //    (x1[0], y1[0]),
            //    (x1[1], y1[1]),
            //    (x1[2], y1[2]),
            //    (x1[3], y1[3]),
            //    (x1[4], y1[2]),
            //    (x1[5], y1[1]),
            //    (x1[6], y1[0]),
            //    (x1[5], y1[4]),
            //    (x1[4], y1[5]),
            //    (x1[3], y1[6]),
            //    (x1[2], y1[5]),
            //    (x1[1], y1[4])
            //};

            //AddCircleColor(group, points);
            //AddCircleColor(group, points);

            return group;
        }

        #endregion



        #region Side
        public static IEnumerable<XElement> GenerateSide(int width, int height, bool isLandscape)
        {
            if (height <= 1399)
                return SideSingle(width, height, isLandscape);

            if (height <= 4999)
                return SideDouble(width, height, isLandscape);

            // ----- Formula -----
            var x1 = CalculateSideX1(width);

            var y1 = CalculateY1(height);

            var x2 = CalculateSideX2(width);

            var y2 = new List<double>
            {
                -2.4041262E-06,
                -20.000004,
                -19.999998,
                -34.64102,
                -34.641018,
                -40.000004,
                -39.999996,
                -34.641014,
                -19.999996,
                -19.99999,
                1.0927848E-06,
                5.9010376E-06,
                20.000006,
                20.000011,
                34.641018,
                34.64102,
                39.999996,
                40.000004,
                34.641006,
                34.641014,
                19.999989,
                19.999994,
                2.4041262E-06,
            };

            var y3 = CalculateY3(height);

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
                   Side([.. x1], [.. y1], "dowel-side-up-right", isLandscape),
                   SideSinglePath([.. x1], y2.ToArray(), "dowel-side-middle-right", isLandscape),
                   Side([.. x1], y3.ToArray(), "dowel-side-down-right", isLandscape),

                   Side([.. x2], y1.ToArray(), "dowel-side-up-left", isLandscape),
                   SideSinglePath([.. x2], [..y2], "dowel-side-middle-left", isLandscape),
                   Side([.. x2], [..y3], "dowel-side-down-left", isLandscape)
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

        static IEnumerable<XElement> SideSingle(int width, int height, bool isLandscape)
        {
            // ----- Formula -----
            var x1 = new List<double>
            {
                0.5 * width - 35,
                0.5 * width - 40.35895,
                0.5 * width - 55,
                0.5 * width - 75,
                0.5 * width - 95,
                0.5 * width - 109.64102,
                0.5 * width -115,
            };

            var y1 = new List<double>
            {
              0.5 * height - 600.0000024041262,
              0.5 * height - 620.000004,
              0.5 * height - 619.999998,
              0.5 * height - 634.64102,
              0.5 * height - 634.641018,
              0.5 * height - 640.000004,
              0.5 * height - 639.999996,
              0.5 * height - 634.641014,
              0.5 * height - 619.999996,
              0.5 * height - 619.99999,
              0.5 * height - 599.9999989,
              0.5 * height - 599.9999940989624,
              0.5 * height - 579.999994,
              0.5 * height - 579.999989,
              0.5 * height - 565.358994,
              0.5 * height - 565.35898,
              0.5 * height - 560.000004,
              0.5 * height - 559.999996,
              0.5 * height - 565.358994,
              0.5 * height - 565.358986,
              0.5 * height - 580.000011,
              0.5 * height - 580.000006,
            };

            var x2 = new List<double>
            {
                -0.5 * width + 115,
                -0.5 * width + 109.64102,
                -0.5 * width + 95,
                -0.5 * width + 75,
                -0.5 * width + 55,
                -0.5 * width + 40.35895,
                -0.5 * width + 35,
            };

            return
              [
                 SideSinglePath([.. x1], [.. y1], "dowel-right", isLandscape),
                   SideSinglePath([.. x2], [..y1], "dowel-left", isLandscape),
              ];
        }

        static IEnumerable<XElement> SideDouble(int width, int height, bool isLandscape)
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
                   Side([.. x1], [.. y1], "dowel-up-right", isLandscape),
                   Side([.. x1], [..y2], "dowel-down-right", isLandscape),
                   Side([.. x2], [..y1], "dowel-up-left", isLandscape),
                   Side([.. x2], [..y2], "dowel-down-left", isLandscape),
              ];
        }

        #endregion

        static XElement Side(double[] x, double[] y, string name, bool isLandscape)
        {
            var group = new XElement("g", new XAttribute("name", name), isLandscape ? new XAttribute("transform", "rotate(-90)") : null);

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

            // ----- Décimo Terceiro Path -----
            var points = new[]
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

            AddCircleColor(group, points);

            // ----- Décimo Quarto Path -----
            AddCircleColor(group, points);
            return group;
        }

        public static XElement SideSinglePath(double[] x, double[] y, string name, bool isLandscape)
        {
            var group = new XElement("g", new XAttribute("name", name), isLandscape ? new XAttribute("transform", "rotate(-90)") : null);

            // ----- Primeiro Path -----
            AddSinglePath(group, x[0], y[0], x[1], y[1], x[1], y[2], x[0], y[0]);

            // ----- Segundo Path -----
            AddSinglePath(group, x[1], y[1], x[2], y[3], x[2], y[3], x[1], y[2]);

            // ----- Terceiro Path -----
            AddSinglePath(group, x[2], y[3], x[3], y[5], x[3], y[6], x[2], y[4]);

            // ----- Quarto Path -----
            AddSinglePath(group, x[3], y[5], x[4], y[7], x[4], y[7], x[3], y[6]);

            // ----- Quinto Path -----
            AddSinglePath(group, x[4], y[7], x[5], y[8], x[5], y[9], x[4], y[7]);

            // ----- Sexto Path -----
            AddSinglePath(group, x[5], y[8], x[6], y[10], x[6], y[11], x[5], y[9]);

            // ----- Sétimo Path -----
            AddSinglePath(group, x[6], y[10], x[5], y[12], x[5], y[13], x[6], y[11]);

            // ----- Oitavo Path -----
            AddSinglePath(group, x[5], y[12], x[4], y[14], x[4], y[15], x[5], y[13]);

            // ----- Nono Path -----
            AddSinglePath(group, x[4], y[14], x[3], y[16], x[3], y[17], x[4], y[15]);

            // ----- Décimo Path -----
            AddSinglePath(group, x[3], y[16], x[2], y[18], x[2], y[19], x[3], y[17]);

            // ----- Décimo Primeiro Path -----
            AddSinglePath(group, x[2], y[18], x[1], y[20], x[1], y[21], x[2], y[19]);

            // ----- Décimo Segundo Path -----
            AddSinglePath(group, x[1], y[20], x[0], y[0], x[0], y[0], x[1], y[21]);

            // ----- Décimo Terceiro Path -----
            var points = new[]
            {
                (x[0], y[0]),
                (x[1], y[1]),
                (x[2], y[3]),
                (x[3], y[5]),
                (x[4], y[3]),
                (x[5], y[8]),
                (x[6], y[10]),
                (x[5], y[12]),
                (x[4], y[14]),
                (x[3], y[16]),
                (x[2], y[14]),
                (x[1], y[20])
            };

            AddSingleCircle(group, points);

            // ----- Décimo Quarto Path -----
            AddSingleCircle(group, points);
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

        static void AddSingleCircle(XElement group, (double, double)[] points)
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
    }
}
