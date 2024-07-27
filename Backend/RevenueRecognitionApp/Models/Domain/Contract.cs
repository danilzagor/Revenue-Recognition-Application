using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognition.Models;

public class Contract
{
    [Key]
    public int Id { get; set; }

    public DateOnly BeginningDate { get; set; }
    
    public DateOnly EndingDate { get; set; }
    
    [ForeignKey("ContractStatus")]
    public int StatusId { get; set; }
    public ContractStatus ContractStatus { get; set; }
    
    [Column(TypeName = "money")]
    public decimal Price { get; set; }
    
    public int ActualisationPeriod { get; set; }
    
    [ForeignKey("SoftwareVersion")]
    public int SoftwareAndVersionId { get; set; }
    public SoftwareVersion SoftwareVersion { get; set; }

    [ForeignKey("SoftwaClientreVersion")]
    public int ClientId { get; set; }
    public Client Client { get; set; }
    public IEnumerable<Transaction> Transactions { get; set; }
}