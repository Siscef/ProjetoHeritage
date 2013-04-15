using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models.Mapeamento
{
    public class EnderecoMap : ClassMap<Endereco>
    {
        public EnderecoMap()
        {
            Table("TB_Endereco");
            Id(x => x.Id_Endereco, "Id_Endereco");
            Map(x => x.Descricao, "Descricao").Not.Nullable().Length(255).Unique();
            Map(x => x.CEP, "CEP").Not.Nullable().Length(8);


            References(x => x.IdAuditoriaInterna, "IdAuditoriaInterna")
                .Cascade.All()
                .ForeignKey("FK_TB_Endereco_ToTB_AuditoriaInterna")
                .Nullable();

            References(x => x.IdBairro, "IdBairro")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_Endereco_ToTB_Bairro")
                .Not.Nullable();

        }
    }
}