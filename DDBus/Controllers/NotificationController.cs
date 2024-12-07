using DDBus.Entity;
using DDBus.Services;
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

        // Lấy danh sách tất cả thông báo
        [HttpGet]
        public async Task<IActionResult> GetAllNotifications()
        {
            var notifications = await _notificationService.GetAllAsync();
            return Ok(notifications);
        }

        // Lấy thông báo theo ID
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetNotificationById(string id)
        {
            var notification = await _notificationService.GetByIdAsync(id);

            if (notification == null)
            {
                return NotFound(new { Message = "Notification not found" });
            }

            return Ok(notification);
        }

        // Tạo một thông báo mới
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] Notifications newNotification)
        {
            if (newNotification == null)
            {
                return BadRequest("Invalid notification data");
            }

            var notificationId = await _notificationService.AddAsync(newNotification);
            return CreatedAtAction(nameof(GetNotificationById), new { id = notificationId }, newNotification);
        }

        // Cập nhật thông báo
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateNotification(string id, [FromBody] Notifications updatedNotification)
        {
            var existingNotification = await _notificationService.GetByIdAsync(id);
            if (existingNotification == null)
            {
                return NotFound(new { Message = "Notification not found" });
            }

            await _notificationService.UpdateAsync(id, updatedNotification);
            return NoContent();
        }

        // Xóa thông báo
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteNotification(string id)
        {
            var existingNotification = await _notificationService.GetByIdAsync(id);
            if (existingNotification == null)
            {
                return NotFound(new { Message = "Notification not found" });
            }

            await _notificationService.DeleteAsync(id);
            return NoContent();
        }
    }
}
