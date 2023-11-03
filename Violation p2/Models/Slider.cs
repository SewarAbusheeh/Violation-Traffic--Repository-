using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Violation_p2.Models;

public partial class Slider
{

    [Key]
   
    public int SliderId { get; set; }

    public string? Imagepath { get; set; }
    public string? Imagepath1 { get; set; }
    [NotMapped]

    public IFormFile Imagefile { get; set; }

    public string? Caption1 { get; set; }

    public string? Caption2 { get; set; }

    public string? Caption3 { get; set; }
}
