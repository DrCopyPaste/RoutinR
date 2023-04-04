using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Networking;
using RoutinR.Core;
using RoutinR.Services;
using RoutinR.Services.Interfaces;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;

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

        [ObservableProperty]
        string newJobName;
    }
}