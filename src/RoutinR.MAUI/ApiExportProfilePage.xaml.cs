using RoutinR.MAUI.ViewModels;

namespace RoutinR.MAUI;

public partial class ApiExportProfilePage : ContentPage
{
	public ApiExportProfilePage(ApiExportProfilePageViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await ((ApiExportProfilePageViewModel)BindingContext).InitAsync();
    }
}