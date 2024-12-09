namespace DDBus.Service
{

    using Microsoft.Extensions.Hosting;
    using DDBus.Entity;
    using DDBus.Services;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class CleanService : BackgroundService
    {
        private Timer? _timer;
        private readonly CRUD_Service<Notifications> _notificationService;

        public CleanService(CRUD_Service<Notifications> notificationService)
        {
            _notificationService = notificationService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var now = DateTime.Now;
            var scheduledTime = DateTime.Now.AddSeconds(20);
            if (now > scheduledTime)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }

            var initialDelay = scheduledTime - now;

            _timer = new Timer(DoWork, null, initialDelay, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            try
            {
                List<Notifications> notifications = (await _notificationService.GetAllAsync()).Where(
                    (notif) => (notif.expire_at <= DateTime.Now) ).ToList();
                foreach (var notif in notifications)
                {
                    await _notificationService.DeleteAsync(notif.Id!);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Task failed: {ex.Message}");
            }
        }


        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }
    }

}
