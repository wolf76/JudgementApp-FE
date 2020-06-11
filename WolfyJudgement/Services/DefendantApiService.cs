using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace WolfyJudgement.Services
{
    public class DefendantApiService
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

        public DefendantData GetDefendants(long pageNumber, string sortQuery, string searchQuery)
        {
            using (var client = new HttpClient(GetInsecureHandler()))
            {
                string uri = $"{BaseUrl}/defendants?pageNumber={pageNumber}&pageSize=15";
                if (sortQuery != "")
                    uri += $"&sort={sortQuery}";

                if (searchQuery != "")
                    uri += $"&search={searchQuery}";

                var response = client.GetStringAsync(uri);
                string result = response.GetAwaiter().GetResult();

                var DefendantsList = JsonConvert.DeserializeObject<DefendantData>(result);

                return DefendantsList;
            };
        }

        public Defendants GetSelectedDefendant(object selectedDefendant)
        {
            var client = new HttpClient(GetInsecureHandler());

            var requestedCaseId = selectedDefendant.GetType().GetProperty("Id").GetValue(selectedDefendant, null);

            var response = client.GetStringAsync($"{BaseUrl}/defendants/{requestedCaseId}");
            var result = response.GetAwaiter().GetResult();

            var SelectedDefendant = JsonConvert.DeserializeObject<Defendants>(result);

            return SelectedDefendant;
        }

        public async Task<bool> EditDefendantAsync(object editedDefendant)
        {
            var client = new HttpClient(GetInsecureHandler());
            var caseID = editedDefendant.GetType().GetProperty("Id").GetValue(editedDefendant, null);

            var json = JsonConvert.SerializeObject(editedDefendant);

            HttpContent content = new StringContent(json);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PutAsync($"{BaseUrl}/defendants/{caseID}", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddDefendantAsync(string firstName, string lastName,
            string middleName, string attorney, string address, int? caseId)
        {
            var client = new HttpClient(GetInsecureHandler());

            var model = new Defendants
            {
                FirstName = firstName,
                LastName = lastName,
                MiddleName = middleName,
                Attorney = attorney,
                Address = address,
                CaseId = caseId
            };

            var json = JsonConvert.SerializeObject(model);

            HttpContent content = new StringContent(json);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync($"{BaseUrl}/defendants", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteDefendantAsync(object CaseID)
        {
            var client = new HttpClient(GetInsecureHandler());

            var response = await client.DeleteAsync($"{BaseUrl}/defendants/{CaseID}");

            return response.IsSuccessStatusCode;
        }
    }
}
