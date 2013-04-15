using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models.Mapeamento
{
    public class EstadoConservacaoMap : ClassMap<EstadoConservacao>
    {
        public EstadoConservacaoMap()
        {
            Table("TB_EstadoConservacao");
            Id(x => x.Id_EstadoConservacao, "Id_EstadoConservacao");
            Map(x => x.Descricao, "Descricao").Not.Nullable().Length(100).Unique();

            References(x => x.IdAuditoriaInterna, "IdAuditoriaInterna")
                .Cascade.All()
                .ForeignKey("FK_TB_EstadoConservacao_ToTB_AuditoriaInterna")
                .Nullable();

            HasMany(x => x.IdsBem)
                .KeyColumn("IdsBem")
                .Cascade.SaveUpdate();



        }
    }
}