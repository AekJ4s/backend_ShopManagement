


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
[Route("users")]

public class UserController : ControllerBase
{

    private const string TokenSecret = "welcometojwtsigninthiskeycanchanginappdotjson";

    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(1);
    private DatabaseContext _db = new DatabaseContext();
    private readonly ILogger<UserController> _logger;
    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }


    // GEN TOKEN
    [HttpPost("token")]
    public string GenerateToken([FromBody] User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(TokenSecret);
        if (user.Username != null)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name , user.Username),

        };


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TokenLifetime),
                Issuer = "http://localhost:5157",
                Audience = "http://localhost:5157",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            if (token != null)
            {
                var jwt = tokenHandler.WriteToken(token);
                return jwt;
            }
            else
            {
                return "Failed to create token.";
            }
        }
        return "Failed to create token.";
    }

    [HttpPost("login")]

    public ActionResult<Response> Login([FromBody] UserLogin requestUser)
    {
        User? user = _db.Users.FirstOrDefault(x => x.Username == requestUser.Username && x.IsDeleted == false);
        string bearerToken = "";
        if (user == null)
        {
            return NotFound(new Response
            {
                Code = 404,
                Message = "NOT FOUND USER",
                Data = null,
            }
                );
        };
        try
        {
            if (user.Username == requestUser.Username && user.Password == requestUser.Password)
            {
                bearerToken = GenerateToken(user);
            }
            else
            {
                return BadRequest(new Response
                {
                    Code = 400,
                    Message = "user password or pin is wrong or all are wrong",
                    Data = user
                });
            }
        }
        catch
        {
            return StatusCode(500);
        }
        return Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = new Dictionary<string, object>
    {
        { "BearerToken", bearerToken },
        { "UserId", user.Id },
        { "UserName", user.Username }
    }
        });

    }

    [HttpGet("ShowAllUsers", Name = "ShowAllUsers")]

    public ActionResult GetAll()
    {
        // .OrderBy(q => q.Salary) เรียงจากน้อยไปมาก
        // .OrderByDescending(q => q.Salary) เรียงจากมากไปน้อย
        List<User> users = backend_ShopManagement.Models.User.GetAllUser(_db);
        return Ok(users);
    }


    [HttpPost("CreateUser", Name = "CreateUser")]

    public ActionResult<Response> CreateUser([FromBody] UserCreate userCreate)
    {
        if (userCreate.Username != null && userCreate.Password != null )
        {
           
                User user = new User
                {
                    Username = userCreate.Username.ToLower(),
                    Password = userCreate.Password.ToLower(),
                    Firstname = userCreate.Firstname,
                    Lastname = userCreate.Lastname,
                };
                user.RoleId = 2;
                user.CreateDate = DateTime.Now;
                user.UpdateDate = DateTime.Now;
                string Message = backend_ShopManagement.Models.User.CreateUser(_db, user);
                return new Response
                {
                    Code = 200,
                    Message = Message,
                    Data = user,
                };
            
        }
        return BadRequest(new Response
        {
            Code = 404,
            Message = "กรอกข้อมูลให้ครบก่อน ****",
        });


    }



    [HttpPut("EditUser", Name = "EditUser")]

    public ActionResult<Response> EditUser([FromForm] UserEditer NewData)
    {
        var CanUseUser = _db.Users.Where(c => c.Username == NewData.Username && c.IsDeleted != true).Count();
        User UserData = _db.Users.Where(e => e.Id == NewData.Id && e.IsDeleted != true).AsNoTracking().ToList().First();

        NewData.Username = (NewData.Username?.ToLower() == "string" || NewData.Username == UserData.Username || NewData.Username == null) ? null : NewData.Username;
        NewData.Firstname = (NewData.Firstname?.ToLower() == "string" || NewData.Firstname == UserData.Firstname || NewData.Firstname == null) ? null : NewData.Firstname;
        NewData.Lastname = (NewData.Lastname?.ToLower() == "string" || NewData.Lastname == UserData.Lastname || NewData.Lastname == null) ? null : NewData.Lastname;
        NewData.RoleId = (NewData.RoleId == 0 || NewData.RoleId == UserData.RoleId || NewData.RoleId == null) ? null : NewData.RoleId;
        

        if(NewData.Username == null && NewData.Firstname == null && NewData.Lastname == null && NewData.RoleId == null){
            NewData = null;
        }
        
        if (NewData != null)
        {
            if (NewData.Password == UserData.Password && UserData.IsDeleted != true)
            {
                UserData.Username = (NewData.Username?.ToLower() == "string" || NewData.Username == UserData.Username || NewData.Username == null) ? UserData.Username : NewData.Username;
                UserData.Firstname = (NewData.Firstname?.ToLower() == "string" || NewData.Firstname == UserData.Firstname || NewData.Firstname == null) ? UserData.Firstname : NewData.Firstname;
                UserData.Lastname = (NewData.Lastname?.ToLower() == "string" || NewData.Lastname == UserData.Lastname || NewData.Lastname == null) ? UserData.Lastname : NewData.Lastname;
                UserData.Password = (NewData.NewPassword == null || NewData.NewPassword.ToLower() == "string") ? UserData.Password : NewData.NewPassword;
                UserData.RoleId = (NewData.RoleId == null || NewData.RoleId == 0)? UserData.RoleId : NewData.RoleId;

                backend_ShopManagement.Models.User.EditPassword(_db, UserData);
                _db.SaveChanges();
                return new Response
                {
                    Code = 200,
                    Message = "User has been updated",
                };
            }
            return BadRequest(new Response
            {
                Code = 404,
                Message = "รหัสผ่านเก่าไม่ถูกต้อง",
            });
        }
        else
            UserData.UpdateDate = DateTime.Now;

            return new Response
            {
                Code = 400,
                Message = "คุณไม่ได้ส่งข้อมูลอะไรมาให้เราอัพเดตเลย",
                Data = null
            };
  
    }




    [HttpDelete("DeleteUser", Name = "DeleteUser")]

    public ActionResult DeleteUser(int id)
    {
        User user = backend_ShopManagement.Models.User.Delete(_db, id);
        return Ok(user);
    }


    [HttpDelete("BanUser", Name = "BanUser")]

    public ActionResult Banned(int id)
    {
        User user = backend_ShopManagement.Models.User.Banned(_db, id);
        return Ok(user);
    }

    [HttpGet("FindUser/{id}", Name = "User")]

    public ActionResult GetUserById(int id)
    {
        User user = backend_ShopManagement.Models.User.GetById(_db, id);
        return Ok(user);
    }


}
