namespace Core.Http;

public interface IHttpRequest
{
    Task<T?> GetAsync<T>(string url, string token = "") where T : class;
    Task<T?> DeleteAsync<T>(string url, string token = "") where T : class;
    Task<T?> PostAsync<T>(string url, object body, string token = "") where T : class;
    Task<T?> PutAsync<T>(string url, object body, string token = "") where T : class;
}