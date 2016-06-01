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
using WinForms=System.Windows.Forms;//se pueden poner abreviaciones :D
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
        ContextMenu contextMenuCartas;
        public MainWindow()
        {
            MenuItem itemMenu;
            ContextMenu contextMenuGridCartas = new ContextMenu();
            contextMenuCartas = new ContextMenu();
            itemMenu = new MenuItem();
            itemMenu.Header = "Crear Carta";
            itemMenu.MouseLeftButtonUp += CreadorDeCartas;
            contextMenuCartas.Items.Add(itemMenu);
            itemMenu = new MenuItem();
            itemMenu.Header = "Crear Carta";
            itemMenu.MouseLeftButtonUp += CreadorDeCartas;
            contextMenuGridCartas.Items.Add(itemMenu);
            itemMenu = new MenuItem();
            itemMenu.Header = "Editar carta";
            itemMenu.MouseLeftButtonUp += (s, e) => {
                CartaTarot cartaAEditar=((Image)s).Tag as CartaTarot;
                //abro el editor de cartas con la carta
                new WinEditorCreadorCartas(cartaAEditar).ShowDialog();
            };
            contextMenuCartas.Items.Add(itemMenu);
            InitializeComponent();
            ugCartasTarot.ContextMenu = contextMenuGridCartas;
            imgs = new Image[] { imgPasado,imgPresente,imgFuturo};
            CargarCartas();
        }

        private void CreadorDeCartas(object sender, MouseButtonEventArgs e)
        {
            new WinEditorCreadorCartas().ShowDialog();
        }

        private void CargarCartas()
        {
            CartaTarot[] cartasCargadas = CartaTarot.Cargar();
            WinForms.FolderBrowserDialog folderCartas;
            Image cartaACargar;
            if(cartasCargadas.Length==0)
            {
                MessageBox.Show("No se ha encontrado las cartas del tartot");
                if(MessageBox.Show("Quieres buscarlas?","No se ha encontrado las cartas en la carpeta "+CartaTarot.pathCartas,MessageBoxButton.YesNo)==MessageBoxResult.Yes)
                {
                    folderCartas = new WinForms.FolderBrowserDialog();
                    if (folderCartas.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        CartaTarot.pathCartas = folderCartas.SelectedPath;
                        CargarCartas();
                    }
                }
            }else
            {
                //las cargo :D
                for(int i=0;i<cartasCargadas.Length;i++)
                {
                    cartaACargar = new Image();
                    cartaACargar.SetImage(cartasCargadas[i].Imagen);
                    cartaACargar.Tag = cartasCargadas[i];
                    cartaACargar.MouseLeftButtonUp += PonCarta;
                    cartaACargar.ContextMenu = contextMenuCartas;
                    ugCartasTarot.Children.Add(cartaACargar);
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
             if(MessageBox.Show("La carta ya esta en la tirada actualmente, la quieres quitar de la posicion donde esta?","Carta puesta",MessageBoxButton.YesNo)==MessageBoxResult.Yes)
                {
                    cartaPuesta = false;
                    for (PosicionCartas i = PosicionCartas.Pasado; i <= PosicionCartas.Futuro && !cartaPuesta; i++)
                    {
                        cartaPuesta = Equals(imgs[(int)i].Tag, imgSender.Tag);
                        if(cartaPuesta)
                        {
                            QuitarCartaDeLaTirada(imgs[(int)i]);
                        }
                    }
                    cartaPuesta = false;
                }
                    //se pone en la primera imagen con tag==null
            for (PosicionCartas i=PosicionCartas.Pasado;i<=PosicionCartas.Futuro&&!cartaPuesta;i++)
                if(imgs[(int)i].Tag==null)
                {
                    imgs[(int)i].Source = imgSender.Source;
                    imgs[(int)i].Tag = imgSender.Tag;
                    cartaPuesta = true;
                    posicionActual = (PosicionCartas)(((int)i + 1) % ((int)PosicionCartas.Futuro + 1));
                }
            if(!cartaPuesta)
            {
                imgs[(int)posicionActual].Source= imgSender.Source;
                imgs[(int)posicionActual].Tag = imgSender.Tag;
                posicionActual =(PosicionCartas) (((int)posicionActual + 1) % ((int)PosicionCartas.Futuro + 1));
            }
        }

        private void QuitarCartaDeLaTirada(object sender, MouseButtonEventArgs e=null)
        {
            Image imgParaQuitar = sender as Image;
            imgParaQuitar.SetImage(Colors.White.ToBitmap(1, 1));
            imgParaQuitar.Tag = null;
        }

        private void btnInterpretar_Click(object sender, RoutedEventArgs e)
        {
            bool completo=true;
            for (int i = 0; i < imgs.Length; i++)
                completo = imgs[i].Tag != null;
            if(completo)
            {
                new WinVisorTirada(imgPasado.Tag as CartaTarot, imgPresente.Tag as CartaTarot, imgFuturo.Tag as CartaTarot).ShowDialog();
            }else
            {
                MessageBox.Show("Faltan cartas por poner!","Tirada Incompleta",MessageBoxButton.OK,MessageBoxImage.Exclamation);
            }

        }
    }
}
