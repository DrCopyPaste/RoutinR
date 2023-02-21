using RoutinR.Constants;
using RoutinR.Core;
using RoutinR.MAUI.ViewModels;

namespace RoutinR.MAUI
{
    public partial class MainPage : ContentPage
    {
        private int appearenceCounter = 0;
        public MainPage(MainPageViewModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (appearenceCounter != 0) ((MainPageViewModel)BindingContext).RefreshEntriesAndSelectJob();

            appearenceCounter = appearenceCounter + 1;
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