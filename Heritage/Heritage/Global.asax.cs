using Heritage.Models;
using Heritage.Models.ContextoBanco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Heritage
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            if (System.Web.Security.Roles.GetAllRoles().Length == 0)
            {
                using (IContextoDados Contexto = new ContextoDadosNH())
                {
                    Contexto.Setup();
                    
                    System.Web.Security.Roles.CreateRole("Administrador");
                    System.Web.Security.Roles.CreateRole("Contabil");
                    MembershipCreateStatus status;
                    Membership.CreateUser("Ademi", "Ademi100787", "ademivieria@gmail.com", null, null, true, out status);
                    if (status == MembershipCreateStatus.Success)
                    {
                        Papel PapelAdministrador = new Papel();
                        PapelAdministrador.Nome = "Administrador";
                        Contexto.Add<Papel>(PapelAdministrador);
                        Contexto.SaveChanges();

                        Roles.AddUserToRole("Ademi", "Administrador");                       
                        Contexto.SaveChanges();

                        Papel PapelContabil = new Papel();
                        PapelContabil.Nome = "Contabil";
                        Contexto.Add<Papel>(PapelContabil);
                        Contexto.SaveChanges();                        

                        Contexto.Dispose();
                    }

                }
            }



        }
    }
}