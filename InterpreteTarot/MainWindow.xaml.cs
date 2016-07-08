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
using Gabriel.Cat.Extension;
using WinForms = System.Windows.Forms;//se pueden poner abreviaciones :D
using Gabriel.Cat;

namespace InterpreteTarot
{
    public enum PosicionCartas
    {
        Pasado, Presente, Futuro
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        PosicionCartas posicionActual;
        Image[] imgs;
        LlistaOrdenada<string, CartaTarot> cartasCargadas;
        public MainWindow()
        {
            MenuItem itemMenu;
            ContextMenu contextMenuGridCartas = new ContextMenu();
            cartasCargadas = new LlistaOrdenada<string, CartaTarot>();
            itemMenu = new MenuItem();
            itemMenu.Header = "Crear Carta";
            itemMenu.Click += CreadorDeCartas;
            contextMenuGridCartas.Items.Add(itemMenu);
            itemMenu = new MenuItem();
            itemMenu.Header = "Cargar cartas";
            itemMenu.Click += CargarCartas;
            contextMenuGridCartas.Items.Add(itemMenu);
            InitializeComponent();
            ContextMenu = contextMenuGridCartas;
            imgs = new Image[] { imgPasado, imgPresente, imgFuturo };
            CargarCartas();
        }

        private void CreadorDeCartas(object sender, RoutedEventArgs e)
        {
            WinEditorCreadorCartas creador = new WinEditorCreadorCartas();
            Image imgCarta=null;
            creador.ShowDialog();
            if (!cartasCargadas.Existeix(creador.Carta.Nombre)||MessageBox.Show("Ya Existe quieres reemplazarla?","Atención",MessageBoxButton.YesNo,MessageBoxImage.Asterisk)==MessageBoxResult.Yes)
            {
                if(cartasCargadas.Existeix(creador.Carta.Nombre))
                {
                    //la quito
                    ugCartasTarot.Children.WhileEach((carta) => {
                        imgCarta = carta as Image;
                        if ((imgCarta.Tag as CartaTarot).Nombre==creador.Carta.Nombre)
                        {
                            cartasCargadas.Elimina(creador.Carta.Nombre);
                        }
                    return cartasCargadas.Existeix(creador.Carta.Nombre);

                    });
                    ugCartasTarot.Children.Remove(imgCarta);

                }
                CargarCarta(creador.Carta);
            }

        }
        private void CargarCartas(object sender, RoutedEventArgs e)
        {
            CartaTarot.pathCartasCargar = "";
            CargarCartas();
        }
        private void CargarCartas()
        {
            CartaTarot[] cartasCargadas = CartaTarot.Cargar();
            WinForms.FolderBrowserDialog folderCartas;

            string nombreCarta = "";
            if (cartasCargadas.Length == 0)
            {
                if (ugCartasTarot.Children.Count == 0)
                    MessageBox.Show("No se ha encontrado las cartas del tartot");

                folderCartas = new WinForms.FolderBrowserDialog();
                if (folderCartas.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    CartaTarot.pathCartasCargar = folderCartas.SelectedPath;
                    CargarCartas();
                }

            }
            else
            {
                //las cargo :D
                for (int i = 0; i < cartasCargadas.Length; i++)
                {
                    nombreCarta = cartasCargadas[i].Nombre;
                    try
                    {
                        CargarCarta(cartasCargadas[i]);
                    }
                    catch { }
                }
            }
        }

