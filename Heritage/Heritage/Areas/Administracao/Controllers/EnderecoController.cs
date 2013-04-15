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
    public class EnderecoController : Controller
    {
        private IContextoDados ContextoEndereco = new ContextoDadosNH();
        //
        // GET: /Administracao/Endereco/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Administracao/Endereco/Details/5

        public ActionResult Details(int id)
        {
            Endereco EnderecoDetails = ContextoEndereco.Get<Endereco>(id);
            return View(EnderecoDetails);
        }

        //
        // GET: /Administracao/Endereco/Create

        public ActionResult Create()
        {
            ViewBag.Bairro = new SelectList(ContextoEndereco.GetAll<Bairro>(), "Id_Bairro", "Nome");
            return View();
        }

        //
        // POST: /Administracao/Endereco/Create

        [HttpPost]
        public ActionResult Create(Endereco EnderecoParaSalvar)
        {
            lock (ContextoEndereco)
            {
                //ModelState[].Errors.Clear();
                ModelState["IdBairro.IdCidade"].Errors.Clear();
                ModelState["IdBairro.Nome"].Errors.Clear();
                if (ModelState.IsValid)
                {


                    AuditoriaInterna AuditoriaInternaEndereco = new AuditoriaInterna();
                    AuditoriaInternaEndereco.Computador = Environment.MachineName;
                    AuditoriaInternaEndereco.DataInsercao = DateTime.Now;
                    AuditoriaInternaEndereco.Usuario = User.Identity.Name;
                    AuditoriaInternaEndereco.Tabela = "TB_Endereco";
                    AuditoriaInternaEndereco.TipoOperacao = TipoOperacao.Insercao.ToString();
                    AuditoriaInternaEndereco.DetalhesOperacao = "Insercao Tabela Endereco, Registro: " + EnderecoParaSalvar.Descricao;
                    ContextoEndereco.Add<AuditoriaInterna>(AuditoriaInternaEndereco);
                    ContextoEndereco.SaveChanges();


                    Endereco EnderecoSalvo = new Endereco();
                    EnderecoSalvo.CEP = EnderecoParaSalvar.CEP;
                    EnderecoSalvo.Descricao = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(EnderecoParaSalvar.Descricao);
                    EnderecoSalvo.IdBairro = ContextoEndereco.Get<Bairro>(EnderecoParaSalvar.IdBairro.Id_Bairro);
                    EnderecoSalvo.IdAuditoriaInterna = ContextoEndereco.Get<AuditoriaInterna>(AuditoriaInternaEndereco.Id_AuditoriaInterna);
                    ContextoEndereco.Add<Endereco>(EnderecoSalvo);
                    ContextoEndereco.SaveChanges();


                    //Auditoria Geral
                   
                    return RedirectToAction("LastAdress", EnderecoSalvo);
                }
                ViewBag.Bairro = new SelectList(ContextoEndereco.GetAll<Bairro>(), "Id_Bairro", "Nome", EnderecoParaSalvar.IdBairro);
                return View();
            }
        }

        //
        // GET: /Administracao/Endereco/Edit/5

        public ActionResult Edit(int id)
        {
            Endereco EnderecoParaEditar = ContextoEndereco.Get<Endereco>(id);
            ViewBag.Bairro = new SelectList(ContextoEndereco.GetAll<Bairro>(), "Id_Bairro", "Nome");
            return View(EnderecoParaEditar);
        }

        //
        // POST: /Administracao/Endereco/Edit/5

        [HttpPost]
        public ActionResult Edit(Endereco EnderecoParaEdicao)
        {
            lock (ContextoEndereco)
            {

                AuditoriaInterna AuditoriaInternaEndereco = new AuditoriaInterna();
                AuditoriaInternaEndereco.Computador = Environment.MachineName;
                AuditoriaInternaEndereco.DataInsercao = DateTime.Now;
                AuditoriaInternaEndereco.Usuario = User.Identity.Name;
                AuditoriaInternaEndereco.DetalhesOperacao = "Alteração Tabela Endereco, Registro: " + EnderecoParaEdicao.Descricao;
                AuditoriaInternaEndereco.Tabela = "TB_Endereco";
                AuditoriaInternaEndereco.TipoOperacao = TipoOperacao.Alteracao.ToString();
                ContextoEndereco.Add<AuditoriaInterna>(AuditoriaInternaEndereco);
                ContextoEndereco.SaveChanges();

                Endereco EnderecoEditado = ContextoEndereco.Get<Endereco>(EnderecoParaEdicao.Id_Endereco);
                EnderecoEditado.CEP = EnderecoParaEdicao.CEP;
                EnderecoEditado.Descricao = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(EnderecoParaEdicao.Descricao);
                EnderecoEditado.IdBairro = ContextoEndereco.Get<Bairro>(EnderecoParaEdicao.IdBairro.Id_Bairro);
                EnderecoEditado.IdAuditoriaInterna = ContextoEndereco.Get<AuditoriaInterna>(AuditoriaInternaEndereco.Id_AuditoriaInterna);
                ContextoEndereco.Add<Endereco>(EnderecoEditado);
                ContextoEndereco.SaveChanges();

                
                return RedirectToAction("LastAdress", EnderecoEditado);
            }
        }

        //
        // GET: /Administracao/Endereco/Delete/5

        public ActionResult Delete(int id)
        {
            Endereco EnderecoParaExcluir = ContextoEndereco.Get<Endereco>(id);

            return View(EnderecoParaExcluir);
        }

        //
        // POST: /Administracao/Endereco/Delete/5

        [HttpPost]
        public ActionResult Delete(Endereco EnderecoParaExcluir)
        {
            AuditoriaInterna AuditoriaInternaEndereco = new AuditoriaInterna();
            AuditoriaInternaEndereco.Computador = Environment.MachineName;
            AuditoriaInternaEndereco.DataInsercao = DateTime.Now;
            AuditoriaInternaEndereco.Usuario = User.Identity.Name;
            AuditoriaInternaEndereco.DetalhesOperacao = "Exclusao Tabela Endereco, Registro: " + EnderecoParaExcluir.Descricao;
            AuditoriaInternaEndereco.Tabela = "TB_Endereco";
            AuditoriaInternaEndereco.TipoOperacao = TipoOperacao.Exclusao.ToString();
            ContextoEndereco.Add<AuditoriaInterna>(AuditoriaInternaEndereco);
            ContextoEndereco.SaveChanges();

           
            Endereco EnderecoExcluido = ContextoEndereco.Get<Endereco>(EnderecoParaExcluir.Id_Endereco);
            ContextoEndereco.Delete<Endereco>(EnderecoExcluido);
            ContextoEndereco.SaveChanges();
                     

            return View("Index");
        }

        public ActionResult LastAdress(Endereco EnderecoSalvo)
        {
            IList<Endereco> ListEnderecoLast = ContextoEndereco.GetAll<Endereco>()
                                              .Where(x => x.Id_Endereco == EnderecoSalvo.Id_Endereco).OrderBy(x => x.Descricao)
                                              .ToList();
            return View(ListEnderecoLast);
        }


    }
}
