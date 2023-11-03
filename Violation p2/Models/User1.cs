using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Violation_p2.Models;

public partial class User1
{
    [Key]
    public int User1Id { get; set; }

    [Required]
    public string? Username { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,}$", ErrorMessage = "The password must have at least one lowercase letter, one uppercase letter, one numeric digit, and be at least 8 characters long.")]
    public string? Password { get; set; }
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    public string? Imagepath { get; set; }


    [NotMapped]
    public IFormFile ImageFile { get; set; }
    public int? RoleId { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
