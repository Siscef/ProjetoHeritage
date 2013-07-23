using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Heritage.Models;
using Heritage.Models.ContextoBanco;
using System.Web.Security;
using System.Web.Helpers;



namespace Heritage.Areas.Administracao.Controllers
{
    [Authorize(Roles = "Administrador,Contabil,Desenvolvedor")]

    public class BemController : Controller
    {
        private IContextoDados ContextoBem = new ContextoDadosNH();

        // GET: /Administracao/Bem/

        public ActionResult Index()
        {

            return View();
        }

        //
        // GET: /Administracao/Bem/Details/5

        public ActionResult Details(int id)
        {
            Bem BemDetails = (from c in ContextoBem.GetAll<Bem>()
                              .Where(x => x.Inativo == false && x.Descontinuado == false && x.Id_Bem == id)
                              select c).First();
            return View(BemDetails);
        }

        //
        // GET: /Administracao/Bem/Create
        #region Create

        public ActionResult Create()
        {
            ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao");
            ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial");
            ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial");
            ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome");
            ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome");
            ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao");
            ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome");

            return View();
        }

        //
        // POST: /Administracao/Bem/Create

        [HttpPost]
        public ActionResult Create(Bem BemParaSalvar)
        {
            #region LimpaErros
            ModelState["IdEstadoConservacao.Descricao"].Errors.Clear();

            ModelState["IdEstabelecimento.CNPJ"].Errors.Clear();
            ModelState["IdEstabelecimento.RazaoSocial"].Errors.Clear();
            ModelState["IdEstabelecimento.InscricaoEstadual"].Errors.Clear();
            ModelState["IdEstabelecimento.Nome"].Errors.Clear();
            ModelState["IdEstabelecimento.Email"].Errors.Clear();
            ModelState["IdEstabelecimento.Telefone"].Errors.Clear();
            ModelState["IdEstabelecimento.IdEndereco"].Errors.Clear();
            ModelState["IdEstabelecimento.IdBairro"].Errors.Clear();
            ModelState["IdEstabelecimento.IdCidade"].Errors.Clear();
            ModelState["IdEstabelecimento.IdEstado"].Errors.Clear();

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

            ModelState["IdCategoria.Nome"].Errors.Clear();
            ModelState["IdGrupo.Nome"].Errors.Clear();
            ModelState["IdLocalizacao.Descricao"].Errors.Clear();

            ModelState["IdResponsavel.Nome"].Errors.Clear();
            ModelState["IdResponsavel.Email"].Errors.Clear();
            ModelState["IdResponsavel.Telefone"].Errors.Clear();
            ModelState["IdResponsavel.IdEndereco"].Errors.Clear();
            ModelState["IdResponsavel.IdBairro"].Errors.Clear();
            ModelState["IdResponsavel.IdCidade"].Errors.Clear();
            ModelState["IdResponsavel.IdEstado"].Errors.Clear();
            #endregion

            if (ValidarDatas(BemParaSalvar.DataAquisicao, BemParaSalvar.DataInicioDepreciacao) == false)
            {
                ViewBag.DataInvalida = "A data informada não pode ser salva: 01/01/0001";
                ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao", BemParaSalvar.IdEstadoConservacao);
                ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial", BemParaSalvar.IdEstabelecimento);
                ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", BemParaSalvar.IdFornecedor);
                ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome", BemParaSalvar.IdCategoria);
                ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome", BemParaSalvar.IdGrupo);
                ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao", BemParaSalvar.IdLocalizacao);
                ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome", BemParaSalvar.IdResponsavel);
                return View();

            }

            if (BemParaSalvar.DataInicioDepreciacao < BemParaSalvar.DataAquisicao)
            {
                ViewBag.DataInicioDepreciacaoMaior = "A data do início da depreciação não pode ser menor que data aquisição";

                ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao", BemParaSalvar.IdEstadoConservacao);
                ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial", BemParaSalvar.IdEstabelecimento);
                ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", BemParaSalvar.IdFornecedor);
                ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome", BemParaSalvar.IdCategoria);
                ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome", BemParaSalvar.IdGrupo);
                ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao", BemParaSalvar.IdLocalizacao);
                ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome", BemParaSalvar.IdResponsavel);
                return View();
            }


            if (ModelState.IsValid)
            {

                try
                {

                    // Inicio Auditoria Bem
                    AuditoriaInterna AuditoriaBem = new AuditoriaInterna();

                    AuditoriaBem.Computador = Environment.MachineName;
                    AuditoriaBem.DataInsercao = DateTime.Now;
                    AuditoriaBem.DetalhesOperacao = "Insercao Tabela Bem, Registro: " + BemParaSalvar.Descricao;
                    AuditoriaBem.Tabela = "TB_Bem";
                    AuditoriaBem.Usuario = User.Identity.Name;
                    AuditoriaBem.TipoOperacao = TipoOperacao.Insercao.ToString();

                    ContextoBem.Add<AuditoriaInterna>(AuditoriaBem);
                    ContextoBem.SaveChanges();

                    //Fim da auditoria

                    Bem BemSalvo = new Bem();

                    //impostos
                    BemSalvo.Cofins = BemParaSalvar.Cofins;
                    BemSalvo.Pis = BemParaSalvar.Pis;

                    //dados da compra
                    BemSalvo.Descricao = BemParaSalvar.Descricao.ToUpperInvariant();
                    BemSalvo.ValorCompra = BemParaSalvar.ValorCompra;
                    BemSalvo.DataAquisicao = BemParaSalvar.DataAquisicao;
                    BemSalvo.NumeroNotaFiscal = BemParaSalvar.NumeroNotaFiscal;

                    //configuração de valores
                    switch (BemParaSalvar.TipoParaDepreciacao)
                    {
                        case TipoDepreciacao.Linear_Quotas_Constantes:
                            BemSalvo.ValorResidual = 0;
                            BemSalvo.ValorSalvamento = 0;
                            BemSalvo.ValorMaximoDepreciacao = 0;
                            BemSalvo.ValorContabil = BemParaSalvar.ValorContabil;
                            BemSalvo.ValorDepreciavel = BemParaSalvar.ValorContabil;
                            BemSalvo.ValorAtual = BemParaSalvar.ValorContabil;
                            BemSalvo.ValorDepreciado = 0;
                            break;
                        case TipoDepreciacao.Soma_Dígitos:
                            if (BemParaSalvar.DataAquisicao.Year != BemParaSalvar.DataInicioDepreciacao.Year)
                            {
                                ViewBag.NomeDiv = "alert alert-block";
                                ViewBag.Titulo = "Atenção";
                                ViewBag.Mensagem = "Esse tipo de depreciação requer que o mesmo ano da aquisição seja o ano do início depreciação.";
                                ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao", BemParaSalvar.IdEstadoConservacao);
                                ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial", BemParaSalvar.IdEstabelecimento);
                                ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", BemParaSalvar.IdFornecedor);
                                ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome", BemParaSalvar.IdCategoria);
                                ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome", BemParaSalvar.IdGrupo);
                                ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao", BemParaSalvar.IdLocalizacao);
                                ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome", BemParaSalvar.IdResponsavel);
                                return View();

                            }

                            if (BemParaSalvar.ValorResidual == 0)
                            {
                                ViewBag.NomeDiv = "alert alert-block";
                                ViewBag.Titulo = "Atenção";
                                ViewBag.Mensagem = "Esse tipo de depreciação requer um valor residual, caso não possua por favor utilizar outro método.";
                                ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao", BemParaSalvar.IdEstadoConservacao);
                                ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial", BemParaSalvar.IdEstabelecimento);
                                ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", BemParaSalvar.IdFornecedor);
                                ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome", BemParaSalvar.IdCategoria);
                                ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome", BemParaSalvar.IdGrupo);
                                ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao", BemParaSalvar.IdLocalizacao);
                                ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome", BemParaSalvar.IdResponsavel);
                                return View();

                            }

                            BemSalvo.ValorResidual = BemParaSalvar.ValorResidual;
                            BemSalvo.ValorSalvamento = 0;
                            BemSalvo.ValorMaximoDepreciacao = 0;
                            BemSalvo.ValorContabil = BemParaSalvar.ValorContabil;
                            BemSalvo.ValorDepreciavel = BemParaSalvar.ValorContabil;
                            BemSalvo.ValorAtual = BemSalvo.ValorContabil;
                            BemSalvo.ValorAtual = BemParaSalvar.ValorContabil - BemParaSalvar.ValorResidual;
                            BemSalvo.ValorDepreciado = 0;
                            break;
                        case TipoDepreciacao.Reducao_de_Saldos:
                            if (BemParaSalvar.ValorSalvamento == 0)
                            {
                                ViewBag.NomeDiv = "alert alert-block";
                                ViewBag.Titulo = "Atenção";
                                ViewBag.Mensagem = "Esse tipo de depreciação requer que o preechimento obrigatório do campo: Valor salvamento.";
                                ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao", BemParaSalvar.IdEstadoConservacao);
                                ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial", BemParaSalvar.IdEstabelecimento);
                                ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", BemParaSalvar.IdFornecedor);
                                ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome", BemParaSalvar.IdCategoria);
                                ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome", BemParaSalvar.IdGrupo);
                                ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao", BemParaSalvar.IdLocalizacao);
                                ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome", BemParaSalvar.IdResponsavel);
                                return View();

                            }

                            BemSalvo.ValorResidual = BemParaSalvar.ValorResidual;
                            BemSalvo.ValorSalvamento = BemParaSalvar.ValorSalvamento;
                            BemSalvo.ValorMaximoDepreciacao = 0;
                            BemSalvo.ValorContabil = BemParaSalvar.ValorContabil;
                            BemSalvo.ValorDepreciavel = BemParaSalvar.ValorContabil;
                            BemSalvo.ValorAtual = BemParaSalvar.ValorContabil - BemParaSalvar.ValorResidual;
                            BemSalvo.ValorDepreciado = 0;

                            break;
                        case TipoDepreciacao.UnidadesProduzidas:
                            if (BemParaSalvar.UnidadesProduzidasPeriodo == 0 && BemParaSalvar.UnidadesEstimadasVidaUtil == 0)
                            {
                                ViewBag.NomeDiv = "alert alert-block";
                                ViewBag.Titulo = "Atenção";
                                ViewBag.Mensagem = "Esse tipo de depreciação requer que o preechimento obrigatório dos campos: unidades produzidas no período e unidades produzidas para a vida útil.";
                                ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao", BemParaSalvar.IdEstadoConservacao);
                                ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial", BemParaSalvar.IdEstabelecimento);
                                ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", BemParaSalvar.IdFornecedor);
                                ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome", BemParaSalvar.IdCategoria);
                                ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome", BemParaSalvar.IdGrupo);
                                ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao", BemParaSalvar.IdLocalizacao);
                                ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome", BemParaSalvar.IdResponsavel);
                                return View();

                            }

                            BemSalvo.ValorContabil = BemParaSalvar.ValorContabil;
                            BemSalvo.ValorDepreciavel = BemParaSalvar.ValorContabil;
                            BemSalvo.ValorResidual = 0;
                            BemSalvo.ValorSalvamento = 0;
                            BemSalvo.ValorMaximoDepreciacao = 0;
                            BemSalvo.ValorAtual = BemParaSalvar.ValorContabil;
                            BemSalvo.ValorDepreciado = 0;
                            BemSalvo.UnidadesEstimadasVidaUtil = BemParaSalvar.UnidadesEstimadasVidaUtil;
                            BemSalvo.UnidadesProduzidasPeriodo = BemParaSalvar.UnidadesProduzidasPeriodo;
                            break;
                        case TipoDepreciacao.Horas_Trabalhadas:
                            if (BemParaSalvar.HorasEstimadaVidaUtil == 0 && BemParaSalvar.HorasTrabalhdadasPeriodo == 0)
                            {
                                ViewBag.NomeDiv = "alert alert-block";
                                ViewBag.Titulo = "Atenção";
                                ViewBag.Mensagem = "Esse tipo de depreciação requer que o preechimento obrigatório dos campos: horas trabalhadas no período e horas para a vida útil.";
                                ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao", BemParaSalvar.IdEstadoConservacao);
                                ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial", BemParaSalvar.IdEstabelecimento);
                                ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", BemParaSalvar.IdFornecedor);
                                ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome", BemParaSalvar.IdCategoria);
                                ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome", BemParaSalvar.IdGrupo);
                                ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao", BemParaSalvar.IdLocalizacao);
                                ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome", BemParaSalvar.IdResponsavel);
                                return View();

                            }

                            BemSalvo.ValorContabil = BemParaSalvar.ValorContabil;
                            BemSalvo.ValorDepreciavel = BemParaSalvar.ValorContabil;
                            BemSalvo.ValorResidual = BemParaSalvar.ValorResidual;
                            BemSalvo.ValorSalvamento = BemParaSalvar.ValorSalvamento;
                            BemSalvo.ValorMaximoDepreciacao = BemParaSalvar.ValorMaximoDepreciacao;
                            BemSalvo.ValorAtual = BemParaSalvar.ValorContabil - BemParaSalvar.ValorResidual;
                            BemSalvo.ValorDepreciado = 0;
                            BemSalvo.HorasTrabalhdadasPeriodo = BemParaSalvar.HorasTrabalhdadasPeriodo;
                            BemSalvo.HorasEstimadaVidaUtil = BemParaSalvar.HorasEstimadaVidaUtil;
                            break;
                        case TipoDepreciacao.Linear_Valor_Maximo_Depreciacao:
                            if (BemParaSalvar.ValorMaximoDepreciacao == 0 || BemParaSalvar.ValorMaximoDepreciacao > BemParaSalvar.ValorCompra || BemSalvo.ValorMaximoDepreciacao > BemParaSalvar.ValorContabil)
                            {
                                ViewBag.NomeDiv = "alert alert-block";
                                ViewBag.Titulo = "Atenção";
                                ViewBag.Mensagem = "Esse tipo de depreciação requer que o preechimento obrigatório do campo: valor máximo depreciação e ele deve ser menor ou igual o valor contábil.";
                                ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao", BemParaSalvar.IdEstadoConservacao);
                                ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial", BemParaSalvar.IdEstabelecimento);
                                ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", BemParaSalvar.IdFornecedor);
                                ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome", BemParaSalvar.IdCategoria);
                                ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome", BemParaSalvar.IdGrupo);
                                ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao", BemParaSalvar.IdLocalizacao);
                                ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome", BemParaSalvar.IdResponsavel);
                                return View();
                            }

                            BemSalvo.ValorContabil = BemParaSalvar.ValorContabil;
                            BemSalvo.ValorResidual = BemParaSalvar.ValorResidual;
                            BemSalvo.ValorSalvamento = BemParaSalvar.ValorSalvamento;
                            BemSalvo.ValorMaximoDepreciacao = BemParaSalvar.ValorMaximoDepreciacao;
                            BemSalvo.ValorDepreciavel = BemParaSalvar.ValorSalvamento;
                            BemSalvo.ValorAtual = BemParaSalvar.ValorContabil - BemParaSalvar.ValorResidual;
                            BemSalvo.ValorDepreciado = 0;
                            break;
                        case TipoDepreciacao.Variacao_Taxas:

                            BemSalvo.ValorResidual = BemParaSalvar.ValorResidual;
                            BemSalvo.ValorSalvamento = BemParaSalvar.ValorSalvamento;
                            BemSalvo.ValorMaximoDepreciacao = BemParaSalvar.ValorMaximoDepreciacao;
                            BemSalvo.ValorContabil = BemParaSalvar.ValorContabil;
                            BemSalvo.ValorDepreciavel = BemParaSalvar.ValorContabil - BemParaSalvar.ValorResidual;
                            BemSalvo.ValorAtual = BemParaSalvar.ValorContabil - BemParaSalvar.ValorResidual;
                            BemSalvo.ValorDepreciado = 0;
                            break;
                        default:

                            BemSalvo.ValorContabil = BemParaSalvar.ValorContabil;
                            BemSalvo.ValorDepreciavel = BemParaSalvar.ValorContabil;
                            BemSalvo.ValorResidual = BemParaSalvar.ValorResidual;
                            BemSalvo.ValorSalvamento = 0;
                            BemSalvo.ValorMaximoDepreciacao = 0;
                            BemSalvo.ValorAtual = BemParaSalvar.ValorContabil - BemParaSalvar.ValorResidual;
                            BemSalvo.ValorDepreciado = 0;
                            break;
                    }



                    //configuração depreciação

                    BemSalvo.TipoParaDepreciacao = BemParaSalvar.TipoParaDepreciacao;
                    BemSalvo.DataInicioDepreciacao = BemParaSalvar.DataInicioDepreciacao;
                    BemSalvo.TaxaDepreciacaoAnual = BemParaSalvar.TaxaDepreciacaoAnual;
                    BemSalvo.CoeficienteDepreciacao = BemParaSalvar.CoeficienteDepreciacao;
                    BemSalvo.VidaUtil = CalcularNumeroMesesVidaUtil(BemParaSalvar.ValorCompra, BemParaSalvar.TaxaDepreciacaoAnual, BemParaSalvar.CoeficienteDepreciacao);


                    if (BemSalvo.Descontinuado == true)
                    {
                        BemSalvo.DepreciacaoAtiva = false;

                    }
                    BemSalvo.Descontinuado = BemParaSalvar.Descontinuado;
                    BemSalvo.Inativo = BemParaSalvar.Inativo;

                    BemSalvo.BemDepreciavel = BemParaSalvar.BemDepreciavel;

                    IList<Parametros> ParametrosSistema = ContextoBem.GetAll<Parametros>()
                                                         .ToList();

                    foreach (var itemParametros in ParametrosSistema)
                    {
                        if (itemParametros.ValorMinimoDepreciacao > BemParaSalvar.ValorCompra)
                        {
                            BemSalvo.DepreciacaoAtiva = false;
                            BemSalvo.BemDepreciavel = false;
                        }
                        else
                        {
                            BemSalvo.DepreciacaoAtiva = true;
                        }

                    }

                    // referencias externas


                    BemSalvo.IdCategoria = ContextoBem.Get<Categoria>(BemParaSalvar.IdCategoria.Id_Categoria);
                    BemSalvo.IdEstabelecimento = ContextoBem.Get<Estabelecimento>(BemParaSalvar.IdEstabelecimento.Id_Pessoa);
                    BemSalvo.IdEstadoConservacao = ContextoBem.Get<EstadoConservacao>(BemParaSalvar.IdEstadoConservacao.Id_EstadoConservacao);
                    BemSalvo.IdFornecedor = ContextoBem.Get<Fornecedor>(BemParaSalvar.IdFornecedor.Id_Pessoa);
                    BemSalvo.IdGrupo = ContextoBem.Get<Grupo>(BemParaSalvar.IdGrupo.Id_Grupo);
                    BemSalvo.IdLocalizacao = ContextoBem.Get<Localizacao>(BemParaSalvar.IdLocalizacao.Id_Localizacao);
                    BemSalvo.IdResponsavel = ContextoBem.Get<Responsavel>(BemParaSalvar.IdResponsavel.Id_Pessoa);
                    BemSalvo.IdAuditoriaInterna = ContextoBem.Get<AuditoriaInterna>(AuditoriaBem.Id_AuditoriaInterna);

                    ContextoBem.Add<Bem>(BemSalvo);
                    ContextoBem.SaveChanges();
                    //Fim do bem

                    //Inicio Depreciacao Bem

                    DepreciacaoBem DepreciacaoBemVazia = new DepreciacaoBem();

                    DepreciacaoBemVazia.DataDepreciacaoBem = BemSalvo.DataInicioDepreciacao;
                    DepreciacaoBemVazia.IdBem = ContextoBem.Get<Bem>(BemSalvo.Id_Bem);
                    DepreciacaoBemVazia.IdAuditoriaInterna = ContextoBem.Get<AuditoriaInterna>(AuditoriaBem.Id_AuditoriaInterna);
                    DepreciacaoBemVazia.ValorCofins = 0;
                    DepreciacaoBemVazia.ValorDepreciado = 0;
                    DepreciacaoBemVazia.ValorPis = 0;
                    DepreciacaoBemVazia.DepreciacaoFeita = false;
                    DepreciacaoBemVazia.TaxaDepreciacao = DepreciacaoBemVazia.CalculaTaxaMensal(BemSalvo.CoeficienteDepreciacao, BemSalvo.TaxaDepreciacaoAnual);
                    DepreciacaoBemVazia.TipoParaDepreciacao = BemSalvo.TipoParaDepreciacao;

                    ContextoBem.Add<DepreciacaoBem>(DepreciacaoBemVazia);
                    ContextoBem.SaveChanges();
                    //Fim depreciacao


                    TimeSpan DiferencaDeMeses = Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(BemSalvo.DataInicioDepreciacao.ToString("yyyy/MM/dd HH:mm:ss"));
                    decimal Meses = Convert.ToDecimal(DiferencaDeMeses.Days);
                    decimal NumeroMeses = Math.Round((Meses / 30), 0);
                    if (NumeroMeses > 1)
                    {
                        CalcularDepreciacaoAtrasada((int)BemSalvo.Id_Bem);
                    }

                    return RedirectToAction("LastProperty", BemSalvo);
                }
                catch (Exception e)
                {

                    ViewBag.NomeDiv = "alert alert-block";
                    ViewBag.Titulo = "Atenção";
                    ViewBag.Mensagem = e.Message;
                    ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao", BemParaSalvar.IdEstadoConservacao);
                    ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial", BemParaSalvar.IdEstabelecimento);
                    ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", BemParaSalvar.IdFornecedor);
                    ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome", BemParaSalvar.IdCategoria);
                    ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome", BemParaSalvar.IdGrupo);
                    ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao", BemParaSalvar.IdLocalizacao);
                    ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome", BemParaSalvar.IdResponsavel);
                    return View();
                }
            }
            ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao", BemParaSalvar.IdEstadoConservacao);
            ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial", BemParaSalvar.IdEstabelecimento);
            ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", BemParaSalvar.IdFornecedor);
            ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome", BemParaSalvar.IdCategoria);
            ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome", BemParaSalvar.IdGrupo);
            ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao", BemParaSalvar.IdLocalizacao);
            ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome", BemParaSalvar.IdResponsavel);
            return View();
        }

