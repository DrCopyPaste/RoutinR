using External.TestApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RoutinR.Core;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Text;

namespace RoutinR.Services.Tests
{
    public class Test_ApiExportService
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl;

        public Test_ApiExportService()
        {
            var appFactory = new WebApplicationFactory<Program>();
            httpClient = appFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Add(External.TestApi.Constants.Api_User, "user");
            httpClient.DefaultRequestHeaders.Add(External.TestApi.Constants.Api_Key, "key");

            if (httpClient.BaseAddress == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }
            baseUrl = httpClient.BaseAddress.ToString();
        }

        [Fact]
        public void Simply_Posting_To_TestApi_Works()
        {
            var route = $"{baseUrl}{External.TestApi.Constants.Timesheet_Post_Route}";

            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(new ApiTimeSheetEntry() { Name = "TestEntry" }), Encoding.UTF8, "application/json");
            var task = httpClient.PostAsync(route, content);
            var response = task.GetAwaiter().GetResult();

            var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }
    }
}