using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.IdentityModel.Tokens;
using backend_ShopManagement.Data;
using backend_ShopManagement.Models;
[ApiController]
[Route("categories")]

public class CategoryController : ControllerBase
{

    private DatabaseContext _db = new DatabaseContext();
    private readonly ILogger<CategoryController> _logger;
    private IHostEnvironment _hostingEnvironment;
    public CategoryController(ILogger<CategoryController> logger, IHostEnvironment? environment)
    {
        _logger = logger;
        _hostingEnvironment = environment;
    }

    [HttpPost("CreateCategory", Name = "CreateCategory")]
    public ActionResult<Response> CreateCategory([FromForm] CategoryCreate request)
    {
        var alreadyHave = _db.Categories.Where(c => c.Name == request.Name).Count();
        if (alreadyHave > 0)
        {
            return BadRequest(new Response
            {
                Code = 400,
                Message = "มีประเภทของสินค้านี้อยู่แล้ว",
                Data = null,
            });
        }
        Category newCategory = new Category
        {
            Name = request.Name,
            Description = request.Description,
            
        };
            newCategory.CreateDate = DateTime.Now;
            newCategory.UpdateDate = DateTime.Now;
            newCategory.IsDeleted = false;
            Category.CreateCategory(_db, newCategory);

        
        try{
             return Ok(new Response {
            Code = 200,
            Message = "สร้างประเภทสินค้าเรียบร้อยแล้ว",
            Data = newCategory,
        });
        }catch{
             return BadRequest(new Response {
            Code = 200,
            Message = "Internal Server Error",
            Data = null,
        });
        }
       
    }



    [HttpPost("ShowAllCategories", Name = "ShowAllCategories")]


    public ActionResult GetAllCategory()
    {
        // .OrderBy(q => q.Salary) เรียงจากน้อยไปมาก
        // .OrderByDescending(q => q.Salary) เรียงจากมากไปน้อย
        List<Category> categories = Category.GetAllCategory(_db);
        return Ok(categories);
    }

    [HttpGet("FindCategory/{id}", Name = "FindCategory")]
    public ActionResult GetCategoryById(int id)
    {
        // ค้นหาโปรเจคจากฐานข้อมูลโดยใช้ Id
        Category? category = _db.Categories.FirstOrDefault(p => p.Id == id && p.IsDeleted != true);

        if (category == null)
        {
            // หากไม่พบโปรเจคในฐานข้อมูล คืนค่า NotFound
            return NotFound(new Response
            {
                Code = 404,
                Message = "ค้นหาไม่เจออ่ะ"
            });
        }

        try
        {
            // ส่งข้อมูลโปรเจคกลับไปยังไคลเอนต์
            return Ok(new Response
            {
                Code = 200,
                Message = "ค้นหาเจอแล้ว",
                Data = category,
            });
        }
        catch (Exception e)
        {
            // หากเกิดข้อผิดพลาดในการส่งข้อมูล คืนค่า StatusCode 500 (Internal Server Error)
            return StatusCode(500, new Response
            {
                Code = 500,
                Message = "Internal server error : " + e.Message
            });
        }
    }

    [HttpPut("EditCategory", Name = "EditCategory")]

    public ActionResult<Response> EditCategory([FromForm] CategoryEditer request)
    {
        var CanUseCategory = _db.Categories.Where(c => c.Name == request.Name && c.IsDeleted != true).Count();
        Category CategoryData = _db.Categories.Where(e => e.Id == request.Id && e.IsDeleted != true).AsNoTracking().ToList().First();

        request.Name = (request.Name?.ToLower() == "string" || request.Name == CategoryData.Name || CategoryData.Name == null) ? null : request.Name;
        request.Description = (request.Description?.ToLower() == "string" || request.Description == CategoryData.Description || request.Description == null) ? null : request.Description;


        if (request.Name == null && request.Description == null)
        {
            request = null;
        }

        //ถ้ามีข้อมูล
        if (request != null)
        {

            CategoryData.Name = (request.Name?.ToLower() == "string" || request.Name == CategoryData.Name || request.Name == null) ? CategoryData.Name : request.Name;
            CategoryData.Description = (request.Description?.ToLower() == "string" || request.Description == CategoryData.Description || request.Description == null) ? CategoryData.Description : request.Description;


            Category.EditCategory(_db, CategoryData);
            _db.SaveChanges();
            try
            {
                return new Response
                {
                    Code = 200,
                    Message = "ประเภทสินค้าถูกแก้ไขแล้ว",
                    Data = CategoryData,
                };
            }
            catch
            {
                return new Response
                {
                    Code = 500,
                    Message = "Internal Server Error : ",
                };
            }


        }

        return BadRequest(new Response
        {
            Code = 404,
            Message = "คุณไม่ได้ส่งอะไรมาแก้ไขเลยนะ",
        });

    }

    // เปลี่ยน IsDeleted ของโปรเจคเป็น true และ กิจกรรมย่อยทั้งหมดเป็น true
   
    [HttpDelete("DeleteCategory", Name = "DeleteCategory")]

    public ActionResult DeleteCategory(int id)
    {
        Category category = Category.DeleteCategory(_db, id);
        return Ok(category);
    }
}
