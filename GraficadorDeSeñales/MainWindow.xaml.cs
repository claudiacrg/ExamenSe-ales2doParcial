using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraficadorDeSeñales
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mostrarSegundaSeñal(false);
        }

        private void BtnGraficar_Click(object sender, RoutedEventArgs e)
        {
            /*double amplitud = double.Parse(txtAmplitud.Text);
            double fase = double.Parse(txtFase.Text);
            double frecuencia = double.Parse(txtFrecuencia.Text);*/
            double tiempoInicial = double.Parse(txtTiempoInicial.Text);
            double tiempoFinal = double.Parse(txtTiempoFinal.Text);
            double frecuenciaDeMuestreo = double.Parse(txtFrecuenciaDeMuestreo.Text);

            Señal señal;
            Señal segundaSeñal = null;
            Señal señalResultante;

            switch (cbTipoSeñal.SelectedIndex)
            {
                case 0: // Parabólica
                    señal = new SeñalParabolica();
                    break;
                case 1: // Senoidal
                    double amplitud =
                        double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion.Children[0])).txtAmplitud.Text);
                    double fase =
                        double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion.Children[0])).txtFase.Text);
                    double frecuencia =
                        double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion.Children[0])).txtFrecuencia.Text);

                    señal = new SeñalSenoidal(amplitud, fase, frecuencia);
                    break;
                case 2: // Exponencial
                    double alfa = double.Parse(((ConfiguracionSeñalExponencial)(panelConfiguracion.Children[0])).txtAlfa.Text);
                    señal = new SeñalExponencial(alfa);
                    break;
                case 3: // Audio
                    string rutaArchivo = ((ConfiguracionAudio)(panelConfiguracion.Children[0])).txtRutaArchivo.Text;
                    señal = new SeñalAudio(rutaArchivo);
                    txtTiempoInicial.Text = señal.TiempoInicial.ToString();
                    txtTiempoFinal.Text = señal.TiempoFinal.ToString();
                    txtFrecuenciaDeMuestreo.Text = señal.FrecuenciaDeMuestreo.ToString();
                    break;
                default:
                    señal = null;
                    break;
            }

            if (cbTipoSeñal.SelectedIndex != 3 && señal != null)
            {
                señal.TiempoInicial = tiempoInicial;
                señal.TiempoFinal = tiempoFinal;
                señal.FrecuenciaDeMuestreo = frecuenciaDeMuestreo;

                señal.construirSeñal();
            }

            //Construir segunda señal si es necesario
            if(cbOperacion.SelectedIndex == 2 || cbOperacion.SelectedIndex == 4)
            {
                switch (cbTipoSeñal_2.SelectedIndex)
                {
                    case 0: // Parabólica
                        segundaSeñal = new SeñalParabolica();
                        break;
                    case 1: // Senoidal
                        double amplitud =
                            double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion_2.Children[0])).txtAmplitud.Text);
                        double fase =
                            double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion_2.Children[0])).txtFase.Text);
                        double frecuencia =
                            double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion_2.Children[0])).txtFrecuencia.Text);

                        segundaSeñal = new SeñalSenoidal(amplitud, fase, frecuencia);
                        break;
                    case 2: // Exponencial
                        double alfa = double.Parse(((ConfiguracionSeñalExponencial)(panelConfiguracion_2.Children[0])).txtAlfa.Text);
                        segundaSeñal = new SeñalExponencial(alfa);
                        break;
                    case 3: // Audio
                        string rutaArchivo = ((ConfiguracionAudio)(panelConfiguracion_2.Children[0])).txtRutaArchivo.Text;
                        segundaSeñal = new SeñalAudio(rutaArchivo);
                        txtTiempoInicial.Text = segundaSeñal.TiempoInicial.ToString();
                        txtTiempoFinal.Text = segundaSeñal.TiempoFinal.ToString();
                        txtFrecuenciaDeMuestreo.Text = segundaSeñal.FrecuenciaDeMuestreo.ToString();
                        break;
                    default:
                        segundaSeñal = null;
                        break;
                }
                if (cbTipoSeñal_2.SelectedIndex != 3 && señal != null)
                {
                    segundaSeñal.TiempoInicial = tiempoInicial;
                    segundaSeñal.TiempoFinal = tiempoFinal;
                    segundaSeñal.FrecuenciaDeMuestreo = frecuenciaDeMuestreo;

                    segundaSeñal.construirSeñal();
                }
            }

            switch (cbOperacion.SelectedIndex)
            {
                case 0: //Escala de amplitud
                    double factorEscala = double.Parse(((OperacionEscalaAmplitud)(panelConfiguracionOperacion.Children[0])).txtFactorEscala.Text);
                    señalResultante = Señal.escalarAmplitud(señal, factorEscala);
                    break;
                case 1: //Desplazamiento de amplitud
                    double cantidadDesplazamiento = double.Parse(((OperacionDesplazamientoAmplitud)(panelConfiguracionOperacion.Children[0])).txtCantidadDesplazamiento.Text);
                    señalResultante = Señal.desplazarAmplitud(señal, cantidadDesplazamiento);
                    break;
                case 2: //Multiplicación
                    señalResultante = Señal.multiplicarSeñales(señal, segundaSeñal);
                    break;
                case 3: //Escala Exponencial
                    double exponente = double.Parse(((OperacionEscalaEsponencial)(panelConfiguracionOperacion.Children[0])).txtExponente.Text);
                    señalResultante = Señal.escalaExponencial(señal, exponente);
                    break;
                case 4: //Límite Superior
                    señalResultante = Señal.limiteSuperior(señal, segundaSeñal);
                    break;
                default:
                    señalResultante = null;
                    break;
            }

            //Elige entre la 1ra y la resultante
            double amplitudMaxima = 
                (señal.AmplitudMaxima >= señalResultante.AmplitudMaxima) 
                ? señal.AmplitudMaxima : señalResultante.AmplitudMaxima;
            if (segundaSeñal != null)
            {
                //Elige entre la mas grande de la 1ra y resultante y la segunda
                amplitudMaxima = (amplitudMaxima >= segundaSeñal.AmplitudMaxima)
                ? amplitudMaxima : segundaSeñal.AmplitudMaxima;
            }

            plnGraficaResultante.Points.Clear();
            plnGrafica.Points.Clear();

            plnGrafica_2.Points.Clear();

            if (segundaSeñal != null)
            {
                foreach (var muestra in segundaSeñal.Muestras)
                {
                    plnGrafica_2.Points.Add(adaptarCoordenadas(muestra.X, muestra.Y, tiempoInicial, amplitudMaxima));
                }
            }
            foreach (Muestra muestra in señal.Muestras)
            {
                plnGrafica.Points.Add(adaptarCoordenadas(muestra.X, muestra.Y, tiempoInicial, amplitudMaxima));
            }

            foreach (Muestra muestra in señalResultante.Muestras)
            {
                plnGraficaResultante.Points.Add(adaptarCoordenadas(muestra.X, muestra.Y, tiempoInicial, amplitudMaxima));
            }

                lblLimiteSuperior.Text = amplitudMaxima.ToString("F");
                lblLimiteInferior.Text = "-" + amplitudMaxima.ToString("F");

                lblLimiteInferiorResultante.Text = "-" + amplitudMaxima.ToString("F");
                lblLimiteSuperiorResultante.Text = amplitudMaxima.ToString("F");

            //Original
            plnEjeX.Points.Clear();
            plnEjeX.Points.Add(
                adaptarCoordenadas(tiempoInicial, 0.0, tiempoInicial, amplitudMaxima)
                );
            plnEjeX.Points.Add(
                adaptarCoordenadas(tiempoFinal, 0.0, tiempoInicial, amplitudMaxima)
                );

            plnEjeY.Points.Clear();
            plnEjeY.Points.Add(adaptarCoordenadas(0.0, -amplitudMaxima, tiempoInicial, amplitudMaxima));
            plnEjeY.Points.Add(adaptarCoordenadas(0.0, amplitudMaxima, tiempoInicial, amplitudMaxima));

            //Resultado
            plnEjeXResultante.Points.Clear();
            plnEjeXResultante.Points.Add(adaptarCoordenadas(tiempoInicial, 0.0, tiempoInicial, amplitudMaxima));
            plnEjeXResultante.Points.Add(adaptarCoordenadas(tiempoFinal, 0.0, tiempoInicial, amplitudMaxima));

            plnEjeYResultante.Points.Clear();
            plnEjeYResultante.Points.Add(adaptarCoordenadas(0.0, amplitudMaxima, tiempoInicial, amplitudMaxima));
            plnEjeYResultante.Points.Add(adaptarCoordenadas(0.0, -amplitudMaxima, tiempoInicial, amplitudMaxima));
        }

        public Point adaptarCoordenadas(double x, double y, double tiempoInicial, double amplitudMaxima)
        {
            return new Point((x - tiempoInicial) * scrGrafica.Width, -1 * ( y * ( ( ( ( scrGrafica.Height ) / 2.0) - 25) / amplitudMaxima) ) + ( scrGrafica.Height / 2.0 ) );
        }

        private void CbTipoSeñal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            panelConfiguracion.Children.Clear();
            switch(cbTipoSeñal.SelectedIndex){
                case 0: //Parabólica
                    break;
                case 1: //Senoidal
                    panelConfiguracion.Children.Add(new ConfiguracionSeñalSenoidal());
                    break;
                case 2: //Exponencial
                    panelConfiguracion.Children.Add(new ConfiguracionSeñalExponencial());
                    break;
                case 3: //Audio
                    panelConfiguracion.Children.Add(new ConfiguracionAudio());
                    break;
                default:
                    break;
            }
        }

        private void CbOperacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mostrarSegundaSeñal(false);
            panelConfiguracionOperacion.Children.Clear();
            switch(cbOperacion.SelectedIndex){
                case 0: //Escala de Amplitud
                    panelConfiguracionOperacion.Children.Add(new OperacionEscalaAmplitud());
                    break;
                case 1: //Cantidad de Desplazamiento
                    panelConfiguracionOperacion.Children.Add(new OperacionDesplazamientoAmplitud());
                    break;
                case 2: //Multiplicación de Señales
                    mostrarSegundaSeñal(true);
                    break;
                case 3: //Exponencial
                    panelConfiguracionOperacion.Children.Add(new OperacionEscalaEsponencial());
                    break;
                case 4: //Límite Superior
                    mostrarSegundaSeñal(true);
                    break;
                default:
                    break;
            }
        }

        private void CbTipoSeñal_2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            panelConfiguracion_2.Children.Clear();
            switch (cbTipoSeñal_2.SelectedIndex)
            {
                case 0: //Parabólica
                    break;
                case 1: //Senoidal
                    panelConfiguracion_2.Children.Add(new ConfiguracionSeñalSenoidal());
                    break;
                case 2: //Exponencial
                    panelConfiguracion_2.Children.Add(new ConfiguracionSeñalExponencial());
                    break;
                case 3: //Audio
                    panelConfiguracion_2.Children.Add(new ConfiguracionAudio());
                    break;
                default:
                    break;
            }

        }

        void mostrarSegundaSeñal(bool mostrar)
        {
            if (mostrar)
            {
                lblTipoSeñal_2.Visibility = Visibility.Visible;
                cbTipoSeñal_2.Visibility = Visibility.Visible;
                panelConfiguracion_2.Visibility = Visibility.Visible;
            } else
            {
                lblTipoSeñal_2.Visibility = Visibility.Hidden;
                cbTipoSeñal_2.Visibility = Visibility.Hidden;
                panelConfiguracion_2.Visibility = Visibility.Hidden;
            }
        }
    }
}