using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Heritage.Models;
using Heritage.Models.ContextoBanco;

namespace Heritage.Areas.Administracao.Controllers
{
    [Authorize(Roles = "Administrador,Contabil")]
    public class GrupoController : Controller
    {
        private IContextoDados ContextoGrupo = new ContextoDadosNH();
        //
        // GET: /Administracao/Grupo/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Administracao/Grupo/Details/5

        public ActionResult Details(int id)
        {
            Grupo GrupoDetails = ContextoGrupo.Get<Grupo>(id);
            return View(GrupoDetails);
        }

        //
        // GET: /Administracao/Grupo/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Administracao/Grupo/Create

        [HttpPost]
        public ActionResult Create(Grupo GrupoParaSalvar)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AuditoriaInterna AuditoraGrupo = new AuditoriaInterna();
                    AuditoraGrupo.Computador = Environment.MachineName;
                    AuditoraGrupo.DataInsercao = DateTime.Now;
                    AuditoraGrupo.Usuario = User.Identity.Name;
                    AuditoraGrupo.DetalhesOperacao = "Insercao Tabela Grupo, Registro: " + GrupoParaSalvar.Nome;
                    AuditoraGrupo.Tabela = "TB_Grupo";
                    AuditoraGrupo.TipoOperacao = TipoOperacao.Insercao.ToString();
                    ContextoGrupo.Add<AuditoriaInterna>(AuditoraGrupo);
                    ContextoGrupo.SaveChanges();

                    Grupo GrupoSalvo = new Grupo();
                    GrupoSalvo.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(GrupoParaSalvar.Nome);
                    GrupoSalvo.IdAuditoriaInterna = ContextoGrupo.Get<AuditoriaInterna>(AuditoraGrupo.Id_AuditoriaInterna);
                    ContextoGrupo.Add<Grupo>(GrupoSalvo);
                    ContextoGrupo.SaveChanges();
                    return RedirectToAction("LastGroup", GrupoSalvo);

                }
                catch
                {

                    return View();
                }


            }

            return View();

        }

        //
        // GET: /Administracao/Grupo/Edit/5

        public ActionResult Edit(int id)
        {
            Grupo GrupoParaEdicao = ContextoGrupo.Get<Grupo>(id);
            return View(GrupoParaEdicao);
        }

        //
        // POST: /Administracao/Grupo/Edit/5

        [HttpPost]
        public ActionResult Edit(Grupo GrupoParaEdicao)
        {
            try
            {
                AuditoriaInterna AuditoraGrupo = new AuditoriaInterna();
                AuditoraGrupo.Computador = Environment.MachineName;
                AuditoraGrupo.DataInsercao = DateTime.Now;
                AuditoraGrupo.Usuario = User.Identity.Name;
                AuditoraGrupo.DetalhesOperacao = "Alteracao Tabela Grupo, Registro: " + GrupoParaEdicao.Nome;
                AuditoraGrupo.Tabela = "TB_Grupo";
                AuditoraGrupo.TipoOperacao = TipoOperacao.Alteracao.ToString();
                ContextoGrupo.Add<AuditoriaInterna>(AuditoraGrupo);
                ContextoGrupo.SaveChanges();

                Grupo GrupoEditado = ContextoGrupo.Get<Grupo>(GrupoParaEdicao.Id_Grupo);
                GrupoEditado.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(GrupoParaEdicao.Nome);
                GrupoEditado.IdAuditoriaInterna = ContextoGrupo.Get<AuditoriaInterna>(AuditoraGrupo.Id_AuditoriaInterna);
                ContextoGrupo.Add<Grupo>(GrupoEditado);
                ContextoGrupo.SaveChanges();

                return RedirectToAction("LastGroup", GrupoEditado);
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Administracao/Grupo/Delete/5

        public ActionResult Delete(int id)
        {
            Grupo GrupoParaExclusao = ContextoGrupo.Get<Grupo>(id);
            return View(GrupoParaExclusao);
        }

        //
        // POST: /Administracao/Grupo/Delete/5

        [HttpPost]
        public ActionResult Delete(Grupo GrupoParaExclusao)
        {
            try
            {
                AuditoriaInterna AuditoraGrupo = new AuditoriaInterna();
                AuditoraGrupo.Computador = Environment.MachineName;
                AuditoraGrupo.DataInsercao = DateTime.Now;
                AuditoraGrupo.Usuario = User.Identity.Name;
                AuditoraGrupo.DetalhesOperacao = "Exclusao Tabela Grupo, Registro: " + GrupoParaExclusao.Nome;
                AuditoraGrupo.Tabela = "TB_Grupo";
                AuditoraGrupo.TipoOperacao = TipoOperacao.Exclusao.ToString();
                ContextoGrupo.Add<AuditoriaInterna>(AuditoraGrupo);
                ContextoGrupo.SaveChanges();

                Grupo GrupoExcluido = ContextoGrupo.Get<Grupo>(GrupoParaExclusao.Id_Grupo);
                ContextoGrupo.Delete<Grupo>(GrupoExcluido);
                ContextoGrupo.SaveChanges();

                if (User.IsInRole("Administrador"))
                {
                    return RedirectToAction("Index", "Home", new { area = "Administracao" });
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { area = "Contabil" });
                }

            }
            catch
            {
                return View();
            }
        }
        public ActionResult LastGroup(Grupo GrupoSalvo)
        {
            IList<Grupo> ListLastGroup = ContextoGrupo.GetAll<Grupo>()
                                         .Where(x => x.Id_Grupo == GrupoSalvo.Id_Grupo)
                                         .OrderBy(x => x.Nome)
                                         .ToList();
            return View(ListLastGroup);
        }

        public ActionResult ListAllGroup()
        {
            IList<Grupo> ListAllGroup = ContextoGrupo.GetAll<Grupo>()
                                         .OrderBy(x => x.Nome)
                                         .ToList();
            return View(ListAllGroup);
        }
    }
}
