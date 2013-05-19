using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Heritage.Models;
using Heritage.Models.ContextoBanco;
using Heritage.Models.Interface;

namespace Heritage.Areas.Administracao.Controllers
{
   [Authorize(Roles = "Administrador,Contabil,Desenvolvedor")]
    public class EstadoController : Controller
    {
        private IContextoDados ContextoEstado = new ContextoDadosNH();
        //
        // GET: /Administracao/Estado/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Administracao/Estado/Details/5

        public ActionResult Details(int id)
        {
            Estado EstadoDetails = ContextoEstado.Get<Estado>(id);
            return View(EstadoDetails);
        }

        //
        // GET: /Administracao/Estado/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Administracao/Estado/Create

        [HttpPost]
        public ActionResult Create(Estado NovoEstado)
        {


            if (ModelState.IsValid)
            {
                AuditoriaInterna AuditoriaEstado = new AuditoriaInterna();
                AuditoriaEstado.Computador = Environment.MachineName;
                AuditoriaEstado.DataInsercao = DateTime.Now;
                AuditoriaEstado.Usuario = User.Identity.Name;
                AuditoriaEstado.DetalhesOperacao = "Insercao Tabela Estado, Registro: " + NovoEstado.Nome;
                AuditoriaEstado.Tabela = "TB_Estado";
                AuditoriaEstado.TipoOperacao = TipoOperacao.Insercao.ToString();
                ContextoEstado.Add<AuditoriaInterna>(AuditoriaEstado);
                ContextoEstado.SaveChanges();

                Estado EstadoSalvo = new Estado();
                EstadoSalvo.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(NovoEstado.Nome);
                EstadoSalvo.Sigla = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(NovoEstado.Sigla);
                EstadoSalvo.IdAuditoriaInterna = ContextoEstado.Get<AuditoriaInterna>(AuditoriaEstado.Id_AuditoriaInterna);
                ContextoEstado.Add<Estado>(EstadoSalvo);
                ContextoEstado.SaveChanges();

                return RedirectToAction("ListLast", EstadoSalvo);

            }

            return View();
        }

        //
        // GET: /Administracao/Estado/Edit/5

        public ActionResult Edit(int id)
        {
            Estado EstadoParaEdicao = ContextoEstado.Get<Estado>(id);
            return View(EstadoParaEdicao);
        }

        //
        // POST: /Administracao/Estado/Edit/5

        [HttpPost]
        public ActionResult Edit(Estado EstadoEditado)
        {
            AuditoriaInterna AuditoriaEstado = new AuditoriaInterna();
            AuditoriaEstado.Computador = Environment.MachineName;
            AuditoriaEstado.DataInsercao = DateTime.Now;
            AuditoriaEstado.Usuario = User.Identity.Name;
            AuditoriaEstado.DetalhesOperacao = "Alteracao Tabela Estado, Registro: " + EstadoEditado.Nome;
            AuditoriaEstado.Tabela = "TB_Estado";
            AuditoriaEstado.TipoOperacao = TipoOperacao.Alteracao.ToString();
            ContextoEstado.Add<AuditoriaInterna>(AuditoriaEstado);
            ContextoEstado.SaveChanges();

            Estado EstadoSalvo = ContextoEstado.Get<Estado>(EstadoEditado.Id_Estado);
            EstadoSalvo.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(EstadoEditado.Nome);
            EstadoSalvo.Sigla = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(EstadoEditado.Sigla);
            EstadoSalvo.IdAuditoriaInterna = ContextoEstado.Get<AuditoriaInterna>(AuditoriaEstado.Id_AuditoriaInterna);
            TryUpdateModel<Estado>(EstadoSalvo);
            ContextoEstado.SaveChanges();


            return RedirectToAction("ListLast", EstadoSalvo);
        }

        //
        // GET: /Administracao/Estado/Delete/5

        public ActionResult Delete(int id)
        {
            Estado EstadoParaExcluir = ContextoEstado.Get<Estado>(id);
            return View(EstadoParaExcluir);
        }

        //
        // POST: /Administracao/Estado/Delete/5

        [HttpPost]
        public ActionResult Delete(Estado EstadoExcluido)
        {

            AuditoriaInterna AuditoriaEstado = new AuditoriaInterna();
            AuditoriaEstado.Computador = Environment.MachineName;
            AuditoriaEstado.DataInsercao = DateTime.Now;
            AuditoriaEstado.Usuario = User.Identity.Name;
            AuditoriaEstado.DetalhesOperacao = "Exclusão Tabela Estado, Registro: " + EstadoExcluido.Nome;
            AuditoriaEstado.Tabela = "TB_Estado";
            AuditoriaEstado.TipoOperacao = TipoOperacao.Exclusao.ToString();
            ContextoEstado.Add<AuditoriaInterna>(AuditoriaEstado);
            ContextoEstado.SaveChanges();

            Estado EstadoExcluidoBanco = ContextoEstado.Get<Estado>(EstadoExcluido.Id_Estado);
            ContextoEstado.Delete<Estado>(EstadoExcluidoBanco);
            ContextoEstado.SaveChanges();
            ContextoEstado.Dispose();
           
            return RedirectToAction("Index","Home");
        }

        public ActionResult ListAll()
        {
            IList<Estado> ListaEstados = ContextoEstado.GetAll<Estado>().OrderBy(x => x.Nome).ToList();
            return View(ListaEstados);
        }

        public ActionResult ListLast(Estado EstadoSalvo)
        {
            IList<Estado> ListaUltimoEstado = ContextoEstado.GetAll<Estado>().Where(x => x.Id_Estado == EstadoSalvo.Id_Estado).OrderBy(x => x.Nome).ToList();
            return View(ListaUltimoEstado);
        }

        public ActionResult ListCity(int id)
        {
            IList<Cidade> ListCity = ContextoEstado.GetAll<Cidade>()
                                     .Where(x => x.IdEstado.Id_Estado == id).ToList();
            return View(ListCity);
        }
    }
}
