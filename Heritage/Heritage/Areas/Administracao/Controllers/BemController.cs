﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Heritage.Models;
using Heritage.Models.ContextoBanco;
using System.Numerics;

namespace Heritage.Areas.Administracao.Controllers
{
    [Authorize(Roles = "Administrador,Contabil")]
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
            Bem BemDetails = ContextoBem.Get<Bem>(id);
            return View(BemDetails);
        }

        //
        // GET: /Administracao/Bem/Create

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

                    AuditoriaInterna AuditoriaBem = new AuditoriaInterna();

                    AuditoriaBem.Computador = Environment.MachineName;
                    AuditoriaBem.DataInsercao = DateTime.Now;
                    AuditoriaBem.DetalhesOperacao = "Insercao Tabela Bem, Registro: " + BemParaSalvar.Descricao;
                    AuditoriaBem.Tabela = "TB_Bem";
                    AuditoriaBem.Usuario = User.Identity.Name;
                    AuditoriaBem.TipoOperacao = TipoOperacao.Insercao.ToString();
                    ContextoBem.Add<AuditoriaInterna>(AuditoriaBem);
                    ContextoBem.SaveChanges();

                    Bem BemSalvo = new Bem();

                    BemSalvo.CoeficienteDepreciacao = BemParaSalvar.CoeficienteDepreciacao;
                    BemSalvo.DepreciacaoAtiva = true;
                    BemSalvo.Cofins = BemParaSalvar.Cofins;
                    BemSalvo.DataAquisicao = BemParaSalvar.DataAquisicao;
                    BemSalvo.DataInicioDepreciacao = BemParaSalvar.DataInicioDepreciacao;
                    BemSalvo.Descontinuado = BemParaSalvar.Descontinuado;
                    BemSalvo.TaxaDepreciacaoAnual = BemParaSalvar.TaxaDepreciacaoAnual;
                    BemSalvo.Descricao = TransformaParaMaiusculo.PrimeiraLetraMaiuscula(BemParaSalvar.Descricao);
                    BemSalvo.IdCategoria = ContextoBem.Get<Categoria>(BemParaSalvar.IdCategoria.Id_Categoria);
                    BemSalvo.IdEstabelecimento = ContextoBem.Get<Estabelecimento>(BemParaSalvar.IdEstabelecimento.Id_Pessoa);
                    BemSalvo.IdEstadoConservacao = ContextoBem.Get<EstadoConservacao>(BemParaSalvar.IdEstadoConservacao.Id_EstadoConservacao);
                    BemSalvo.IdFornecedor = ContextoBem.Get<Fornecedor>(BemParaSalvar.IdFornecedor.Id_Pessoa);
                    BemSalvo.IdGrupo = ContextoBem.Get<Grupo>(BemParaSalvar.IdGrupo.Id_Grupo);
                    BemSalvo.IdLocalizacao = ContextoBem.Get<Localizacao>(BemParaSalvar.IdLocalizacao.Id_Localizacao);
                    BemSalvo.IdResponsavel = ContextoBem.Get<Responsavel>(BemParaSalvar.IdResponsavel.Id_Pessoa);
                    BemSalvo.Inativo = BemParaSalvar.Inativo;
                    BemSalvo.NumeroNotaFiscal = BemParaSalvar.NumeroNotaFiscal;
                    BemSalvo.Pis = BemParaSalvar.Pis;
                    BemSalvo.ValorCompra = BemParaSalvar.ValorCompra;
                    BemSalvo.IdAuditoriaInterna = ContextoBem.Get<AuditoriaInterna>(AuditoriaBem.Id_AuditoriaInterna);

                    ContextoBem.Add<Bem>(BemSalvo);
                    ContextoBem.SaveChanges();

                    DepreciacaoBem DepreciacaoBemVazia = new DepreciacaoBem();

                    DepreciacaoBemVazia.DataDepreciacaoBem = BemSalvo.DataInicioDepreciacao;
                    DepreciacaoBemVazia.IdBem = ContextoBem.Get<Bem>(BemSalvo.Id_Bem);
                    DepreciacaoBemVazia.IdAuditoriaInterna = ContextoBem.Get<AuditoriaInterna>(AuditoriaBem.Id_AuditoriaInterna);
                    DepreciacaoBemVazia.ValorCofins = 0;
                    DepreciacaoBemVazia.ValorDepreciado = 0;
                    DepreciacaoBemVazia.ValorPis = 0;
                    ContextoBem.Add<DepreciacaoBem>(DepreciacaoBemVazia);
                    ContextoBem.SaveChanges();

                    return RedirectToAction("LastProperty", BemSalvo);
                }
                catch
                {
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

            return View(BemParaEdicao);
        }

        //
        // POST: /Administracao/Bem/Edit/5

        [HttpPost]
        public ActionResult Edit(Bem BemParaEdicao)
        {
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
                    BemEditado.ValorCompra = BemParaEdicao.ValorCompra;
                    BemEditado.IdAuditoriaInterna = ContextoBem.Get<AuditoriaInterna>(AuditoriaBem.Id_AuditoriaInterna);

                    TryUpdateModel<Bem>(BemEditado);
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

        //
        // GET: /Administracao/Bem/Delete/5

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

                AuditoriaBem.Computador = Environment.MachineName;
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
                                     .OrderBy(x => x.Descricao)
                                     .ToList();

            return View(AllProperty);
        }

        private bool ValidarDatas(DateTime DataInicial, DateTime DataFinal)
        {

            if (DataInicial.ToString() == "01/01/0001 00:00:00" || DataFinal.ToString() == "01/01/0001 00:00:00")
            {
                return false;
            }
            return true;
        }

        public ActionResult CalculatesDepreciationOfAssets()
        {
            IList<Bem> ListaBensParaCalculoImpostos = ContextoBem.GetAll<Bem>()
                                                      .Where(x => x.Descontinuado == false && x.Inativo == false && x.DataInicioDepreciacao.Day == DateTime.Now.Day)
                                                      .ToList();
            if (ListaBensParaCalculoImpostos.Count() > 0)
            {
                foreach (var itemBens in ListaBensParaCalculoImpostos)
                {
                    var VerificarValorDepreciado = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                                    .Where(x => x.IdBem.Id_Bem == itemBens.Id_Bem)
                                                    select c.ValorDepreciado).Sum();

                    if (VerificarValorDepreciado < itemBens.ValorCompra)
                    {
                        Bem BemParaAtualizarImpostos = ContextoBem.Get<Bem>(itemBens.Id_Bem);


                        IList<DepreciacaoBem> VerificarDeprecicaoDuplicada = ContextoBem.GetAll<DepreciacaoBem>()
                                                                             .Where(x => x.DataDepreciacaoBem.Day == itemBens.DataInicioDepreciacao.Day && x.DataDepreciacaoBem.Month == itemBens.DataInicioDepreciacao.Month && x.IdBem.Id_Bem == itemBens.Id_Bem
                                                                             && x.ValorCofins == ((itemBens.ValorCompra * itemBens.Cofins) / 100) && x.ValorPis == ((itemBens.Pis * itemBens.ValorCompra) / 100)
                                                                             && x.ValorDepreciado == itemBens.ValorDepreciado)
                                                                             .ToList();
                        if (VerificarDeprecicaoDuplicada.Count() == 0)
                        {
                            AuditoriaInterna AuditoriaCoeficienteUm = new AuditoriaInterna();
                            AuditoriaCoeficienteUm.Computador = Environment.MachineName;
                            AuditoriaCoeficienteUm.DataInsercao = DateTime.Now;
                            AuditoriaCoeficienteUm.DetalhesOperacao = "Insercao Tabela Depreciacao Bem Registro: " + itemBens.Descricao;
                            AuditoriaCoeficienteUm.Tabela = "TB_DepreciacaoBem";
                            AuditoriaCoeficienteUm.TipoOperacao = TipoOperacao.Insercao.ToString();
                            AuditoriaCoeficienteUm.Usuario = User.Identity.Name;
                            ContextoBem.Add<AuditoriaInterna>(AuditoriaCoeficienteUm);
                            ContextoBem.SaveChanges();

                            DepreciacaoBem DepreciacaoBemCoeficienteUm = new DepreciacaoBem();

                            DepreciacaoBemCoeficienteUm.DataDepreciacaoBem = DateTime.Now;
                            DepreciacaoBemCoeficienteUm.ValorCofins = (BemParaAtualizarImpostos.Cofins * BemParaAtualizarImpostos.ValorCompra) / 100;
                            DepreciacaoBemCoeficienteUm.ValorPis = (BemParaAtualizarImpostos.Pis * BemParaAtualizarImpostos.ValorCompra) / 100;
                            DepreciacaoBemCoeficienteUm.ValorDepreciado = (((BemParaAtualizarImpostos.TaxaDepreciacaoAnual * Convert.ToInt64(BemParaAtualizarImpostos.CoeficienteDepreciacao) * BemParaAtualizarImpostos.ValorCompra) / 100) / 12);
                            DepreciacaoBemCoeficienteUm.IdAuditoriaInterna = ContextoBem.Get<AuditoriaInterna>(AuditoriaCoeficienteUm.Id_AuditoriaInterna);
                            DepreciacaoBemCoeficienteUm.IdBem = ContextoBem.Get<Bem>(itemBens.Id_Bem);

                            ContextoBem.Add<DepreciacaoBem>(DepreciacaoBemCoeficienteUm);
                            ContextoBem.SaveChanges();

                            BemParaAtualizarImpostos.ValorDepreciado = DepreciacaoBemCoeficienteUm.ValorDepreciado;
                            BemParaAtualizarImpostos.ValorAtual = BemParaAtualizarImpostos.ValorCompra - BemParaAtualizarImpostos.ValorDepreciado;
                            TryUpdateModel<Bem>(BemParaAtualizarImpostos);
                            ContextoBem.SaveChanges();

                        }
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }


        public ActionResult LateCalculateDepreciation(int id)
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
            decimal NumeroMeses = Math.Round((Meses / 30), 0);

            for (int i = 1; i < NumeroMeses; i++)
            {

                var VerificarValorDepreciado = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                                    .Where(x => x.IdBem.Id_Bem == BemParaCalculoDepreciacaoAtrasada.Id_Bem)
                                                select c.ValorDepreciado).Sum();

                if (VerificarValorDepreciado < BemParaCalculoDepreciacaoAtrasada.ValorCompra)
                {
                    DepreciacaoBem Depreciacao = new DepreciacaoBem();

                    long IdUltimaDepreciacao = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                                      .Where(x => x.IdBem.Id_Bem == BemParaCalculoDepreciacaoAtrasada.Id_Bem)
                                                select c.Id_DepreciacaoBem).Max();
                    DateTime UltimoMes = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                          .Where(x => x.Id_DepreciacaoBem == IdUltimaDepreciacao)
                                          select c.DataDepreciacaoBem).First();

                    Depreciacao.ValorCofins = (BemParaCalculoDepreciacaoAtrasada.Cofins * BemParaCalculoDepreciacaoAtrasada.ValorCompra) / 100;
                    Depreciacao.ValorPis = (BemParaCalculoDepreciacaoAtrasada.Pis * BemParaCalculoDepreciacaoAtrasada.ValorCompra) / 100;
                    Depreciacao.ValorDepreciado = (((BemParaCalculoDepreciacaoAtrasada.TaxaDepreciacaoAnual * Convert.ToInt64(BemParaCalculoDepreciacaoAtrasada.CoeficienteDepreciacao) * BemParaCalculoDepreciacaoAtrasada.ValorCompra) / 100) / 12);
                    Depreciacao.IdBem = ContextoBem.Get<Bem>(BemParaCalculoDepreciacaoAtrasada.Id_Bem);
                    Depreciacao.IdAuditoriaInterna = ContextoBem.Get<AuditoriaInterna>(Auditoria.Id_AuditoriaInterna);
                    if (UltimoMes.Month == 12)
                    {
                        Depreciacao.DataDepreciacaoBem = UltimoMes.AddMonths(1).AddYears(1);
                    }
                    Depreciacao.DataDepreciacaoBem = UltimoMes.AddMonths(1);

                    ContextoBem.Add<DepreciacaoBem>(Depreciacao);
                    ContextoBem.SaveChanges();

                    BemParaCalculoDepreciacaoAtrasada.ValorDepreciado = (from c in ContextoBem.GetAll<DepreciacaoBem>()
                                                                         .Where(x => x.IdBem.Id_Bem == BemParaCalculoDepreciacaoAtrasada.Id_Bem)
                                                                         select c.ValorDepreciado).Sum();
                    BemParaCalculoDepreciacaoAtrasada.ValorAtual = BemParaCalculoDepreciacaoAtrasada.ValorCompra - BemParaCalculoDepreciacaoAtrasada.ValorDepreciado;
                    TryUpdateModel<Bem>(BemParaCalculoDepreciacaoAtrasada);
                    ContextoBem.SaveChanges();

                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }

            return RedirectToAction("Index", "Home");
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
    }
}
