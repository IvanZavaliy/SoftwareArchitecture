using System.ComponentModel.DataAnnotations;

namespace DemoAPI.Model;

public class StudentEntity
{
    [Key]
    public int Id { get; set; }
        
    [Required] // Валідація: поле обов'язкове
    public string Name { get; set; }
        
    [Required]
    public int Age { get; set; }
        
    [Required]
    public string Standard { get; set; } // Наприклад, клас навчання
        
    [Required]
    public string EmailAddress { get; set; }
}