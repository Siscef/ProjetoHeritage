using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class Bairro
    {
        public virtual long Id_Bairro { get; set; }
        [Required(ErrorMessage = "O nome do bairro não pode ser vazio.")]
        [StringLength(50, ErrorMessage = "O nome tem que ter no mínimo 3 letras e no máximo 50.", MinimumLength = 3)]
        [Display(Name = "Nome:")]
        public virtual string Nome { get; set; }
        [Required(ErrorMessage = "Por favor, escolha uma cidade.")]
        public virtual Cidade IdCidade { get; set; }
        public virtual AuditoriaInterna IdAuditoriaInterna { get; set; }
        public virtual IList<Endereco> IdsEndereco { get; set; }
    }
}