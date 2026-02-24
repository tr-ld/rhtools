namespace RHWebFront.Services;

public class SelectedSymbolManager : ISelectedSymbolManager
{
    public string SelectedSymbol { get; set; }
    public bool ShowSymbolSelector { get; set; } = true;

    public void ClearSelection()
    {
        SelectedSymbol = null;
        ShowSymbolSelector = true;
    }
}