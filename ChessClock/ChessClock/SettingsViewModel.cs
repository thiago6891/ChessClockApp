using System;

namespace ChessClock
{
    public enum Delay
    {
        None,
        Fischer,
        Bronstein,
        Normal
    }

    public class SettingsViewModel : ViewModelBase
    {
        private TimeSpan _gameTime;
        private TimeSpan _delayTime;
        private Delay _delayType;

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

        public Delay DelayType
        {
            get
            {
                return _delayType;
            }
            set
            {
                if (_delayType == value) return;
                _delayType = value;
                RaisePropertyChanged(() => DelayType);
            }
        }

        public SettingsViewModel()
        {
            GameTime = TimeSpan.FromMinutes(5);
            DelayType = Delay.None;
            DelayTime = TimeSpan.Zero;
        }
    }
}