using System;
using System.Collections.Generic;

namespace MilkTeaShopBOs.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? OrderId { get; set; }

    public int? PaymentMethodId { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public virtual Order? Order { get; set; }

    public virtual PaymentMethod? PaymentMethod { get; set; }
}
