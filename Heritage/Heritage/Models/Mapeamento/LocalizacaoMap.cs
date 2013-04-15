using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models.Mapeamento
{
    public class LocalizacaoMap : ClassMap<Localizacao>
    {
        public LocalizacaoMap()
        {
            Table("TB_Localizacao");
            Id(x => x.Id_Localizacao, "Id_Localizacao");
            Map(x => x.Descricao, "Descricao").Not.Nullable().Length(255).Unique();

            References(x => x.IdAuditoriaInterna, "IdAuditoriaInterna")
                .Cascade.All()
                .ForeignKey("FK_TB_Localizacao_ToTB_AuditoriaInterna")
                .Nullable();

            HasMany(x => x.IdsBem)
                .Cascade.SaveUpdate()
                .KeyColumn("IdsBem");

        }
    }
}