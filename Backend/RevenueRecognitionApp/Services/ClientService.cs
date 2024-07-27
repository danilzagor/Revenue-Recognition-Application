using RevenueRecognition.Exceptions;
using RevenueRecognition.Models;
using RevenueRecognition.Models.ResponseModels.ClientResponseModel;
using RevenueRecognition.Repositories;
using RevenueRecognition.RequestModels;
using RevenueRecognition.ResponseModels.ContractResponseModels;

namespace RevenueRecognition.Services;

public interface IClientService
{
    public Task AddPhysicalClientAsync(AddPhysicalClientRequestModel requestModel);
    public Task AddCompanyClientAsync(AddCompanyClientRequestModel requestModel);
    public Task DeletePhysicalClientAsync(int clientId);
    public Task EditCompanyClientAsync(int clientId, EditCompanyClientRequestModel requestModel);
    public Task EditPhysicalClientAsync(int clientId, EditPhysicalClientRequestModel requestModel);
    public Task<List<PhysicalClient>> GetAllPhysicalClients();
    Task<List<CompanyClient>> GetAllCompanyClients();
    Task<GetPhysicalClientResponseModel> GetPhysicalClientById(int id);
    Task<GetCompanyClientResponseModel> GetCompanyClientById(int id);
}

public class ClientService(IClientRepository clientRepository) : IClientService
{
    public async Task AddPhysicalClientAsync(AddPhysicalClientRequestModel requestModel)
    {
        var client = new Client
        {
            Address = requestModel.Address,
            Email = requestModel.Email,
            PhoneNumber = requestModel.PhoneNumber
        };
        await CheckIfClientExists(client);
        var physicalClient = new PhysicalClient
        {
            Client = client,
            FirstName = requestModel.FirstName,
            LastName = requestModel.LastName,
            PESEL = requestModel.PESEL
        };

        await clientRepository.AddAsync(client);
        await clientRepository.AddPhysicalClientAsync(physicalClient);
        await clientRepository.SaveAsync();
    }

    public async Task AddCompanyClientAsync(AddCompanyClientRequestModel requestModel)
    {
        var client = new Client
        {
            Address = requestModel.Address,
            Email = requestModel.Email,
            PhoneNumber = requestModel.PhoneNumber
        };
        await CheckIfClientExists(client);
        var companyClient = new CompanyClient
        {
            Client = client,
            Name = requestModel.Name,
            KRS = requestModel.KRS
        };

        await clientRepository.AddAsync(client);
        await clientRepository.AddCompanyClientAsync(companyClient);
        await clientRepository.SaveAsync();
    }

    public async Task DeletePhysicalClientAsync(int clientId)
    {
        var physicalClient = await clientRepository.GetPhysicalClientByIdAsync(clientId);

        if (physicalClient is null)
        {
            throw new NotFoundException($"Client with id:{clientId} does not exist.");
        }

        physicalClient.Client.DeletedAt = DateTime.Now;

        await clientRepository.SaveAsync();
    }
    
    public async Task EditCompanyClientAsync(int clientId, EditCompanyClientRequestModel requestModel)
         
    {
        var companyClient = await clientRepository.GetCompanyClientByIdAsync(clientId);
        if (companyClient is null)
        {
            throw new NotFoundException($"Client with id:{clientId} does not exist.");
        }

        companyClient.Name = requestModel.Name ?? companyClient.Name;
        companyClient.Client.Address = requestModel.Address ?? companyClient.Client.Address;
        companyClient.Client.Email = requestModel.Email ?? companyClient.Client.Email;
        companyClient.Client.PhoneNumber = requestModel.PhoneNumber ?? companyClient.Client.PhoneNumber;

        await clientRepository.SaveAsync();
    }

    public async Task EditPhysicalClientAsync(int clientId, EditPhysicalClientRequestModel requestModel)
         
    {
        var physicalClient = await clientRepository.GetPhysicalClientByIdAsync(clientId);
        if (physicalClient is null)
        {
            throw new NotFoundException($"Client with id:{clientId} does not exist.");
        }

        physicalClient.FirstName = requestModel.FirstName ?? physicalClient.FirstName;
        physicalClient.LastName = requestModel.LastName ?? physicalClient.LastName;
        physicalClient.Client.Address = requestModel.Address ?? physicalClient.Client.Address;
        physicalClient.Client.Email = requestModel.Email ?? physicalClient.Client.Email;
        physicalClient.Client.PhoneNumber = requestModel.PhoneNumber ?? physicalClient.Client.PhoneNumber;

        await clientRepository.SaveAsync();
    }

    public async Task<List<PhysicalClient>> GetAllPhysicalClients()
    {
        return await clientRepository.GetAllPhysicalClients();
    }

    public async Task<List<CompanyClient>> GetAllCompanyClients()
    {
        return await clientRepository.GetAllCompanyClients();
    }

    public async Task<GetPhysicalClientResponseModel> GetPhysicalClientById(int id)
    {
        var client = await clientRepository.GetPhysicalClientByIdAsync(id);
        if (client is null)
        {
            throw new NotFoundException($"Client with id:{id} does not exist.");
        }
        return new GetPhysicalClientResponseModel
        {
            
            Address = client.Client.Address,
            Email = client.Client.Email,
            PhoneNumber = client.Client.PhoneNumber,
            FirstName = client.FirstName,
            LastName = client.LastName,
            PESEL = client.PESEL
        };
    }

    public async Task<GetCompanyClientResponseModel> GetCompanyClientById(int id)
    {
        var client = await clientRepository.GetCompanyClientByIdAsync(id);
        if (client is null)
        {
            throw new NotFoundException($"Client with id:{id} does not exist.");
        }
        return new GetCompanyClientResponseModel
        {
            Address = client.Client.Address,
            Email = client.Client.Email,
            PhoneNumber = client.Client.PhoneNumber,
            Name = client.Name,
            KRS = client.KRS
        };
    }

    private async Task CheckIfClientExists(Client client)
    {
        var existingClient = await clientRepository.GetClientByPhoneOrEmailAsync(client);
        if (existingClient == null) return;
        if (existingClient.PhoneNumber == client.PhoneNumber)
        {
            throw new ClientAlreadyExistsException(
                $"Client with the phone number:{client.PhoneNumber} already exists");
        }

        if (existingClient.Email == client.Email)
        {
            throw new ClientAlreadyExistsException(
                $"Client with the email:{client.Email} already exists");
        }
    }
}