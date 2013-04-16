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
    public class LocalizacaoController : Controller
    {
        private IContextoDados ContextoLocalizacao = new ContextoDadosNH();
        //
        // GET: /Administracao/Localizacao/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Administracao/Localizacao/Details/5

        public ActionResult Details(int id)
        {
            Localizacao LocalizacaoDetails = ContextoLocalizacao.Get<Localizacao>(id);
            return View(LocalizacaoDetails);
        }

        //
        // GET: /Administracao/Localizacao/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Administracao/Localizacao/Create

        [HttpPost]
        public ActionResult Create(Localizacao LocalizacaoBem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AuditoriaInterna AuditoriaLocalizacao = new AuditoriaInterna();
                    AuditoriaLocalizacao.Computador = Environment.MachineName;
                    AuditoriaLocalizacao.DataInsercao = DateTime.Now;
                    AuditoriaLocalizacao.DetalhesOperacao = "Insercao Tabela Localizacao, Registro: " + LocalizacaoBem.Descricao;
                    AuditoriaLocalizacao.Tabela = "TB_Localizacao";
                    AuditoriaLocalizacao.TipoOperacao = TipoOperacao.Insercao.ToString();
                    AuditoriaLocalizacao.Usuario = User.Identity.Name;
                    ContextoLocalizacao.Add<AuditoriaInterna>(AuditoriaLocalizacao);
                    ContextoLocalizacao.SaveChanges();

                    Localizacao LocalizacaoSalva = new Localizacao();
                    LocalizacaoSalva.Descricao = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(LocalizacaoBem.Descricao);
                    LocalizacaoSalva.IdAuditoriaInterna = ContextoLocalizacao.Get<AuditoriaInterna>(AuditoriaLocalizacao.Id_AuditoriaInterna);
                    ContextoLocalizacao.Add<Localizacao>(LocalizacaoSalva);
                    ContextoLocalizacao.SaveChanges();

                    return RedirectToAction("LastLocalization", LocalizacaoSalva);

                }
                catch
                {

                    return View();
                }


            }
            return View();

        }

        //
        // GET: /Administracao/Localizacao/Edit/5

        public ActionResult Edit(int id)
        {
            Localizacao LocalizacaoParaEdicao = ContextoLocalizacao.Get<Localizacao>(id);
            return View(LocalizacaoParaEdicao);
        }

        //
        // POST: /Administracao/Localizacao/Edit/5

        [HttpPost]
        public ActionResult Edit(Localizacao LocalizacaoParaEdicao)
        {
            try
            {
                AuditoriaInterna AuditoriaLocalizacao = new AuditoriaInterna();
                AuditoriaLocalizacao.Computador = Environment.MachineName;
                AuditoriaLocalizacao.DataInsercao = DateTime.Now;
                AuditoriaLocalizacao.DetalhesOperacao = "Alteração Tabela Localizacao, Registro: " + LocalizacaoParaEdicao.Descricao;
                AuditoriaLocalizacao.Tabela = "TB_Localizacao";
                AuditoriaLocalizacao.TipoOperacao = TipoOperacao.Alteracao.ToString();
                AuditoriaLocalizacao.Usuario = User.Identity.Name;
                ContextoLocalizacao.Add<AuditoriaInterna>(AuditoriaLocalizacao);
                ContextoLocalizacao.SaveChanges();

                Localizacao LocalizacaoSalva = ContextoLocalizacao.Get<Localizacao>(LocalizacaoParaEdicao.Id_Localizacao);
                LocalizacaoSalva.Descricao = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(LocalizacaoParaEdicao.Descricao);
                LocalizacaoSalva.IdAuditoriaInterna = ContextoLocalizacao.Get<AuditoriaInterna>(AuditoriaLocalizacao.Id_AuditoriaInterna);
                ContextoLocalizacao.Add<Localizacao>(LocalizacaoSalva);
                ContextoLocalizacao.SaveChanges();
                return RedirectToAction("LastLocalization", LocalizacaoSalva);
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Administracao/Localizacao/Delete/5

        public ActionResult Delete(int id)
        {
            Localizacao LocalizacaoParaExlusao = ContextoLocalizacao.Get<Localizacao>(id);

            return View(LocalizacaoParaExlusao);
        }

        //
        // POST: /Administracao/Localizacao/Delete/5

        [HttpPost]
        public ActionResult Delete(Localizacao LocalizacaoParaExclusao)
        {
            try
            {
                // TODO: Add delete logic here
                AuditoriaInterna AuditoriaLocalizacao = new AuditoriaInterna();
                AuditoriaLocalizacao.Computador = Environment.MachineName;
                AuditoriaLocalizacao.DataInsercao = DateTime.Now;
                AuditoriaLocalizacao.DetalhesOperacao = "Alteração Tabela Localizacao, Registro: " + LocalizacaoParaExclusao.Descricao;
                AuditoriaLocalizacao.Tabela = "TB_Localizacao";
                AuditoriaLocalizacao.TipoOperacao = TipoOperacao.Exclusao.ToString();
                AuditoriaLocalizacao.Usuario = User.Identity.Name;
                ContextoLocalizacao.Add<AuditoriaInterna>(AuditoriaLocalizacao);
                ContextoLocalizacao.SaveChanges();


                Localizacao LocalizacaoExcluida = ContextoLocalizacao.Get<Localizacao>(LocalizacaoParaExclusao.Id_Localizacao);
                ContextoLocalizacao.Delete<Localizacao>(LocalizacaoExcluida);
                ContextoLocalizacao.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult LastLocalization(Localizacao LocalizacaoSalva)
        {
            IList<Localizacao> LastLocalization = ContextoLocalizacao.GetAll<Localizacao>()
                                                  .Where(x => x.Id_Localizacao == LocalizacaoSalva.Id_Localizacao)
                                                  .ToList();
            return View(LastLocalization);
        }

        public ActionResult ListLocation()
        {
            IList<Localizacao> ListLocationView = ContextoLocalizacao.GetAll<Localizacao>()
                                                  .OrderBy(x => x.Descricao)
                                                  .ToList();
            return View(ListLocationView);
        }
    }
}
