using Heritage.Models.ContextoBanco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Heritage.Models;
using System.Web.Security;

namespace Heritage.Areas.Administracao.Controllers
{
    [Authorize(Roles = "Administrador, Desenvolvedor")]
    public class HomeController : Controller
    {
        private IContextoDados ContextoBem = new ContextoDadosNH();
        //
        // GET: /Administracao/Home/

        public ActionResult Index()
        {
                       
           var TodosBens = (from bem in ContextoBem.GetAll<Bem>()
                            select new { Id_Bem = bem.Id_Bem, ValorAtual = bem.ValorAtual, ValorCompra = bem.ValorCompra, ValorDepreciado = bem.ValorDepreciado, TaxaDepreciacaoAnual = bem.TaxaDepreciacaoAnual ,Descontinuado = bem.Descontinuado,Inativo = bem.Inativo }).ToList();
           decimal BensParaCalculo = TodosBens.Count();

           var TodasDepreciacao = (from depreciacao in ContextoBem.GetAll<DepreciacaoBem>()
                                   select new { Id_DepreciacaoBem = depreciacao.Id_DepreciacaoBem, IdBem = depreciacao.IdBem, ValorCofins = depreciacao.ValorCofins, ValorDepreciado = depreciacao.ValorDepreciado ,ValorPis = depreciacao.ValorPis,DataDepreciacaoBem = depreciacao.DataDepreciacaoBem}).ToList();

            ViewBag.TotalAmortizacaoMensal = (from c in TodasDepreciacao
                                            .Where(x => x.DataDepreciacaoBem.Month == DateTime.Now.Month && x.DataDepreciacaoBem.Year == DateTime.Now.Year)
                                            select c.ValorDepreciado).Sum();
            double TotalAmortizacaoMensal = ViewBag.TotalAmortizacaoMensal;

            ViewBag.TotalAmortizacaoMensal = Math.Round(TotalAmortizacaoMensal,2);

            ViewBag.TotalPisMensal = (from c in TodasDepreciacao
                                            .Where(x => x.DataDepreciacaoBem.Month == DateTime.Now.Month && x.DataDepreciacaoBem.Year == DateTime.Now.Year)
                                      select c.ValorPis).Sum();

            double ValorAtivos = (from c in TodosBens
                                  .Where(x => x.Inativo == false)
                                  select c.ValorCompra).Sum();

            decimal Inativos = (from c in TodosBens
                             .Where(x => x.Inativo == true)
                                select c).Count();

            double TotalPisMensal = ViewBag.TotalPisMensal;

            ViewBag.TotalPisMensal = Math.Round(TotalPisMensal, 2);

            ViewBag.TotalCofinsMensal = (from c in TodasDepreciacao
                                            .Where(x => x.DataDepreciacaoBem.Month == DateTime.Now.Month && x.DataDepreciacaoBem.Year == DateTime.Now.Year)
                                         select c.ValorCofins).Sum();
            double TotalCofinsMensal = ViewBag.TotalCofinsMensal;

            ViewBag.TotalCofinsMensal = Math.Round(TotalCofinsMensal, 2);

            double quantidadeAtivos = (from c in TodosBens
                                       where c.Inativo == false
                                       select c).Count();
            double porcentagemAtivos = (quantidadeAtivos /TodosBens.Count()) * 100;

            ViewBag.MediaAtivos = Math.Round(porcentagemAtivos, 2);

            ViewBag.TotalBens = TodosBens.Count();
            ViewBag.TotalBensAtivos = (from b in TodosBens
                                       where b.Inativo == false
                                       select b).Count();
            ViewBag.TotalBensInativos = (from b in TodosBens
                                       where b.Inativo == true
                                       select b).Count();
            ViewBag.TotalBensObsoletos = (from b in TodosBens
                                       where b.Descontinuado == true
                                       select b).Count();

            ViewBag.ValorAtivos = Math.Round(ValorAtivos, 2) >= 0 ? Math.Round(ValorAtivos, 2) : 0;//ok
            ViewBag.PorcentagemInativos = ((BensParaCalculo * Inativos) / 100) >= 0 ? ((BensParaCalculo * Inativos) / 100) : 0;//ok

            //BemController Bem = new BemController();
            //Bem.CalculatesDepreciationOfAssets(User.Identity.Name);
            
            return View();
        }

        public ActionResult Principal()
        {
            IList<Parametros> ListaVerParametrosSistema = ContextoBem.GetAll<Parametros>()
                                                          .ToList();
            if (ListaVerParametrosSistema.Count() == 0)
            {
               return RedirectToAction("Create", "ConfiguracaoSistema");
                
            }
            else
            {
                IList<Bem> BensParaExibir = ContextoBem.GetAll<Bem>().AsParallel().ToList();
                if (BensParaExibir.Count() > 0)
                {
                    return RedirectToAction("Index");
                }
                return View();

            }
            
        }
    }
}
