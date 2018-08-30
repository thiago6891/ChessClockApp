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
                BindingContext = new SettingsViewModel
                {
                    // TODO: get data for the view model from the clock model (maybe it should be done from the
                    // SettingsViewModel class, and NOT from this view).
                    GameTime = TimeSpan.FromMinutes(5),
                    SelectedDelayType = SettingsViewModel.DelayTypes[0],
                    DelayTime = TimeSpan.Zero
                }
            }, true);
        }
    }
}