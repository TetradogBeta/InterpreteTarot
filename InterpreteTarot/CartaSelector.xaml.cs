using Gabriel.Cat.Extension;
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

namespace InterpreteTarot
{
    /// <summary>
    /// Interaction logic for CartaSelector.xaml
    /// </summary>
    public partial class CartaSelector : UserControl
    {
        bool seleccionada;
        CartaTarot carta;
        public event EventHandler<CartaSeleccionadaArgs> Seleccionada;
        public CartaSelector()
        {
            InitializeComponent();
        }

        public bool Seleccionado { get { return seleccionada; }  set
            {
                if(value)
                {
                    grid.Background = Brushes.Yellow;
                    if (Seleccionada != null)
                        Seleccionada(this, new CartaSeleccionadaArgs(carta));
                }
                else
                {
                    grid.Background = Brushes.White;
                }
                seleccionada = value;
            }

        }

        public CartaTarot Carta
        {
            get
            {
                return carta;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                carta = value;
                imgCarta.SetImage(carta.Imagen);
            }
        }

        private void imgCarta_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Seleccionado = true;
        }
    }
    public class CartaSeleccionadaArgs:EventArgs
    {
        CartaTarot carta;

        public CartaSeleccionadaArgs(CartaTarot carta)
        {
            this.Carta = carta;
        }

        public CartaTarot Carta
        {
            get
            {
                return carta;
            }

           private set
            {
                carta = value;
            }
        }
    }
}
