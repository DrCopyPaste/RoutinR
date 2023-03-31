using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace RoutinR.SQLite.Entities
{
    [PrimaryKey(nameof(StartTime), nameof(EndTime))]
    public class TimeSheetEntry
    {
        public Job Job { get;set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}