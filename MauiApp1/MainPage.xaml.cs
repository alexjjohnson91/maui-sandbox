using MauiApp1.ViewModels;

namespace MauiApp1;

public partial class MainPage
{

    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainPageViewModel();
    }


    private void Scan_Receipt_OnClicked(object? sender, EventArgs e)
    {
        ScanReceipt.IsVisible = false;
        RecognizedText.IsVisible = true;
    }
}
