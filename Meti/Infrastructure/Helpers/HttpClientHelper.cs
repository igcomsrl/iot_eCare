//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace Meti.Infrastructure.Helpers
{
    public static class HttpClientHelper
    {
        public static async Task<HttpClientResponseModel> Get(string baseAddress, string endpoint, bool configureAwait)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);

                //Richiesta http Get
                HttpResponseMessage sendAsyncResult = await client.GetAsync(endpoint).ConfigureAwait(configureAwait);

                var result = await sendAsyncResult.Content.ReadAsAsync<HttpClientResponseModel>();

                HttpClientResponseModel response = new HttpClientResponseModel();
                response.HttpStatusCode = sendAsyncResult.StatusCode;
                response.IsStatusSuccessCode = sendAsyncResult.IsSuccessStatusCode;

                if (!sendAsyncResult.IsSuccessStatusCode)
                {
                    response.Response = sendAsyncResult.Content.ReadAsStringAsync().Result;
                    return response;
                }

                response.Response = result;

                return response;
            }
        }
    }
}
