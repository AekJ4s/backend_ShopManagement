

using System.ComponentModel.DataAnnotations;
using System.Data;
using backend_ShopManagement.Data;
using backend_ShopManagement.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace backend_ShopManagement.Models
{
    public class PaymentsTypeMetadata
    {
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? AccountNumber { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDeleted { get; set; }

    }

    public class TypeCreate
    {
        public string? Name { get; set; }

        public string? AccountNumber { get; set; } 

    }

    public class TypeEditer{

        required public int Id { get; set; }
        public string? Name { get; set; }

            public string? AccountNumber { get; set; }



    }


    [MetadataType(typeof(PaymentsType))]

    public partial class PaymentsType
    {
        public static string CreateType(DatabaseContext db, PaymentsType type)
        {
            try
            {
                type.CreateDate = DateTime.Now;
                type.UpdateDate = DateTime.Now;
                type.IsDeleted = false;
                db.PaymentsTypes.Add(type);
                db.SaveChanges();
                return "success";

            }
            catch (Exception e)
            {      
                return e.InnerException.Message;
            }

        }

        public static List<PaymentsType> GetAllPaymentsType(DatabaseContext db)
        {
            List<PaymentsType> returnThis = db.PaymentsTypes.Where(q => q.IsDeleted != true).ToList();
            return returnThis;
        }

        public static PaymentsType EditType(DatabaseContext db, PaymentsType type)
        {
            type.UpdateDate = DateTime.Now;
            db.Entry(type).State = EntityState.Modified;
            db.SaveChanges();
            return type;
        }

        public static PaymentsType GetPaymentsTypeId(DatabaseContext db, int id)
        {
            PaymentsType? returnThis = db.PaymentsTypes.Where(q => q.Id == id && q.IsDeleted != true).FirstOrDefault();
            return returnThis ?? new PaymentsType();
        }

        public static PaymentsType DeletePaymentsType(DatabaseContext db, int id)
        {
            PaymentsType type = GetPaymentsTypeId(db, id);
            type.IsDeleted = true;
            // db.Employees.Remove(employee); เป็นวิธีการลบแบบให้หายไปเลย
            db.Entry(type).State = EntityState.Modified; // Soft Delete
            db.SaveChanges();

            return type;
        }


    }

}