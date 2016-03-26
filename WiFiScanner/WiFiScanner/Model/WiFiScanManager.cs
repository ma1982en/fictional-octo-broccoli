using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.WiFi;

namespace WiFiScanner.Model
{
    public class WiFiScanManager
    {
        private readonly ObservableCollection<CurrentWiFiNetwork> _currentWiFiNetworks;
        private WiFiAdapter _adapter;

        public WiFiScanManager(ObservableCollection<CurrentWiFiNetwork> scannedNetworks)
        {
            _currentWiFiNetworks = scannedNetworks;
        }

        public async void StartRequestCurrentNetworksAsync()
        {
            if (_adapter == null)
                return;

            await _adapter.ScanAsync();

            FindAllNetworks(_adapter);
        }

        public async void SetAdapter()
        {
            _adapter = await GetAdapter();
        }

        private static async Task<WiFiAdapter> GetAdapter()
        {
            var access = await WiFiAdapter.RequestAccessAsync();

            if (access != WiFiAccessStatus.Allowed)
            {
                Debug.WriteLine("Access denied");
            }
            else
            {
                var result = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
                if (result.Count >= 1)
                    return await WiFiAdapter.FromIdAsync(result[0].Id);
                else
                    Debug.WriteLine("No WiFi Adapters detected on this machine.");
            }

            return null;
        }

        private  void FindAllNetworks(WiFiAdapter wifiAdapter)
        {
            _currentWiFiNetworks.Clear();
            foreach (var currentWiFiNetwork in wifiAdapter
                .NetworkReport
                .AvailableNetworks
                .OrderByDescending( x => x.NetworkRssiInDecibelMilliwatts)
                .ToList().Select(network => new CurrentWiFiNetwork
                {
                    Ssid = network.Ssid,
                    MacAddress = network.Bssid,
                    Leistung = network.NetworkRssiInDecibelMilliwatts.ToString(CultureInfo.InvariantCulture),
                    UpTime = network.Uptime.ToString(),
                    BeaconIntervall = network.BeaconInterval.ToString(),
                    ChannelFrequence = network.ChannelCenterFrequencyInKilohertz.ToString()
                   
                }))
            {
                _currentWiFiNetworks.Add(currentWiFiNetwork);
            }
        }

       
    }
}