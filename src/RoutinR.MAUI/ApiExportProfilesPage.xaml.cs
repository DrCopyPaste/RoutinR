using RoutinR.MAUI.ViewModels;

namespace RoutinR.MAUI;

public partial class ApiExportProfilesPage : ContentPage
{
	public ApiExportProfilesPage(ApiExportProfilesPageViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}