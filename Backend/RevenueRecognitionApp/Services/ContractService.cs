using RevenueRecognition.Exceptions;
using RevenueRecognition.Models;
using RevenueRecognition.Repositories;
using RevenueRecognition.RequestModels.ContractRequestModels;
using RevenueRecognition.ResponseModels.ContractResponseModels;

namespace RevenueRecognition.Services;

public interface IContractService
{
    Task<GetContractResponseModel> AddContractAsync(AddContractRequestModel requestModel, int clientId);
    Task<GetContractResponseModel> GetContractAsync(int contractId);
    Task<List<GetContractResponseModel>> GetContractByClientIdAsync(int clientId);
}

public class ContractService(IContractRepository contractRepository, ISoftwareRepository softwareRepository, IClientRepository clientRepository, ITransactionRepository transactionRepository) : IContractService
{
    public async Task<GetContractResponseModel> AddContractAsync(AddContractRequestModel requestModel, int clientId)
    {
        var timePeriod = (requestModel.EndingDate.ToDateTime(TimeOnly.MinValue) -
                              requestModel.BeginningDate.ToDateTime(TimeOnly.MinValue)).Days;
        switch (timePeriod)
        {
            case < 3:
                throw new DatePeriodIsIncorrectException(
                    $"Time period between {requestModel.BeginningDate} and {requestModel.EndingDate} is less than 3 days.");
            case > 30:
                throw new DatePeriodIsIncorrectException(
                    $"Time period between {requestModel.BeginningDate} and {requestModel.EndingDate} is more than 30 days.");
        }

        var client = await clientRepository.GetByIdAsync(clientId);
        
        if (client is null)
        {
            throw new NotFoundException($"Client with id:{clientId} does not exist.");
        }
        
        var software = await softwareRepository.GetByIdAsync(requestModel.SoftwareId);

        if (software is null)
        {
            throw new NotFoundException($"Software with id:{requestModel.SoftwareId} does not exist.");
        }
        
        var activeContract = await contractRepository.ContractWithClientActiveAsync(clientId, requestModel.BeginningDate, requestModel.SoftwareId);
        
        if (activeContract is not null)
        {
            throw new ActiveContractException($"Client with id:{clientId} has an active contract.");
        }

        

        var sale = software.SoftwareSales
            .Where(sales => sales.Sale.StartAt <= requestModel.BeginningDate
                            && sales.Sale.EndAt >= requestModel.BeginningDate).ToList();
            

        var price = software.Price;

        price += (requestModel.ActualisationPeriod - 1) * 1000;

        if (await contractRepository.ClientInPastAsync(clientId) is not null)
        {
            price -= price * (decimal)0.05;
        }

        if (sale.Count != 0)
        {
            price -= price * sale.Max(sales => sales.Value);
        }

        var contract = new Contract
        {
            ActualisationPeriod = requestModel.ActualisationPeriod,
            BeginningDate = requestModel.BeginningDate,
            EndingDate = requestModel.EndingDate,
            ClientId = clientId,
            StatusId = 1,
            Price = price,
            SoftwareAndVersionId = requestModel.SoftwareVersionId
        };
        
        await contractRepository.AddAsync(contract);
        await contractRepository.SaveAsync();

        return await GetContractAsync(contract.Id);
    }

    public async Task<GetContractResponseModel> GetContractAsync(int contractId)
    {
        var contract = await contractRepository.GetByIdAsync(contractId);
        if (contract is null)
        {
            throw new NotFoundException($"Contract with id:{contractId} does not exist.");
        }

        var result = new GetContractResponseModel
        {
            Contract = new ContractDetailsResponseModel
            {
                Id = contract.Id,
                ActualisationPeriod = contract.ActualisationPeriod,
                BeginningDate = contract.BeginningDate,
                EndingDate = contract.EndingDate,
                ContractStatus = contract.ContractStatus.Name,
                Price = contract.Price,
            },
            Software = new SoftwareResponseModel
            {
                Description = contract.SoftwareVersion.Software.Description,
                Id = contract.SoftwareVersion.SoftwareId,
                Name = contract.SoftwareVersion.Software.Name,
                Price = contract.SoftwareVersion.Software.Price
            },
            SoftwareVersion = contract.SoftwareVersion.Name,
            Client = new ClientDetailsResponseModel
            {
                Address = contract.Client.Address,
                Email = contract.Client.Email,
                Id = contract.Client.Id,
                PhoneNumber = contract.Client.PhoneNumber
            },
            PaidAmount = transactionRepository
                .GetAllByContractId(contractId)
                .Sum(contractInRepo=>contractInRepo.Amount)
        };
        return result;
    }

    public async Task<List<GetContractResponseModel>> GetContractByClientIdAsync(int clientId)
    {
        var contracts = contractRepository.GetAll();
        var result =  contracts
            .Where(contract => contract.ClientId == clientId)
            .Select(contract => new GetContractResponseModel
            {
                Contract = new ContractDetailsResponseModel
                {
                    Id = contract.Id,
                    ActualisationPeriod = contract.ActualisationPeriod,
                    BeginningDate = contract.BeginningDate,
                    EndingDate = contract.EndingDate,
                    ContractStatus = contract.ContractStatus.Name,
                    Price = contract.Price,
                },
                Software = new SoftwareResponseModel
                {
                    Description = contract.SoftwareVersion.Software.Description,
                    Id = contract.SoftwareVersion.SoftwareId,
                    Name = contract.SoftwareVersion.Software.Name,
                    Price = contract.SoftwareVersion.Software.Price
                },
                SoftwareVersion = contract.SoftwareVersion.Name,
                Client = new ClientDetailsResponseModel
                {
                    Address = contract.Client.Address,
                    Email = contract.Client.Email,
                    Id = contract.Client.Id,
                    PhoneNumber = contract.Client.PhoneNumber
                },
                PaidAmount = transactionRepository
                    .GetAllByContractId(contract.Id)
                    .Sum(contractInRepo => contractInRepo.Amount)
            }).ToList();

        return result;
    }
}