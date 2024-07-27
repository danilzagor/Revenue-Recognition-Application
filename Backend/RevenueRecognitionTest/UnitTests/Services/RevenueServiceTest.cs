using Moq;
using RevenueRecognition.Models;
using RevenueRecognition.Repositories;
using RevenueRecognition.Services;

namespace RevenueRecognitionTest.UnitTests.Services;

public class RevenueServiceTest
{

    [Fact]
    public async void GetActualRevenueAsync_Without_ProductId_Without_Currency()
    {
        var mockRepository = new Mock<IContractRepository>();

        mockRepository.Setup(r => r.GetSignedContracts())
            .Returns([
                new Contract
                {
                    Price = 100
                },
                new Contract
                {
                    Price = 150
                }
            ]);
        
        var revenueService = new RevenueService(mockRepository.Object, new HttpClient());

        var res = await revenueService.GetActualRevenueAsync(null, null);
        
        Assert.Equal(250, res);
    }
    [Fact]
    public async void GetActualRevenueAsync_With_ProductId_With_Currency()
    {
        var mockRepository = new Mock<IContractRepository>();

        mockRepository.Setup(r => r.GetSignedContractsByProductId(It.IsAny<int>()))
            .Returns([
                new Contract
                {
                    Price = 100
                },
                new Contract
                {
                    Price = 150
                }
            ]);
        
        var revenueService = new RevenueService(mockRepository.Object, new HttpClient());

        var res = await revenueService.GetActualRevenueAsync(1, "usd");
        
    }
    
    [Fact]
    public async void GetExpectedRevenueAsync_Without_ProductId_Without_Currency()
    {
        var mockRepository = new Mock<IContractRepository>();

        mockRepository.Setup(r => r.GetActiveAndSignedContracts())
            .Returns([
                new Contract
                {
                    Price = 100
                },
                new Contract
                {
                    Price = 150
                }
            ]);
        
        var revenueService = new RevenueService(mockRepository.Object, new HttpClient());

        var res = await revenueService.GetExpectedRevenueAsync(null, null);
        
        Assert.Equal(250, res);
    }
    [Fact]
    public async void GetExpectedRevenueAsync_With_ProductId_With_Currency()
    {
        var mockRepository = new Mock<IContractRepository>();

        mockRepository.Setup(r => r.GetActiveAndSignedContractsByProductId(It.IsAny<int>()))
            .Returns([
                new Contract
                {
                    Price = 100
                },
                new Contract
                {
                    Price = 150
                }
            ]);
        
        var revenueService = new RevenueService(mockRepository.Object, new HttpClient());

        var res = await revenueService.GetExpectedRevenueAsync(1, "usd");
        
    }
}