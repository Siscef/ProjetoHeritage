using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models.Mapeamento
{
    public class FornecedorMap : ClassMap<Fornecedor>
    {
        public FornecedorMap()
        {
            Table("TB_Fornecedor");
            Id(x => x.Id_Pessoa, "IdFornecedor");
            Map(x => x.Nome, "Nome").Not.Nullable().Length(100).Unique();
            Map(x => x.Email, "Email").Not.Nullable().Length(100).Unique();
            Map(x => x.Telefone, "Telefone").Not.Nullable().Length(11);
            Map(x => x.CNPJ, "CNPJ").Not.Nullable().Length(14).Unique();
            Map(x => x.RazaoSocial, "RazaoSocial").Not.Nullable().Length(100).Unique();
            Map(x => x.InscricaoEstadual, "InscricaoEstadual").Not.Nullable().Length(9).Unique();


            References(x => x.IdAuditoriaInterna, "IdAuditoriaInterna")
               .Cascade.All()
               .ForeignKey("FK_TB_Fornecedor_ToTB_AuditoriaInterna")
               .Nullable();

            References(x => x.IdEndereco, "IdEndereco")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_Fornecedor_ToTB_Endereco")
                .Not.Nullable();

            References(x => x.IdBairro, "IdBairro")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_Fornecedor_ToTB_Bairro")
                .Not.Nullable();

            References(x => x.IdCidade, "IdCidade")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_Fornecedor_ToTB_Cidade")
                .Not.Nullable();

            References(x => x.IdEstado, "IdEstado")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_Fornecedor_ToTB_Estado")
                .Not.Nullable();

            HasMany(x => x.IdsBem)
                .Cascade.SaveUpdate()
                .KeyColumn("IdsBem");



        }
    }
}