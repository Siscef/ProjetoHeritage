using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Heritage.Models
{
    public class Graficos
    {
        [Display(Name="Largura: ")]
        public virtual int Largura { get; set; }
        [Display(Name = "Altura: ")]
        public virtual int Altura { get; set; }
        [Display(Name="Cor:")]
        public virtual Cores Cor { get; set; }
        [Display(Name="Tipo de gráfico:")]
        public virtual TipoGrafico TipoGrafico { get; set; }
        [Display(Name="Número de elementos:")]
        public virtual int NumeroElementos { get; set; }
        [Display(Name= "Valor padrão:")]
        public virtual bool ValorPadrao { get; set; }
    }

    public enum Cores
    {
        Amarelo = 1,
        Azul = 2,
        Verde = 3,
        Vermelho = 4
    }

    public enum TipoGrafico
    {
        Padrao,
        Area,
        Bar,
        BoxPlot,
        Bubble,
        Candlestick,
        Column,
        Doughnut,
        ErrorBar,
        FastLine,
        FastPoint,
        Funnel,
        Kagi,
        Line,
        Pie,
        Point,
        PointAndFigure,
        Polar,
        Pyramid,
        Radar,
        Range,
        RangeBar,
        RangeColumn


    }
}