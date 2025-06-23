using System.Globalization;
using System.Xml.Linq;

namespace testesSvg.Components
{
    #region testes
    //PecaRebaixoMinifixMinParafuso --OK (UM FURO)
    //var payload = new SvgRequest
    //{
    //    Width = "12000",
    //    Height = "20000",
    //    Thickness = "700",
    //    Offset = "0",
    //    JoinSystemType = "minifix",
    //    Type = "Side"
    //};
    //PecaRebaixoMinifixMAXParafuso --OK (UM FURO)
    //var payload = new SvgRequest
    //{
    //    Width = "12000",
    //    Height = "13999",
    //    Thickness = "700",
    //    Offset = "0",
    //    JoinSystemType = "minifix",
    //    Type = "Side"
    //};

    //PecaRebaixoMinifixMinParafuso --OK (DOIS FUROS)
    //var payload = new SvgRequest
    //{
    //    Width = "12000",
    //    Height = "14000",
    //    Thickness = "700",
    //    Offset = "0",
    //    JoinSystemType = "minifix",
    //    Type = "Side"
    //};

    //PecaRebaixoMinifixMaxParafuso --OK (DOIS FUROS)
    //var payload = new SvgRequest
    //{
    //    Width = "219000",
    //    Height = "100000",
    //    Thickness = "700",
    //    Offset = "0",
    //    JoinSystemType = "minifix",
    //    Type = "Side"
    //};

    ////PecaCanalMinifixMinParafuso --OK
    //var payload = new SvgRequest
    //{
    //    Width = "30000",
    //    Height = "30000",
    //    Thickness = "700",
    //    Offset = "1500",
    //    JoinSystemType = "minifix"
    //};

    ////PecaCanalMinifixMaxParafuso --OK
    //var payload = new SvgRequest
    //{
    //    Width = "219000",
    //    Height = "159000",
    //    Thickness = "700",
    //    Offset = "1500",
    //    JoinSystemType = "minifix"
    //};

    //PecaRebaixoMinifixMinTambor - OK
    //var payload = new SvgRequest
    //{
    //    Width = "12000",
    //    Height = "20000",
    //    Thickness = "700",
    //    Offset = "0",
    //    JoinSystemType = "minifix"
    //};

    ////PecaRebaixoMinifixMaxTambor -- OK
    //var payload = new SvgRequest
    //{
    //    Width = "150000",
    //    Height = "270000",
    //    Thickness = "700",
    //    Offset = "0",
    //    JoinSystemType = "minifix",
    //    Type = "Base"
    //};

    ////PecaCanalMinifixMinTambor --OK
    //var payload = new SvgRequest
    //{
    //    Width = "12000",
    //    Height = "20000",
    //    Thickness = "700",
    //    Offset = "1500",
    //    JoinSystemType = "minifix"
    //};

    ////PecaCanalMinifixMaxTambor --OK
    //var payload = new SvgRequest
    //{
    //    Width = "150000",
    //    Height = "270000",
    //    Thickness = "700",
    //    Offset = "1500",
    //    JoinSystemType = "minifix"
    //};

    //MinifixAndDowelsParafuso-- MIN TRES FUROS
    //var payload = new SvgRequest
    //{
    //    Width = "12000",
    //    Height = "25000",
    //    Thickness = "700",
    //    Offset = "0",
    //    JoinSystemType = "minifixanddowels",
    //    Type = "Side"
    //};

    //MinifixAndDowelsParafuso-- MAX TRES FUROS
    //var payload = new SvgRequest
    //{
    //    Width = "102000",
    //    Height = "55000",
    //    Thickness = "700",
    //    Offset = "0",
    //    JoinSystemType = "minifixanddowels",
    //    Type = "Side"
    //};

    //MinifixAndDowelsParafuso-- MIN DOIS FUROS
    //var payload = new SvgRequest
    //{
    //    Width = "12000",
    //    Height = "14999",
    //    Thickness = "700",
    //    Offset = "0",
    //    JoinSystemType = "minifixanddowels",
    //    Type = "Side"
    //};

