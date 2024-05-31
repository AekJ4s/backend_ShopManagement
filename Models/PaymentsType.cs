using System;
using System.Collections.Generic;

namespace backend_ShopManagement.Models;

public partial class PaymentsType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? AccountNumber = "0";

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
