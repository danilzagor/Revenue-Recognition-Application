using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognition.Models;

public class PhysicalClient
{
    [Key]
    [ForeignKey("Client")]
    public int Id { get; set; }
    public Client Client { get; set; }

    [MaxLength(50)]
    public string FirstName { get; set; }
    
    [MaxLength(75)]
    public string LastName { get; set; }

    [MaxLength(11)]
    public string PESEL { get; set; }
}