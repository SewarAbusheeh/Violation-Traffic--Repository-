using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Violation_p2.Models;

public partial class Testimonial
{
    [Key]
    public int TestimonialId { get; set; }

    public int UserId { get; set; }

    //[System.ComponentModel.DataAnnotations.Required]
    public string? Content { get; set; }

    //[System.ComponentModel.DataAnnotations.Required]
    public string? Subject { get; set; }
    public Boolean Isapproved { get; set; }

    public virtual User1? User { get; set; }
}
