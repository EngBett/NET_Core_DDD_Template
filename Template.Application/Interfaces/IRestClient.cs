using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Template.Application.Interfaces;

public interface IRestClient
{
    Task<(M response, HttpStatusCode statusCode)> SendAsync<M>(string requestpath, string httpclientname, HttpMethod method, Dictionary<string, string> headers, object payload = null);
    Task<(M response, HttpStatusCode statusCode)> PostFormData<M>(string requestpath, string httpclientname, HttpMethod method, Dictionary<string, string> headers, FormUrlEncodedContent payload = null);
}
