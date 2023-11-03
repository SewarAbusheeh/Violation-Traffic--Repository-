using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Violation_p2.Models;

public partial class Vehicle
{
    [Key]
    public int VehicleId { get; set; }

    public int? User1Id { get; set; }

    public string? Licenseplate { get; set; }

    public string? Model { get; set; }

    public string? Type { get; set; }

    public string? Color { get; set; }
    public string? Imagepath { get; set; }
    [NotMapped]
    public IFormFile  ImageFile { get; set; }
    public DateTime? Licenseexpirydate { get; set; }

    public virtual User1? User { get; set; }

    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
