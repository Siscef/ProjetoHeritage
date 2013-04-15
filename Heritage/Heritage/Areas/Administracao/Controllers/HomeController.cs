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
    [Authorize(Roles = "Administrador")]
    public class HomeController : Controller
    {
        private IContextoDados ContextoBem = new ContextoDadosNH();
        //
        // GET: /Administracao/Home/

        public ActionResult Index()
        {
            
           var TodosBens = (from bem in ContextoBem.GetAll<Bem>()
                            select new { Id_Bem = bem.Id_Bem, ValorAtual = bem.ValorAtual, ValorCompra = bem.ValorCompra, ValorDepreciado = bem.ValorDepreciado, TaxaDepreciacaoAnual = bem.TaxaDepreciacaoAnual ,Descontinuado = bem.Descontinuado,Inativo = bem.Inativo }).ToList();

           var TodasDepreciacao = (from depreciacao in ContextoBem.GetAll<DepreciacaoBem>()
                                   select new { Id_DepreciacaoBem = depreciacao.Id_DepreciacaoBem, IdBem = depreciacao.IdBem, ValorCofins = depreciacao.ValorCofins, ValorDepreciado = depreciacao.ValorDepreciado ,ValorPis = depreciacao.ValorPis,DataDepreciacaoBem = depreciacao.DataDepreciacaoBem}).ToList();

            ViewBag.TotalAmortizacaoMensal = (from c in TodasDepreciacao
                                            .Where(x => x.DataDepreciacaoBem.Month == DateTime.Now.Month)
                                          select c.ValorDepreciado).Sum();
            double TotalAmortizacaoMensal = ViewBag.TotalAmortizacaoMensal;

            ViewBag.TotalAmortizacaoMensal = Math.Round(TotalAmortizacaoMensal,2);

            ViewBag.TotalPisMensal = (from c in TodasDepreciacao
                                            .Where(x => x.DataDepreciacaoBem.Month == DateTime.Now.Month)
                                      select c.ValorPis).Sum();

            double TotalPisMensal = ViewBag.TotalPisMensal;

            ViewBag.TotalPisMensal = Math.Round(TotalPisMensal, 2);

            ViewBag.TotalCofinsMensal = (from c in TodasDepreciacao
                                            .Where(x => x.DataDepreciacaoBem.Month == DateTime.Now.Month)
                                         select c.ValorCofins).Sum();
            double TotalCofinsMensal = ViewBag.TotalCofinsMensal;

            ViewBag.TotalCofinsMensal = Math.Round(TotalCofinsMensal, 2);

            ViewBag.MediaDepreciacao = (from b in TodasDepreciacao
                                        join d in TodosBens
                                        on b.IdBem.Id_Bem equals d.Id_Bem
                                        select b.ValorDepreciado).Average();
            double MediaDepreciacao = ViewBag.MediaDepreciacao;

            ViewBag.MediaDepreciacao = Math.Round(MediaDepreciacao, 2);

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



            return View();
        }

        public ActionResult Principal()
        {
            return View();
        }
    }
}
