using RoutinR.Constants;
using RoutinR.MAUI.ViewModels;

namespace RoutinR.MAUI;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsPageViewModel viewModel)
	{
        BindingContext = viewModel;
        InitializeComponent();
	}

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        Preferences.Default.Set(SettingNames.ExportOnTimeSheetCompletion, e.Value);
    }
}