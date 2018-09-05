using System;
using System.Collections.Generic;
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

        private readonly List<int> _hoursPickerItems;
        private readonly List<int> _minutesPickerItems;
        private readonly List<int> _secondsPickerItems;

        private TimeSpan _gameTime;
        private TimeSpan _delayTime;
        private string _selectedDelayType;
        private bool _delayEntryEnabled;

        public IList<int> HoursPickerItems => _hoursPickerItems.AsReadOnly();
        public IList<int> MinutesPickerItems => _minutesPickerItems.AsReadOnly();
        public IList<int> SecondsPickerItems => _secondsPickerItems.AsReadOnly();

        public int GameTimeHours
        {
            get
            {
                return _gameTime.Hours;
            }
            set
            {
                if (_gameTime.Hours == value) return;
                _gameTime = new TimeSpan(value, _gameTime.Minutes, _gameTime.Seconds);
                OnPropertyChanged(nameof(GameTimeHours));
            }
        }

        public int GameTimeMinutes
        {
            get
            {
                return _gameTime.Minutes;
            }
            set
            {
                if (_gameTime.Minutes == value) return;
                _gameTime = new TimeSpan(_gameTime.Hours, value, _gameTime.Seconds);
                OnPropertyChanged(nameof(GameTimeMinutes));
            }
        }

        public int GameTimeSeconds
        {
            get
            {
                return _gameTime.Seconds;
            }
            set
            {
                if (_gameTime.Seconds == value) return;
                _gameTime = new TimeSpan(_gameTime.Hours, _gameTime.Minutes, value);
                OnPropertyChanged(nameof(GameTimeSeconds));
            }
        }

        public int DelayTimeHours
        {
            get
            {
                return _delayTime.Hours;
            }
            set
            {
                if (_delayTime.Hours == value) return;
                _delayTime = new TimeSpan(value, _delayTime.Minutes, _delayTime.Seconds);
                OnPropertyChanged(nameof(DelayTimeHours));
            }
        }

        public int DelayTimeMinutes
        {
            get
            {
                return _delayTime.Minutes;
            }
            set
            {
                if (_delayTime.Minutes == value) return;
                _delayTime = new TimeSpan(_delayTime.Hours, value, _delayTime.Seconds);
                OnPropertyChanged(nameof(DelayTimeMinutes));
            }
        }

        public int DelayTimeSeconds
        {
            get
            {
                return _delayTime.Seconds;
            }
            set
            {
                if (_delayTime.Seconds == value) return;
                _delayTime = new TimeSpan(_delayTime.Hours, _delayTime.Minutes, value);
                OnPropertyChanged(nameof(DelayTimeSeconds));
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
                OnPropertyChanged(nameof(SelectedDelayType));
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
                OnPropertyChanged(nameof(DelayEntryEnabled));
            }
        }

        public void Save() => MessagingCenter.Send(this, "Settings_Saved", GetSettings());

        private ClockSettings GetSettings()
        {
            var delay = ClockSettings.DelayType.None;
            if (_selectedDelayType == DelayTypes[1]) delay = ClockSettings.DelayType.Fischer;
            else if (_selectedDelayType == DelayTypes[2]) delay = ClockSettings.DelayType.Bronstein;
            else if (_selectedDelayType == DelayTypes[3]) delay = ClockSettings.DelayType.Normal;
            return new ClockSettings(_gameTime, delay, _delayTime);
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

            _hoursPickerItems = new List<int>();
            for (int i = 0; i <= 10; i++) _hoursPickerItems.Add(i);

            _minutesPickerItems = new List<int>();
            _secondsPickerItems = new List<int>();
            for (int i = 0; i <= 59; i++)
            {
                _minutesPickerItems.Add(i);
                _secondsPickerItems.Add(i);
            }
        }
    }
}