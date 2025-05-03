using System.Globalization;
using System.Xml.Linq;

namespace testesSvg.Components
{
    public static class Dowels
    {
        #region min
        //X1 min (width 3000)
        //1465
        //1.459.641
        //1445
        //1425
        //1405
        //1.390.359
        //1385

        //y1 min (height 30000)
        //-1220
        //-1240
        //-1.254.641
        //-1260
        //-1200
        //-1.185.359
        //-1180

        //x2 min
        //-1385
        //-1.390.359
        //-1405
        //-1425
        //-1445
        //-1.459.641
        //-1465

        //y2 min
        //1040
        //1020
        //1.005.359
        //1000
        //1060
        //1.074.641
        //1080

        #endregion

        #region max
        //x1 max (width 21900)
        //10915
        //10.909.641
        //10895
        //10875
        //10855
        //10.840.358
        //10835
        //10.840.359
        //10.909.642

        //y1 max (height 15900)
        //-7630
        //-7650
        //-76.646.416
        //-76.646.406
        //-7670
        //-7610
        //-75.953.594
        //-7.595.359
        //-7590

        //x2 max
        //-10835
        //-10840.359
        //-10855
        //-10875
        //-10895
        //-10909.642
        //-10915
        //-10909.641
        //-10840.358

        //y2 max
        //-2.4041262E-06
        //-20.000004
        //-19.999998
        //-34.64102
        //-34.641018
        //-40.000004
        //-39.999996
        //-34.641014
        //-19.999996
        //-19.99999
        //1.0927848E-06
        //5.9010376E-06
        //20.000006
        //20.000011
        //34.641006
        //19.999989
        //19.999994

        //y3 max 
        //7470
        //7450
        //7435.359
        //7430
        //7490
        //7504.6406
        //7504.6416
        //7510
        #endregion



        public static IEnumerable<XElement> GenerateSide(int width, int height)
        {
            // ----- Formula -----
            double[] x1 =
            [
                0.5 * width - 35,     // x1
                0.5 * width - 40.359,  // x2
                0.5 * width - 55,     // x3
                0.5 * width -75,        // x4
                0.5 * width -95,      // x5
                0.5 * width -109.641,   // x6
                0.5 * width -115,     // x7
            ];


            var a = Math.Sqrt(x1[0] * 1309.6);
            var b = Math.Sqrt(x1[1] * 1309.6);


            double[] y1 =
            [
              -(-0.01399 * height + 1261.97), // y1
              -(-0.01705 * height + 1291.15),  // y2
              -(-0.01932 * height + 1312.56),   // y3
              -(-0.02016 * height + 1320.48),   // y4
              -(-0.00617 * height + 1218.52),    // y5
              -(-0.00359 * height + 1196.13),   // y6
              -(-0.00278 * height + 1188.33)   // y7
            ];


            //PENDENTE CORRIGIR
            double[] x2 =
            [
                0.5 * width - 115,     // x1
                -(0.5 * width - 40.359),  // x2
                -(0.5 * width - 55),     // x3
                -(0.5 * width -75),        // x4
                -(0.5 * width -95),      // x5
                -(0.5 * width -109.641),   // x6
                -(0.5 * width -115),     // x7
            ];

            double[] y2 =
            [
               -0.01399 * height + 1261.97, // y1
              -0.01705 * height + 1291.15,  // y2
              -0.01932 * height + 1312.56,   // y3
              -0.02016 * height + 1320.48,   // y4
              -0.00617 * height + 1218.52,    // y5
              -0.00359 * height + 1196.13,   // y6
              -0.00278 * height + 1188.33   // y7
            ];
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
                   Side(x1, y1, "dowel-side-up-right"),
                   Side(x1, y2, "dowel-side-down-right"),
                   Side(x2, y1, "dowel-side-up-left"),
                   Side(x2, y2, "dowel-side-down-left")
                ];
            //}


            //return null;

            //double[] yLeft1 = new double[yRight.Length];

            //for (int i = 0; i < yRight.Length; i++)
            //{
            //    double rightValue = yRight[i];

            //    // Passo 1: Extrair o coeficiente da fórmula original
            //    double coefficientRight = Math.Abs(rightValue / height);

            //    // Passo 2: Aplicar uma regra para espelhar o coeficiente
            //    double coefficientLeft = 1.0 - coefficientRight;

            //    // Ajuste fino opcional (se quiser maior controle visual):
            //    coefficientLeft *= 0.85; // reduz um pouco para compensar proporção visual

            //    // Passo 3: Recalcular com o novo coeficiente e inverter o sinal
            //    yLeft1[i] = coefficientLeft * height + ((rightValue % 1) != 0 ? 0.359 : -0.5);
            //}

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
}
