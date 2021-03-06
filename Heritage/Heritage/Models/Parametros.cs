﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    /// <summary>
    /// Author Ademi Vieira
    /// esta classe serve como base para exibir nome dos relatórios
    /// e configuração de qual modalidade a ser usada para depreciação
    /// 
    ///     /// </summary>
    public class Parametros
    {
        public virtual long IdParametro { get; set; }
        [Display(Name="Nome empresa:")]
        public virtual string NomeEmpresaParaExibir { get; set; }
        [Display(Name="CNPJ:")]
        public virtual string CnpjParaExibir { get; set; }
        [Display(Name="Inscrição estadual:")]
        public virtual string IEParaExibir { get; set; }
        [Display(Name="Endereço:")]
        public virtual string EnderecoParaExibir { get; set; }
        [Display(Name="Bairro:")]
        public virtual string BairroParaExibir { get; set; }
        [Display(Name="Cidade:")]
        public virtual string CidadeParaExibir { get; set; }
        [Display(Name="Estado:")]
        public virtual string EstadoParaExibir { get; set; }
        [Display(Name="Telefone:")]
        public virtual string TelefoneParaExibir { get; set; }
        [Display(Name="E-mail:")]
        public virtual string EmailParaExibir { get; set; }       
        [Display(Name="Matriz?")]
        public virtual bool EMatriz { get; set; }
        [Display(Name="Atividade econômica:")]
        public virtual string RamoEmpresarial { get; set; }
        [Display(Name="Valor mínimo depreciação:")]
        public virtual double ValorMinimoDepreciacao { get; set; }
        [Display(Name="Idade em anos para início depreciação:")]
        public virtual int VidaUtilMinima { get; set; }
    }
}