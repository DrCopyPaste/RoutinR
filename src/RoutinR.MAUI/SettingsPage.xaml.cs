using RoutinR.MAUI.ViewModels;

namespace RoutinR.MAUI;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsPageViewModel viewModel)
	{
        BindingContext = viewModel;
        InitializeComponent();
	}
}