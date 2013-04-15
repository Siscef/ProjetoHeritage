using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class PecaDaManutencao
    {
        public virtual long Id_PecaDaManutencao { get; set; }
        public virtual Peca IdPeca { get; set; }
        public virtual ManutencaoBem IdManutencaoBem { get; set; }
        public virtual Bem IdBem { get; set; }
    }
}