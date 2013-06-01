using Heritage.Models;
using Heritage.Models.ContextoBanco;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Heritage
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Account", action = "LogOn", id = UrlParameter.Optional },
                new string [] {"Heritage.Controllers"}// Parameter defaults
            );

            
        }

       

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            if (System.Web.Security.Roles.GetAllRoles().Length == 0)
            {
                using (IContextoDados Contexto = new ContextoDadosNH())
                {
                    Contexto.Setup();

                    System.Web.Security.Roles.CreateRole("Administrador");
                    System.Web.Security.Roles.CreateRole("Contabil");
                    System.Web.Security.Roles.CreateRole("Desenvolvedor");
                    MembershipCreateStatus status;
                    Membership.CreateUser("Ademi", "Ademi100787", "ademivieria@gmail.com", null, null, true, out status);
                    if (status == MembershipCreateStatus.Success)
                    {
                        Papel PapelAdministrador = new Papel();
                        PapelAdministrador.Nome = "Administrador";
                        Contexto.Add<Papel>(PapelAdministrador);
                        Contexto.SaveChanges();

                        Papel PapelDesenvolvedor = new Papel();
                        PapelDesenvolvedor.Nome = "Desenvolvedor";
                        Contexto.Add<Papel>(PapelDesenvolvedor);
                        Contexto.SaveChanges();

                        Roles.AddUserToRole("Ademi", "Desenvolvedor");
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