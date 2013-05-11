using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Heritage.Models.Interface
{
    public interface IMaiusculo
    {
        string TransformarParaMaiusculo(string Campo);
        string PrimeiraLetraMaiuscula(string campo);
    }
}
