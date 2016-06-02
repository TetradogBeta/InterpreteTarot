using Gabriel.Cat.Extension;
using Microsoft.Win32;
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
        enum BlockText
        {
            Significado,Pasado,Presente,Futuro
        }
        CartaTarot cartaTemp,carta;
        BlockText blockActual;

        public WinEditorCreadorCartas()
        {
            cartaTemp = new CartaTarot();
            carta = new CartaTarot();
            blockActual = BlockText.Significado;
            InitializeComponent();
           
        }
        public WinEditorCreadorCartas(CartaTarot cartaAEditar) : this()
        {
            carta = cartaAEditar.ClonProfundoConPropiedades();
            cartaTemp = cartaAEditar;
            //pongo toda la info
            imgCarta.SetImage(cartaTemp.Imagen);
            imgSigno1.SetImage(cartaTemp.Signo1);
            imgSigno2.SetImage(cartaTemp.Signo2);
            imgSigno3.SetImage(cartaTemp.Signo3);
            txt.Text = cartaTemp.Significado;
            txtNombreCarta.Text = cartaTemp.Nombre;
            txtPalabrasClave.Text = cartaTemp.PalabrasClave;
            
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
        private void CambiarImagen_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog opnFile = new OpenFileDialog();
            Image img = sender as Image;
            System.Drawing.Bitmap bmp;
            if(opnFile.ShowDialog().Value)
            {
                bmp = new System.Drawing.Bitmap(opnFile.FileName);
                switch (img.Name)
                {
                    case "imgCarta":
                        
                        cartaTemp.Imagen = bmp;


                        break;
                    case "imgSigno1":
                        cartaTemp.Signo1 =bmp;
                     
                        break;
                    case "imgSigno2":
                        cartaTemp.Signo2 = bmp;
                   
                        break;
                    case "iimgSigno3":
                        cartaTemp.Signo3 = bmp;
                   
                        break;
                }
                img.SetImage(bmp);
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            carta.SetBytes(cartaTemp.GetBytes());
        }

        private void CambiarTexto_Checked(object sender, RoutedEventArgs e)
        {
            if (txt != null)
            {
                switch (blockActual)
                {
                    case BlockText.Significado: cartaTemp.Significado = txt.Text; break;
                    case BlockText.Pasado: cartaTemp.Pasado = txt.Text; break;
                    case BlockText.Presente: cartaTemp.Presente = txt.Text; break;
                    case BlockText.Futuro: cartaTemp.Futuro = txt.Text; break;

                }
                switch (((RadioButton)sender).Name)
                {
                    case "rbSignificado": blockActual = BlockText.Significado; txt.Text = cartaTemp.Significado; break;
                    case "rbPasado": blockActual = BlockText.Pasado; txt.Text = cartaTemp.Pasado; break;
                    case "rbPresente": blockActual = BlockText.Presente; txt.Text = cartaTemp.Presente; break;
                    case "rbFuturo": blockActual = BlockText.Futuro; txt.Text = cartaTemp.Futuro; break;
                }
            }
        }
    }
}
