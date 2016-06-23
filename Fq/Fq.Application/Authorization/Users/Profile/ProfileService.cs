using System.Threading.Tasks;
using Fq.Profile.Dtos;

namespace Fq.Profile
{
    public class ProfileService : FqAppServiceBase, IProfileService
    {
        public ProfileService()
        {

        }
        public async Task ChangePassword(ChangePasswordInput input)
        {
            var user = await GetCurrentUserAsync();
            CheckErrors(await UserManager.ChangePasswordAsync(user.Id, input.CurrentPassword, input.NewPassword));
        }
    }
}
