using RoutinR.MAUI.ViewModels;

namespace RoutinR.MAUI
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage(AboutPageViewModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}