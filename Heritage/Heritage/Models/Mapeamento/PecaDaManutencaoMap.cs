using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models.Mapeamento
{
    public class PecaDaManutencaoMap : ClassMap<PecaDaManutencao>
    {
        public PecaDaManutencaoMap()
        {
            Table("TB_PecaDaManutencao");
            Id(x => x.Id_PecaDaManutencao, "Id_PecaDaManutencao");


            References(x => x.IdBem, "IdBem")
             .Cascade.SaveUpdate()
             .ForeignKey("FK_TB_PecaDaManutencao_ToTB_Bem")
             .Not.Nullable();

            References(x => x.IdManutencaoBem, "IdManutencaoBem")
             .Cascade.SaveUpdate()
             .ForeignKey("FK_TB_PecaDaManutencao_ToTB_ManutencaoBem")
             .Not.Nullable();

            References(x => x.IdPeca, "IdPeca")
            .Cascade.SaveUpdate()
            .ForeignKey("FK_TB_PecaDaManutencao_ToTB_Peca")
            .Not.Nullable();

        }
    }
}