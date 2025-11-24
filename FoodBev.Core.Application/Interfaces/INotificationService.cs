using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodBev.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for managing user notifications.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Retrieves notifications for a user.
        /// </summary>
        Task<IEnumerable<NotificationDto>> GetNotificationsAsync(string userId, bool includeRead = false);

        /// <summary>
        /// Marks a notification as read.
        /// </summary>
        Task MarkAsReadAsync(string userId, int notificationId);

        /// <summary>
        /// Creates a new notification.
        /// </summary>
        Task CreateNotificationAsync(string userId, string message, string type = "Info");
    }

    /// <summary>
    /// DTO for notification data.
    /// </summary>
    public class NotificationDto
    {
        public int NotificationID { get; set; }
        public string UserID { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public bool IsRead { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}

