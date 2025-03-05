using System;
using System.Collections.Generic;

namespace MilkTeaShopBOs.Models;

public partial class ExtraProduct
{
    public int ExtraProductId { get; set; }

    public string ExtraProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<OrderExtraProduct> OrderExtraProducts { get; set; } = new List<OrderExtraProduct>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
