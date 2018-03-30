using System;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Infrastructure;

namespace Mtwx.Web.Security
{
    public class SystemWebCookieManager : ICookieManager
    {
        public void AppendResponseCookie(IOwinContext context, string key, string value, CookieOptions options)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var webContext = context.Get<HttpContextBase>(typeof(HttpContextBase).FullName);

            var domainHasValue = !string.IsNullOrEmpty(options.Domain);
            var pathHasValue = !string.IsNullOrEmpty(options.Path);
            var expiresHasValue = options.Expires.HasValue;

            var cookie = new HttpCookie(key, value);
            if (domainHasValue)
            {
                cookie.Domain = options.Domain;
            }
            if (pathHasValue)
            {
                cookie.Path = options.Path;
            }
            if (expiresHasValue)
            {
                cookie.Expires = options.Expires.Value;
            }
            if (options.Secure)
            {
                cookie.Secure = true;
            }
            if (options.HttpOnly)
            {
                cookie.HttpOnly = true;
            }

            webContext.Response.AppendCookie(cookie);
        }

        public void DeleteCookie(IOwinContext context, string key, CookieOptions options)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            AppendResponseCookie(context, key, string.Empty, new CookieOptions
            {
                Path = options.Path,
                Domain = options.Domain,
                Expires = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            });
        }

        public string GetRequestCookie(IOwinContext context, string key)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var webContext = context.Get<HttpContextBase>(typeof(HttpContextBase).FullName);
            var cookie = webContext.Request.Cookies[key];
            return cookie?.Value;
        }
    }
}