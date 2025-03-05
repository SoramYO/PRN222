using System;
using System.Collections.Generic;

namespace MilkTeaShopBOs.Models;

public partial class ProductVariant
{
    public int VariantId { get; set; }

    public int? ProductId { get; set; }

    public string? Size { get; set; }

    public decimal? Price { get; set; }

    public bool? Status { get; set; }

    public virtual Product? Product { get; set; }
}
