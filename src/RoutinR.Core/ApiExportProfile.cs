namespace RoutinR.Core
{
    public class ApiExportProfile
    {
        private string name;
        private string postUrl;

        public string Name => name;
        public string PostUrl => postUrl;

        public IEnumerable<string> Headers;

        public ApiExportProfile(string name, string postUrl)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name is empty or null");
            if (string.IsNullOrEmpty(postUrl)) throw new ArgumentNullException("post url is empty or null");
            if (!postUrl.StartsWith("http://") && !postUrl.StartsWith("https://")) throw new ArgumentNullException("post url did not start with http:// or https://");

            this.name = name;
            this.postUrl = postUrl;
        }
    }
}
