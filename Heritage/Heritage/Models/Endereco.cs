using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class Endereco
    {
        public virtual long Id_Endereco { get; set; }
        [Required(ErrorMessage = "A descrição não pode ser vazia.")]
        [Display(Name = "Rua/Avenida:")]
        public virtual string Descricao { get; set; }
        [Required(ErrorMessage = "Por favor, escolha um bairro.")]
        public virtual Bairro IdBairro { get; set; }
        [Display(Name = "CEP:")]
        [Required(ErrorMessage = "O CEP não pode ser vazio.")]
        [StringLength(8, ErrorMessage = "O CEP tem que ter no mínimo 8 letras e no máximo 8.", MinimumLength = 6)]
        public virtual string CEP { get; set; }
        public virtual AuditoriaInterna IdAuditoriaInterna { get; set; }
    }
}