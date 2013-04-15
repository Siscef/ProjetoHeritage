using System.Web.Mvc;

namespace Heritage.Areas.Contabil
{
    public class ContabilAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Contabil";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Contabil_default",
                "Contabil/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "Heritage.Areas.Contabil.Controllers" }
            );
        }
    }
}
