using System.Xml.Linq;

namespace testesSvg
{
    public static class Label
    {
        public static XElement Height(int width, int height)
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

        public static XElement Width(int width, int height)
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

        public static XElement Footer(int height)
        {
            var group = new XElement("g", new XAttribute("name", "footer-label"));

            // Posição centralizada horizontalmente e posicionada abaixo do conteúdo principal
            double labelX = 0;           // Centralizado horizontalmente
            double labelY = height / 10 - 100;        // Ajuste conforme necessário

            // Cria o elemento <text>
            var textElement = new XElement("text",
                new XAttribute("x", labelX),
                new XAttribute("y", labelY),
                new XAttribute("font-size", "120"),
                new XAttribute("fill", "black"),
                new XAttribute("text-anchor", "middle"),
                new XAttribute("dominant-baseline", "middle"),
                "Face de Alinhamento"
            );

            group.Add(textElement);

            return group;
        }
    }
}
