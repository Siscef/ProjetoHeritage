using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Heritage.Models;
using Heritage.Models.ContextoBanco;

namespace Heritage.Areas.Administracao.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class CidadeController : Controller
    {
        private IContextoDados ContextoCidade = new ContextoDadosNH();
        //
        // GET: /Administracao/Cidade/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Administracao/Cidade/Details/5

        public ActionResult Details(int id)
        {
            Cidade DetalhesCidade = ContextoCidade.Get<Cidade>(id);
            return View(DetalhesCidade);
        }

        //
        // GET: /Administracao/Cidade/Create

        public ActionResult Create()
        {
            ViewBag.Estado = new SelectList(ContextoCidade.GetAll<Estado>(), "Id_Estado", "Nome");
            return View();
        }

        //
        // POST: /Administracao/Cidade/Create

        [HttpPost]
        public ActionResult Create(Cidade Cidade)
        {
            lock (ContextoCidade)
            {
                ModelState["IdEstado.Nome"].Errors.Clear();
                ModelState["IdEstado.Sigla"].Errors.Clear();

                if (ModelState.IsValid)
                {
                    AuditoriaInterna AuditoriaCidade = new AuditoriaInterna();
                    AuditoriaCidade.Computador = Environment.MachineName;
                    AuditoriaCidade.DataInsercao = DateTime.Now;
                    AuditoriaCidade.Usuario = User.Identity.Name;
                    AuditoriaCidade.DetalhesOperacao = "Insercao Tabela Cidade, Registro: " + Cidade.Nome;
                    AuditoriaCidade.Tabela = "TB_Cidade";
                    AuditoriaCidade.TipoOperacao = TipoOperacao.Insercao.ToString();
                    ContextoCidade.Add<AuditoriaInterna>(AuditoriaCidade);
                    ContextoCidade.SaveChanges();

                    Cidade CidadeSalva = new Cidade();
                    CidadeSalva.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(Cidade.Nome);
                    CidadeSalva.IdEstado = ContextoCidade.Get<Estado>(Cidade.IdEstado.Id_Estado);
                    CidadeSalva.IdAuditoriaInterna = ContextoCidade.Get<AuditoriaInterna>(AuditoriaCidade.Id_AuditoriaInterna);
                    ContextoCidade.Add<Cidade>(CidadeSalva);
                    ContextoCidade.SaveChanges();                    

                    return RedirectToAction("ListLast", CidadeSalva);

                }
            }
            ViewBag.Estado = new SelectList(ContextoCidade.GetAll<Estado>(), "Id_Estado", "Nome", Cidade.IdEstado);
            return View();
        }

        //
        // GET: /Administracao/Cidade/Edit/5

        public ActionResult Edit(int id)
        {
            Cidade CidadeParaAlterar = ContextoCidade.Get<Cidade>(id);
            ViewBag.Estado = new SelectList(ContextoCidade.GetAll<Estado>(), "Id_Estado", "Nome");
            return View(CidadeParaAlterar);
        }

        //
        // POST: /Administracao/Cidade/Edit/5

        [HttpPost]
        public ActionResult Edit(Cidade CidadeParaAlterar)
        {
            AuditoriaInterna AuditoriaCidade = new AuditoriaInterna();
            AuditoriaCidade.Computador = Environment.MachineName;
            AuditoriaCidade.DataInsercao = DateTime.Now;
            AuditoriaCidade.Usuario = User.Identity.Name;
            AuditoriaCidade.Tabela = "TB_Cidade";
            AuditoriaCidade.TipoOperacao = TipoOperacao.Alteracao.ToString();
            AuditoriaCidade.DetalhesOperacao = "Alteração Tabela Cidade, Registro: " + CidadeParaAlterar.Nome;
            ContextoCidade.Add<AuditoriaInterna>(AuditoriaCidade);
            ContextoCidade.SaveChanges();

            Cidade CidadeAlterada = ContextoCidade.Get<Cidade>(CidadeParaAlterar.Id_Cidade);
            CidadeAlterada.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(CidadeParaAlterar.Nome);
            CidadeAlterada.IdEstado = ContextoCidade.Get<Estado>(CidadeParaAlterar.IdEstado.Id_Estado);
            CidadeAlterada.IdAuditoriaInterna = ContextoCidade.Get<AuditoriaInterna>(AuditoriaCidade.Id_AuditoriaInterna);
            TryUpdateModel<Cidade>(CidadeAlterada);
            ContextoCidade.SaveChanges();
                  

            return RedirectToAction("ListLast", CidadeAlterada);
        }

        //
        // GET: /Administracao/Cidade/Delete/5

        public ActionResult Delete(int id)
        {
            Cidade CidadeParaExcluir = ContextoCidade.Get<Cidade>(id);
            return View(CidadeParaExcluir);
        }

        //
        // POST: /Administracao/Cidade/Delete/5

        [HttpPost]
        public ActionResult Delete(Cidade CidadeParaExcluir)
        {
            Cidade CidadeExcluida = ContextoCidade.Get<Cidade>(CidadeParaExcluir.Id_Cidade);
            ContextoCidade.Delete<Cidade>(CidadeExcluida);
            ContextoCidade.SaveChanges();
           
            AuditoriaInterna AuditoriaCidade = new AuditoriaInterna();
            AuditoriaCidade.Computador = Environment.MachineName;
            AuditoriaCidade.DataInsercao = DateTime.Now;
            AuditoriaCidade.Usuario = User.Identity.Name;
            AuditoriaCidade.DetalhesOperacao = "Exclusão Tabela Cidade, Registro: " + CidadeParaExcluir.Nome;
            AuditoriaCidade.Tabela = "TB_Cidade";
            AuditoriaCidade.TipoOperacao = TipoOperacao.Exclusao.ToString();
            ContextoCidade.Add<AuditoriaInterna>(AuditoriaCidade);
            ContextoCidade.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult ListLast(Cidade CidadeSalva)
        {
            IList<Cidade> ListaUltimaCidade = ContextoCidade.GetAll<Cidade>().Where(x => x.Id_Cidade == CidadeSalva.Id_Cidade).OrderBy(x => x.Nome).ToList();
            return View(ListaUltimaCidade);
        }
    }
}
