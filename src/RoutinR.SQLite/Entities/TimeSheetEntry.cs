namespace RoutinR.SQLite.Entities
{
    public class TimeSheetEntry
    {
        public Job Job { get;set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}