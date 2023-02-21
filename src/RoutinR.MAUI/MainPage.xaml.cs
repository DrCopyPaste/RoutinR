using RoutinR.Constants;
using RoutinR.Core;
using RoutinR.MAUI.ViewModels;

namespace RoutinR.MAUI
{
    public partial class MainPage : ContentPage
    {
        private bool initialized = false;
        public MainPage(MainPageViewModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (initialized) ((MainPageViewModel)BindingContext).RefreshEntriesAndSelectJob();

            initialized = true;
            ((MainPageViewModel)BindingContext).Paused = false;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            ((MainPageViewModel)BindingContext).Paused = true;
            base.OnDisappearing();
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            // event might fire before instances are available ;/
            if (((MainPageViewModel)BindingContext).CurrentJob != null) Preferences.Default.Set(SettingNames.CurrentJobName, ((MainPageViewModel)BindingContext).CurrentJob.Name);
        }
    }
}