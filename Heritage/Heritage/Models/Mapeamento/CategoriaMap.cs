using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models.Mapeamento
{
    public class CategoriaMap : ClassMap<Categoria>
    {
        public CategoriaMap()
        {
            Table("TB_Categoria");
            Id(x => x.Id_Categoria, "Id_Categoria");
            Map(x => x.Nome, "Nome").Not.Nullable().Length(50).Unique().Unique();

            References(x => x.IdAuditoriaInterna, "IdAuditoriaInterna")
                .Cascade.All()
                .ForeignKey("FK_TB_Categoria_ToTB_AuditoriaInterna")
                .Nullable();

            HasMany(x => x.IdsBem)
                .Cascade.SaveUpdate()
                .KeyColumn("IdsBem");



        }
    }
}