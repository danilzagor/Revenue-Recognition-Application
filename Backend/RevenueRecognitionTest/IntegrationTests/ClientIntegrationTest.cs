
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RevenueRecognition.RequestModels;
using RevenueRecognitionTest.IntegrationTests.Config;

namespace RevenueRecognitionTest.IntegrationTests;


public class ClientIntegrationTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private const string AuthToken = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL" +
                                     "3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsidXNlciIsImFkbWluIl0sImV4cCI6" +
                                     "MTcyMDc3MzY2NiwiaXNzIjoiS08tVG9rZW5zIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9" +
                                     "zdDo1MDIyIn0.dTfg5WRiZhOlT-6au06KpqgGd0PNa36m0eFtYzSkPQE";
    
    
    public ClientIntegrationTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }
    
    
    [Fact]
    public async Task PostCompanyClientEndpoint_ReturnsSuccess()
    {
        _factory.ResetDatabase();
        var request = "/api/clients/company";
        _client.DefaultRequestHeaders.Add("Authorization", AuthToken);
        var requestModel = new AddCompanyClientRequestModel
        {
            Address = "lololol",
            Email = "lolo@gmail",
            KRS = "28743598344",
            Name = "Michal",
            PhoneNumber = "8005553535"
        };
        var json = JsonSerializer.Serialize(requestModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(request, content);
        response.EnsureSuccessStatusCode();
        
    }
    [Fact]
    public async Task PostCompanyClientEndpoint_ReturnsBadRequest()
    {
        _factory.ResetDatabase();
        var request = "/api/clients/company";
        _client.DefaultRequestHeaders.Add("Authorization", AuthToken);
        var requestModel = new AddCompanyClientRequestModel
        {
            KRS = "28743598344",
            Name = "Michal",
            Address = "123 Main St",
            Email = "client1@example.com",
            PhoneNumber = "123-456-7890"
        };
        var json = JsonSerializer.Serialize(requestModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(request, content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
    }
    
    [Fact]
    public async Task PostPhysicalClientEndpoint_ReturnsSuccess()
    {
        _factory.ResetDatabase();
        var request = "/api/clients/physical";
        _client.DefaultRequestHeaders.Add("Authorization", AuthToken);
        var requestModel = new AddPhysicalClientRequestModel()
        {
            Address = "123",
            Email = "123",
            PESEL = "28743598344",
            FirstName = "Michal",
            PhoneNumber = "345",
            LastName = "ASOJUHEFIUO"
        };
        var json = JsonSerializer.Serialize(requestModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(request, content);
        response.EnsureSuccessStatusCode();
        
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
    
    [Fact]
    public async Task DeletePhysicalClientEndpoint_ReturnsSuccess()
    {
        _factory.ResetDatabase();
        var request = "/api/clients/physical/2";
        _client.DefaultRequestHeaders.Add("Authorization", AuthToken);
        var response = await _client.DeleteAsync(request);
        response.EnsureSuccessStatusCode();

        var res = await _factory.CreateDbContext()
            .Clients.FirstOrDefaultAsync(client => client.Id == 2);
        
        Assert.NotNull(res);
        Assert.NotNull(res.DeletedAt);
    }
    
    [Fact]
    public async Task DeletePhysicalClientEndpoint_BadRequest()
    {
        _factory.ResetDatabase();
        var request = "/api/clients/physical/3";
        _client.DefaultRequestHeaders.Add("Authorization", AuthToken);
        var response = await _client.DeleteAsync(request);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task EditCompanyClientEndpoint_ReturnsSuccess()
    {
        _factory.ResetDatabase();
        var request = "/api/clients/company/1";
        _client.DefaultRequestHeaders.Add("Authorization", AuthToken);
        var requestModel = new EditCompanyClientRequestModel()
        {
            Address = "lololol",
            Email = "lolo@gmail",
            Name = "Michal",
            PhoneNumber = "8005553535"
        };
        var json = JsonSerializer.Serialize(requestModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PutAsync(request, content);
        response.EnsureSuccessStatusCode();
        
        var res = await _factory.CreateDbContext()
            .CompanyClients
            .Include(client => client.Client)
            .FirstOrDefaultAsync(client => client.Id == 1);
        
        Assert.NotNull(res);
        Assert.Equal(res.Client.Address, requestModel.Address);
        Assert.Equal(res.Client.PhoneNumber, requestModel.PhoneNumber);
        Assert.Equal(res.Client.Email, requestModel.Email);
        Assert.Equal(res.Name, requestModel.Name);
    }
    [Fact]
    public async Task EditCompanyClientEndpoint_ThrowsNotFound()
    {
        _factory.ResetDatabase();
        var request = "/api/clients/company/2";
        _client.DefaultRequestHeaders.Add("Authorization", AuthToken);
        var requestModel = new EditCompanyClientRequestModel()
        {
            Address = "lololol",
            Email = "lolo@gmail",
            Name = "Michal",
            PhoneNumber = "8005553535"
        };
        var json = JsonSerializer.Serialize(requestModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PutAsync(request, content);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task EditPhysicalClientEndpoint_ReturnsSuccess()
    {
        _factory.ResetDatabase();
        var request = "/api/clients/physical/2";
        _client.DefaultRequestHeaders.Add("Authorization", AuthToken);
        var requestModel = new EditPhysicalClientRequestModel()
        {
            Address = "gdsfg",
            Email = "lolo@dfeg",
            FirstName = "sdgfDG",
            LastName = "ASDF",
            PhoneNumber = "235345"
        };
        var json = JsonSerializer.Serialize(requestModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PutAsync(request, content);
        response.EnsureSuccessStatusCode();
        
        var res = await _factory.CreateDbContext()
            .PhysicalClients
            .Include(client => client.Client)
            .FirstOrDefaultAsync(client => client.Id == 2);
        
        Assert.NotNull(res);
        Assert.Equal(res.Client.Address, requestModel.Address);
        Assert.Equal(res.Client.PhoneNumber, requestModel.PhoneNumber);
        Assert.Equal(res.Client.Email, requestModel.Email);
        Assert.Equal(res.FirstName, requestModel.FirstName);
        Assert.Equal(res.LastName, requestModel.LastName);
    }
    
    [Fact]
    public async Task EditPhysicalClientEndpoint_ThrowsNotFound()
    {
        _factory.ResetDatabase();
        var request = "/api/clients/physical/1";
        _client.DefaultRequestHeaders.Add("Authorization", AuthToken);
        var requestModel = new EditPhysicalClientRequestModel()
        {
            Address = "gdsfg",
            Email = "lolo@dfeg",
            FirstName = "sdgfDG",
            LastName = "ASDF",
            PhoneNumber = "235345"
        };
        var json = JsonSerializer.Serialize(requestModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PutAsync(request, content);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        
        
    }
}