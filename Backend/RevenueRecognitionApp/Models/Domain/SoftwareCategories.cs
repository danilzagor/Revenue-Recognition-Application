using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RevenueRecognition.Models;

[Table("Software_Categories")]
[PrimaryKey("SoftwareId", "CategoryId")]
public class SoftwareCategories
{
    [ForeignKey("Software")]
    public int SoftwareId { get; set; }
    public Software Software { get; set; }
    
    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}