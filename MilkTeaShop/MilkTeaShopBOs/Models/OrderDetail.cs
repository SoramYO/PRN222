using System;
using System.Collections.Generic;

namespace MilkTeaShopBOs.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Order? Order { get; set; }

    public virtual ICollection<OrderExtraProduct> OrderExtraProducts { get; set; } = new List<OrderExtraProduct>();

    public virtual Product? Product { get; set; }
}
