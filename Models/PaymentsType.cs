using System;
using System.Collections.Generic;

namespace backend_ShopManagement.Models;

public partial class PaymentsType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? AccountNumber { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public DateTime? IsDeleted { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
