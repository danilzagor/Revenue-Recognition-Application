using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.Models;

public class ContractStatus
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public IEnumerable<Contract> Contracts { get; set; }
}