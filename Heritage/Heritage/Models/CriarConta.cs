using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class CriarConta
    {
        public virtual long Id_CriarConta { get; set; }
        [Required(ErrorMessage = "Nome não pode ser vazio.")]
        public virtual string Nome { get; set; }
        [Required(ErrorMessage = "Email não pode ser vazio.")]
        [DataType(DataType.EmailAddress)]
        public virtual string Email { get; set; }
        [Required(ErrorMessage = "Senha não pode ser vazia"), MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        [DataType(DataType.Password)]
        public virtual string Senha { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirme a Senha")]
        [Compare("Senha", ErrorMessage = "As Senhas não conferem.")]
        [Required(ErrorMessage = "O campo compare a senha é obrigatório"), StringLength(25)]
        public virtual string ConfirmeSenha { get; set; }

       
    }

    public class TrocarSenha
    {
        public virtual long Id_TrocarSenha { get; set; }
        [Required(ErrorMessage=" A senha atual é obrigatória.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha Atual:")]
        public virtual string SenhaAtual { get; set; }
        [Required(ErrorMessage="A nova senha é obrigatória,não pode existir senha vazia.")]
        [StringLength(100, ErrorMessage = "A senha tem que ter no mínimo 6 letras.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha:")]
        public virtual string NovaSenha { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirma Nova Senha:")]
        [Compare("NovaSenha", ErrorMessage = "A nova senha e a senha fornecida não conferem.")]
        public virtual string ConfirmaSenha { get; set; }
    }

    public class Login
    {
        public virtual long Id_login { get; set; }
        [Required(ErrorMessage = "O nome do usuário é obrigatório."), MaxLength(20, ErrorMessage = "O Campo nome não pode ter mais de 20 caracteres"), MinLength(3, ErrorMessage = "O Campo nome requer pelo menos três caracteres")]
        public virtual string Nome { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "A senha do usuário é obrigatória."), MinLength(6, ErrorMessage = "O Campo senha requer no mínimo 6 caracteres"), MaxLength(12, ErrorMessage = "O Campo senha requer no máximo doze caracteres")]
        public virtual string Senha { get; set; }
        public virtual bool Lembrar { get; set; }

    }
}