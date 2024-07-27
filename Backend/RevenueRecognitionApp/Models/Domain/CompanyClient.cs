using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognition.Models;

public class CompanyClient
{
    [Key]
    [ForeignKey("Client")]
    public int Id { get; set; }
    public Client Client { get; set; }

    [MaxLength(150)]
    public string Name { get; set; }

    [MaxLength(14)]
    public string KRS { get; set; }
}