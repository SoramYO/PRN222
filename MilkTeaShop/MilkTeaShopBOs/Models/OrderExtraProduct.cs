using System;
using System.Collections.Generic;

namespace MilkTeaShopBOs.Models;

public partial class OrderExtraProduct
{
    public int OrderDetailId { get; set; }

    public int ExtraProductId { get; set; }

    public int Quantity { get; set; }

    public virtual ExtraProduct ExtraProduct { get; set; } = null!;

    public virtual OrderDetail OrderDetail { get; set; } = null!;
}
