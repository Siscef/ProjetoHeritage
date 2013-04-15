using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class EstadoConservacao
    {
        public virtual long Id_EstadoConservacao { get; set; }
        [Required(ErrorMessage="A descrição não pode ser vazia.")]
        [StringLength(100, ErrorMessage = "A descrição tem que ter no mínimo 3 letras e no máximo 100.", MinimumLength = 3)]
        [Display(Name="Descrição:")]
        public virtual string Descricao { get; set; }
        public virtual AuditoriaInterna IdAuditoriaInterna { get; set; }
        public virtual IList<Bem> IdsBem { get; set; }
    }
}