using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.Authorization;
using Abp.MultiTenancy;
using Abp.Threading;
using Abp.UI;
using Abp.Web.Mvc.Authorization;
using Fq.Authorization;
using Fq.Authorization.Users;
using Fq.Authorization.Users.Dto;
using Fq.Game;

namespace Fq.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : FqControllerBase
    {
        private readonly ITransitionService _transitionService;
        private readonly IUserAppService _userService;
        public HomeController(ITransitionService transitionService, IUserAppService userService)
        {
            _transitionService = transitionService;
            _userService = userService;
        }
        public ActionResult Index()
        {
            var status = _transitionService.GetTransitionStatus();
            if (status == TransitionStatus.Running)
            {
                return View("~/Front/Main/layout/index.cshtml");
            }
            else if (status == TransitionStatus.Success)
            {
                return RedirectToAction("JieBan");
            }
            else
            {
                throw new UserFriendlyException("不允许");
            }
        }

        public ActionResult Admin()
        {
            //var permissions = PermissionFinder
            //    .GetAllPermissions(new FqAuthorizationProvider())
            //    .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Host))
            //    .Select(a => a.Name)
            //    .ToList();

            //UpdateUserPermissionsInput input = new UpdateUserPermissionsInput()
            //{
            //    Id = AbpSession.UserId.Value,
            //    GrantedPermissionNames = permissions
            //};
            //_userService.UpdateUserPermissions(input);

            return View("~/App/Main/Views/Layout/layout.cshtml");
        }

        public ActionResult JieBan()
        {
            var status = _transitionService.GetTransitionStatus();
            if (status == TransitionStatus.Success)
            {
                var dto = AsyncHelper.RunSync(() => _transitionService.GetPrevTransition());

                return View(dto);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult JieBan(long Id)
        {
            AsyncHelper.RunSync(() => _transitionService.JieBan(Id));

            return RedirectToAction("Index");
        }
    }
}