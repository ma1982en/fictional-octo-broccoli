using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml;
using Prism.Commands;
using Prism.Mvvm;
using WiFiScanner.Model;

namespace WiFiScanner.ViewModels
{
    public class WifiScannerViewModel : BindableBase
    {
        public ObservableCollection<CurrentWiFiNetwork> ScannedNetworks { get; set; }
        public ICommand ScanCommand { get; set; }
        public ICommand StopCommand { get; set; }
        public DispatcherTimer Timer { get; set; }

        public WifiScannerViewModel()
        {
            ScannedNetworks = new ObservableCollection<CurrentWiFiNetwork>();
            ScanCommand = new DelegateCommand(OnStartScan);
            StopCommand = new DelegateCommand(OnStopScan);
            _wifimanager = new WiFiScanManager(ScannedNetworks);
            Timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 5) };
            Timer.Tick += OnTimerElapsed;
        }

        private void OnTimerElapsed(object sender, object o)
        {
            _wifimanager.StartRequestCurrentNetworksAsync();
        }

        private void OnStartScan()
        {
            _wifimanager.SetAdapter();
            Timer.Start();
        }

        private void OnStopScan()
        {
            Timer.Stop();
        }

        private readonly WiFiScanManager _wifimanager;
    }
}