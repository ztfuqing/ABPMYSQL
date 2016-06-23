using System;
using System.Threading.Tasks;
using Abp;
using Abp.Localization;
using Abp.Notifications;
using Fq.Authorization.Users;

namespace Fq.Notifications
{
    public class AppNotifier : FqDomainServiceBase, IAppNotifier
    {
        private readonly INotificationPublisher _notificationPublisher;

        public AppNotifier(INotificationPublisher notificationPublisher)
        {
            _notificationPublisher = notificationPublisher;
        }

        public async Task WelcomeToTheApplicationAsync(User user)
        {
            await _notificationPublisher.PublishAsync(
                "WelcomeToTheApplication",
                new MessageNotificationData("»¶Ó­"),
                severity: NotificationSeverity.Success,
                userIds: new[] { user.ToUserIdentifier() }
                );
        }

        public async Task NewUserRegisteredAsync(User user)
        {
            var notificationData = new MessageNotificationData("»¶Ó­");

            notificationData["userName"] = user.UserName;
            notificationData["emailAddress"] = user.EmailAddress;

            await _notificationPublisher.PublishAsync("NewUserRegistered", notificationData, tenantIds: new[] { user.TenantId });
        }

        public async Task SendMessageToAllUser(int? tenantId, string title, string text)
        {
            await _notificationPublisher.PublishAsync(
               title,
               new MessageNotificationData(text),
               severity: NotificationSeverity.Success,
               userIds: new[] { new UserIdentifier(null, 1), new UserIdentifier(null, 2), new UserIdentifier(null, 3), new UserIdentifier(null, 4) }
               );
        }
    }
}