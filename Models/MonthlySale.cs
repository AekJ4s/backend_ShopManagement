using System;
using System.Collections.Generic;

namespace backend_ShopManagement.Models;

public partial class MonthlySale
{
    public int Id { get; set; }

    public int DailysalesId { get; set; }

    public DateTime? Month { get; set; }

    public int? Total { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual DailySale Dailysales { get; set; } = null!;
}
