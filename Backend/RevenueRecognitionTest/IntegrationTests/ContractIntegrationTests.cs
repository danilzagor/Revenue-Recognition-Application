using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RevenueRecognition.Controllers;
using RevenueRecognition.Models;
using RevenueRecognition.RequestModels.ContractRequestModels;
using RevenueRecognition.RequestModels.TransactionRequestModels;
using RevenueRecognition.ResponseModels.ContractResponseModels;
using RevenueRecognitionTest.IntegrationTests.Config;
using Xunit.Abstractions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace RevenueRecognitionTest.IntegrationTests;

public class ContractControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;

    private const string AuthToken = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL" +
                                     "3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsidXNlciIsImFkbWluIl0sImV4cCI6" +
                                     "MTcyMDc3MzY2NiwiaXNzIjoiS08tVG9rZW5zIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9" +
                                     "zdDo1MDIyIn0.dTfg5WRiZhOlT-6au06KpqgGd0PNa36m0eFtYzSkPQE";
    
    
    public ContractControllerTest(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = _factory.CreateClient();
    }


    [Fact]
    public async void AddContract_ReturnsSuccess_And_Contract()
    {
        _factory.ResetDatabase();
        var request = "/api/contract/2";
        _client.DefaultRequestHeaders.Add("Authorization", AuthToken);
        var requestModel = new AddContractRequestModel
        {
            ActualisationPeriod = 2,
            BeginningDate = new DateOnly(2024,03,04),
            EndingDate = new DateOnly(2024,03,10),
            SoftwareId = 2,
            SoftwareVersionId = 2
        };
        var json = JsonSerializer.Serialize(requestModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(request, content);
        response.EnsureSuccessStatusCode();

        var context = _factory.CreateDbContext();
        var responseString = await response.Content.ReadAsStringAsync();
        var contractResponse = JsonConvert.DeserializeObject<GetContractResponseModel>(responseString);

        Assert.NotNull(contractResponse);
        
        var contract = await
            context.Contracts
                .Include(contract => contract.SoftwareVersion)
                .FirstOrDefaultAsync(contractInDb => contractInDb.Id==contractResponse.Contract.Id);

        Assert.NotNull(contract);
        
        Assert.Equal(requestModel.ActualisationPeriod, contractResponse.Contract.ActualisationPeriod);
        Assert.Equal(requestModel.ActualisationPeriod, contract.ActualisationPeriod);
        Assert.Equal(requestModel.SoftwareId, contractResponse.Software.Id);
        Assert.Equal(requestModel.SoftwareId, contract.SoftwareVersion.SoftwareId);
        
    }
    
    [Fact]
    public async void AddContract_ReturnsNotFound()
    {
        _factory.ResetDatabase();
        var request = "/api/contract/4";
        _client.DefaultRequestHeaders.Add("Authorization", AuthToken);
        var requestModel = new AddContractRequestModel
        {
            ActualisationPeriod = 2,
            BeginningDate = new DateOnly(2024,03,04),
            EndingDate = new DateOnly(2024,03,10),
            SoftwareId = 2,
            SoftwareVersionId = 2
        };
        var json = JsonSerializer.Serialize(requestModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(request, content);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        
    }
    
    [Fact]
    public async void GetContract_ReturnsSuccess_And_Contract()
    {
        _factory.ResetDatabase();
        var request = "/api/contract/1";
        _client.DefaultRequestHeaders.Add("Authorization", AuthToken);


        var response = await _client.GetAsync(request);
        response.EnsureSuccessStatusCode();
        
        var responseString = await response.Content.ReadAsStringAsync();
        var contractResponse = JsonConvert.DeserializeObject<GetContractResponseModel>(responseString);

        Assert.NotNull(contractResponse);
        Assert.Equal(1, contractResponse.Contract.Id);
        
    }
    
    [Fact]
    public async void GetContract_ReturnsNotFound()
    {
        _factory.ResetDatabase();
        var request = "/api/contract/4";
        _client.DefaultRequestHeaders.Add("Authorization", AuthToken);


        var response = await _client.GetAsync(request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        
        
    }
}