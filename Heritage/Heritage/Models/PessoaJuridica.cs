using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class PessoaJuridica:Pessoa
    {
        //00.000.000/0000-00
        //00000000000000
        //00.000.000-0
        [Required(ErrorMessage = "O CNPJ não pode ser vazio.")]
        [StringLength(14, ErrorMessage = "O CNPJ tem que ter no mínimo 14 letras e no máximo 18.", MinimumLength = 14)]
        [Display(Name = "CNPJ:")]
        public virtual string CNPJ { get; set; }
        [Display(Name = "Razão Social:")]
        [Required(ErrorMessage = "A razão social não pode ser vazia.")]
        [StringLength(100, ErrorMessage = "A razão social tem que ter no mínimo 3 letras e no máximo 100.", MinimumLength = 3)]
        public virtual string RazaoSocial { get; set; }
        [Display(Name = "Inscrição Estadual:")]
        [Required(ErrorMessage = "A inscrição estadual não pode ser vazia.")]
        [StringLength(16, ErrorMessage = "A inscrição estadual que ter no mínimo 9 letras e no máximo 16.", MinimumLength = 9)]
        public virtual string InscricaoEstadual { get; set; }

        public static bool IsCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }
    }
}