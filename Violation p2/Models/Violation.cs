using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Violation_p2.Models;

public partial class Violation
{
    [Key]
    public int ViolationId { get; set; }

    public int? VehicleId { get; set; }

    public int? ViolationtypeId { get; set; }

    public DateTime? Violationdate { get; set; }

    public string? RazorpayPaymentId { get; set; }
    public decimal? Fineamount { get; set; }

    public bool Ispaid { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Vehicle? Vehicle { get; set; }

    public virtual Violationtype? Violationtype { get; set; }
}