    //MinifixAndDowelsParafuso-- MAX DOIS FUROS
    //var payload = new SvgRequest
    //{
    //    Width = "102000",
    //    Height = "24999",
    //    Thickness = "700",
    //    Offset = "0",
    //    JoinSystemType = "minifixanddowels",
    //    Type = "Side"
    //};

    //MinifixAndDowelsTambor-- MAX DOIS FUROS
    //var payload = new SvgRequest
    //{
    //    Width = "102000",
    //    Height = "24999",
    //    Thickness = "700",
    //    Offset = "0",
    //    JoinSystemType = "minifixanddowels",
    //    Type = "Base"
    //};

    //MinifixAndDowelsTambor-- MIN DOIS FUROS
    //var payload = new SvgRequest
    //{
    //    Width = "12000",
    //    Height = "20000",
    //    Thickness = "700",
    //    Offset = "0",
    //    JoinSystemType = "minifixanddowels",
    //    Type = "Base"
    //};

    //MinifixAndDowelsTambor-- MIN TRES FUROS
    //var payload = new SvgRequest
    //{
    //    Width = "12000",
    //    Height = "25000",
    //    Thickness = "700",
    //    Offset = "0",
    //    JoinSystemType = "minifixanddowels",
    //    Type = "Base"
    //};

    //MinifixAndDowelsTambor-- MAX TRES FUROS
    //var payload = new SvgRequest
    //{
    //    Width = "102000",
    //    Height = "55000",
    //    Thickness = "700",
    //    Offset = "0",
    //    JoinSystemType = "minifixanddowels",
    //    Type = "Base"
    //};

    #endregion

    public static class Minifix
    {
        #region Base (Tambor)
        public static IEnumerable<XElement> GenerateBase(int width, int height, bool isLandscape)
        {
            // ----- Formula -----
            double[] x1 = CalculateBaseX1(width);
            double[] x2 = CalculateBaseX2(width);
            double[] y1 = CalculateBaseY1(height);
            double[] y2 = CalculateBaseY2(height);

            // ----- Formula Circle-----
            double[] circleX1 = CalculateBaseCircleX1(width);
            double[] circleX2 = CalculateBaseCircleX2(width);
            double[] circleY1 = CalculateBaseCircleY1(height);
            double[] circleY2 = CalculateBaseCircleY2(height);

            return
              [
                 Base(x1, y1, "minifix-base-up-right", isLandscape),
                 BaseCircle(circleX1, circleY1, "minifix-base-circle-up-right", isLandscape),
                 BaseDown(x1, y2, "minifix-base-down-right", isLandscape),
                 BaseCircle(circleX1, circleY2, "minifix-base-circle-down-right", isLandscape),
                 Base(x2, y1, "minifix-base-up-left", isLandscape),
                 BaseCircle(circleX2, circleY1, "minifix-base-circle-up-left", isLandscape),
                 BaseDown(x2, y2, "minifix-base-down-left", isLandscape),
                 BaseCircle(circleX2, circleY2, "minifix-base-circle-down-left", isLandscape),

              ];
        }

        static double[] CalculateBaseX1(int width)
        {
            return [
                0.5 * width - 239.5,
                0.5 * width + 0.5,
            ];
        }

        static double[] CalculateBaseX2(int width)
        {
            return [
                -(0.5 * width + 0.5),
                -(0.5 * width - 239.5),
            ];
        }

        static double[] CalculateBaseY1(int height)
        {
            return [
             -(0.5 * height - 320),// y1
            -(0.5 * height - 314.641),  // y2
            -(0.5 * height - 300),   // y3
            -(0.5 * height - 280),   // y4
            -(0.5 * height - 260),    // y5
            -(0.5 * height - 245.359),   // y6
            -(0.5 * height - 240)  // y7
            ];
        }

