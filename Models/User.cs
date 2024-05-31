using System;
using System.Collections.Generic;

namespace backend_ShopManagement.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public int? RoleId { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsBanned { get; set; }

    public virtual UserRole? Role { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
