using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class LancamentoDepreciacao
    {
        public virtual long Id_LancamentoDepreciacao { get; set; }
        [Display(Name="Data Lançamento:")]
        public virtual DateTime DataLancamento { get; set; }
        [Display(Name = "Depreciação Acumulada:")]
        public virtual double Credito { get; set; }
        [Display(Name = "Despesa de Depreciação:")]
        public virtual double Debito { get; set; }
    }
}