using System.Web;
using System.Web.Mvc;

namespace PassionProjectN01661067
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
