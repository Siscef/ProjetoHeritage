using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Heritage.Models;
using Heritage.Models.ContextoBanco;
using System.Web.Security;

namespace Heritage.Areas.Administracao.Controllers
{
    [Authorize(Roles = "Administrador, Desenvolvedor")]
    public class CriarContaUsuarioController : Controller
    {
        private IContextoDados ContextoAccount = new ContextoDadosNH();

        // GET: /Administracao/CriarContaUsuario/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Administracao/CriarContaUsuario/Details/5

        public ActionResult Details(int id)
        {
            CriarConta CriarContaDetails = ContextoAccount.Get<CriarConta>(id);
            return View(CriarContaDetails);
        }

        //
        // GET: /Administracao/CriarContaUsuario/Create

        public ActionResult Create()
        {
            ViewBag.Papel = new SelectList(ContextoAccount.GetAll<Papel>(), "Id_Papel", "Nome");
            return View();
        }

        //
        // POST: /Administracao/CriarContaUsuario/Create

        [HttpPost]
        public ActionResult Create(CriarConta CriarContaParaSalvar)
        {
            ModelState["IdPapel.Nome"].Errors.Clear();
            if (ModelState.IsValid)
            {
                try
                {
                    MembershipCreateStatus statusCriacao;
                    Membership.CreateUser(TransformaParaMaiusculo.PrimeiraLetraMaiuscula(CriarContaParaSalvar.Nome), TransformaParaMaiusculo.PrimeiraLetraMaiuscula(CriarContaParaSalvar.Senha), CriarContaParaSalvar.Email, null, null, true, out statusCriacao);
                    if (statusCriacao == MembershipCreateStatus.Success)
                    {
                        AuditoriaInterna AuditoriaCriarConta = new AuditoriaInterna();
                        AuditoriaCriarConta.Computador = Environment.MachineName;
                        AuditoriaCriarConta.DataInsercao = DateTime.Now;
                        AuditoriaCriarConta.Usuario = User.Identity.Name;
                        AuditoriaCriarConta.DetalhesOperacao = "Insercao Tabela CriarConta, Registro: " + CriarContaParaSalvar.Nome;
                        AuditoriaCriarConta.Tabela = "TB_CriarConta";
                        AuditoriaCriarConta.TipoOperacao = TipoOperacao.Insercao.ToString();
                        ContextoAccount.Add<AuditoriaInterna>(AuditoriaCriarConta);
                        ContextoAccount.SaveChanges();

                        CriarConta CriarContaSalva = new CriarConta();

                        CriarContaSalva.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(CriarContaParaSalvar.Nome);
                        CriarContaSalva.Senha = CriarContaSalva.getMD5Hash(CriarContaParaSalvar.Senha);
                        CriarContaSalva.ConfirmeSenha = CriarContaSalva.Senha;
                        CriarContaSalva.Email = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(CriarContaParaSalvar.Email);
                        CriarContaSalva.IdPapel = ContextoAccount.Get<Papel>(CriarContaParaSalvar.IdPapel.Id_Papel);
                        CriarContaSalva.IdAuditoriaInterna = ContextoAccount.Get<AuditoriaInterna>(AuditoriaCriarConta.Id_AuditoriaInterna);

                        ContextoAccount.Add<CriarConta>(CriarContaSalva);
                        ContextoAccount.SaveChanges();
                        string NomePapel = RetornaNomePapel(CriarContaParaSalvar.IdPapel.Id_Papel);

                        Roles.AddUserToRole(CriarContaSalva.Nome, NomePapel);
                        ContextoAccount.SaveChanges();

                        return RedirectToAction("Index", "Home");

                    }

                }
                catch
                {
                    ViewBag.Papel = new SelectList(ContextoAccount.GetAll<Papel>(), "Id_Papel", "Nome", CriarContaParaSalvar.IdPapel);
                    return View();
                }
            }
            ViewBag.Papel = new SelectList(ContextoAccount.GetAll<Papel>(), "Id_Papel", "Nome", CriarContaParaSalvar.IdPapel);
            return View();
        }

        //
        // GET: /Administracao/CriarContaUsuario/Edit/5

        public ActionResult Edit(int id)
        {
            CriarConta CriarContaEdit = ContextoAccount.Get<CriarConta>(id);
            ViewBag.Papel = new SelectList(ContextoAccount.GetAll<Papel>(), "Id_Papel", "Nome");
            return View(CriarContaEdit);
        }

        //
        // POST: /Administracao/CriarContaUsuario/Edit/5

