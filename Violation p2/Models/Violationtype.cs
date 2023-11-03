using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Violation_p2.Models;

public partial class Violationtype
{
    [Key]
    public int ViolationtypeId { get; set; }

    public string? Description { get; set; }

    public decimal? Basefineamount { get; set; }

    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
