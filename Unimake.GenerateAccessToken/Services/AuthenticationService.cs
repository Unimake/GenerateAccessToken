using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Unimake.GenerateAccessToken.Models;

namespace Unimake.GenerateAccessToken.Services
{
    public class AuthenticationService
    {
        #region Private Fields

        private static JsonSerializerSettings _settings;
        private HttpClient httpClient;

        #endregion Private Fields

        #region Private Properties

        private static JsonSerializerSettings JsonSerializerSettings => _settings ??= new JsonSerializerSettings();

        #endregion Private Properties

        #region Private Methods

        private static string EnsureURL() => $"https://authserver.online/api/auth";

        private static void PrepareRequest(AuthRequest authRequest, HttpRequestMessage request, string url)
        {
            var content = new StringContent(JsonConvert.SerializeObject(authRequest, JsonSerializerSettings));
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            request.Content = content;
            request.Method = new HttpMethod("POST");
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);
        }

        private static async Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(HttpResponseMessage response)
        {
            if (response == null || response.Content == null)
            {
                return new ObjectResponseResult<T>(default(T), string.Empty);
            }

            {
                try
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    using (var streamReader = new System.IO.StreamReader(responseStream))
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        var serializer = JsonSerializer.Create(JsonSerializerSettings);
                        var typedBody = serializer.Deserialize<T>(jsonTextReader);
                        return new ObjectResponseResult<T>(typedBody, string.Empty);
                    }
                }
                catch (JsonException exception)
                {
                    var message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
                    throw new ApiException(message, response.StatusCode, string.Empty, exception);
                }
            }
        }

        #endregion Private Methods

        #region Protected Methods

        protected async Task<AuthResponse> ProcessResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var objectResponse = await ReadObjectResponseAsync<AuthResponse>(response).ConfigureAwait(false);

                if (objectResponse.Object == null)
                {
                    throw new ApiException("Response was null which was not expected.", response.StatusCode, objectResponse.Text);
                }

                return objectResponse.Object;
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var objectResponse_ = await ReadObjectResponseAsync<Error>(response).ConfigureAwait(false);

                if (objectResponse_.Object == null)
                {
                    throw new ApiException("Response was null which was not expected.", response.StatusCode, objectResponse_.Text);
                }

                throw new ApiException("Server Error", response.StatusCode, objectResponse_.Text);
            }
            else
            {
                var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new ApiException("The HTTP status code of the response was not expected (" + response.StatusCode + ").", response.StatusCode, responseData);
            }
        }

        #endregion Protected Methods

        #region Public Constructors

        public AuthenticationService() => httpClient = new HttpClient();

        #endregion Public Constructors

        #region Public Methods

        public async Task<AuthResponse> AuthAsync(AuthRequest authRequest)
        {
            try
            {
                using var request = new HttpRequestMessage();
                PrepareRequest(authRequest, request, EnsureURL());
                var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

                try
                {
                    return await ProcessResponse(response);
                }
                finally
                {
                    response.Dispose();
                }
            }
            finally
            {
                httpClient.Dispose();
            }
        }

        #endregion Public Methods
    }
}