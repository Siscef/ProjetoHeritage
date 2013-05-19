using Heritage.Models;
using Heritage.Models.ContextoBanco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Heritage.Areas.Administracao.Controllers
{
    [Authorize(Roles = "Administrador,Contabil,Desenvolvedor")]
    public class BairroController : Controller
    {
        private IContextoDados ContextoBairro = new ContextoDadosNH();
        //
        // GET: /Administracao/Bairro/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Administracao/Bairro/Details/5

        public ActionResult Details(int id)
        {
            Bairro BairroDetails = ContextoBairro.Get<Bairro>(id);

            return View(BairroDetails);
        }

        //
        // GET: /Administracao/Bairro/Create

        public ActionResult Create()
        {
            ViewBag.Cidade = new SelectList(ContextoBairro.GetAll<Cidade>(), "Id_Cidade", "Nome");
            return View();
        }

        //
        // POST: /Administracao/Bairro/Create

        [HttpPost]
        public ActionResult Create(Bairro BairroParaSalvar)
        {
            lock (ContextoBairro)
            {
                ModelState["IdCidade.Nome"].Errors.Clear();
                ModelState["IdCidade.IdEstado"].Errors.Clear();
                if (ModelState.IsValid)
                {

                    AuditoriaInterna AuditoraBairro = new AuditoriaInterna();
                    AuditoraBairro.Computador = Environment.MachineName;
                    AuditoraBairro.DataInsercao = DateTime.Now;
                    AuditoraBairro.Usuario = User.Identity.Name;
                    AuditoraBairro.DetalhesOperacao = "Insercao Tabela Bairro, Registro: " + BairroParaSalvar.Nome;
                    AuditoraBairro.Tabela = "TB_Bairro";
                    AuditoraBairro.TipoOperacao = TipoOperacao.Insercao.ToString();
                    ContextoBairro.Add<AuditoriaInterna>(AuditoraBairro);
                    ContextoBairro.SaveChanges();

                    Bairro BairroSalvo = new Bairro();
                    BairroSalvo.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(BairroParaSalvar.Nome);
                    BairroSalvo.IdCidade = ContextoBairro.Get<Cidade>(BairroParaSalvar.IdCidade.Id_Cidade);
                    BairroSalvo.IdAuditoriaInterna = ContextoBairro.Get<AuditoriaInterna>(AuditoraBairro.Id_AuditoriaInterna);
                    ContextoBairro.Add<Bairro>(BairroSalvo);
                    ContextoBairro.SaveChanges();

                    return RedirectToAction("ListNeighborhood", BairroSalvo);
                }
            }
            ViewBag.Cidade = new SelectList(ContextoBairro.GetAll<Cidade>(), "Id_Cidade", "Nome", BairroParaSalvar.IdCidade);
            return View();
        }

        //
        // GET: /Administracao/Bairro/Edit/5

        public ActionResult Edit(int id)
        {

            Bairro BairroParaEditar = ContextoBairro.Get<Bairro>(id);
            ViewBag.Cidade = new SelectList(ContextoBairro.GetAll<Cidade>(), "Id_Cidade", "Nome");
            return View(BairroParaEditar);
        }

        //
        // POST: /Administracao/Bairro/Edit/5

        [HttpPost]
        public ActionResult Edit(Bairro BairroParaEditar)
        {
            lock (ContextoBairro)
            {


                AuditoriaInterna AuditoraBairro = new AuditoriaInterna();
                AuditoraBairro.Computador = Environment.MachineName;
                AuditoraBairro.DataInsercao = DateTime.Now;
                AuditoraBairro.Usuario = User.Identity.Name;
                AuditoraBairro.DetalhesOperacao = "Alteração Tabela Bairro, Registro: " + BairroParaEditar.Nome;
                AuditoraBairro.Tabela = "TB_Bairro";
                AuditoraBairro.TipoOperacao = TipoOperacao.Alteracao.ToString();
                ContextoBairro.Add<AuditoriaInterna>(AuditoraBairro);
                ContextoBairro.SaveChanges();

                Bairro BairroEditado = ContextoBairro.Get<Bairro>(BairroParaEditar.Id_Bairro);
                BairroEditado.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(BairroParaEditar.Nome);
                BairroEditado.IdCidade = ContextoBairro.Get<Cidade>(BairroParaEditar.IdCidade.Id_Cidade);
                BairroEditado.IdAuditoriaInterna = ContextoBairro.Get<AuditoriaInterna>(AuditoraBairro.Id_AuditoriaInterna);
                ContextoBairro.Add<Bairro>(BairroEditado);
                ContextoBairro.SaveChanges();


                return RedirectToAction("ListNeighborhood", BairroEditado);
            }
        }

        //
        // GET: /Administracao/Bairro/Delete/5

        public ActionResult Delete(int id)
        {
            Bairro BairroParaSerExcluido = ContextoBairro.Get<Bairro>(id);
            return View(BairroParaSerExcluido);
        }

        //
        // POST: /Administracao/Bairro/Delete/5

        [HttpPost]
        public ActionResult Delete(Bairro BairroParaExclusao)
        {
            lock (this)
            {

                AuditoriaInterna AuditoriaBairro = new AuditoriaInterna();
                AuditoriaBairro.Computador = Environment.MachineName;
                AuditoriaBairro.DataInsercao = DateTime.Now;
                AuditoriaBairro.Usuario = User.Identity.Name;
                AuditoriaBairro.DetalhesOperacao = "Exclusao Tabela Bairro, Registro: " + BairroParaExclusao.Nome;
                AuditoriaBairro.Tabela = "TB_Bairro";
                AuditoriaBairro.TipoOperacao = TipoOperacao.Exclusao.ToString();
                ContextoBairro.Add<AuditoriaInterna>(AuditoriaBairro);
                ContextoBairro.SaveChanges();

                Bairro BairroExcluido = ContextoBairro.Get<Bairro>(BairroParaExclusao.Id_Bairro);
                ContextoBairro.Delete<Bairro>(BairroExcluido);
                ContextoBairro.SaveChanges();

                return View("Index");

            }
        }

        public ActionResult ListAdress(int id)
        {
            IList<Endereco> ListAdress = ContextoBairro.GetAll<Endereco>()
                                         .Where(x => x.IdBairro.Id_Bairro == id)
                                         .OrderBy(x => x.Descricao)
                                         .ToList();
            return View(ListAdress);
        }

        public ActionResult ListNeighborhood(Bairro BairroSalvo)
        {
            IList<Bairro> ListNeighborhood = ContextoBairro.GetAll<Bairro>()
                                             .Where(x => x.Id_Bairro == BairroSalvo.Id_Bairro)
                                             .ToList();
            return View(ListNeighborhood);
        }
    }
}
