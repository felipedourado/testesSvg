using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace testesSvg
{
    public class SvgGenerate
    {

        public string GerarSvg(SvgRequest request)
        {
            //payload
            //tipo da board
            //dados do canal (dadoThickness)
            //component type (side, door, base) (door é dobradiça, side é o tipo da fixação se é parafuso, base é o tipo da fixação tambor)
            //HingeSku - obrigatorio no door (faz dobradiça)
            //offset (a altura informa se é canal ou rebaixo)??
            //JoinSystemType - N tipos de furação
            //fita de borda (4 lados)

            //obs:
            //remover o fundo cinza
            //gerar svg sem board, sem furação e sem usinagem apenas com plano de corte
            //adicionar label de indicação da peça


            ValidateBoard(request.Board);
            GetSizes(request.Board, request.Width, request.Height);
            //ValidateThickness();


            // Criando o SVG com os elementos fixos
            //var svg = new XElement("svg",
            //    new XAttribute("viewBox", $"{viewBoxX} {viewBoxY} {viewBoxWidth} {viewBoxHeight}"),
            //    new XAttribute("width", viewBoxWidth),
            //    new XAttribute("height", viewBoxHeight),


            return string.Empty;
        }

        public void ValidateBoard(string board)
        {
            //ainda está obscuro como funciona
            //validar regra dos tipos de boards e boards que sao permitidos
            //validar altura e largura para cada tipo de board

            //board 15

        }

        public void validateComponentType()
        {
            //fixacao tipo parafuso (side) height e width maximo é de: 350000, width e height minimo é de 1
            //fixacao tipo tambor (base) height máximo é de: 270000, width máximo é de: 150000, width mínimo é de 12000 e height é de 20000
            //fixacao tipo dobradiça (door) height máximo é de: 270000, width máximo é de: 100000, width mínimo é de 20000 e height é de 30000

            //se uma peça tiver fixação parafuso a outra será tambor, se a peça tiver tambor a outra será parafuso,
            //as peças são amarradas pois dependem uma da outra - automaticamente é gerada outra peça com o mesmo valor de Thickness (canal ou rebaixo)
        }
        public void GetSizes(string board, string width, string height)
        {

        }

        public void Thickness()
        {
            //Valor nao pode ser inferior a 600 e nem superior a 2700
        }

        public XElement CreateBorder(int w, int h, int x, int y, bool top, bool right, bool bottom, bool left)
        {
            var group = new XElement("g");

            // Estilo padrão
            string defaultStyle = "fill:none;stroke:#666;stroke-width:10;stroke-linejoin:round;stroke-dasharray:none;";
            // Estilo em destaque
            string highlightStyle = "fill:none;stroke:#000000;stroke-width:30;stroke-linejoin:round;";

            // Top border
            group.Add(new XElement("line",
                new XAttribute("x1", x),
                new XAttribute("y1", y),
                new XAttribute("x2", x + w),
                new XAttribute("y2", y),
                new XAttribute("style", top ? highlightStyle : defaultStyle)
            ));

            // Right border
            group.Add(new XElement("line",
                new XAttribute("x1", x + w),
                new XAttribute("y1", y),
                new XAttribute("x2", x + w),
                new XAttribute("y2", y + h),
                new XAttribute("style", right ? highlightStyle : defaultStyle)
            ));

            // Bottom border
            group.Add(new XElement("line",
                new XAttribute("x1", x + w),
                new XAttribute("y1", y + h),
                new XAttribute("x2", x),
                new XAttribute("y2", y + h),
                new XAttribute("style", bottom ? highlightStyle : defaultStyle)
            ));

            // Left border
            group.Add(new XElement("line",
                new XAttribute("x1", x),
                new XAttribute("y1", y + h),
                new XAttribute("x2", x),
                new XAttribute("y2", y),
                new XAttribute("style", left ? highlightStyle : defaultStyle)
            ));

            return group;
        }
    } 
}
