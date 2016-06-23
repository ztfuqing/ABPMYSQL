using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Fq.Authorization.Users.Dto;

namespace Fq.Authorization.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task UpdateUserPermissions(UpdateUserPermissionsInput input);

        Task RemoveFromRole(long userId, string roleName);

        Task<PagedResultOutput<UserListDto>> GetUsers(GetUsersInput input);

        Task<GetUserForEditOutput> GetUserForEdit(NullableIdInput<long> input);

        Task CreateOrUpdateUser(CreateOrUpdateUserInput input);

        Task DeleteUser(IdInput<long> input);
    }
}