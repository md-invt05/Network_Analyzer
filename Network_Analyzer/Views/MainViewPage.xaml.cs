using System.Windows;
using System.Windows.Controls;

namespace Network_Analyzer.Views;

public partial class MainViewPage : Page
{
    public MainViewPage()
    {
        InitializeComponent();
    }

    private void OnNavigateToUrlAnalyzerPage(object sender, RoutedEventArgs e)
    {
        var urlAnalyzerViewPage = new UrlAnalyzerViewPage(); 
        var frame = (Frame)Window.GetWindow(this).FindName("MainFrame");
        frame.Navigate(UrlAnalyzerViewPage);
    }
}