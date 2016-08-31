using System;
using System.Collections.Generic;
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
        private ObservableCollection<CurrentWiFiNetwork> _currentWiFiNetworks;
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
            var scannedNetworks = wifiAdapter
                .NetworkReport
                .AvailableNetworks
                .OrderByDescending(x => x.NetworkRssiInDecibelMilliwatts)
                .ToList().Select(network => new CurrentWiFiNetwork
                {
                    Ssid = network.Ssid,
                    MacAddress = network.Bssid,
                    Leistung = network.NetworkRssiInDecibelMilliwatts.ToString(CultureInfo.InvariantCulture),
                    UpTime = network.Uptime.ToString(),
                    BeaconIntervall = network.BeaconInterval.ToString(),
                    ChannelFrequence = network.ChannelCenterFrequencyInKilohertz.ToString()

                });

           UpdateScannedData(scannedNetworks);
        }

        private void UpdateScannedData(IEnumerable<CurrentWiFiNetwork> updateNetworks)
        {
            foreach (var currentWiFiNetwork in updateNetworks)
            {
                var foundedNetwork = _currentWiFiNetworks.FirstOrDefault(x => x.MacAddress.Equals(currentWiFiNetwork.MacAddress));
                if (foundedNetwork == null)
                {
                    _currentWiFiNetworks.Add(currentWiFiNetwork);
                    continue;
                }
                foundedNetwork.Leistung = currentWiFiNetwork.Leistung;
                foundedNetwork.UpTime = currentWiFiNetwork.UpTime;
            }
        }
       
    }
}