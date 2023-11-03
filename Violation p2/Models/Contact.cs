using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Violation_p2.Models;

public partial class Contact
{
    [Key]
    public int ContactId { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Subject { get; set; }
    [Required]
    public string? Message { get; set; }
    [Required]
    public string? Email { get; set; }
    public DateTime? Contactdate { get; set; }
}
