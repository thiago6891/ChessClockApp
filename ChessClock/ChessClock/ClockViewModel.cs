using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;

namespace ChessClock
{
    public class ClockViewModel : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            var name = GetMemberInfo(property).Name;
            OnPropertyChanged(name);
        }

        private MemberInfo GetMemberInfo(Expression expression)
        {
            MemberExpression operand;
            LambdaExpression lambdaExpression = (LambdaExpression)expression;
            if (lambdaExpression.Body as UnaryExpression != null)
            {
                UnaryExpression body = (UnaryExpression)lambdaExpression.Body;
                operand = (MemberExpression)body.Operand;
            }
            else
            {
                operand = (MemberExpression)lambdaExpression.Body;
            }
            return operand.Member;
        }
    }
}