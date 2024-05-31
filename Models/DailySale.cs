using System;
using System.Collections.Generic;

namespace backend_ShopManagement.Models;

public partial class DailySale
{
    public int Id { get; set; }

    public int? TransactionsId { get; set; }

    public DateTime? SaleDate { get; set; }

    public int? Total { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<MonthlySale> MonthlySales { get; set; } = new List<MonthlySale>();

    public virtual Transaction? Transactions { get; set; }
}
