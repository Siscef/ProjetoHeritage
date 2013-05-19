using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class ManutencaoBem
    {
        public virtual long Id_ManutencaoBem { get; set; }
        [Required(ErrorMessage = "A data de saída para conserto não pode ser vazia.")]
        [Display(Name = "Data Saída Conserto:")]
        [DataType(DataType.Date)]
        public virtual DateTime DataSaidaParaConserto { get; set; }
        [Required(ErrorMessage = "Por favor, selecione um bem.")]
        public virtual Bem IdBem { get; set; }
        [Required(ErrorMessage = "O valor da manutenção não pode ser vazio.")]
        [Range(0, int.MaxValue, ErrorMessage = "O valor da manutenção não pode ser negativo")]
        [Display(Name = "Valor:")]
        public virtual double Valor { get; set; }
        [Required(ErrorMessage = "A descrição do problema não pode ser vazia.")]
        [StringLength(255, ErrorMessage = "A descrição do problema tem que ter no mínimo 3 letras e no máximo 255.", MinimumLength = 3)]
        [Display(Name = "Descrição problema:")]
        public virtual string DescricaoProblema { get; set; }
        [StringLength(255, ErrorMessage = "A descrição do conserto tem que ter no mínimo 3 letras e no máximo 255.", MinimumLength = 3)]
        [Display(Name = "Descrição conserto:")]
        public virtual string DescricaoConserto { get; set; }
        [Display(Name = "Data volta conserto:")]
        [DataType(DataType.Date)]
        public virtual DateTime DataVoltaConserto { get; set; }
        [Required(ErrorMessage = "Por favor, selecione uma assistência técnica ou oficina.")]
        public virtual AssistenciaTecnica IdAssistenciaTecnica { get; set; }
        public virtual TipoManutencao TipoDaManutencao { get; set; }
        public virtual AuditoriaInterna IdAuditoriaInterna { get; set; }
        public virtual IList<Peca> IdPecas { get; set; }
    }
}