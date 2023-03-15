namespace External.TestApi
{
    /// <summary>
    /// mimicks a timesheet entry for kimai
    /// </summary>
    public class ApiTimeSheetEntry
    {
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public int Project { get; set; }
        public int Activity { get; set; }
        public string Tags { get; set; }
        public string Description { get; set; }
    }
}
