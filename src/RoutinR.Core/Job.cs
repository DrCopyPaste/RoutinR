using RoutinR.Constants;
using System.Reflection.PortableExecutable;

namespace RoutinR.Core
{
    public class Job
    {
        private readonly string name = JobNames.Idle;
        public string Name => name;

        public override bool Equals(object? obj)
        {
            if (obj is not Job that) return false;
            if (this == null ||  that == null) return this == that;

            return this.Name == that.Name;
        }

        /// <summary>
        /// Get HashCode of this instance
        /// 
        /// e.g. see https://stackoverflow.com/a/263416
        /// </summary>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                // pick 2 prime numbers, the number to multiply by should be large
                int hash = 17;

                hash = hash * 486187739 + Name.GetHashCode();

                return hash;
            }
        }

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