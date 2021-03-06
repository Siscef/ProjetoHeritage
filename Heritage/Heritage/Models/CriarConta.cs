﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Heritage.Models
{
    public class CriarConta
    {
        public virtual long Id_CriarConta { get; set; }
        [Display(Name = "Nome:")]
        [Required(ErrorMessage = "O nome do usuário é obrigatório."), StringLength(20, ErrorMessage = "O Campo nome não pode ter mais de 20 caracteres")]
        public virtual string Nome { get; set; }
        [Display(Name = "E-mail:")]
        [Required(ErrorMessage = "Email não pode ser vazio.")]
        [DataType(DataType.EmailAddress)]
        public virtual string Email { get; set; }
        [Required(ErrorMessage = "A senha do usuário é obrigatória."), StringLength(12)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha:")]
        public virtual string Senha { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirme a Senha")]
        [Compare("Senha", ErrorMessage = "As Senhas não conferem.")]
        [Required(ErrorMessage = "O campo compare a senha é obrigatório"), StringLength(25)]
        public virtual string ConfirmeSenha { get; set; }
        [Required(ErrorMessage = "O papel/função do novo usuário é obrigatório.")]
        [Display(Name = "Papel:")]
        public virtual Papel IdPapel { get; set; }
        public virtual AuditoriaInterna IdAuditoriaInterna { get; set; }


        public virtual string getMD5Hash(string input)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }

    public class TrocarSenha
    {
        public virtual long Id_TrocarSenha { get; set; }
        [Required(ErrorMessage = " A senha atual é obrigatória.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha Atual:")]
        public virtual string SenhaAtual { get; set; }
        [Required(ErrorMessage = "A nova senha é obrigatória,não pode existir senha vazia.")]
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
        [Required(ErrorMessage = "O nome do usuário é obrigatório."), StringLength(20, ErrorMessage = "O Campo nome não pode ter mais de 20 caracteres")]
        [Display(Name="Nome:")]
        public virtual string Nome { get; set; }
        [DataType(DataType.Password)]
        [Display(Name="Senha:")]
        [Required(ErrorMessage = "A senha do usuário é obrigatória."), StringLength(12, ErrorMessage = "O Campo senha requer no máximo doze caracteres")]
        public virtual string Senha { get; set; }
        public virtual bool Lembrar { get; set; }

    }
}