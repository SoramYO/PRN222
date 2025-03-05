using System;
using System.Collections.Generic;

namespace MilkTeaShopBOs.Models;

public partial class PaymentMethod
{
    public int PaymentMethodId { get; set; }

    public string MethodName { get; set; } = null!;

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
