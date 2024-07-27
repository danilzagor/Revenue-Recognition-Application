using Microsoft.EntityFrameworkCore;
using RevenueRecognition.Contexts;
using RevenueRecognition.Models;

namespace RevenueRecognition.Repositories;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
    public IEnumerable<Transaction> GetAllByContractId(int id);
    public IEnumerable<Transaction> GetAllBySoftwareId(int id);
}

public class TransactionRepository(DatabaseContext context) : GenericRepository<Transaction>(context), ITransactionRepository
{
    public IEnumerable<Transaction> GetAllByContractId(int id)
    {
        return context.Transactions
            .Include(transaction => transaction.Contract)
            .ThenInclude(contract => contract.Client)
            .Where(transaction => transaction.ContractId == id);
    }

    public IEnumerable<Transaction> GetAllBySoftwareId(int id)
    {
        return context.Transactions
            .Include(transaction => transaction.Contract)
            .ThenInclude(contract => contract.SoftwareVersion)
            .Where(transaction => transaction.Contract.SoftwareVersion.SoftwareId==id);
    }
}