using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ChessClock
{
    public partial class App : Application
    {
        public static readonly string CLOCK_KEY = "clock";

        public App()
        {
            InitializeComponent();

            if (!Current.Properties.ContainsKey(CLOCK_KEY))
            {
                Current.Properties[CLOCK_KEY] = ChessClock.CreateClock(new ClockSettings(
                    TimeSpan.FromMinutes(5),
                    ClockSettings.DelayType.None,
                    TimeSpan.Zero));
            }

            MainPage = new ClockView();
        }

        protected override void OnStart() { }

        protected override void OnSleep()
        {
            ((ChessClock)Current.Properties[CLOCK_KEY]).Pause();
        }

        protected override void OnResume()
        {
            ((ChessClock)Current.Properties[CLOCK_KEY]).Resume();
        }
    }
}