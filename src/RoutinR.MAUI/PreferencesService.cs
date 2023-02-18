using RoutinR.Constants;

namespace RoutinR.MAUI
{
    internal class PreferencesService
    {
        public static void EnsureDefaultConfig()
        {
            // bool currentIsRunning = Preferences.Default.Get(SettingNames.CurrentIsRunning, false);
            // string currentJobName = Preferences.Default.Get(SettingNames.CurrentJobName, JobNames.Idle);
            // DateTime? currentStartTime = Preferences.Default.Get<DateTime?>(SettingNames.CurrentStartTime, null);

            // Preferences.Default.Set(SettingNames.CurrentJobName, currentJobName);
        }

        public static void Reset()
        {
            Preferences.Clear();
        }
    }
}
