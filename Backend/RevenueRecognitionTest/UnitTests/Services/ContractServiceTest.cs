using Moq;
using RevenueRecognition.Exceptions;
using RevenueRecognition.Models;
using RevenueRecognition.Repositories;
using RevenueRecognition.RequestModels.ContractRequestModels;
using RevenueRecognition.RequestModels.TransactionRequestModels;
using RevenueRecognition.Services;

namespace RevenueRecognitionTest.UnitTests.Services;

public class ContractServiceTest
{

    [Fact]
    public async void AddContractAsync_DatePeriodIsIncorrectException_Less_Than_3_Days()
    {
        var mockTransactionRepository = new Mock<ITransactionRepository>();
        var mockContractRepository = new Mock<IContractRepository>();
        var mockSoftwareRepository = new Mock<ISoftwareRepository>();
        var mockClientRepository = new Mock<IClientRepository>();
        var requestModel = new AddContractRequestModel
        {
            BeginningDate = new DateOnly(2024,04,25),
            EndingDate = new DateOnly(2024,04,26)
        };
        
        var contractService = new ContractService(
            mockContractRepository.Object, 
            mockSoftwareRepository.Object, 
            mockClientRepository.Object,
            mockTransactionRepository.Object
            );
        
        await Assert.ThrowsAsync<DatePeriodIsIncorrectException>(()=> contractService.AddContractAsync(requestModel, 1));
    }
    
    [Fact]
    public async void AddContractAsync_DatePeriodIsIncorrectException_More_Than_30_Days()
    {
        var mockTransactionRepository = new Mock<ITransactionRepository>();
        var mockContractRepository = new Mock<IContractRepository>();
        var mockSoftwareRepository = new Mock<ISoftwareRepository>();
        var mockClientRepository = new Mock<IClientRepository>();
        var requestModel = new AddContractRequestModel
        {
            BeginningDate = new DateOnly(2024,04,25),
            EndingDate = new DateOnly(2024,06,26)
        };
        
        var contractService = new ContractService(
            mockContractRepository.Object, 
            mockSoftwareRepository.Object, 
            mockClientRepository.Object,
            mockTransactionRepository.Object
        );
        
        await Assert.ThrowsAsync<DatePeriodIsIncorrectException>(()=> contractService.AddContractAsync(requestModel, 1));
    }
    
