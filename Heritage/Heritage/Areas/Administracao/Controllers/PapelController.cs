using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Heritage.Models;
using Heritage.Models.ContextoBanco;
using System.Web.Security;

namespace Heritage.Areas.Administracao.Controllers
{
    [Authorize(Roles="Administrador")]
    public class PapelController : Controller
    {
        private IContextoDados ContextoPapel = new ContextoDadosNH();
        //
        // GET: /Administracao/Papel/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Administracao/Papel/Details/5

        public ActionResult Details(int id)
        {
            Papel PapelDetails = ContextoPapel.Get<Papel>(id);
            return View(PapelDetails);
        }

        //
        // GET: /Administracao/Papel/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Administracao/Papel/Create

        [HttpPost]
        public ActionResult Create(Papel PapelParaSalvar)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AuditoriaInterna AuditoraPapel = new AuditoriaInterna();
                    AuditoraPapel.Computador = Environment.MachineName;
                    AuditoraPapel.DataInsercao = DateTime.Now;
                    AuditoraPapel.Usuario = User.Identity.Name;
                    AuditoraPapel.DetalhesOperacao = "Insercao Tabela Papel, Registro: " + PapelParaSalvar.Nome;
                    AuditoraPapel.Tabela = "TB_Papel";
                    AuditoraPapel.TipoOperacao = TipoOperacao.Insercao.ToString();
                    ContextoPapel.Add<AuditoriaInterna>(AuditoraPapel);
                    ContextoPapel.SaveChanges();

                    Papel PapelSalvo = new Papel();
                    PapelSalvo.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(PapelParaSalvar.Nome);
                    PapelSalvo.IdAuditoriaInterna = ContextoPapel.Get<AuditoriaInterna>(AuditoraPapel.Id_AuditoriaInterna);
                    ContextoPapel.Add<Papel>(PapelSalvo);
                    ContextoPapel.SaveChanges();

                    System.Web.Security.Roles.CreateRole(PapelSalvo.Nome);

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                } 
            }
            return View();
            
        }

        //
        // GET: /Administracao/Papel/Edit/5

        public ActionResult Edit(int id)
        {
            Papel PapelParaEdicao = ContextoPapel.Get<Papel>(id);
            return View(PapelParaEdicao);
        }

        //
        // POST: /Administracao/Papel/Edit/5

        [HttpPost]
        public ActionResult Edit(Papel PapelParaEdicao)
        {
            try
            {
                // TODO: Add update logic here
                AuditoriaInterna AuditoraPapel = new AuditoriaInterna();
                AuditoraPapel.Computador = Environment.MachineName;
                AuditoraPapel.DataInsercao = DateTime.Now;
                AuditoraPapel.Usuario = User.Identity.Name;
                AuditoraPapel.DetalhesOperacao = "Alteracao Tabela Papel, Registro: " + PapelParaEdicao.Nome;
                AuditoraPapel.Tabela = "TB_Papel";
                AuditoraPapel.TipoOperacao = TipoOperacao.Alteracao.ToString();
                ContextoPapel.Add<AuditoriaInterna>(AuditoraPapel);
                ContextoPapel.SaveChanges();

                Papel PapelEditado = ContextoPapel.Get<Papel>(PapelParaEdicao.Id_Papel);
                PapelEditado.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(PapelParaEdicao.Nome);
                PapelEditado.IdAuditoriaInterna = ContextoPapel.Get<AuditoriaInterna>(AuditoraPapel.Id_AuditoriaInterna);
                ContextoPapel.Add<Papel>(PapelEditado);
                ContextoPapel.SaveChanges();
                string NomePapel = (from c in ContextoPapel.GetAll<Papel>()
                                    .Where(x => x.Nome == PapelEditado.Nome)
                                    select c.Nome).First();

                System.Web.Security.Roles.DeleteRole(NomePapel);
                System.Web.Security.Roles.CreateRole(PapelEditado.Nome);

                return RedirectToAction("LastResponsible", "PapelEditado");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Administracao/Papel/Delete/5

        public ActionResult Delete(int id)
        {
            Papel PapelParaExclusao = ContextoPapel.Get<Papel>(id);
            return View(PapelParaExclusao);
        }

        //
        // POST: /Administracao/Papel/Delete/5

        [HttpPost]
        public ActionResult Delete(Papel PapelParaExcluir)
        {
            try
            {
                // TODO: Add delete logic here
                Papel PapelParaExclusao = ContextoPapel.Get<Papel>(PapelParaExcluir.Id_Papel);

                string NomePapel = (from c in ContextoPapel.GetAll<Papel>()
                                  .Where(x => x.Nome == PapelParaExclusao.Nome)
                                    select c.Nome).First();
                System.Web.Security.Roles.DeleteRole(NomePapel);               
                ContextoPapel.Delete<Papel>(PapelParaExclusao);
                ContextoPapel.SaveChanges();

               

                AuditoriaInterna AuditoraPapel = new AuditoriaInterna();
                AuditoraPapel.Computador = Environment.MachineName;
                AuditoraPapel.DataInsercao = DateTime.Now;
                AuditoraPapel.Usuario = User.Identity.Name;
                AuditoraPapel.DetalhesOperacao = "Exclusao Tabela Papel, Registro: " + PapelParaExcluir.Nome;
                AuditoraPapel.Tabela = "TB_Papel";
                AuditoraPapel.TipoOperacao = TipoOperacao.Exclusao.ToString();
                ContextoPapel.Add<AuditoriaInterna>(AuditoraPapel);
                ContextoPapel.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ListAllRoles()
        {
            IList<Papel> ListAllRoles = ContextoPapel.GetAll<Papel>()
                                        .OrderBy(x => x.Nome)
                                        .ToList();
            return View(ListAllRoles);
        }

        public ActionResult LastResponsible(Papel PapelSalvo)
        {
            IList<Papel> Last = ContextoPapel.GetAll<Papel>()
                                 .Where(x => x.Id_Papel == PapelSalvo.Id_Papel)
                                 .ToList();
            return View(Last);
        }
       
    }
}