        static double[] CalculateBaseY2(int height)
        {
            return [
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
        }

        static double[] CalculateBaseCircleX1(int width)
        {
            return [
                0.5 * width - 165,
                0.5 * width - 175.0481,
                0.5 * width - 202.5,
                0.5 * width - 240,
                0.5 * width - 277.5,
                0.5 * width - 304.9517,
                0.5 * width - 315
            ];
        }

        static double[] CalculateBaseCircleX2(int width)
        {
            return [
                -(0.5 * width - 315),     // x1
                -(0.5 * width - 304.9517),  // x2
                -(0.5 * width - 277.5),  // x3
                -(0.5 * width - 240),  // x4
                -(0.5 * width - 202.5),  // x5
                -(0.5 * width - 175.0483),  // x6
                -(0.5 * width - 165)  // x7
           ];
        }

        static double[] CalculateBaseCircleY1(int height)
        {
            return [
                -(0.5 * height - 280), //y1
                -(0.5 * height - 242.5), //y2
                -(0.5 * height - 215.048), //y3
                -(0.5 * height - 205), //y4
                -(0.5 * height - 318), //y5
                -(0.5 * height - 344.951), //y6
                -(0.5 * height - 355)//y7
            ];
        }

        static double[] CalculateBaseCircleY2(int height)
        {
            return [
                0.5 * height - 460, //y1
                0.5 * height - 497.5, //y2
                0.5 * height - 524.951, //y3
                0.5 * height - 535, //y4
                0.5 * height - 422.5, //y5
                0.5 * height - 395.048, //y6
                0.5 * height - 385//y7
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

        static XElement BaseCircle(double[] x, double[] y, string name, bool isLandscape)
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

        static XElement BaseDown(double[] x, double[] y, string name, bool isLandscape)
        {
            var group = new XElement("g", new XAttribute("name", name), isLandscape ? new XAttribute("transform", "rotate(-90)") : null);

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
        public static IEnumerable<XElement> GenerateSide(int width, int height, bool isLandscape)
        {

            // ----- Formula -----
            double[] x1 = CalculateSideX1(width);
            double[] x2 = CalculateSideX2(width);
            double[] y1 = CalculateSideY1(height);
            double[] y2 = CalculateSideY2(height);

            return
              [
                Side(x1, y1, "minifix-side-up-right", isLandscape),

                //VALIDAR OS AddMinifixBaseQuadPath dos 3 comparando com o svg no vscode
                //finalizado testar o min e max
                Side(x1, y2, "minifix-side-down-right", isLandscape),
                Side(x2, y1, "minifix-side-up-left", isLandscape),
                Side(x2, y2, "minifix-side-up-left", isLandscape)

              ];
        }

        static double[] CalculateSideX1(int width)
        {
            return [
                0.5 * width - 50,
                0.5 * width - 53.34937,
                0.5 * width - 62.5,
                0.5 * width - 75,
                0.5 * width - 87.5,
                0.5 * width - 96.65063,
                0.5 * width - 100
             ];
        }

        static double[] CalculateSideX2(int width)
        {
            return [
                -0.5 * width + 100,
                -0.5 * width + 96.65063,
                -0.5 * width + 87.5,
                -0.5 * width + 75,
                -0.5 * width + 62.5,
                -0.5 * width + 53.34937,
                -0.5 * width + 50,
            ];
        }

        static double[] CalculateSideY1(int height)
        {
            return [
            -(0.5 * height - 280),
            -(0.5 * height - 267.5),
            -(0.5 * height - 258.34937),
            -(0.5 * height - 255),
            -(0.5 * height - 292.5),
            -(0.5 * height - 301.65063),
            -(0.5 * height - 305)
           ];
        }

        static double[] CalculateSideY2(int height)
        {
            return [
            0.5 * height - 460,
            0.5 * height - 472.5,
            0.5 * height - 481.65063,
            0.5 * height - 485,
            0.5 * height - 447.5,
            0.5 * height - 438.34937,
            0.5 * height - 435,
           ];
        }


        static XElement Side(double[] x, double[] y, string name, bool isLandscape)
        {
            var group = new XElement("g", new XAttribute("name", name), isLandscape ? new XAttribute("transform", "rotate(-90)") : null);

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



        #region MinifixAndDowels
        public static IEnumerable<XElement> GenerateMinifixDowelsSide(int width, int height, bool isLandscape)
        {
            var x1 = CalculateSideX1(width);
            var x2 = CalculateSideX2(width);
            var y1 = CalculateSideY1(height);
            var y3 = CalculateSideY2(height);
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
            var dowelX1 = Dowels.CalculateSideX1(width);
            var dowelX2 = Dowels.CalculateSideX2(width);

            return
             [
                Side(x1, y1, "minifix-side-up-right", isLandscape),
                Dowels.SideSinglePath(dowelX1.ToArray(), y2.ToArray(), "dowel-side-middle-right", isLandscape),
                Side(x1, y3, "minifix-side-down-right", isLandscape),

                Side(x2, y1, "minifix-side-up-left", isLandscape),
                Dowels.SideSinglePath(dowelX2.ToArray(), [..y2], "dowel-side-middle-left", isLandscape),
                Side(x2, y3, "minifix-side-up-left", isLandscape)
             ];
        }

        public static IEnumerable<XElement> GenerateMinifixDowelsBase(int width, int height, bool isLandscape)
        {
            var x1 = CalculateBaseX1(width);
            var x2 = CalculateBaseX2(width);
            var y1 = CalculateBaseY1(height);
            var y3 = CalculateBaseY2(height);

            var circleX1 = CalculateBaseCircleX1(width);
            var circleX2 = CalculateBaseCircleX2(width);
            var circleY1 = CalculateBaseCircleY1(height);
            var circleY2 = CalculateBaseCircleY2(height);

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
            var dowelX1 = Dowels.CalculateBaseX1(width);
            var dowelX2 = Dowels.CalculateBaseX2(width);

            return
             [
                 Base(x1, y1, "minifix-base-up-right", isLandscape),
                 BaseCircle(circleX1, circleY1, "minifix-base-circle-up-right", isLandscape),

                 Dowels.BaseSinglePath(dowelX1.ToArray(), y2.ToArray(), "dowel-side-middle-right", isLandscape),

                 BaseDown(x1, y3, "minifix-base-down-right", isLandscape),
                 BaseCircle(circleX1, circleY2, "minifix-base-circle-down-right", isLandscape),

                 Base(x2, y1, "minifix-base-up-left", isLandscape),
                 BaseCircle(circleX2, circleY1, "minifix-base-circle-up-left", isLandscape),

                 Dowels.BaseSinglePath(dowelX2.ToArray(), [..y2], "dowel-side-middle-left", isLandscape),
                 BaseDown(x2, y3, "minifix-base-down-left", isLandscape),
                 BaseCircle(circleX2, circleY2, "minifix-base-circle-down-left", isLandscape),

             ];
        }

        #endregion


        #region TicketPrint
        public static IEnumerable<XElement> GenerateSideForTicketView(ViewBoxData rectBox, double scale)
        {
            // Calcular as coordenadas proporcionais baseadas no rectBox
            double[] x1 = CalculateSideX1ForTicketView(rectBox);
            double[] y1 = CalculateSideY1ForTicketView(rectBox);

            double[] x2 = CalculateSideX2ForTicketView(rectBox);
            double[] y2 = CalculateSideY2ForTicketView(rectBox);

            return new[]
            {
                SideForTicketView(x1, y1, "minifix-side-up-right", rectBox.IsLandscape, scale),
                SideForTicketView(x1, y2, "minifix-side-down-right", rectBox.IsLandscape, scale),
                SideForTicketView(x2, y1, "minifix-side-up-left", rectBox.IsLandscape, scale),
                SideForTicketView(x2, y2, "minifix-side-down-left", rectBox.IsLandscape, scale)
            };
        }

        static double[] CalculateSideX1ForTicketView(ViewBoxData rectBox)
        {
            // No SVG original: 
            // - Quadrado vai de -1500 a 1500 (largura = 3000)
            // - Minifix centro em x = 1425
            // - Posição relativa: (1425 - (-1500)) / 3000 = 2925 / 3000 = 0.975

            double squareLeft = rectBox.X;
            double squareWidth = rectBox.Width;
            double minifixCenterX = squareLeft + (squareWidth * 0.975);

            // Aumentando o raio para ficar mais visível - usando 50 unidades para um quadrado de 3000
            double scaledRadius = (squareWidth / 3000.0) * 50.0;

            return new double[]
            {
            minifixCenterX + scaledRadius * 1.0,     // x[0] - ponto direito
            minifixCenterX + scaledRadius * 0.866,   // x[1] 
            minifixCenterX + scaledRadius * 0.5,     // x[2]
            minifixCenterX,                          // x[3] - centro
            minifixCenterX - scaledRadius * 0.5,     // x[4]
            minifixCenterX - scaledRadius * 0.866,   // x[5]
            minifixCenterX - scaledRadius * 1.0      // x[6] - ponto esquerdo
            };
        }

        static double[] CalculateSideY1ForTicketView(ViewBoxData rectBox)
        {
            // No SVG original:
            // - Quadrado vai de -1500 a 1500 (altura = 3000)  
            // - Minifix centro em y = -1220
            // - Posição relativa: (-1220 - (-1500)) / 3000 = 280 / 3000 = 0.0933

            double squareTop = rectBox.Y;
            double squareHeight = rectBox.Height;
            double minifixCenterY = squareTop + (squareHeight * 0.0933);

            // Aumentando o raio para ficar mais visível - usando 50 unidades para um quadrado de 3000
            double scaledRadius = (squareHeight / 3000.0) * 50.0;

            return new double[]
            {
            minifixCenterY,                          // y[0] - centro
            minifixCenterY - scaledRadius * 0.5,     // y[1] - superior
            minifixCenterY - scaledRadius * 0.866,   // y[2] - mais superior
            minifixCenterY - scaledRadius * 1.0,     // y[3] - topo
            minifixCenterY + scaledRadius * 0.5,     // y[4] - inferior  
            minifixCenterY + scaledRadius * 0.866,   // y[5] - mais inferior
            minifixCenterY + scaledRadius * 1.0      // y[6] - base
            };
        }

        static double[] CalculateSideX2ForTicketView(ViewBoxData rectBox)
        {
            // No SVG original analisando o minifix-side-up-left:
            // - Quadrado vai de -1500 a 1500 (largura = 3000)
            // - Minifix centro em x = -1425 (valor médio dos pontos do minifix)
            // - Posição relativa: (-1425 - (-1500)) / 3000 = 75 / 3000 = 0.025

            double squareLeft = rectBox.X;
            double squareWidth = rectBox.Width;
            double minifixCenterX = squareLeft + (squareWidth * 0.025);

            // Usando o mesmo raio proporcional que no método x1
            double scaledRadius = (squareWidth / 3000.0) * 50.0;

            return new double[]
            {
        minifixCenterX + scaledRadius * 1.0,     // x[0] - ponto direito
        minifixCenterX + scaledRadius * 0.866,   // x[1] 
        minifixCenterX + scaledRadius * 0.5,     // x[2]
        minifixCenterX,                          // x[3] - centro
        minifixCenterX - scaledRadius * 0.5,     // x[4]
        minifixCenterX - scaledRadius * 0.866,   // x[5]
        minifixCenterX - scaledRadius * 1.0      // x[6] - ponto esquerdo
            };
        }

        static double[] CalculateSideY2ForTicketView(ViewBoxData rectBox)
        {
            // No SVG original analisando o minifix-side-up-left:
            // - Quadrado vai de -1500 a 1500 (altura = 3000)  
            // - Minifix centro em y = 1040 (valor médio dos pontos do minifix)
            // - Posição relativa: (1040 - (-1500)) / 3000 = 2540 / 3000 = 0.8467

            double squareTop = rectBox.Y;
            double squareHeight = rectBox.Height;
            double minifixCenterY = squareTop + (squareHeight * 0.8467);

            // Usando o mesmo raio proporcional que no método y1
            double scaledRadius = (squareHeight / 3000.0) * 50.0;

            return new double[]
            {
        minifixCenterY,                          // y[0] - centro
        minifixCenterY - scaledRadius * 0.5,     // y[1] - superior
        minifixCenterY - scaledRadius * 0.866,   // y[2] - mais superior
        minifixCenterY - scaledRadius * 1.0,     // y[3] - topo
        minifixCenterY + scaledRadius * 0.5,     // y[4] - inferior  
        minifixCenterY + scaledRadius * 0.866,   // y[5] - mais inferior
        minifixCenterY + scaledRadius * 1.0      // y[6] - base
            };
        }

        static XElement SideForTicketView(double[] x, double[] y, string name, bool isLandscape, double scale)
        {
            var group = new XElement("g",
                new XAttribute("name", name),
                isLandscape ? new XAttribute("transform", "rotate(-90)") : null);

            // Ajustar a espessura do stroke baseada na escala
            double strokeWidth = Math.Max(0.1, 4 * scale);

            // Criar os paths individuais do minifix (12 segmentos)
            AddMinifixBaseQuadPathForTicketView(group, x[0], y[0], x[1], y[1], strokeWidth);
            AddMinifixBaseQuadPathForTicketView(group, x[1], y[1], x[2], y[2], strokeWidth);
            AddMinifixBaseQuadPathForTicketView(group, x[2], y[2], x[3], y[3], strokeWidth);
            AddMinifixBaseQuadPathForTicketView(group, x[3], y[3], x[4], y[2], strokeWidth);
            AddMinifixBaseQuadPathForTicketView(group, x[4], y[2], x[5], y[1], strokeWidth);
            AddMinifixBaseQuadPathForTicketView(group, x[5], y[1], x[6], y[0], strokeWidth);
            AddMinifixBaseQuadPathForTicketView(group, x[6], y[0], x[5], y[4], strokeWidth);
            AddMinifixBaseQuadPathForTicketView(group, x[5], y[4], x[4], y[5], strokeWidth);
            AddMinifixBaseQuadPathForTicketView(group, x[4], y[5], x[3], y[6], strokeWidth);
            AddMinifixBaseQuadPathForTicketView(group, x[3], y[6], x[2], y[5], strokeWidth);
            AddMinifixBaseQuadPathForTicketView(group, x[2], y[5], x[1], y[4], strokeWidth);
            AddMinifixBaseQuadPathForTicketView(group, x[1], y[4], x[0], y[0], strokeWidth);

            // Criar o path completo do círculo
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

            AddDrawColorCircleForTicketView(group, points, strokeWidth);

            return group;
        }

        static void AddMinifixBaseQuadPathForTicketView(XElement group, double x1, double y1, double x2, double y2, double strokeWidth)
        {
            var ci = CultureInfo.InvariantCulture;
            string d = $"M {x1.ToString("0.#####", ci)} {y1.ToString("0.#####", ci)} " +
                      $"L {x2.ToString("0.#####", ci)} {y2.ToString("0.#####", ci)} " +
                      $"L {x2.ToString("0.#####", ci)} {y2.ToString("0.#####", ci)} " +
                      $"L {x1.ToString("0.#####", ci)} {y1.ToString("0.#####", ci)} Z";

            group.Add(new XElement("path",
                new XAttribute("d", d),
                new XAttribute("style", $"fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:{strokeWidth.ToString("0.##", ci)};"),
                new XAttribute("stroke", "black")
            ));
        }

        static void AddDrawColorCircleForTicketView(XElement group, (double X, double Y)[] points, double strokeWidth)
        {
            var ci = CultureInfo.InvariantCulture;
            var d = "M " + string.Join(" L ", points.Select(p =>
                $"{p.X.ToString("0.#####", ci)} {p.Y.ToString("0.#####", ci)}"
            )) + " Z";

            group.Add(new XElement("path",
                new XAttribute("d", d),
                new XAttribute("style", $"fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:{strokeWidth.ToString("0.##", ci)};"),
                new XAttribute("stroke", "black")
            ));
        }






        #endregion

    }
}
