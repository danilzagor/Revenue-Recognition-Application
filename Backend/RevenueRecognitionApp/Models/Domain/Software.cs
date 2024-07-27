using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognition.Models;

public class Software
{
    [Key] 
    public int Id { get; set; }

    [MaxLength(50)]
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    [Column(TypeName = "money")]
    public decimal Price { get; set; }
    
    public IEnumerable<SoftwareVersion> SoftwareVersions { get; set; }
    
    public IEnumerable<SoftwareCategories> SoftwareCategories { get; set; }
    public IEnumerable<SoftwareSales> SoftwareSales { get; set; }
    
}