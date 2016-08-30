using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Prism.Commands;
using Prism.Mvvm;
using WiFiScanner.Views;

namespace WiFiScanner.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        public UserControl View
        {
            get { return _view; }
            set { _view = value; this.OnPropertyChanged(); }
        }

        public ICommand NextViewCommand { get; set; }
        public ICommand PreviousViewCommand { get; set; }

        public MainPageViewModel()
        {
            LoadViews();
            NextViewCommand = new DelegateCommand(OnGoNextView);
            PreviousViewCommand = new DelegateCommand(OnGoPreviousView);           
        }

        private void OnGoNextView()
        {
            if (View == null)
            {
                View = _loadedViews[0];
                return;
            }

            var currentindex = _loadedViews.IndexOf(View);

            currentindex++;

            if (currentindex < _loadedViews.Count)
                View = _loadedViews[currentindex];
            else
                View = _loadedViews[0];
        }

        private void OnGoPreviousView()
        {
            if (View == null)
            {
                View = _loadedViews[0];
                return;
            }

            var index = _loadedViews.IndexOf(View);

            index--;

            if (index < 0)
                View = _loadedViews[_loadedViews.Count - 1];
            else
                View = _loadedViews[index];
        }

        private void LoadViews()
        {
            _loadedViews.Clear();
            _loadedViews.Add(new WiFiView {DataContext = new WifiScannerViewModel()});
            _loadedViews.Add(new BatteryView {DataContext = new BatteryViewModel()});
            View = _loadedViews[0];
        }

        private readonly List<UserControl> _loadedViews = new List<UserControl>();
        private UserControl _view;
    }
}