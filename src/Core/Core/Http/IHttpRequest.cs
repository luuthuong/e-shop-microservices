namespace Core.Http;

public interface IHttpRequest
{
    Task<T?> GetAsync<T>(string url, object? query, string token = "") where T : class;
    Task<T?> DeleteAsync<T>(string url, object? query, string token = "") where T : class;
    Task<T?> PostAsync<T>(string url, object body, string token = "") where T : class;
    Task<T?> PutAsync<T>(string url, object body, string token = "") where T : class;
}