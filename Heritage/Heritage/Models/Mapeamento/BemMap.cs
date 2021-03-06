﻿using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Heritage.Models;

namespace Heritage.Models.Mapeamento
{
    public class BemMap : ClassMap<Bem>
    {
        public BemMap()
        {
            Table("TB_Bem");
            Id(x => x.Id_Bem, "Id_Bem");
            Map(x => x.Descricao, "Descricao").Not.Nullable().Length(255);
            Map(x => x.Inativo, "Inativo ").Nullable();
            Map(x => x.Descontinuado, "Descontinuado").Nullable();
            Map(x => x.NumeroNotaFiscal, "NumeroNotaFiscal").Nullable();
            Map(x => x.ValorCompra, "ValorCompra").Not.Nullable();
            Map(x => x.CoeficienteDepreciacao, "CoeficienteDepreciacao").Not.Nullable();
            Map(x => x.ValorAtual, "ValorAtual").Nullable();
            Map(x => x.ValorDepreciado, "ValorDepreciado").Nullable();
            Map(x => x.Pis, "Pis").Nullable().Precision(3).Check("Pis >= 0"); ;
            Map(x => x.Cofins, "Cofins").Nullable().Precision(3).Check("Cofins >= 0");
            Map(x => x.DataAquisicao, "DataAquisicao").Not.Nullable();
            Map(x => x.DataInicioDepreciacao, "DataInicioDepreciacao").Nullable();
            Map(x => x.TaxaDepreciacaoAnual, "TaxaDepreciacaoAnual").Not.Nullable();
            Map(x => x.DepreciacaoAtiva, "DepreciacaoAtiva").Nullable();
            Map(x => x.BemDepreciavel, "BemDepreciavel").Nullable();
            Map(x => x.ValorContabil, "ValorContabil").Nullable().Check("ValorContabil > 0");
            Map(x => x.ValorResidual, "ValorResidual").Nullable().Check("ValorResidual >= 0");
            Map(x => x.ValorDepreciavel, "ValorDepreciavel").Nullable().Check("ValorDepreciavel > 0");
            Map(x => x.TipoParaDepreciacao, "TipoParaDepreciacao");
            Map(x => x.HorasEstimadaVidaUtil, "HorasEstimadaVidaUtil").Nullable().Check("HorasEstimadaVidaUtil >= 0");
            Map(x => x.HorasTrabalhdadasPeriodo, "HorasTrabalhdadasPeriodo").Nullable().Check("HorasTrabalhdadasPeriodo >=0");
            Map(x => x.UnidadesEstimadasVidaUtil, "UnidadesEstimadasVidaUtil").Nullable().Check("UnidadesEstimadasVidaUtil >= 0");
            Map(x => x.UnidadesProduzidasPeriodo, "UnidadesProduzidasPeriodo").Nullable().Check("UnidadesProduzidasPeriodo >=0");
            Map(x => x.ValorSalvamento, "ValorSalvamento").Nullable().Check("ValorSalvamento >= 0");
            Map(x => x.ValorMaximoDepreciacao, "ValorMaximoDepreciacao").Nullable().Check("ValorMaximoDepreciacao >= 0");
            Map(x => x.VidaUtil, "VidaUtil").Nullable().Check("VidaUtil > 0");


            References(x => x.IdEstadoConservacao, "IdEstadoConservacao")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_Bens_ToTB_EstadoConservacao")
                .Not.Nullable();

            References(x => x.IdEstabelecimento, "IdEstabelecimento")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_Bens_ToTB_Estabelecimento")
                .Not.Nullable();

            References(x => x.IdFornecedor, "IdFornecedor")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_Bens_ToTB_Fornecedor")
                .Not.Nullable();

            References(x => x.IdCategoria, "IdCategoria")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_Bens_ToTB_Categoria")
                .Not.Nullable();

            References(x => x.IdGrupo, "IdGrupo")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_Bens_ToTB_Grupo")
                .Not.Nullable();

            References(x => x.IdLocalizacao, "IdLocalizacao")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_Bens_ToTB_Localizacao")
                .Not.Nullable();

            References(x => x.IdResponsavel, "IdResponsavel")
                .Cascade.SaveUpdate()
                .ForeignKey("FK_TB_Bens_ToTB_Responsavel")
                .Not.Nullable();

            References(x => x.IdAuditoriaInterna, "IdAuditoriaInterna")
                .Cascade.All()
                .ForeignKey("FK_TB_Bens_ToTB_AuditoriaInterna")
                .Nullable();

            HasMany(x => x.IdsHistoricoBem)
                .Cascade.SaveUpdate()
                .KeyColumn("IdsHistoricoBem");

            HasMany(x => x.IdsDepreciacaoBem)
                .Cascade.SaveUpdate()
                .KeyColumn("IdsDepreciacaoBem");

            HasMany(x => x.IdsManutencaoBem)
                .Cascade.SaveUpdate()
                .KeyColumn("IdsManutencaoBem");



        }
    }
}