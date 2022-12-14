using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace Template.Common.Shared.Flurl
{
    public class FlurlHttpClient : IFlurlHttpClient
    {
        private readonly ILogger<FlurlHttpClient> _logger;

        /// <summary>
        /// Commented out all Config.REST_REQUEST_TIMEOUT Temporarily.
        /// 
        ///
        private readonly int TimeOut = 120;
        private FlurlClient flurlClient;
        public FlurlHttpClient(ILogger<FlurlHttpClient> logger)
        {
            _logger = logger;
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain,
              errors) => true;
            HttpClient httpClient = new HttpClient(httpClientHandler);
            flurlClient = new FlurlClient(httpClient);
        }
        public async Task<T> GetJSON<T>(string path, object queryParams = null, object headers = null, object cookies = null)
        {

            #region leye implementation
            try
            {
                T result = await this.flurlClient.Request(path)
                    .SetQueryParams(queryParams ?? new { })
                    .AllowAnyHttpStatus()
                    .WithCookies(cookies ?? new { })
                    .WithTimeout(60)
                    .WithHeaders(headers ?? new { })
                    .GetAsync().ReceiveJson<T>();

                _logger.LogInformation("Using Flurl to call api endpoint with values {@endpoint}|{@response}", path, JsonConvert.SerializeObject(result));


                return result;
            }
            catch (FlurlHttpException ex)
            {
                this._logger.LogError(ex, "Error Occurred");
                return default(T);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error Occurred");
                return default(T);
            }

            #endregion


        }

        public IList<dynamic> GetJsonArray(string path, object queryParams = null, object headers = null, object cookies = null)
        {

            try
            {
                Task<IList<dynamic>> result = new Url(path)
                    .SetQueryParams(queryParams ?? new { })
                    .WithCookies(cookies ?? new { })
                    .AllowAnyHttpStatus()
                    .WithTimeout(TimeOut)
                    .WithHeaders(headers ?? new { })
                    .GetJsonAsync<IList<dynamic>>();
                

                return result.Result;
            }
            catch (TaskCanceledException)
            {
                return default(dynamic);
            }
        }
        public async Task<string> GetString(string path, object queryParams = null, object headers = null,
                object cookies = null)
        {
            try
            {
                return await new Url(path)
                    .SetQueryParams(queryParams ?? new { })
                    .WithCookies(cookies ?? new { })
                   .WithTimeout(TimeOut)
                       .AllowAnyHttpStatus()
                    .WithHeaders(headers ?? new { })
                    .GetStringAsync();
            }
            catch (TaskCanceledException)
            {
                return string.Empty;
            }
            catch (IOException)
            {
                return string.Empty;
            }
        }
        public async Task<T> PostJSON<T>(string path, object payload = null,
             object headers = null, object cookies = null)
        {
            try
            {
                T result = await flurlClient.Request(path)
                    .WithTimeout(TimeOut)
                        .AllowHttpStatus("200,400,404")
                    .WithCookies(cookies ?? new { })
                                   .WithHeaders(headers ?? new { })
                                    .PostJsonAsync(payload ?? new object()).ReceiveJson<T>();
                _logger.LogInformation("Using Flurl to call api endpoint with values {@endpoint}|{@request}|{@response}", path, payload, result);



                return result;

            }
            catch (FlurlHttpException ex)
            {
                _logger.LogError(ex, "Error Occurred Flurl {@request}", payload);
                return default(T);
            }
            catch (TaskCanceledException)
            {
                return default(T);
            }
            catch (IOException)
            {
                return default(T);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<string> PostUrlEncodedAsyncXml(string path, object payload = null, object headers = null,
                                  object cookies = null)
        {

            string result = await flurlClient.Request(path).WithCookies(cookies ?? new { }).WithTimeout(TimeOut)
                               .WithHeaders(headers ?? new { })
                                   .AllowHttpStatus("200")
                                      .PostUrlEncodedAsync(payload ?? new object()).ReceiveString();
            return result;


        }
        public async Task<T> PostUrlEncodedAsyncXml<T>(string path, object payload = null, object headers = null,
                          object cookies = null)
        {
            return await flurlClient.Request(path).WithCookies(cookies ?? new { }).WithTimeout(TimeOut)
                              .WithHeaders(headers ?? new { })
                                  .AllowHttpStatus("200")
                                     .PostUrlEncodedAsync(payload ?? new object()).ReceiveJson<T>();



        }
        public async Task<T> PostJSONForm<T>(string path,
            object payload = null,
            object file = null,
             object headers = null, object cookies = null)
        {
            try
            {


                T resp = await $"{path}"
                    .PostMultipartAsync(mp => mp
                        .AddFile("File", $"{file}")
                        // .AddFile("File", file.OpenReadStream(), "")

                        .AddJson("json", payload)).ReceiveJson<T>();
                return resp;

            }
            catch (FlurlHttpException ex)
            {
                if (ex.Call.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw ex;
                }

                Task<T> response = ex.GetResponseJsonAsync<T>();
                return response.Result;
            }
            catch (TaskCanceledException)
            {
                return default(T);
            }
            catch (IOException)
            {
                return default(T);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public async Task<string> PostJSONForString(string path, object payload = null, object headers = null,
                                                  object cookies = null)
        {
            try
            {
                string result = await new Url(path)
                    .WithTimeout(TimeOut)
                        .AllowAnyHttpStatus()
                            .AllowAnyHttpStatus()
                    .WithCookies(cookies ?? new { })
                                   .WithHeaders(headers ?? new { })
                    .PostJsonAsync(payload ?? new object()).ReceiveString();
                return result;
            }
            catch (TaskCanceledException)
            {
                return string.Empty;
            }
            catch (IOException)
            {
                return string.Empty;
            }
        }
        public async Task PostJSONAsync(string path, object payload = null, object headers = null,
                                                  object cookies = null)
        {
            try
            {
                await new Url(path).WithCookies(cookies ?? new { }).WithTimeout(TimeOut)
                                  .WithHeaders(headers ?? new { })
                                   .PostJsonAsync(payload ?? new object());
            }
            catch (TaskCanceledException)
            {
            }
            catch (IOException)
            {
            }
        }



        public async Task<string> UploadByteArrayAsync(string path, byte[] imageBytes, byte[] secondaryImageBytes, string token, ICollection<KeyValuePair<string, string>> payload = null)
        {
            try
            {
                using (Stream primaryFileStream = new MemoryStream(imageBytes))
                {
                    using (Stream secondaryFileStream = secondaryImageBytes == null ? new MemoryStream() : new MemoryStream(secondaryImageBytes))
                    {
                        return await new Url(path).WithTimeout(TimeOut).PostMultipartAsync((mp) =>
                        {
                            mp.AddFile("File", primaryFileStream, "my_uploaded_image.jpg");
                            if (secondaryImageBytes != null)
                            {
                                mp.AddFile("BackFile", secondaryFileStream, "my_secondary_uploaded_image.jpg");
                            }
                            if (payload != null)
                            {
                                foreach (KeyValuePair<string, string> item in payload)
                                {
                                    mp.AddString(item.Key, item.Value);
                                }
                            }
                        }).ReceiveJson<string>();
                    }
                }
            }
            catch (TaskCanceledException)
            {
                return string.Empty;
            }
            catch (IOException)
            {
                return string.Empty;
            }
        }
        public async Task<T> PostUrlEncodedAsync<T>(string path, object payload = null, object headers = null,
                                                 object cookies = null)
        {
            try
            {
                T result = await flurlClient.Request(path).WithCookies(cookies ?? new { }).WithTimeout(TimeOut)
                                   .WithHeaders(headers ?? new { })
                                       .AllowAnyHttpStatus()
                                          .PostUrlEncodedAsync(payload ?? new object()).ReceiveJson<T>();

                return result;
            }
            catch (FlurlHttpException ex)
            {

                Task<T> error = ex.GetResponseJsonAsync<T>();
                return error.Result;
            }
            catch (TaskCanceledException)
            {
                return default;
            }
            catch (IOException)
            {
                return default;
            }

        }
        public async Task PostUrlEncodedAsync(string path, object payload = null, object headers = null,
                                                   object cookies = null)
        {
            try
            {
                await new Url(path).WithCookies(cookies ?? new { }).WithTimeout(TimeOut)
                                  .WithHeaders(headers ?? new { })
                                   .PostUrlEncodedAsync(payload ?? new object());
            }
            catch (TaskCanceledException)
            {

            }
            catch (IOException)
            {

            }

            catch (FlurlHttpException)
            {


            }
        }
        public T PutJSONAsync<T>(string path, object payload = null, object headers = null,
                                                  object cookies = null)
        {
            try
            {
                return new Url(path).WithCookies(cookies ?? new { }).WithTimeout(TimeOut)
                                   .WithHeaders(headers ?? new { })
                                          .PutJsonAsync(payload ?? new object()).ReceiveJson<T>().Result;
            }
            catch (TaskCanceledException)
            {
                return default(T);
            }

            catch (FlurlHttpException ex)
            {
                Task<T> error = ex.GetResponseJsonAsync<T>();
                return error.Result;
            }

        }



        public async Task PutJSONAsync(string path, object payload = null, object headers = null,
                                                      object cookies = null)
        {
            try
            {
                await new Url(path).WithCookies(cookies ?? new { }).WithTimeout(TimeOut)
                                  .WithHeaders(headers ?? new { })
                                   .PutJsonAsync(payload ?? new object());
            }
            catch (TaskCanceledException)
            {

            }
            catch (IOException)
            {

            }

        }
        public async Task<T> DeleteAsync<T>(string path,
                               object queryParams = null, object headers = null,
                              object cookies = null)
        {
            try
            {
                return await new Url(path)
                         .SetQueryParams(queryParams ?? new { })
                    .WithCookies(cookies ?? new { }).WithTimeout(TimeOut)
                                   .WithHeaders(headers ?? new { })
                    .DeleteAsync().ReceiveJson<T>();
            }
            catch (TaskCanceledException)
            {
                return default(T);
            }
            catch (IOException)
            {
                return default(T);
            }
        }
        public async Task DeleteAsync(string path,
                               object queryParams = null, object headers = null,
                              object cookies = null)
        {
            try
            {
                await new Url(path)
                   .SetQueryParams(queryParams ?? new { }).WithTimeout(TimeOut)
                   .WithCookies(cookies ?? new { })
                                  .WithHeaders(headers ?? new { })
                                         .DeleteAsync();
            }
            catch (TaskCanceledException)
            {

            }
            catch (IOException)
            {

            }
        }
        public async Task<byte[]> GetBytesAsync(string path,
                               object queryParams = null, object headers = null,
                              object cookies = null)
        {
            try
            {
                return await new Url(path)
                   .SetQueryParams(queryParams ?? new { }).WithTimeout(60)
                   .WithCookies(cookies ?? new { })
                    .WithHeaders(headers ?? new { })
                    .GetBytesAsync();
            }
            catch (TaskCanceledException)
            {
                return default(byte[]);
            }
            catch (IOException)
            {
                return default(byte[]);
            }
        }
    }
}