using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models.Mapeamento
{
    public class GrupoMap : ClassMap<Grupo>
    {
        public GrupoMap()
        {
            Table("TB_Grupo");
            Id(x => x.Id_Grupo, "Id_Grupo");
            Map(x => x.Nome, "Nome").Not.Nullable().Length(50).Unique();

            References(x => x.IdAuditoriaInterna, "IdAuditoriaInterna")
                .Cascade.All()
                .ForeignKey("FK_TB_Grupo_ToTB_AuditoriaInterna")
                .Nullable();

            HasMany(x => x.IdsBem)
                .Cascade.SaveUpdate()
                .KeyColumn("IdsBem");


        }
    }
}