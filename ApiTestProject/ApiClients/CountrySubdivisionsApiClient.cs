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

    public class CountrySubdivisionsApiClient : ICountrySubdivisionsClient
    {
        public async Task<CountrySubdivision> GetCountrySubdivision(int id)
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(Path.Combine(ConfigurationManager.AppSettings["Endpoint.Countries"], "v1/CountrySubdivisions", id.ToString()));
                using (var response = await client.GetAsync(uri))
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<CountrySubdivision>(responseContent);
                    }

                    throw new HttpRequestException($"{response.StatusCode}: {responseContent}");
                }
            }
        }

        public async Task<CountrySubdivision> CreateCountrySubdivision(CountrySubdivision countrySubdivision)
        {
            var uri = new Uri(Path.Combine(ConfigurationManager.AppSettings["Endpoint.Countries"], "v1/CountrySubdivisions"));
            var content = new StringContent(JsonConvert.SerializeObject(countrySubdivision, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (var client = new HttpClient())
            {
                using (var response = await client.PostAsync(uri, content))
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<CountrySubdivision>(responseContent);
                    }

                    throw new HttpRequestException($"{response.StatusCode}: {responseContent}");
                }
            }
        }

        public async Task<CountrySubdivision> UpdateCountrySubdivision(CountrySubdivision countrySubdivision)
        {
            var uri = new Uri(Path.Combine(ConfigurationManager.AppSettings["Endpoint.Countries"], "v1/CountrySubdivisions"));
            var content = new StringContent(JsonConvert.SerializeObject(countrySubdivision, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (var client = new HttpClient())
            {
                using (var response = await client.PutAsync(uri, content))
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<CountrySubdivision>(responseContent);
                    }

                    throw new HttpRequestException($"{response.StatusCode}: {responseContent}");
                }
            }
        }

        public async Task DeleteCountrySubdivision(int id)
        {
            var uri = new Uri(Path.Combine(ConfigurationManager.AppSettings["Endpoint.Countries"], "v1/CountrySubdivisions"));
            using (var client = new HttpClient())
            {
                using (var response = await client.DeleteAsync(uri))
                {
                    if (!response.IsSuccessStatusCode) throw new Exception(response.Content.ToString());
                }
            }
        }
    }
}
