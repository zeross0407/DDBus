using DDBus.Entity;
using DDBus.Service;
using DDBus.Services;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Generators;


namespace DDBus.Controllers
{
    public class AuthController : Controller
    {
        private readonly TokenService _Token_Service;
        private readonly CRUD_Service<Account> _Account_Service;
        public AuthController(

            CRUD_Service<Account> Account_Service,
            TokenService Token_Service
            )
        {
            _Account_Service = Account_Service;
            _Token_Service = Token_Service;
        }


        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginModel login)
        {
            Account ac = await _Account_Service.Get_One_Item_Async(login.Email, "Email");

            if (ac != null && BCrypt.Net.BCrypt.Verify(login.Password, ac.Password) && ac.active)
            {
                var token = await _Token_Service.GenerateToken(ac.Id!);

                return Ok(new
                {
                    token,
                    ac.role
                });
            }
            return Unauthorized("Thông tin tài khoản không hợp lệ");
        }

        public class LoginModel
        {
            public required string Email { get; set; }
            public required string Password { get; set; }
        }



    }
}
