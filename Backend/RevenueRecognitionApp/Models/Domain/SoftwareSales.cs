using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RevenueRecognition.Models;

[Table("Software_Sales")]
[PrimaryKey("SoftwareId", "SaleId")]
public class SoftwareSales
{
    [ForeignKey("Software")]
    public int SoftwareId { get; set; }
    public Software Software { get; set; }
    
    [ForeignKey("Sale")]
    public int SaleId { get; set; }
    public Sale Sale { get; set; }
    
    [Column(TypeName = "decimal(5,2)")]
    public decimal Value { get; set; }
}