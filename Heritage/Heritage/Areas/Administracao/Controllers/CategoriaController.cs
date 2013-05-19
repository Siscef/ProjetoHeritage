using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Heritage.Models;
using Heritage.Models.ContextoBanco;


namespace Heritage.Areas.Administracao.Controllers
{
    [Authorize(Roles = "Administrador,Contabil,Desenvolvedor")]
    public class CategoriaController : Controller
    {
        private IContextoDados ContextoCategoria = new ContextoDadosNH();
        //
        // GET: /Administracao/Categoria/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Administracao/Categoria/Details/5

        public ActionResult Details(int id)
        {
            Categoria CategoriaDetails = ContextoCategoria.Get<Categoria>(id);
            return View(CategoriaDetails);
        }

        //
        // GET: /Administracao/Categoria/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Administracao/Categoria/Create

        [HttpPost]
        public ActionResult Create(Categoria CategoriaParaSalva)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AuditoriaInterna AuditoriaCategoria = new AuditoriaInterna();

                    AuditoriaCategoria.Computador = Environment.MachineName;
                    AuditoriaCategoria.DataInsercao = DateTime.Now;
                    AuditoriaCategoria.DetalhesOperacao = "Insercao Tabela Categoria, Registro: " + CategoriaParaSalva.Nome;
                    AuditoriaCategoria.Tabela = "TB_Categoria";
                    AuditoriaCategoria.Usuario = User.Identity.Name;
                    AuditoriaCategoria.TipoOperacao = TipoOperacao.Insercao.ToString();
                    ContextoCategoria.Add<AuditoriaInterna>(AuditoriaCategoria);
                    ContextoCategoria.SaveChanges();


                    Categoria CategoriaSalva = new Categoria();
                    CategoriaSalva.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(CategoriaParaSalva.Nome);
                    CategoriaSalva.IdAuditoriaInterna = ContextoCategoria.Get<AuditoriaInterna>(AuditoriaCategoria.Id_AuditoriaInterna);
                    ContextoCategoria.Add<Categoria>(CategoriaSalva);
                    ContextoCategoria.SaveChanges();

                   return RedirectToAction("LastCategory", CategoriaSalva);

                }
                catch
                {

                    return View();
                }

            }
            return View();
        }

        //
        // GET: /Administracao/Categoria/Edit/5

        public ActionResult Edit(int id)
        {
            Categoria CategoriaParaEdicao = ContextoCategoria.Get<Categoria>(id);

            return View(CategoriaParaEdicao);
        }

        //
        // POST: /Administracao/Categoria/Edit/5

        [HttpPost]
        public ActionResult Edit(Categoria CategoriaParaEdicao)
        {
            try
            {
                AuditoriaInterna AuditoriaCategoria = new AuditoriaInterna();

                AuditoriaCategoria.Computador = Environment.MachineName;
                AuditoriaCategoria.DataInsercao = DateTime.Now;
                AuditoriaCategoria.DetalhesOperacao = "Alteracao Tabela Categoria, Registro: " + CategoriaParaEdicao.Nome;
                AuditoriaCategoria.Tabela = "TB_Categoria";
                AuditoriaCategoria.Usuario = User.Identity.Name;
                AuditoriaCategoria.TipoOperacao = TipoOperacao.Alteracao.ToString();
                ContextoCategoria.Add<AuditoriaInterna>(AuditoriaCategoria);
                ContextoCategoria.SaveChanges();

                Categoria CategoriaEditada = ContextoCategoria.Get<Categoria>(CategoriaParaEdicao.Id_Categoria);
                CategoriaEditada.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(CategoriaParaEdicao.Nome);
                CategoriaEditada.IdAuditoriaInterna = ContextoCategoria.Get<AuditoriaInterna>(AuditoriaCategoria.Id_AuditoriaInterna);
                ContextoCategoria.Add<Categoria>(CategoriaEditada);
                ContextoCategoria.SaveChanges();


                return RedirectToAction("LastCategory", CategoriaEditada);
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Administracao/Categoria/Delete/5

        public ActionResult Delete(int id)
        {
            Categoria CategoriaParaExcluir = ContextoCategoria.Get<Categoria>(id);
            return View(CategoriaParaExcluir);
        }

        //
        // POST: /Administracao/Categoria/Delete/5

        [HttpPost]
        public ActionResult Delete(Categoria CategoriaParaExcluir)
        {
            try
            {
                AuditoriaInterna AuditoriaCategoria = new AuditoriaInterna();

                AuditoriaCategoria.Computador = Environment.MachineName;
                AuditoriaCategoria.DataInsercao = DateTime.Now;
                AuditoriaCategoria.DetalhesOperacao = "Exclusao Tabela Categoria, Registro: " + CategoriaParaExcluir.Nome;
                AuditoriaCategoria.Tabela = "TB_Categoria";
                AuditoriaCategoria.Usuario = User.Identity.Name;
                AuditoriaCategoria.TipoOperacao = TipoOperacao.Exclusao.ToString();
                ContextoCategoria.Add<AuditoriaInterna>(AuditoriaCategoria);
                ContextoCategoria.SaveChanges();

                Categoria CategoriaExcluida = ContextoCategoria.Get<Categoria>(CategoriaParaExcluir.Id_Categoria);
                ContextoCategoria.Delete<Categoria>(CategoriaExcluida);
                ContextoCategoria.SaveChanges();


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

        public ActionResult LastCategory(Categoria CategoriaSalva)
        {
            IList<Categoria> LastCategoryView = ContextoCategoria.GetAll<Categoria>()
                                            .Where(x => x.Id_Categoria == CategoriaSalva.Id_Categoria)
                                            .OrderBy(x => x.Nome)
                                            .ToList();
            return View(LastCategoryView);
        }

        public ActionResult ListAllCategory()
        {
            IList<Categoria> ListAllCategory = ContextoCategoria.GetAll<Categoria>()
                                               .OrderBy(x => x.Nome)
                                               .ToList();
            return View(ListAllCategory);
        }
    }
}
