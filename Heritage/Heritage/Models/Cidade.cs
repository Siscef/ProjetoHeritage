using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class Cidade
    {
        public virtual long Id_Cidade { get; set; }
        [Required(ErrorMessage = "O nome da cidade não pode ser vazia.")]
        [Display(Name="Nome:")]
        [StringLength(50, ErrorMessage = "O nome tem que ter no mínimo 3 letras e no máximo 50.", MinimumLength = 3)]
        public virtual string Nome { get; set; }
        [Required(ErrorMessage="Por favor, selecione um estado.")]
        public virtual Estado IdEstado { get; set; }
        public virtual AuditoriaInterna IdAuditoriaInterna { get; set; }
        public virtual IList<Bairro> IdsBairro { get; set; }

    }
}