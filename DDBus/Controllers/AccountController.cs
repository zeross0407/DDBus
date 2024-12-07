using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DDBus.Entity;
using DDBus.Services;
using DDBus.Service;
using System.Text.RegularExpressions;
using System.Security.Claims;

namespace Reflectly.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly CRUD_Service<Account> _Account_Service;
        public AccountController(
            IConfiguration configuration,
            CRUD_Service<Account> Account_Service)
        {
            _configuration = configuration;
            _Account_Service = Account_Service;
        }

        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _Account_Service.GetAllAsync());
        }

        //[Authorize]
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetAccountById(string id)
        {
            var account = await _Account_Service.GetByIdAsync(id);

            if (account == null)
            {
                return NotFound(new { Message = "Không tìm thấy tài khoản" });
            }
            if (account.role == 1) return BadRequest("Không được xóa tài khoản root");

            return Ok(account);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] Account newAccount)
        {
            if (newAccount == null)
            {
                return BadRequest("Dữ liệu tài khoản không hợp lệ");
            }

            if (!Regex.IsMatch(newAccount.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$") || newAccount.Password.Length < 8)
                return BadRequest("Tài khoản không hợp lệ");

            var existingUser = await _Account_Service.Get_One_Item_Async(newAccount.Email, "Email");
            if (existingUser != null)
            {
                return BadRequest("Email đã tồn tại");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newAccount.Password);

            // Tạo đối tượng Account
            var accountToCreate = new Account
            {
                Username = newAccount.Username,
                Email = newAccount.Email,
                Password = hashedPassword,
                role = newAccount.role,
                active = true,
            };

            await _Account_Service.AddAsync(accountToCreate);

            return Ok();
        }

        //[Authorize]
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateAccount(string id, [FromBody] Account updatedAccount)
        {
            var existingAccount = await _Account_Service.GetByIdAsync(id);
            if (existingAccount == null)
            {
                return NotFound(new { Message = "Không tìm thấy tài khoản" });
            }

            await _Account_Service.UpdateAsync(id, updatedAccount);
            return Ok();
        }

        //[Authorize]
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var existingAccount = await _Account_Service.GetByIdAsync(id);
            if (existingAccount == null)
            {
                return NotFound(new { Message = "Không tìm thấy tài khoản" });
            }
            await _Account_Service.DeleteAsync(id);
            return Ok();
        }

        private async Task<bool> is_root(string userid)
        {
            Account ac = await _Account_Service.GetByIdAsync(userid);
            if (ac != null && ac.role == 0) return true;
            return false;
        }
    }
}
