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
    public class ManutencaoBemController : Controller
    {
        private IContextoDados ContextoManutencao = new ContextoDadosNH();
        //
        // GET: /Administracao/ManutencaoBem/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Administracao/ManutencaoBem/Details/5

        public ActionResult Details(int id)
        {
            ManutencaoBem ManutencaoDetails = ContextoManutencao.Get<ManutencaoBem>(id);
            return View(ManutencaoDetails);
        }

        //
        // GET: /Administracao/ManutencaoBem/Create

        public ActionResult Create()
        {
            ViewBag.BemManutencao = new SelectList(ContextoManutencao.GetAll<Bem>(), "Id_Bem", "Descricao");
            ViewBag.IdPecas = new MultiSelectList(ContextoManutencao.GetAll<Peca>(), "Id_Peca", "Descricao");
            ViewBag.AssistenciaTecnica = new SelectList(ContextoManutencao.GetAll<AssistenciaTecnica>(), "Id_Pessoa", "RazaoSocial");
            return View();
        }

        //
        // POST: /Administracao/ManutencaoBem/Create

        [HttpPost]
        public ActionResult Create(ManutencaoBem ManutencaoBemParaSalvar, string[] IdPecas)
        {
            ModelState["IdBem.Descricao"].Errors.Clear();
            ModelState["IdBem.IdEstadoConservacao"].Errors.Clear();
            ModelState["IdBem.IdEstabelecimento"].Errors.Clear();
            ModelState["IdBem.CoeficienteDepreciacao"].Errors.Clear();
            ModelState["IdBem.IdFornecedor"].Errors.Clear();
            ModelState["IdBem.IdCategoria"].Errors.Clear();
            ModelState["IdBem.IdGrupo"].Errors.Clear();
            ModelState["IdBem.IdLocalizacao"].Errors.Clear();
            ModelState["IdBem.IdResponsavel"].Errors.Clear();
            ModelState["IdAssistenciaTecnica.CNPJ"].Errors.Clear();
            ModelState["IdAssistenciaTecnica.RazaoSocial"].Errors.Clear();
            ModelState["IdAssistenciaTecnica.InscricaoEstadual"].Errors.Clear();
            ModelState["IdAssistenciaTecnica.Nome"].Errors.Clear();
            ModelState["IdAssistenciaTecnica.Email"].Errors.Clear();
            ModelState["IdAssistenciaTecnica.Telefone"].Errors.Clear();
            ModelState["IdAssistenciaTecnica.IdEndereco"].Errors.Clear();
            ModelState["IdAssistenciaTecnica.IdBairro"].Errors.Clear();
            ModelState["IdAssistenciaTecnica.IdCidade"].Errors.Clear();
            ModelState["IdAssistenciaTecnica.IdEstado"].Errors.Clear();
            if (IdPecas != null)
            {
                ModelState["IdPecas"].Errors.Clear();
            }

            if (ManutencaoBemParaSalvar.DataSaidaParaConserto.ToString() == "01/01/0001 00:00:00" || ManutencaoBemParaSalvar.DataVoltaConserto.ToString() == "01/01/0001 00:00:00")
            {
                ViewBag.BemManutencao = new SelectList(ContextoManutencao.GetAll<Bem>(), "Id_Bem", "Descricao", ManutencaoBemParaSalvar.IdBem);
                ViewBag.AssistenciaTecnica = new SelectList(ContextoManutencao.GetAll<AssistenciaTecnica>(), "Id_Pessoa", "RazaoSocial", ManutencaoBemParaSalvar.IdAssistenciaTecnica);
                return View();

            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (IdPecas == null)
                    {
                        string DescricaoManutencao = ManutencaoBemParaSalvar.DescricaoProblema;
                        AuditoriaInterna AuditoraManutencaoBem = new AuditoriaInterna();

                        AuditoraManutencaoBem.Computador = Environment.MachineName;
                        AuditoraManutencaoBem.DataInsercao = DateTime.Now;
                        AuditoraManutencaoBem.Usuario = User.Identity.Name;
                        AuditoraManutencaoBem.DetalhesOperacao = "Insercao Tabela ManutencaoBem, Registro: " + DescricaoManutencao;
                        AuditoraManutencaoBem.TipoOperacao = TipoOperacao.Insercao.ToString();
                        AuditoraManutencaoBem.Tabela = "TB_ManutencaoBem";
                        ContextoManutencao.Add<AuditoriaInterna>(AuditoraManutencaoBem);
                        ContextoManutencao.SaveChanges();

                        ManutencaoBem ManutencaoBemSalvo = new ManutencaoBem();

                        ManutencaoBemSalvo.DataSaidaParaConserto = ManutencaoBemParaSalvar.DataSaidaParaConserto;
                        ManutencaoBemSalvo.DataVoltaConserto = ManutencaoBemParaSalvar.DataVoltaConserto;
                        ManutencaoBemSalvo.DescricaoConserto = TransformaParaMaiusculo.TransformarParaMaiusculo(ManutencaoBemParaSalvar.DescricaoConserto);
                        ManutencaoBemSalvo.DescricaoProblema = TransformaParaMaiusculo.TransformarParaMaiusculo(ManutencaoBemParaSalvar.DescricaoProblema);
                        ManutencaoBemSalvo.IdAssistenciaTecnica = ContextoManutencao.Get<AssistenciaTecnica>(ManutencaoBemParaSalvar.IdAssistenciaTecnica.Id_Pessoa);
                        ManutencaoBemSalvo.IdAuditoriaInterna = ContextoManutencao.Get<AuditoriaInterna>(AuditoraManutencaoBem.Id_AuditoriaInterna);
                        ManutencaoBemSalvo.IdBem = ContextoManutencao.Get<Bem>(ManutencaoBemParaSalvar.IdBem.Id_Bem);
                        ManutencaoBemSalvo.Valor = ManutencaoBemParaSalvar.Valor;
                        ContextoManutencao.Add<ManutencaoBem>(ManutencaoBemSalvo);
                        ContextoManutencao.SaveChanges();

                        return RedirectToAction("LastMaintenance", ManutencaoBemSalvo);

                    }
                    else
                    {
                        string DescricaoManutencao = ManutencaoBemParaSalvar.DescricaoProblema;
                        AuditoriaInterna AuditoraManutencaoBem = new AuditoriaInterna();

                        AuditoraManutencaoBem.Computador = Environment.MachineName;
                        AuditoraManutencaoBem.DataInsercao = DateTime.Now;
                        AuditoraManutencaoBem.Usuario = User.Identity.Name;
                        AuditoraManutencaoBem.DetalhesOperacao = "Insercao Tabela ManutencaoBem, Registro: " + DescricaoManutencao;
                        AuditoraManutencaoBem.TipoOperacao = TipoOperacao.Insercao.ToString();
                        AuditoraManutencaoBem.Tabela = "TB_ManutencaoBem";
                        ContextoManutencao.Add<AuditoriaInterna>(AuditoraManutencaoBem);
                        ContextoManutencao.SaveChanges();

                        ManutencaoBem ManutencaoBemSalvo = new ManutencaoBem();

                        ManutencaoBemSalvo.DataSaidaParaConserto = ManutencaoBemParaSalvar.DataSaidaParaConserto;
                        ManutencaoBemSalvo.DataVoltaConserto = ManutencaoBemParaSalvar.DataVoltaConserto;
                        ManutencaoBemSalvo.DescricaoConserto = TransformaParaMaiusculo.TransformarParaMaiusculo(ManutencaoBemParaSalvar.DescricaoConserto);
                        ManutencaoBemSalvo.DescricaoProblema = TransformaParaMaiusculo.TransformarParaMaiusculo(ManutencaoBemParaSalvar.DescricaoProblema);
                        ManutencaoBemSalvo.IdAssistenciaTecnica = ContextoManutencao.Get<AssistenciaTecnica>(ManutencaoBemParaSalvar.IdAssistenciaTecnica.Id_Pessoa);
                        ManutencaoBemSalvo.IdAuditoriaInterna = ContextoManutencao.Get<AuditoriaInterna>(AuditoraManutencaoBem.Id_AuditoriaInterna);
                        ManutencaoBemSalvo.IdBem = ContextoManutencao.Get<Bem>(ManutencaoBemParaSalvar.IdBem.Id_Bem);
                        ManutencaoBemSalvo.Valor = ManutencaoBemParaSalvar.Valor;
                        ContextoManutencao.Add<ManutencaoBem>(ManutencaoBemSalvo);
                        ContextoManutencao.SaveChanges();

                        if (IdPecas.Count() > 0)
                        {
                            for (int i = 1; i <= IdPecas.Length; i++)
                            {
                                IList<Peca> ListaPecas = ContextoManutencao.GetAll<Peca>()
                                                         .Where(x => x.Id_Peca == i && x.AcrescentaValorAoBem == true)
                                                         .ToList();
                                if (ListaPecas.Count() > 0)
                                {

                                    foreach (var item in ListaPecas)
                                    {
                                        Bem BemParaAumentarValor = ContextoManutencao.Get<Bem>(ManutencaoBemParaSalvar.IdBem.Id_Bem);
                                        BemParaAumentarValor.ValorCompra += item.Valor;
                                        TryUpdateModel<Bem>(BemParaAumentarValor);
                                        ContextoManutencao.SaveChanges();

                                        PecaDaManutencao PecaManutencao = new PecaDaManutencao();

                                        PecaManutencao.IdBem = ContextoManutencao.Get<Bem>(ManutencaoBemParaSalvar.IdBem.Id_Bem);
                                        PecaManutencao.IdManutencaoBem = ContextoManutencao.Get<ManutencaoBem>(ManutencaoBemSalvo.Id_ManutencaoBem);
                                        PecaManutencao.IdPeca = (from c in ContextoManutencao.GetAll<Peca>()
                                                                 .Where(x => x.Id_Peca == item.Id_Peca)
                                                                 select c).First();
                                        ContextoManutencao.Add<PecaDaManutencao>(PecaManutencao);
                                        ContextoManutencao.SaveChanges();

                                    }
                                }



                            }


                        }

                        return RedirectToAction("LastMaintenance", ManutencaoBemSalvo);
                    }

                }

                catch
                {
                    ViewBag.BemManutencao = new SelectList(ContextoManutencao.GetAll<Bem>(), "Id_Bem", "Descricao", ManutencaoBemParaSalvar.IdBem);
                    ViewBag.AssistenciaTecnica = new SelectList(ContextoManutencao.GetAll<AssistenciaTecnica>(), "Id_Pessoa", "RazaoSocial", ManutencaoBemParaSalvar.IdAssistenciaTecnica);
                    return View();
                }
            }
            ViewBag.BemManutencao = new SelectList(ContextoManutencao.GetAll<Bem>(), "Id_Bem", "Descricao", ManutencaoBemParaSalvar.IdBem);
            ViewBag.AssistenciaTecnica = new SelectList(ContextoManutencao.GetAll<AssistenciaTecnica>(), "Id_Pessoa", "RazaoSocial", ManutencaoBemParaSalvar.IdAssistenciaTecnica);
            return View();
        }

        //
        // GET: /Administracao/ManutencaoBem/Edit/5

        public ActionResult Edit(int id)
        {       

            ManutencaoBem ManutencaoBemParaEdicao = ContextoManutencao.Get<ManutencaoBem>(id);
            ViewBag.IdPecas = new MultiSelectList(ContextoManutencao.GetAll<Peca>(), "Id_Peca", "Descricao");
            ViewBag.Bem = new SelectList(ContextoManutencao.GetAll<Bem>(), "Id_Bem", "Descricao");
            ViewBag.AssistenciaTecnica = new SelectList(ContextoManutencao.GetAll<AssistenciaTecnica>(), "Id_Pessoa", "RazaoSocial");
            ViewBag.DescricaoProblema = (from c in ContextoManutencao.GetAll<ManutencaoBem>()
                                        .Where(x => x.Id_ManutencaoBem == ManutencaoBemParaEdicao.Id_ManutencaoBem)
                                         select c.DescricaoProblema).First();
            return View(ManutencaoBemParaEdicao);
        }

        //
        // POST: /Administracao/ManutencaoBem/Edit/5

        [HttpPost]
        public ActionResult Edit(ManutencaoBem ManutencaoBemParaEdicao, string [] IdPecas)
        {

            ModelState["IdBem.Descricao"].Errors.Clear();
            ModelState["IdBem.IdEstadoConservacao"].Errors.Clear();
            ModelState["IdBem.IdEstabelecimento"].Errors.Clear();
            ModelState["IdBem.CoeficienteDepreciacao"].Errors.Clear();
            ModelState["IdBem.IdFornecedor"].Errors.Clear();
            ModelState["IdBem.IdCategoria"].Errors.Clear();
            ModelState["IdBem.IdGrupo"].Errors.Clear();
            ModelState["IdBem.IdLocalizacao"].Errors.Clear();
            ModelState["IdBem.IdResponsavel"].Errors.Clear();
            ModelState["IdAssistenciaTecnica.CNPJ"].Errors.Clear();
            ModelState["IdAssistenciaTecnica.RazaoSocial"].Errors.Clear();
            ModelState["IdAssistenciaTecnica.InscricaoEstadual"].Errors.Clear();
            ModelState["IdAssistenciaTecnica.Nome"].Errors.Clear();
            ModelState["IdAssistenciaTecnica.Email"].Errors.Clear();
            ModelState["IdAssistenciaTecnica.Telefone"].Errors.Clear();
            ModelState["IdAssistenciaTecnica.IdEndereco"].Errors.Clear();
            ModelState["IdAssistenciaTecnica.IdBairro"].Errors.Clear();
            ModelState["IdAssistenciaTecnica.IdCidade"].Errors.Clear();
            ModelState["IdAssistenciaTecnica.IdEstado"].Errors.Clear();
            if (IdPecas != null)
            {
                ModelState["IdPecas"].Errors.Clear();
            }

            if (ManutencaoBemParaEdicao.DataSaidaParaConserto.ToString() == "01/01/0001 00:00:00" || ManutencaoBemParaEdicao.DataVoltaConserto.ToString() == "01/01/0001 00:00:00")
            {
                ViewBag.BemManutencao = new SelectList(ContextoManutencao.GetAll<Bem>(), "Id_Bem", "Descricao", ManutencaoBemParaEdicao.IdBem);
                ViewBag.AssistenciaTecnica = new SelectList(ContextoManutencao.GetAll<AssistenciaTecnica>(), "Id_Pessoa", "RazaoSocial", ManutencaoBemParaEdicao.IdAssistenciaTecnica);
                return View();

            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (IdPecas == null)
                    {
                        string DescricaoManutencao = ManutencaoBemParaEdicao.DescricaoProblema;
                        ManutencaoBem ManutencaoBemSalvo = ContextoManutencao.Get<ManutencaoBem>(ManutencaoBemParaEdicao.Id_ManutencaoBem);
                        AuditoriaInterna AuditoraManutencaoBem = new AuditoriaInterna();

                        AuditoraManutencaoBem.Computador = Environment.MachineName;
                        AuditoraManutencaoBem.DataInsercao = DateTime.Now;
                        AuditoraManutencaoBem.Usuario = User.Identity.Name;
                        AuditoraManutencaoBem.DetalhesOperacao = "Alteracao Tabela ManutencaoBem, Registro: " + ManutencaoBemSalvo.DescricaoProblema + " Para: " + DescricaoManutencao;
                        AuditoraManutencaoBem.TipoOperacao = TipoOperacao.Alteracao.ToString();
                        AuditoraManutencaoBem.Tabela = "TB_ManutencaoBem";
                        ContextoManutencao.Add<AuditoriaInterna>(AuditoraManutencaoBem);
                        ContextoManutencao.SaveChanges();
                       

                        ManutencaoBemSalvo.DataSaidaParaConserto = ManutencaoBemParaEdicao.DataSaidaParaConserto;
                        ManutencaoBemSalvo.DataVoltaConserto = ManutencaoBemParaEdicao.DataVoltaConserto;
                        ManutencaoBemSalvo.DescricaoConserto = TransformaParaMaiusculo.TransformarParaMaiusculo(ManutencaoBemParaEdicao.DescricaoConserto);
                        ManutencaoBemSalvo.DescricaoProblema = TransformaParaMaiusculo.TransformarParaMaiusculo(ManutencaoBemParaEdicao.DescricaoProblema);
                        ManutencaoBemSalvo.IdAssistenciaTecnica = ContextoManutencao.Get<AssistenciaTecnica>(ManutencaoBemParaEdicao.IdAssistenciaTecnica.Id_Pessoa);
                        ManutencaoBemSalvo.IdAuditoriaInterna = ContextoManutencao.Get<AuditoriaInterna>(AuditoraManutencaoBem.Id_AuditoriaInterna);
                        ManutencaoBemSalvo.IdBem = ContextoManutencao.Get<Bem>(ManutencaoBemParaEdicao.IdBem.Id_Bem);
                        ManutencaoBemSalvo.Valor = ManutencaoBemParaEdicao.Valor;
                        TryUpdateModel<ManutencaoBem>(ManutencaoBemSalvo);
                        ContextoManutencao.SaveChanges();

                        return RedirectToAction("LastMaintenance", ManutencaoBemSalvo);

                    }
                    else
                    {
                        string DescricaoManutencao = ManutencaoBemParaEdicao.DescricaoProblema;
                        ManutencaoBem ManutencaoBemSalvo = ContextoManutencao.Get<ManutencaoBem>(ManutencaoBemParaEdicao.Id_ManutencaoBem);

                        AuditoriaInterna AuditoraManutencaoBem = new AuditoriaInterna();

                        AuditoraManutencaoBem.Computador = Environment.MachineName;
                        AuditoraManutencaoBem.DataInsercao = DateTime.Now;
                        AuditoraManutencaoBem.Usuario = User.Identity.Name;
                        AuditoraManutencaoBem.DetalhesOperacao = "Alteracao Tabela ManutencaoBem, Registro: " + ManutencaoBemSalvo.DescricaoProblema + " Para: " + DescricaoManutencao;
                        AuditoraManutencaoBem.TipoOperacao = TipoOperacao.Alteracao.ToString();
                        AuditoraManutencaoBem.Tabela = "TB_ManutencaoBem";
                        ContextoManutencao.Add<AuditoriaInterna>(AuditoraManutencaoBem);
                        ContextoManutencao.SaveChanges();

                       

                        ManutencaoBemSalvo.DataSaidaParaConserto = ManutencaoBemParaEdicao.DataSaidaParaConserto;
                        ManutencaoBemSalvo.DataVoltaConserto = ManutencaoBemParaEdicao.DataVoltaConserto;
                        ManutencaoBemSalvo.DescricaoConserto = TransformaParaMaiusculo.TransformarParaMaiusculo(ManutencaoBemParaEdicao.DescricaoConserto);
                        ManutencaoBemSalvo.DescricaoProblema = TransformaParaMaiusculo.TransformarParaMaiusculo(ManutencaoBemParaEdicao.DescricaoProblema);
                        ManutencaoBemSalvo.IdAssistenciaTecnica = ContextoManutencao.Get<AssistenciaTecnica>(ManutencaoBemParaEdicao.IdAssistenciaTecnica.Id_Pessoa);
                        ManutencaoBemSalvo.IdAuditoriaInterna = ContextoManutencao.Get<AuditoriaInterna>(AuditoraManutencaoBem.Id_AuditoriaInterna);
                        ManutencaoBemSalvo.IdBem = ContextoManutencao.Get<Bem>(ManutencaoBemParaEdicao.IdBem.Id_Bem);
                        ManutencaoBemSalvo.Valor = ManutencaoBemParaEdicao.Valor;
                        TryUpdateModel<ManutencaoBem>(ManutencaoBemSalvo);
                        ContextoManutencao.SaveChanges();

                        if (IdPecas.Count() > 0)
                        {
                            for (int i = 1; i <= IdPecas.Length; i++)
                            {
                                IList<Peca> ListaPecas = ContextoManutencao.GetAll<Peca>()
                                                         .Where(x => x.Id_Peca == i && x.AcrescentaValorAoBem == true)
                                                         .ToList();
                                if (ListaPecas.Count() > 0)
                                {

                                    foreach (var item in ListaPecas)
                                    {
                                        Bem BemParaAumentarValor = ContextoManutencao.Get<Bem>(ManutencaoBemParaEdicao.IdBem.Id_Bem);
                                        BemParaAumentarValor.ValorCompra += item.Valor;
                                        TryUpdateModel<Bem>(BemParaAumentarValor);
                                        ContextoManutencao.SaveChanges();

                                        IList<PecaDaManutencao> ListaPecasManutencao = ContextoManutencao.GetAll<PecaDaManutencao>()
                                                                                        .Where(x => x.IdManutencaoBem.Id_ManutencaoBem == ManutencaoBemSalvo.Id_ManutencaoBem)
                                                                                        .ToList();
                                        foreach (var itemPecas in ListaPecasManutencao)
                                        {
                                            PecaDaManutencao PecaManutencao = ContextoManutencao.Get<PecaDaManutencao>(itemPecas.IdManutencaoBem.Id_ManutencaoBem);

                                            PecaManutencao.IdBem = ContextoManutencao.Get<Bem>(ManutencaoBemParaEdicao.IdBem.Id_Bem);
                                            PecaManutencao.IdManutencaoBem = ContextoManutencao.Get<ManutencaoBem>(ManutencaoBemSalvo.Id_ManutencaoBem);
                                            PecaManutencao.IdPeca = (from c in ContextoManutencao.GetAll<Peca>()
                                                                     .Where(x => x.Id_Peca == item.Id_Peca)
                                                                     select c).First();
                                            TryUpdateModel<PecaDaManutencao>(PecaManutencao);
                                            ContextoManutencao.SaveChanges();
                                        }
                                       

                                    }
                                }



                            }


                        }

                        return RedirectToAction("LastMaintenance", ManutencaoBemSalvo);
                    }

                }

                catch
                {
                    ViewBag.BemManutencao = new SelectList(ContextoManutencao.GetAll<Bem>(), "Id_Bem", "Descricao", ManutencaoBemParaEdicao.IdBem);
                    ViewBag.AssistenciaTecnica = new SelectList(ContextoManutencao.GetAll<AssistenciaTecnica>(), "Id_Pessoa", "RazaoSocial", ManutencaoBemParaEdicao.IdAssistenciaTecnica);
                    return View();
                }
            }
            ViewBag.BemManutencao = new SelectList(ContextoManutencao.GetAll<Bem>(), "Id_Bem", "Descricao", ManutencaoBemParaEdicao.IdBem);
            ViewBag.AssistenciaTecnica = new SelectList(ContextoManutencao.GetAll<AssistenciaTecnica>(), "Id_Pessoa", "RazaoSocial", ManutencaoBemParaEdicao.IdAssistenciaTecnica);
            return View();
        }

        //
        // GET: /Administracao/ManutencaoBem/Delete/5

        public ActionResult Delete(int id)
        {
            ManutencaoBem ManutencaoBemParaExclusao = ContextoManutencao.Get<ManutencaoBem>(id);
            ViewBag.DescricaoManutencao = (from c in ContextoManutencao.GetAll<ManutencaoBem>()
                                          .Where(x => x.Id_ManutencaoBem == ManutencaoBemParaExclusao.Id_ManutencaoBem)
                                           select c.DescricaoProblema).First();
            return View(ManutencaoBemParaExclusao);
        }

        //
        // POST: /Administracao/ManutencaoBem/Delete/5

        [HttpPost]
        public ActionResult Delete(ManutencaoBem ManutencaoBemParaExclusao)
        {
            try
            {
                AuditoriaInterna AuditoraManutencaoBem = new AuditoriaInterna();
                AuditoraManutencaoBem.Computador = Environment.MachineName;
                AuditoraManutencaoBem.DataInsercao = DateTime.Now;
                AuditoraManutencaoBem.Usuario = User.Identity.Name;
                AuditoraManutencaoBem.DetalhesOperacao = "Exclusao Tabela ManutencaoBem, Registro: " + ManutencaoBemParaExclusao.DescricaoConserto;
                AuditoraManutencaoBem.Tabela = "TB_ManutencaoBem";
                AuditoraManutencaoBem.TipoOperacao = TipoOperacao.Exclusao.ToString();
                ContextoManutencao.Add<AuditoriaInterna>(AuditoraManutencaoBem);
                ContextoManutencao.SaveChanges();

                ManutencaoBem ManutencaoExcluida = ContextoManutencao.Get<ManutencaoBem>(ManutencaoBemParaExclusao.Id_ManutencaoBem);
                ContextoManutencao.Delete<ManutencaoBem>(ManutencaoExcluida);
                ContextoManutencao.SaveChanges();


                return RedirectToAction("Index","Home");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult LastMaintenance(ManutencaoBem ManutencaoSalva)
        {
            IList<ManutencaoBem> LastMaintenance = ContextoManutencao.GetAll<ManutencaoBem>()
                                                   .Where(x => x.Id_ManutencaoBem == ManutencaoSalva.Id_ManutencaoBem)
                                                   .ToList();
            return View(LastMaintenance);
        }

        public ActionResult AllMaintenance()
        {
            IList<ManutencaoBem> AllMaintenance = ContextoManutencao.GetAll<ManutencaoBem>()
                                                  .OrderBy(x => x.Id_ManutencaoBem)
                                                  .ToList();
            return View(AllMaintenance);
        }
    }
}
