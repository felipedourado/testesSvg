namespace testesSvg;

public class SvgRequest
{
    public string Type { get; set; }
    public string Board { get; set; }
    public string Width { get; set; }
    public string Height { get; set; }
    public string Thickness { get; set; }
    public string HingeSku { get; set; }
    public string JoinSystemType { get; set; }
    public string Offset { get; set; }
    public bool IsBorderTop { get; set; }
    public bool IsBorderBottom { get; set; }
    public bool IsBorderLeft { get; set; }
    public bool IsBorderRight { get; set; }
    public bool IsLandscape { get; set; }

    public string WidthLabel { get; set; }
    public string HeightLabel { get; set; }
    public bool IsTicket { get; set; }
    public string BorderTopLabel { get; set; }
    public string BorderBottomLabel { get; set; }
    public string BorderLeftLabel { get; set; }
    public string BorderRightLabel { get; set; }

    public bool IsAnyBorderValid() => IsBorderBottom || IsBorderTop || IsBorderLeft || IsBorderRight;
    
}