        [HttpPost]
        public ActionResult Edit(CriarConta CriarContaParaEdicao)
        {
            ModelState["IdPapel.Nome"].Errors.Clear();
            ModelState["Senha"].Errors.Clear();
            ModelState["ConfirmeSenha"].Errors.Clear();
            if (ModelState.IsValid)
            {
                try
                {
                    CriarConta CriarContaEditada = ContextoAccount.Get<CriarConta>(CriarContaParaEdicao.Id_CriarConta);
                    Membership.DeleteUser(CriarContaEditada.Nome);

                    MembershipCreateStatus status;
                    Membership.CreateUser(TransformaParaMaiusculo.PrimeiraLetraMaiuscula(CriarContaParaEdicao.Nome), CriarContaParaEdicao.Senha, TransformaParaMaiusculo.PrimeiraLetraMaiuscula(CriarContaParaEdicao.Email), null, null, true, out status);

                    if (status == MembershipCreateStatus.Success)
                    {
                        AuditoriaInterna AuditoriaCriarConta = new AuditoriaInterna();
                        AuditoriaCriarConta.Computador = Environment.MachineName;
                        AuditoriaCriarConta.DataInsercao = DateTime.Now;
                        AuditoriaCriarConta.Usuario = User.Identity.Name;
                        AuditoriaCriarConta.DetalhesOperacao = "Alteracao Tabela CriarConta, Registro: " + CriarContaEditada.Nome + " Para: " + CriarContaParaEdicao.Nome;
                        AuditoriaCriarConta.Tabela = "TB_CriarConta";
                        AuditoriaCriarConta.TipoOperacao = TipoOperacao.Alteracao.ToString();
                        ContextoAccount.Add<AuditoriaInterna>(AuditoriaCriarConta);
                        ContextoAccount.SaveChanges();

                        CriarContaEditada.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(CriarContaParaEdicao.Nome);
                        CriarContaEditada.Senha = CriarContaEditada.getMD5Hash(CriarContaParaEdicao.Senha);
                        CriarContaEditada.ConfirmeSenha = CriarContaEditada.Senha;
                        CriarContaEditada.Email = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(CriarContaParaEdicao.Email);
                        CriarContaEditada.IdPapel = ContextoAccount.Get<Papel>(CriarContaParaEdicao.IdPapel.Id_Papel);
                        CriarContaEditada.IdAuditoriaInterna = ContextoAccount.Get<AuditoriaInterna>(AuditoriaCriarConta.Id_AuditoriaInterna);

                        TryUpdateModel<CriarConta>(CriarContaEditada);
                        ContextoAccount.SaveChanges();

                        string NomePapelNovo = RetornaNomePapel(CriarContaEditada.IdPapel.Id_Papel);

                        Roles.AddUserToRole(CriarContaEditada.Nome, NomePapelNovo);
                        ContextoAccount.SaveChanges();


                    }

                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    ViewBag.Papel = new SelectList(ContextoAccount.GetAll<CriarConta>(), "Id_Papel", "Nome", CriarContaParaEdicao.IdPapel);
                    return View();
                }

            }
            ViewBag.Papel = new SelectList(ContextoAccount.GetAll<CriarConta>(), "Id_Papel", "Nome", CriarContaParaEdicao.IdPapel);
            return View();


        }

        //
        // GET: /Administracao/CriarContaUsuario/Delete/5

        public ActionResult Delete(int id)
        {
            CriarConta CriarContaParaExclusao = ContextoAccount.Get<CriarConta>(id);
            return View(CriarContaParaExclusao);
        }

        //
        // POST: /Administracao/CriarContaUsuario/Delete/5

        [HttpPost]
        public ActionResult Delete(CriarConta CriarContaParaExcluir)
        {
            try
            {
                CriarConta CriarContaExcluida = ContextoAccount.Get<CriarConta>(CriarContaParaExcluir.Id_CriarConta);

                AuditoriaInterna AuditoriaCriarConta = new AuditoriaInterna();
                AuditoriaCriarConta.Computador = Environment.MachineName;
                AuditoriaCriarConta.DataInsercao = DateTime.Now;
                AuditoriaCriarConta.Usuario = User.Identity.Name;
                AuditoriaCriarConta.DetalhesOperacao = "Exclusao Tabela CriarConta, Registro: " + CriarContaExcluida.Nome;
                AuditoriaCriarConta.Tabela = "TB_CriarConta";
                AuditoriaCriarConta.TipoOperacao = TipoOperacao.Exclusao.ToString();
                ContextoAccount.Add<AuditoriaInterna>(AuditoriaCriarConta);
                ContextoAccount.SaveChanges();

                string NomePapel = RetornaNomePapel(CriarContaExcluida.IdPapel.Id_Papel);

                Membership.DeleteUser(CriarContaExcluida.Nome);
                ContextoAccount.SaveChanges();

                ContextoAccount.Delete<CriarConta>(CriarContaExcluida);
                ContextoAccount.SaveChanges();


                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AllAccount()
        {
            List<CriarConta> All = ContextoAccount.GetAll<CriarConta>()
                                   .OrderBy(x => x.Nome)
                                   .ToList();
            return View(All);
        }

        public ActionResult LastAccount(CriarConta CriarContaSalva)
        {
            List<CriarConta> Last = ContextoAccount.GetAll<CriarConta>()
                                     .Where(x => x.Id_CriarConta == CriarContaSalva.Id_CriarConta)
                                     .ToList();

            return View(Last);
        }

        private string RetornaNomePapel(long Id)
        {
            string NomePapel = (from c in ContextoAccount.GetAll<Papel>()
                                .Where(x => x.Id_Papel == Id)
                                select c.Nome).First();
            return NomePapel;
        }
    }
}
