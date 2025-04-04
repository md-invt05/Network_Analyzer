using System.Windows;
using System.Windows.Controls;

namespace Network_Analyzer.Views;

public partial class URLAnalyzerViewPage : Page
{
    public URLAnalyzerViewPage()
    {
        InitializeComponent();
    }

    private void OnBackToMainViewModelPage(object sender, RoutedEventArgs e)
    {
        var frame = (Frame)Window.GetWindow(this).FindName("MainFrame");
        frame.GoBack();
    }
}