using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [MaxLength(50)]
    public string Name { get; set; }
    
    public IEnumerable<SoftwareCategories> SoftwareCategories { get; set; }
}