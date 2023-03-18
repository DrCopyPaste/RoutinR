using RoutinR.Core;
using System.Text;

namespace RoutinR.Services
{
    public class ExportService
    {
        /// <summary>
        /// Exports a timeSheetEntry to an external api
        /// 
        /// httpClient's DefaultHeaders are cleared before applying headers from export profile
        /// </summary>
        /// <param name="timeSheetEntry">the entry to be exported</param>
        /// <param name="apiExportProfile">the export profile containing target url, headers, placeholders and post template</param>
        /// <param name="httpClient">the http client to be used with this post request</param>
        /// <returns></returns>
        public async Task<ExportResult> ExportToApi(TimeSheetEntry timeSheetEntry, ApiExportProfile apiExportProfile, HttpClient httpClient)
        {
            if (!apiExportProfile.JobNameJsonTemplates.Any(template => template.Key == timeSheetEntry.Job.Name)) return new ExportResult($"Export profile does not contain a template definition for job {timeSheetEntry.Job.Name}");

            var postTemplate = apiExportProfile.JobNameJsonTemplates.First(template => template.Key == timeSheetEntry.Job.Name).Value;

            postTemplate = postTemplate.Replace(apiExportProfile.StartTimeToken, timeSheetEntry.StartTime.ToString("yyyy-MM-ddTHH:mm"));
            postTemplate = postTemplate.Replace(apiExportProfile.EndTimeToken, timeSheetEntry.EndTime.ToString("yyyy-MM-ddTHH:mm"));

            var postContent = new StringContent(postTemplate, Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Clear();
            if (apiExportProfile.Headers != null)
            {
                foreach (var headerItem in apiExportProfile.Headers) httpClient.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
            }
            
            var httpResponse = await httpClient.PostAsync(apiExportProfile.PostUrl, postContent);
            
            if (httpResponse == null) return new ExportResult("Response is null");

            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            if (responseContent == null) return new ExportResult("Response content was empty");
            if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK) return new ExportResult($"Response was not OK ({httpResponse.StatusCode})");

            return new ExportResult();
        }
    }
}