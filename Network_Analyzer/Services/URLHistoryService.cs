using System.Collections.ObjectModel;

namespace Network_Analyzer.Services;

public class URLHistoryService
{
    public ObservableCollection<string> History { get; } = new ObservableCollection<string>();
    
    public void AddUrlToHistory(string url)
    {
        if (!string.IsNullOrWhiteSpace(url))
        {
            History.Insert(0, url);
        }
    }
}