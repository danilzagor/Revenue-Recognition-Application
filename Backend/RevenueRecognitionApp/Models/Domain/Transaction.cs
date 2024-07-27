using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace RevenueRecognition.Models;

public class Transaction
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("Contract")]
    public int ContractId { get; set; }
    public Contract Contract { get; set; }

    [Column(TypeName = "money")]
    public decimal Amount { get; set; }
    
    public DateOnly TransactionDate { get; set; }
}