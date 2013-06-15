using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models.Mapeamento
{
    public class LancamentoDepreciacaoMap : ClassMap<LancamentoDepreciacao>
    {
        public LancamentoDepreciacaoMap()
        {
            Table("TB_LancamentoDepreciacao");
            Id(x => x.Id_LancamentoDepreciacao, "Id_LancamentoDepreciacao");
            Map(x => x.DataLancamento, "DataLancamento").Not.Nullable();
            Map(x => x.Credito, "Credito").Nullable();
            Map(x => x.Debito, "Debito").Nullable();
        }
    }
}