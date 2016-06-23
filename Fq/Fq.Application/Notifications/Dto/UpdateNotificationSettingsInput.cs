using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace Fq.Notifications.Dto
{
    public class UpdateNotificationSettingsInput : IInputDto
    {
        public bool ReceiveNotifications { get; set; }

        public List<NotificationSubscriptionDto> Notifications { get; set; }
    }
}