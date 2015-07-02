using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcCalculator.Helper
{
    public class CookieProcessor
    {
        private HttpContextBase context;

        public CookieProcessor(HttpContextBase context)
        {
            this.context = context;
        }

        public string ReadCookie(string name)
        {
            HttpCookie c = context.Request.Cookies[name];
            if (c == null)
            {
                return null;
            }
            else
            {
                return c.Value;
            }
        }

        public void WriteCookie(string name, string value)
        {
            if (ReadCookie(name) == null)
            {
                context.Response.AppendCookie(new HttpCookie(name, value));
            }
            else
            {
                context.Response.SetCookie(new HttpCookie(name, value));
            }
        }

        public void RemoveCooke(string name)
        {
            HttpCookie cookie = new HttpCookie(name);
            cookie.Expires = DateTime.Now.AddDays(-1);
            context.Response.AppendCookie(cookie);
        }

        public bool CheckCookies()
        {
            double v1;
            double v2;
            return Double.TryParse(ReadCookie("v1"), out v1)
                    && Double.TryParse(ReadCookie("v2"), out v2)
                    && (Enum.Parse(typeof(Operation), ReadCookie("operation")) != null);
        }
    }
}