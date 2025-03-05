using System;
using System.Collections.Generic;

namespace MilkTeaShopBOs.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Role { get; set; }

    public DateTime? JoinDate { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
