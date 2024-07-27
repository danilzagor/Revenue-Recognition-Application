using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.Models;

public class Client
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(100)]
    public string Address { get; set; }
    
    [MaxLength(254)]
    public string Email { get; set; }
    
    [MaxLength(15)]
    public string PhoneNumber { get; set; }
    
    public DateTime? DeletedAt { get; set; }

    public IEnumerable<Contract> Contracts { get; set; }
    public IEnumerable<Transaction> Transactions { get; set; }
    
}