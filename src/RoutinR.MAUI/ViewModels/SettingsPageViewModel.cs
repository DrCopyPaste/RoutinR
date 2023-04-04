using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoutinR.Services.Interfaces;

namespace RoutinR.MAUI.ViewModels
{
    public partial class SettingsPageViewModel : BaseViewModel
    {
        private readonly IDataService dataService;

        public SettingsPageViewModel(IDataService dataService)
        {
            this.dataService = dataService;

            NewJobName = "Blabla";
        }

        [RelayCommand]
        async Task ExportDatabase(CancellationToken cancellationToken)
        {
            try
            {
                var result = await FolderPicker.Default.PickAsync(cancellationToken);
                if (result.IsSuccessful && result.Folder != null)
                {
                    var sourcePath = MauiProgram.dbPath;
                    var targetPath = Path.Combine(result.Folder.Path, $"Export_{nameof(RoutinR)}.db");

                    File.Copy(sourcePath, targetPath, true);
                }
            }
            catch (Exception ex)
            { }
        }

        [RelayCommand]
        async Task ImportDatabase()
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(new PickOptions() { PickerTitle = "Import database file" });
                if (result != null)
                {
                    // import database ...
                }

                return;
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
            }

            return;
        }

        [ObservableProperty]
        string newJobName;
    }
}