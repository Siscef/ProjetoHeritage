using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class Bem
    {
        public virtual long Id_Bem { get; set; }
        [Required(ErrorMessage = "A descrição do bem não pode ser vazia.")]
        [StringLength(255, ErrorMessage = "O nome tem que ter no mínimo 6 letras e no máximo 255.", MinimumLength = 6)]
        [Display(Name = "Descrição:")]
        public virtual string Descricao { get; set; }
        [Display(Name = "Inativo:")]
        public virtual bool Inativo { get; set; }
        [Display(Name = "Descontinuado:")]
        public virtual bool Descontinuado { get; set; }
        [Display(Name = "Número nota fiscal:")]
        public virtual string NumeroNotaFiscal { get; set; }
        [Required(ErrorMessage = "O valor da compra do bem não pode ser vazio.")]
        [Range(0, int.MaxValue, ErrorMessage = "O valor da compra não pode ser negativo.")]
        [Display(Name = "Valor compra:")]
        public virtual double ValorCompra { get; set; }
        [Required(ErrorMessage = "O coeficiente de depreciação do bem não pode ser vazio.")]
        [Display(Name = "Coeficiente depreciação:")]
        [Range(1.0, 2.0)]
        public virtual double CoeficienteDepreciacao { get; set; }
        [Display(Name = "Valor atual:")]
        public virtual double ValorAtual { get; set; }
        [Display(Name = "Valor depreciado:")]
        public virtual double ValorDepreciado { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "O Pis não pode ser negativo.")]
        [Display(Name = "Pis:")]
        public virtual double Pis { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "O Cofins não pode ser negativo.")]
        [Display(Name = "Cofins:")]
        public virtual double Cofins { get; set; }
        [Required(ErrorMessage = "A data de aquisição do bem não pode ser vazia.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data aquisição:")]
        public virtual DateTime DataAquisicao { get; set; }
        [Display(Name = "Taxa Depreciação Anual %:")]
        [Required(ErrorMessage = "A taxa depreciação anual não pode ser vazia")]
        [Range(0, 100, ErrorMessage = "A taxa depreciação não pode ser negativa")]
        public virtual int TaxaDepreciacaoAnual { get; set; }
        [Required(ErrorMessage = "Por favor, escolha o estado de conservação que se encontra o bem.")]
        public virtual EstadoConservacao IdEstadoConservacao { get; set; }
        [Required(ErrorMessage = "Por favor, escolha o estabelecimento que comprou o bem.")]
        public virtual Estabelecimento IdEstabelecimento { get; set; }
        [Required(ErrorMessage = "Por favor, escolha o fornecedor que o bem foi comprado.")]
        public virtual Fornecedor IdFornecedor { get; set; }
        [Required(ErrorMessage = "Por favor, escolha a categoria do bem.")]
        public virtual Categoria IdCategoria { get; set; }
        [Required(ErrorMessage = "Por favor, escolha o grupo do bem.")]
        public virtual Grupo IdGrupo { get; set; }
        [Required(ErrorMessage = "Por favor, escolha onde o bem está localizado.")]
        public virtual Localizacao IdLocalizacao { get; set; }
        [Required(ErrorMessage = "Por favor, escolha o responsável pelo bem.")]
        public virtual Responsavel IdResponsavel { get; set; }
        [Required(ErrorMessage = "A data para início da depreciação não pode ser vazia.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data Início Depreciação:")]
        public virtual DateTime DataInicioDepreciacao { get; set; }
        [Display(Name = "Depreciação Ativa?")]
        public virtual bool DepreciacaoAtiva { get; set; }
        [Display(Name = "O bem é depreciável?")]
        public virtual bool BemDepreciavel { get; set; }
        public virtual AuditoriaInterna IdAuditoriaInterna { get; set; }
        public virtual IList<HistoricoBem> IdsHistoricoBem { get; set; }
        public virtual IList<DepreciacaoBem> IdsDepreciacaoBem { get; set; }
        public virtual IList<ManutencaoBem> IdsManutencaoBem { get; set; }

    }
}