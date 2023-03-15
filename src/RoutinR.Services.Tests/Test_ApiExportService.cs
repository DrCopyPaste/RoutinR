using External.TestApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RoutinR.Core;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Text;
using Newtonsoft.Json.Linq;

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

            if (httpClient.BaseAddress == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }
            baseUrl = httpClient.BaseAddress.ToString();
        }

        [Fact]
        [Trait("Category", "External Api Integration")]
        public void ExportToApi_can_post_and_get_successful_result()
        {
            var exportService = new ExportService();
            var timesheetEntry = new JobTimeSheetEntry(job: Job.NewDefault(), DateTime.Now.Subtract(TimeSpan.FromMinutes(1)), DateTime.Now);

            var exportProfile = new ApiExportProfile(
                name: "TestExportProfile",
                postUrl: $"{baseUrl}{External.TestApi.Constants.Timesheet_Post_Route}",
                headers: new Dictionary<string, string>()
                {
                    { External.TestApi.Constants.Api_User, External.TestApi.Constants.Api_User },
                    { External.TestApi.Constants.Api_Key, External.TestApi.Constants.Api_Key }
                },
                jobNameJsonTemplates: new Dictionary<string, string>()
                {
                    {
                        Job.NewDefault().Name,
                        @"{
  ""begin"": ""_RoutinRStartTime_"",
  ""end"": ""_RoutinREndTime_"",
  ""project"": 1,
  ""activity"": 1,
  ""description"": ""api post test"",
  ""tags"": ""api test""
}" }
                },
                startTimeToken: "_RoutinRStartTime_",
                endTimeToken: "_RoutinREndTime_");

            var result = exportService.ExportToApi(timesheetEntry, exportProfile, httpClient).GetAwaiter().GetResult();

            Assert.True(result != null, "Result has no value");
            Assert.True(!result.HasError, "Result for sample api has error");
        }

        [Fact]
        [Trait("Category", "Test Post To Kimai API")]
        public void Sample_Call_To_Real_API()
        {
            var httpClient = this.httpClient;
            var postUrl = $"{baseUrl}{External.TestApi.Constants.Timesheet_Post_Route}";
            var userNameKey = External.TestApi.Constants.Api_User;
            var apiKeyName = External.TestApi.Constants.Api_Key;
            var userName = External.TestApi.Constants.Api_User;
            var apiKey = External.TestApi.Constants.Api_Key;
            var postTemplate = @"{
  ""begin"": ""_RoutinRStartTime_"",
  ""end"": ""_RoutinREndTime_"",
  ""project"": 1,
  ""activity"": 1,
  ""description"": ""api post test"",
  ""tags"": ""api test""
}";

            // DONT COMMIT sensitive login data here
            // replace to temporarily post to a real api on the internet

            //            httpClient = new HttpClient();
            //            postUrl = "https://posturl";
            //            userNameKey = "X-AUTH-USER";
            //            apiKeyName = "X-AUTH-TOKEN";
            //            userName = "username";
            //            apiKey = "apikey";
            //            postTemplate = @"{
            //  ""begin"": ""2023-03-15T18:32:56"",
            //  ""end"": ""2023-03-15T18:33:56"",
            //  ""project"": 1,
            //  ""activity"": 1,
            //  ""description"": ""api post test"",
            //  ""tags"": ""api test""
            //}";

            // DONT COMMIT sensitive login data here

            var exportService = new ExportService();
            var timesheetEntry = new JobTimeSheetEntry(job: Job.NewDefault(), DateTime.Now.Subtract(TimeSpan.FromMinutes(1)), DateTime.Now);

            var exportProfile = new ApiExportProfile(
                name: "TestExportProfile",
                postUrl: postUrl,
                headers: new Dictionary<string, string>()
                {
                    { userNameKey, userName },
                    { apiKeyName, apiKey }
                },
                jobNameJsonTemplates: new Dictionary<string, string>()
                {
                    { Job.NewDefault().Name, postTemplate }
                },
                startTimeToken: "_RoutinRStartTime_",
                endTimeToken: "_RoutinREndTime_");

            var result = exportService.ExportToApi(timesheetEntry, exportProfile, httpClient).GetAwaiter().GetResult();

            Assert.True(result != null, "Result has no value");
            Assert.True(!result.HasError, "Result for sample api has error");


        }
    }
}