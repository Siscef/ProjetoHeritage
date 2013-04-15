using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models.Mapeamento
{
    public class BairroMap : ClassMap<Bairro>
    {
        public BairroMap()
        {
            Table("TB_Bairro");
            Id(x => x.Id_Bairro,"Id_Bairro");
            Map(x => x.Nome, "Nome").Not.Nullable().Length(50).Unique();
            
            References(x => x.IdCidade, "IdCidade")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_Bairro_ToTB_Cidade")
                .Not.Nullable();

            References(x => x.IdAuditoriaInterna, "IdAuditoriaInterna")
                .Cascade.All()
                .ForeignKey("FK_TB_Bairro_ToTB_AuditoriaInterna")
                .Nullable();

            HasMany(x => x.IdsEndereco)
                .KeyColumn("IdsEndereco")
                .Cascade.SaveUpdate();

        }
    }
}