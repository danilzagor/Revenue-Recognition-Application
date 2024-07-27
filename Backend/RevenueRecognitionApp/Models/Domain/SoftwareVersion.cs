using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognition.Models;

public class SoftwareVersion
{
    public int Id { get; set; }
    
    [MaxLength(25)]
    public string Name { get; set; }

    [ForeignKey("Software")]
    public int SoftwareId { get; set; }
    public Software Software { get; set; }
}