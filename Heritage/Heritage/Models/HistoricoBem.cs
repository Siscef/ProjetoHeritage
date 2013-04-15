using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class HistoricoBem
    {
        public virtual long Id_HistoricoBem { get; set; }
        [Required(ErrorMessage = "O tipo de operação não pode ser vazio.")]
        [StringLength(100, ErrorMessage = "O tipo da operação tem que ter no mínimo 3 letras e no máximo 100.", MinimumLength = 3)]
        [Display(Name = "Tipo operação:")]
        public virtual string TipoOPeracao { get; set; }
        [Required(ErrorMessage = "A data da operação não pode ser vazia.")]
        [Display(Name = "Data histórico:")]
        public virtual DateTime Data { get; set; }
        [Required(ErrorMessage = "Os detalhes da operação não pode ser vazio.")]
        [StringLength(255, ErrorMessage = "Os detalhes da operação tem que ter no mínimo 3 letras e no máximo 255.", MinimumLength = 3)]
        [Display(Name = "Detalhes da operação:")]
        public virtual string DetalhesOperacao { get; set; }
        [StringLength(255, ErrorMessage = "A observação tem que ter no mínimo 3 letras e no máximo 255.", MinimumLength = 3)]
        [Display(Name = "Observação:")]
        public virtual string Observacao { get; set; }
        [Required(ErrorMessage = "Por favor, selecione um bem.")]
        public virtual Bem IdBem { get; set; }
        public virtual AuditoriaInterna IdAuditoriaInterna { get; set; }
    }
}