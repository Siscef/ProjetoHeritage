using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models.Mapeamento
{
    public class EstadoMap : ClassMap<Estado>
    {
        public EstadoMap()
        {
            Table("TB_Estado");
            Id(x => x.Id_Estado, "Id_Estado");
            Map(x => x.Nome, "Nome").Not.Nullable().Length(100).Unique();
            Map(x => x.Sigla, "Sigla").Not.Nullable().Length(2).Unique();

            References(x => x.IdAuditoriaInterna, "IdAuditoriaInterna")
                .Cascade.All()
                .ForeignKey("FK_TB_Estado_ToTB_AuditoriaInterna")
                .Nullable();

            HasMany(x => x.IdCidades)
                .KeyColumn("IdCidades")
                .Cascade.SaveUpdate();
        }
    }
}