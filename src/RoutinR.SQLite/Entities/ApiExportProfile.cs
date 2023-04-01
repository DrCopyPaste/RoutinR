using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace RoutinR.SQLite.Entities
{
    [PrimaryKey(nameof(Id))]
    public class ApiExportProfile
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PostUrl { get; set; } = string.Empty;

        public string StartTimeToken { get; set; } = string.Empty;
        public string EndTimeToken { get; set; } = string.Empty;

        public ICollection<JobTemplate>? JobTemplates { get; set; }

        public string Headers { get; set; } = string.Empty;
    }
}
