using System;
using System.Collections.Generic;

namespace PRN222.Lab1.Repository.Models;

public partial class AccountMember
{
    public string MemerId { get; set; } = null!;

    public string MemberPassword { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public int? MemberRole { get; set; }
}
