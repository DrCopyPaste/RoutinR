using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoutinR.Constants;
using RoutinR.Services.Interfaces;
using RoutinR.SQLite.Services;

namespace RoutinR.MAUI.ViewModels
{
    public partial class SettingsPageViewModel : BaseViewModel
    {
        private readonly IDataService dataService;

        public SettingsPageViewModel(IDataService dataService)
        {
            this.dataService = dataService;

            ExportOnStoppingTimeSheet = Preferences.Default.Get(SettingNames.ExportOnTimeSheetCompletion, false);
        }

        [ObservableProperty]
        private bool exportOnStoppingTimeSheet;

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
                    var importDbPath = result.FullPath;
                    var targetDbPath = MauiProgram.dbPath;

                    if (this.dataService.GetType() == typeof(RoutinRSQLiteService)) ((RoutinRSQLiteService)this.dataService).Dispose();
                    File.Copy(importDbPath, targetDbPath, true);

                    this.dataService.Initialize();
                }

                return;
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
            }

            return;
        }
    }
}