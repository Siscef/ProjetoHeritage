using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models.Mapeamento
{
    public class DepreciacaoBemMap:ClassMap<DepreciacaoBem>
    {
        public DepreciacaoBemMap()
        {
            Table("TB_DepreciacaoBem");
            Id(x => x.Id_DepreciacaoBem,"Id_DepreciacaoBem");
            Map(x => x.ValorDepreciado, "ValorDepreciado");
            Map(x => x.ValorCofins, "ValorCofins");
            Map(x => x.ValorPis, "ValorPis");
            Map(x => x.DataDepreciacaoBem,"DataDepreciacaoBem").Not.Nullable();


            References(x => x.IdBem, "IdBem")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_DepreciacaoBem_ToTB_Bem")
                .Not.Nullable();

            References(x => x.IdAuditoriaInterna, "IdAuditoriaInterna")
                .Cascade.All()
                .ForeignKey("FK_TB_DepreciacaoBem_ToTB_AuditoriaInterna")
                .Nullable();

        }
    }
}