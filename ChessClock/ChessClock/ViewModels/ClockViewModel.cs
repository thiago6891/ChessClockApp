using System;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;

namespace ChessClock
{
    public class ClockViewModel : ViewModelBase
    {
        private const double CLOCK_REFRESH_RATE = 33.3;

        private ChessClock _clock;

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
            ResetButtonEnabled = true;
            SettingsButtonEnabled = false;
            _clock.PressButton(Player.ONE);
        }

        private void ButtonTwoClick()
        {
            ButtonOneEnabled = true;
            ButtonTwoEnabled = false;
            ResetButtonEnabled = true;
            SettingsButtonEnabled = false;
            _clock.PressButton(Player.TWO);
        }

        public void Reset()
        {
            ButtonOneEnabled = true;
            ButtonTwoEnabled = true;
            ResetButtonEnabled = false;
            SettingsButtonEnabled = true;
            _clock.Reset();
        }

        public ClockViewModel()
        {
            _clock = new NoDelayChessClock(TimeSpan.FromSeconds(10));

            ClockOneTime = _clock.GetRemainingTime(Player.ONE);
            ClockTwoTime = _clock.GetRemainingTime(Player.TWO);
            ButtonOneEnabled = true;
            ButtonTwoEnabled = true;
            ResetButtonEnabled = false;
            SettingsButtonEnabled = true;

            var timer = new Timer(CLOCK_REFRESH_RATE);
            timer.Elapsed += (s, e) =>
            {
                ClockOneTime = _clock.GetRemainingTime(Player.ONE);
                ClockTwoTime = _clock.GetRemainingTime(Player.TWO);
            };
            timer.Start();

            MessagingCenter.Subscribe<SettingsViewModel, ClockSettings>(this, "Settings_Saved", SettingsChanged);
        }

        private void SettingsChanged(SettingsViewModel sender, ClockSettings settings)
        {
            switch (settings.Delay)
            {
                case ClockSettings.DelayType.None:
                    _clock = new NoDelayChessClock(settings.GameTime);
                    break;
                case ClockSettings.DelayType.Fischer:
                    _clock = new FischerDelayChessClock(settings.GameTime, settings.DelayTime);
                    break;
                case ClockSettings.DelayType.Bronstein:
                    _clock = new BronsteinDelayChessClock(settings.GameTime, settings.DelayTime);
                    break;
                case ClockSettings.DelayType.Normal:
                    _clock = new NormalDelayChessClock(settings.GameTime, settings.DelayTime);
                    break;
            }
        }

        public ClockSettings GetSettings() => _clock.GetSettings();
    }
}