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
            new WinEditorCreadorCartas().ShowDialog();
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
            Image cartaACargar;
            ContextMenu contextMenuCartas;
            MenuItem itemMenu;
            string hexCarta = "";
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
                    hexCarta = cartasCargadas[i].GetBytes().ToHex();
                    if (!this.cartasCargadas.Existeix(hexCarta))
                    {
                        this.cartasCargadas.Afegir(hexCarta, cartasCargadas[i]);
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


                        cartaACargar.SetImage(cartasCargadas[i].Imagen);
                        cartaACargar.Tag = cartasCargadas[i];
                        cartaACargar.MouseLeftButtonUp += PonCarta;
                        cartaACargar.ContextMenu = contextMenuCartas;
                        ugCartasTarot.Children.Add(cartaACargar);
                        if (!System.IO.File.Exists(CartaTarot.pathCartasCarpetaGuardado + System.IO.Path.AltDirectorySeparatorChar + cartasCargadas[i].Nombre + CartaTarot.ExtensionCarta))
                            CartaTarot.GuardarCarta(cartasCargadas[i]);
                    }
                }
            }
        }

        private void PonCarta(object sender, MouseButtonEventArgs e)
        {
            bool cartaPuesta = false;
            Image imgSender = sender as Image;
            //miro que no este puesta...
            for (PosicionCartas i = PosicionCartas.Pasado; i <= PosicionCartas.Futuro && !cartaPuesta; i++)
                cartaPuesta = Equals(imgs[(int)i].Tag, imgSender.Tag);
            if (cartaPuesta)
            {
                if (MessageBox.Show("La carta ya esta en la tirada actualmente, la quieres quitar de la posicion donde esta?", "Carta puesta", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    cartaPuesta = false;
                    for (PosicionCartas i = PosicionCartas.Pasado; i <= PosicionCartas.Futuro && !cartaPuesta; i++)
                    {
                        cartaPuesta = Equals(imgs[(int)i].Tag, imgSender.Tag);
                        if (cartaPuesta)
                        {
                            QuitarCartaDeLaTirada(imgs[(int)i]);
                        }
                    }
                    cartaPuesta = false;
                    SiNoEstaPuestaPonlaEnLaPosicion(cartaPuesta, imgSender);
                }
            }
            else
            {
                //se pone en la primera imagen con tag==null
                for (PosicionCartas i = PosicionCartas.Pasado; i <= PosicionCartas.Futuro && !cartaPuesta; i++)
                    if (imgs[(int)i].Tag == null)
                    {
                        imgs[(int)i].Source = imgSender.Source;
                        imgs[(int)i].Tag = imgSender.Tag;
                        cartaPuesta = true;
                        posicionActual = (PosicionCartas)(((int)i + 1) % ((int)PosicionCartas.Futuro + 1));
                    }
                SiNoEstaPuestaPonlaEnLaPosicion(cartaPuesta, imgSender);
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
