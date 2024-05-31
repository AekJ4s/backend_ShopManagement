

using System.ComponentModel.DataAnnotations;
using System.Data;
using backend_ShopManagement.Data;
using backend_ShopManagement.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace backend_ShopManagement.Models
{
    public class UserMetadata
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

    }

    public class UserCreate
    {
        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? Firstname { get; set;}

        public string? Lastname { get; set;}

    }

    public class UserEditer{
        public int? Id { get; set; }
        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? Firstname { get; set;}

        public string? Lastname { get; set;}
        public int? RoleId { get; set; }
        public string? NewPassword {get; set;}


    }


    public class UserLogin
    {
        public string? Username { get; set; }

        public string? Password { get; set; }

    }

    [MetadataType(typeof(UserMetadata))]

    public partial class User
    {
        public static string CreateUser(DatabaseContext db, User user)
        {
            try
            {
                user.RoleId = 1;
                user.CreateDate = DateTime.Now;
                user.UpdateDate = DateTime.Now;
                user.IsDeleted = false;
                db.Users.Add(user);
                db.SaveChanges();
                return "success";

            }
            catch (Exception e)
            {      
                return e.InnerException.Message;
            }

        }

        public static List<User> GetAllUser(DatabaseContext db)
        {
            List<User> returnThis = db.Users.Where(q => q.IsDeleted != true).ToList();
            return returnThis;
        }

        public static User EditPassword(DatabaseContext db, User user)
        {
            user.UpdateDate = DateTime.Now;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return user;
        }

        public static User GetById(DatabaseContext db, int id)
        {
            User? returnThis = db.Users.Where(q => q.Id == id && q.IsDeleted != true).FirstOrDefault();
            return returnThis ?? new User();
        }


        // DELETE CAN NOT RECOVER
        public static User Delete(DatabaseContext db, int id)
        {
            User user = GetById(db, id);
            user.IsDeleted = true;
            db.Entry(user).State = EntityState.Modified; // Soft Delete
            db.SaveChanges();

            return user;
        }

        // DELETEC CAN RECOVER
        public static User Banned(DatabaseContext db,int id)
         {
            User user = GetById(db, id);
            user.IsBanned = !user.IsBanned;
            db.Entry(user).State = EntityState.Modified; // Soft Delete
            db.SaveChanges();
            return user;
        }


    }

}