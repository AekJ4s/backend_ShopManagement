using System;
using System.Collections.Generic;

namespace backend_ShopManagement.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int? TransactionId { get; set; }

    public int? PaymentTypeid { get; set; }

    public int? Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual PaymentsType? PaymentType { get; set; }

    public virtual Transaction? Transaction { get; set; }
}
