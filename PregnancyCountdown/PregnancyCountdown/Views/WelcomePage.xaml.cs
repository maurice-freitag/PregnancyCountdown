using System;
using System.ComponentModel;
using PregnancyCountdown.ViewModels;
using Xamarin.Forms;

namespace PregnancyCountdown.Views
{
    [DesignTimeVisible(false)]
    public partial class WelcomePage : ContentPage
    {
        private WelcomeViewModel viewModel;

        public WelcomePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new WelcomeViewModel();
        }

        private async void OnSettingsItemClicked(object sender, EventArgs e)
        {
            var page = new NavigationPage(new SettingsPage());
            page.Disappearing += OnSettingsPageDisappearing;
            await Navigation.PushModalAsync(page);
        }

        private void OnSettingsPageDisappearing(object sender, EventArgs e)
        {
            viewModel.Refresh();
            if (sender is NavigationPage page)
                page.Disappearing -= OnSettingsPageDisappearing;
        }
    }
}