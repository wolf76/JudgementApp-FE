using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace WolfyJudgement.ViewModels
{
    public class PlaintiffsViewModel
    {
        public PlaintiffsViewModel()
        {
            GetPlaintiffs();
        }

        private void GetPlaintiffs()
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
                string uri = "https://localhost:5001/plaintiffs";
                var response = client.GetStringAsync(uri);
                var result = response.GetAwaiter().GetResult();

                //handling the answer
                var PlaintiffsList = JsonConvert.DeserializeObject<List<Plaintiffs>>(result);

                PlaintiffsCollection = new ObservableCollection<Plaintiffs>(PlaintiffsList);
            };
        }

        public ObservableCollection<Plaintiffs> PlaintiffsCollection { get; private set; }
    }
}
