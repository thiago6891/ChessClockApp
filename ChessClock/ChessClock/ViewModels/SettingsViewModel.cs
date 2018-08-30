using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace ChessClock
{
    public class SettingsViewModel : ViewModelBase
    {
        private static readonly List<string> _delayTypes = new List<string>
        {
            "None",
            "Fischer",
            "Bronstein",
            "Normal"
        };
        public static IList<string> DelayTypes => _delayTypes.AsReadOnly();

        private TimeSpan _gameTime;
        private TimeSpan _delayTime;
        private string _selectedDelayType;
        private bool _delayEntryEnabled;

        public TimeSpan GameTime
        {
            get
            {
                return _gameTime;
            }
            set
            {
                if (_gameTime == value) return;
                _gameTime = value;
                RaisePropertyChanged(() => GameTime);
            }
        }

        public TimeSpan DelayTime
        {
            get
            {
                return _delayTime;
            }
            set
            {
                if (_delayTime == value) return;
                _delayTime = value;
                RaisePropertyChanged(() => DelayTime);
            }
        }

        public string SelectedDelayType
        {
            get
            {
                return _selectedDelayType;
            }
            set
            {
                if (_selectedDelayType == value) return;
                _selectedDelayType = value;
                DelayEntryEnabled = value != _delayTypes[0];
                RaisePropertyChanged(() => SelectedDelayType);
            }
        }

        public bool DelayEntryEnabled
        {
            get
            {
                return _delayEntryEnabled;
            }
            private set
            {
                if (_delayEntryEnabled == value) return;
                _delayEntryEnabled = value;
                RaisePropertyChanged(() => DelayEntryEnabled);
            }
        }

        public void Save() => MessagingCenter.Send(this, "Settings_Saved", GetSettings());

        private ClockSettings GetSettings()
        {
            var delay = ClockSettings.DelayType.None;
            if (_selectedDelayType == DelayTypes[1]) delay = ClockSettings.DelayType.Fischer;
            else if (_selectedDelayType == DelayTypes[2]) delay = ClockSettings.DelayType.Bronstein;
            else if (_selectedDelayType == DelayTypes[3]) delay = ClockSettings.DelayType.Normal;
            return new ClockSettings(GameTime, delay, DelayTime);
        }

        public SettingsViewModel()
            : this(new ClockSettings(TimeSpan.Zero, ClockSettings.DelayType.None, TimeSpan.Zero)) { }

        public SettingsViewModel(ClockSettings settings)
        {
            _gameTime = settings.GameTime;
            _delayTime = settings.DelayTime;
            switch (settings.Delay)
            {
                case ClockSettings.DelayType.None:
                    _selectedDelayType = DelayTypes[0];
                    _delayEntryEnabled = false;
                    break;
                case ClockSettings.DelayType.Fischer:
                    _selectedDelayType = DelayTypes[1];
                    _delayEntryEnabled = true;
                    break;
                case ClockSettings.DelayType.Bronstein:
                    _selectedDelayType = DelayTypes[2];
                    _delayEntryEnabled = true;
                    break;
                case ClockSettings.DelayType.Normal:
                    _selectedDelayType = DelayTypes[3];
                    _delayEntryEnabled = true;
                    break;
            }
        }
    }
}