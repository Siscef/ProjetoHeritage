using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Heritage.Models;
using Heritage.Models.ContextoBanco;

namespace Heritage.Areas.Administracao.Controllers
{
    [Authorize(Roles="Desenvolvedor, Administrador")]
    public class ConfiguracaoSistemaController : Controller
    {
        private IContextoDados ContextoConfiguracao = new ContextoDadosNH();
        public ActionResult Index()
        {
            return View();
        }

             

        //
        // GET: /Administracao/ConfiguracaoSistema/Create

        public ActionResult Create()
        {
            IList<Parametros> Parametr = (from c in ContextoConfiguracao.GetAll<Parametros>()
                                          select c).ToList();
            if (Parametr.Count() > 0)
            {
                foreach (var item in Parametr)
                {
                    return View(item);
                    
                }
                
            }

            

            return View();
        } 

        //
        // POST: /Administracao/ConfiguracaoSistema/Create

        [HttpPost]
        public ActionResult Create(Parametros ParametrosParaSalvar)
        {
            try
            {
                IList<Parametros> ListParametros = (from c in ContextoConfiguracao.GetAll<Parametros>()                                                   
                                                    select c).ToList();
                if (ListParametros.Count() > 0)
                {
                    foreach (var item in ListParametros)
                    {
                        Edit(ParametrosParaSalvar);
                        
                    }
                    
                }
                else
                {
                    ContextoConfiguracao.Add<Parametros>(ParametrosParaSalvar);
                    ContextoConfiguracao.SaveChanges();


                }
               
               
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Administracao/ConfiguracaoSistema/Edit/5
 
        public ActionResult Edit(int id)
        {
            Parametros ParametrosParaEditar = ContextoConfiguracao.Get<Parametros>(id);

            return View(ParametrosParaEditar);
        }

        //
        // POST: /Administracao/ConfiguracaoSistema/Edit/5

        [HttpPost]
        public ActionResult Edit(Parametros ParametrosParaEditar)
        {
            try
            {
                Parametros ParametrosEditados = ContextoConfiguracao.Get<Parametros>(ParametrosParaEditar.IdParametro);

                ParametrosEditados.BairroParaExibir = ParametrosParaEditar.BairroParaExibir;
                ParametrosEditados.CidadeParaExibir = ParametrosParaEditar.CidadeParaExibir;
                ParametrosEditados.CnpjParaExibir = ParametrosParaEditar.CnpjParaExibir;
                ParametrosEditados.EmailParaExibir = ParametrosParaEditar.EmailParaExibir;
                ParametrosEditados.EMatriz = ParametrosParaEditar.EMatriz;
                ParametrosEditados.EnderecoParaExibir = ParametrosParaEditar.EnderecoParaExibir;
                ParametrosEditados.EstadoParaExibir = ParametrosParaEditar.EstadoParaExibir;
                ParametrosEditados.IEParaExibir = ParametrosParaEditar.IEParaExibir;
                ParametrosEditados.NomeEmpresaParaExibir = ParametrosParaEditar.NomeEmpresaParaExibir;
                ParametrosEditados.RamoEmpresarial = ParametrosParaEditar.RamoEmpresarial;
                ParametrosEditados.TelefoneParaExibir = ParametrosParaEditar.TelefoneParaExibir;
                ParametrosEditados.VidaUtilMinima = ParametrosParaEditar.VidaUtilMinima;
                ParametrosEditados.ValorMinimoDepreciacao = ParametrosParaEditar.ValorMinimoDepreciacao;
               
               
                ContextoConfiguracao.SaveChanges();
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Administracao/ConfiguracaoSistema/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Administracao/ConfiguracaoSistema/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
