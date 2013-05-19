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
    public class FornecedorController : Controller
    {
        private IContextoDados ContextoFornecedor = new ContextoDadosNH();
        //
        // GET: /Administracao/Fornecedor/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Administracao/Fornecedor/Details/5

        public ActionResult Details(int id)
        {
            Fornecedor FornecedorDetails = ContextoFornecedor.Get<Fornecedor>(id);
            return View(FornecedorDetails);
        }

        //
        // GET: /Administracao/Fornecedor/Create

        public ActionResult Create()
        {

            return View();
        }

        //
        // POST: /Administracao/Fornecedor/Create

        [HttpPost]
        public ActionResult Create(Fornecedor FornecedorParaSalvar)
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
                    string NomeCidade = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(FornecedorParaSalvar.IdCidade.Nome);
                    string SiglaEstado = FornecedorParaSalvar.IdEstado.Sigla;
                    string NomeBairro = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(FornecedorParaSalvar.IdBairro.Nome);
                    string DescricaoEndereco = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(FornecedorParaSalvar.IdEndereco.Descricao);
                    string CEPEndereco = FornecedorParaSalvar.IdEndereco.CEP;
                    if (PessoaJuridica.IsCnpj(FornecedorParaSalvar.CNPJ) == false)
                    {
                        ViewBag.IsNotCNPJ = "O CNPJ não é válido!";
                        return View();
                    }

                    Fornecedor FornecedorSalvo = new Fornecedor();

                    IList<Estado> VerificarEstado = ContextoFornecedor.GetAll<Estado>().Where(x => x.Sigla == SiglaEstado).ToList();
                    if (VerificarEstado.Count() >= 1)
                    {
                        foreach (var item in VerificarEstado)
                        {
                            FornecedorSalvo.IdEstado = ContextoFornecedor.Get<Estado>(item.Id_Estado);

                        }

                    }
                    else
                    {
                        Estado NovoEstado = new Estado();
                        NovoEstado.Nome = SiglaEstado;
                        NovoEstado.Sigla = SiglaEstado;
                        ContextoFornecedor.Add<Estado>(NovoEstado);
                        ContextoFornecedor.SaveChanges();
                        FornecedorSalvo.IdEstado = ContextoFornecedor.Get<Estado>(NovoEstado.Id_Estado);

                    }

                    IList<Cidade> VerificarCidade = ContextoFornecedor.GetAll<Cidade>().Where(x => x.Nome == NomeCidade).ToList();
                    if (VerificarCidade.Count() >= 1)
                    {
                        foreach (var item in VerificarCidade)
                        {
                            FornecedorSalvo.IdCidade = ContextoFornecedor.Get<Cidade>(item.Id_Cidade);
                        }
                    }
                    else
                    {
                        Cidade NovaCidade = new Cidade();
                        NovaCidade.Nome = NomeCidade;
                        NovaCidade.IdEstado = (from c in ContextoFornecedor.GetAll<Estado>()
                                               .Where(x => x.Sigla == SiglaEstado)
                                               select c).First();
                        ContextoFornecedor.Add<Cidade>(NovaCidade);
                        ContextoFornecedor.SaveChanges();
                        FornecedorSalvo.IdCidade = ContextoFornecedor.Get<Cidade>(NovaCidade.Id_Cidade);
                    }

                    IList<Bairro> VerificaBairro = ContextoFornecedor.GetAll<Bairro>().Where(x => x.Nome == NomeBairro).ToList();

                    if (VerificaBairro.Count() >= 1)
                    {
                        foreach (var item in VerificaBairro)
                        {
                            FornecedorSalvo.IdBairro = ContextoFornecedor.Get<Bairro>(item.Id_Bairro);
                        }

                    }
                    else
                    {
                        Bairro NovoBairro = new Bairro();
                        NovoBairro.Nome = NomeBairro;
                        NovoBairro.IdCidade = ContextoFornecedor.GetAll<Cidade>().Where(x => x.Nome == NomeCidade).First();
                        ContextoFornecedor.Add<Bairro>(NovoBairro);
                        ContextoFornecedor.SaveChanges();
                        FornecedorSalvo.IdBairro = ContextoFornecedor.Get<Bairro>(NovoBairro.Id_Bairro);

                    }

                    IList<Endereco> VerificaEndereco = ContextoFornecedor.GetAll<Endereco>().Where(x => x.Descricao == DescricaoEndereco).ToList();

                    if (VerificaEndereco.Count() >= 1)
                    {
                        foreach (var item in VerificaEndereco)
                        {
                            FornecedorSalvo.IdEndereco = ContextoFornecedor.Get<Endereco>(item.Id_Endereco);
                        }

                    }
                    else
                    {
                        Endereco NovoEndereco = new Endereco();
                        NovoEndereco.Descricao = DescricaoEndereco;
                        NovoEndereco.IdBairro = ContextoFornecedor.GetAll<Bairro>().Where(x => x.Nome == NomeBairro).First();
                        NovoEndereco.CEP = CEPEndereco;
                        ContextoFornecedor.Add<Endereco>(NovoEndereco);
                        ContextoFornecedor.SaveChanges();
                        FornecedorSalvo.IdEndereco = ContextoFornecedor.Get<Endereco>(NovoEndereco.Id_Endereco);

                    }


                    AuditoriaInterna AuditoraFornecedor = new AuditoriaInterna();
                    AuditoraFornecedor.Computador = Environment.MachineName;
                    AuditoraFornecedor.DataInsercao = DateTime.Now;
                    AuditoraFornecedor.Usuario = User.Identity.Name;
                    AuditoraFornecedor.DetalhesOperacao = "Insercao Tabela Fornecedor, Registro: " + FornecedorParaSalvar.Nome;
                    AuditoraFornecedor.Tabela = "TB_Fornecedor";
                    AuditoraFornecedor.TipoOperacao = TipoOperacao.Insercao.ToString();
                    ContextoFornecedor.Add<AuditoriaInterna>(AuditoraFornecedor);
                    ContextoFornecedor.SaveChanges();


                    FornecedorSalvo.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(FornecedorParaSalvar.Nome);
                    FornecedorSalvo.RazaoSocial = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(FornecedorParaSalvar.RazaoSocial);
                    FornecedorSalvo.Email = TransformaParaMaiusculo.TransformarParaMinusculo(FornecedorParaSalvar.Email);
                    FornecedorSalvo.Telefone = FornecedorParaSalvar.Telefone;
                    FornecedorSalvo.InscricaoEstadual = FornecedorParaSalvar.InscricaoEstadual;
                    FornecedorSalvo.CNPJ = FornecedorParaSalvar.CNPJ;
                    FornecedorSalvo.IdAuditoriaInterna = ContextoFornecedor.Get<AuditoriaInterna>(AuditoraFornecedor.Id_AuditoriaInterna);

                    ContextoFornecedor.Add<Fornecedor>(FornecedorSalvo);
                    ContextoFornecedor.SaveChanges();


                    return RedirectToAction("ListLastProvider", FornecedorSalvo);
                }
                catch
                {

                    return View();
                }
            }

            return View();
        }

        //
        // GET: /Administracao/Fornecedor/Edit/5

        public ActionResult Edit(int id)
        {
            Fornecedor FornecedorParaEdicao = ContextoFornecedor.Get<Fornecedor>(id);
            return View(FornecedorParaEdicao);
        }

        //
        // POST: /Administracao/Fornecedor/Edit/5

        [HttpPost]
        public ActionResult Edit(Fornecedor FornecedorParaEdicao)
        {
            try
            {
                string NomeCidade = FornecedorParaEdicao.IdCidade.Nome;
                string SiglaEstado = FornecedorParaEdicao.IdEstado.Sigla;
                string NomeBairro = FornecedorParaEdicao.IdBairro.Nome;
                string DescricaoEndereco = FornecedorParaEdicao.IdEndereco.Descricao;
                string CEPEndereco = FornecedorParaEdicao.IdEndereco.CEP;
                if (PessoaJuridica.IsCnpj(FornecedorParaEdicao.CNPJ) == false)
                {
                    ViewBag.IsNotCNPJ = "O CNPJ não é válido!";
                    return View();
                }

                Fornecedor FornecedorEditado = ContextoFornecedor.Get<Fornecedor>(FornecedorParaEdicao.Id_Pessoa);

                IList<Estado> VerificarEstado = ContextoFornecedor.GetAll<Estado>().Where(x => x.Sigla == SiglaEstado).ToList();
                if (VerificarEstado.Count() >= 1)
                {
                    foreach (var item in VerificarEstado)
                    {
                        FornecedorEditado.IdEstado = ContextoFornecedor.Get<Estado>(item.Id_Estado);

                    }

                }
                else
                {
                    Estado NovoEstado = new Estado();
                    NovoEstado.Nome = SiglaEstado;
                    NovoEstado.Sigla = SiglaEstado;
                    ContextoFornecedor.Add<Estado>(NovoEstado);
                    ContextoFornecedor.SaveChanges();
                    FornecedorEditado.IdEstado = ContextoFornecedor.Get<Estado>(NovoEstado.Id_Estado);

                }

                IList<Cidade> VerificarCidade = ContextoFornecedor.GetAll<Cidade>().Where(x => x.Nome == NomeCidade).ToList();
                if (VerificarCidade.Count() >= 1)
                {
                    foreach (var item in VerificarCidade)
                    {
                        FornecedorEditado.IdCidade = ContextoFornecedor.Get<Cidade>(item.Id_Cidade);
                    }
                }
                else
                {
                    Cidade NovaCidade = new Cidade();
                    NovaCidade.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(NomeCidade);
                    NovaCidade.IdEstado = (from c in ContextoFornecedor.GetAll<Estado>()
                                           .Where(x => x.Sigla == SiglaEstado)
                                           select c).First();
                    ContextoFornecedor.Add<Cidade>(NovaCidade);
                    ContextoFornecedor.SaveChanges();
                    FornecedorEditado.IdCidade = ContextoFornecedor.Get<Cidade>(NovaCidade.Id_Cidade);
                }

                IList<Bairro> VerificaBairro = ContextoFornecedor.GetAll<Bairro>().Where(x => x.Nome == NomeBairro).ToList();

                if (VerificaBairro.Count() >= 1)
                {
                    foreach (var item in VerificaBairro)
                    {
                        FornecedorEditado.IdBairro = ContextoFornecedor.Get<Bairro>(item.Id_Bairro);
                    }

                }
                else
                {
                    Bairro NovoBairro = new Bairro();
                    NovoBairro.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(NomeBairro);
                    NovoBairro.IdCidade = ContextoFornecedor.GetAll<Cidade>().Where(x => x.Nome == NomeCidade).First();
                    ContextoFornecedor.Add<Bairro>(NovoBairro);
                    ContextoFornecedor.SaveChanges();
                    FornecedorEditado.IdBairro = ContextoFornecedor.Get<Bairro>(NovoBairro.Id_Bairro);

                }

                IList<Endereco> VerificaEndereco = ContextoFornecedor.GetAll<Endereco>().Where(x => x.Descricao == DescricaoEndereco).ToList();

                if (VerificaEndereco.Count() >= 1)
                {
                    foreach (var item in VerificaEndereco)
                    {
                        FornecedorEditado.IdEndereco = ContextoFornecedor.Get<Endereco>(item.Id_Endereco);
                    }

                }
                else
                {
                    Endereco NovoEndereco = new Endereco();
                    NovoEndereco.Descricao = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(DescricaoEndereco);
                    NovoEndereco.IdBairro = ContextoFornecedor.GetAll<Bairro>().Where(x => x.Nome == NomeBairro).First();
                    NovoEndereco.CEP = CEPEndereco;
                    ContextoFornecedor.Add<Endereco>(NovoEndereco);
                    ContextoFornecedor.SaveChanges();
                    FornecedorEditado.IdEndereco = ContextoFornecedor.Get<Endereco>(NovoEndereco.Id_Endereco);

                }


                AuditoriaInterna AuditoraFornecedor = new AuditoriaInterna();
                AuditoraFornecedor.Computador = Environment.MachineName;
                AuditoraFornecedor.DataInsercao = DateTime.Now;
                AuditoraFornecedor.Usuario = User.Identity.Name;
                AuditoraFornecedor.DetalhesOperacao = "Alteracao Tabela Fornecedor, Registro: " + FornecedorEditado.Nome;
                AuditoraFornecedor.Tabela = "TB_Fornecedor";
                AuditoraFornecedor.TipoOperacao = TipoOperacao.Alteracao.ToString();
                ContextoFornecedor.Add<AuditoriaInterna>(AuditoraFornecedor);
                ContextoFornecedor.SaveChanges();
                //aqui                
                FornecedorEditado.CNPJ = FornecedorParaEdicao.CNPJ;
                FornecedorEditado.Email = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(FornecedorParaEdicao.Email);
                FornecedorEditado.InscricaoEstadual = FornecedorParaEdicao.InscricaoEstadual;
                FornecedorEditado.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(FornecedorParaEdicao.Nome);
                FornecedorEditado.RazaoSocial = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(FornecedorParaEdicao.RazaoSocial);
                FornecedorEditado.Telefone = FornecedorParaEdicao.Telefone;
                FornecedorEditado.IdAuditoriaInterna = ContextoFornecedor.Get<AuditoriaInterna>(AuditoraFornecedor.Id_AuditoriaInterna);
                TryUpdateModel<Fornecedor>(FornecedorEditado);
                ContextoFornecedor.SaveChanges();



                return RedirectToAction("ListLastProvider", FornecedorEditado);
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Administracao/Fornecedor/Delete/5

        public ActionResult Delete(int id)
        {
            Fornecedor FornecedorParaEdicao = ContextoFornecedor.Get<Fornecedor>(id);

            return View(FornecedorParaEdicao);
        }

        //
        // POST: /Administracao/Fornecedor/Delete/5

        [HttpPost]
        public ActionResult Delete(Fornecedor FornecedorParaExcluir)
        {
            try
            {
                string NomeFornecedor = FornecedorParaExcluir.RazaoSocial;
                Fornecedor FornecedorExcluido = ContextoFornecedor.Get<Fornecedor>(FornecedorParaExcluir.Id_Pessoa);
                ContextoFornecedor.Delete<Fornecedor>(FornecedorExcluido);
                ContextoFornecedor.SaveChanges();

                AuditoriaInterna AuditoraFornecedor = new AuditoriaInterna();
                AuditoraFornecedor.Computador = Environment.MachineName;
                AuditoraFornecedor.DataInsercao = DateTime.Now;
                AuditoraFornecedor.Usuario = User.Identity.Name;
                AuditoraFornecedor.DetalhesOperacao = "Exclusão Tabela Fornecedor, Registro: " + NomeFornecedor;
                AuditoraFornecedor.Tabela = "TB_Fornecedor";
                AuditoraFornecedor.TipoOperacao = TipoOperacao.Exclusao.ToString();
                ContextoFornecedor.Add<AuditoriaInterna>(AuditoraFornecedor);
                ContextoFornecedor.SaveChanges();

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

        public ActionResult ListLastProvider(Fornecedor FornecedorSalvo)
        {
            IList<Fornecedor> LastProvider = ContextoFornecedor.GetAll<Fornecedor>()
                                             .Where(x => x.Id_Pessoa == FornecedorSalvo.Id_Pessoa)
                                             .ToList();
            return View(LastProvider);
        }

        public ActionResult ListAllProvider()
        {
            IList<Fornecedor> ListAll = ContextoFornecedor.GetAll<Fornecedor>()
                                        .OrderBy(x => x.RazaoSocial)
                                        .ToList();
            return View(ListAll);
        }
    }
}
