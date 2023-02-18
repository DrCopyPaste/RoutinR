using RoutinR.MAUI.ViewModels;

namespace RoutinR.MAUI;

public partial class JobsPage : ContentPage
{
	public JobsPage(JobsPageViewModel viewModel)
	{
        BindingContext = viewModel;
        InitializeComponent();
	}
}