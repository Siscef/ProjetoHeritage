using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models.Mapeamento
{
    public class HistoricoBemMap : ClassMap<HistoricoBem>
    {
        public HistoricoBemMap()
        {
            Table("TB_HistoricoBem");
            Id(x => x.Id_HistoricoBem, "Id_HistoricoBem");
            Map(x => x.TipoOPeracao, "TipoOPeracao").Not.Nullable().Length(100);
            Map(x => x.Data, "Data").Not.Nullable();
            Map(x => x.DetalhesOperacao, "DetalhesOperacao").Not.Nullable().Length(255);
            Map(x => x.Observacao, "Observacao").Nullable().Length(255);


            References(x => x.IdAuditoriaInterna, "IdAuditoriaInterna")
               .Cascade.All()
               .ForeignKey("FK_TB_HistoricoBem_ToTB_AuditoriaInterna")
               .Nullable();

            References(x => x.IdBem, "IdBem")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_HistoricoBem_ToTB_Bem")
                .Not.Nullable();
        }
    }
}