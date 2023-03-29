using System.Collections.ObjectModel;

namespace RoutinR.SQLite.Entities
{
    public class ApiExportProfile
    {
        public string Name { get; set; } = string.Empty;
        public string PostUrl { get; set; } = string.Empty;

        public string StartTimeToken { get; set; } = string.Empty;
        public string EndTimeToken { get; set; } = string.Empty;

        public ReadOnlyDictionary<string, string> JobNameJsonTemplates;

        public IEnumerable<ApiExportProfileHeader> Headers;
    }
}
