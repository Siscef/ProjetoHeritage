using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class DepreciacaoBem
    {
        public virtual long Id_DepreciacaoBem { get; set; }
        [Display(Name = "Data Depreciação:")]
        [Required(ErrorMessage = "A data depreciação bem não pode ser vazia.")]
        public virtual DateTime DataDepreciacaoBem { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "O valor depreciado não pode ser negativo.")]
        [Display(Name = "Valor depreciado:")]
        public virtual double ValorDepreciado { get; set; }
        [Display(Name = "Valor Pis:")]
        public virtual double ValorPis { get; set; }
        [Display(Name = "Valor Cofins:")]
        public virtual double ValorCofins { get; set; }
        [Display(Name = "Depreciação Feita:")]
        public virtual bool DepreciacaoFeita { get; set; }
        [Display(Name="Taxa Depreciação:")]
        public virtual double TaxaDepreciacao { get; set; }
        [Required(ErrorMessage = "Por favor,selecione um bem.")]
        public virtual Bem IdBem { get; set; }
        public virtual AuditoriaInterna IdAuditoriaInterna { get; set; }
        public virtual TipoDepreciacao TipoParaDepreciacao { get; set; }





        protected internal virtual double CalcularPisAnual(double ValorContabil, double TaxaPis)
        {
            return (ValorContabil * TaxaPis) / 100;
        
        }

       protected internal virtual double CalculaPisMensal(double ValorContabil, double TaxaPis)
        {
            return ((ValorContabil * TaxaPis) / 100) / 12;
        }

        protected internal virtual double CalculaCofinsAnual(double ValorContabil, double TaxaCofins)
        {
            return (ValorContabil * TaxaCofins) / 100;

        }

      protected internal virtual double CalculaCofinsMensal(double ValorContabil, double TaxaCofins)
        {
            return ((ValorContabil * TaxaCofins) / 100) / 12;
 
        }

        protected internal virtual double CalculaDepreciacaoLinearAnual(double TaxaDepreciacao, double CoeficienteDepreciao, double ValorDepreciavel)
        {
            return ((TaxaDepreciacao * CoeficienteDepreciao) * ValorDepreciavel) / 100;
        }

        protected internal virtual double CalculaDepreciacaoLinearMensal(double TaxaDepreciacao, double CoeficienteDepreciao, double ValorDepreciavel)
        {
            return (((TaxaDepreciacao * CoeficienteDepreciao) * ValorDepreciavel) / 100)/12;
        }

        protected internal virtual double CalculaDepreciacaoSomaDigitosAnual(double TotalDiferencaDeMeses, double SomaDigitosMesesVidaUtil,double ValorDepreciavel)
        {
            return ((TotalDiferencaDeMeses / SomaDigitosMesesVidaUtil) * ValorDepreciavel) *12; 
        }

        protected internal virtual double CalculaDepreciacaoSomaDigitosMensal(double TotalDiferencaDeMeses, double SomaDigitosMesesVidaUtil, double ValorDepreciavel)
        {
            return ((TotalDiferencaDeMeses / SomaDigitosMesesVidaUtil) * ValorDepreciavel);
        }

        protected internal virtual double CalculaTaxaMensal(double Coeficiente, double Taxa)
        {
            return (100 / (100 / (Taxa * Coeficiente))) / 12; 
        }

        protected internal virtual double CalcularDepreciacaoReducaoSaldosAnual(double ValorContabil,double ValorCompra, double VidaUtilAnos, double ValorResidual)
        {   
            double Taxa = 1- (Math.Pow((ValorResidual/ValorCompra),1/VidaUtilAnos));
            return ValorContabil * Taxa;

        }
       
        protected internal virtual double CalcularDepreciacaoReducaoSaldosMensal(double ValorContabil, double ValorCompra, double VidaUtilAnos, double ValorResidual)
        {
            double VidaUtilMeses = VidaUtilAnos * 12;
            double Taxa = 1 - (Math.Pow((ValorResidual / ValorCompra), 1 / VidaUtilMeses));
            return ValorContabil * Taxa;

        }

        protected internal virtual double CalcularDepreciacaoUnidadesProduzidasAnual(double UnidadesProduzidasPeriodo, double UnidadesProduzidasEstimadaDuranteVidaUtil, double ValorContabil)
        {
            double Taxa = UnidadesProduzidasPeriodo / UnidadesProduzidasEstimadaDuranteVidaUtil;
            return ValorContabil * Taxa;
        }

        protected internal virtual double CalcularDepreciacaoUnidadesProduzidasMensal(double UnidadesProduzidasPeriodo, double UnidadesProduzidasEstimadaDuranteVidaUtil, double ValorContabil)
        {
            double Taxa = UnidadesProduzidasPeriodo / UnidadesProduzidasEstimadaDuranteVidaUtil;
            return ValorContabil * Taxa;
        }

        protected internal virtual double CalcularDepreciacaoHorasTrabalhadasAnual(double HorasTrabalhadasPeriodo, double HorasTrabalhadasDuranteVidaUtil, double ValorContabil)
        {
            double Taxa = HorasTrabalhadasPeriodo / HorasTrabalhadasDuranteVidaUtil;
            return ValorContabil * Taxa;
        }

        protected internal virtual double CalcularDepreciacaoHorasTrabalhadasMensal(double HorasTrabalhadasPeriodo, double HorasTrabalhadasDuranteVidaUtil, double ValorContabil)
        {
            double Taxa = HorasTrabalhadasPeriodo / HorasTrabalhadasDuranteVidaUtil;
            return ValorContabil * Taxa;
        }

        protected internal virtual double CalculaDepreciacaoLinearComValorMaximoDepreciacaoAnual(double Taxa, double Coeficiente, double ValorMaximoDepreciacaoAnual)
        {
            return (CalculaTaxaMensal(Coeficiente,Taxa)*12) * ValorMaximoDepreciacaoAnual;
        }

        protected internal virtual double CalculaDepreciacaoLinearComValorMaximoDepreciacaoMensal(double Taxa, double Coeficiente, double ValorMaximoDepreciacaoAnual)
        {
            return CalculaTaxaMensal(Taxa,Coeficiente) * ValorMaximoDepreciacaoAnual;
        }

        protected internal virtual double CalculaVidaUtilAnos(double CoeficienteDepreciacao, double TaxaDepreciacao)
        { 
            return (100 / (CoeficienteDepreciacao * TaxaDepreciacao));

        }

        protected internal virtual double CalculaDepreciacaoVariacaoDasTaxasMensal(double ValorDepreciavel, double TaxaVidaUtilRestanteMensal)
        {

            return (ValorDepreciavel * TaxaVidaUtilRestanteMensal) / 100; 
        }

        protected internal virtual double CalculaDepreciacaoVariacaoDasTaxasAnual(double ValorDepreciavel, double TaxaVidaUtilRestanteAnual)
        {

            return (ValorDepreciavel * TaxaVidaUtilRestanteAnual) / 100;
        }


    }
}