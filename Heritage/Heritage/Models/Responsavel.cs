﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class Responsavel:PessoaFisica
    {
        public virtual AuditoriaInterna IdAuditoriaInterna { get; set; }
        public virtual IList<Bem> IdsBem { get; set; }
    }
}