using Microsoft.EntityFrameworkCore;
using RoutinR.Constants;

namespace RoutinR.SQLite.Entities
{
    [PrimaryKey(nameof(Id))]
    public class Job
    {
        public int Id { get; set; }
        public string Name { get; set; } = JobNames.Idle;
    }
}