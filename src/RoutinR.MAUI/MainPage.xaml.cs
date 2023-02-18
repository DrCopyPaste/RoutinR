using RoutinR.MAUI.ViewModels;

namespace RoutinR.MAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            ((MainPageViewModel)BindingContext).Paused = false;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            ((MainPageViewModel)BindingContext).Paused = true;
            base.OnDisappearing();
        }
    }
}