using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChessClock
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsView : ContentPage
	{
		public SettingsView()
		{
			InitializeComponent();
		}

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            ((SettingsViewModel)BindingContext).Save();
            GoBackOnePage();
        }

        private void CancelButton_Clicked(object sender, EventArgs e) => GoBackOnePage();

        private void GoBackOnePage() => Navigation.PopModalAsync(true);
    }
}