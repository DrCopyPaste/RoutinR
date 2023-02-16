using PunchClock.Constants;

namespace PunchClock.Core
{
    public class Job
    {
        private string name = JobNames.Idle;
        public string Name => name;

        private Job() { }

        private Job(string name)
        {
            this.name = name;
        }

        public static Job NewDefault()
        {
            return new Job();
        }

        public static Job NewFromName(string name)
        {
            return new Job(name);
        }
    }
}