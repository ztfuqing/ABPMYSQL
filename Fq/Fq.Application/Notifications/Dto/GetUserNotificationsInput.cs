using Abp.Notifications;
using Fq.Dto;

namespace Fq.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }
    }
}