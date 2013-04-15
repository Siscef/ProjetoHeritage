using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class AuditoriaInterna
    {
        public virtual long Id_AuditoriaInterna { get; set; }
        [Display(Name = "Nome usuário:")]
        public virtual string Usuario { get; set; }
        [Display(Name = "Data ocorrência:")]
        public virtual DateTime DataInsercao { get; set; }
        [Display(Name = "Computador:")]
        public virtual string Computador { get; set; }
        [Display(Name = "Tipo operação:")]
        public virtual string TipoOperacao { get; set; }
        [Display(Name = "Detalhes da operação:")]
        public virtual string DetalhesOperacao { get; set; }
        [Display(Name = "Tabela afetada:")]
        public virtual string Tabela { get; set; }
    }
}