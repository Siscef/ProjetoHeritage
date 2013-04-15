using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models.Mapeamento
{
    public class CriarContaMap : ClassMap<CriarConta>
    {
        public CriarContaMap()
        {
            Table("TB_CriarConta");
            Id(x => x.Id_CriarConta, "Id_CriarConta");
            Map(x => x.Nome, "Nome").Not.Nullable().Length(20);
            Map(x => x.Senha, "Senha").Not.Nullable().Not.Nullable();
            Map(x => x.ConfirmeSenha, "ConfirmeSenha");
            Map(x => x.Email, "Email").Not.Nullable();

            References(x => x.IdAuditoriaInterna, "IdAuditoriaInterna")
               .Cascade.All()
               .ForeignKey("FK_TB_CriarConta_ToTB_AuditoriaInterna")
               .Nullable();

            References(x => x.IdPapel, "IdPapel")
              .Cascade.SaveUpdate()
              .ForeignKey("FK_TB_CriarConta_ToTB_Papel")
              .Not.Nullable();
        }
    }
}