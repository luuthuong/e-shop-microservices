using System.Net.Http.Headers;
using System.Text;
using Core.Http;
using Newtonsoft.Json;

namespace Core.Infrastructure.Http;

public class HttpRequestHandler(HttpClient httpClient) : IHttpRequest
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    private const string TokenScheme = "Bearer";

    public async Task<T?> GetAsync<T>(string url, string token = "") where T : class
    {
        AddToken(token);
        var response = await _httpClient.GetAsync(url);
        return await DeserializeResponse<T>(response);
    }

    public async Task<T?> DeleteAsync<T>(string url, string token = "") where T : class
    {
        AddToken(token);
        var httpMessage = new HttpRequestMessage(HttpMethod.Delete, url);
        var response = await _httpClient.SendAsync(httpMessage);
        return await DeserializeResponse<T>(response);
    }

    public async Task<T?> PostAsync<T>(string url, object body, string token = "") where T : class
    {
        AddToken(token);
        var response = await _httpClient.PostAsync(url, SerializeBody(body));
        return await DeserializeResponse<T>(response);
    }

    public async Task<T?> PutAsync<T>(string url, object body, string token = "") where T : class
    {
        AddToken(token);
        var response = await _httpClient.PutAsync(url, SerializeBody(body));
        return await DeserializeResponse<T>(response);
    }

    private void AddToken(string token)
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
        var deserializeObject = JsonConvert.DeserializeObject<T>(content);
        return deserializeObject;
    }
}