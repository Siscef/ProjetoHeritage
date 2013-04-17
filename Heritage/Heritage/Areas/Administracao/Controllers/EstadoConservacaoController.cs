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
    public class EstadoConservacaoController : Controller
    {
        private IContextoDados ContextoEstadoConservacao = new ContextoDadosNH();
        //
        // GET: /Administracao/EstadoConservacao/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Administracao/EstadoConservacao/Details/5

        public ActionResult Details(int id)
        {
            EstadoConservacao EstadoConservacaoDetails = ContextoEstadoConservacao.Get<EstadoConservacao>(id);
            return View(EstadoConservacaoDetails);
        }

        //
        // GET: /Administracao/EstadoConservacao/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Administracao/EstadoConservacao/Create

        [HttpPost]
        public ActionResult Create(EstadoConservacao EstadoConservacaoParaSalvar)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AuditoriaInterna AuditoriaEstadoConservacao = new AuditoriaInterna();

                    AuditoriaEstadoConservacao.Computador = Environment.MachineName;
                    AuditoriaEstadoConservacao.DataInsercao = DateTime.Now;
                    AuditoriaEstadoConservacao.DetalhesOperacao = "Insercao Tabela EstadoConservacao, Registro: " + EstadoConservacaoParaSalvar.Descricao;
                    AuditoriaEstadoConservacao.Tabela = "TB_EstadoConservacao";
                    AuditoriaEstadoConservacao.Usuario = User.Identity.Name;
                    AuditoriaEstadoConservacao.TipoOperacao = TipoOperacao.Insercao.ToString();
                    ContextoEstadoConservacao.Add<AuditoriaInterna>(AuditoriaEstadoConservacao);
                    ContextoEstadoConservacao.SaveChanges();

                    EstadoConservacao EstadoConservacaoSalvo = new EstadoConservacao();

                    EstadoConservacaoSalvo.Descricao = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(EstadoConservacaoParaSalvar.Descricao);
                    EstadoConservacaoSalvo.IdAuditoriaInterna = ContextoEstadoConservacao.Get<AuditoriaInterna>(AuditoriaEstadoConservacao.Id_AuditoriaInterna);
                    ContextoEstadoConservacao.Add<EstadoConservacao>(EstadoConservacaoSalvo);
                    ContextoEstadoConservacao.SaveChanges();

                    return RedirectToAction("LastCondition", EstadoConservacaoSalvo);
                }
                catch
                {

                    return View();
                }
            }
            return View();
        }

        //
        // GET: /Administracao/EstadoConservacao/Edit/5

        public ActionResult Edit(int id)
        {
            EstadoConservacao EstadoConservacaoParaEdicao = ContextoEstadoConservacao.Get<EstadoConservacao>(id);

            return View(EstadoConservacaoParaEdicao);
        }

        //
        // POST: /Administracao/EstadoConservacao/Edit/5

        [HttpPost]
        public ActionResult Edit(EstadoConservacao EstadoConservacaoParaEdicao)
        {
            try
            {
                EstadoConservacao EstadoConservacaoEditado = ContextoEstadoConservacao.Get<EstadoConservacao>(EstadoConservacaoParaEdicao.Id_EstadoConservacao);
                AuditoriaInterna AuditoriaEstadoConservacao = new AuditoriaInterna();

                AuditoriaEstadoConservacao.Computador = Environment.MachineName;
                AuditoriaEstadoConservacao.DataInsercao = DateTime.Now;
                AuditoriaEstadoConservacao.DetalhesOperacao = "Alteracao Tabela EstadoConservacao, Registro: " + EstadoConservacaoEditado.Descricao + "  Para: " + EstadoConservacaoParaEdicao.Descricao;
                AuditoriaEstadoConservacao.Tabela = "TB_EstadoConservacao";
                AuditoriaEstadoConservacao.Usuario = User.Identity.Name;
                AuditoriaEstadoConservacao.TipoOperacao = TipoOperacao.Alteracao.ToString();
                ContextoEstadoConservacao.Add<AuditoriaInterna>(AuditoriaEstadoConservacao);
                ContextoEstadoConservacao.SaveChanges();

                EstadoConservacaoEditado.Descricao = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(EstadoConservacaoParaEdicao.Descricao);
                EstadoConservacaoEditado.IdAuditoriaInterna = ContextoEstadoConservacao.Get<AuditoriaInterna>(AuditoriaEstadoConservacao.Id_AuditoriaInterna);
                TryUpdateModel<EstadoConservacao>(EstadoConservacaoEditado);
                ContextoEstadoConservacao.SaveChanges();

                return RedirectToAction("LastCondition", EstadoConservacaoEditado);
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Administracao/EstadoConservacao/Delete/5

        public ActionResult Delete(int id)
        {
            EstadoConservacao EstadoConservacaoParaExclusao = ContextoEstadoConservacao.Get<EstadoConservacao>(id);
            return View(EstadoConservacaoParaExclusao);
        }

        //
        // POST: /Administracao/EstadoConservacao/Delete/5

        [HttpPost]
        public ActionResult Delete(EstadoConservacao EstadoConservacaoParaExclusao)
        {
            try
            {
                AuditoriaInterna AuditoriaEstadoConservacao = new AuditoriaInterna();

                AuditoriaEstadoConservacao.Computador = Environment.MachineName;
                AuditoriaEstadoConservacao.DataInsercao = DateTime.Now;
                AuditoriaEstadoConservacao.DetalhesOperacao = "Exclusao Tabela EstadoConservacao, Registro: " + EstadoConservacaoParaExclusao.Descricao;
                AuditoriaEstadoConservacao.Tabela = "TB_EstadoConservacao";
                AuditoriaEstadoConservacao.Usuario = User.Identity.Name;
                AuditoriaEstadoConservacao.TipoOperacao = TipoOperacao.Exclusao.ToString();
                ContextoEstadoConservacao.Add<AuditoriaInterna>(AuditoriaEstadoConservacao);
                ContextoEstadoConservacao.SaveChanges();

                EstadoConservacao EstadoConservacaoExcluido = ContextoEstadoConservacao.Get<EstadoConservacao>(EstadoConservacaoParaExclusao.Id_EstadoConservacao);
                ContextoEstadoConservacao.Delete<EstadoConservacao>(EstadoConservacaoExcluido);
                ContextoEstadoConservacao.SaveChanges();

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

        public ActionResult LastCondition(EstadoConservacao EstadoConservacaoSalvo)
        {
            IList<EstadoConservacao> LastCondition = ContextoEstadoConservacao.GetAll<EstadoConservacao>()
                                                     .Where(x => x.Id_EstadoConservacao == EstadoConservacaoSalvo.Id_EstadoConservacao)
                                                     .ToList();

            return View(LastCondition);
        }

        public ActionResult AllCondition()
        {
            IList<EstadoConservacao> AllCondition = ContextoEstadoConservacao.GetAll<EstadoConservacao>()
                                                    .OrderBy(x => x.Descricao)
                                                    .ToList();
            return View(AllCondition);
        }
    }
}
