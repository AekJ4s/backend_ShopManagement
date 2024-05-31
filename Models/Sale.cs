using System;
using System.Collections.Generic;

namespace backend_ShopManagement.Models;

public partial class Sale
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public DateTime? SaleDate { get; set; }

    public int? TotalAmount { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User? User { get; set; }
}
