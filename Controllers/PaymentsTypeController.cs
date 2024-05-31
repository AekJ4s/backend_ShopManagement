


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
[Route("paymentsType")]

public class PaymentsTypeController : ControllerBase
{

    private const string TokenSecret = "welcometojwtsigninthiskeycanchanginappdotjson";

    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(1);
    private DatabaseContext _db = new DatabaseContext();
    private readonly ILogger<PaymentsTypeController> _logger;
    public PaymentsTypeController(ILogger<PaymentsTypeController> logger)
    {
        _logger = logger;
    }

    [HttpGet("ShowAllPaymentsType", Name = "ShowAllPaymentsType")]

    public ActionResult ShowAllPaymentsType()
    {
        // .OrderBy(q => q.Salary) เรียงจากน้อยไปมาก
        // .OrderByDescending(q => q.Salary) เรียงจากมากไปน้อย
        List<PaymentsType> types = PaymentsType.GetAllPaymentsType(_db);
        return Ok(types);
    }


    [HttpPost("CreatePaymentsType", Name = "CreatePaymentsType")]

    public ActionResult<Response> CreatePaymentsType([FromBody] TypeCreate Request)
    {
        var CanUseType = _db.PaymentsTypes.Where(r => r.Name == Request.Name && r.IsDeleted != true).Count();
        var CanUseBank = _db.PaymentsTypes.Where(r => r.AccountNumber == Request.AccountNumber && r.AccountNumber != "0" && r.AccountNumber != "0" && r.IsDeleted != true).Count();
        if (Request.Name != null && CanUseType < 1 && CanUseBank < 1)
        {
            if (Request.AccountNumber.Length == 10 || Request.AccountNumber.Length == 1)
            {
                PaymentsType paymentsType = new PaymentsType
                {
                    Name = Request.Name.ToLower(),
                    AccountNumber = (Request.AccountNumber == null || Request.AccountNumber == "0" || Request.AccountNumber == "string") ? "0" : Request.AccountNumber
                };

                string Message = PaymentsType.CreateType(_db, paymentsType);
                return new Response
                {
                    Code = 200,
                    Message = Message,
                    Data = paymentsType,
                };
            }
            return BadRequest(new Response
            {
                Code = 400,
                Message = "เลขบัญชี ต้องมีแค่ 1 (ในกรณีไม่ระบุ) ตัวหรือมีแค่ 10 ตัวเท่านั้น",
                Data = null,
            });


        }
        return BadRequest(new Response
        {
            Code = 404,
            Message = "ชื่อ หรือว่า เลขบัญชี ซ้ำ",
        });


    }

    [HttpPut("EditpaymentsType", Name = "EditpaymentsType")]

    public ActionResult<Response> EditpaymentsType([FromBody] TypeEditer Request)
    {

        PaymentsType paymentsTypeData = _db.PaymentsTypes.Where(e => e.Id == Request.Id && e.IsDeleted != true).AsNoTracking().ToList().First();
        if (Request.AccountNumber.Length != 1 || Request.AccountNumber.Length != 10)
        {
            Request.AccountNumber = "0";
        }
        Request.Name = (Request.Name?.ToLower() == "string" || Request.Name == paymentsTypeData.Name || Request.Name == null) ? null : Request.Name;
        Request.AccountNumber = (Request.AccountNumber == paymentsTypeData.AccountNumber || Request.AccountNumber == null) ? null : Request.AccountNumber;

        if (Request.Name == null && Request.AccountNumber == null)
        {
            Request = null;
        }


        if (paymentsTypeData != null && Request != null)
        {
            var CanUseType = _db.PaymentsTypes.Where(r => r.Name == Request.Name && r.IsDeleted != true).Count();
            var CanUseBank = _db.PaymentsTypes.Where(r => r.AccountNumber == Request.AccountNumber && r.AccountNumber != "0" && r.IsDeleted != true).Count();
            if (Request.Id == paymentsTypeData.Id && CanUseBank < 1 && CanUseType < 1)
            {
                if (Request.AccountNumber.Length != 1 && Request.AccountNumber.Length != 10 )
                {
                    return BadRequest(new Response
                    {
                        Code = 400,
                        Message = "เลขบัญชี ต้องมีแค่ 1 (ในกรณีไม่ระบุ) ตัวหรือมีแค่ 10 ตัวเท่านั้น",
                        Data = null,
                    });
                }
                paymentsTypeData.Name = (Request.Name == null || Request.Name == "string") ? paymentsTypeData.Name : Request.Name;
                paymentsTypeData.AccountNumber = (Request.AccountNumber == null || Request.AccountNumber.Length == 1 || Request.AccountNumber == "string") ? "0" : Request.AccountNumber;
                PaymentsType.EditType(_db, paymentsTypeData);
                _db.SaveChanges();
                return new Response
                {
                    Code = 200,
                    Message = "ประเภทของการชำระเงินถูกอัพเดตแล้ว",
                };


            }
            return BadRequest(new Response
            {
                Code = 404,
                Message = "ไม่เจอประเภทของการชำระเงิน",
            });
        }
        else
            return new Response
            {
                Code = 400,
                Message = "ไม่เจอประเภทของการชำระเงินที่ส่งเข้ามา หรือ คุณไม่ได้ทำการแก้ไขอะไรเลย",
                Data = null
            };
    }




    [HttpDelete("DeletepaymentsType", Name = "DeletepaymentsType")]

    public ActionResult DeletepaymentsType(int id)
    {
        PaymentsType paymentsType = PaymentsType.DeletePaymentsType(_db, id);
        return Ok(paymentsType);
    }


    [HttpGet("FindpaymentsType/{id}", Name = "FindpaymentsType")]

    public ActionResult GetpaymentsTypeById(int id)
    {
        PaymentsType paymentsType = PaymentsType.GetPaymentsTypeId(_db, id);
        return Ok(paymentsType);
    }


}
