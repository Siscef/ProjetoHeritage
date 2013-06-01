using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class ValidarData
    {
        [Required(ErrorMessage = "Por favor forneça a data inicial")]
        [DataType(DataType.Date)]
        [Display(Name = "Data Inicial:")]
        public virtual DateTime DataInicial { get; set; }
        [Required(ErrorMessage = "Por favor forneça a data final")]
        [DataType(DataType.Date)]
        [Display(Name = "Data Final:")]
        [DateRange(StartDate = "DataInicial", EndDate = "DataFinal", ErrorMessage = "Por favor verifique a data inicial e final")]
        public virtual DateTime DataFinal { get; set; }
    }
}