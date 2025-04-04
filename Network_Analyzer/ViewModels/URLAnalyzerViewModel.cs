using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Network_Analyzer.Services;

namespace Network_Analyzer.ViewModels;

public class UrlAnalyzerViewModel : INotifyPropertyChanged
    {
        private readonly PingService _pingService;
        private readonly DnsService _dnsService;
        private readonly AddresService _addressService;
        private readonly URLHistoryService _urlHistoryService;

        public UrlAnalyzerViewModel()
        {
            _pingService = new PingService();
            _dnsService = new DnsService();
            _addressService = new AddresService();
            _urlHistoryService = new URLHistoryService();

            // Инициализируем команду с асинхронным методом анализа URL
            AnalyzeCommand = new RelayCommandService(async _ => await AnalyzeUrlAsync(), _ => !string.IsNullOrWhiteSpace(Url));
        }

        // Входной URL для анализа
        private string _url;
        public string Url
        {
            get => _url;
            set
            {
                if (_url != value)
                {
                    _url = value;
                    OnPropertyChanged(nameof(Url));
                }
            }
        }

        // Разобранные компоненты URL
        private string _scheme;
        public string Scheme
        {
            get => _scheme;
            set { _scheme = value; OnPropertyChanged(nameof(Scheme)); }
        }

        private string _host;
        public string Host
        {
            get => _host;
            set { _host = value; OnPropertyChanged(nameof(Host)); }
        }

        private string _port;
        public string Port
        {
            get => _port;
            set { _port = value; OnPropertyChanged(nameof(Port)); }
        }

        private string _path;
        public string Path
        {
            get => _path;
            set { _path = value; OnPropertyChanged(nameof(Path)); }
        }

        private string _query;
        public string Query
        {
            get => _query;
            set { _query = value; OnPropertyChanged(nameof(Query)); }
        }

        private string _fragment;
        public string Fragment
        {
            get => _fragment;
            set { _fragment = value; OnPropertyChanged(nameof(Fragment)); }
        }

        // Результаты проверки пинга, DNS и типа адреса
        private string _pingResult;
        public string PingResult
        {
            get => _pingResult;
            set { _pingResult = value; OnPropertyChanged(nameof(PingResult)); }
        }

        private string _dnsInfo;
        public string DnsInfo
        {
            get => _dnsInfo;
            set { _dnsInfo = value; OnPropertyChanged(nameof(DnsInfo)); }
        }

        private string _addressType;
        public string AddressType
        {
            get => _addressType;
            set { _addressType = value; OnPropertyChanged(nameof(AddressType)); }
        }

        // Команда для анализа URL
        public ICommand AnalyzeCommand { get; }

        // История URL из URLHistoryService (ObservableCollection обновляет UI при изменении)
        public ObservableCollection<string> UrlHistory => _urlHistoryService.History;

        // Асинхронный метод анализа URL
        private async Task AnalyzeUrlAsync()
        {
            if (Uri.TryCreate(Url, UriKind.Absolute, out Uri uri))
            {
                // Добавляем URL в историю
                _urlHistoryService.AddUrlToHistory(Url);

                // Разбираем URL
                Scheme = uri.Scheme;
                Host = uri.Host;
                Port = uri.Port.ToString();
                Path = uri.AbsolutePath;
                Query = uri.Query;
                Fragment = uri.Fragment;

                // Выполняем асинхронный пинг хоста
                bool isReachable = await _pingService.PingHostAsync(Host);
                PingResult = isReachable ? "Host is reachable" : "Host is unreachable";

                // Получаем информацию DNS
                DnsInfo = _dnsService.GetDnsInfo(Host);

                // Определяем тип IP-адреса
                AddressType = _addressService.GetAddressType(Host);
            }
            // Можно добавить обработку случая, когда URL некорректен.
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }