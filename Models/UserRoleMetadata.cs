

using System.ComponentModel.DataAnnotations;
using System.Data;
using backend_ShopManagement.Data;
using backend_ShopManagement.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace backend_ShopManagement.Models
{
    public class UserRoleMetadata
    {
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
    }

    public class RoleCreate
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

    }

    public class RoleEditer{

        required public int Id { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }

    }


    [MetadataType(typeof(UserRoleMetadata))]

    public partial class UserRole
    {
        public static string CreateRole(DatabaseContext db, UserRole role)
        {
            try
            {
                role.CreateDate = DateTime.Now;
                role.UpdateDate = DateTime.Now;
                role.IsDeleted = false;
                db.UserRoles.Add(role);
                db.SaveChanges();
                return "success";

            }
            catch (Exception e)
            {      
                return e.InnerException.Message;
            }

        }

        public static List<UserRole> GetAllRole(DatabaseContext db)
        {
            List<UserRole> returnThis = db.UserRoles.Where(q => q.IsDeleted != true).ToList();
            return returnThis;
        }

        public static UserRole EditRole(DatabaseContext db, UserRole role)
        {
            role.UpdateDate = DateTime.Now;
            db.Entry(role).State = EntityState.Modified;
            db.SaveChanges();
            return role;
        }

        public static UserRole GetRoleId(DatabaseContext db, int id)
        {
            UserRole? returnThis = db.UserRoles.Where(q => q.Id == id && q.IsDeleted != true).FirstOrDefault();
            return returnThis ?? new UserRole();
        }

        public static UserRole DeleteRole(DatabaseContext db, int id)
        {
            UserRole role = GetRoleId(db, id);
            role.IsDeleted = true;
            // db.Employees.Remove(employee); เป็นวิธีการลบแบบให้หายไปเลย
            db.Entry(role).State = EntityState.Modified; // Soft Delete
            db.SaveChanges();

            return role;
        }


    }

}