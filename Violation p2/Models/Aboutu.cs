using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Violation_p2.Models;

public partial class Aboutu
{
    [Key]
    public int AboutuId { get; set; }

    public string? Imagepath { get; set; }
    [NotMapped]
    public IFormFile Imagefile { get; set; }


    public string Title { get; set; } = null!;

    public string? Pargraph { get; set; }

    public string? Content { get; set; }
}
