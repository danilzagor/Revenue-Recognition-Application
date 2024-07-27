using Microsoft.EntityFrameworkCore;
using RevenueRecognition.Contexts;
using RevenueRecognition.Models;

namespace RevenueRecognition.Repositories;

public interface IContractRepository : IGenericRepository<Contract>
{
    public new Task<Contract?> GetByIdAsync(int id);
    public new Task AddAsync(Contract contract);
    public Task<Contract?> ContractWithClientActiveAsync(int clientId, DateOnly beginningDate, int softwareId);
    public Task<Contract?> ClientInPastAsync(int clientId);
    public Task<int> GetStatusIdByNameAsync(string name);
    public IEnumerable<Contract> GetSignedContracts();
    public IEnumerable<Contract> GetSignedContractsByProductId(int productId);
    
    public IEnumerable<Contract> GetActiveAndSignedContracts();
    public IEnumerable<Contract> GetActiveAndSignedContractsByProductId(int productId);
    public IEnumerable<Contract> GetAll();
}

public class ContractRepository(DatabaseContext context) : GenericRepository<Contract>(context), IContractRepository
{
    public new async Task<Contract?> GetByIdAsync(int id)
    {
        return await context.Contracts
            .Include(contract => contract.SoftwareVersion)
            .ThenInclude(version => version.Software)
            .Include(contract => contract.ContractStatus)
            .Include(contract => contract.Client)
            .FirstOrDefaultAsync(contract => contract.Id==id);
    }

    public new async Task AddAsync(Contract contract)
    {
        await context.Contracts.AddAsync(contract);
    }

    public async Task<Contract?> ContractWithClientActiveAsync(int clientId, DateOnly beginningDate, int softwareId)
    {
        var result = await context.Contracts
            .Include(contract => contract.Client)
            .Include(contract => contract.SoftwareVersion)
            .Where(contract => 
                contract.Client.Id == clientId &&
                contract.EndingDate >= beginningDate &&  
                contract.SoftwareVersion.SoftwareId == softwareId
            )
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<Contract?> ClientInPastAsync(int clientId)
    {
        return await context.Contracts
            .Include(contract => contract.Client)
            .Where(contract => contract.Client.Id == clientId)
            .FirstOrDefaultAsync();
    }

    public async Task<int> GetStatusIdByNameAsync(string name)
    {
        var contractStatus = await context.ContractStatuses
            .FirstOrDefaultAsync(status => status.Name == name);
        if (contractStatus is null)
        {
            return -1;
        }

        return contractStatus.Id;
    }

    public IEnumerable<Contract> GetSignedContracts()
    {
        return context.Contracts
            .Include(contract => contract.ContractStatus)
            .Where(contract => contract.ContractStatus.Name == "Signed");
    }

    public IEnumerable<Contract> GetSignedContractsByProductId(int productId)
    {
        return context.Contracts
            .Include(contract => contract.ContractStatus)
            .Include(contract => contract.SoftwareVersion)
            .Where(contract => contract.ContractStatus.Name == "Signed" && contract.SoftwareVersion.SoftwareId==productId);
            
    }

    public IEnumerable<Contract> GetActiveAndSignedContracts()
    {
        return context.Contracts
            .Include(contract => contract.ContractStatus)
            .Include(contract => contract.SoftwareVersion)
            .Where(contract => contract.ContractStatus.Name != "Outdated");
    }

    public IEnumerable<Contract> GetActiveAndSignedContractsByProductId(int productId)
    {
        return context.Contracts
            .Include(contract => contract.ContractStatus)
            .Include(contract => contract.SoftwareVersion)
            .Where(contract => contract.ContractStatus.Name != "Outdated" && contract.SoftwareVersion.SoftwareId==productId);
    }

    public IEnumerable<Contract> GetAll()
    {
        return context.Contracts
            .Include(contract => contract.Client)
            .Include(contract => contract.ContractStatus)
            .Include(contract => contract.SoftwareVersion)
            .ThenInclude(version => version.Software);
    }
}
