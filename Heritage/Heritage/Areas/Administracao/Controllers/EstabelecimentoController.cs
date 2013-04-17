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
    public class EstabelecimentoController : Controller
    {
        private IContextoDados ContextoEstabelecimento = new ContextoDadosNH();
        //
        // GET: /Administracao/Estabelecimento/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Administracao/Estabelecimento/Details/5

        public ActionResult Details(int id)
        {
            Estabelecimento DetailsEstabelecimento = ContextoEstabelecimento.Get<Estabelecimento>(id);

            return View(DetailsEstabelecimento);
        }

        //
        // GET: /Administracao/Estabelecimento/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Administracao/Estabelecimento/Create

        [HttpPost]
        public ActionResult Create(Estabelecimento EstabelecimentoParaSalvar)
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
                    string NomeCidade = EstabelecimentoParaSalvar.IdCidade.Nome;
                    string SiglaEstado = EstabelecimentoParaSalvar.IdEstado.Sigla;
                    string NomeBairro = EstabelecimentoParaSalvar.IdBairro.Nome;
                    string DescricaoEndereco = EstabelecimentoParaSalvar.IdEndereco.Descricao;
                    string CEPEndereco = EstabelecimentoParaSalvar.IdEndereco.CEP;
                    if (PessoaJuridica.IsCnpj(EstabelecimentoParaSalvar.CNPJ) == false)
                    {
                        ViewBag.IsNotCNPJ = "O CNPJ não é válido!";
                        return View();
                    }

                    Estabelecimento EstabelecimentoSalvo = new Estabelecimento();

                    IList<Estado> VerificarEstado = ContextoEstabelecimento.GetAll<Estado>().Where(x => x.Sigla == SiglaEstado).ToList();
                    if (VerificarEstado.Count() >= 1)
                    {
                        foreach (var item in VerificarEstado)
                        {
                            EstabelecimentoSalvo.IdEstado = ContextoEstabelecimento.Get<Estado>(item.Id_Estado);

                        }

                    }
                    else
                    {
                        Estado NovoEstado = new Estado();
                        NovoEstado.Nome = SiglaEstado;
                        NovoEstado.Sigla = SiglaEstado;
                        ContextoEstabelecimento.Add<Estado>(NovoEstado);
                        ContextoEstabelecimento.SaveChanges();
                        EstabelecimentoSalvo.IdEstado = ContextoEstabelecimento.Get<Estado>(NovoEstado.Id_Estado);

                    }

                    IList<Cidade> VerificarCidade = ContextoEstabelecimento.GetAll<Cidade>().Where(x => x.Nome == NomeCidade).ToList();
                    if (VerificarCidade.Count() >= 1)
                    {
                        foreach (var item in VerificarCidade)
                        {
                            EstabelecimentoSalvo.IdCidade = ContextoEstabelecimento.Get<Cidade>(item.Id_Cidade);
                        }
                    }
                    else
                    {
                        Cidade NovaCidade = new Cidade();
                        NovaCidade.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(NomeCidade);
                        NovaCidade.IdEstado = (from c in ContextoEstabelecimento.GetAll<Estado>()
                                               .Where(x => x.Sigla == SiglaEstado)
                                               select c).First();
                        ContextoEstabelecimento.Add<Cidade>(NovaCidade);
                        ContextoEstabelecimento.SaveChanges();
                        EstabelecimentoSalvo.IdCidade = ContextoEstabelecimento.Get<Cidade>(NovaCidade.Id_Cidade);
                    }

                    IList<Bairro> VerificaBairro = ContextoEstabelecimento.GetAll<Bairro>().Where(x => x.Nome == NomeBairro).ToList();

                    if (VerificaBairro.Count() >= 1)
                    {
                        foreach (var item in VerificaBairro)
                        {
                            EstabelecimentoSalvo.IdBairro = ContextoEstabelecimento.Get<Bairro>(item.Id_Bairro);
                        }

                    }
                    else
                    {
                        Bairro NovoBairro = new Bairro();
                        NovoBairro.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(NomeBairro);
                        NovoBairro.IdCidade = ContextoEstabelecimento.GetAll<Cidade>().Where(x => x.Nome == NomeCidade).First();
                        ContextoEstabelecimento.Add<Bairro>(NovoBairro);
                        ContextoEstabelecimento.SaveChanges();
                        EstabelecimentoSalvo.IdBairro = ContextoEstabelecimento.Get<Bairro>(NovoBairro.Id_Bairro);

                    }

                    IList<Endereco> VerificaEndereco = ContextoEstabelecimento.GetAll<Endereco>().Where(x => x.Descricao == DescricaoEndereco).ToList();

                    if (VerificaEndereco.Count() >= 1)
                    {
                        foreach (var item in VerificaEndereco)
                        {
                            EstabelecimentoSalvo.IdEndereco = ContextoEstabelecimento.Get<Endereco>(item.Id_Endereco);
                        }

                    }
                    else
                    {
                        Endereco NovoEndereco = new Endereco();
                        NovoEndereco.Descricao = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(DescricaoEndereco);
                        NovoEndereco.IdBairro = ContextoEstabelecimento.GetAll<Bairro>().Where(x => x.Nome == NomeBairro).First();
                        NovoEndereco.CEP = CEPEndereco;
                        ContextoEstabelecimento.Add<Endereco>(NovoEndereco);
                        ContextoEstabelecimento.SaveChanges();
                        EstabelecimentoSalvo.IdEndereco = ContextoEstabelecimento.Get<Endereco>(NovoEndereco.Id_Endereco);

                    }


                    AuditoriaInterna AuditoraEstabeleciemnto = new AuditoriaInterna();
                    AuditoraEstabeleciemnto.Computador = Environment.MachineName;
                    AuditoraEstabeleciemnto.DataInsercao = DateTime.Now;
                    AuditoraEstabeleciemnto.Usuario = User.Identity.Name;
                    AuditoraEstabeleciemnto.DetalhesOperacao = "Insercao Tabela Estabelecimento, Registro: " + EstabelecimentoParaSalvar.Nome;
                    AuditoraEstabeleciemnto.Tabela = "TB_Estabelecimento";
                    AuditoraEstabeleciemnto.TipoOperacao = TipoOperacao.Insercao.ToString();
                    ContextoEstabelecimento.Add<AuditoriaInterna>(AuditoraEstabeleciemnto);
                    ContextoEstabelecimento.SaveChanges();


                    EstabelecimentoSalvo.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(EstabelecimentoParaSalvar.Nome);
                    EstabelecimentoSalvo.RazaoSocial = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(EstabelecimentoParaSalvar.RazaoSocial);
                    EstabelecimentoSalvo.Email = TransformaParaMaiusculo.TransformarParaMinusculo(EstabelecimentoParaSalvar.Email);
                    EstabelecimentoSalvo.Telefone = EstabelecimentoParaSalvar.Telefone;
                    EstabelecimentoSalvo.InscricaoEstadual = EstabelecimentoParaSalvar.InscricaoEstadual;
                    EstabelecimentoSalvo.CNPJ = EstabelecimentoParaSalvar.CNPJ;
                    EstabelecimentoSalvo.IdAuditoriaInterna = ContextoEstabelecimento.Get<AuditoriaInterna>(AuditoraEstabeleciemnto.Id_AuditoriaInterna);

                    ContextoEstabelecimento.Add<Estabelecimento>(EstabelecimentoSalvo);
                    ContextoEstabelecimento.SaveChanges();


                    return RedirectToAction("ListLastEstablishment", EstabelecimentoSalvo);
                }
                catch (Exception e)
                {
                    ViewBag.Message = "Erro codigo: " + e.StackTrace + " mensagem  " + e.Message;
                    return View();
                }
            }

            return View();
        }

        //
        // GET: /Administracao/Estabelecimento/Edit/5

        public ActionResult Edit(int id)
        {
            Estabelecimento EstabelecimentoParaAlterar = ContextoEstabelecimento.Get<Estabelecimento>(id);
            return View();
        }

        //
        // POST: /Administracao/Estabelecimento/Edit/5

        [HttpPost]
        public ActionResult Edit(Estabelecimento EstabelecimentoParaEdicao)
        {
            try
            {
                string NomeCidade = EstabelecimentoParaEdicao.IdCidade.Nome;
                string SiglaEstado = EstabelecimentoParaEdicao.IdEstado.Sigla;
                string NomeBairro = EstabelecimentoParaEdicao.IdBairro.Nome;
                string DescricaoEndereco = EstabelecimentoParaEdicao.IdEndereco.Descricao;
                string CEPEndereco = EstabelecimentoParaEdicao.IdEndereco.CEP;
                if (PessoaJuridica.IsCnpj(EstabelecimentoParaEdicao.CNPJ) == false)
                {
                    ViewBag.IsNotCNPJ = "O CNPJ não é válido!";
                    return View();
                }

                Estabelecimento EstabelecimentoEditado = ContextoEstabelecimento.Get<Estabelecimento>(EstabelecimentoParaEdicao.Id_Pessoa);

                IList<Estado> VerificarEstado = ContextoEstabelecimento.GetAll<Estado>().Where(x => x.Sigla == SiglaEstado).ToList();
                if (VerificarEstado.Count() >= 1)
                {
                    foreach (var item in VerificarEstado)
                    {
                        EstabelecimentoEditado.IdEstado = ContextoEstabelecimento.Get<Estado>(item.Id_Estado);

                    }

                }
                else
                {
                    Estado NovoEstado = new Estado();
                    NovoEstado.Nome = SiglaEstado;
                    NovoEstado.Sigla = SiglaEstado;
                    ContextoEstabelecimento.Add<Estado>(NovoEstado);
                    ContextoEstabelecimento.SaveChanges();
                    EstabelecimentoEditado.IdEstado = ContextoEstabelecimento.Get<Estado>(NovoEstado.Id_Estado);

                }

                IList<Cidade> VerificarCidade = ContextoEstabelecimento.GetAll<Cidade>().Where(x => x.Nome == NomeCidade).ToList();
                if (VerificarCidade.Count() >= 1)
                {
                    foreach (var item in VerificarCidade)
                    {
                        EstabelecimentoEditado.IdCidade = ContextoEstabelecimento.Get<Cidade>(item.Id_Cidade);
                    }
                }
                else
                {
                    Cidade NovaCidade = new Cidade();
                    NovaCidade.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(NomeCidade);
                    NovaCidade.IdEstado = (from c in ContextoEstabelecimento.GetAll<Estado>()
                                           .Where(x => x.Sigla == SiglaEstado)
                                           select c).First();
                    ContextoEstabelecimento.Add<Cidade>(NovaCidade);
                    ContextoEstabelecimento.SaveChanges();
                    EstabelecimentoEditado.IdCidade = ContextoEstabelecimento.Get<Cidade>(NovaCidade.Id_Cidade);
                }

                IList<Bairro> VerificaBairro = ContextoEstabelecimento.GetAll<Bairro>().Where(x => x.Nome == NomeBairro).ToList();

                if (VerificaBairro.Count() >= 1)
                {
                    foreach (var item in VerificaBairro)
                    {
                        EstabelecimentoEditado.IdBairro = ContextoEstabelecimento.Get<Bairro>(item.Id_Bairro);
                    }

                }
                else
                {
                    Bairro NovoBairro = new Bairro();
                    NovoBairro.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(NomeBairro);
                    NovoBairro.IdCidade = ContextoEstabelecimento.GetAll<Cidade>().Where(x => x.Nome == NomeCidade).First();
                    ContextoEstabelecimento.Add<Bairro>(NovoBairro);
                    ContextoEstabelecimento.SaveChanges();
                    EstabelecimentoEditado.IdBairro = ContextoEstabelecimento.Get<Bairro>(NovoBairro.Id_Bairro);

                }

                IList<Endereco> VerificaEndereco = ContextoEstabelecimento.GetAll<Endereco>().Where(x => x.Descricao == DescricaoEndereco).ToList();

                if (VerificaEndereco.Count() >= 1)
                {
                    foreach (var item in VerificaEndereco)
                    {
                        EstabelecimentoEditado.IdEndereco = ContextoEstabelecimento.Get<Endereco>(item.Id_Endereco);
                    }

                }
                else
                {
                    Endereco NovoEndereco = new Endereco();
                    NovoEndereco.Descricao = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(DescricaoEndereco);
                    NovoEndereco.IdBairro = ContextoEstabelecimento.GetAll<Bairro>().Where(x => x.Nome == NomeBairro).First();
                    NovoEndereco.CEP = CEPEndereco;
                    ContextoEstabelecimento.Add<Endereco>(NovoEndereco);
                    ContextoEstabelecimento.SaveChanges();
                    EstabelecimentoEditado.IdEndereco = ContextoEstabelecimento.Get<Endereco>(NovoEndereco.Id_Endereco);

                }


                AuditoriaInterna AuditoraEstabelecimento = new AuditoriaInterna();
                AuditoraEstabelecimento.Computador = Environment.MachineName;
                AuditoraEstabelecimento.DataInsercao = DateTime.Now;
                AuditoraEstabelecimento.Usuario = User.Identity.Name;
                AuditoraEstabelecimento.DetalhesOperacao = "Alteracao Tabela Estabelecimento, Registro: " + EstabelecimentoEditado.Nome;
                AuditoraEstabelecimento.Tabela = "TB_Estabelecimento";
                AuditoraEstabelecimento.TipoOperacao = TipoOperacao.Alteracao.ToString();
                ContextoEstabelecimento.Add<AuditoriaInterna>(AuditoraEstabelecimento);
                ContextoEstabelecimento.SaveChanges();
                //aqui                
                EstabelecimentoEditado.CNPJ = EstabelecimentoParaEdicao.CNPJ;
                EstabelecimentoEditado.Email = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(EstabelecimentoParaEdicao.Email);
                EstabelecimentoEditado.InscricaoEstadual = EstabelecimentoParaEdicao.InscricaoEstadual;
                EstabelecimentoEditado.Nome = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(EstabelecimentoParaEdicao.Nome);
                EstabelecimentoEditado.RazaoSocial = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(EstabelecimentoParaEdicao.RazaoSocial);
                EstabelecimentoEditado.Telefone = EstabelecimentoParaEdicao.Telefone;
                EstabelecimentoEditado.IdAuditoriaInterna = ContextoEstabelecimento.Get<AuditoriaInterna>(AuditoraEstabelecimento.Id_AuditoriaInterna);
                TryUpdateModel<Estabelecimento>(EstabelecimentoEditado);
                ContextoEstabelecimento.SaveChanges();


                return RedirectToAction("ListLastEstablishment", EstabelecimentoEditado);
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Administracao/Estabelecimento/Delete/5

        public ActionResult Delete(int id)
        {
            Estabelecimento EstabelecimentoParaExcluir = ContextoEstabelecimento.Get<Estabelecimento>(id);
            return View(EstabelecimentoParaExcluir);
        }

        //
        // POST: /Administracao/Estabelecimento/Delete/5

        [HttpPost]
        public ActionResult Delete(Estabelecimento EstabelecimentoParaExcluir)
        {
            try
            {
                string NomeEstabelecimento = EstabelecimentoParaExcluir.RazaoSocial;
                Estabelecimento EstabelecimentoExcluido = ContextoEstabelecimento.Get<Estabelecimento>(EstabelecimentoParaExcluir.Id_Pessoa);
                ContextoEstabelecimento.Delete<Estabelecimento>(EstabelecimentoExcluido);
                ContextoEstabelecimento.SaveChanges();

                AuditoriaInterna AuditoraEstabelecimento = new AuditoriaInterna();
                AuditoraEstabelecimento.Computador = Environment.MachineName;
                AuditoraEstabelecimento.DataInsercao = DateTime.Now;
                AuditoraEstabelecimento.Usuario = User.Identity.Name;
                AuditoraEstabelecimento.DetalhesOperacao = "Exclusão Tabela Estabelecimento, Registro: " + NomeEstabelecimento;
                AuditoraEstabelecimento.Tabela = "TB_Estabelecimento";
                AuditoraEstabelecimento.TipoOperacao = TipoOperacao.Exclusao.ToString();
                ContextoEstabelecimento.Add<AuditoriaInterna>(AuditoraEstabelecimento);
                ContextoEstabelecimento.SaveChanges();

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

        public ActionResult ListAllEstablishment()
        {
            IList<Estabelecimento> ListEstablishment = ContextoEstabelecimento.GetAll<Estabelecimento>()
                                                       .OrderBy(x => x.RazaoSocial)
                                                       .ToList();
            return View(ListEstablishment);
        }

        public ActionResult ListLastEstablishment(Estabelecimento EstabelecimentoSalvo)
        {
            IList<Estabelecimento> ListLast = ContextoEstabelecimento.GetAll<Estabelecimento>()
                                              .Where(x => x.Id_Pessoa == EstabelecimentoSalvo.Id_Pessoa)
                                              .ToList();
            return View(ListLast);
        }
    }
}
