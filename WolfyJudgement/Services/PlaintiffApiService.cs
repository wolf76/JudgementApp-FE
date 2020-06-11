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
    public class PlaintiffApiService
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

        public PlaintiffData GetPlaintiffs(long pageNumber, string sortQuery, string searchQuery)
        {
            using (var client = new HttpClient(GetInsecureHandler()))
            {
                string uri = $"{BaseUrl}/plaintiffs?pageNumber={pageNumber}&pageSize=15";
                if (sortQuery != "")
                    uri += $"&sort={sortQuery}";

                if (searchQuery != "")
                    uri += $"&search={searchQuery}";

                var response = client.GetStringAsync(uri);
                string result = response.GetAwaiter().GetResult();

                var PlaintiffsList = JsonConvert.DeserializeObject<PlaintiffData>(result);

                return PlaintiffsList;
            };
        }

        public Plaintiffs GetSelectedPlaintiff(object selectedPlaintiff)
        {
            var client = new HttpClient(GetInsecureHandler());

            var requestedCaseId = selectedPlaintiff.GetType().GetProperty("Id").GetValue(selectedPlaintiff, null);

            var response = client.GetStringAsync($"{BaseUrl}/plaintiffs/{requestedCaseId}");
            var result = response.GetAwaiter().GetResult();

            var SelectedPlaintiff = JsonConvert.DeserializeObject<Plaintiffs>(result);

            return SelectedPlaintiff;
        }

        public async Task<bool> EditPlaintiffAsync(object editedPlaintiff)
        {
            var client = new HttpClient(GetInsecureHandler());
            var caseID = editedPlaintiff.GetType().GetProperty("Id").GetValue(editedPlaintiff, null);

            var json = JsonConvert.SerializeObject(editedPlaintiff);

            HttpContent content = new StringContent(json);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PutAsync($"{BaseUrl}/plaintiffs/{caseID}", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddPlaintiffAsync(string firstName, string lastName,
            string middleName, string attorney, string address, int? caseId)
        {
            var client = new HttpClient(GetInsecureHandler());

            var model = new Plaintiffs
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

            var response = await client.PostAsync($"{BaseUrl}/plaintiffs", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeletePlaintiffAsync(object CaseID)
        {
            var client = new HttpClient(GetInsecureHandler());

            var response = await client.DeleteAsync($"{BaseUrl}/plaintiffs/{CaseID}");

            return response.IsSuccessStatusCode;
        }
    }
}
