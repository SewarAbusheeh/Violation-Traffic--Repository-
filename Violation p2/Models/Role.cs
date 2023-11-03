using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Violation_p2.Models;

public partial class Role
{
    [Key]
    public int RoleId { get; set; }

    public string? Rolename { get; set; }

    public string? RoleCategory { get; set; }

    public virtual ICollection<User1> User1s { get; set; } = new List<User1>();
}
