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
        private Color _buttonOneColor;
        private Color _buttonTwoColor;
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
                if (_clockOneTime == TimeSpan.Zero)
                {
                    ButtonOneEnabled = false;
                    ButtonOneColor = Color.Red;
                }
                else
                {
                    ButtonOneColor = Color.Default;
                }
                OnPropertyChanged(nameof(ClockOneTime));
            }
        }

        public TimeSpan ClockTwoTime
        {
            get => _clockTwoTime;
            private set
            {
                if (_clockTwoTime == value) return;
                _clockTwoTime = value;
                if (_clockTwoTime == TimeSpan.Zero)
                {
                    ButtonTwoEnabled = false;
                    ButtonTwoColor = Color.Red;
                }
                else
                {
                    ButtonTwoColor = Color.Default;
                }
                OnPropertyChanged(nameof(ClockTwoTime));
            }
        }

        public Color ButtonOneColor
        {
            get => _buttonOneColor;
            private set
            {
                if (_buttonOneColor == value) return;
                _buttonOneColor = value;
                OnPropertyChanged(nameof(ButtonOneColor));
            }
        }

        public Color ButtonTwoColor
        {
            get => _buttonTwoColor;
            private set
            {
                if (_buttonTwoColor == value) return;
                _buttonTwoColor = value;
                OnPropertyChanged(nameof(ButtonTwoColor));
            }
        }

        public bool ButtonOneEnabled
        {
            get => _buttonOneEnabled;
            private set
            {
                if (_buttonOneEnabled == value) return;
                _buttonOneEnabled = value;
                OnPropertyChanged(nameof(ButtonOneEnabled));
            }
        }

        public bool ButtonTwoEnabled
        {
            get => _buttonTwoEnabled;
            private set
            {
                if (_buttonTwoEnabled == value) return;
                _buttonTwoEnabled = value;
                OnPropertyChanged(nameof(ButtonTwoEnabled));
            }
        }

        public bool ResetButtonEnabled
        {
            get => _resetButtonEnabled;
            private set
            {
                if (_resetButtonEnabled == value) return;
                _resetButtonEnabled = value;
                OnPropertyChanged(nameof(ResetButtonEnabled));
            }
        }

        public bool SettingsButtonEnabled
        {
            get => _settingsButtonEnabled;
            private set
            {
                if (_settingsButtonEnabled == value) return;
                _settingsButtonEnabled = value;
                OnPropertyChanged(nameof(SettingsButtonEnabled));
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
            _clock = new NoDelayChessClock(TimeSpan.FromMinutes(5));

            ClockOneTime = _clock.GetRemainingTime(Player.ONE);
            ClockTwoTime = _clock.GetRemainingTime(Player.TWO);
            ButtonOneColor = Color.Default;
            ButtonTwoColor = Color.Default;
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