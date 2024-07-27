
using System.Net;
using Newtonsoft.Json;
using RevenueRecognitionTest.IntegrationTests.Config;


namespace RevenueRecognitionTest.IntegrationTests;


public class RevenueIntegrationTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    private const string AuthToken =
        "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL" +
        "3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsidXNlciIsImFkbWluIl0sImV4cCI6" +
        "MTcyMDc3MzY2NiwiaXNzIjoiS08tVG9rZW5zIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9" +
        "zdDo1MDIyIn0.dTfg5WRiZhOlT-6au06KpqgGd0PNa36m0eFtYzSkPQE";


    public RevenueIntegrationTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }


    [Fact]
    public async Task GetActualRevenue_ReturnsSuccess()
    {
        _factory.ResetDatabase();
        var request = "/api/revenue/actual?&currency=usd";
        _client.DefaultRequestHeaders.Add("Authorization", AuthToken);
        var response = await _client.GetAsync(request);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseString);
        
    }

    [Fact]
    public async Task GetActualRevenue_ThrowsNotFoundException()
    {
        _factory.ResetDatabase();
        var request = "/api/revenue/actual?productId=10&currency=usd";
        _client.DefaultRequestHeaders.Add("Authorization", AuthToken);
        var response = await _client.GetAsync(request);
        if (response.StatusCode != HttpStatusCode.NotFound)
        {
            Assert.Fail();
        }

    }
    
    [Fact]
    public async Task GetActualRevenue_ReturnsBadRequest()
    {
        _factory.ResetDatabase();
        var request = "/api/revenue/actual?currency=usasdfsd";
        _client.DefaultRequestHeaders.Add("Authorization", AuthToken);
        var response = await _client.GetAsync(request);
        if (response.StatusCode != HttpStatusCode.BadRequest)
        {
            Assert.Fail();
        }

    }
    
    [Fact]
    public async Task GetExpectedRevenue_ReturnsSuccess()
    {
        _factory.ResetDatabase();
        var request = "/api/revenue/expected?currency=usd";
        _client.DefaultRequestHeaders.Add("Authorization", AuthToken);
        var response = await _client.GetAsync(request);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseString);
        
    }

    [Fact]
    public async Task GetExpectedRevenue_ThrowsNotFoundException()
    {
        _factory.ResetDatabase();
        var request = "/api/revenue/expected?productId=10&currency=usd";
        _client.DefaultRequestHeaders.Add("Authorization", AuthToken);
        var response = await _client.GetAsync(request);
        if (response.StatusCode != HttpStatusCode.NotFound)
        {
            Assert.Fail();
        }

    }
    
    [Fact]
    public async Task GetExpectedRevenue_ReturnsBadRequest()
    {
        _factory.ResetDatabase();
        var request = "/api/revenue/expected?currency=usasdfsd";
        _client.DefaultRequestHeaders.Add("Authorization", AuthToken);
        var response = await _client.GetAsync(request);
        if (response.StatusCode != HttpStatusCode.BadRequest)
        {
            Assert.Fail();
        }

    }




}