        private void CargarCarta(CartaTarot cartaCargada)
        {
            Image cartaACargar;
            ContextMenu contextMenuCartas;
            MenuItem itemMenu;
            if (cartaCargada!=null&&!this.cartasCargadas.Existeix(cartaCargada.Nombre))
            {
                this.cartasCargadas.Afegir(cartaCargada.Nombre, cartaCargada);
                contextMenuCartas = new ContextMenu();
                cartaACargar = new Image();
                itemMenu = new MenuItem();
                itemMenu.Header = "Crear Carta";
                itemMenu.Click += CreadorDeCartas;
                contextMenuCartas.Items.Add(itemMenu);

                itemMenu = new MenuItem();
                itemMenu.Header = "Editar carta";
                itemMenu.Tag = cartaACargar;
                itemMenu.Click += (s, e) =>
                {
                    Image imgCartaAEditar = ((MenuItem)s).Tag as Image;
                    CartaTarot cartaAEditar = imgCartaAEditar.Tag as CartaTarot;
                    //abro el editor de cartas con la carta
                    new WinEditorCreadorCartas(cartaAEditar).ShowDialog();
                    //si a cambiado la imagen la actualizo
                    imgCartaAEditar.SetImage(cartaAEditar.Imagen);
                    CartaTarot.GuardarCarta(cartaAEditar);//actualizo los datos
                 
                };
                contextMenuCartas.Items.Add(itemMenu);


                cartaACargar.SetImage(cartaCargada.Imagen);
                cartaACargar.Tag = cartaCargada;
                cartaACargar.MouseLeftButtonUp += PonCarta;
                cartaACargar.ContextMenu = contextMenuCartas;
                ugCartasTarot.Children.Add(cartaACargar);
                if (!System.IO.File.Exists(CartaTarot.pathCartasCarpetaGuardado + System.IO.Path.AltDirectorySeparatorChar + cartaCargada.Nombre + CartaTarot.ExtensionCarta))
                    CartaTarot.GuardarCarta(cartaCargada);
            }
        }

        private void PonCarta(object sender, MouseButtonEventArgs e)
        {
            bool acabado = false;

            Image imgSender = sender as Image;
            //miro que no este puesta
            for (PosicionCartas i = PosicionCartas.Pasado; i <= PosicionCartas.Futuro && !acabado; i++)
            {

                if (Equals(imgs[(int)i].Tag, imgSender.Tag))
                {
                    QuitarCartaDeLaTirada(imgs[(int)i]);
                    SiNoEstaPuestaPonlaEnLaPosicion(acabado, imgSender);
                    acabado = true;
                }

            }
            if (!acabado)
            {
                //se pone en la primera imagen con tag==null
                for (PosicionCartas i = PosicionCartas.Pasado; i <= PosicionCartas.Futuro && !acabado; i++)
                    if (imgs[(int)i].Tag == null)
                    {
                        imgs[(int)i].Source = imgSender.Source;
                        imgs[(int)i].Tag = imgSender.Tag;
                        acabado = true;
                        posicionActual = (PosicionCartas)(((int)i + 1) % ((int)PosicionCartas.Futuro + 1));
                    }
                SiNoEstaPuestaPonlaEnLaPosicion(acabado, imgSender);
            }
        }

        private void SiNoEstaPuestaPonlaEnLaPosicion(bool cartaPuesta, Image imgSender)
        {
            if (!cartaPuesta)
            {
                imgs[(int)posicionActual].Source = imgSender.Source;
                imgs[(int)posicionActual].Tag = imgSender.Tag;
                posicionActual = (PosicionCartas)(((int)posicionActual + 1) % ((int)PosicionCartas.Futuro + 1));
            }
        }

        private void QuitarCartaDeLaTirada(object sender, MouseButtonEventArgs e = null)
        {
            Image imgParaQuitar = sender as Image;
            imgParaQuitar.SetImage(Colors.White.ToBitmap(1, 1));
            imgParaQuitar.Tag = null;
        }

        private void btnInterpretar_Click(object sender, RoutedEventArgs e)
        {
            bool completo = true;
            for (int i = 0; i < imgs.Length; i++)
                completo = imgs[i].Tag != null;
            if (completo)
            {
                new WinVisorTirada(imgPasado.Tag as CartaTarot, imgPresente.Tag as CartaTarot, imgFuturo.Tag as CartaTarot).ShowDialog();
            }
            else
            {
                MessageBox.Show("Faltan cartas por poner!", "Tirada Incompleta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }
    }
}
