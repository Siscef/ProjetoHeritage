using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class DepreciacaoBem
    {
        public virtual long Id_DepreciacaoBem { get; set; }
        [Display(Name = "Data Depreciação:")]
        [Required(ErrorMessage = "A data depreciação bem não pode ser vazia.")]
        public virtual DateTime DataDepreciacaoBem { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "O valor depreciado não pode ser negativo.")]
        [Display(Name = "Valor depreciado:")]
        public virtual double ValorDepreciado { get; set; }
        [Display(Name = "Valor Pis:")]
        public virtual double ValorPis { get; set; }
        [Display(Name = "Valor Cofins:")]
        public virtual double ValorCofins { get; set; }
        [Required(ErrorMessage = "Por favor,selecione um bem.")]
        public virtual Bem IdBem { get; set; }
        public virtual AuditoriaInterna IdAuditoriaInterna { get; set; }
    }
}