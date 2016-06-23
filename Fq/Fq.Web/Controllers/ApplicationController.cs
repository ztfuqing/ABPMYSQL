using System.Web.Mvc;
using Abp.Auditing;
using Abp.Web.Mvc.Authorization;

namespace Fq.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ApplicationController : FqControllerBase
    {
        [DisableAuditing]
        public ActionResult Index()
        {
            return View("~/App/Main/Views/Layout/layout.cshtml");
        }
    }
}