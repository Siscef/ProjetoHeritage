using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models.Mapeamento
{
    public class PapelMap : ClassMap<Papel>
    {
        public PapelMap()
        {
            Table("TB_Papel");
            Id(x => x.Id_Papel, "Id_Papel");
            Map(x => x.Nome, "Nome").Not.Nullable().Length(50).Unique();

            References(x => x.IdAuditoriaInterna, "IdAuditoriaInterna")
                .Cascade.All()
                .ForeignKey("FK_TB_Papel_ToTB_AuditoriaInterna")
                .Nullable();
        }
    }
}