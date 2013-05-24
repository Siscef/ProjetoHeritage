using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public abstract class TransformaParaMaiusculo
    {
        public static string TransformarParaMaiusculo(string Campo)
        {
            if (Campo != null)
            {
                Campo = Campo.ToUpper();
            }
            return Campo;
        }
        public static string TransformarParaMinusculo(string Campo)
        {
            if (Campo != null)
            {
                Campo = Campo.ToLower();
            }
            return Campo;
        }

        public static string PrimeiraLetraMaiuscula(string campo)
        {
            if (campo != null)
            {
                campo = TransformarParaMinusculo(campo);
                string[] palavras = campo.Split(' ');

                string primeiraLetra = "";

                string restante = "";
                for (int i = 0; i < palavras.Length; i++)
                {
                    primeiraLetra = palavras[i].Substring(0, 1).ToString().ToUpper();
                    restante = palavras[i].Substring(1, palavras[i].Length - 1).ToString().ToLower();
                    palavras[i] = primeiraLetra + restante;
                }
                campo = String.Join(" ", palavras);
                return campo;
                
            }
            else
            {
                return campo;
            }
            
        }
    }
}