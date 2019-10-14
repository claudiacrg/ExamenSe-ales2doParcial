using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorDeSeñales
{
    class SeñalExponencial : Señal
    {
        public double Alfa;

        public SeñalExponencial()
        {
            Alfa = 0.0;
            AmplitudMaxima = 0.0;
            Muestras = new List<Muestra>();
        }

        public SeñalExponencial(double alfa)
        {
            Alfa = alfa;
            AmplitudMaxima = 0.0;
            Muestras = new List<Muestra>();
        }

        public override double evaluar(double tiempo)
        {
            double resultado;

            resultado = Math.Exp(Alfa * tiempo);

            return resultado;
        }
    }
}
