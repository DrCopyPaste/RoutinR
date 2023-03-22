using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace RoutinR.Core
{
    public class ApiExportProfile
    {
        private readonly string name;
        public string Name => name;
        public readonly string PostUrl;

        public readonly string StartTimeToken;
        public readonly string EndTimeToken;

        public readonly ReadOnlyDictionary<string, string> JobNameJsonTemplates;
        public readonly ReadOnlyDictionary<string, string>? Headers = null;

        public override bool Equals(object? obj)
        {
            if (obj is not ApiExportProfile that) return false;

            if (this.Name != that.Name) return false;
            if (this.PostUrl != that.PostUrl) return false;
            if (this.StartTimeToken != that.StartTimeToken) return false;
            if (this.EndTimeToken != that.EndTimeToken) return false;

            if (!this.JobNameJsonTemplates.OrderBy(x => x.Key).SequenceEqual(that.JobNameJsonTemplates.OrderBy(x => x.Key))) return false;

            if (this.Headers == null || that.Headers == null) return this.Headers == that.Headers;
            return this.Headers.OrderBy(x => x.Key).SequenceEqual(that.Headers.OrderBy(x => x.Key));
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
                hash = hash * 486187739 + PostUrl.GetHashCode();
                hash = hash * 486187739 + StartTimeToken.GetHashCode();
                hash = hash * 486187739 + EndTimeToken.GetHashCode();

                foreach (var template in JobNameJsonTemplates.OrderBy(x => x.Key))
                {
                    hash = hash * 486187739 + template.Key.GetHashCode();
                    hash = hash * 486187739 + template.Value.GetHashCode();
                }

                if (Headers == null) return hash;
                foreach (var header in Headers.OrderBy(x => x.Key))
                {
                    hash = hash * 486187739 + header.Key.GetHashCode();
                    hash = hash * 486187739 + header.Value.GetHashCode();
                }

                return hash;
            }
        }

        public ApiExportProfile(
            string name,
            string postUrl,
            Dictionary<string, string>? headers = null,
            Dictionary<string, string>? jobNameJsonTemplates = null,
            string startTimeToken = "_RoutinRStartTime_",
            string endTimeToken = "_RoutinREndTime_")
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(postUrl)) throw new ArgumentNullException(nameof(postUrl));
            if (!postUrl.StartsWith("http://") && !postUrl.StartsWith("https://")) throw new ArgumentException("post url did not start with http:// or https://");
            if (jobNameJsonTemplates == null || !jobNameJsonTemplates.Any()) throw new ArgumentException("jobJsonTemplates is null or empty");
            if (startTimeToken == endTimeToken) throw new ArgumentException("startTimeToken is identical to endTimeToken");
            if (string.IsNullOrEmpty(startTimeToken)) throw new ArgumentNullException(nameof(startTimeToken));
            if (string.IsNullOrEmpty(endTimeToken)) throw new ArgumentNullException(nameof(endTimeToken));

            this.name = name;
            PostUrl = postUrl;
            StartTimeToken = startTimeToken;
            EndTimeToken = endTimeToken;

            if (jobNameJsonTemplates.Any(template => !template.Value.Contains(startTimeToken) || !template.Value.Contains(endTimeToken))) throw new ArgumentException("cannot add template, because it does not contain the expected start time token");
            foreach (var template in jobNameJsonTemplates)
            {
                if (
                    !template.Value.Replace(startTimeToken, string.Empty).Contains(endTimeToken)
                    ||
                    !template.Value.Replace(endTimeToken, string.Empty).Contains(startTimeToken)
                    )
                {
                    throw new ArgumentException("starttime and endtime token overlap, cannot add template");
                }
            }

            JobNameJsonTemplates = new ReadOnlyDictionary<string, string>(jobNameJsonTemplates);

            if (headers == null) return;
            Headers = new ReadOnlyDictionary<string, string>(headers);
        }
    }
}
