namespace Core.Http;

public interface IHttpRequest
{
    Task<T?> GetAsync<T>(string url, string token = "") where T : class;
    Task<T?> DeleteAsync<T>(string url, string token = "") where T : class;
    Task<T?> PostAsync<T>(string url, object body, string token = "") where T : class;

    Task<TResult?> PostAsync<TBody, TResult>(string url, TBody body, string token = "") where TBody : notnull;
    Task<T?> PutAsync<T>(string url, object body, string token = "") where T : class;
}