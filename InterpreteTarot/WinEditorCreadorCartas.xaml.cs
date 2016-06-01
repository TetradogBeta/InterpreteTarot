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

namespace InterpreteTarot
{
    /// <summary>
    /// Interaction logic for WinEditorCreadorCartas.xaml
    /// </summary>
    public partial class WinEditorCreadorCartas : Window
    {
        CartaTarot carta;
        public WinEditorCreadorCartas()
        {
            InitializeComponent();
            carta = new CartaTarot();
        }
        public WinEditorCreadorCartas(CartaTarot cartaAEditar) : this()
        {
            carta = cartaAEditar;
            //pongo toda la info
        }
    }
}
