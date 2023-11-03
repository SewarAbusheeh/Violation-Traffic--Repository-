using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Violation_p2.Models;

public partial class Blog
{
    [Key]
    public int BlogId { get; set; }

    public string Title { get; set; } = null!;

    public string? Imagepath { get; set; }
    [NotMapped]
    public IFormFile? Imagefile { get; set; }
    public string? Content { get; set; }

    public DateTime? PublishDate { get; set; }

    public int? Userid { get; set; }

    public virtual User1? User { get; set; }
}
