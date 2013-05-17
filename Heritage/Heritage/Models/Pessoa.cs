using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class Pessoa
    {
        [Display(Name = "Id:")]
        public virtual long Id_Pessoa { get; set; }
        [Display(Name = "Nome/Fantasia:")]
        [Required(ErrorMessage = "O nome não pode ser vazio.")]
        [StringLength(100, ErrorMessage = "O nome tem que ter no mínimo 3 letras e no máximo 100.", MinimumLength = 3)]
        public virtual string Nome { get; set; }
        [Required(ErrorMessage = "O email não pode ser vazio.")]
        [StringLength(100, ErrorMessage = "O email tem que ter no mínimo 3 letras e no máximo 100.", MinimumLength = 3)]
        [DataType(DataType.EmailAddress, ErrorMessage = "O email informado não é válido.")]
        [Display(Name = "Email:")]
        public virtual string Email { get; set; }
        [Required(ErrorMessage = "O número do telefone não pode ser vazio.")]
        [StringLength(11, ErrorMessage = "O número do telefone tem que ter no mínimo 11 letras e no máximo 11.", MinimumLength = 11)]
        [Display(Name = "Telefone:")]
        [DataType(DataType.PhoneNumber)]
        public virtual string Telefone { get; set; }
        [Required(ErrorMessage = "Por favor, selecione um endereço.")]
        public virtual Endereco IdEndereco { get; set; }
        [Required(ErrorMessage = "Por favor, selecione um bairro.")]
        public virtual Bairro IdBairro { get; set; }
        [Required(ErrorMessage = "Por favor,selecione uma cidade.")]
        public virtual Cidade IdCidade { get; set; }
        [Required(ErrorMessage = "Por favor, selecione um estado.")]
        public virtual Estado IdEstado { get; set; }
    }
}