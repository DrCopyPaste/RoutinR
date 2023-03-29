using RoutinR.MAUI.ViewModels;

namespace RoutinR.MAUI;

public partial class ApiExportProfilesPage : ContentPage
{
    private bool initialized;

    public ApiExportProfilesPage(ApiExportProfilesPageViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        if (initialized) ((ApiExportProfilesPageViewModel)BindingContext).Refresh();

        initialized = true;
        base.OnAppearing();
    }
}