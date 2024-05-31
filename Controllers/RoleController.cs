


using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend_ShopManagement.Data;
using backend_ShopManagement.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace backend_ShopManagement.Controllers;

[ApiController]
[Route("role")]

public class UserRoleController : ControllerBase
{

    private const string TokenSecret = "welcometojwtsigninthiskeycanchanginappdotjson";

    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(1);
    private DatabaseContext _db = new DatabaseContext();
    private readonly ILogger<UserController> _logger;
    public UserRoleController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    [HttpGet("ShowAllRole", Name = "ShowAllRole")]

    public ActionResult GetAllRole()
    {
        // .OrderBy(q => q.Salary) เรียงจากน้อยไปมาก
        // .OrderByDescending(q => q.Salary) เรียงจากมากไปน้อย
        List<UserRole> roles = UserRole.GetAllRole(_db);
        return Ok(roles);
    }


    [HttpPost("CreateRole", Name = "CreateRole")]

    public ActionResult<Response> CreateRole([FromBody] RoleCreate roleCreate)
    {
        var CanUseRole = _db.UserRoles.Where(r => r.Name == roleCreate.Name && r.IsDeleted != true).Count();
        if (roleCreate.Name != null && CanUseRole < 1)
        {
           
                UserRole role = new UserRole
                {
                    Name = roleCreate.Name.ToLower(),
                    Description = (roleCreate.Description == null || roleCreate.Description.ToLower() == "string")? "" : roleCreate.Description
                };
                role.CreateDate = DateTime.Now;
                role.UpdateDate = DateTime.Now;
                string Message = UserRole.CreateRole(_db, role);
                return new Response
                {
                    Code = 200,
                    Message = Message,
                    Data = role,
                };
            
        }
        return BadRequest(new Response
        {
            Code = 404,
            Message = "กรอกข้อมูลไม่ครบ หรือว่า ชื่อตำแหน่ง ซ้ำ",
        });


    }

    [HttpPut("EditRole", Name = "EditRole")]

    public ActionResult<Response> EditRole([FromBody] RoleEditer Request)
    {
        UserRole RoleData = _db.UserRoles.Where(e => e.Id == Request.Id && e.IsDeleted != true).AsNoTracking().ToList().First();

        if (Request != null && Request.Name != RoleData.Name )
        {
            if (Request.Id == RoleData.Id)
            {

                RoleData.Name = (Request.Name == null || Request.Name == "string") ? RoleData.Name : Request.Name;
                RoleData.Description = (Request.Description == null || Request.Description == "string") ? RoleData.Description : Request.Description;

                UserRole.EditRole(_db, RoleData);
                _db.SaveChanges();
                return new Response
                {
                    Code = 200,
                    Message = "User Role Has Changed",
                };
            }
            return BadRequest(new Response
            {
                Code = 404,
                Message = "Role is not found",
            });
        }
        else
            return new Response
            {
                Code = 400,
                Message = "Bad Request",
                Data = null
            };
    }




    [HttpDelete("DeleteRole", Name = "DeleteRole")]

    public ActionResult DeleteRole(int id)
    {
        UserRole role = UserRole.DeleteRole(_db, id);
        return Ok(role);
    }


    [HttpGet("FindRole/{id}", Name = "FindRole")]

    public ActionResult GetRoleById(int id)
    {
        UserRole role = UserRole.GetRoleId(_db, id);
        return Ok(role);
    }


}
