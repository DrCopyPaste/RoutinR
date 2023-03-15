using System.Runtime.CompilerServices;

namespace RoutinR.Core
{
    public class ApiExportProfile
    {
        public readonly string Name;
        public readonly string PostUrl;

        public readonly string StartTimeToken;
        public readonly string EndTimeToken;

        public readonly Dictionary<string, string> Headers = new Dictionary<string, string>();
        public readonly Dictionary<string, string> JobNameJsonTemplates = new Dictionary<string, string>();

        public ApiExportProfile(
            string name,
            string postUrl,
            Dictionary<string, string>? headers = null,
            Dictionary<string, string>? jobNameJsonTemplates = null,
            string startTimeToken = "_RoutinRStartTime_",
            string endTimeToken = "_RoutinREndTime_")
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name is empty or null");
            if (string.IsNullOrEmpty(postUrl)) throw new ArgumentNullException("post url is empty or null");
            if (!postUrl.StartsWith("http://") && !postUrl.StartsWith("https://")) throw new ArgumentException("post url did not start with http:// or https://");
            if (string.IsNullOrEmpty(startTimeToken)) throw new ArgumentNullException("startTimeToken is empty or null");
            if (string.IsNullOrEmpty(endTimeToken)) throw new ArgumentNullException("endTimeToken is empty or null");

            Name = name;
            PostUrl = postUrl;
            StartTimeToken = startTimeToken;
            EndTimeToken = endTimeToken;

            if (headers == null) return;
            foreach (var header in headers) Headers.Add(header.Key, header.Value);

            if (jobNameJsonTemplates == null) return;
            if (jobNameJsonTemplates.Any(template => !template.Value.Contains(startTimeToken) || !template.Value.Contains(endTimeToken))) throw new ArgumentException("cannot add template, because it does not contain the expected start time token");
            foreach (var template in jobNameJsonTemplates) JobNameJsonTemplates.Add(template.Key, template.Value);
        }
    }
}
