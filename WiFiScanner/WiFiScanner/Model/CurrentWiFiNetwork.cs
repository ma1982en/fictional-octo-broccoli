namespace WiFiScanner.Model
{
    public class CurrentWiFiNetwork
    {
        public string Ssid { get; set; }
        public string MacAddress { get; set; }
        public string Leistung { get; set; }
        public string BeaconIntervall { get; set; }
        public string UpTime { get; set; }
        public string ChannelFrequence { get; set; }
    }
}