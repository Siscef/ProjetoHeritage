using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models.Mapeamento
{
    public class EstabelecimentoMap : ClassMap<Estabelecimento>
    {
        public EstabelecimentoMap()
        {
            Table("TB_Estabelecimento");
            Id(x => x.Id_Pessoa, "IdEstabelecimento");
            Map(x => x.Nome, "Nome").Not.Nullable().Length(100).Unique();
            Map(x => x.Email, "Email").Not.Nullable().Length(100).Unique();
            Map(x => x.Telefone, "Telefone").Not.Nullable().Length(16);//(083) 8832-4192
            Map(x => x.CNPJ, "CNPJ").Not.Nullable().Length(20).Unique();//02.742.263/0001/00
            Map(x => x.RazaoSocial, "RazaoSocial").Not.Nullable().Length(100).Unique();
            Map(x => x.InscricaoEstadual, "InscricaoEstadual").Nullable().Length(16);

            References(x => x.IdAuditoriaInterna, "IdAuditoriaInterna")
               .Cascade.All()
               .ForeignKey("FK_TB_Estabelecimento_ToTB_AuditoriaInterna")
               .Nullable();

            References(x => x.IdEndereco, "IdEndereco")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_Estabelecimento_ToTB_Endereco")
                .Not.Nullable();

            References(x => x.IdBairro, "IdBairro")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_Estabelecimento_ToTB_Bairro")
                .Not.Nullable();

            References(x => x.IdCidade, "IdCidade")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_Estabelecimento_ToTB_Cidade")
                .Not.Nullable();

            References(x => x.IdEstado, "IdEstado")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_Estabelecimento_ToTB_Estado")
                .Not.Nullable();

            HasMany(x => x.IdsBem)
                .Cascade.SaveUpdate()
                .KeyColumn("IdsBem");

        }
    }
}