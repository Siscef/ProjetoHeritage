using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models.Mapeamento
{
    public class CidadeMap : ClassMap<Cidade>
    {
        public CidadeMap()
        {
            Table("TB_Cidade");
            Id(x => x.Id_Cidade, "Id_Cidade");
            Map(x => x.Nome, "Nome").Not.Nullable().Length(50).Unique();

            References(x => x.IdEstado, "IdEstado")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_Cidade_ToTB_Estado")
                .Not.Nullable();

            References(x => x.IdAuditoriaInterna, "IdAuditoriaInterna")
                .Cascade.All()
                .ForeignKey("FK_TB_Cidade_ToTB_AuditoriaInterna")
                .Nullable();

            HasMany(x => x.IdsBairro)
                .KeyColumn("IdsBairro")
                .Cascade.SaveUpdate();
        }
    }
}