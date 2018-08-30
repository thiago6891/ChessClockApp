using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace ChessClock
{
    public class SettingsViewModel : ViewModelBase
    {
        private static readonly List<string> _delayTypes = new List<string> { "None", "Fischer", "Bronstein", "Normal" };
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

        public ICommand SaveCommand => new Command(Save);
        private void Save()
        {
            // TODO: implement save
            throw new NotImplementedException();
        }
    }
}