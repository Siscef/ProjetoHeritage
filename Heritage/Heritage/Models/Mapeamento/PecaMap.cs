using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models.Mapeamento
{
    public class PecaMap : ClassMap<Peca>
    {
        public PecaMap()
        {
            Table("TB_Peca");
            Id(x => x.Id_Peca, "Id_Peca");
            Map(x => x.Descricao, "Descricao").Length(100).Not.Nullable();
            Map(x => x.Valor, "Valor").Not.Nullable();
            Map(x => x.AcrescentaValorAoBem, "AcrescentaValorAoBem").Nullable();

            References(x => x.IdFornecedor, "IdFornecedor")
              .Cascade.SaveUpdate()
              .ForeignKey("FK_TB_Peca_ToTB_Fornecedor")
              .Not.Nullable();

            References(x => x.IdAuditoriaInterna, "IdAuditoriaInterna")
              .Cascade.SaveUpdate()
              .ForeignKey("FK_TB_Peca_ToTB_AuditoriaInterna")
              .Not.Nullable();




        }
    }
}