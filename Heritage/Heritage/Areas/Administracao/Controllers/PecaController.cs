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
    public class PecaController : Controller
    {
        private IContextoDados ContextoPeca = new ContextoDadosNH();
        //
        // GET: /Administracao/Peca/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Administracao/Peca/Details/5

        public ActionResult Details(int id)
        {
            Peca PecaDetails = ContextoPeca.Get<Peca>(id);
            return View(PecaDetails);
        }

        //
        // GET: /Administracao/Peca/Create

        public ActionResult Create()
        {
            ViewBag.Fornecedor = new SelectList(ContextoPeca.GetAll<Fornecedor>().OrderBy(x => x.RazaoSocial), "Id_Pessoa", "RazaoSocial");
            return View();
        }

        //
        // POST: /Administracao/Peca/Create

        [HttpPost]
        public ActionResult Create(Peca PecaParaSalvar)
        {
            ModelState["IdFornecedor.CNPJ"].Errors.Clear();
            ModelState["IdFornecedor.RazaoSocial"].Errors.Clear();
            ModelState["IdFornecedor.InscricaoEstadual"].Errors.Clear();
            ModelState["IdFornecedor.Nome"].Errors.Clear();
            ModelState["IdFornecedor.Email"].Errors.Clear();
            ModelState["IdFornecedor.Telefone"].Errors.Clear();
            ModelState["IdFornecedor.IdEndereco"].Errors.Clear();
            ModelState["IdFornecedor.IdBairro"].Errors.Clear();
            ModelState["IdFornecedor.IdCidade"].Errors.Clear();
            ModelState["IdFornecedor.IdEstado"].Errors.Clear();
            if (ModelState.IsValid)
            {


                try
                {
                    AuditoriaInterna AuditoraPeca = new AuditoriaInterna();
                    AuditoraPeca.Computador = Environment.MachineName;
                    AuditoraPeca.DataInsercao = DateTime.Now;
                    AuditoraPeca.Usuario = User.Identity.Name;
                    AuditoraPeca.DetalhesOperacao = "Insercao Tabela Peca, Registro: " + PecaParaSalvar.Descricao;
                    AuditoraPeca.Tabela = "TB_Peca";
                    AuditoraPeca.TipoOperacao = TipoOperacao.Insercao.ToString();
                    ContextoPeca.Add<AuditoriaInterna>(AuditoraPeca);
                    ContextoPeca.SaveChanges();

                    Peca PecaSalva = new Peca();

                    PecaSalva.Descricao = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(PecaParaSalvar.Descricao);
                    PecaSalva.AcrescentaValorAoBem = PecaParaSalvar.AcrescentaValorAoBem;
                    PecaSalva.IdFornecedor = ContextoPeca.Get<Fornecedor>(PecaParaSalvar.IdFornecedor.Id_Pessoa);
                    PecaSalva.Valor = PecaParaSalvar.Valor;
                    PecaSalva.IdAuditoriaInterna = ContextoPeca.Get<AuditoriaInterna>(AuditoraPeca.Id_AuditoriaInterna);

                    ContextoPeca.Add<Peca>(PecaSalva);
                    ContextoPeca.SaveChanges();


                    return RedirectToAction("LastSins",PecaSalva);
                }
                catch
                {
                    ViewBag.Fornecedor = new SelectList(ContextoPeca.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", PecaParaSalvar.IdFornecedor);
                    return View();
                }
            }
            ViewBag.Fornecedor = new SelectList(ContextoPeca.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", PecaParaSalvar.IdFornecedor);
            return View();
        }

        //
        // GET: /Administracao/Peca/Edit/5

        public ActionResult Edit(int id)
        {
            Peca PecaParaEdicao = ContextoPeca.Get<Peca>(id);
            ViewBag.Fornecedor = new SelectList(ContextoPeca.GetAll<Fornecedor>().OrderBy(x => x.RazaoSocial), "Id_Pessoa", "RazaoSocial");
            return View(PecaParaEdicao);
        }

        //
        // POST: /Administracao/Peca/Edit/5

        [HttpPost]
        public ActionResult Edit(Peca PecaParaEdicao)
        {
            ModelState["IdFornecedor.CNPJ"].Errors.Clear();
            ModelState["IdFornecedor.RazaoSocial"].Errors.Clear();
            ModelState["IdFornecedor.InscricaoEstadual"].Errors.Clear();
            ModelState["IdFornecedor.Nome"].Errors.Clear();
            ModelState["IdFornecedor.Email"].Errors.Clear();
            ModelState["IdFornecedor.Telefone"].Errors.Clear();
            ModelState["IdFornecedor.IdEndereco"].Errors.Clear();
            ModelState["IdFornecedor.IdBairro"].Errors.Clear();
            ModelState["IdFornecedor.IdCidade"].Errors.Clear();
            ModelState["IdFornecedor.IdEstado"].Errors.Clear();

            if (ModelState.IsValid)
            {


                try
                {
                    Peca PecaEditada = ContextoPeca.Get<Peca>(PecaParaEdicao.Id_Peca);

                    AuditoriaInterna AuditoraPeca = new AuditoriaInterna();
                    AuditoraPeca.Computador = Environment.MachineName;
                    AuditoraPeca.DataInsercao = DateTime.Now;
                    AuditoraPeca.Usuario = User.Identity.Name;
                    AuditoraPeca.DetalhesOperacao = "Alteracao Tabela Peca, Registro: " + PecaEditada.Descricao + " Para: " + PecaParaEdicao.Descricao;
                    AuditoraPeca.Tabela = "TB_Peca";
                    AuditoraPeca.TipoOperacao = TipoOperacao.Alteracao.ToString();
                    ContextoPeca.Add<AuditoriaInterna>(AuditoraPeca);
                    ContextoPeca.SaveChanges();


                    PecaEditada.Descricao = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(PecaParaEdicao.Descricao);
                    PecaEditada.AcrescentaValorAoBem = PecaParaEdicao.AcrescentaValorAoBem;
                    PecaEditada.Valor = PecaParaEdicao.Valor;
                    PecaEditada.IdAuditoriaInterna = ContextoPeca.Get<AuditoriaInterna>(AuditoraPeca.Id_AuditoriaInterna);
                    PecaEditada.IdFornecedor = ContextoPeca.Get<Fornecedor>(PecaParaEdicao.IdFornecedor.Id_Pessoa);

                    TryUpdateModel<Peca>(PecaEditada);
                    ContextoPeca.SaveChanges();

                    return RedirectToAction("LastSins",PecaEditada);
                }
                catch
                {
                    ViewBag.Fornecedor = new SelectList(ContextoPeca.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", PecaParaEdicao.IdFornecedor);
                    return View();
                }
            }
            ViewBag.Fornecedor = new SelectList(ContextoPeca.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", PecaParaEdicao.IdFornecedor);
            return View();
        }

        //
        // GET: /Administracao/Peca/Delete/5

        public ActionResult Delete(int id)
        {
            Peca PecaParaExclusao = ContextoPeca.Get<Peca>(id);
            return View(PecaParaExclusao);
        }

        //
        // POST: /Administracao/Peca/Delete/5

        [HttpPost]
        public ActionResult Delete(Peca PecaParaExcluir)
        {
            try
            {
                Peca PecaExcluida = ContextoPeca.Get<Peca>(PecaParaExcluir.Id_Peca);

                string Descricao = PecaParaExcluir.Descricao;

                AuditoriaInterna AuditoraPeca = new AuditoriaInterna();

                AuditoraPeca.Computador = Environment.MachineName;
                AuditoraPeca.DataInsercao = DateTime.Now;
                AuditoraPeca.Usuario = User.Identity.Name;
                AuditoraPeca.DetalhesOperacao = "Exclusao Tabela Peca, Registro: " + Descricao;
                AuditoraPeca.Tabela = "TB_Peca";
                AuditoraPeca.TipoOperacao = TipoOperacao.Exclusao.ToString();
                ContextoPeca.Add<AuditoriaInterna>(AuditoraPeca);
                ContextoPeca.SaveChanges();

                ContextoPeca.Delete<Peca>(PecaExcluida);
                ContextoPeca.SaveChanges();

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
        public ActionResult LastSins(Peca PecaSalva)
        {
            IList<Peca> Sins = ContextoPeca.GetAll<Peca>()
                               .Where(x => x.Id_Peca == PecaSalva.Id_Peca)
                               .ToList();
            return View(Sins);
        }

        public ActionResult AllSins()
        {
            IList<Peca> All = ContextoPeca.GetAll<Peca>()
                              .OrderBy(x => x.Descricao)
                              .ToList();
            return View(All);
        }
    }
}
