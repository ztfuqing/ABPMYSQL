using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Runtime.Security;
using Abp.Threading;
using Abp.UI;
using Abp.Web.Mvc.Models;
using Fq.Authorization.Roles;
using Fq.Authorization.Users;
using Fq.MultiTenancy;
using Fq.Web.Models.Account;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Fq.Web.Controllers
{
    public class AccountController : FqControllerBase
    {
        private readonly TenantManager _tenantManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public AccountController(
            TenantManager tenantManager,
            UserManager userManager,
            RoleManager roleManager,
            IUnitOfWorkManager unitOfWorkManager,
            IMultiTenancyConfig multiTenancyConfig)
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWorkManager = unitOfWorkManager;
            _multiTenancyConfig = multiTenancyConfig;
        }

        #region 登录
        public ActionResult Login(string userNameOrEmailAddress = "", string returnUrl = "", string successMessage = "")
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Url.Action("Index", "Application");
            }
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;
            AsyncHelper.RunSync(() => _userManager.GetUserByIdAsync(1));
            return View(
                new LoginFormViewModel
                {
                    SuccessMessage = successMessage,
                    UserNameOrEmailAddress = userNameOrEmailAddress
                });
        }

        [HttpPost]
        [DisableAuditing]
        public async Task<JsonResult> Login(LoginViewModel loginModel, string returnUrl = "", string returnUrlHash = "")
        {
            CheckModelState();
            using (var uow = _unitOfWorkManager.Begin())
            {
                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                var loginResult = await GetLoginResultAsync(loginModel.UsernameOrEmailAddress, loginModel.Password, "");
                if (loginResult.User.ShouldChangePasswordOnNextLogin)
                {
                    loginResult.User.SetNewPasswordResetCode();
                    await uow.CompleteAsync();
                    return Json(new MvcAjaxResponse
                    {
                        TargetUrl = Url.Action(
                            "ResetPassword",
                            new ResetPasswordViewModel
                            {
                                UserId = SimpleStringCipher.Instance.Encrypt(loginResult.User.Id.ToString()),
                                ResetCode = loginResult.User.PasswordResetCode
                            })
                    });
                }


                await SignInAsync(loginResult.User, loginResult.Identity, loginModel.RememberMe);

                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    returnUrl = Request.ApplicationPath;
                }

                if (!string.IsNullOrWhiteSpace(returnUrlHash))
                {
                    returnUrl = returnUrl + returnUrlHash;
                }
            }

            return Json(new MvcAjaxResponse { TargetUrl = returnUrl });
        }

        private async Task<AbpUserManager<Tenant, Role, User>.AbpLoginResult> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _userManager.LoginAsync(usernameOrEmailAddress, password);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        private async Task SignInAsync(User user, ClaimsIdentity identity = null, bool rememberMe = false)
        {
            if (identity == null)
            {
                identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            }

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = rememberMe }, identity);
        }

        private Exception CreateExceptionForFailedLoginAttempt(AbpLoginResultType result, string usernameOrEmailAddress, string tenancyName)
        {
            switch (result)
            {
                case AbpLoginResultType.Success:
                    return new ApplicationException("Don't call this method with a success result!");
                case AbpLoginResultType.InvalidUserNameOrEmailAddress:
                    return new UserFriendlyException("登录失败", "用户不存在");
                case AbpLoginResultType.InvalidPassword:
                    return new UserFriendlyException("登录失败", "密码错误");
                case AbpLoginResultType.InvalidTenancyName:
                    return new UserFriendlyException("登录失败", "租户不存在");
                case AbpLoginResultType.TenantIsNotActive:
                    return new UserFriendlyException("登录失败", "租户已被锁定");
                case AbpLoginResultType.UserIsNotActive:
                    return new UserFriendlyException("登录失败", "用户未激活");
                case AbpLoginResultType.UserEmailIsNotConfirmed:
                    return new UserFriendlyException(L("LoginFailed"), "Your email address is not confirmed. You can not login"); //TODO: localize message
                default: //Can not fall to default actually. But other result types can be added in the future and we may forget to handle it
                    Logger.Warn("Unhandled login fail reason: " + result);
                    return new UserFriendlyException("登录失败");
            }
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }
        #endregion

        #region 忘记密码

        [UnitOfWork]
        public virtual async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            CheckModelState();

            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);

            var userId = Convert.ToInt64(SimpleStringCipher.Instance.Decrypt(model.UserId));

            var user = await _userManager.GetUserByIdAsync(userId);
            if (user == null || user.PasswordResetCode.IsNullOrEmpty() || user.PasswordResetCode != model.ResetCode)
            {
                throw new UserFriendlyException("非法操作", "密码重置码错误");
            }

            return View(model);
        }

        [HttpPost]
        [UnitOfWork]
        public virtual async Task<ActionResult> ResetPassword(ResetPasswordFormViewModel model)
        {
            CheckModelState();

            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);

            var userId = Convert.ToInt64(SimpleStringCipher.Instance.Decrypt(model.UserId));

            var user = await _userManager.GetUserByIdAsync(userId);

            if (user == null || user.PasswordResetCode.IsNullOrEmpty() || user.PasswordResetCode != model.ResetCode)
            {
                throw new UserFriendlyException("非法操作", "密码重置码错误");
            }

            user.Password = new PasswordHasher().HashPassword(model.Password);
            user.PasswordResetCode = null;
            user.IsEmailConfirmed = true;
            user.ShouldChangePasswordOnNextLogin = false;

            await _userManager.UpdateAsync(user);

            if (user.IsActive)
            {
                await SignInAsync(user);
            }

            return RedirectToAction("Index", "Application");
        }
        #endregion
    }
}