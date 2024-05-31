using System;
using System.Collections.Generic;

namespace backend_ShopManagement.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public int? Price { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<DailySale> DailySales { get; set; } = new List<DailySale>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
