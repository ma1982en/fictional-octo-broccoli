using Prism.Mvvm;

namespace WiFiScanner.Model
{
    public class CurrentWiFiNetwork : BindableBase
    {
        public string Ssid
        {
            get { return _ssid; }
            set { _ssid = value; OnPropertyChanged();}
        }

        public string MacAddress
        {
            get { return _macAddress; }
            set { _macAddress = value; OnPropertyChanged();}
        }

        public string Leistung
        {
            get { return _leistung; }
            set { _leistung = value; OnPropertyChanged(); }
        }

        public string BeaconIntervall
        {
            get { return _beaconIntervall; }
            set { _beaconIntervall = value;OnPropertyChanged(); }
        }

        public string UpTime
        {
            get { return _upTime; }
            set { _upTime = value; OnPropertyChanged();}
        }

        public string ChannelFrequence
        {
            get { return _channelFrequence; }
            set { _channelFrequence = value;OnPropertyChanged(); }
        }

        private string _leistung;
        private string _macAddress;
        private string _ssid;
        private string _beaconIntervall;
        private string _upTime;
        private string _channelFrequence;
    }
}