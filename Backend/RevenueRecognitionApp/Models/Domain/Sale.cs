using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.Models;

public class Sale
{
    [Key]
    public int Id { get; set; }

    [MaxLength(25)]
    public string Name { get; set; }
    
    public DateOnly StartAt { get; set; }
    
    public DateOnly EndAt { get; set; }
}