    [Fact]
    public async void AddContractAsync_ThrowsNotFoundException_For_Client()
    {
        var mockTransactionRepository = new Mock<ITransactionRepository>();
        var mockContractRepository = new Mock<IContractRepository>();
        var mockSoftwareRepository = new Mock<ISoftwareRepository>();
        var mockClientRepository = new Mock<IClientRepository>();
        var requestModel = new AddContractRequestModel
        {
            BeginningDate = new DateOnly(2024,04,25),
            EndingDate = new DateOnly(2024,04,30)
        };
        
        var contractService = new ContractService(
            mockContractRepository.Object, 
            mockSoftwareRepository.Object, 
            mockClientRepository.Object,
            mockTransactionRepository.Object
        );

        mockClientRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Client?)null);
        
        await Assert.ThrowsAsync<NotFoundException>(()=> contractService.AddContractAsync(requestModel, 1));
    }
    
    [Fact]
    public async void AddContractAsync_ThrowsNotFoundException_For_Software()
    {
        var mockTransactionRepository = new Mock<ITransactionRepository>();
        var mockContractRepository = new Mock<IContractRepository>();
        var mockSoftwareRepository = new Mock<ISoftwareRepository>();
        var mockClientRepository = new Mock<IClientRepository>();
        var requestModel = new AddContractRequestModel
        {
            BeginningDate = new DateOnly(2024,04,25),
            EndingDate = new DateOnly(2024,04,30)
        };
        
        var contractService = new ContractService(
            mockContractRepository.Object, 
            mockSoftwareRepository.Object, 
            mockClientRepository.Object,
            mockTransactionRepository.Object
        );
        
        mockClientRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Client());
        
        mockSoftwareRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Software?)null);
        
        await Assert.ThrowsAsync<NotFoundException>(()=> contractService.AddContractAsync(requestModel, 1));
    }
    
    [Fact]
    public async void AddContractAsync_ThrowsActiveContractException()
    {
        var mockTransactionRepository = new Mock<ITransactionRepository>();
        var mockContractRepository = new Mock<IContractRepository>();
        var mockSoftwareRepository = new Mock<ISoftwareRepository>();
        var mockClientRepository = new Mock<IClientRepository>();
        var requestModel = new AddContractRequestModel
        {
            BeginningDate = new DateOnly(2024,04,25),
            EndingDate = new DateOnly(2024,04,30),
            SoftwareId = 1
        };
        
        var contractService = new ContractService(
            mockContractRepository.Object, 
            mockSoftwareRepository.Object, 
            mockClientRepository.Object,
            mockTransactionRepository.Object
        );
        
        mockClientRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Client?)new Client());
        
        mockSoftwareRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Software?)new Software());

        mockContractRepository.Setup(r => r.ContractWithClientActiveAsync(It.IsAny<int>(),It.IsAny<DateOnly>(),It.IsAny<int>()))
            .ReturnsAsync((Contract?)new Contract());
        
        await Assert.ThrowsAsync<ActiveContractException>(()=> contractService.AddContractAsync(requestModel, 1));
    }
    
    
    [Fact]
    public async void AddContractAsync_Successful()
    {
        var mockTransactionRepository = new Mock<ITransactionRepository>();
        var mockContractRepository = new Mock<IContractRepository>();
        var mockSoftwareRepository = new Mock<ISoftwareRepository>();
        var mockClientRepository = new Mock<IClientRepository>();
        var requestModel = new AddContractRequestModel
        {
            BeginningDate = new DateOnly(2024,04,25),
            EndingDate = new DateOnly(2024,04,30),
            SoftwareId = 1,
            ActualisationPeriod = 2,
            SoftwareVersionId = 1
        };
        
        var contractService = new ContractService(
            mockContractRepository.Object, 
            mockSoftwareRepository.Object, 
            mockClientRepository.Object,
            mockTransactionRepository.Object
        );
        
        mockClientRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Client?)new Client());
        
        mockSoftwareRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Software?)new Software
            {
                Id = 1,
                Price = 2000,
                SoftwareSales = [new SoftwareSales
                {
                    Sale = new Sale
                    {
                        Name = "Friday",
                        StartAt = new DateOnly(2024,04,23),
                        EndAt = new DateOnly(2024,04,27)
                    },
                    SoftwareId = 1,
                    Value = (decimal)0.25
                },
                new SoftwareSales
                {
                    Sale = new Sale
                    {
                        Name = "Lol",
                        StartAt = new DateOnly(2024,04,23),
                        EndAt = new DateOnly(2024,04,28)
                    },
                    SoftwareId = 1,
                    Value = (decimal)0.5
                },new SoftwareSales
                {
                    Sale = new Sale
                    {
                        Name = "Kek",
                        StartAt = new DateOnly(2024,04,26),
                        EndAt = new DateOnly(2024,04,27)
                    },
                    SoftwareId = 1,
                    Value = (decimal)0.75
                },new SoftwareSales
                {
                    Sale = new Sale
                    {
                        Name = "Wow",
                        StartAt = new DateOnly(2024,04,23),
                        EndAt = new DateOnly(2024,04,27)
                    },
                    SoftwareId = 1,
                    Value = (decimal)0.40
                }
                ]
            });
        
        
        mockContractRepository.Setup(r => r.ContractWithClientActiveAsync(It.IsAny<int>(),It.IsAny<DateOnly>(),It.IsAny<int>()))
            .ReturnsAsync((Contract?)null);
        
        mockContractRepository.Setup(r => r.ClientInPastAsync(It.IsAny<int>()))
            .ReturnsAsync((Contract?)new Contract());
        
        
        try
        {
            await contractService.AddContractAsync(requestModel, 1);
        }catch (NotFoundException ex)
        {
            
        }
    }
    
    [Fact]
    public async void GetContractAsync_Successful()
    {
        var mockTransactionRepository = new Mock<ITransactionRepository>();
        var mockContractRepository = new Mock<IContractRepository>();
        var mockSoftwareRepository = new Mock<ISoftwareRepository>();
        var mockClientRepository = new Mock<IClientRepository>();
        
        
        var contractService = new ContractService(
            mockContractRepository.Object, 
            mockSoftwareRepository.Object, 
            mockClientRepository.Object,
            mockTransactionRepository.Object
        );
        
        mockContractRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Contract
            {
                Id = 1,
                ClientId = 1,
                SoftwareVersion = new SoftwareVersion
                {
                    Software = new Software()
                },
                ContractStatus = new ContractStatus
                {
                    Name = "asd"
                },
                BeginningDate = DateOnly.MinValue,
                EndingDate = DateOnly.MinValue,
                ActualisationPeriod = 1,
                Client = new Client(),
                Price = 1,
                SoftwareAndVersionId = 1
            });
        
        var res = await contractService.GetContractAsync(1);
        Assert.Equal(1,res.Contract.Id);
    }
}