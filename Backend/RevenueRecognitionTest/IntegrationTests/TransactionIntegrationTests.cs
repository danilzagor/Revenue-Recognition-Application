using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RevenueRecognition.Controllers;
using RevenueRecognition.RequestModels;
using RevenueRecognition.RequestModels.TransactionRequestModels;
using RevenueRecognitionTest.IntegrationTests.Config;
using Xunit.Abstractions;

namespace RevenueRecognitionTest.IntegrationTests;

public class TransactionControllerTest: IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;

    private const string AuthToken = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL" +
                                     "3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsidXNlciIsImFkbWluIl0sImV4cCI6" +
                                     "MTcyMDc3MzY2NiwiaXNzIjoiS08tVG9rZW5zIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9" +
                                     "zdDo1MDIyIn0.dTfg5WRiZhOlT-6au06KpqgGd0PNa36m0eFtYzSkPQE";
    
    
    public TransactionControllerTest(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = _factory.CreateClient();
    }


    [Fact]
    public async void MakeTransaction_ReturnsSuccess()
    {
        _factory.ResetDatabase();
        var request = "/api/transaction/2";
        _client.DefaultRequestHeaders.Add("Authorization", AuthToken);
        var requestModel = new MakeTransactionRequestModel
        {
            Amount = 200,
            TransactionDate = new DateOnly(2024,01,23)
        };
        var json = JsonSerializer.Serialize(requestModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(request, content);
        response.EnsureSuccessStatusCode();

        var context = _factory.CreateDbContext();
        var transaction = await
            context.Transactions.FirstOrDefaultAsync(transaction =>
                transaction.TransactionDate == new DateOnly(2024, 01, 23));

        Assert.NotNull(transaction);
        
        Assert.Equal(200, transaction.Amount);
    }
    
    
    [Fact]
    public async Task PostPhysicalClientEndpoint_ReturnsBadRequest()
    {
        _factory.ResetDatabase();
        var request = "/api/clients/physical";
        _client.DefaultRequestHeaders.Add("Authorization", AuthToken);
        var requestModel = new AddPhysicalClientRequestModel()
        {
            PESEL = "28743598344",
            FirstName = "Michal",
            LastName = "ASOJUHEFIUO",
            Address = "123 Main St",
            Email = "client1@example.com",
            PhoneNumber = "123-456-7890"
        };
        var json = JsonSerializer.Serialize(requestModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(request, content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
    }
}