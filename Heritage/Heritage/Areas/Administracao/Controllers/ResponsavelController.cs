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
    public class ResponsavelController : Controller
    {
        private IContextoDados ContextoResponsavel = new ContextoDadosNH();
        // GET: /Administracao/Responsavel/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Administracao/Responsavel/Details/5

        public ActionResult Details(int id)
        {
            Responsavel ResponsavelDetails = ContextoResponsavel.Get<Responsavel>(id);
            return View(ResponsavelDetails);
        }

        //
        // GET: /Administracao/Responsavel/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Administracao/Responsavel/Create

        [HttpPost]
        public ActionResult Create(Responsavel ResponsavelParaSalvar)
        {
            //8 10 12 14
            ModelState["IdEndereco.IdBairro"].Errors.Clear();
            ModelState["IdBairro.IdCidade"].Errors.Clear();
            ModelState["IdCidade.IdEstado"].Errors.Clear();
            ModelState["IdEstado.Sigla"].Errors.Clear();
            ModelState["IdEstado.Nome"].Errors.Clear();

            if (ModelState.IsValid)
            {
                try
                {
                    string NomeCidade = ResponsavelParaSalvar.IdCidade.Nome;
                    string SiglaEstado = ResponsavelParaSalvar.IdEstado.Sigla;
                    string NomeBairro = ResponsavelParaSalvar.IdBairro.Nome;
                    string DescricaoEndereco = ResponsavelParaSalvar.IdEndereco.Descricao;
                    string CEPEndereco = ResponsavelParaSalvar.IdEndereco.CEP;

                    Responsavel ResponsavelSalvo = new Responsavel();
                    IList<Estado> VerificarEstado = ContextoResponsavel.GetAll<Estado>().Where(x => x.Sigla == SiglaEstado).ToList();
                    if (VerificarEstado.Count() >= 1)
                    {
                        foreach (var item in VerificarEstado)
                        {
                            ResponsavelSalvo.IdEstado = ContextoResponsavel.Get<Estado>(item.Id_Estado);

                        }

                    }
                    else
                    {
                        Estado NovoEstado = new Estado();
                        NovoEstado.Nome = SiglaEstado;
                        NovoEstado.Sigla = SiglaEstado;
                        ContextoResponsavel.Add<Estado>(NovoEstado);
                        ContextoResponsavel.SaveChanges();
                        ResponsavelSalvo.IdEstado = ContextoResponsavel.Get<Estado>(NovoEstado.Id_Estado);

                    }

                    IList<Cidade> VerificarCidade = ContextoResponsavel.GetAll<Cidade>().Where(x => x.Nome == NomeCidade).ToList();
                    if (VerificarCidade.Count() >= 1)
                    {
                        foreach (var item in VerificarCidade)
                        {
                            ResponsavelSalvo.IdCidade = ContextoResponsavel.Get<Cidade>(item.Id_Cidade);
                        }
                    }
                    else
                    {
                        Cidade NovaCidade = new Cidade();
                        NovaCidade.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(NomeCidade);
                        NovaCidade.IdEstado = (from c in ContextoResponsavel.GetAll<Estado>()
                                               .Where(x => x.Sigla == SiglaEstado)
                                               select c).First();
                        ContextoResponsavel.Add<Cidade>(NovaCidade);
                        ContextoResponsavel.SaveChanges();
                        ResponsavelSalvo.IdCidade = ContextoResponsavel.Get<Cidade>(NovaCidade.Id_Cidade);
                    }

                    IList<Bairro> VerificaBairro = ContextoResponsavel.GetAll<Bairro>().Where(x => x.Nome == NomeBairro).ToList();

                    if (VerificaBairro.Count() >= 1)
                    {
                        foreach (var item in VerificaBairro)
                        {
                            ResponsavelSalvo.IdBairro = ContextoResponsavel.Get<Bairro>(item.Id_Bairro);
                        }

                    }
                    else
                    {
                        Bairro NovoBairro = new Bairro();
                        NovoBairro.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(NomeBairro);
                        NovoBairro.IdCidade = ContextoResponsavel.GetAll<Cidade>().Where(x => x.Nome == NomeCidade).First();
                        ContextoResponsavel.Add<Bairro>(NovoBairro);
                        ContextoResponsavel.SaveChanges();
                        ResponsavelSalvo.IdBairro = ContextoResponsavel.Get<Bairro>(NovoBairro.Id_Bairro);

                    }

                    IList<Endereco> VerificaEndereco = ContextoResponsavel.GetAll<Endereco>().Where(x => x.Descricao == DescricaoEndereco).ToList();

                    if (VerificaEndereco.Count() >= 1)
                    {
                        foreach (var item in VerificaEndereco)
                        {
                            ResponsavelSalvo.IdEndereco = ContextoResponsavel.Get<Endereco>(item.Id_Endereco);
                        }

                    }
                    else
                    {
                        Endereco NovoEndereco = new Endereco();
                        NovoEndereco.Descricao = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(DescricaoEndereco);
                        NovoEndereco.IdBairro = ContextoResponsavel.GetAll<Bairro>().Where(x => x.Nome == NomeBairro).First();
                        NovoEndereco.CEP = CEPEndereco;
                        ContextoResponsavel.Add<Endereco>(NovoEndereco);
                        ContextoResponsavel.SaveChanges();
                        ResponsavelSalvo.IdEndereco = ContextoResponsavel.Get<Endereco>(NovoEndereco.Id_Endereco);

                    }


                    AuditoriaInterna AuditoraResponsavel = new AuditoriaInterna();
                    AuditoraResponsavel.Computador = Environment.MachineName;
                    AuditoraResponsavel.DataInsercao = DateTime.Now;
                    AuditoraResponsavel.Usuario = User.Identity.Name;
                    AuditoraResponsavel.DetalhesOperacao = "Insercao Tabela Responsavel, Registro: " + ResponsavelParaSalvar.Nome;
                    AuditoraResponsavel.Tabela = "TB_Responsavel";
                    AuditoraResponsavel.TipoOperacao = TipoOperacao.Insercao.ToString();
                    ContextoResponsavel.Add<AuditoriaInterna>(AuditoraResponsavel);
                    ContextoResponsavel.SaveChanges();


                    ResponsavelSalvo.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(ResponsavelParaSalvar.Nome);
                    ResponsavelSalvo.DataNascimento = ResponsavelParaSalvar.DataNascimento;
                    ResponsavelSalvo.Email = TransformaParaMaiusculo.TransformarParaMinusculo(ResponsavelParaSalvar.Email);
                    ResponsavelSalvo.Telefone = ResponsavelParaSalvar.Telefone;
                    ResponsavelSalvo.IdAuditoriaInterna = ContextoResponsavel.Get<AuditoriaInterna>(AuditoraResponsavel.Id_AuditoriaInterna);

                    ContextoResponsavel.Add<Responsavel>(ResponsavelSalvo);
                    ContextoResponsavel.SaveChanges();


                    return RedirectToAction("LastResponsible", ResponsavelSalvo);
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        //
        // GET: /Administracao/Responsavel/Edit/5

        public ActionResult Edit(int id)
        {
            Responsavel ResponsavelParaEdicao = ContextoResponsavel.Get<Responsavel>(id);
            return View(ResponsavelParaEdicao);
        }

        //
        // POST: /Administracao/Responsavel/Edit/5

        [HttpPost]
        public ActionResult Edit(Responsavel ResponsavelParaEdicao)
        {
            try
            {
                string NomeCidade = ResponsavelParaEdicao.IdCidade.Nome;
                string SiglaEstado = ResponsavelParaEdicao.IdEstado.Sigla;
                string NomeBairro = ResponsavelParaEdicao.IdBairro.Nome;
                string DescricaoEndereco = ResponsavelParaEdicao.IdEndereco.Descricao;
                string CEPEndereco = ResponsavelParaEdicao.IdEndereco.CEP;

                Responsavel ResponsavelEditado = ContextoResponsavel.Get<Responsavel>(ResponsavelParaEdicao.Id_Pessoa);

                IList<Estado> VerificarEstado = ContextoResponsavel.GetAll<Estado>().Where(x => x.Sigla == SiglaEstado).ToList();
                if (VerificarEstado.Count() >= 1)
                {
                    foreach (var item in VerificarEstado)
                    {
                        ResponsavelEditado.IdEstado = ContextoResponsavel.Get<Estado>(item.Id_Estado);

                    }

                }
                else
                {
                    Estado NovoEstado = new Estado();
                    NovoEstado.Nome = SiglaEstado;
                    NovoEstado.Sigla = SiglaEstado;
                    ContextoResponsavel.Add<Estado>(NovoEstado);
                    ContextoResponsavel.SaveChanges();
                    ResponsavelEditado.IdEstado = ContextoResponsavel.Get<Estado>(NovoEstado.Id_Estado);

                }

                IList<Cidade> VerificarCidade = ContextoResponsavel.GetAll<Cidade>().Where(x => x.Nome == NomeCidade).ToList();
                if (VerificarCidade.Count() >= 1)
                {
                    foreach (var item in VerificarCidade)
                    {
                        ResponsavelEditado.IdCidade = ContextoResponsavel.Get<Cidade>(item.Id_Cidade);
                    }
                }
                else
                {
                    Cidade NovaCidade = new Cidade();
                    NovaCidade.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(NomeCidade);
                    NovaCidade.IdEstado = (from c in ContextoResponsavel.GetAll<Estado>()
                                           .Where(x => x.Sigla == SiglaEstado)
                                           select c).First();
                    ContextoResponsavel.Add<Cidade>(NovaCidade);
                    ContextoResponsavel.SaveChanges();
                    ResponsavelEditado.IdCidade = ContextoResponsavel.Get<Cidade>(NovaCidade.Id_Cidade);
                }

                IList<Bairro> VerificaBairro = ContextoResponsavel.GetAll<Bairro>().Where(x => x.Nome == NomeBairro).ToList();

                if (VerificaBairro.Count() >= 1)
                {
                    foreach (var item in VerificaBairro)
                    {
                        ResponsavelEditado.IdBairro = ContextoResponsavel.Get<Bairro>(item.Id_Bairro);
                    }

                }
                else
                {
                    Bairro NovoBairro = new Bairro();
                    NovoBairro.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(NomeBairro);
                    NovoBairro.IdCidade = ContextoResponsavel.GetAll<Cidade>().Where(x => x.Nome == NomeCidade).First();
                    ContextoResponsavel.Add<Bairro>(NovoBairro);
                    ContextoResponsavel.SaveChanges();
                    ResponsavelEditado.IdBairro = ContextoResponsavel.Get<Bairro>(NovoBairro.Id_Bairro);

                }

                IList<Endereco> VerificaEndereco = ContextoResponsavel.GetAll<Endereco>().Where(x => x.Descricao == DescricaoEndereco).ToList();

                if (VerificaEndereco.Count() >= 1)
                {
                    foreach (var item in VerificaEndereco)
                    {
                        ResponsavelEditado.IdEndereco = ContextoResponsavel.Get<Endereco>(item.Id_Endereco);
                    }

                }
                else
                {
                    Endereco NovoEndereco = new Endereco();
                    NovoEndereco.Descricao = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(DescricaoEndereco);
                    NovoEndereco.IdBairro = ContextoResponsavel.GetAll<Bairro>().Where(x => x.Nome == NomeBairro).First();
                    NovoEndereco.CEP = CEPEndereco;
                    ContextoResponsavel.Add<Endereco>(NovoEndereco);
                    ContextoResponsavel.SaveChanges();
                    ResponsavelEditado.IdEndereco = ContextoResponsavel.Get<Endereco>(NovoEndereco.Id_Endereco);

                }


                AuditoriaInterna AuditoraResponsavel = new AuditoriaInterna();
                AuditoraResponsavel.Computador = Environment.MachineName;
                AuditoraResponsavel.DataInsercao = DateTime.Now;
                AuditoraResponsavel.Usuario = User.Identity.Name;
                AuditoraResponsavel.DetalhesOperacao = "Alteracao Tabela Responsavel, Registro: " + ResponsavelEditado.Nome;
                AuditoraResponsavel.Tabela = "TB_Responsavel";
                AuditoraResponsavel.TipoOperacao = TipoOperacao.Alteracao.ToString();
                ContextoResponsavel.Add<AuditoriaInterna>(AuditoraResponsavel);
                ContextoResponsavel.SaveChanges();
                //aqui                

                ResponsavelEditado.Email = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(ResponsavelParaEdicao.Email);
                ResponsavelEditado.DataNascimento = ResponsavelParaEdicao.DataNascimento;
                ResponsavelEditado.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(ResponsavelParaEdicao.Nome);
                ResponsavelEditado.Telefone = ResponsavelParaEdicao.Telefone;
                ResponsavelEditado.IdAuditoriaInterna = ContextoResponsavel.Get<AuditoriaInterna>(AuditoraResponsavel.Id_AuditoriaInterna);
                TryUpdateModel<Responsavel>(ResponsavelEditado);
                ContextoResponsavel.SaveChanges();


                return RedirectToAction("LastResponsible", ResponsavelEditado);
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Administracao/Responsavel/Delete/5

        public ActionResult Delete(int id)
        {
            Responsavel ResponsavelParaExclusao = ContextoResponsavel.Get<Responsavel>(id);

            return View(ResponsavelParaExclusao);
        }

        //
        // POST: /Administracao/Responsavel/Delete/5

        [HttpPost]
        public ActionResult Delete(Responsavel ResponsavelParaExcluir)
        {
            try
            {
                string NomeResponsavel = ResponsavelParaExcluir.Nome;
                Responsavel ResponsavelExcluido = ContextoResponsavel.Get<Responsavel>(ResponsavelParaExcluir.Id_Pessoa);
                ContextoResponsavel.Delete<Responsavel>(ResponsavelExcluido);
                ContextoResponsavel.SaveChanges();

                AuditoriaInterna AuditoraResponsavel = new AuditoriaInterna();
                AuditoraResponsavel.Computador = Environment.MachineName;
                AuditoraResponsavel.DataInsercao = DateTime.Now;
                AuditoraResponsavel.Usuario = User.Identity.Name;
                AuditoraResponsavel.DetalhesOperacao = "Exclusão Tabela Responsavel, Registro: " + NomeResponsavel;
                AuditoraResponsavel.Tabela = "TB_Responsavel";
                AuditoraResponsavel.TipoOperacao = TipoOperacao.Exclusao.ToString();
                ContextoResponsavel.Add<AuditoriaInterna>(AuditoraResponsavel);
                ContextoResponsavel.SaveChanges();

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

        public ActionResult LastResponsible(Responsavel ResponsavelSalvo)
        {
            IList<Responsavel> LastResponsible = ContextoResponsavel.GetAll<Responsavel>()
                                                 .Where(x => x.Id_Pessoa == ResponsavelSalvo.Id_Pessoa)
                                                 .ToList();
            return View(LastResponsible);
        }

        public ActionResult AllResponsible()
        {
            IList<Responsavel> AllResponsible = ContextoResponsavel.GetAll<Responsavel>()
                                                .OrderBy(x => x.Nome)
                                                .ToList();

            return View(AllResponsible);
        }
    }
}
