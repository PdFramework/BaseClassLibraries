namespace ApiClients
{
    using Contracts;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Configuration;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class CountriesApiClient : ICountriesClient
    {
        public async Task<Country> GetCountry(int id)
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(Path.Combine(ConfigurationManager.AppSettings["Countries.Endpoint"], "v1/Countries", id.ToString()));
                using (var response = await client.GetAsync(uri))
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<Country>(responseContent);
                    }

                    throw new HttpRequestException($"{response.StatusCode}: {responseContent}");
                }
            }
        }

        public async Task<Country> CreateCountry(Country country)
        {
            var uri = new Uri(Path.Combine(ConfigurationManager.AppSettings["Countries.Endpoint"], "v1/Countries"));
            var content = new StringContent(JsonConvert.SerializeObject(country, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (var client = new HttpClient())
            {
                using (var response = await client.PostAsync(uri, content))
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<Country>(responseContent);
                    }

                    throw new HttpRequestException($"{response.StatusCode}: {responseContent}");
                }
            }
        }

        public async Task<Country> UpdateCountry(Country country)
        {
            var uri = new Uri(Path.Combine(ConfigurationManager.AppSettings["Countries.Endpoint"], "v1/Countries"));
            var content = new StringContent(JsonConvert.SerializeObject(country, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (var client = new HttpClient())
            {
                using (var response = await client.PutAsync(uri, content))
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<Country>(responseContent);
                    }

                    throw new HttpRequestException($"{response.StatusCode}: {responseContent}");
                }
            }
        }

        public async Task DeleteCountry(int id)
        {
            var uri = new Uri(Path.Combine(ConfigurationManager.AppSettings["Countries.Endpoint"], "v1/Countries", id.ToString()));
            using (var client = new HttpClient())
            {
                using (var response = await client.DeleteAsync(uri))
                {
                    if (!response.IsSuccessStatusCode) throw new HttpRequestException(response.Content.ToString());
                }
            }
        }
    }
}
