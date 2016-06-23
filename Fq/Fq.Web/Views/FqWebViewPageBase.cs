using Abp.Web.Mvc.Views;

namespace Fq.Web.Views
{
    public abstract class FqWebViewPageBase : FqWebViewPageBase<dynamic>
    {

    }

    public abstract class FqWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected FqWebViewPageBase()
        {
            LocalizationSourceName = FqConsts.LocalizationSourceName;
        }
    }
}