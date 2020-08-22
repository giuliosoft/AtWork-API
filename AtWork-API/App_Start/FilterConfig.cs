using AtWork_API.Models;
using System.Web;
using System.Web.Mvc;

namespace AtWork_API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
