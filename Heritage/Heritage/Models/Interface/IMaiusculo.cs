using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heritage.Models.Interface
{
    interface IMaiusculo
    {
        string TransformarParaMaiusculo(string Campo);
        string PrimeiraLetraMaiuscula(string campo);
    }
}
