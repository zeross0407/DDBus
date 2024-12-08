using DDBus.Entity;
using DDBus.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDBus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly CRUD_Service<Notifications> _notificationService;

        public NotificationController(CRUD_Service<Notifications> notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotifications()
        {
            var notifications = await _notificationService.GetAllAsync();
            return Ok(notifications);
        }


        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetNotificationById(string id)
        {
            var notification = await _notificationService.GetByIdAsync(id);

            if (notification == null)
            {
                return NotFound("Không tìm thấy thông báo");
            }

            return Ok(notification);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] Notifications newNotification)
        {
            if (newNotification == null)
            {
                return BadRequest("Dữ liệu thông báo không hợp lệ");
            }

            await _notificationService.AddAsync(newNotification);
            return Ok();
        }

        [Authorize]
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateNotification(string id, [FromBody] Notifications updatedNotification)
        {
            var existingNotification = await _notificationService.GetByIdAsync(id);
            if (existingNotification == null)
            {
                return NotFound(new { Message = "Không tìm thấy thông báo" });
            }

            await _notificationService.UpdateAsync(id, updatedNotification);
            return Ok();
        }

        [Authorize]
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteNotification(string id)
        {
            var existingNotification = await _notificationService.GetByIdAsync(id);
            if (existingNotification == null)
            {
                return NotFound("Không tìm thấy thông báo");
            }

            await _notificationService.DeleteAsync(id);
            return Ok();
        }
    }
}
