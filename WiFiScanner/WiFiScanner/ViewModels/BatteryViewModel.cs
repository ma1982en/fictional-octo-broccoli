using System;
using System.Diagnostics;
using System.Windows.Input;
using Windows.Devices.Enumeration;
using Prism.Mvvm;
using Windows.Devices.Power;
using Prism.Commands;

namespace WiFiScanner.ViewModels
{
    public class BatteryViewModel : BindableBase
    {
        private BatteryReport _currentBatteryReport;

        public BatteryReport CurrentBatteryReport
        {
            get { return _currentBatteryReport; }
            set { _currentBatteryReport = value; this.OnPropertyChanged();}
        }

        public ICommand GetBatteryStateCommand { get; set; }

        public BatteryViewModel()
        {
            GetBatteryStateCommand = new DelegateCommand(OnGetBatteryState);
        }

        private async void OnGetBatteryState()
        {
            // Find batteries 
            var deviceInfo = await DeviceInformation.FindAllAsync(Battery.GetDeviceSelector());
            foreach (DeviceInformation device in deviceInfo)
            {
                try
                {
                    // Create battery object
                    var battery = await Battery.FromIdAsync(device.Id);

                    // Get report
                    CurrentBatteryReport = battery.GetReport();

                    //// Update UI
                    //AddReportUI(BatteryReportPanel, report, battery.DeviceId);
                }
                catch(Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                }
            }
        }
    }
}
