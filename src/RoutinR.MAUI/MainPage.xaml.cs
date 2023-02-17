using Microsoft.Maui.Controls.PlatformConfiguration;
using RoutinR.Core;
using RoutinR.MAUI.ViewModels;
using RoutinR.Services;

namespace RoutinR.MAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}