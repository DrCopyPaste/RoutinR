using RoutinR.MAUI.ViewModels;

namespace RoutinR.MAUI;

public partial class JobTimeSheetEntriesPage : ContentPage
{
	public JobTimeSheetEntriesPage(JobTimeSheetEntriesPageViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        ((JobTimeSheetEntriesPageViewModel)BindingContext).RefreshEntries();
        base.OnAppearing();
    }
}