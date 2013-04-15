using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class Localizacao
    {
        public virtual long Id_Localizacao { get; set; }
        [Required(ErrorMessage="A descrição da localização não pode ser vazia.")]
        [StringLength(255, ErrorMessage = "O nome tem que ter no mínimo 3 letras e no máximo 255.", MinimumLength = 3)]
        [Display(Name="Descrição localização:")]
        public virtual string Descricao { get; set; }
        public virtual AuditoriaInterna IdAuditoriaInterna { get; set; }
        public virtual IList<Bem> IdsBem { get; set; }
    }
}