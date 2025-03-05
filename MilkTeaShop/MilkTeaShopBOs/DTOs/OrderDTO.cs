using System;
using System.Collections.Generic;

namespace MilkTeaShopBOs.DTOs;
public class OrderDTO
{
    public int CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderDetailDTO> OrderDetails { get; set; } = new();
    public int PaymentMethodId { get; set; }
}

public class OrderDetailDTO
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public List<OrderExtraProductDTO> ExtraProducts { get; set; } = new();
}

public class OrderExtraProductDTO
{
    public int ExtraProductId { get; set; }
    public int Quantity { get; set; }
}