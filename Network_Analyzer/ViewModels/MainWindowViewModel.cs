using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.NetworkInformation;
using Network_Analyzer.Models;

namespace Network_Analyzer.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
    {
        // Коллекция для хранения информации о сетевых интерфейсах
        public ObservableCollection<NetworkInterfaceInfo> Interfaces { get; set; }
        
        // Выбранный интерфейс, при изменении которого обновляется детальная информация
        private NetworkInterfaceInfo _selectedInterface;
        public NetworkInterfaceInfo SelectedInterface
        {
            get => _selectedInterface;
            set
            {
                if (_selectedInterface != value)
                {
                    _selectedInterface = value;
                    OnPropertyChanged(nameof(SelectedInterface));
                    UpdateSelectedInterfaceInfo();
                }
            }
        }

        // Свойство, содержащее подробную информацию о выбранном интерфейсе,
        // которое будет привязано к соответствующему элементу UI
        private string _interfaceDetails;
        public string InterfaceDetails
        {
            get => _interfaceDetails;
            set
            {
                if (_interfaceDetails != value)
                {
                    _interfaceDetails = value;
                    OnPropertyChanged(nameof(InterfaceDetails));
                }
            }
        }

        // Конструктор, который инициализирует коллекцию и загружает данные об интерфейсах
        public MainWindowViewModel()
        {
            Interfaces = new ObservableCollection<NetworkInterfaceInfo>();
            GetNetworkInterfaces();
        }

        // Метод для получения списка всех сетевых интерфейсов и заполнения коллекции
        private void GetNetworkInterfaces()
        {
            foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                var ipProperties = adapter.GetIPProperties();
                var unicast = ipProperties.UnicastAddresses
                    .FirstOrDefault(ip => ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                
                string ipv4Address = unicast?.Address.ToString() ?? "N/A";
                string subnetMask = unicast?.IPv4Mask.ToString() ?? "N/A";

                Interfaces.Add(new NetworkInterfaceInfo
                {
                    Name = adapter.Name,
                    IPAddress = ipv4Address,
                    SubnetMask = subnetMask,
                    MacAddress = adapter.GetPhysicalAddress()?.ToString() ?? "N/A",
                    Status = adapter.OperationalStatus.ToString(),
                    Speed = adapter.Speed.ToString(),
                    InterfaceType = adapter.NetworkInterfaceType.ToString()
                });
            }
        }

        // Метод обновляет свойство InterfaceDetails на основе выбранного интерфейса
        private void UpdateSelectedInterfaceInfo()
        {
            if (SelectedInterface != null)
            {
                InterfaceDetails = $"IP: {SelectedInterface.IPAddress}\n" +
                                   $"Subnet: {SelectedInterface.SubnetMask}\n" +
                                   $"MAC: {SelectedInterface.MacAddress}\n" +
                                   $"Status: {SelectedInterface.Status}\n" +
                                   $"Speed: {SelectedInterface.Speed}\n" +
                                   $"Type: {SelectedInterface.InterfaceType}";
            }
            else
            {
                InterfaceDetails = "No interface selected.";
            }
        }

        // Реализация INotifyPropertyChanged для поддержки привязки данных в UI
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }