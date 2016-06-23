using System.Threading.Tasks;
using Abp.Application.Services;
using Fq.Profile.Dtos;

namespace Fq.Profile
{
    public interface IProfileService : IApplicationService
    {
        Task ChangePassword(ChangePasswordInput input);
    }
}
