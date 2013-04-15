using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models.Mapeamento
{
    public class AuditoriaInternaMap : ClassMap<AuditoriaInterna>
    {
        public AuditoriaInternaMap()
        {
            Table("TB_AuditoriaInterna");
            Id(x => x.Id_AuditoriaInterna, "Id_AuditoriaInterna");
            Map(x => x.Usuario, "Usuario").Not.Nullable().Length(100);
            Map(x => x.DataInsercao, "DataInsercao").Not.Nullable();
            Map(x => x.Computador, "Computador").Not.Nullable().Length(100);
            Map(x => x.TipoOperacao, "TipoOperacao").Not.Nullable().Length(50);
            Map(x => x.DetalhesOperacao, "DetalhesOperacao").Not.Nullable().Length(500);
            Map(x => x.Tabela, "Tabela").Not.Nullable().Length(50);
        }
    }
}