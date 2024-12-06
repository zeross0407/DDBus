using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DDBus.Entity;
using DDBus.Services;





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


        [HttpGet]
        public async Task<List<Account>> Get() => await _Account_Service.GetAllAsync();



        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetAccountById(string id)
        {
            var account = await _Account_Service.GetByIdAsync(id);

            if (account == null)
            {
                return NotFound(new { Message = "Account not found" });
            }

            return Ok(account);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] Account newAccount)
        {
            if (newAccount == null)
            {
                return BadRequest("Invalid account data");
            }

            var accountId = await _Account_Service.AddAsync(newAccount);
            return CreatedAtAction(nameof(GetAccountById), new { id = accountId }, newAccount);
        }


        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateAccount(string id, [FromBody] Account updatedAccount)
        {
            var existingAccount = await _Account_Service.GetByIdAsync(id);
            if (existingAccount == null)
            {
                return NotFound(new { Message = "Account not found" });
            }

            await _Account_Service.UpdateAsync(id, updatedAccount);
            return NoContent();
        }


        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var existingAccount = await _Account_Service.GetByIdAsync(id);
            if (existingAccount == null)
            {
                return NotFound(new { Message = "Account not found" });
            }

            await _Account_Service.DeleteAsync(id);
            return NoContent();
        }






    }




}
