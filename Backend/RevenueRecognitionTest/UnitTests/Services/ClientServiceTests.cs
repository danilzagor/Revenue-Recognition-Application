using Moq;
using RevenueRecognition.Exceptions;
using RevenueRecognition.Models;
using RevenueRecognition.Repositories;
using RevenueRecognition.RequestModels;
using RevenueRecognition.Services;

namespace RevenueRecognitionTest.UnitTests.Services;

public class ClientServiceTests
{

    [Fact]
    public async void AddPhysicalClientAsync_Adds_Client_And_PhysicalClient()
    {
        var requestModel = new AddPhysicalClientRequestModel
        {
            Address = "123 Main St",
            Email = "test@example.com",
            PhoneNumber = "123-456-7890",
            FirstName = "John",
            LastName = "Doe",
            PESEL = "12345678901"
        };

        var mockRepository = new Mock<IClientRepository>();
        
        mockRepository.Setup(r => r.AddAsync(It.IsAny<Client>()))
            .Callback<Client>(client => client.Id = 1) 
            .Returns(Task.CompletedTask);

        mockRepository.Setup(r => r.AddPhysicalClientAsync(It.IsAny<PhysicalClient>()))
            .Returns(Task.CompletedTask);

        
        mockRepository.Setup(r => r.SaveAsync())
            .Returns(Task.CompletedTask);

        var clientService = new ClientService(mockRepository.Object);
        
        await clientService.AddPhysicalClientAsync(requestModel);
        
        mockRepository.Verify(r => r.AddAsync(It.IsAny<Client>()));
        mockRepository.Verify(r => r.AddPhysicalClientAsync(It.IsAny<PhysicalClient>()));
        mockRepository.Verify(r => r.SaveAsync());
    }
    
    [Fact]
    public async void AddPhysicalClientAsync_Returns_ClientAlreadyExistsException_When_Same_PhoneNumber()
    {
        var requestModel = new AddPhysicalClientRequestModel
        {
            Address = "123 Main St",
            Email = "test@example.com",
            PhoneNumber = "123-456-7890",
            FirstName = "John",
            LastName = "Doe",
            PESEL = "12345678901"
        };

        var mockRepository = new Mock<IClientRepository>();
        
        mockRepository.Setup(r => r.GetClientByPhoneOrEmailAsync(It.IsAny<Client>()))
            .Callback<Client>(client => client.Id = 1) 
            .ReturnsAsync(new Client
            {
                PhoneNumber = requestModel.PhoneNumber
            });
        

        var clientService = new ClientService(mockRepository.Object);
        
       
        await Assert.ThrowsAsync<ClientAlreadyExistsException>(() =>  clientService.AddPhysicalClientAsync(requestModel));
    }
    
    [Fact]
    public async void AddPhysicalClientAsync_Returns_ClientAlreadyExistsException_When_Same_Email()
    {
        var requestModel = new AddPhysicalClientRequestModel
        {
            Address = "123 Main St",
            Email = "test@example.com",
            PhoneNumber = "123-456-7890",
            FirstName = "John",
            LastName = "Doe",
            PESEL = "12345678901"
        };

        var mockRepository = new Mock<IClientRepository>();
        
        mockRepository.Setup(r => r.GetClientByPhoneOrEmailAsync(It.IsAny<Client>()))
            .Callback<Client>(client => client.Id = 1) 
            .ReturnsAsync(new Client
            {
                Email = requestModel.Email
            });
        

        var clientService = new ClientService(mockRepository.Object);
        
       
        await Assert.ThrowsAsync<ClientAlreadyExistsException>(() =>  clientService.AddPhysicalClientAsync(requestModel));
    }



    [Fact]
    public async void AddCompanyClientAsync_Adds_Client_And_CompanyClient()
    {
        var requestModel = new AddCompanyClientRequestModel
        {
            Address = "123 Main St",
            Email = "test@example.com",
            PhoneNumber = "123-456-7890",
            Name = "John",
            KRS = "12345678901"
        };

        var mockRepository = new Mock<IClientRepository>();
        
        mockRepository.Setup(r => r.AddAsync(It.IsAny<Client>()))
            .Callback<Client>(client => client.Id = 1) 
            .Returns(Task.CompletedTask);

        mockRepository.Setup(r => r.AddCompanyClientAsync(It.IsAny<CompanyClient>()))
            .Returns(Task.CompletedTask);

        
        mockRepository.Setup(r => r.SaveAsync())
            .Returns(Task.CompletedTask);

        var clientService = new ClientService(mockRepository.Object);
        
        await clientService.AddCompanyClientAsync(requestModel);
        
        mockRepository.Verify(r => r.AddAsync(It.IsAny<Client>()));
        mockRepository.Verify(r => r.AddCompanyClientAsync(It.IsAny<CompanyClient>()));
        mockRepository.Verify(r => r.SaveAsync());
    }
    
    [Fact]
    public async Task DeletePhysicalClientAsync_ExistingClient_DeletesSuccessfully()
    {
        var clientId = 1;
        var mockRepository = new Mock<IClientRepository>();
        var service = new ClientService(mockRepository.Object);

        var existingPhysicalClient = new PhysicalClient
        {
            Id = clientId,
            Client = new Client { Id = clientId }
        };

        mockRepository.Setup(repo => repo.GetPhysicalClientByIdAsync(clientId))
            .ReturnsAsync(existingPhysicalClient);
        
        await service.DeletePhysicalClientAsync(clientId);
        
        Assert.NotNull(existingPhysicalClient.Client.DeletedAt);
        mockRepository.Verify(repo => repo.SaveAsync(), Times.Once);
    }

