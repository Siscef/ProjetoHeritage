using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class Peca
    {
        public virtual long Id_Peca { get; set; }
        [Display(Name = "Descrição:")]
        [StringLength(100, ErrorMessage = "A descrição tem que ter no mínimo 6 letras e no máximo 100.", MinimumLength = 6)]
        [Required(ErrorMessage = "A descrição da peça ou acessório não pode ser vazia.")]
        public virtual string Descricao { get; set; }
        [Range(0, int.MaxValue)]
        [Required(ErrorMessage = "O valor da peça ou acessório não pode ser vazio.")]
        [Display(Name = "Valor:")]
        public virtual double Valor { get; set; }
        [Display(Name = "Acrescentar valor ao Bem?")]
        public virtual bool AcrescentaValorAoBem { get; set; }
        public virtual Fornecedor IdFornecedor { get; set; }
        public virtual AuditoriaInterna IdAuditoriaInterna { get; set; }
    }
}