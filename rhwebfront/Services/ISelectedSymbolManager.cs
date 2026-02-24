namespace RHWebFront.Services;

public interface ISelectedSymbolManager
{
    string SelectedSymbol { get; set; }
    bool ShowSymbolSelector { get; set; }
    void ClearSelection();
}