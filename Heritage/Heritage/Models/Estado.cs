using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class Estado
    {
        public virtual long Id_Estado { get; set; }
        [Required(ErrorMessage = "O nome não pode ser vazio.")]
        [Display(Name = "Nome:")]
        [StringLength(100, ErrorMessage = "O nome tem que ter no mínimo 3 letras e no máximo 100.", MinimumLength = 3)]
        public virtual string Nome { get; set; }
        [Required(ErrorMessage = "A sigla não pode ser vazia.")]
        [StringLength(2, ErrorMessage = "A sigla tem que ter no mínimo 2 letras e no máximo 2.", MinimumLength = 2)]
        [Display(Name = "Sigla:")]
        public virtual string Sigla { get; set; }
        public virtual AuditoriaInterna IdAuditoriaInterna { get; set; }
        public virtual IList<Cidade> IdCidades { get; set; }
    }
}