namespace RHWebFront.Services;

public interface ISelectedSymbolManagementService
{
    string SelectedSymbol { get; set; }
    bool ShowSymbolSelector { get; set; }
    void ClearSelection();
}