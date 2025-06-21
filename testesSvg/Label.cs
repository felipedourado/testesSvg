using System.Xml.Linq;

namespace testesSvg
{
    public static class Label
    {
        public static XElement HeightSystemType(int width, int height)
        {
            var group = new XElement("g", new XAttribute("name", "label-height"));

            // Cálculo da posição do texto (do lado esquerdo)
            //double labelX = -Math.Ceiling((width / 100) * 0.0606);
            //double labelY = -((height / 10) - 70);

            double labelX = -(width / 10) / 2 - 80; // 80 unidades à esquerda da borda
            double labelY = 0; // centro vertical

            // Valor que será exibido no label
            int labelValue = height / 10;

            // Cria o elemento <text>
            var textElement = new XElement("text",
                new XAttribute("x", labelX),
                new XAttribute("y", labelY),
                new XAttribute("font-size", "120"),
                new XAttribute("fill", "black"),
                new XAttribute("text-anchor", "middle"),
                new XAttribute("dominant-baseline", "middle"),
                new XAttribute("transform", $"rotate(-90 {labelX} 0)"),
                labelValue.ToString()
            );

            group.Add(textElement);

            return group;
        }

        public static XElement WidthSystemType(int width, int height)
        {
            var group = new XElement("g", new XAttribute("name", "label-width"));

            // Cálculo da posição do texto (acima do retângulo)
            double labelX = -Math.Ceiling((width / 100) * 0.0909);
            double labelY = -Math.Ceiling((height / 10.0) * 0.54);

            // Valor que será exibido no label
            int labelValue = width / 10; // 3500

            // Cria o elemento <text>
            var textElement = new XElement("text",
                new XAttribute("x", labelX),
                new XAttribute("y", labelY),
                new XAttribute("font-size", "120"),
                new XAttribute("fill", "black"),
                new XAttribute("text-anchor", "middle"),
                new XAttribute("dominant-baseline", "middle"),
                labelValue.ToString()
            );

            group.Add(textElement);

            return group;
        }

        public static XElement Footer(int viewBoxHeight, int y)
        {
            var group = new XElement("g", new XAttribute("name", "footer-label"));

            int fontSize = Math.Max(8, viewBoxHeight / 30);

            // Posição centralizada horizontalmente e posicionada abaixo do conteúdo principal
            double labelX = 0;

            // Centralizado horizontalmente
            int marginDistance = (int)(viewBoxHeight * 0.02);

            double labelY = (y + viewBoxHeight) - marginDistance;

            // Cria o elemento <text>
            var textElement = new XElement("text",
                new XAttribute("x", labelX),
                new XAttribute("y", labelY),
                new XAttribute("font-size", fontSize),
                new XAttribute("fill", "black"),
                new XAttribute("text-anchor", "middle"),
                new XAttribute("dominant-baseline", "middle"),
                "Face de Alinhamento"
            );

            group.Add(textElement);

            return group;
        }

        public static XElement CreateTopWidthText(int rectX, int rectY, int width, int height, int viewBoxWidth, int viewBoxHeight, bool isLandscape)
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

        // Versão alternativa mais configurável
        //static XElement CreateTopWidthTextConfigurable(int rectX, int rectY, int width, int height,
        //    int viewBoxWidth, int viewBoxHeight, bool isLandscape,
        //    int? customFontSize = null, int? customMargin = null)
        //{
        //    string text = isLandscape ? height.ToString() : width.ToString();

        //    // Usar valores customizados ou padrões
        //    int fontSize = customFontSize ?? 2;
        //    int textMargin = customMargin ?? 2;

        //    int textX, textY;

        //    if (isLandscape)
        //    {
        //        int centerX = viewBoxWidth / 2;
        //        int centerY = viewBoxHeight / 2;

        //        textX = centerX;
        //        textY = centerY - (width / 2) - textMargin;
        //    }
        //    else
        //    {
        //        textX = rectX + (width / 2);
        //        textY = rectY - textMargin;
        //    }

        //    return new XElement("g",
        //        new XAttribute("name", "top-width-label"),
        //        new XElement("text",
        //            new XAttribute("x", textX),
        //            new XAttribute("y", textY),
        //            new XAttribute("text-anchor", "middle"),
        //            new XAttribute("dominant-baseline", "bottom"),
        //            new XAttribute("font-family", "Arial, sans-serif"),
        //            new XAttribute("font-size", fontSize),
        //            new XAttribute("fill", "#666666"),
        //            text
        //        )
        //    );
        //}
    }
}
