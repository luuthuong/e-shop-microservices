using System.Net.Http.Headers;
using System.Text;
using Core.Http;
using Core.Infrastructure.Reflections;
using Newtonsoft.Json;

namespace Core.Infrastructure.Http;

public class HttpRequestHandler(IHttpClientFactory httpClientFactory, string httpClientName = HttpClientNames.Default) : IHttpRequest
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(httpClientName);
    
    private const string TokenScheme = "Bearer";

    public async Task<T?> GetAsync<T>(string url, string token = "") where T : class
    {
        TryAddToken(token);
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await DeserializeResponse<T>(response);
    }

    public async Task<T?> DeleteAsync<T>(string url, string token = "") where T : class
    {
        TryAddToken(token);
        var httpMessage = new HttpRequestMessage(HttpMethod.Delete, url);
        var response = await _httpClient.SendAsync(httpMessage);
        response.EnsureSuccessStatusCode();
        return await DeserializeResponse<T>(response);
    }

    public async Task<T?> PostAsync<T>(string url, object body, string token = "") where T : class
    {
        TryAddToken(token);
        var response = await _httpClient.PostAsync(url, SerializeBody(body));
        response.EnsureSuccessStatusCode();
        return await DeserializeResponse<T>(response);
    }

    public async Task<TResult?> PostAsync<TBody, TResult>(string url, TBody body, string token = "") where TBody : notnull
    {
        TryAddToken(token);
        var response = await _httpClient.PostAsync(url, SerializeBody(body));
        response.EnsureSuccessStatusCode();
        return await DeserializeResponse<TResult>(response);
    }

    public async Task<T?> PutAsync<T>(string url, object body, string token = "") where T : class
    {
        TryAddToken(token);
        var response = await _httpClient.PutAsync(url, SerializeBody(body));
        response.EnsureSuccessStatusCode();
        return await DeserializeResponse<T>(response);
    }

    private void TryAddToken(string token)
    {
        if (!string.IsNullOrEmpty(token))
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TokenScheme, token);
    }

    private static StringContent SerializeBody(object body)
    {
        if (body is null)
            throw new ArgumentNullException(nameof(body));
        var json = JsonConvert.SerializeObject(body);
        return new(json, Encoding.UTF8, "application/json");
    }

    private static async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        var deserializeObject = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings()
        {
            ContractResolver = new PrivateResolver()
        });
        return deserializeObject;
    }
}