    [Fact]
    public void DeletePhysicalClientAsync_NonExistingClient_ThrowsNotFoundException()
    {
        var clientId = 1;
        var mockRepository = new Mock<IClientRepository>();
        var service = new ClientService(mockRepository.Object);

        mockRepository.Setup(repo => repo.GetPhysicalClientByIdAsync(clientId))
            .ReturnsAsync((PhysicalClient)null);
        
        Assert.ThrowsAsync<NotFoundException>(async () => await service.DeletePhysicalClientAsync(clientId));
        mockRepository.Verify(repo => repo.SaveAsync(), Times.Never);
    }

    [Fact]
    public async void EditCompanyClientAsync_EditsClientSuccessfully()
    {
        var requestModel = new EditCompanyClientRequestModel()
        {
            Address = "123 Main St",
            Email = "test@example.com",
            PhoneNumber = "123-456-7890",
            Name = "John"
        };

        var clientToEdit = new CompanyClient
        {
            Client = new Client
            {
                Address = "123 Main S",
                Email = "test@example.co",
                PhoneNumber = "123-456-789"
            },
            Name = "Joh"
        };
        
        var mockRepository = new Mock<IClientRepository>();
        
        mockRepository.Setup(r => r.AddAsync(It.IsAny<Client>()))
            .Callback<Client>(client => client.Id = 1) 
            .Returns(Task.CompletedTask);
        
        mockRepository.Setup(repo => repo.GetCompanyClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(clientToEdit);
        
        mockRepository.Setup(r => r.SaveAsync())
            .Returns(Task.CompletedTask);

        var clientService = new ClientService(mockRepository.Object);
        
        await clientService.EditCompanyClientAsync(1, requestModel);
        
        Assert.Equal(requestModel.PhoneNumber, clientToEdit.Client.PhoneNumber);
        Assert.Equal(requestModel.Email, clientToEdit.Client.Email);
        Assert.Equal(requestModel.Address, clientToEdit.Client.Address);
        Assert.Equal(requestModel.Name, clientToEdit.Name);
        mockRepository.Verify(r => r.SaveAsync());
    }
    
    [Fact]
    public async void EditCompanyClientAsync_ThrowsNotFoundException()
    {
        var requestModel = new EditCompanyClientRequestModel()
        {
            Address = "123 Main St",
            Email = "test@example.com",
            PhoneNumber = "123-456-7890",
            Name = "John"
        };
        
        var mockRepository = new Mock<IClientRepository>();

        mockRepository.Setup(r => r.AddAsync(It.IsAny<Client>()))
            .Callback<Client>(client => client.Id = 1);
        
        mockRepository.Setup(repo => repo.GetCompanyClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((CompanyClient?)null);
        
        var clientService = new ClientService(mockRepository.Object);
        
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await clientService.EditCompanyClientAsync(2, requestModel));
    }
    
    
    
    [Fact]
    public async void EditPhysicalClientAsync_EditsClientSuccessfully()
    {
        var requestModel = new EditPhysicalClientRequestModel()
        {
            Address = "123 Main St",
            Email = "test@example.com",
            PhoneNumber = "123-456-7890",
            FirstName = "John",
            LastName = "Doe"
        };

        var clientToEdit = new PhysicalClient()
        {
            Client = new Client
            {
                Address = "123 Main S",
                Email = "test@example.co",
                PhoneNumber = "123-456-789"
            },
            FirstName = "John",
            LastName = "Doe"
        };
        
        var mockRepository = new Mock<IClientRepository>();
        
        mockRepository.Setup(r => r.AddAsync(It.IsAny<Client>()))
            .Callback<Client>(client => client.Id = 1) 
            .Returns(Task.CompletedTask);
        
        mockRepository.Setup(repo => repo.GetPhysicalClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(clientToEdit);
        
        mockRepository.Setup(r => r.SaveAsync())
            .Returns(Task.CompletedTask);

        var clientService = new ClientService(mockRepository.Object);
        
        await clientService.EditPhysicalClientAsync(1, requestModel);
        
        Assert.Equal(requestModel.PhoneNumber, clientToEdit.Client.PhoneNumber);
        Assert.Equal(requestModel.Email, clientToEdit.Client.Email);
        Assert.Equal(requestModel.Address, clientToEdit.Client.Address);
        Assert.Equal(requestModel.FirstName, clientToEdit.FirstName);
        Assert.Equal(requestModel.LastName, clientToEdit.LastName);
        mockRepository.Verify(r => r.SaveAsync());
    }
    
    [Fact]
    public async void EditPhysicalClientAsync_ThrowsNotFoundException()
    {
        var requestModel = new EditPhysicalClientRequestModel()
        {
            Address = "123 Main St",
            Email = "test@example.com",
            PhoneNumber = "123-456-7890",
            FirstName = "John",
            LastName = "Doe"
        };
        
        var mockRepository = new Mock<IClientRepository>();

        mockRepository.Setup(r => r.AddAsync(It.IsAny<Client>()))
            .Callback<Client>(client => client.Id = 1);
        
        mockRepository.Setup(repo => repo.GetPhysicalClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((PhysicalClient?)null);
        
        var clientService = new ClientService(mockRepository.Object);
        
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await clientService.EditPhysicalClientAsync(2, requestModel));
    }
}