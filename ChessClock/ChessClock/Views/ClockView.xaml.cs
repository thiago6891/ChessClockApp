using System;
using Xamarin.Forms;

namespace ChessClock
{
    public partial class ClockView : ContentPage
    {
        public ClockView()
        {
            InitializeComponent();
        }

        private async void ResetButton_Tapped(object sender, EventArgs e)
        {
            if (await DisplayAlert("Confirm Reset", "Reset the clock?", "Reset", "Cancel"))
                ((ClockViewModel)BindingContext).Reset();
        }

        private void SettingsButton_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SettingsView
            {
                BindingContext = new SettingsViewModel(((ClockViewModel)BindingContext).GetSettings())
            }, true);
        }
    }
}