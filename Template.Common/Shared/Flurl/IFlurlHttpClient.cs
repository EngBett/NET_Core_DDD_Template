namespace Template.Common.Shared.Flurl
{
    public interface IFlurlHttpClient
    {
        Task DeleteAsync(string path, object queryParams = null, object headers = null, object cookies = null);
        Task<T> DeleteAsync<T>(string path, object queryParams = null, object headers = null, object cookies = null);
        Task<byte[]> GetBytesAsync(string path, object queryParams = null, object headers = null, object cookies = null);
        Task<T> GetJSON<T>(string path, object queryParams = null, object headers = null, object cookies = null);
        Task<string> PostUrlEncodedAsyncXml(string path, object payload = null, object headers = null,
                                  object cookies = null);
        Task<T> PostUrlEncodedAsyncXml<T>(string path, object payload = null, object headers = null,
                          object cookies = null);
        IList<dynamic> GetJsonArray(string path, object queryParams = null, object headers = null, object cookies = null);
        Task<string> GetString(string path, object queryParams = null, object headers = null, object cookies = null);
        Task<T> PostJSON<T>(string path, object payload = null, object headers = null, object cookies = null);
        Task PostJSONAsync(string path, object payload = null, object headers = null, object cookies = null);
        Task<string> PostJSONForString(string path, object payload = null, object headers = null, object cookies = null);
        Task PostUrlEncodedAsync(string path, object payload = null, object headers = null, object cookies = null);
        Task<T> PostUrlEncodedAsync<T>(string path, object payload = null, object headers = null, object cookies = null);
        Task PutJSONAsync(string path, object payload = null, object headers = null, object cookies = null);
        T PutJSONAsync<T>(string path, object payload = null, object headers = null, object cookies = null);
        Task<T> PostJSONForm<T>(string path,
           object payload = null,
           object file = null,
            object headers = null, object cookies = null);
        Task<string> UploadByteArrayAsync(string path, byte[] imageBytes, byte[] secondaryImageBytes, string token, ICollection<KeyValuePair<string, string>> payload = null);
    }
}