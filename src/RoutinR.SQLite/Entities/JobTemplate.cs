using Microsoft.EntityFrameworkCore;
using RoutinR.Constants;

namespace RoutinR.SQLite.Entities
{
    [PrimaryKey(nameof(Id))]
    public class JobTemplate
    {
        public int Id { get; set; }
        public Job Key { get; set; }
        public ApiExportProfile ApiExportProfile { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}