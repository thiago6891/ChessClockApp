using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace ChessClock
{
    public class ClockViewModel : ViewModelBase
    {
        private TimeSpan _clockOneTime;
        private TimeSpan _clockTwoTime;
        private bool _buttonOneEnabled;
        private bool _buttonTwoEnabled;
        private bool _resetButtonEnabled;
        private bool _settingsButtonEnabled;

        public TimeSpan ClockOneTime
        {
            get => _clockOneTime;
            private set
            {
                if (_clockOneTime == value) return;
                _clockOneTime = value;
                RaisePropertyChanged(() => ClockOneTime);
            }
        }

        public TimeSpan ClockTwoTime
        {
            get => _clockTwoTime;
            private set
            {
                if (_clockTwoTime == value) return;
                _clockTwoTime = value;
                RaisePropertyChanged(() => ClockTwoTime);
            }
        }

        public bool ButtonOneEnabled
        {
            get => _buttonOneEnabled;
            private set
            {
                if (_buttonOneEnabled == value) return;
                _buttonOneEnabled = value;
                RaisePropertyChanged(() => ButtonOneEnabled);
            }
        }

        public bool ButtonTwoEnabled
        {
            get => _buttonTwoEnabled;
            private set
            {
                if (_buttonTwoEnabled == value) return;
                _buttonTwoEnabled = value;
                RaisePropertyChanged(() => ButtonTwoEnabled);
            }
        }

        public bool ResetButtonEnabled
        {
            get => _resetButtonEnabled;
            private set
            {
                if (_resetButtonEnabled == value) return;
                _resetButtonEnabled = value;
                RaisePropertyChanged(() => ResetButtonEnabled);
            }
        }

        public bool SettingsButtonEnabled
        {
            get => _settingsButtonEnabled;
            private set
            {
                if (_settingsButtonEnabled == value) return;
                _settingsButtonEnabled = value;
                RaisePropertyChanged(() => SettingsButtonEnabled);
            }
        }

        public ICommand ButtonOneClickCommand => new Command(ButtonOneClick);
        public ICommand ButtonTwoClickCommand => new Command(ButtonTwoClick);

        private void ButtonOneClick()
        {
            ButtonOneEnabled = false;
            ButtonTwoEnabled = true;
            // TODO: implement the proper clock actions (in the model)
        }

        private void ButtonTwoClick()
        {
            ButtonOneEnabled = true;
            ButtonTwoEnabled = false;
            // TODO: implement the proper clock actions (in the model)
        }

        public void Reset()
        {
            ButtonOneEnabled = true;
            ButtonTwoEnabled = true;
            // TODO: implement proper actions
        }

        public ClockViewModel()
        {
            // TODO: Remove this and get the real time from the model
            ClockOneTime = TimeSpan.FromMinutes(1.1);
            ClockTwoTime = TimeSpan.FromSeconds(60 * 60 + 10);
            
            ButtonOneEnabled = true;
            ButtonTwoEnabled = true;
            ResetButtonEnabled = false;
            SettingsButtonEnabled = true;
        }
    }
}