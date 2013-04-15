using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class Categoria
    {
        public virtual long Id_Categoria { get; set; }
        [Required(ErrorMessage = "O nome da categoria não pode ser vazia.")]
        [StringLength(50, ErrorMessage = "O nome tem que ter no mínimo 3 letras e no máximo 50.", MinimumLength = 3)]
        [Display(Name="Nome:")]
        public virtual string Nome { get; set; }
        public virtual AuditoriaInterna IdAuditoriaInterna { get; set; }
        public virtual IList<Bem> IdsBem { get; set; }
    }
}