        #endregion

        #region edit
        //
        // GET: /Administracao/Bem/Edit/5

        public ActionResult Edit(int id)
        {
            Bem BemParaEdicao = ContextoBem.Get<Bem>(id);

            ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao");
            ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial");
            ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial");
            ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome");
            ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome");
            ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao");
            ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome");
            ViewBag.Descricao = BemParaEdicao.Descricao;
            return View(BemParaEdicao);
        }

        //
        // POST: /Administracao/Bem/Edit/5

        [HttpPost]
        public ActionResult Edit(Bem BemParaEdicao)
        {
            #region LimparErrosEdicao
            ModelState["IdEstadoConservacao.Descricao"].Errors.Clear();

            ModelState["IdEstabelecimento.CNPJ"].Errors.Clear();
            ModelState["IdEstabelecimento.RazaoSocial"].Errors.Clear();
            ModelState["IdEstabelecimento.InscricaoEstadual"].Errors.Clear();
            ModelState["IdEstabelecimento.Nome"].Errors.Clear();
            ModelState["IdEstabelecimento.Email"].Errors.Clear();
            ModelState["IdEstabelecimento.Telefone"].Errors.Clear();
            ModelState["IdEstabelecimento.IdEndereco"].Errors.Clear();
            ModelState["IdEstabelecimento.IdBairro"].Errors.Clear();
            ModelState["IdEstabelecimento.IdCidade"].Errors.Clear();
            ModelState["IdEstabelecimento.IdEstado"].Errors.Clear();

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

            ModelState["IdCategoria.Nome"].Errors.Clear();
            ModelState["IdGrupo.Nome"].Errors.Clear();
            ModelState["IdLocalizacao.Descricao"].Errors.Clear();

            ModelState["IdResponsavel.Nome"].Errors.Clear();
            ModelState["IdResponsavel.Email"].Errors.Clear();
            ModelState["IdResponsavel.Telefone"].Errors.Clear();
            ModelState["IdResponsavel.IdEndereco"].Errors.Clear();
            ModelState["IdResponsavel.IdBairro"].Errors.Clear();
            ModelState["IdResponsavel.IdCidade"].Errors.Clear();
            ModelState["IdResponsavel.IdEstado"].Errors.Clear();
            #endregion

            #region CondicoesParaEditarBem
            if (ValidarDatas(BemParaEdicao.DataAquisicao, BemParaEdicao.DataInicioDepreciacao) == false)
            {
                ViewBag.DataInvalida = "A data informada não pode ser salva: 01/01/0001";

                ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao", BemParaEdicao.IdEstadoConservacao);
                ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial", BemParaEdicao.IdEstabelecimento);
                ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", BemParaEdicao.IdFornecedor);
                ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome", BemParaEdicao.IdCategoria);
                ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome", BemParaEdicao.IdGrupo);
                ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao", BemParaEdicao.IdLocalizacao);
                ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome", BemParaEdicao.IdResponsavel);
                return View();

            }

            if (BemParaEdicao.DataInicioDepreciacao < BemParaEdicao.DataAquisicao)
            {
                ViewBag.DataInicioDepreciacaoMaior = "A data do início da depreciação não pode ser menor que data aquisição";

                ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao", BemParaEdicao.IdEstadoConservacao);
                ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial", BemParaEdicao.IdEstabelecimento);
                ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", BemParaEdicao.IdFornecedor);
                ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome", BemParaEdicao.IdCategoria);
                ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome", BemParaEdicao.IdGrupo);
                ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao", BemParaEdicao.IdLocalizacao);
                ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome", BemParaEdicao.IdResponsavel);
                return View();
            }

            if (BemParaEdicao.TipoParaDepreciacao.ToString() == "Soma_Dígitos" && BemParaEdicao.ValorResidual == 0)
            {
                ViewBag.NomeDiv = "alert alert-block";
                ViewBag.Titulo = "Atenção";
                ViewBag.Mensagem = "Esse tipo de depreciação requer um valor residual, caso não possua por favor utilizar outro método.";

                ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao", BemParaEdicao.IdEstadoConservacao);
                ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial", BemParaEdicao.IdEstabelecimento);
                ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", BemParaEdicao.IdFornecedor);
                ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome", BemParaEdicao.IdCategoria);
                ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome", BemParaEdicao.IdGrupo);
                ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao", BemParaEdicao.IdLocalizacao);
                ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome", BemParaEdicao.IdResponsavel);
                return View();

            }

            if (BemParaEdicao.DataAquisicao.Year != BemParaEdicao.DataInicioDepreciacao.Year && BemParaEdicao.TipoParaDepreciacao.ToString() == "Soma_Dígitos")
            {
                ViewBag.NomeDiv = "alert alert-block";
                ViewBag.Titulo = "Atenção";
                ViewBag.Mensagem = "Esse tipo de depreciação requer que o mesmo ano da aquisição seja o ano do início depreciação.";

                ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao", BemParaEdicao.IdEstadoConservacao);
                ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial", BemParaEdicao.IdEstabelecimento);
                ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", BemParaEdicao.IdFornecedor);
                ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome", BemParaEdicao.IdCategoria);
                ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome", BemParaEdicao.IdGrupo);
                ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao", BemParaEdicao.IdLocalizacao);
                ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome", BemParaEdicao.IdResponsavel);
                return View();

            }

            if (BemParaEdicao.ValorSalvamento == 0 && BemParaEdicao.TipoParaDepreciacao.ToString() == "Reducao_de_Saldos")
            {
                ViewBag.NomeDiv = "alert alert-block";
                ViewBag.Titulo = "Atenção";
                ViewBag.Mensagem = "Esse tipo de depreciação requer que o preechimento obrigatório do campo: Valor salvamento.";

                ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao", BemParaEdicao.IdEstadoConservacao);
                ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial", BemParaEdicao.IdEstabelecimento);
                ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", BemParaEdicao.IdFornecedor);
                ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome", BemParaEdicao.IdCategoria);
                ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome", BemParaEdicao.IdGrupo);
                ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao", BemParaEdicao.IdLocalizacao);
                ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome", BemParaEdicao.IdResponsavel);
                return View();

            }

            if (BemParaEdicao.UnidadesProduzidasPeriodo == 0 && BemParaEdicao.UnidadesEstimadasVidaUtil == 0 && BemParaEdicao.TipoParaDepreciacao.ToString() == "UnidadesProduzidas")
            {
                ViewBag.NomeDiv = "alert alert-block";
                ViewBag.Titulo = "Atenção";
                ViewBag.Mensagem = "Esse tipo de depreciação requer que o preechimento obrigatório dos campos: unidades produzidas no período e unidades produzidas para a vida útil.";

                ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao", BemParaEdicao.IdEstadoConservacao);
                ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial", BemParaEdicao.IdEstabelecimento);
                ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", BemParaEdicao.IdFornecedor);
                ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome", BemParaEdicao.IdCategoria);
                ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome", BemParaEdicao.IdGrupo);
                ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao", BemParaEdicao.IdLocalizacao);
                ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome", BemParaEdicao.IdResponsavel);
                return View();

            }

            if (BemParaEdicao.HorasEstimadaVidaUtil == 0 && BemParaEdicao.HorasTrabalhdadasPeriodo == 0 && BemParaEdicao.TipoParaDepreciacao.ToString() == "Horas_Trabalhadas")
            {
                ViewBag.NomeDiv = "alert alert-block";
                ViewBag.Titulo = "Atenção";
                ViewBag.Mensagem = "Esse tipo de depreciação requer que o preechimento obrigatório dos campos: horas trabalhadas no período e horas para a vida útil.";

                ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao", BemParaEdicao.IdEstadoConservacao);
                ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial", BemParaEdicao.IdEstabelecimento);
                ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", BemParaEdicao.IdFornecedor);
                ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome", BemParaEdicao.IdCategoria);
                ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome", BemParaEdicao.IdGrupo);
                ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao", BemParaEdicao.IdLocalizacao);
                ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome", BemParaEdicao.IdResponsavel);
                return View();

            }

            if (BemParaEdicao.ValorMaximoDepreciacao == 0 && BemParaEdicao.TipoParaDepreciacao.ToString() == "Linear_Valor_Maximo_Depreciacao")
            {

                ViewBag.NomeDiv = "alert alert-block";
                ViewBag.Titulo = "Atenção";
                ViewBag.Mensagem = "Esse tipo de depreciação requer que o preechimento obrigatório do campo: valor máximo depreciação.";

                ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao", BemParaEdicao.IdEstadoConservacao);
                ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial", BemParaEdicao.IdEstabelecimento);
                ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", BemParaEdicao.IdFornecedor);
                ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome", BemParaEdicao.IdCategoria);
                ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome", BemParaEdicao.IdGrupo);
                ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao", BemParaEdicao.IdLocalizacao);
                ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome", BemParaEdicao.IdResponsavel);
                return View();

            }

            if (BemParaEdicao.ValorMaximoDepreciacao > BemParaEdicao.ValorCompra && BemParaEdicao.TipoParaDepreciacao.ToString() == "Linear_Valor_Maximo_Depreciacao")
            {
                ViewBag.NomeDiv = "alert alert-block";
                ViewBag.Titulo = "Atenção";
                ViewBag.Mensagem = "Esse tipo de depreciação requer que o preechimento obrigatório do campo: valor máximo depreciação.";

                ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao", BemParaEdicao.IdEstadoConservacao);
                ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial", BemParaEdicao.IdEstabelecimento);
                ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", BemParaEdicao.IdFornecedor);
                ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome", BemParaEdicao.IdCategoria);
                ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome", BemParaEdicao.IdGrupo);
                ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao", BemParaEdicao.IdLocalizacao);
                ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome", BemParaEdicao.IdResponsavel);
                return View();

            }

            #endregion

            if (ModelState.IsValid)
            {


                try
                {

                    Bem BemEditado = ContextoBem.Get<Bem>(BemParaEdicao.Id_Bem);

                    AuditoriaInterna AuditoriaBem = new AuditoriaInterna();

                    AuditoriaBem.Computador = Environment.MachineName;
                    AuditoriaBem.DataInsercao = DateTime.Now;
                    AuditoriaBem.DetalhesOperacao = "Alteracao Tabela Bem, Registro: " + BemEditado.Descricao + " Para: " + BemParaEdicao.Descricao;
                    AuditoriaBem.Tabela = "TB_Bem";
                    AuditoriaBem.Usuario = User.Identity.Name;
                    AuditoriaBem.TipoOperacao = TipoOperacao.Alteracao.ToString();

                    ContextoBem.Add<AuditoriaInterna>(AuditoriaBem);
                    ContextoBem.SaveChanges();

                    BemEditado.CoeficienteDepreciacao = BemParaEdicao.CoeficienteDepreciacao;
                    BemEditado.DepreciacaoAtiva = BemParaEdicao.DepreciacaoAtiva;
                    BemEditado.BemDepreciavel = BemParaEdicao.BemDepreciavel;
                    BemEditado.Cofins = BemParaEdicao.Cofins;
                    BemEditado.DataAquisicao = BemParaEdicao.DataAquisicao;
                    BemEditado.DataInicioDepreciacao = BemParaEdicao.DataInicioDepreciacao;
                    BemEditado.TaxaDepreciacaoAnual = BemParaEdicao.TaxaDepreciacaoAnual;
                    BemEditado.Descontinuado = BemParaEdicao.Descontinuado;
                    BemEditado.Descricao = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(BemParaEdicao.Descricao);
                    BemEditado.IdCategoria = ContextoBem.Get<Categoria>(BemParaEdicao.IdCategoria.Id_Categoria);
                    BemEditado.IdEstabelecimento = ContextoBem.Get<Estabelecimento>(BemParaEdicao.IdEstabelecimento.Id_Pessoa);
                    BemEditado.IdEstadoConservacao = ContextoBem.Get<EstadoConservacao>(BemParaEdicao.IdEstadoConservacao.Id_EstadoConservacao);
                    BemEditado.IdFornecedor = ContextoBem.Get<Fornecedor>(BemParaEdicao.IdFornecedor.Id_Pessoa);
                    BemEditado.IdGrupo = ContextoBem.Get<Grupo>(BemParaEdicao.IdGrupo.Id_Grupo);
                    BemEditado.IdLocalizacao = ContextoBem.Get<Localizacao>(BemParaEdicao.IdLocalizacao.Id_Localizacao);
                    BemEditado.IdResponsavel = ContextoBem.Get<Responsavel>(BemParaEdicao.IdResponsavel.Id_Pessoa);
                    BemEditado.Inativo = BemParaEdicao.Inativo;
                    BemEditado.NumeroNotaFiscal = BemParaEdicao.NumeroNotaFiscal;
                    BemEditado.Pis = BemParaEdicao.Pis;
                    BemEditado.Cofins = BemParaEdicao.Cofins;
                    BemEditado.ValorResidual = BemParaEdicao.ValorResidual;
                    BemEditado.ValorCompra = BemParaEdicao.ValorCompra;
                    BemEditado.ValorContabil = BemParaEdicao.ValorContabil - BemParaEdicao.ValorDepreciado;
                    BemEditado.ValorDepreciavel = BemParaEdicao.ValorContabil - BemParaEdicao.ValorResidual;
                    BemEditado.TipoParaDepreciacao = BemParaEdicao.TipoParaDepreciacao;
                    BemEditado.IdAuditoriaInterna = ContextoBem.Get<AuditoriaInterna>(AuditoriaBem.Id_AuditoriaInterna);
                    BemEditado.ValorMaximoDepreciacao = BemParaEdicao.ValorMaximoDepreciacao;
                    BemEditado.ValorSalvamento = BemParaEdicao.ValorSalvamento;
                    BemEditado.HorasEstimadaVidaUtil = BemParaEdicao.HorasEstimadaVidaUtil;
                    BemEditado.HorasTrabalhdadasPeriodo = BemEditado.HorasTrabalhdadasPeriodo;
                    BemEditado.UnidadesProduzidasPeriodo = BemParaEdicao.UnidadesProduzidasPeriodo;
                    BemEditado.UnidadesEstimadasVidaUtil = BemParaEdicao.UnidadesEstimadasVidaUtil;

                    ContextoBem.SaveChanges();


                    return RedirectToAction("LastProperty", BemEditado);
                }
                catch
                {
                    return View();
                }
            }
            ViewBag.EstadoConservacao = new SelectList(ContextoBem.GetAll<EstadoConservacao>(), "Id_EstadoConservacao", "Descricao", BemParaEdicao.IdEstadoConservacao);
            ViewBag.Estabelecimento = new SelectList(ContextoBem.GetAll<Estabelecimento>(), "Id_Pessoa", "RazaoSocial", BemParaEdicao.IdEstabelecimento);
            ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>(), "Id_Pessoa", "RazaoSocial", BemParaEdicao.IdFornecedor);
            ViewBag.Categoria = new SelectList(ContextoBem.GetAll<Categoria>(), "Id_Categoria", "Nome", BemParaEdicao.IdCategoria);
            ViewBag.Grupo = new SelectList(ContextoBem.GetAll<Grupo>(), "Id_Grupo", "Nome", BemParaEdicao.IdGrupo);
            ViewBag.Localizacao = new SelectList(ContextoBem.GetAll<Localizacao>(), "Id_Localizacao", "Descricao", BemParaEdicao.IdLocalizacao);
            ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>(), "Id_Pessoa", "Nome", BemParaEdicao.IdResponsavel);
            return View();

        }
        #endregion

        #region delete
        public ActionResult Delete(int id)
        {
            Bem BemParaExclusao = ContextoBem.Get<Bem>(id);
            ViewBag.Descricao = BemParaExclusao.Descricao;
            return View(BemParaExclusao);
        }

        //
        // POST: /Administracao/Bem/Delete/5

        [HttpPost]
        public ActionResult Delete(Bem BemParaExcluir)
        {
            try
            {
                string DescricaoBem = BemParaExcluir.Descricao;

                AuditoriaInterna AuditoriaBem = new AuditoriaInterna();

                AuditoriaBem.Computador = Request.ServerVariables["REMOTE _HOST"];
                AuditoriaBem.DataInsercao = DateTime.Now;
                AuditoriaBem.DetalhesOperacao = "Exclusao Tabela Bem, Registro: " + DescricaoBem;
                AuditoriaBem.Tabela = "TB_Bem";
                AuditoriaBem.Usuario = User.Identity.Name;
                AuditoriaBem.TipoOperacao = TipoOperacao.Exclusao.ToString();
                ContextoBem.Add<AuditoriaInterna>(AuditoriaBem);
                ContextoBem.SaveChanges();

                Bem BemExcluido = ContextoBem.Get<Bem>(BemParaExcluir.Id_Bem);

                ContextoBem.Delete<Bem>(BemExcluido);
                ContextoBem.SaveChanges();

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

        #endregion

        #region ConsultasUltimoeTodosBens

        public ActionResult LastProperty(Bem BemSalvo)
        {
            IList<Bem> LastProperty = ContextoBem.GetAll<Bem>()
                                      .Where(x => x.Id_Bem == BemSalvo.Id_Bem)
                                      .ToList();
            return View(LastProperty);
        }

        public ActionResult AllProperty()
        {
            IList<Bem> AllProperty = ContextoBem.GetAll<Bem>()
                                     .Where(x => x.Inativo == false && x.Descontinuado == false)
                                     .OrderBy(x => x.Descricao)
                                     .ToList();

            return View(AllProperty);
        }

        #endregion

        #region ChamadaPrincipalSistema
        public ActionResult CalculatesDepreciationOfAssets(string NomeUsuarioLogado)
        {
            try
            {

            }
                
            finally
            {

            }
            IList<Bem> ListaBensParaCalculoImpostos = ContextoBem.GetAll<Bem>()
                                                      .Where(x => x.Descontinuado == false && x.Inativo == false && x.DataInicioDepreciacao.Day == DateTime.Now.Day && x.BemDepreciavel == true && x.DepreciacaoAtiva == true)
                                                      .ToList();
            if (ListaBensParaCalculoImpostos.Count() > 0)
            {
                foreach (var itemBens in ListaBensParaCalculoImpostos)
                {
                    var VerificarValorDepreciado = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                                    .Where(x => x.IdBem.Id_Bem == itemBens.Id_Bem)
                                                    select c.ValorDepreciado).Sum();

                    if (VerificarValorDepreciado < itemBens.ValorDepreciavel)
                    {
                        try
                        {
                            switch (itemBens.TipoParaDepreciacao)
                            {
                                case TipoDepreciacao.Linear_Quotas_Constantes:
                                    CalcularDepreciacaoLinear((int)itemBens.Id_Bem, NomeUsuarioLogado);
                                    break;
                                case TipoDepreciacao.Soma_Dígitos:
                                    CalcularDepreciacaoSomadosDigitos((int)itemBens.Id_Bem, NomeUsuarioLogado);
                                    break;
                                case TipoDepreciacao.Reducao_de_Saldos:
                                    CalcularDepreciacaoReducaoSaldos((int)itemBens.Id_Bem, NomeUsuarioLogado);
                                    break;
                                case TipoDepreciacao.UnidadesProduzidas:
                                    CalcularDepreciacaoUnidadesProduzidas((int)itemBens.Id_Bem, NomeUsuarioLogado);
                                    break;
                                case TipoDepreciacao.Horas_Trabalhadas:
                                    CalcularDepreciacaoHorasTrabalhadas((int)itemBens.Id_Bem, NomeUsuarioLogado);
                                    break;
                                case TipoDepreciacao.Linear_Valor_Maximo_Depreciacao:
                                    CalcularDepreciacaoLinearComValorMaximoDepreciacao((int)itemBens.Id_Bem, NomeUsuarioLogado);
                                    break;
                                case TipoDepreciacao.Variacao_Taxas:
                                    CalcularDepreciacaoVariacaodasTaxas((int)itemBens.Id_Bem, NomeUsuarioLogado);
                                    break;
                                default:
                                    break;
                            }

                        }
                        finally
                        {
                            
                        }
                       

                    }
                    else
                    {
                        Bem BemParaAtualizarDepreciacao = ContextoBem.Get<Bem>(itemBens.Id_Bem);
                        BemParaAtualizarDepreciacao.DepreciacaoAtiva = false;
                        ContextoBem.SaveChanges();
                    }
                }
            }
            return null;
        }

        public ActionResult RetornarParaPaginaInicial()
        {
            return View("Index", "Bem");
        }
        #endregion

        #region Consultas

        public ActionResult DepreciationsToDate()
        {
            ViewBag.Bens = new SelectList(ContextoBem.GetAll<Bem>().Where(x => x.Inativo == false && x.Descontinuado == false).OrderBy(x => x.Descricao), "Id_Bem", "Descricao");
            return View();
        }

        [HttpPost]
        public ActionResult DepreciationsToDate(ValidarData Datas, int? Bens)
        {
            if (Datas.DataFinal < Datas.DataInicial)
            {
                ViewBag.Bens = new SelectList(ContextoBem.GetAll<Bem>().OrderBy(x => x.Descricao), "Id_Bem", "Descricao");
                ViewBag.NomeDiv = "alert alert-block";
                ViewBag.Titulo = "Atenção";
                ViewBag.Mensagem = "A data final não pode ser menor que a inicial";

                return View();
            }
            if (Bens == null)
            {
                IList<DepreciacaoBem> TodasDepreciacoes = ContextoBem.GetAll<DepreciacaoBem>()
                                                          .Where(x => x.DataDepreciacaoBem >= Datas.DataInicial && x.DataDepreciacaoBem <= Datas.DataFinal)
                                                          .ToList();
                ViewBag.ValorDepreciado = (from c in TodasDepreciacoes
                                           select c.ValorDepreciado).Sum();
                ViewBag.ValorCofins = (from c in TodasDepreciacoes
                                       select c.ValorCofins).Sum();
                ViewBag.ValorPis = (from c in TodasDepreciacoes
                                    select c.ValorPis).Sum();

                return View("ListDepreciations", TodasDepreciacoes);

            }
            IList<DepreciacaoBem> TodasDepreciacaoComBens = ContextoBem.GetAll<DepreciacaoBem>()
                                                           .Where(x => x.DataDepreciacaoBem >= Datas.DataInicial && x.DataDepreciacaoBem <= Datas.DataFinal && x.IdBem.Id_Bem == Bens)
                                                           .ToList();

            ViewBag.ValorDepreciado = (from c in TodasDepreciacaoComBens
                                       select c.ValorDepreciado).Sum();
            ViewBag.ValorCofins = (from c in TodasDepreciacaoComBens
                                   select c.ValorCofins).Sum();
            ViewBag.ValorPis = (from c in TodasDepreciacaoComBens
                                select c.ValorPis).Sum();

            return View("ListDepreciations", TodasDepreciacaoComBens);
        }

        public ActionResult ListDepreciations()
        {
            return View();
        }

        public ActionResult CheckHistoricalDepreciation(int id)
        {
            IList<DepreciacaoBem> ListHistoricoBemDepreciado = ContextoBem.GetAll<DepreciacaoBem>()
                                                               .Where(x => x.IdBem.Id_Bem == id)
                                                               .ToList();
            ViewBag.NomeBem = (from c in ContextoBem.GetAll<Bem>()
                               .Where(x => x.Id_Bem == id)
                               select c.Descricao).First();

            return View(ListHistoricoBemDepreciado);
        }

        [HttpGet]
        public ActionResult GraficoBemPorValores()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GraficoBemPorValores(Graficos Grafico)
        {
            int numero = (int)Grafico.NumeroElementos;
            int largura = (int)Grafico.Largura;
            int altura = (int)Grafico.Altura;
            string tipo = Grafico.TipoGrafico.ToString();
            string cor = Grafico.Cor.ToString();

            var Bens = (from c in ContextoBem.GetAll<Bem>()
                        .Where(x => x.Inativo == false && x.Descontinuado == false)
                        .Take((int)Grafico.NumeroElementos)
                        select new { Descricao = c.Descricao, ValorCompra = c.ValorCompra }).ToList();

            var GraficoBem = new Chart(width: largura, height: altura)
            .AddTitle("Gráfico Valores dos bens")
            .AddSeries(
            name: "Bens",
            chartType: tipo,
            legend: "gráfico de bens")
                //.DataBindCrossTable(Bens,"Descricao","Descricao","ValorCompra",null,"Ascending")
            .DataBindTable(Bens, "Descricao")
            .Write();


            return View("VerGraficoValorBem");
        }

        public ActionResult VerGraficoValorBem()
        {
            return View();
        }


        public ActionResult ShowAmountProperty()
        {
            IList<Bem> BemValorPatrimonial = (from c in ContextoBem.GetAll<Bem>()
                                       .Where(x => x.Inativo == false && x.Descontinuado == false)
                                              select c)
                                       .ToList();

            return View(BemValorPatrimonial);
        }


        public ActionResult ShowValueofAssetsbyCategory(int id)
        {
            IList<Bem> ValorDeBensPorCategoria = (from c in ContextoBem.GetAll<Bem>()
                                                  .Where(x => x.Inativo == false && x.Descontinuado == false && x.IdCategoria.Id_Categoria == id)
                                                  select c)
                                                  .ToList();
            return View(ValorDeBensPorCategoria);
        }

        public ActionResult PisCofinsToDate()
        {
            ViewBag.Bens = new SelectList(ContextoBem.GetAll<Bem>().Where(x => x.Inativo == false && x.Descontinuado == false).OrderBy(x => x.Descricao), "Id_Bem", "Descricao");
            return View();
        }

        [HttpPost]
        public ActionResult PisCofinsToDate(ValidarData Datas, int? Bens)
        {
            if (Datas.DataFinal < Datas.DataInicial)
            {
                ViewBag.Bens = new SelectList(ContextoBem.GetAll<Bem>().OrderBy(x => x.Descricao), "Id_Bem", "Descricao");
                ViewBag.NomeDiv = "alert alert-block";
                ViewBag.Titulo = "Atenção";
                ViewBag.Mensagem = "A data final não pode ser menor que a inicial";

                return View();
            }

            if (Bens == null)
            {
                IList<DepreciacaoBem> ListPisToCofins = ContextoBem.GetAll<DepreciacaoBem>()
                                                        .Where(x => x.DataDepreciacaoBem >= Datas.DataInicial && x.DataDepreciacaoBem <= Datas.DataFinal)
                                                        .ToList();
                ViewBag.ValorCofins = (from c in ListPisToCofins
                                       select c.ValorCofins).Sum();
                ViewBag.ValorPis = (from c in ListPisToCofins
                                    select c.ValorPis).Sum();

                return View("ListPisCofins", ListPisToCofins);


            }
            else
            {
                IList<DepreciacaoBem> ListPisToCofins = ContextoBem.GetAll<DepreciacaoBem>()
                                                       .Where(x => x.DataDepreciacaoBem >= Datas.DataInicial && x.DataDepreciacaoBem <= Datas.DataFinal && x.IdBem.Id_Bem == Bens)
                                                       .ToList();
                ViewBag.ValorCofins = (from c in ListPisToCofins
                                       select c.ValorCofins).Sum();
                ViewBag.ValorPis = (from c in ListPisToCofins
                                    select c.ValorPis).Sum();

                return View("ListPisCofins", ListPisToCofins);
            }


        }

        public ActionResult ListPisCofins()
        {
            return View();
        }

        #endregion

        #region StatusProperty

        public ActionResult StatusProperty()
        {
            IList<Bem> Bens = ContextoBem.GetAll<Bem>().AsParallel()
                              .ToList();
            decimal TodosBens = Bens.Count();

            decimal Inativos = (from c in Bens
                               .Where(x => x.Inativo == true)
                                select c).Count();
            decimal Depreciavel = (from c in Bens
                                   .Where(x => x.BemDepreciavel == true)
                                   select c).Count();
            decimal Ativos = (from c in Bens
                              .Where(x => x.Inativo == false)
                              select c).Count();
            decimal NaoDepreciavel = (from c in Bens
                                   .Where(x => x.BemDepreciavel == false)
                                      select c).Count();
            decimal Descontinuado = (from c in Bens
                                  .Where(x => x.Descontinuado == true)
                                     select c).Count();
            decimal NaoDescontinuado = (from c in Bens
                                    .Where(x => x.Descontinuado == false)
                                        select c).Count();//ok
            decimal DepreciacaoAtiva = (from c in Bens
                                    .Where(x => x.DepreciacaoAtiva == true)
                                        select c).Count();//ok
            decimal DepreciacaoInativa = (from c in Bens
                                      .Where(x => x.DepreciacaoAtiva == false)
                                          select c).Count();
            double ValorTotal = (from c in Bens
                                 select c.ValorCompra).Sum();
            double ValorAtivos = (from c in Bens
                                   .Where(x => x.Inativo == false)
                                  select c.ValorCompra).Sum();
            double ValorInativos = (from c in Bens
                                   .Where(x => x.Inativo == true)
                                    select c.ValorCompra).Sum();
            double ValorDescontinuado = (from c in Bens
                                        .Where(x => x.Descontinuado == true)
                                         select c.ValorCompra).Sum();
            double ValorNaoDescontinuado = (from c in Bens
                                             .Where(x => x.Descontinuado == false)
                                            select c.ValorCompra).Sum();
            double ValorNaoDepreciavel = (from c in Bens
                                          .Where(x => x.BemDepreciavel == false)
                                          select c.ValorCompra).Sum();
            double ValorDepreciavel = (from c in Bens
                                       .Where(x => x.BemDepreciavel == true)
                                       select c.ValorCompra).Sum();
            double ValorDepreciacaoAtiva = (from c in Bens
                                           .Where(x => x.DepreciacaoAtiva == true)
                                            select c.ValorCompra).Sum();
            double ValorDepreciacaoinativa = (from c in Bens
                                           .Where(x => x.DepreciacaoAtiva == false)
                                              select c.ValorCompra).Sum();
            double FezManutencao = (from c in ContextoBem.GetAll<ManutencaoBem>()
                                    .Where(x => x.IdBem.Id_Bem != 0)
                                    select c).Count();
            decimal Novos = (from c in Bens
                             .Where(x => x.IdEstadoConservacao.Descricao.StartsWith("Novo"))
                             select c).Count();
            decimal Usados = (from c in Bens
                             .Where(x => x.IdEstadoConservacao.Descricao.StartsWith("Usado"))
                              select c).Count();

            ViewBag.Novos = Novos >= 0 ? Novos : 0;//ok
            ViewBag.Usados = Usados >= 0 ? Usados : 0;//ok
            ViewBag.FezManutencao = FezManutencao >= 0 ? FezManutencao : 0; // 0
            ViewBag.TodosBens = TodosBens >= 0 ? TodosBens : 0;
            ViewBag.Ativos = Ativos >= 0 ? Ativos : 0;//ok
            ViewBag.Inativos = Inativos >= 0 ? Inativos : 0;//ok
            ViewBag.Depreciavel = Depreciavel >= 0 ? Depreciavel : 0;//ok
            ViewBag.NaoDepreciavel = NaoDepreciavel >= 0 ? NaoDepreciavel : 0;//ok
            ViewBag.Descontinuado = Descontinuado >= 0 ? Descontinuado : 0;//ok
            ViewBag.NaoDescontinuado = NaoDescontinuado >= 0 ? NaoDescontinuado : 0;//ok
            ViewBag.DepreciacaoAtiva = DepreciacaoAtiva >= 0 ? DepreciacaoAtiva : 0;//ok
            ViewBag.DepreciacaoInativa = DepreciacaoInativa >= 0 ? DepreciacaoInativa : 0;//ok
            ViewBag.ValorTotal = ValorTotal >= 0 ? ValorTotal : 0;//ok
            ViewBag.ValorAtivos = ValorAtivos >= 0 ? ValorAtivos : 0;//ok
            ViewBag.ValorInativos = ValorInativos >= 0 ? ValorInativos : 0;//ok
            ViewBag.ValorDescontinuado = ValorDescontinuado >= 0 ? ValorDescontinuado : 0;//ok
            ViewBag.ValorNaoDescontinuado = ValorNaoDescontinuado >= 0 ? ValorNaoDescontinuado : 0;
            ViewBag.ValorNaoDepreciavel = ValorNaoDepreciavel >= 0 ? ValorNaoDepreciavel : 0;//ok
            ViewBag.ValorDepreciavel = ValorDepreciavel >= 0 ? ValorDepreciavel : 0;//ok
            ViewBag.ValorDepreciacaoAtiva = ValorDepreciacaoAtiva >= 0 ? ValorDepreciacaoAtiva : 0;//ok
            ViewBag.ValorDepreciacaoinativa = ValorDepreciacaoinativa >= 0 ? ValorDepreciacaoinativa : 0;//ok
            ViewBag.PorcentagemInativos = ((TodosBens * Inativos) / 100) >= 0 ? ((TodosBens * Inativos) / 100) : 0;//ok

            ContextoBem.Dispose();


            return View();
        }

        #endregion

        #region BensCompradosNoPeriodo

        public ActionResult BuyToDate()
        {
            ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>().OrderBy(x => x.RazaoSocial), "Id_Pessoa", "RazaoSocial");
            return View();
        }

        [HttpPost]
        public ActionResult BuyToDate(ValidarData Datas, int? Fornecedor)
        {
            if (Datas.DataFinal < Datas.DataInicial)
            {
                ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>().OrderBy(x => x.RazaoSocial), "Id_Pessoa", "RazaoSocial");
                ViewBag.NomeDiv = "alert alert-block";
                ViewBag.Titulo = "Atenção";
                ViewBag.Mensagem = "A data final não pode ser menor que a inicial";

                return View();
            }

            if (Fornecedor == null)
            {
                IList<Bem> BensCompradosPeriodo = ContextoBem.GetAll<Bem>()
                                                  .Where(x => x.DataAquisicao >= Datas.DataInicial && x.DataAquisicao <= Datas.DataFinal)
                                                  .ToList();
                return View("BensCompradosPeriodo", BensCompradosPeriodo);

            }

            else
            {
                IList<Bem> BensCompradosPeriodo = ContextoBem.GetAll<Bem>()
                                                 .Where(x => x.DataAquisicao >= Datas.DataInicial && x.DataAquisicao <= Datas.DataFinal && x.IdFornecedor.Id_Pessoa == Fornecedor)
                                                 .ToList();
                return View("BensCompradosPeriodo", BensCompradosPeriodo);

            }



        }


        public ActionResult BensCompradosPeriodo()
        {
            return View();
        }

        #endregion

        #region BensPorResponsavel

        public ActionResult PropertyByResponsable()
        {
            ViewBag.Responsavel = new SelectList(ContextoBem.GetAll<Responsavel>().OrderBy(x => x.Nome), "Id_Pessoa", "Nome");
            return View();
        }


        [HttpPost]
        public ActionResult PropertyByResponsable(ValidarData Datas, int? Responsavel)
        {
            if (Datas.DataFinal < Datas.DataInicial)
            {
                ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Responsavel>().OrderByDescending(x => x.Nome), "Id_Pessoa", "Nome");
                ViewBag.NomeDiv = "alert alert-block";
                ViewBag.Titulo = "Atenção";
                ViewBag.Mensagem = "A data final não pode ser menor que a inicial";

                return View();
            }

            if (Responsavel == null)
            {
                IList<Bem> BensPorTodosResponsaveis = ContextoBem.GetAll<Bem>()
                                                  .Where(x => x.DataAquisicao >= Datas.DataInicial && x.DataAquisicao <= Datas.DataFinal)
                                                  .ToList();
                return View("BensPorResponsavel", BensPorTodosResponsaveis);

            }

            else
            {
                IList<Bem> BensParaEsteResponsavel = ContextoBem.GetAll<Bem>()
                                                 .Where(x => x.DataAquisicao >= Datas.DataInicial && x.DataAquisicao <= Datas.DataFinal && x.IdFornecedor.Id_Pessoa == Responsavel)
                                                 .ToList();
                return View("BensPorResponsavel", BensParaEsteResponsavel);

            }


        }
        public ActionResult BensPorResponsavel()
        {
            return View();
        }

        #endregion

        #region BensPorFornecedor

        public ActionResult PropertyToProvider()
        {
            ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>().OrderBy(x => x.RazaoSocial), "Id_Pessoa", "RazaoSocial");
            return View();
        }

        [HttpPost]
        public ActionResult PropertyToProvider(ValidarData Datas, int? Fornecedor)
        {
            if (Datas.DataFinal < Datas.DataInicial)
            {
                ViewBag.Fornecedor = new SelectList(ContextoBem.GetAll<Fornecedor>().OrderBy(x => x.RazaoSocial), "Id_Pessoa", "RazaoSocial");
                ViewBag.NomeDiv = "alert alert-block";
                ViewBag.Titulo = "Atenção";
                ViewBag.Mensagem = "A data final não pode ser menor que a inicial";

                return View();
            }

            if (Fornecedor == null)
            {
                IList<Bem> BensCompradosPeriodo = ContextoBem.GetAll<Bem>()
                                                  .Where(x => x.DataAquisicao >= Datas.DataInicial && x.DataAquisicao <= Datas.DataFinal)
                                                  .ToList();
                return View("BensCompradosPeriodo", BensCompradosPeriodo);

            }

            else
            {
                IList<Bem> BensCompradosPeriodo = ContextoBem.GetAll<Bem>()
                                                 .Where(x => x.DataAquisicao >= Datas.DataInicial && x.DataAquisicao <= Datas.DataFinal && x.IdFornecedor.Id_Pessoa == Fornecedor)
                                                 .ToList();
                return View("BensCompradosPeriodo", BensCompradosPeriodo);

            }



        }

        #endregion

        #region Metodos

        // 100% OK com lançamento contábil
        private void CalcularDepreciacaoLinear(int id, string NomeUsuario)
        {
            DepreciacaoBem DepreciacaoVerUltimaData = VerificarUltimaDepreciacaoBem(id);

            if (DepreciacaoVerUltimaData.DataDepreciacaoBem < DateTime.Now)
            {
                Bem BemParaAtualizarImpostos = ContextoBem.Get<Bem>(id);

                var VerificarValorDepreciado = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                                       .Where(x => x.IdBem.Id_Bem == id)
                                                select c.ValorDepreciado).Sum();
                var VerificarValorDepreciavel = (from c in ContextoBem.GetAll<Bem>()
                                            .Where(x => x.Id_Bem == id)
                                                 select c.ValorDepreciavel).First();

                if (VerificarValorDepreciado < VerificarValorDepreciavel)
                {

                    AuditoriaInterna AuditoriaCoeficienteUm = new AuditoriaInterna();
                    AuditoriaCoeficienteUm.Computador = Environment.MachineName;
                    AuditoriaCoeficienteUm.DataInsercao = DateTime.Now;
                    AuditoriaCoeficienteUm.DetalhesOperacao = "Insercao Tabela Depreciacao Bem Registro: " + BemParaAtualizarImpostos.Descricao;
                    AuditoriaCoeficienteUm.Tabela = "TB_DepreciacaoBem";
                    AuditoriaCoeficienteUm.TipoOperacao = TipoOperacao.Insercao.ToString();
                    AuditoriaCoeficienteUm.Usuario = NomeUsuario;

                    ContextoBem.Add<AuditoriaInterna>(AuditoriaCoeficienteUm);
                    ContextoBem.SaveChanges();

                    DepreciacaoBem DepreciacaoBemCoeficienteUm = new DepreciacaoBem();

                    DepreciacaoBemCoeficienteUm.DataDepreciacaoBem = DateTime.Now;
                    DepreciacaoBemCoeficienteUm.DepreciacaoFeita = true;
                    DepreciacaoBemCoeficienteUm.TaxaDepreciacao = DepreciacaoBemCoeficienteUm.CalculaTaxaMensal(BemParaAtualizarImpostos.CoeficienteDepreciacao, BemParaAtualizarImpostos.TaxaDepreciacaoAnual);
                    DepreciacaoBemCoeficienteUm.ValorCofins = DepreciacaoBemCoeficienteUm.CalculaCofinsMensal(BemParaAtualizarImpostos.ValorContabil, BemParaAtualizarImpostos.Cofins);
                    DepreciacaoBemCoeficienteUm.ValorPis = DepreciacaoBemCoeficienteUm.CalculaPisMensal(BemParaAtualizarImpostos.ValorContabil, BemParaAtualizarImpostos.Pis);
                    DepreciacaoBemCoeficienteUm.ValorDepreciado = DepreciacaoBemCoeficienteUm.CalculaDepreciacaoLinearMensal(BemParaAtualizarImpostos.TaxaDepreciacaoAnual, BemParaAtualizarImpostos.CoeficienteDepreciacao, BemParaAtualizarImpostos.ValorDepreciavel);
                    DepreciacaoBemCoeficienteUm.IdAuditoriaInterna = ContextoBem.Get<AuditoriaInterna>(AuditoriaCoeficienteUm.Id_AuditoriaInterna);
                    DepreciacaoBemCoeficienteUm.IdBem = ContextoBem.Get<Bem>(id);
                    DepreciacaoBemCoeficienteUm.TipoParaDepreciacao = BemParaAtualizarImpostos.TipoParaDepreciacao;
                    ContextoBem.Add<DepreciacaoBem>(DepreciacaoBemCoeficienteUm);
                    ContextoBem.SaveChanges();


                    BemParaAtualizarImpostos.ValorDepreciado = DepreciacaoBemCoeficienteUm.ValorDepreciado;
                    BemParaAtualizarImpostos.ValorAtual = BemParaAtualizarImpostos.ValorContabil - BemParaAtualizarImpostos.ValorDepreciado;
                    BemParaAtualizarImpostos.ValorContabil = BemParaAtualizarImpostos.ValorContabil - BemParaAtualizarImpostos.ValorDepreciado;

                    ContextoBem.SaveChanges();

                    LancamentoDepreciacao LancamentoContabil = new LancamentoDepreciacao();
                    LancamentoContabil.Credito = DepreciacaoBemCoeficienteUm.ValorDepreciado;
                    LancamentoContabil.Debito = DepreciacaoBemCoeficienteUm.ValorDepreciado * -1;
                    LancamentoContabil.DataLancamento = DateTime.Now;
                    ContextoBem.Add<LancamentoDepreciacao>(LancamentoContabil);
                    ContextoBem.SaveChanges();

                }



            }

        }
        // 100% OK com lançamento contábil
        private void CalcularDepreciacaoVariacaodasTaxas(int id, string NomeUsuario)
        {
            DepreciacaoBem DepreciacaoBemUltimaDesteBem = VerificarUltimaDepreciacaoBem(id);
            if (DepreciacaoBemUltimaDesteBem.DataDepreciacaoBem < DateTime.Now)
            {
                var VerificarValorDepreciado = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                                  .Where(x => x.IdBem.Id_Bem == id)
                                                select c.ValorDepreciado).Sum();
                var VerificarValorDepreciavel = (from c in ContextoBem.GetAll<Bem>()
                                            .Where(x => x.Id_Bem == id)
                                                 select c.ValorDepreciavel).First();

                if (VerificarValorDepreciado < VerificarValorDepreciavel)
                {
                    Bem BemParaAtualizarImpostos = ContextoBem.Get<Bem>(id);


                    AuditoriaInterna AuditoriaCoeficienteUm = new AuditoriaInterna();
                    AuditoriaCoeficienteUm.Computador = Environment.MachineName;
                    AuditoriaCoeficienteUm.DataInsercao = DateTime.Now;
                    AuditoriaCoeficienteUm.DetalhesOperacao = "Insercao Tabela Depreciacao Bem Registro: " + BemParaAtualizarImpostos.Descricao;
                    AuditoriaCoeficienteUm.Tabela = "TB_DepreciacaoBem";
                    AuditoriaCoeficienteUm.TipoOperacao = TipoOperacao.Insercao.ToString();
                    AuditoriaCoeficienteUm.Usuario = NomeUsuario;

                    ContextoBem.Add<AuditoriaInterna>(AuditoriaCoeficienteUm);
                    ContextoBem.SaveChanges();

                    DepreciacaoBem DepreciacaoBemCoeficienteUm = new DepreciacaoBem();

                    DepreciacaoBemCoeficienteUm.DataDepreciacaoBem = DateTime.Now;
                    DepreciacaoBemCoeficienteUm.DepreciacaoFeita = true;
                    DepreciacaoBemCoeficienteUm.TaxaDepreciacao = DepreciacaoBemCoeficienteUm.CalculaTaxaMensal(BemParaAtualizarImpostos.CoeficienteDepreciacao, BemParaAtualizarImpostos.TaxaDepreciacaoAnual);
                    DepreciacaoBemCoeficienteUm.ValorCofins = DepreciacaoBemCoeficienteUm.CalculaCofinsMensal(BemParaAtualizarImpostos.ValorContabil, BemParaAtualizarImpostos.Cofins);
                    DepreciacaoBemCoeficienteUm.ValorPis = DepreciacaoBemCoeficienteUm.CalculaPisMensal(BemParaAtualizarImpostos.ValorContabil, BemParaAtualizarImpostos.Pis);
                    DepreciacaoBemCoeficienteUm.ValorDepreciado = DepreciacaoBemCoeficienteUm.CalculaDepreciacaoVariacaoDasTaxasMensal(BemParaAtualizarImpostos.ValorDepreciavel, CalcularTaxaVidaUtilRestanteDepreciacaoVariacaoTaxas(id));
                    DepreciacaoBemCoeficienteUm.IdAuditoriaInterna = ContextoBem.Get<AuditoriaInterna>(AuditoriaCoeficienteUm.Id_AuditoriaInterna);
                    DepreciacaoBemCoeficienteUm.IdBem = ContextoBem.Get<Bem>(id);
                    DepreciacaoBemCoeficienteUm.TipoParaDepreciacao = BemParaAtualizarImpostos.TipoParaDepreciacao;
                    ContextoBem.Add<DepreciacaoBem>(DepreciacaoBemCoeficienteUm);
                    ContextoBem.SaveChanges();

                    BemParaAtualizarImpostos.ValorDepreciado = DepreciacaoBemCoeficienteUm.ValorDepreciado;
                    BemParaAtualizarImpostos.ValorAtual = BemParaAtualizarImpostos.ValorContabil - BemParaAtualizarImpostos.ValorDepreciado;
                    BemParaAtualizarImpostos.ValorContabil = BemParaAtualizarImpostos.ValorAtual;
                    BemParaAtualizarImpostos.ValorDepreciavel = BemParaAtualizarImpostos.ValorContabil - BemParaAtualizarImpostos.ValorResidual;

                    ContextoBem.SaveChanges();

                    LancamentoDepreciacao LancamentoContabil = new LancamentoDepreciacao();
                    LancamentoContabil.Credito = DepreciacaoBemCoeficienteUm.ValorDepreciado;
                    LancamentoContabil.Debito = DepreciacaoBemCoeficienteUm.ValorDepreciado * -1;
                    LancamentoContabil.DataLancamento = DateTime.Now;
                    ContextoBem.Add<LancamentoDepreciacao>(LancamentoContabil);
                    ContextoBem.SaveChanges();

                }



            }

        }
        // 100% Ok com lancamento contábil
        private void CalcularDepreciacaoSomadosDigitos(int id, string NomeUsuario)
        {
            DepreciacaoBem UltimaDepreciacaoBem = VerificarUltimaDepreciacaoBem(id);
            if (UltimaDepreciacaoBem.DataDepreciacaoBem < DateTime.Now)
            {
                double NumeroDeMesesVidaUtil = CalcularNumeroMesesVidaUtil(id);//15

                double SomaDigitosMesesVidaUtil = CalcularSomaDigitoMesesVidaUtil(NumeroDeMesesVidaUtil);

                Bem BemParaCalcularSomaDigitos = ContextoBem.Get<Bem>(id);



                AuditoriaInterna Auditoria = new AuditoriaInterna();
                Auditoria.Computador = Environment.MachineName;
                Auditoria.DataInsercao = DateTime.Now;
                Auditoria.DetalhesOperacao = "Insercao Tabela Depreciacao Bem Registro: " + BemParaCalcularSomaDigitos.Descricao;
                Auditoria.Tabela = "TB_DepreciacaoBem";
                Auditoria.TipoOperacao = TipoOperacao.Insercao.ToString();
                Auditoria.Usuario = NomeUsuario;
                ContextoBem.Add<AuditoriaInterna>(Auditoria);
                ContextoBem.SaveChanges();

                TimeSpan DiferencaDeMeses = Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(BemParaCalcularSomaDigitos.DataInicioDepreciacao.ToString("yyyy/MM/dd"));
                decimal Meses = Convert.ToDecimal(DiferencaDeMeses.Days);
                decimal NumeroMeses = Math.Round((Meses / 30), 0);
                var VerificarValorDepreciado = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                                   .Where(x => x.IdBem.Id_Bem == BemParaCalcularSomaDigitos.Id_Bem)
                                                select c.ValorDepreciado).Sum();
                long IdUltimaDepreciacao = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                                         .Where(x => x.IdBem.Id_Bem == BemParaCalcularSomaDigitos.Id_Bem)
                                            select c.Id_DepreciacaoBem).Max();

                DateTime UltimoMes = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                      .Where(x => x.Id_DepreciacaoBem == IdUltimaDepreciacao)
                                      select c.DataDepreciacaoBem).First();
                DepreciacaoBem Depreciacao = new DepreciacaoBem();

                for (int indeceNumeroMeses = 1; indeceNumeroMeses <= NumeroMeses; indeceNumeroMeses++)
                {

                    if (VerificarValorDepreciado < BemParaCalcularSomaDigitos.ValorDepreciavel)
                    {

                        if (UltimoMes.Month == 12 && UltimoMes.Year < DateTime.Now.Year)
                        {
                            Depreciacao.DataDepreciacaoBem = UltimoMes.AddMonths(1).AddYears(1);
                        }
                        Depreciacao.DataDepreciacaoBem = UltimoMes.AddMonths(1);

                        if (Depreciacao.DataDepreciacaoBem < DateTime.Now)
                        {

                            Depreciacao.ValorCofins = Depreciacao.CalculaCofinsMensal(BemParaCalcularSomaDigitos.ValorContabil, BemParaCalcularSomaDigitos.Cofins);
                            Depreciacao.ValorPis = Depreciacao.CalculaPisMensal(BemParaCalcularSomaDigitos.ValorContabil, BemParaCalcularSomaDigitos.Pis);

                            Depreciacao.ValorDepreciado = Depreciacao.CalculaDepreciacaoSomaDigitosMensal(BemParaCalcularSomaDigitos.VidaUtil, SomaDigitosMesesVidaUtil, BemParaCalcularSomaDigitos.ValorDepreciavel); //(TotalDiferencaDeMeses / SomaDigitosMesesVidaUtil) * BemParaCalcularSomaDigitos.ValorCompra;
                            Depreciacao.IdBem = ContextoBem.Get<Bem>(BemParaCalcularSomaDigitos.Id_Bem);
                            Depreciacao.IdAuditoriaInterna = ContextoBem.Get<AuditoriaInterna>(Auditoria.Id_AuditoriaInterna);

                            Depreciacao.TaxaDepreciacao = Depreciacao.CalculaTaxaMensal(BemParaCalcularSomaDigitos.CoeficienteDepreciacao, BemParaCalcularSomaDigitos.TaxaDepreciacaoAnual);
                            Depreciacao.DepreciacaoFeita = true;
                            Depreciacao.TipoParaDepreciacao = BemParaCalcularSomaDigitos.TipoParaDepreciacao;

                            ContextoBem.Add<DepreciacaoBem>(Depreciacao);
                            ContextoBem.SaveChanges();

                            BemParaCalcularSomaDigitos.ValorDepreciado = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                                                          .Where(x => x.IdBem.Id_Bem == BemParaCalcularSomaDigitos.Id_Bem)
                                                                          select c.ValorDepreciado).Sum();

                            BemParaCalcularSomaDigitos.ValorDepreciado = Depreciacao.ValorDepreciado;
                            BemParaCalcularSomaDigitos.ValorAtual = BemParaCalcularSomaDigitos.ValorContabil - BemParaCalcularSomaDigitos.ValorDepreciado;
                            BemParaCalcularSomaDigitos.ValorContabil = BemParaCalcularSomaDigitos.ValorContabil - Depreciacao.ValorDepreciado;
                            BemParaCalcularSomaDigitos.VidaUtil = BemParaCalcularSomaDigitos.VidaUtil - 1;
                            ContextoBem.SaveChanges();

                            LancamentoDepreciacao LancamentoContabil = new LancamentoDepreciacao();
                            LancamentoContabil.Credito = Depreciacao.ValorDepreciado;
                            LancamentoContabil.Debito = Depreciacao.ValorDepreciado * -1;
                            LancamentoContabil.DataLancamento = DateTime.Now;
                            ContextoBem.Add<LancamentoDepreciacao>(LancamentoContabil);
                            ContextoBem.SaveChanges();

                        }


                    }

                }


            }

        }


        // 100% Ok com lancamento contábil
        private void CalcularDepreciacaoAtrasada(int id)
        {

            Bem BemParaCalculoDepreciacaoAtrasada = ContextoBem.Get<Bem>(id);

            AuditoriaInterna Auditoria = new AuditoriaInterna();
            Auditoria.Computador = Environment.MachineName;
            Auditoria.DataInsercao = DateTime.Now;
            Auditoria.DetalhesOperacao = "Insercao Tabela Depreciacao Bem Registro: " + BemParaCalculoDepreciacaoAtrasada.Descricao;
            Auditoria.Tabela = "TB_DepreciacaoBem";
            Auditoria.TipoOperacao = TipoOperacao.Insercao.ToString();
            Auditoria.Usuario = User.Identity.Name;
            ContextoBem.Add<AuditoriaInterna>(Auditoria);
            ContextoBem.SaveChanges();

            TimeSpan DiferencaDeMeses = Convert.ToDateTime(DateTime.Now) - Convert.ToDateTime(BemParaCalculoDepreciacaoAtrasada.DataInicioDepreciacao.ToString("yyyy/MM/dd HH:mm:ss"));
            decimal Meses = Convert.ToDecimal(DiferencaDeMeses.Days);
            decimal NumeroMeses = Math.Round((Meses / 30), 0) - 1;

            for (int i = 1; i <= NumeroMeses; i++)
            {

                var VerificarValorDepreciado = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                                .Where(x => x.IdBem.Id_Bem == BemParaCalculoDepreciacaoAtrasada.Id_Bem)
                                                select c.ValorDepreciado).Sum();

                if (VerificarValorDepreciado < BemParaCalculoDepreciacaoAtrasada.ValorDepreciavel)
                {
                    DepreciacaoBem Depreciacao = new DepreciacaoBem();

                    long IdUltimaDepreciacao = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                                .Where(x => x.IdBem.Id_Bem == BemParaCalculoDepreciacaoAtrasada.Id_Bem)
                                                select c.Id_DepreciacaoBem).Max();
                    DateTime UltimoMes = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                          .Where(x => x.Id_DepreciacaoBem == IdUltimaDepreciacao)
                                          select c.DataDepreciacaoBem).First();
                    var NumeroDepreciacaoesFeitas = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                                            .Where(x => x.IdBem.Id_Bem == BemParaCalculoDepreciacaoAtrasada.Id_Bem && x.ValorDepreciado > 0)
                                                     select c).ToList();
                    var NumerodeMesesQuejaForamFeitasDepreciacoesAtivas = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                                                          .Where(x => x.IdBem.Id_Bem == id && x.DepreciacaoFeita == true)
                                                                           select c);
                    double NumerodeMesesQuejaForamFeitasDepreciacoes = NumerodeMesesQuejaForamFeitasDepreciacoesAtivas.Count();

                    double TotalDiferencaDeMeses = CalcularNumeroMesesVidaUtil((int)BemParaCalculoDepreciacaoAtrasada.Id_Bem) - Convert.ToDouble(NumeroDepreciacaoesFeitas.Count());

                    Depreciacao.ValorCofins = Depreciacao.CalculaCofinsMensal(BemParaCalculoDepreciacaoAtrasada.ValorContabil, BemParaCalculoDepreciacaoAtrasada.Cofins);
                    Depreciacao.ValorPis = Depreciacao.CalculaPisMensal(BemParaCalculoDepreciacaoAtrasada.ValorContabil, BemParaCalculoDepreciacaoAtrasada.Pis);
                    switch (BemParaCalculoDepreciacaoAtrasada.TipoParaDepreciacao)
                    {
                        case TipoDepreciacao.Linear_Quotas_Constantes:
                            Depreciacao.ValorDepreciado = Depreciacao.CalculaDepreciacaoLinearMensal(BemParaCalculoDepreciacaoAtrasada.TaxaDepreciacaoAnual, BemParaCalculoDepreciacaoAtrasada.CoeficienteDepreciacao, BemParaCalculoDepreciacaoAtrasada.ValorDepreciavel);
                            break;
                        case TipoDepreciacao.Soma_Dígitos:
                            Depreciacao.ValorDepreciado = Depreciacao.CalculaDepreciacaoSomaDigitosMensal(TotalDiferencaDeMeses, CalcularSomaDigitoMesesVidaUtil(CalcularNumeroMesesVidaUtil((int)BemParaCalculoDepreciacaoAtrasada.Id_Bem)), BemParaCalculoDepreciacaoAtrasada.ValorDepreciavel);

                            break;
                        case TipoDepreciacao.Reducao_de_Saldos:
                            Depreciacao.ValorDepreciado = Depreciacao.CalcularDepreciacaoReducaoSaldosMensal(BemParaCalculoDepreciacaoAtrasada.ValorContabil, BemParaCalculoDepreciacaoAtrasada.ValorCompra, Depreciacao.CalculaVidaUtilAnos(BemParaCalculoDepreciacaoAtrasada.CoeficienteDepreciacao, BemParaCalculoDepreciacaoAtrasada.TaxaDepreciacaoAnual), BemParaCalculoDepreciacaoAtrasada.ValorResidual);
                            break;
                        case TipoDepreciacao.UnidadesProduzidas:
                            Depreciacao.ValorDepreciado = Depreciacao.CalcularDepreciacaoUnidadesProduzidasMensal(BemParaCalculoDepreciacaoAtrasada.UnidadesProduzidasPeriodo, BemParaCalculoDepreciacaoAtrasada.UnidadesEstimadasVidaUtil, BemParaCalculoDepreciacaoAtrasada.ValorContabil);
                            break;
                        case TipoDepreciacao.Horas_Trabalhadas:
                            Depreciacao.ValorDepreciado = Depreciacao.CalcularDepreciacaoHorasTrabalhadasMensal(BemParaCalculoDepreciacaoAtrasada.HorasTrabalhdadasPeriodo, BemParaCalculoDepreciacaoAtrasada.HorasEstimadaVidaUtil, BemParaCalculoDepreciacaoAtrasada.ValorContabil);
                            break;
                        case TipoDepreciacao.Linear_Valor_Maximo_Depreciacao:
                            Depreciacao.ValorDepreciado = Depreciacao.CalculaDepreciacaoLinearComValorMaximoDepreciacaoMensal(BemParaCalculoDepreciacaoAtrasada.TaxaDepreciacaoAnual, BemParaCalculoDepreciacaoAtrasada.CoeficienteDepreciacao, BemParaCalculoDepreciacaoAtrasada.ValorMaximoDepreciacao);
                            break;
                        case TipoDepreciacao.Variacao_Taxas:
                            Depreciacao.ValorDepreciado = Depreciacao.CalculaDepreciacaoVariacaoDasTaxasMensal(BemParaCalculoDepreciacaoAtrasada.ValorDepreciavel, CalcularNumeroMesesVidaUtil((int)BemParaCalculoDepreciacaoAtrasada.Id_Bem) - NumerodeMesesQuejaForamFeitasDepreciacoes);
                            break;
                        default:
                            break;
                    }

                    Depreciacao.IdBem = ContextoBem.Get<Bem>(BemParaCalculoDepreciacaoAtrasada.Id_Bem);
                    Depreciacao.IdAuditoriaInterna = ContextoBem.Get<AuditoriaInterna>(Auditoria.Id_AuditoriaInterna);
                    if (UltimoMes.Month == 12)
                    {
                        Depreciacao.DataDepreciacaoBem = UltimoMes.AddMonths(1).AddYears(1);
                    }

                    Depreciacao.DataDepreciacaoBem = UltimoMes.AddMonths(1);

                    if (Depreciacao.DataDepreciacaoBem.Month < DateTime.Now.Month + 1)
                    {
                        Depreciacao.DataDepreciacaoBem = UltimoMes.AddMonths(1);

                    }
                    Depreciacao.DepreciacaoFeita = true;
                    Depreciacao.TaxaDepreciacao = Depreciacao.CalculaTaxaMensal(BemParaCalculoDepreciacaoAtrasada.CoeficienteDepreciacao, BemParaCalculoDepreciacaoAtrasada.TaxaDepreciacaoAnual);
                    Depreciacao.TipoParaDepreciacao = BemParaCalculoDepreciacaoAtrasada.TipoParaDepreciacao;
                    ContextoBem.Add<DepreciacaoBem>(Depreciacao);
                    ContextoBem.SaveChanges();

                    BemParaCalculoDepreciacaoAtrasada.ValorDepreciado = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                                                         .Where(x => x.IdBem.Id_Bem == BemParaCalculoDepreciacaoAtrasada.Id_Bem)
                                                                         select c.ValorDepreciado).Sum();
                    switch (BemParaCalculoDepreciacaoAtrasada.TipoParaDepreciacao)
                    {
                        case TipoDepreciacao.Linear_Quotas_Constantes:
                            BemParaCalculoDepreciacaoAtrasada.ValorDepreciado = Depreciacao.ValorDepreciado;
                            BemParaCalculoDepreciacaoAtrasada.ValorAtual = BemParaCalculoDepreciacaoAtrasada.ValorContabil - BemParaCalculoDepreciacaoAtrasada.ValorDepreciado;
                            BemParaCalculoDepreciacaoAtrasada.ValorContabil = BemParaCalculoDepreciacaoAtrasada.ValorContabil - Depreciacao.ValorDepreciado;

                            break;
                        case TipoDepreciacao.Soma_Dígitos:
                            BemParaCalculoDepreciacaoAtrasada.ValorDepreciado = Depreciacao.ValorDepreciado;
                            BemParaCalculoDepreciacaoAtrasada.ValorAtual = BemParaCalculoDepreciacaoAtrasada.ValorContabil - BemParaCalculoDepreciacaoAtrasada.ValorDepreciado;
                            BemParaCalculoDepreciacaoAtrasada.ValorContabil = BemParaCalculoDepreciacaoAtrasada.ValorContabil - Depreciacao.ValorDepreciado;

                            break;
                        case TipoDepreciacao.Reducao_de_Saldos:
                            BemParaCalculoDepreciacaoAtrasada.ValorDepreciado = Depreciacao.ValorDepreciado;
                            BemParaCalculoDepreciacaoAtrasada.ValorAtual = BemParaCalculoDepreciacaoAtrasada.ValorContabil - BemParaCalculoDepreciacaoAtrasada.ValorDepreciado;
                            BemParaCalculoDepreciacaoAtrasada.ValorContabil = BemParaCalculoDepreciacaoAtrasada.ValorContabil - Depreciacao.ValorDepreciado;
                            BemParaCalculoDepreciacaoAtrasada.ValorDepreciavel = BemParaCalculoDepreciacaoAtrasada.ValorContabil - BemParaCalculoDepreciacaoAtrasada.ValorResidual;
                            break;
                        case TipoDepreciacao.UnidadesProduzidas:
                            BemParaCalculoDepreciacaoAtrasada.ValorDepreciado = Depreciacao.ValorDepreciado;
                            BemParaCalculoDepreciacaoAtrasada.ValorAtual = BemParaCalculoDepreciacaoAtrasada.ValorContabil - BemParaCalculoDepreciacaoAtrasada.ValorDepreciado;
                            BemParaCalculoDepreciacaoAtrasada.ValorContabil = BemParaCalculoDepreciacaoAtrasada.ValorContabil - Depreciacao.ValorDepreciado;
                            break;
                        case TipoDepreciacao.Horas_Trabalhadas:
                            BemParaCalculoDepreciacaoAtrasada.ValorDepreciado = Depreciacao.ValorDepreciado;
                            BemParaCalculoDepreciacaoAtrasada.ValorAtual = BemParaCalculoDepreciacaoAtrasada.ValorContabil - BemParaCalculoDepreciacaoAtrasada.ValorDepreciado;
                            BemParaCalculoDepreciacaoAtrasada.ValorContabil = BemParaCalculoDepreciacaoAtrasada.ValorContabil - Depreciacao.ValorDepreciado;
                            break;
                        case TipoDepreciacao.Linear_Valor_Maximo_Depreciacao:
                            BemParaCalculoDepreciacaoAtrasada.ValorDepreciado = Depreciacao.ValorDepreciado;
                            BemParaCalculoDepreciacaoAtrasada.ValorAtual = BemParaCalculoDepreciacaoAtrasada.ValorContabil - BemParaCalculoDepreciacaoAtrasada.ValorDepreciado;
                            BemParaCalculoDepreciacaoAtrasada.ValorDepreciavel = BemParaCalculoDepreciacaoAtrasada.ValorSalvamento;
                            break;
                        case TipoDepreciacao.Variacao_Taxas:
                            BemParaCalculoDepreciacaoAtrasada.ValorDepreciado = Depreciacao.ValorDepreciado;
                            BemParaCalculoDepreciacaoAtrasada.ValorAtual = BemParaCalculoDepreciacaoAtrasada.ValorContabil - BemParaCalculoDepreciacaoAtrasada.ValorDepreciado;
                            BemParaCalculoDepreciacaoAtrasada.ValorContabil = BemParaCalculoDepreciacaoAtrasada.ValorContabil - Depreciacao.ValorDepreciado;
                            BemParaCalculoDepreciacaoAtrasada.ValorDepreciavel = BemParaCalculoDepreciacaoAtrasada.ValorContabil - BemParaCalculoDepreciacaoAtrasada.ValorResidual;
                            break;
                        default:
                            BemParaCalculoDepreciacaoAtrasada.ValorDepreciado = Depreciacao.ValorDepreciado;
                            BemParaCalculoDepreciacaoAtrasada.ValorAtual = BemParaCalculoDepreciacaoAtrasada.ValorContabil - BemParaCalculoDepreciacaoAtrasada.ValorDepreciado;
                            BemParaCalculoDepreciacaoAtrasada.ValorContabil = BemParaCalculoDepreciacaoAtrasada.ValorContabil - Depreciacao.ValorDepreciado;
                            break;
                    }
                    ContextoBem.SaveChanges();

                    if (Depreciacao.DataDepreciacaoBem.Year == DateTime.Now.Year)
                    {
                        LancamentoDepreciacao LancamentoContabil = new LancamentoDepreciacao();
                        LancamentoContabil.Credito = Depreciacao.ValorDepreciado;
                        LancamentoContabil.Debito = Depreciacao.ValorDepreciado * -1;
                        LancamentoContabil.DataLancamento = DateTime.Now;
                        ContextoBem.Add<LancamentoDepreciacao>(LancamentoContabil);
                        ContextoBem.SaveChanges();


                    }

                }


            }



        }
        //100% ok com lancamento contábil
        private void CalcularDepreciacaoUnidadesProduzidas(int id, string NomeUsuario)
        {
            DepreciacaoBem UltimaDepreciacoBem = VerificarUltimaDepreciacaoBem(id);
            if (UltimaDepreciacoBem.DataDepreciacaoBem < DateTime.Now)
            {
                var VerificarValorDepreciado = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                                 .Where(x => x.IdBem.Id_Bem == id)
                                                select c.ValorDepreciado).Sum();
                var VerificarValorDepreciavel = (from c in ContextoBem.GetAll<Bem>()
                                            .Where(x => x.Id_Bem == id)
                                                 select c.ValorDepreciavel).First();

                if (VerificarValorDepreciado < VerificarValorDepreciavel)
                {
                    Bem BemParaAtualizarImpostos = ContextoBem.Get<Bem>(id);



                    AuditoriaInterna AuditoriaCoeficienteUm = new AuditoriaInterna();
                    AuditoriaCoeficienteUm.Computador = Environment.MachineName;
                    AuditoriaCoeficienteUm.DataInsercao = DateTime.Now;
                    AuditoriaCoeficienteUm.DetalhesOperacao = "Insercao Tabela Depreciacao Bem Registro: " + BemParaAtualizarImpostos.Descricao;
                    AuditoriaCoeficienteUm.Tabela = "TB_DepreciacaoBem";
                    AuditoriaCoeficienteUm.TipoOperacao = TipoOperacao.Insercao.ToString();
                    AuditoriaCoeficienteUm.Usuario = NomeUsuario;

                    ContextoBem.Add<AuditoriaInterna>(AuditoriaCoeficienteUm);
                    ContextoBem.SaveChanges();

                    DepreciacaoBem DepreciacaoBemCoeficienteUm = new DepreciacaoBem();

                    DepreciacaoBemCoeficienteUm.DataDepreciacaoBem = DateTime.Now;
                    DepreciacaoBemCoeficienteUm.DepreciacaoFeita = true;
                    DepreciacaoBemCoeficienteUm.TaxaDepreciacao = DepreciacaoBemCoeficienteUm.CalculaTaxaMensal(BemParaAtualizarImpostos.CoeficienteDepreciacao, BemParaAtualizarImpostos.TaxaDepreciacaoAnual);
                    DepreciacaoBemCoeficienteUm.ValorCofins = DepreciacaoBemCoeficienteUm.CalculaCofinsMensal(BemParaAtualizarImpostos.ValorContabil, BemParaAtualizarImpostos.Cofins);
                    DepreciacaoBemCoeficienteUm.ValorPis = DepreciacaoBemCoeficienteUm.CalculaPisMensal(BemParaAtualizarImpostos.ValorContabil, BemParaAtualizarImpostos.Pis);
                    DepreciacaoBemCoeficienteUm.ValorDepreciado = DepreciacaoBemCoeficienteUm.CalcularDepreciacaoUnidadesProduzidasMensal(BemParaAtualizarImpostos.UnidadesProduzidasPeriodo, BemParaAtualizarImpostos.UnidadesEstimadasVidaUtil, BemParaAtualizarImpostos.ValorContabil);
                    DepreciacaoBemCoeficienteUm.IdAuditoriaInterna = ContextoBem.Get<AuditoriaInterna>(AuditoriaCoeficienteUm.Id_AuditoriaInterna);
                    DepreciacaoBemCoeficienteUm.IdBem = ContextoBem.Get<Bem>(id);
                    DepreciacaoBemCoeficienteUm.TipoParaDepreciacao = BemParaAtualizarImpostos.TipoParaDepreciacao;
                    ContextoBem.Add<DepreciacaoBem>(DepreciacaoBemCoeficienteUm);
                    ContextoBem.SaveChanges();


                    BemParaAtualizarImpostos.ValorDepreciado = DepreciacaoBemCoeficienteUm.ValorDepreciado;
                    BemParaAtualizarImpostos.ValorAtual = BemParaAtualizarImpostos.ValorContabil - BemParaAtualizarImpostos.ValorDepreciado;
                    BemParaAtualizarImpostos.ValorContabil = BemParaAtualizarImpostos.ValorContabil;

                    ContextoBem.SaveChanges();

                    LancamentoDepreciacao LancamentoContabil = new LancamentoDepreciacao();
                    LancamentoContabil.Credito = DepreciacaoBemCoeficienteUm.ValorDepreciado;
                    LancamentoContabil.Debito = DepreciacaoBemCoeficienteUm.ValorDepreciado * -1;
                    LancamentoContabil.DataLancamento = DateTime.Now;
                    ContextoBem.Add<LancamentoDepreciacao>(LancamentoContabil);
                    ContextoBem.SaveChanges();


                }



            }

        }
        //100% com lancamento contabil e tudo.
        private void CalcularDepreciacaoHorasTrabalhadas(int id, string NomeUsuario)
        {
            DepreciacaoBem UltimaDepreciacao = VerificarUltimaDepreciacaoBem(id);
            if (UltimaDepreciacao.DataDepreciacaoBem < DateTime.Now)
            {
                var VerificarValorDepreciado = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                                  .Where(x => x.IdBem.Id_Bem == id)
                                                select c.ValorDepreciado).Sum();
                var VerificarValorDepreciavel = (from c in ContextoBem.GetAll<Bem>()
                                                .Where(x => x.Id_Bem == id)
                                                 select c.ValorDepreciavel).First();

                if (VerificarValorDepreciado < VerificarValorDepreciavel)
                {
                    Bem BemParaAtualizarImpostos = ContextoBem.Get<Bem>(id);

                    AuditoriaInterna AuditoriaCoeficienteUm = new AuditoriaInterna();
                    AuditoriaCoeficienteUm.Computador = Environment.MachineName;
                    AuditoriaCoeficienteUm.DataInsercao = DateTime.Now;
                    AuditoriaCoeficienteUm.DetalhesOperacao = "Insercao Tabela Depreciacao Bem Registro: " + BemParaAtualizarImpostos.Descricao;
                    AuditoriaCoeficienteUm.Tabela = "TB_DepreciacaoBem";
                    AuditoriaCoeficienteUm.TipoOperacao = TipoOperacao.Insercao.ToString();
                    AuditoriaCoeficienteUm.Usuario = NomeUsuario;

                    ContextoBem.Add<AuditoriaInterna>(AuditoriaCoeficienteUm);
                    ContextoBem.SaveChanges();

                    DepreciacaoBem DepreciacaoBemCoeficienteUm = new DepreciacaoBem();

                    DepreciacaoBemCoeficienteUm.DataDepreciacaoBem = DateTime.Now;
                    DepreciacaoBemCoeficienteUm.DepreciacaoFeita = true;
                    DepreciacaoBemCoeficienteUm.TaxaDepreciacao = DepreciacaoBemCoeficienteUm.CalculaTaxaMensal(BemParaAtualizarImpostos.CoeficienteDepreciacao, BemParaAtualizarImpostos.TaxaDepreciacaoAnual);
                    DepreciacaoBemCoeficienteUm.ValorCofins = DepreciacaoBemCoeficienteUm.CalculaCofinsMensal(BemParaAtualizarImpostos.ValorContabil, BemParaAtualizarImpostos.Cofins);
                    DepreciacaoBemCoeficienteUm.ValorPis = DepreciacaoBemCoeficienteUm.CalculaPisMensal(BemParaAtualizarImpostos.ValorContabil, BemParaAtualizarImpostos.Pis);
                    DepreciacaoBemCoeficienteUm.ValorDepreciado = DepreciacaoBemCoeficienteUm.CalcularDepreciacaoHorasTrabalhadasMensal(BemParaAtualizarImpostos.HorasTrabalhdadasPeriodo, BemParaAtualizarImpostos.HorasEstimadaVidaUtil, BemParaAtualizarImpostos.ValorContabil);
                    DepreciacaoBemCoeficienteUm.IdAuditoriaInterna = ContextoBem.Get<AuditoriaInterna>(AuditoriaCoeficienteUm.Id_AuditoriaInterna);
                    DepreciacaoBemCoeficienteUm.IdBem = ContextoBem.Get<Bem>(id);
                    DepreciacaoBemCoeficienteUm.TipoParaDepreciacao = BemParaAtualizarImpostos.TipoParaDepreciacao;
                    ContextoBem.Add<DepreciacaoBem>(DepreciacaoBemCoeficienteUm);
                    ContextoBem.SaveChanges();


                    BemParaAtualizarImpostos.ValorDepreciado = DepreciacaoBemCoeficienteUm.ValorDepreciado;
                    BemParaAtualizarImpostos.ValorAtual = BemParaAtualizarImpostos.ValorContabil - BemParaAtualizarImpostos.ValorDepreciado;
                    BemParaAtualizarImpostos.ValorContabil = BemParaAtualizarImpostos.ValorContabil;

                    ContextoBem.SaveChanges();

                    LancamentoDepreciacao LancamentoContabil = new LancamentoDepreciacao();
                    LancamentoContabil.Credito = DepreciacaoBemCoeficienteUm.ValorDepreciado;
                    LancamentoContabil.Debito = DepreciacaoBemCoeficienteUm.ValorDepreciado * -1;
                    LancamentoContabil.DataLancamento = DateTime.Now;
                    ContextoBem.Add<LancamentoDepreciacao>(LancamentoContabil);
                    ContextoBem.SaveChanges();

                }




            }
        }
        //100% com lancamento contabil e tudo.
        private void CalcularDepreciacaoReducaoSaldos(int id, string NomeUsuario)
        {
            DepreciacaoBem UltimaDepreciacaoBem = VerificarUltimaDepreciacaoBem(id);
            if (UltimaDepreciacaoBem.DataDepreciacaoBem < DateTime.Now)
            {
                Bem BemParaAtualizarImpostos = ContextoBem.Get<Bem>(id);
                var VerificarValorDepreciado = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                                       .Where(x => x.IdBem.Id_Bem == id)
                                                select c.ValorDepreciado).Sum();
                var VerificarValorDepreciavel = (from c in ContextoBem.GetAll<Bem>()
                                              .Where(x => x.Id_Bem == id)
                                                 select c.ValorDepreciavel).First();

                if (VerificarValorDepreciado < VerificarValorDepreciavel && VerificarValorDepreciavel > BemParaAtualizarImpostos.ValorAtual)
                {


                    AuditoriaInterna AuditoriaCoeficienteUm = new AuditoriaInterna();
                    AuditoriaCoeficienteUm.Computador = Environment.MachineName;
                    AuditoriaCoeficienteUm.DataInsercao = DateTime.Now;
                    AuditoriaCoeficienteUm.DetalhesOperacao = "Insercao Tabela Depreciacao Bem Registro: " + BemParaAtualizarImpostos.Descricao;
                    AuditoriaCoeficienteUm.Tabela = "TB_DepreciacaoBem";
                    AuditoriaCoeficienteUm.TipoOperacao = TipoOperacao.Insercao.ToString();
                    AuditoriaCoeficienteUm.Usuario = NomeUsuario;

                    ContextoBem.Add<AuditoriaInterna>(AuditoriaCoeficienteUm);
                    ContextoBem.SaveChanges();

                    DepreciacaoBem DepreciacaoBemCoeficienteUm = new DepreciacaoBem();

                    DepreciacaoBemCoeficienteUm.DataDepreciacaoBem = DateTime.Now;
                    DepreciacaoBemCoeficienteUm.DepreciacaoFeita = true;
                    DepreciacaoBemCoeficienteUm.TaxaDepreciacao = DepreciacaoBemCoeficienteUm.CalculaTaxaMensal(BemParaAtualizarImpostos.CoeficienteDepreciacao, BemParaAtualizarImpostos.TaxaDepreciacaoAnual);
                    DepreciacaoBemCoeficienteUm.ValorCofins = DepreciacaoBemCoeficienteUm.CalculaCofinsMensal(BemParaAtualizarImpostos.ValorContabil, BemParaAtualizarImpostos.Cofins);
                    DepreciacaoBemCoeficienteUm.ValorPis = DepreciacaoBemCoeficienteUm.CalculaPisMensal(BemParaAtualizarImpostos.ValorContabil, BemParaAtualizarImpostos.Pis);
                    DepreciacaoBemCoeficienteUm.ValorDepreciado = DepreciacaoBemCoeficienteUm.CalcularDepreciacaoReducaoSaldosMensal(BemParaAtualizarImpostos.ValorContabil, BemParaAtualizarImpostos.ValorCompra, DepreciacaoBemCoeficienteUm.CalculaVidaUtilAnos(BemParaAtualizarImpostos.CoeficienteDepreciacao, BemParaAtualizarImpostos.TaxaDepreciacaoAnual), BemParaAtualizarImpostos.ValorResidual);
                    DepreciacaoBemCoeficienteUm.IdAuditoriaInterna = ContextoBem.Get<AuditoriaInterna>(AuditoriaCoeficienteUm.Id_AuditoriaInterna);
                    DepreciacaoBemCoeficienteUm.IdBem = ContextoBem.Get<Bem>(id);
                    DepreciacaoBemCoeficienteUm.TipoParaDepreciacao = BemParaAtualizarImpostos.TipoParaDepreciacao;
                    ContextoBem.Add<DepreciacaoBem>(DepreciacaoBemCoeficienteUm);
                    ContextoBem.SaveChanges();


                    BemParaAtualizarImpostos.ValorDepreciado = DepreciacaoBemCoeficienteUm.ValorDepreciado;
                    BemParaAtualizarImpostos.ValorAtual = BemParaAtualizarImpostos.ValorContabil - BemParaAtualizarImpostos.ValorDepreciado;
                    BemParaAtualizarImpostos.ValorContabil = BemParaAtualizarImpostos.ValorContabil - BemParaAtualizarImpostos.ValorDepreciado;
                    BemParaAtualizarImpostos.ValorDepreciavel = BemParaAtualizarImpostos.ValorDepreciavel - DepreciacaoBemCoeficienteUm.ValorDepreciado;

                    ContextoBem.SaveChanges();

                    LancamentoDepreciacao LancamentoContabil = new LancamentoDepreciacao();
                    LancamentoContabil.Credito = DepreciacaoBemCoeficienteUm.ValorDepreciado;
                    LancamentoContabil.Debito = DepreciacaoBemCoeficienteUm.ValorDepreciado * -1;
                    LancamentoContabil.DataLancamento = DateTime.Now;
                    ContextoBem.Add<LancamentoDepreciacao>(LancamentoContabil);
                    ContextoBem.SaveChanges();

                }



            }

        }
        //100% com lancamento contabil e tudo.
        private void CalcularDepreciacaoLinearComValorMaximoDepreciacao(int id, string NomeUsuario)
        {
            DepreciacaoBem UltimaDepreciacaoDesteBem = VerificarUltimaDepreciacaoBem(id);
            if (UltimaDepreciacaoDesteBem.DataDepreciacaoBem < DateTime.Now)
            {
                var VerificarValorDepreciado = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                                   .Where(x => x.IdBem.Id_Bem == id)
                                                select c.ValorDepreciado).Sum();
                var VerificarValorDepreciavel = (from c in ContextoBem.GetAll<Bem>()
                                            .Where(x => x.Id_Bem == id)
                                                 select c.ValorMaximoDepreciacao).First();

                if (VerificarValorDepreciado < VerificarValorDepreciavel)
                {
                    Bem BemParaAtualizarImpostos = ContextoBem.Get<Bem>(id);

                    AuditoriaInterna AuditoriaCoeficienteUm = new AuditoriaInterna();
                    AuditoriaCoeficienteUm.Computador = Environment.MachineName;
                    AuditoriaCoeficienteUm.DataInsercao = DateTime.Now;
                    AuditoriaCoeficienteUm.DetalhesOperacao = "Insercao Tabela Depreciacao Bem Registro: " + BemParaAtualizarImpostos.Descricao;
                    AuditoriaCoeficienteUm.Tabela = "TB_DepreciacaoBem";
                    AuditoriaCoeficienteUm.TipoOperacao = TipoOperacao.Insercao.ToString();
                    AuditoriaCoeficienteUm.Usuario = NomeUsuario;

                    ContextoBem.Add<AuditoriaInterna>(AuditoriaCoeficienteUm);
                    ContextoBem.SaveChanges();

                    DepreciacaoBem DepreciacaoBemCoeficienteUm = new DepreciacaoBem();

                    DepreciacaoBemCoeficienteUm.DataDepreciacaoBem = DateTime.Now;
                    DepreciacaoBemCoeficienteUm.DepreciacaoFeita = true;
                    DepreciacaoBemCoeficienteUm.TaxaDepreciacao = DepreciacaoBemCoeficienteUm.CalculaTaxaMensal(BemParaAtualizarImpostos.CoeficienteDepreciacao, BemParaAtualizarImpostos.TaxaDepreciacaoAnual);
                    DepreciacaoBemCoeficienteUm.ValorCofins = DepreciacaoBemCoeficienteUm.CalculaCofinsMensal(BemParaAtualizarImpostos.ValorContabil, BemParaAtualizarImpostos.Cofins);
                    DepreciacaoBemCoeficienteUm.ValorPis = DepreciacaoBemCoeficienteUm.CalculaPisMensal(BemParaAtualizarImpostos.ValorContabil, BemParaAtualizarImpostos.Pis);
                    DepreciacaoBemCoeficienteUm.ValorDepreciado = DepreciacaoBemCoeficienteUm.CalculaDepreciacaoLinearComValorMaximoDepreciacaoMensal(BemParaAtualizarImpostos.TaxaDepreciacaoAnual, BemParaAtualizarImpostos.CoeficienteDepreciacao, BemParaAtualizarImpostos.ValorMaximoDepreciacao);
                    DepreciacaoBemCoeficienteUm.IdAuditoriaInterna = ContextoBem.Get<AuditoriaInterna>(AuditoriaCoeficienteUm.Id_AuditoriaInterna);
                    DepreciacaoBemCoeficienteUm.IdBem = ContextoBem.Get<Bem>(id);
                    DepreciacaoBemCoeficienteUm.TipoParaDepreciacao = BemParaAtualizarImpostos.TipoParaDepreciacao;
                    ContextoBem.Add<DepreciacaoBem>(DepreciacaoBemCoeficienteUm);
                    ContextoBem.SaveChanges();


                    BemParaAtualizarImpostos.ValorDepreciado = DepreciacaoBemCoeficienteUm.ValorDepreciado;
                    BemParaAtualizarImpostos.ValorAtual = BemParaAtualizarImpostos.ValorContabil - BemParaAtualizarImpostos.ValorDepreciado;
                    BemParaAtualizarImpostos.ValorContabil = BemParaAtualizarImpostos.ValorContabil - BemParaAtualizarImpostos.ValorDepreciado;

                    ContextoBem.SaveChanges();

                    LancamentoDepreciacao LancamentoContabil = new LancamentoDepreciacao();
                    LancamentoContabil.Credito = DepreciacaoBemCoeficienteUm.ValorDepreciado;
                    LancamentoContabil.Debito = DepreciacaoBemCoeficienteUm.ValorDepreciado * -1;
                    LancamentoContabil.DataLancamento = DateTime.Now;
                    ContextoBem.Add<LancamentoDepreciacao>(LancamentoContabil);
                    ContextoBem.SaveChanges();

                }


            }

        }

        private bool ValidarDatas(DateTime DataInicial, DateTime DataFinal)
        {

            if (DataInicial.ToString() == "01/01/0001 00:00:00" || DataFinal.ToString() == "01/01/0001 00:00:00")
            {
                return false;
            }
            return true;
        }

        private double CalcularNumeroMesesVidaUtil(int id)
        {
            Bem BemParaCalcularSomaDigitos = ContextoBem.Get<Bem>(id);

            double NumeroDeMesesVidaUtil = ((BemParaCalcularSomaDigitos.ValorCompra / (BemParaCalcularSomaDigitos.TaxaDepreciacaoAnual * BemParaCalcularSomaDigitos.CoeficienteDepreciacao)));
            //100  / (10 * 1) = 10

            return NumeroDeMesesVidaUtil * 12;
        }

        private double CalcularNumeroMesesVidaUtil(double ValorCompra, double TaxaDepreciacaoAnual, double CoeficienteDepreciacao)
        {

            double NumeroDeMesesVidaUtil = ((ValorCompra / (TaxaDepreciacaoAnual * CoeficienteDepreciacao)));
            //100  / (10 * 1) = 10

            return NumeroDeMesesVidaUtil * 12;
        }

        private DepreciacaoBem VerificarUltimaDepreciacaoBem(int id)
        {
            long IdUltimaDepreciacao = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                        .Where(x => x.IdBem.Id_Bem == id)
                                        select c.Id_DepreciacaoBem).Max();
            DepreciacaoBem DepreciacaoBemUltima = ContextoBem.Get<DepreciacaoBem>(IdUltimaDepreciacao);
            return DepreciacaoBemUltima;
        }

        public double CalcularSomaDigitoMesesVidaUtil(double NumeroDeMesesVidaUtil)
        {
            double SomaDigitosMesesVidaUtil = 0;
            for (int i = 1; i <= NumeroDeMesesVidaUtil; i++)
            {
                SomaDigitosMesesVidaUtil += i;
                // 1 
            }
            return SomaDigitosMesesVidaUtil;
        }

        private double CalcularTaxaVidaUtilRestanteDepreciacaoVariacaoTaxas(int id)
        {
            double NumeroMesesVidaUtil = CalcularNumeroMesesVidaUtil(id);
            double NumeroDepreciacaoFeitas = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                              .Where(x => x.IdBem.Id_Bem == id && x.DepreciacaoFeita == true)
                                              select c).Count();
            double DepreciacaoFeitas = NumeroMesesVidaUtil - NumeroDepreciacaoFeitas;
            if (DepreciacaoFeitas <= 12)
            {
                DepreciacaoFeitas = ((100 / DepreciacaoFeitas) / 100) / 12;
            }

            DepreciacaoFeitas = ((100 / DepreciacaoFeitas) / 100);

            return DepreciacaoFeitas;

        }
        #endregion


    }
}
