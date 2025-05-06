using System.Xml.Linq;

namespace testesSvg.Components
{
    public static class Thickness
    {
        #region testes
        ////PecaRebaixoSemOffset --OK
        //var payload = new SvgRequest
        //{
        //    Width = "12000",
        //    Height = "20000",
        //    Thickness = "700"
        //};

        ////PecaRebaixoMinimo --OK
        //var payload = new SvgRequest
        //{
        //    Width = "12000",
        //    Height = "20000",
        //    Thickness = "700",
        //    Offset = "0",
        //};

        ////PecaRebaixoMaximo--OK
        //var payload = new SvgRequest
        //{
        //    Width = "219000",
        //    Height = "100000",
        //    Thickness = "700",
        //    Offset = "0"
        //};

        ////PecaCanalMinimo --OK
        //var payload = new SvgRequest
        //{
        //    Width = "30000",
        //    Height = "30000",
        //    Thickness = "700",
        //    Offset = "1500",
        //};

        ////PecaCanalMaximo --OK
        //var payload = new SvgRequest
        //{
        //    Width = "219000",
        //    Height = "159000",
        //    Thickness = "700",
        //    Offset = "1500",
        //};
        #endregion 

        public static XElement CreateThickness(int w, int h, int x, int? offsetFromEnd, int dadoThickness)
        {
            var group = new XElement("g", new XAttribute("name", "thickness"));

            int highlightHeight = dadoThickness / 10;
            double highlightY;

            if (offsetFromEnd != null)
            {
                int offset = offsetFromEnd == 0 ? 0 : (int)(offsetFromEnd / 10.0 * 1.0);
                highlightY = h / 20 - highlightHeight - offset;
            }
            else
            {
                highlightY = h / 20 - highlightHeight - 160;
            }

            //validar se quando é zero no offset se tem uma correcao de +1 no highlightY e +2 highlightY + highlightHeight (rebaixo minimo e maximo)

            var coords = new[]
            {
            [x - 2, highlightY, x + w + 2, highlightY, x + w + 2, highlightY + highlightHeight, x - 2, highlightY + highlightHeight],
            [x - 2, highlightY, x + w + 2, highlightY, x + w + 2, highlightY + highlightHeight, x - 2, highlightY + highlightHeight],
            [x - 2, highlightY, x + w + 2, highlightY, x + w + 2, highlightY, x - 2, highlightY],
            [x - 2, highlightY + highlightHeight, x + w + 2, highlightY + highlightHeight, x + w + 2, highlightY + highlightHeight, x - 2, highlightY + highlightHeight],
            [x + w + 2, highlightY, x + w + 2, highlightY + highlightHeight, x + w + 2, highlightY + highlightHeight, x + w + 2, highlightY],
            new[] { x - 2, highlightY, x - 2, highlightY + highlightHeight, x - 2, highlightY + highlightHeight, x - 2, highlightY }
        };

            foreach (var path in coords)
            {
                group.Add(new XElement("path",
                    new XAttribute("d", $"M {path[0]} {path[1]} L {path[2]} {path[3]} L {path[4]} {path[5]} L {path[6]} {path[7]} Z"),
                    new XAttribute("style", "fill:red;fill-opacity:0.4;stroke-linejoin:round;stroke-width:4;"),
                    new XAttribute("stroke", "black")
                ));
            }

            return group;
        }
    }
}
