using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Heritage.Models.Mapeamento
{
    public class ParametroMap : ClassMap<Parametros>
    {
        public ParametroMap()
        {
            Table("TB_Paramentros");
            Id(x => x.IdParametro, "IdParametro");
            Map(x => x.NomeEmpresaParaExibir, "NomeEmpresaParaExibir").Length(100);
            Map(x => x.CnpjParaExibir, "CnpjParaExibir").Length(23);
            Map(x => x.IEParaExibir, "IEParaExibir").Length(23);
            Map(x => x.EmailParaExibir, "EmailParaExibir").Length(100);
            Map(x => x.TelefoneParaExibir, "TelefoneParaExibir").Length(23);
            Map(x => x.EnderecoParaExibir, "EnderecoParaExibir").Length(100);
            Map(x => x.BairroParaExibir, "BairroParaExibir").Length(100);
            Map(x => x.CidadeParaExibir, "CidadeParaExibir").Length(100);
            Map(x => x.EstadoParaExibir, "EstadoParaExibir").Length(100);
            Map(x => x.EMatriz, "EMatriz");
            Map(x => x.RamoEmpresarial, "RamoEmpresarial").Length(100);
            Map(x => x.TipoParaDepreciacao, "TipoParaDepreciacao");

        }
    }
}