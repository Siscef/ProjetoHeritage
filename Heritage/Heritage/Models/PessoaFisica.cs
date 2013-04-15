using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class PessoaFisica:Pessoa
    {
        [Required(ErrorMessage="A data nascimento não pode ser vazia.")]
        [DataType(DataType.Date,ErrorMessage="A data nascimento não é válida.")]
        [Display(Name="Data Nascimento:")]
        public virtual DateTime DataNascimento { get; set; }
    }
}