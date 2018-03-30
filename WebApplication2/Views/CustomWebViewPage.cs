using System.Web.Mvc;
using Mtwx.Web.Domain;

namespace Mtwx.Web.Views
{
    public class CustomWebViewPage : WebViewPage
    {
        public AppUserState CurrentAppUserState { get; set; }


        public override void InitHelpers()
        {
            base.InitHelpers();
            CurrentAppUserState = new AppUserState(ViewData["UserState"].ToString());
        }

        public override void Execute()
        {
        
        }
    }

    public class CustomWebViewPage<T> : WebViewPage<T>
    {
        public AppUserState CurrentAppUserState { get; set; }

        public override void InitHelpers()
        {
            base.InitHelpers();
            CurrentAppUserState = new AppUserState(ViewData["UserState"]?.ToString());
        }

        public override void Execute()
        {
        }
    }
}