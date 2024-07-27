using RevenueRecognition.Exceptions;
using RevenueRecognition.Models;
using RevenueRecognition.Repositories;
using RevenueRecognition.RequestModels.TransactionRequestModels;

namespace RevenueRecognition.Services;

public interface ITransactionService
{
    Task MakeTransactionAsync(int contractId, MakeTransactionRequestModel requestModel);
}

public class TransactionService(ITransactionRepository transactionRepository, IContractRepository contractRepository) : ITransactionService
{
    public async Task MakeTransactionAsync(int contractId , MakeTransactionRequestModel requestModel)
    {
        var contract = await contractRepository.GetByIdAsync(contractId);
        if (contract is null)
        {
            throw new NotFoundException($"Contract with id:{contractId} does not exist.");
        }

        if (contract.ContractStatus.Name == "Signed")
        {
            throw new ContractIsSignedException(
                $"Contract with id:{contractId} has been already payed and signed.");
        }
        
        if (contract.ContractStatus.Name=="Outdated")
        {
            throw new ContractIsOutdatedException(
                $"Contract with id:{contractId} is outdated. This contract is no longer active and your money will be returned.");
        }
        
        if (contract.EndingDate < requestModel.TransactionDate)
        {
            contract.StatusId = await contractRepository.GetStatusIdByNameAsync("Outdated");
            await contractRepository.SaveAsync();
            throw new ContractIsOutdatedException(
                $"Contract with id:{contractId} is outdated. This contract is no longer active and your money will be returned.");
        }
        
        

        var transaction = new Transaction
        {
            ContractId = contractId,
            Amount = requestModel.Amount,
            TransactionDate = requestModel.TransactionDate
        };

        await transactionRepository.AddAsync(transaction);

        var allTransactionsByThisContract = transactionRepository
            .GetAllByContractId(contractId);

        var overallTransactionsAmount = allTransactionsByThisContract.Sum(transactionInRepo => transactionInRepo.Amount);
        if (overallTransactionsAmount > contract.Price)
        {
            throw new TransactionAbovePriceException(
                $"Your transaction amount is overpaying for contract with id:{contractId}. " +
                $"Price of a contract:{contract.Price}." +
                $" You paid (not including this transaction):{overallTransactionsAmount-requestModel.Amount}. You need to pay the exact amount.");
        }
        if (overallTransactionsAmount == contract.Price)
        {
            contract.StatusId = await contractRepository.GetStatusIdByNameAsync("Signed");
        }
        
        await transactionRepository.SaveAsync();
    }
}