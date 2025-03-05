using System;
using System.Collections.Generic;

namespace MilkTeaShopBOs.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int? CategoryId { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public bool? Status { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();

    public virtual ICollection<ExtraProduct> ExtraProducts { get; set; } = new List<ExtraProduct>();
}
