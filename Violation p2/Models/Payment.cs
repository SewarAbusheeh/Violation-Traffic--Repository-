using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Violation_p2.Models;

public partial class Payment
{
    [Key]
    public int PaymentId { get; set; }

    public int? ViolationId { get; set; }
    public int? VehicleId { get; set; }

    public DateTime? Paymentdate { get; set; }

    public decimal? Paymentamount { get; set; }
   // public string RazorpayPaymentId { get; set; }
    public string? Paymentmethod { get; set; }  // Assuming you're still integrating with Razorpay
  
    public virtual Violation? Violation { get; set; }
    public virtual Vehicle? Vehicle { get; set; }
}
