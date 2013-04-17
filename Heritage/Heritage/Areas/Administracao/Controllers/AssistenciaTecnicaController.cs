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
    public class AssistenciaTecnicaController : Controller
    {
        private IContextoDados ContextoAssistencia = new ContextoDadosNH();
        //
        // GET: /Administracao/AssistenciaTecnica/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Administracao/AssistenciaTecnica/Details/5

        public ActionResult Details(int id)
        {
            AssistenciaTecnica AssistenciaTecnicaDetails = ContextoAssistencia.Get<AssistenciaTecnica>(id);
            return View(AssistenciaTecnicaDetails);
        }

        //
        // GET: /Administracao/AssistenciaTecnica/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Administracao/AssistenciaTecnica/Create

        [HttpPost]
        public ActionResult Create(AssistenciaTecnica AssistenciaTecnicaParaSalvar)
        {
            ModelState["IdEndereco.IdBairro"].Errors.Clear();
            ModelState["IdBairro.IdCidade"].Errors.Clear();
            ModelState["IdCidade.IdEstado"].Errors.Clear();
            ModelState["IdEstado.Sigla"].Errors.Clear();
            ModelState["IdEstado.Nome"].Errors.Clear();

            if (ModelState.IsValid)
            {

                try
                {
                    string NomeCidade = AssistenciaTecnicaParaSalvar.IdCidade.Nome;
                    string SiglaEstado = AssistenciaTecnicaParaSalvar.IdEstado.Sigla;
                    string NomeBairro = AssistenciaTecnicaParaSalvar.IdBairro.Nome;
                    string DescricaoEndereco = AssistenciaTecnicaParaSalvar.IdEndereco.Descricao;
                    string CEPEndereco = AssistenciaTecnicaParaSalvar.IdEndereco.CEP;
                    if (PessoaJuridica.IsCnpj(AssistenciaTecnicaParaSalvar.CNPJ) == false)
                    {
                        ViewBag.IsNotCNPJ = "O CNPJ não é válido!";
                        return View();
                    }

                    AssistenciaTecnica AssistenciaTecnicaSalva = new AssistenciaTecnica();

                    IList<Estado> VerificarEstado = ContextoAssistencia.GetAll<Estado>().Where(x => x.Sigla == SiglaEstado).ToList();
                    if (VerificarEstado.Count() >= 1)
                    {
                        foreach (var item in VerificarEstado)
                        {
                            AssistenciaTecnicaSalva.IdEstado = ContextoAssistencia.Get<Estado>(item.Id_Estado);

                        }

                    }
                    else
                    {
                        Estado NovoEstado = new Estado();
                        NovoEstado.Nome = SiglaEstado;
                        NovoEstado.Sigla = SiglaEstado;
                        ContextoAssistencia.Add<Estado>(NovoEstado);
                        ContextoAssistencia.SaveChanges();
                        AssistenciaTecnicaSalva.IdEstado = ContextoAssistencia.Get<Estado>(NovoEstado.Id_Estado);

                    }

                    IList<Cidade> VerificarCidade = ContextoAssistencia.GetAll<Cidade>().Where(x => x.Nome == NomeCidade).ToList();
                    if (VerificarCidade.Count() >= 1)
                    {
                        foreach (var item in VerificarCidade)
                        {
                            AssistenciaTecnicaSalva.IdCidade = ContextoAssistencia.Get<Cidade>(item.Id_Cidade);
                        }
                    }
                    else
                    {
                        Cidade NovaCidade = new Cidade();
                        NovaCidade.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(NomeCidade);
                        NovaCidade.IdEstado = (from c in ContextoAssistencia.GetAll<Estado>()
                                               .Where(x => x.Sigla == SiglaEstado)
                                               select c).First();
                        ContextoAssistencia.Add<Cidade>(NovaCidade);
                        ContextoAssistencia.SaveChanges();
                        AssistenciaTecnicaSalva.IdCidade = ContextoAssistencia.Get<Cidade>(NovaCidade.Id_Cidade);
                    }

                    IList<Bairro> VerificaBairro = ContextoAssistencia.GetAll<Bairro>().Where(x => x.Nome == NomeBairro).ToList();

                    if (VerificaBairro.Count() >= 1)
                    {
                        foreach (var item in VerificaBairro)
                        {
                            AssistenciaTecnicaSalva.IdBairro = ContextoAssistencia.Get<Bairro>(item.Id_Bairro);
                        }

                    }
                    else
                    {
                        Bairro NovoBairro = new Bairro();
                        NovoBairro.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(NomeBairro);
                        NovoBairro.IdCidade = ContextoAssistencia.GetAll<Cidade>().Where(x => x.Nome == NomeCidade).First();
                        ContextoAssistencia.Add<Bairro>(NovoBairro);
                        ContextoAssistencia.SaveChanges();
                        AssistenciaTecnicaSalva.IdBairro = ContextoAssistencia.Get<Bairro>(NovoBairro.Id_Bairro);

                    }

                    IList<Endereco> VerificaEndereco = ContextoAssistencia.GetAll<Endereco>().Where(x => x.Descricao == DescricaoEndereco).ToList();

                    if (VerificaEndereco.Count() >= 1)
                    {
                        foreach (var item in VerificaEndereco)
                        {
                            AssistenciaTecnicaSalva.IdEndereco = ContextoAssistencia.Get<Endereco>(item.Id_Endereco);
                        }

                    }
                    else
                    {
                        Endereco NovoEndereco = new Endereco();
                        NovoEndereco.Descricao = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(DescricaoEndereco);
                        NovoEndereco.IdBairro = ContextoAssistencia.GetAll<Bairro>().Where(x => x.Nome == NomeBairro).First();
                        NovoEndereco.CEP = CEPEndereco;
                        ContextoAssistencia.Add<Endereco>(NovoEndereco);
                        ContextoAssistencia.SaveChanges();
                        AssistenciaTecnicaSalva.IdEndereco = ContextoAssistencia.Get<Endereco>(NovoEndereco.Id_Endereco);

                    }


                    AuditoriaInterna AuditoraAssistenciaTecnica = new AuditoriaInterna();
                    AuditoraAssistenciaTecnica.Computador = Environment.MachineName;
                    AuditoraAssistenciaTecnica.DataInsercao = DateTime.Now;
                    AuditoraAssistenciaTecnica.Usuario = User.Identity.Name;
                    AuditoraAssistenciaTecnica.DetalhesOperacao = "Insercao Tabela Assistencia Tecnica, Registro: " + AssistenciaTecnicaParaSalvar.Nome;
                    AuditoraAssistenciaTecnica.Tabela = "TB_AssistenciaTecnica";
                    AuditoraAssistenciaTecnica.TipoOperacao = TipoOperacao.Insercao.ToString();
                    ContextoAssistencia.Add<AuditoriaInterna>(AuditoraAssistenciaTecnica);
                    ContextoAssistencia.SaveChanges();

                    AssistenciaTecnicaSalva.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(AssistenciaTecnicaParaSalvar.Nome);
                    AssistenciaTecnicaSalva.RazaoSocial = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(AssistenciaTecnicaParaSalvar.RazaoSocial);
                    AssistenciaTecnicaSalva.Email = TransformaParaMaiusculo.TransformarParaMinusculo(AssistenciaTecnicaParaSalvar.Email);
                    AssistenciaTecnicaSalva.Telefone = AssistenciaTecnicaParaSalvar.Telefone;
                    AssistenciaTecnicaSalva.InscricaoEstadual = AssistenciaTecnicaParaSalvar.InscricaoEstadual;
                    AssistenciaTecnicaSalva.CNPJ = AssistenciaTecnicaParaSalvar.CNPJ;
                    AssistenciaTecnicaSalva.IdAuditoriaInterna = ContextoAssistencia.Get<AuditoriaInterna>(AuditoraAssistenciaTecnica.Id_AuditoriaInterna);

                    ContextoAssistencia.Add<AssistenciaTecnica>(AssistenciaTecnicaSalva);
                    ContextoAssistencia.SaveChanges();

                    return RedirectToAction("LastTechnicalAssistance", AssistenciaTecnicaSalva);
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        //
        // GET: /Administracao/AssistenciaTecnica/Edit/5

        public ActionResult Edit(int id)
        {
            AssistenciaTecnica AssistenciaTecnicaParaEdicao = ContextoAssistencia.Get<AssistenciaTecnica>(id);
            return View(AssistenciaTecnicaParaEdicao);
        }

        //
        // POST: /Administracao/AssistenciaTecnica/Edit/5

        [HttpPost]
        public ActionResult Edit(AssistenciaTecnica AssistenciaTecnicaParaEdicao)
        {
            try
            {
                string NomeCidade = AssistenciaTecnicaParaEdicao.IdCidade.Nome;
                string SiglaEstado = AssistenciaTecnicaParaEdicao.IdEstado.Sigla;
                string NomeBairro = AssistenciaTecnicaParaEdicao.IdBairro.Nome;
                string DescricaoEndereco = AssistenciaTecnicaParaEdicao.IdEndereco.Descricao;
                string CEPEndereco = AssistenciaTecnicaParaEdicao.IdEndereco.CEP;
                if (PessoaJuridica.IsCnpj(AssistenciaTecnicaParaEdicao.CNPJ) == false)
                {
                    ViewBag.IsNotCNPJ = "O CNPJ não é válido!";
                    return View();
                }

                AssistenciaTecnica AssistenciaTecnicaEditado = ContextoAssistencia.Get<AssistenciaTecnica>(AssistenciaTecnicaParaEdicao.Id_Pessoa);

                IList<Estado> VerificarEstado = ContextoAssistencia.GetAll<Estado>().Where(x => x.Sigla == SiglaEstado).ToList();
                if (VerificarEstado.Count() >= 1)
                {
                    foreach (var item in VerificarEstado)
                    {
                        AssistenciaTecnicaEditado.IdEstado = ContextoAssistencia.Get<Estado>(item.Id_Estado);

                    }

                }
                else
                {
                    Estado NovoEstado = new Estado();
                    NovoEstado.Nome = SiglaEstado;
                    NovoEstado.Sigla = SiglaEstado;
                    ContextoAssistencia.Add<Estado>(NovoEstado);
                    ContextoAssistencia.SaveChanges();
                    AssistenciaTecnicaEditado.IdEstado = ContextoAssistencia.Get<Estado>(NovoEstado.Id_Estado);

                }

                IList<Cidade> VerificarCidade = ContextoAssistencia.GetAll<Cidade>().Where(x => x.Nome == NomeCidade).ToList();
                if (VerificarCidade.Count() >= 1)
                {
                    foreach (var item in VerificarCidade)
                    {
                        AssistenciaTecnicaEditado.IdCidade = ContextoAssistencia.Get<Cidade>(item.Id_Cidade);
                    }
                }
                else
                {
                    Cidade NovaCidade = new Cidade();
                    NovaCidade.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(NomeCidade);
                    NovaCidade.IdEstado = (from c in ContextoAssistencia.GetAll<Estado>()
                                           .Where(x => x.Sigla == SiglaEstado)
                                           select c).First();
                    ContextoAssistencia.Add<Cidade>(NovaCidade);
                    ContextoAssistencia.SaveChanges();
                    AssistenciaTecnicaEditado.IdCidade = ContextoAssistencia.Get<Cidade>(NovaCidade.Id_Cidade);
                }

                IList<Bairro> VerificaBairro = ContextoAssistencia.GetAll<Bairro>().Where(x => x.Nome == NomeBairro).ToList();

                if (VerificaBairro.Count() >= 1)
                {
                    foreach (var item in VerificaBairro)
                    {
                        AssistenciaTecnicaEditado.IdBairro = ContextoAssistencia.Get<Bairro>(item.Id_Bairro);
                    }

                }
                else
                {
                    Bairro NovoBairro = new Bairro();
                    NovoBairro.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(NomeBairro);
                    NovoBairro.IdCidade = ContextoAssistencia.GetAll<Cidade>().Where(x => x.Nome == NomeCidade).First();
                    ContextoAssistencia.Add<Bairro>(NovoBairro);
                    ContextoAssistencia.SaveChanges();
                    AssistenciaTecnicaEditado.IdBairro = ContextoAssistencia.Get<Bairro>(NovoBairro.Id_Bairro);

                }

                IList<Endereco> VerificaEndereco = ContextoAssistencia.GetAll<Endereco>().Where(x => x.Descricao == DescricaoEndereco).ToList();

                if (VerificaEndereco.Count() >= 1)
                {
                    foreach (var item in VerificaEndereco)
                    {
                        AssistenciaTecnicaEditado.IdEndereco = ContextoAssistencia.Get<Endereco>(item.Id_Endereco);
                    }

                }
                else
                {
                    Endereco NovoEndereco = new Endereco();
                    NovoEndereco.Descricao = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(DescricaoEndereco);
                    NovoEndereco.IdBairro = ContextoAssistencia.GetAll<Bairro>().Where(x => x.Nome == NomeBairro).First();
                    NovoEndereco.CEP = CEPEndereco;
                    ContextoAssistencia.Add<Endereco>(NovoEndereco);
                    ContextoAssistencia.SaveChanges();
                    AssistenciaTecnicaEditado.IdEndereco = ContextoAssistencia.Get<Endereco>(NovoEndereco.Id_Endereco);

                }


                AuditoriaInterna AuditoraAssistenciaTecnica = new AuditoriaInterna();
                AuditoraAssistenciaTecnica.Computador = Environment.MachineName;
                AuditoraAssistenciaTecnica.DataInsercao = DateTime.Now;
                AuditoraAssistenciaTecnica.Usuario = User.Identity.Name;
                AuditoraAssistenciaTecnica.DetalhesOperacao = "Alteracao Tabela AssistenciaTecnica, Registro: " + AssistenciaTecnicaEditado.Nome;
                AuditoraAssistenciaTecnica.Tabela = "TB_AssistenciaTecnica";
                AuditoraAssistenciaTecnica.TipoOperacao = TipoOperacao.Alteracao.ToString();
                ContextoAssistencia.Add<AuditoriaInterna>(AuditoraAssistenciaTecnica);
                ContextoAssistencia.SaveChanges();
                //aqui                
                AssistenciaTecnicaEditado.CNPJ = AssistenciaTecnicaParaEdicao.CNPJ;
                AssistenciaTecnicaEditado.Email = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(AssistenciaTecnicaParaEdicao.Email);
                AssistenciaTecnicaEditado.InscricaoEstadual = AssistenciaTecnicaParaEdicao.InscricaoEstadual;
                AssistenciaTecnicaEditado.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(AssistenciaTecnicaParaEdicao.Nome);
                AssistenciaTecnicaEditado.RazaoSocial = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(AssistenciaTecnicaParaEdicao.RazaoSocial);
                AssistenciaTecnicaEditado.Telefone = AssistenciaTecnicaParaEdicao.Telefone;
                AssistenciaTecnicaEditado.IdAuditoriaInterna = ContextoAssistencia.Get<AuditoriaInterna>(AuditoraAssistenciaTecnica.Id_AuditoriaInterna);
                TryUpdateModel<AssistenciaTecnica>(AssistenciaTecnicaEditado);
                ContextoAssistencia.SaveChanges();

                return RedirectToAction("LastTechnicalAssistance", AssistenciaTecnicaEditado);
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Administracao/AssistenciaTecnica/Delete/5

        public ActionResult Delete(int id)
        {
            AssistenciaTecnica AssistenciaTecnicaParaExclusao = ContextoAssistencia.Get<AssistenciaTecnica>(id);
            return View(AssistenciaTecnicaParaExclusao);
        }

        //
        // POST: /Administracao/AssistenciaTecnica/Delete/5

        [HttpPost]
        public ActionResult Delete(AssistenciaTecnica AssistenciaTecnicaParaExcluir)
        {
            try
            {
                string NomeAssistenciaTecnica = AssistenciaTecnicaParaExcluir.RazaoSocial;
                AssistenciaTecnica AssistenciaTecnicaExcluido = ContextoAssistencia.Get<AssistenciaTecnica>(AssistenciaTecnicaParaExcluir.Id_Pessoa);
                ContextoAssistencia.Delete<AssistenciaTecnica>(AssistenciaTecnicaExcluido);
                ContextoAssistencia.SaveChanges();

                AuditoriaInterna AuditoraAssistenciaTecnica = new AuditoriaInterna();
                AuditoraAssistenciaTecnica.Computador = Environment.MachineName;
                AuditoraAssistenciaTecnica.DataInsercao = DateTime.Now;
                AuditoraAssistenciaTecnica.Usuario = User.Identity.Name;
                AuditoraAssistenciaTecnica.DetalhesOperacao = "Exclusão Tabela AssistenciaTecnica, Registro: " + NomeAssistenciaTecnica;
                AuditoraAssistenciaTecnica.Tabela = "TB_AssistenciaTecnica";
                AuditoraAssistenciaTecnica.TipoOperacao = TipoOperacao.Exclusao.ToString();
                ContextoAssistencia.Add<AuditoriaInterna>(AuditoraAssistenciaTecnica);
                ContextoAssistencia.SaveChanges();


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

        public ActionResult AllTechnicalAssistance()
        {
            IList<AssistenciaTecnica> AllTechinicalAssistance = ContextoAssistencia.GetAll<AssistenciaTecnica>()
                                                                .OrderBy(x => x.Nome)
                                                                .ToList();
            return View(AllTechinicalAssistance);
        }

        public ActionResult LastTechnicalAssistance(AssistenciaTecnica AssistenciaTecnicaSalva)
        {
            IList<AssistenciaTecnica> LastTechinical = ContextoAssistencia.GetAll<AssistenciaTecnica>()
                                                       .Where(x => x.Id_Pessoa == AssistenciaTecnicaSalva.Id_Pessoa)
                                                       .ToList();
            return View(LastTechinical);
        }
    }
}
