using System.Xml.Linq;

namespace testesSvg
{
    public static class Label
    {
        public static XElement CreateLabelGroup(int width, int height)
        {
            var group = new XElement("g", new XAttribute("name", "label-left"));

            // Cálculo da posição do texto (do lado esquerdo)
            double labelX = -Math.Ceiling((width / 100) * 0.0606);
            double labelY = -((height / 10) - 70);

            // Valor que será exibido no label
            int labelValue = width / 10;

            // Cria o elemento <text>
            var textElement = new XElement("text",
                new XAttribute("x", labelX),
                new XAttribute("y", labelY),
                new XAttribute("font-size", "120"),
                new XAttribute("fill", "black"),
                new XAttribute("text-anchor", "middle"),
                new XAttribute("dominant-baseline", "middle"),
                new XAttribute("transform", "rotate(-90)"),
                labelValue.ToString()
            );

            group.Add(textElement);

            return group;
        }

        public static XElement CreateTopLabelGroup(int width, int height)
        {
            var group = new XElement("g", new XAttribute("name", "label-top"));

            // Cálculo da posição do texto (acima do retângulo)
            double labelX = -Math.Ceiling((width / 100) * 0.0909);
            double labelY = -Math.Ceiling((height / 10.0) * 0.54); // Posição acima do topo (-3500 - 70)

            // Valor que será exibido no label
            int labelValue = height / 10; // 3500

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
    }
}
