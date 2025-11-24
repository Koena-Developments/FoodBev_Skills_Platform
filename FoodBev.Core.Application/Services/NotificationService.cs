using FoodBev.Application.Interfaces;
using FoodBev.Core.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodBev.Application.Services
{
    /// <summary>
    /// Implements notification service. For now, uses in-memory storage.
    /// In production, this should use a database table for notifications.
    /// </summary>
    public class NotificationService : INotificationService
    {
        // In-memory storage for notifications (replace with database in production)
        private static readonly Dictionary<string, List<NotificationDto>> _notifications = new();

        public Task<IEnumerable<NotificationDto>> GetNotificationsAsync(string userId, bool includeRead = false)
        {
            if (!_notifications.ContainsKey(userId))
                return Task.FromResult(Enumerable.Empty<NotificationDto>());

            var userNotifications = _notifications[userId];
            var filtered = includeRead 
                ? userNotifications 
                : userNotifications.Where(n => !n.IsRead);

            return Task.FromResult(filtered.OrderByDescending(n => n.CreatedAt).AsEnumerable());
        }

        public Task MarkAsReadAsync(string userId, int notificationId)
        {
            if (_notifications.ContainsKey(userId))
            {
                var notification = _notifications[userId].FirstOrDefault(n => n.NotificationID == notificationId);
                if (notification != null)
                {
                    notification.IsRead = true;
                }
            }

            return Task.CompletedTask;
        }

        public Task CreateNotificationAsync(string userId, string message, string type = "Info")
        {
            if (!_notifications.ContainsKey(userId))
                _notifications[userId] = new List<NotificationDto>();

            var notification = new NotificationDto
            {
                NotificationID = _notifications[userId].Count + 1,
                UserID = userId,
                Message = message,
                Type = type,
                IsRead = false,
                CreatedAt = System.DateTime.UtcNow
            };

            _notifications[userId].Add(notification);
            return Task.CompletedTask;
        }
    }
}

