namespace PeinearyDevelopment.Framework.BaseClassLibraries.Apis.Clients
{
    using Contracts;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public static class ApiInvoker
    {
        /// <summary>
        /// Used to invoke an api endpoint that will return the uniquely identifier resource of type T located at the route.
        /// </summary>
        /// <typeparam name="TContract">The type of the object to return from the api call.</typeparam>
        /// <typeparam name="TId">The type of the id property of T.</typeparam>
        /// <param name="endpointKey">Identifies the endpoint. Used by the endpoint resolver to determine the base path of the api to invoke.</param>
        /// <param name="route">The part of the Uri that is used by the api to determine where to route the request.</param>
        /// <param name="resourceIdentifier">The value that uniquely identifies the resource to be returned.</param>
        /// <returns>Object of type T deserialized from JSON object.</returns>
        public static async Task<TContract> Get<TContract, TId>(string endpointKey, string route, TId resourceIdentifier)
        {
            return await Get<TContract>(GenerateUri(endpointKey, route, resourceIdentifier.ToString()));
        }

        /// <summary>
        /// Used to invoke an api endpoint that will delete the uniquely identified resource located at the route.
        /// </summary>
        /// <typeparam name="TId">The type of the id property of the resource to delete.</typeparam>
        /// <param name="endpointKey">Identifies the endpoint. Used by the endpoint resolver to determine the base path of the api to invoke.</param>
        /// <param name="route">The part of the Uri that is used by the api to determine where to route the request.</param>
        /// <param name="resourceIdentifier">The value that uniquely identifies the resource to be deleted.</param>
        public static async Task Delete<TId>(string endpointKey, string route, TId resourceIdentifier)
        {
            await Delete(GenerateUri(endpointKey, route, resourceIdentifier.ToString()));
        }

        /// <summary>
        /// Used to invoke an api endpoint that will create a given resource.
        /// </summary>
        /// <typeparam name="TContract">The type of the resource to being created and returned.</typeparam>
        /// <param name="endpointKey">Identifies the endpoint. Used by the endpoint resolver to determine the base path of the api to invoke.</param>
        /// <param name="route">The part of the Uri that is used by the api to determine where to route the request.</param>
        /// <param name="postBodyContent">The resource to be created.</param>
        /// <returns>The created resource.</returns>
        public static async Task<TContract> Create<TContract>(string endpointKey, string route, TContract postBodyContent)
        {
            return await Post<TContract, TContract>(GenerateUri(endpointKey, route), postBodyContent);
        }

        /// <summary>
        /// Used to invoke an api endpoint that will update a given resource.
        /// </summary>
        /// <typeparam name="TContract">The type of the resource to being created and returned.</typeparam>
        /// <param name="endpointKey">Identifies the endpoint. Used by the endpoint resolver to determine the base path of the api to invoke.</param>
        /// <param name="route">The part of the Uri that is used by the api to determine where to route the request.</param>
        /// <param name="putBodyContent">The resource containing the updated values. Must contain a field that uniquely identifies the resource to be updated.</param>
        /// <returns>The updated resource.</returns>
        public static async Task<TContract> Update<TContract>(string endpointKey, string route, TContract putBodyContent)
        {
            return await Update(GenerateUri(endpointKey, route), putBodyContent);
        }

        private static Uri GenerateUri(string endpointKey, params string[] routeParts)
        {
            return new Uri(Path.Combine(new[] { ConfigurationManager.AppSettings[endpointKey] }.Concat(routeParts).ToArray()));
        }

        private static async Task<TResponse> Post<TBodyContent, TResponse>(Uri uri, TBodyContent postBodyContent)
        {
            var content = new StringContent(JsonConvert.SerializeObject(postBodyContent, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (var client = new HttpClient())
            {
                using (var response = await client.PostAsync(uri, content).ConfigureAwait(false))
                {
                    return await GetContentObject<TResponse>(response);
                }
            }
        }

        private static async Task<T> Get<T>(Uri uri)
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(uri).ConfigureAwait(false))
                {
                    return await GetContentObject<T>(response);
                }
            }
        }

        private static async Task<T> Update<T>(Uri uri, T putBodyContent)
        {
            var content = new StringContent(JsonConvert.SerializeObject(putBodyContent, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (var client = new HttpClient())
            {
                using (var response = await client.PutAsync(uri, content).ConfigureAwait(false))
                {
                    return await GetContentObject<T>(response);
                }
            }
        }

        private static async Task Delete(Uri uri)
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.DeleteAsync(uri).ConfigureAwait(false))
                {
                    if (!response.IsSuccessStatusCode) throw new ApiInvokerException(response);
                }
            }
        }

        private static async Task<T> GetContentObject<T>(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(responseContent);
            }

            throw new ApiInvokerException(response);
        }
    }
}
