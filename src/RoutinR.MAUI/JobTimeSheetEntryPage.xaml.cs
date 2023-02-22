using RoutinR.MAUI.ViewModels;

namespace RoutinR.MAUI;

public partial class JobTimeSheetEntryPage : ContentPage
{
	public JobTimeSheetEntryPage(JobTimeSheetEntryPageViewModel viewModel)
	{
        BindingContext = viewModel;
        InitializeComponent();
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await ((JobTimeSheetEntryPageViewModel)BindingContext).InitAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}