using System.Net;
using System.Text;
using System.Text.Json;
using Core.Http;
using Core.Infrastructure.Http;
using Moq;
using Moq.Protected;

namespace Core.Test.Http;

public class HttpRequestTests
{
    private Mock<IHttpClientFactory> _mockFactory;
    private Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private HttpClient _httpClient;
    private string _clientName;
    private IHttpRequest _httpRequest;

    [SetUp]
    public void Setup()
    {
        _clientName = "testClient";
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://api.test.com/")
        };

        _mockFactory = new Mock<IHttpClientFactory>();
        _mockFactory.Setup(factory => factory.CreateClient(_clientName))
            .Returns(_httpClient);

        _httpRequest = new HttpRequestHandler(_mockFactory.Object, _clientName);
    }

    [Test]
    public async Task GetAsync_ReturnsExpectedResult()
    {
        // Arrange
        var testData = new TestModel { Id = 1, Name = "Test" };
        var jsonResponse = JsonSerializer.Serialize(testData);

        SetupMockResponse(HttpStatusCode.OK, jsonResponse);

        // Act
        var result = await _httpRequest.GetAsync<TestModel>("api/test");

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("Test"));

        VerifyHttpCall(HttpMethod.Get, "api/test", Times.Once());
    }

    [Test]
    public async Task GetAsync_WithToken_SetsAuthorizationHeader()
    {
        // Arrange
        var testData = new TestModel { Id = 1, Name = "Test" };
        var jsonResponse = JsonSerializer.Serialize(testData);
        var token = "test-token";

        SetupMockResponse(HttpStatusCode.OK, jsonResponse);

        // Act
        var result = await _httpRequest.GetAsync<TestModel>("api/test", token);

        // Assert
        Assert.IsNotNull(result);

        VerifyHttpCall(HttpMethod.Get, "api/test", Times.Once(), request => request.Headers.Authorization is
                                                                            {
                                                                                Scheme: "Bearer"
                                                                            } &&
                                                                            request.Headers.Authorization.Parameter ==
                                                                            token);
    }

    [Test]
    public async Task DeleteAsync_ReturnsExpectedResult()
    {
        // Arrange
        var testData = new TestModel { Id = 1, Name = "Test" };
        var jsonResponse = JsonSerializer.Serialize(testData);

        SetupMockResponse(HttpStatusCode.OK, jsonResponse);

        // Act
        var result = await _httpRequest.DeleteAsync<TestModel>("api/test/1");

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Id, Is.EqualTo(1));

        VerifyHttpCall(HttpMethod.Delete, "api/test/1", Times.Once());
    }

    [Test]
    public async Task PostAsync_SendsCorrectBodyAndReturnsExpectedResult()
    {
        // Arrange
        var requestBody = new TestModel { Id = 2, Name = "Post Test" };
        var responseData = new TestModel { Id = 2, Name = "Post Test Response" };
        var jsonResponse = JsonSerializer.Serialize(responseData);

        SetupMockResponse(HttpStatusCode.OK, jsonResponse);

        // Act
        var result = await _httpRequest.PostAsync<TestModel>("api/test", requestBody);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(2));
            Assert.That(result.Name, Is.EqualTo("Post Test Response"));
        });
        VerifyHttpCall(HttpMethod.Post, "api/test", Times.Once(), request => request.Content != null);
    }

    [Test]
    public async Task PostAsync_WithGenericTypes_ReturnsExpectedResult()
    {
        // Arrange
        var requestBody = new RequestModel { RequestId = 3, Message = "Test Request" };
        var responseData = new ResponseModel { ResponseId = 5, Status = "Success" };
        var jsonResponse = JsonSerializer.Serialize(responseData);

        SetupMockResponse(HttpStatusCode.OK, jsonResponse);

        // Act
        var result = await _httpRequest.PostAsync<RequestModel, ResponseModel>("api/process", requestBody);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.ResponseId, Is.EqualTo(5));
            Assert.That(result.Status, Is.EqualTo("Success"));
        });
        VerifyHttpCall(HttpMethod.Post, "api/process", Times.Once(), request => request.Content != null);
    }

    [Test]
    public async Task PutAsync_SendsCorrectBodyAndReturnsExpectedResult()
    {
        // Arrange
        var requestBody = new TestModel { Id = 4, Name = "Put Test" };
        var responseData = new TestModel { Id = 4, Name = "Put Test Response" };
        var jsonResponse = JsonSerializer.Serialize(responseData);

        SetupMockResponse(HttpStatusCode.OK, jsonResponse);

        // Act
        var result = await _httpRequest.PutAsync<TestModel>("api/test/4", requestBody);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(4));
            Assert.That(result.Name, Is.EqualTo("Put Test Response"));
        });
        VerifyHttpCall(HttpMethod.Put, "api/test/4", Times.Once(), request => request.Content != null);
    }

    [Test]
    public void HttpRequest_ThrowsException_WhenResponseIsNotSuccessful()
    {
        // Arrange
        SetupMockResponse(HttpStatusCode.BadRequest, "Bad Request");

        // Act & Assert
        Assert.ThrowsAsync<HttpRequestException>(async () =>
            await _httpRequest.GetAsync<TestModel>("api/test"));
    }

    private void SetupMockResponse(HttpStatusCode statusCode, string content)
    {
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            });
    }

    private void VerifyHttpCall(
        HttpMethod method,
        string url,
        Times times,
        Func<HttpRequestMessage, bool>? match = null)
    {
        _mockHttpMessageHandler
            .Protected()
            .Verify(
                "SendAsync",
                times,
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == method &&
                    req.RequestUri != null &&
                    req.RequestUri.ToString().EndsWith(url) &&
                    (match == null || match(req))),
                ItExpr.IsAny<CancellationToken>());
    }
}

// Models for testing
public class TestModel
{
    public int Id { get; init; }
    public required string Name { get; init; }
}

public class RequestModel
{
    public int RequestId { get; set; }
    public required string Message { get; set; }
}

public class ResponseModel
{
    public int ResponseId { get; init; }
    public required string Status { get; init; }
}