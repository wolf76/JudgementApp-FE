using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace WolfyJudgement.ViewModels
{
    public class DefendantsViewModel
    {
        public DefendantsViewModel()
        {
            GetDefendants();
        }

        private void GetDefendants()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };

            using (var client = new HttpClient(handler))
            {
                //send a GET request
                string uri = "https://localhost:5001/defendants";
                var response = client.GetStringAsync(uri);
                var result = response.GetAwaiter().GetResult();

                //handling the answer
                var DefendantsList = JsonConvert.DeserializeObject<List<Defendants>>(result);

                DefendantsCollection = new ObservableCollection<Defendants>(DefendantsList);
            };
        }

        public ObservableCollection<Defendants> DefendantsCollection { get; private set; }
    }
}
