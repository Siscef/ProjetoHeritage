using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models.Mapeamento
{
    public class ManutencaoBemMap : ClassMap<ManutencaoBem>
    {
        public ManutencaoBemMap()
        {
            Table("TB_ManutencaoBem");
            Id(x => x.Id_ManutencaoBem, "Id_ManutencaoBem");
            Map(x => x.DataSaidaParaConserto, "DataSaidaParaConserto").Not.Nullable();
            Map(x => x.Valor, "Valor").Not.Nullable();
            Map(x => x.DescricaoProblema, "DescricaoProblema").Not.Nullable().Length(255);
            Map(x => x.DescricaoConserto, "DescricaoConserto").Nullable().Length(255);
            Map(x => x.DataVoltaConserto, "DataVoltaConserto").Nullable();

            References(x => x.IdBem, "IdBem")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_ManutencaoBem_ToTB_Bem")
                .Not.Nullable();

            References(x => x.IdAssistenciaTecnica, "IdAssistenciaTecnica")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_ManutencaoBem_ToTB_AssistenciaTecnica")
                .Not.Nullable();

            References(x => x.IdAuditoriaInterna, "IdAuditoriaInterna")
                .Cascade.All()
                .ForeignKey("FK_TB_ManutencaoBem_ToTB_AuditoriaInterna")
                .Nullable();

            HasMany(x => x.IdPecas)
                .Cascade.SaveUpdate()
                .KeyColumn("IdPecas");




        }
    }
}