using System.Text.Json;
using RevenueRecognition.Exceptions;
using RevenueRecognition.Repositories;

namespace RevenueRecognition.Services;

public interface IRevenueService
{
    Task<decimal> GetActualRevenueAsync(int? productId, string? currency);
    Task<decimal> GetExpectedRevenueAsync(int? productId, string? currency);
}

public class RevenueService(IContractRepository contractRepository, HttpClient httpClient) : IRevenueService
{
        
    
    public async Task<decimal> GetActualRevenueAsync(int? productId, string? currency)
    {
        decimal revenue = 0;
        if (productId == null)
        {
            var signedContracts = contractRepository.GetSignedContracts();
            revenue = signedContracts.Sum(contract => contract.Price);
        }
        else
        {
            var contracts = contractRepository
                .GetSignedContractsByProductId((int)productId);

            var enumerable = contracts.ToList();
            if (enumerable.Count == 0)
            {
                throw new NotFoundException($"Software with id:{productId} does not have any signed contracts.");
            }
            
            revenue = enumerable
                .Sum(contract => contract.Price);
        }
        if (currency is null)
        {
            return revenue;
        }

        var exchangeRate = await ExchangeRate(currency);
        return revenue / exchangeRate;
    }

    public async Task<decimal> GetExpectedRevenueAsync(int? productId, string? currency)
    {
        decimal revenue;
        if (productId == null)
        {
            revenue = contractRepository.GetActiveAndSignedContracts()
                .Sum(contract => contract.Price);
        }
        else
        {
            var contracts = contractRepository
                .GetActiveAndSignedContractsByProductId((int)productId);
            
            var enumerable = contracts.ToList();
            if (enumerable.Count == 0)
            {
                throw new NotFoundException($"Software with id:{productId} does not have any active or signed contracts.");
            }
            
            revenue = enumerable
                .Sum(contract => contract.Price);
        }

        if (currency is null)
        {
            return revenue;
        }

        var exchangeRate = await ExchangeRate(currency);
        return revenue / exchangeRate;
    }

    private async Task<decimal> ExchangeRate(string currency)
    {
        var url = $"https://api.nbp.pl/api/exchangerates/rates/a/{currency}?format=json";
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var data = JsonDocument.Parse(jsonResponse);
        var rateElement = data.RootElement.GetProperty("rates")[0];
        var exchangeRate = rateElement.GetProperty("mid").GetDecimal();
        
        return exchangeRate;
    }
}