using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace WolfyJudgement.Services
{
    public class CaseApiServices
    {
        public static string BaseUrl = Device.RuntimePlatform == Device.Android ? "https://10.0.2.2:5001" : "https://localhost:5001";

        public HttpClientHandler GetInsecureHandler()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }

        public CaseData GetCases(long pageNumber,string sortQuery, string searchQuery)
        {
            using (var client = new HttpClient(GetInsecureHandler()))
            {
                string uri = $"{BaseUrl}/cases?pageNumber={pageNumber}&pageSize=15";
                if(sortQuery != "")
                    uri += $"&sort={sortQuery}";

                if(searchQuery != "")
                    uri += $"&search={searchQuery}";

                var response = client.GetStringAsync(uri);
                string result = response.GetAwaiter().GetResult();

                var CasesList = JsonConvert.DeserializeObject<CaseData>(result);

                return CasesList;
            };
        }

        public Cases GetSelectedCase(object selectedCase)
        {
            var client = new HttpClient(GetInsecureHandler());

            var requestedCaseId = selectedCase.GetType().GetProperty("Id").GetValue(selectedCase, null);

            var response = client.GetStringAsync($"{BaseUrl}/cases/{requestedCaseId}");
            var result = response.GetAwaiter().GetResult();

            var SelectedCase = JsonConvert.DeserializeObject<Cases>(result);

            return SelectedCase;
        }

        public async Task<bool> EditCaseAsync(object editedCase)
        {
            var client = new HttpClient(GetInsecureHandler());
            var caseID = editedCase.GetType().GetProperty("Id").GetValue(editedCase, null);

            var json = JsonConvert.SerializeObject(editedCase);

            HttpContent content = new StringContent(json);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PutAsync($"{BaseUrl}/cases/{caseID}", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddCaseAsync(string type, decimal? amount,
            string courtType, string caseType, DateTime? fillingDate,
            string judge, string docketType, string description,
            string caseNo, string caseUrl)
        {
            var client = new HttpClient(GetInsecureHandler());

            var model = new Cases
            {
                Type = type,
                Amount = amount,
                CourtType = courtType,
                CaseType = caseType,
                FillingDate = fillingDate,
                Judge = judge,
                DocketType = docketType,
                Description = description,
                CaseNo = caseNo,
                CaseUrl = caseUrl
            };

            var json = JsonConvert.SerializeObject(model);

            HttpContent content = new StringContent(json);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync($"{BaseUrl}/cases", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCaseAsync(object CaseID)
        {
            var client = new HttpClient(GetInsecureHandler());

            var response = await client.DeleteAsync($"{BaseUrl}/cases/{CaseID}");

            return response.IsSuccessStatusCode;
        }
    }
}
