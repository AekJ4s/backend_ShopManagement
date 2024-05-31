

using System.ComponentModel.DataAnnotations;
using System.Data;
using backend_ShopManagement.Data;
using backend_ShopManagement.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace backend_ShopManagement.Models
{
    public class CategoryMetadata
    {
   public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? CreateDate { get; set; }

    public string? UpdateDate { get; set; }

    public string? IsDeleted { get; set; }

    }

    public class CategoryCreate
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class CategoryEditer{
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

    }

    [MetadataType(typeof(CategoryMetadata))]

    public partial class Category
    {
        public static string CreateCategory(DatabaseContext db, Category category)
        {
            try
            {
                category.CreateDate = DateTime.Now;
                category.UpdateDate = DateTime.Now;
                category.IsDeleted = false;
                db.Categories.Add(category);
                db.SaveChanges();
                return "success";

            }
            catch (Exception e)
            {      
                return e.InnerException.Message;
            }

        }

        public static List<Category> GetAllCategory(DatabaseContext db)
        {
            List<Category> returnThis = db.Categories.Where(q => q.IsDeleted != true).ToList();
            return returnThis;
        }

        public static Category EditCategory(DatabaseContext db, Category category)
        {
            category.UpdateDate = DateTime.Now;
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
            return category;
        }

        public static Category GetCategoryById(DatabaseContext db, int id)
        {
            Category? returnThis = db.Categories.Where(q => q.Id == id && q.IsDeleted != true).FirstOrDefault();
            return returnThis ?? new Category();
        }


        // DELETE CAN NOT RECOVER
        public static Category DeleteCategory(DatabaseContext db, int id)
        {
            Category category = GetCategoryById(db, id);
            category.IsDeleted = true;
            db.Entry(category).State = EntityState.Modified; // Soft Delete
            db.SaveChanges();

            return category;
        }



    }

}