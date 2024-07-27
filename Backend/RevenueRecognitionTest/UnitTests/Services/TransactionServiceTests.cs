using System.Collections;
using Moq;
using RevenueRecognition.Exceptions;
using RevenueRecognition.Models;
using RevenueRecognition.Repositories;
using RevenueRecognition.RequestModels.TransactionRequestModels;
using RevenueRecognition.Services;

namespace RevenueRecognitionTest.UnitTests.Services;

public class TransactionServiceTest
{

    [Fact]
    public async void MakeTransactionAsync_ThrowsNotFoundException()
    {
        var mockTransactionRepository = new Mock<ITransactionRepository>();
        var mockContractRepository = new Mock<IContractRepository>();

        mockContractRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Contract?)null);
        
        var transactionService = new TransactionService(mockTransactionRepository.Object, mockContractRepository.Object);
        
        await Assert.ThrowsAsync<NotFoundException>(()=> transactionService.MakeTransactionAsync(1, new MakeTransactionRequestModel()));
    }
    
    [Fact]
    public async void MakeTransactionAsync_ThrowsContractIsSignedException()
    {
        var mockTransactionRepository = new Mock<ITransactionRepository>();
        var mockContractRepository = new Mock<IContractRepository>();

        mockContractRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Contract
            {
                ContractStatus = new ContractStatus
                {
                    Name = "Signed"
                }
            });
        
        var transactionService = new TransactionService(mockTransactionRepository.Object, mockContractRepository.Object);
        
        await Assert.ThrowsAsync<ContractIsSignedException>(()=> transactionService.MakeTransactionAsync(1, new MakeTransactionRequestModel()));
    }
    
    [Fact]
    public async void MakeTransactionAsync_Sets_Status_Outdated_And_ThrowsContractIsOutdatedException()
    {
        var mockTransactionRepository = new Mock<ITransactionRepository>();
        var mockContractRepository = new Mock<IContractRepository>();
        var requestModel = new MakeTransactionRequestModel
        {
            TransactionDate = new DateOnly(2024, 04, 26)
        };
        mockContractRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Contract
            {
                EndingDate = new DateOnly(2024, 04, 25),
                ContractStatus = new ContractStatus
                {
                    Id = 0,
                    Name = "Waiting for a payment"
                },
                StatusId = 0
            });
        mockContractRepository.Setup(r => r.GetStatusIdByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(3);
        
        var transactionService = new TransactionService(mockTransactionRepository.Object, mockContractRepository.Object);
        

        
        await Assert.ThrowsAsync<ContractIsOutdatedException>(()=> transactionService.MakeTransactionAsync(1, requestModel));
    }
    
    [Fact]
    public async void MakeTransactionAsync_ThrowsContractIsOutdatedException()
    {
        var mockTransactionRepository = new Mock<ITransactionRepository>();
        var mockContractRepository = new Mock<IContractRepository>();

        mockContractRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Contract
            {
                ContractStatus = new ContractStatus
                {
                    Name = "Outdated"
                }
            });
        
        var transactionService = new TransactionService(mockTransactionRepository.Object, mockContractRepository.Object);
        
        await Assert.ThrowsAsync<ContractIsOutdatedException>(()=> transactionService.MakeTransactionAsync(1, new MakeTransactionRequestModel()));
    }
    
    [Fact]
    public async void MakeTransactionAsync_ThrowsTransactionAbovePriceException()
    {
        var mockTransactionRepository = new Mock<ITransactionRepository>();
        var mockContractRepository = new Mock<IContractRepository>();
        var requestModel = new MakeTransactionRequestModel
        {
            Amount = 2
        };
        mockContractRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Contract
            {
                ContractStatus = new ContractStatus
                {
                    Name = "Waiting for payment"
                },
                Price = 10
            });

        var transactionsBefore = new List<Transaction>
        {
            new Transaction
            {
                Amount = 5
            },
            new Transaction
            {
                Amount = 4
            },
            new Transaction
            {
                Amount = 2
            },
        };

        mockTransactionRepository.Setup(r => r.GetAllByContractId(It.IsAny<int>()))
            .Returns(transactionsBefore);
        
        var transactionService = new TransactionService(mockTransactionRepository.Object, mockContractRepository.Object);
        
        await Assert.ThrowsAsync<TransactionAbovePriceException>(()=> transactionService.MakeTransactionAsync(1, requestModel));
    }
    
    [Fact]
    public async void MakeTransactionAsync_Successful_And_Changes_Status_To_Signed()
    {
        var mockTransactionRepository = new Mock<ITransactionRepository>();
        var mockContractRepository = new Mock<IContractRepository>();
        var requestModel = new MakeTransactionRequestModel
        {
            Amount = 2
        };
        var contract = new Contract
        {
            ContractStatus = new ContractStatus
            {
                Name = "Waiting for payment"
            },
            Price = 10
        };
        mockContractRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(contract);

        var transactionsBefore = new List<Transaction>
        {
            new Transaction
            {
                Amount = 5
            },
            new Transaction
            {
                Amount = 4
            },
            new Transaction
            {
                Amount = 1
            },
        };

        mockTransactionRepository.Setup(r => r.GetAllByContractId(It.IsAny<int>()))
            .Returns(transactionsBefore);
        
        var transactionService = new TransactionService(mockTransactionRepository.Object, mockContractRepository.Object);
        mockContractRepository.Setup(r => r.GetStatusIdByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(1);
        await transactionService.MakeTransactionAsync(1, requestModel);
        
        Assert.Equal(1, contract.StatusId);
    }
}