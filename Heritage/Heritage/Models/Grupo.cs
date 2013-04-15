using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class Grupo
    {
        public virtual long Id_Grupo { get; set; }
        [Display(Name="Nome:")]
        [StringLength(50, ErrorMessage = "O nome tem que ter no mínimo 3 letras e no máximo 100.", MinimumLength = 3)]
        [Required(ErrorMessage="O nome não pode ser vazio.")]
        public virtual string Nome { get; set; }
        public virtual AuditoriaInterna IdAuditoriaInterna { get; set; }
        public virtual IList<Bem> IdsBem { get; set; }
    }
}