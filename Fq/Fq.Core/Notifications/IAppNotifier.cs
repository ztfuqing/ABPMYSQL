using System.Threading.Tasks;
using Fq.Authorization.Users;

namespace Fq.Notifications
{
    public interface IAppNotifier
    {
        Task WelcomeToTheApplicationAsync(User user);

        Task NewUserRegisteredAsync(User user);

        Task SendMessageToAllUser(int? tenantId, string title, string text);
    }
}