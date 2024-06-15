using System;
using System.Collections.Generic;

namespace ApiBlog.Data.Models;

public partial class UserProfile
{
    public int UserId { get; set; }

    public string? Bio { get; set; }

    public string? ProfileImageUrl { get; set; }

    public virtual User User { get; set; } = null!;
}
