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
using System.Windows.Shapes;
using Gabriel.Cat.Extension;
namespace InterpreteTarot
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class WinVisorTirada : Window
    {
        CartaSelector[] tirada;
        public WinVisorTirada(CartaTarot cartaPasado,CartaTarot cartaPresente,CartaTarot cartaFuturo)
        {
            InitializeComponent();
            tirada = new CartaSelector[] {cslPasado,cslPresente,cslFuturo };
            cslPasado.Carta = cartaPasado;
            cslPresente.Carta = cartaPresente;
            cslFuturo.Carta = cartaFuturo;
            VisualizaCarta(cartaPasado, PosicionCartas.Pasado);

        }

        private void csl_Seleccionada(object sender, CartaSeleccionadaArgs e)
        {
            PosicionCartas posicion=PosicionCartas.Pasado;
            for (int i = 0; i < tirada.Length; i++)
                if (tirada[i] != sender)
                    tirada[i].Seleccionado = false;
                else posicion = (PosicionCartas)i;
            VisualizaCarta(e.Carta,posicion);
        }

        private void VisualizaCarta(CartaTarot carta,PosicionCartas posicion)
        {
            tblNombreCarta.Text = carta.Nombre;
            imgCarta.SetImage(carta.Imagen);
            if (carta.Signo1 != null)
                imgSigno1.SetImage(carta.Signo1);
            if (carta.Signo2 != null)
                imgSigno1.SetImage(carta.Signo2);
            if (carta.Signo3!= null)
                imgSigno1.SetImage(carta.Signo3);
            txtSignificadoCarta.Text = carta.Significado;
            tblPosicion.Text = posicion.ToString();
            switch(posicion)
            {
                case PosicionCartas.Pasado:txtSignificadoPosicion.Text = carta.Pasado;break;
                case PosicionCartas.Presente: txtSignificadoPosicion.Text = carta.Presente; break;
                case PosicionCartas.Futuro: txtSignificadoPosicion.Text = carta.Futuro; break;
            }
            lstPalabrasClave.Items.Clear();
            lstPalabrasClave.Items.AddRange(carta.PalabrasClave.Split('·'));
            
        }
    }
}
