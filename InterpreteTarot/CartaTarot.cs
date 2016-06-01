using Gabriel.Cat.Binaris;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Gabriel.Cat.Extension;
namespace InterpreteTarot
{
   public class CartaTarot:IElementoBinario,IComparable
    {
        public enum TipusCarta
        {
            ArcanoMayor, Oro, Copa, Espada
        }
        static string[] numerosFiguras = "As;Dos;Tres;Cuatro;Cinco;Seis;Siete;Ocho;Nueve;Diez;Sota;Caballo;Reina;Rey".Split(';');
        public static string ExtensionCarta = ".CartaTarot";
        public static readonly string pathCartasCarpetaGuardado= Environment.CurrentDirectory + Path.AltDirectorySeparatorChar + "Cartas";
        public static string pathCartasCargar =pathCartasCarpetaGuardado;
        string nombre;
        Bitmap imagen;
        string significado;
        string presente;
        string futuro;
        string pasado;
        string siNo;
        Bitmap[] signos;
        DateTime fechaInicio;
        DateTime fechaFin;
        string estacion;
        string palabrasClave;
        Formato formato;
        

        public CartaTarot()
        {
            signos = new Bitmap[3];
            formato = new Formato();
            formato.ElementosArchivo.Afegir(ElementoBinario.ElementosTipoAceptado(Gabriel.Cat.Serializar.TiposAceptados.String));
            formato.ElementosArchivo.Afegir(ElementoBinario.ElementosTipoAceptado(Gabriel.Cat.Serializar.TiposAceptados.Bitmap));
            formato.ElementosArchivo.Afegir(ElementoBinario.ElementosTipoAceptado(Gabriel.Cat.Serializar.TiposAceptados.String));
            formato.ElementosArchivo.Afegir(ElementoBinario.ElementosTipoAceptado(Gabriel.Cat.Serializar.TiposAceptados.String));
            formato.ElementosArchivo.Afegir(ElementoBinario.ElementosTipoAceptado(Gabriel.Cat.Serializar.TiposAceptados.String));
            formato.ElementosArchivo.Afegir(ElementoBinario.ElementosTipoAceptado(Gabriel.Cat.Serializar.TiposAceptados.String));
            formato.ElementosArchivo.Afegir(ElementoBinario.ElementosTipoAceptado(Gabriel.Cat.Serializar.TiposAceptados.String));
            formato.ElementosArchivo.Afegir(ElementoBinario.ElementosTipoAceptado(Gabriel.Cat.Serializar.TiposAceptados.String));
            formato.ElementosArchivo.Afegir(ElementoBinario.ElementosTipoAceptado(Gabriel.Cat.Serializar.TiposAceptados.String));
            formato.ElementosArchivo.Afegir(ElementoBinario.ElementosTipoAceptado(Gabriel.Cat.Serializar.TiposAceptados.DateTime));
            formato.ElementosArchivo.Afegir(ElementoBinario.ElementosTipoAceptado(Gabriel.Cat.Serializar.TiposAceptados.DateTime));
            formato.ElementosArchivo.Afegir(new ElementoIEnumerableBinario(ElementoBinario.ElementosTipoAceptado(Gabriel.Cat.Serializar.TiposAceptados.Bitmap), (byte)3));


        }
        public TipusCarta Tipo
        {
            get
            {
                string[] tipos;
                int posicion = 0;
                tipos = Enum.GetNames(typeof(TipusCarta));
                while (posicion < tipos.Length && !Nombre.Contains(tipos[posicion])) posicion++;
                if (posicion < tipos.Length)
                    return (TipusCarta)Enum.Parse(typeof(TipusCarta), tipos[posicion]);
                else
                    return TipusCarta.ArcanoMayor;
            }
        }
        public string Nombre
        {
            get
            {
                return nombre;
            }

            set
            {
                nombre = value;
            }
        }

        public Bitmap Imagen
        {
            get
            {
                return imagen;
            }

            set
            {
                imagen = value;
            }
        }

        public string Significado
        {
            get
            {
                return significado;
            }

            set
            {
                significado = value;
            }
        }

        public string Presente
        {
            get
            {
                return presente;
            }

            set
            {
                presente = value;
            }
        }

        public string Futuro
        {
            get
            {
                return futuro;
            }

            set
            {
                futuro = value;
            }
        }

        public string Pasado
        {
            get
            {
                return pasado;
            }

            set
            {
                pasado = value;
            }
        }

        public string SiNo
        {
            get
            {
                return siNo;
            }

            set
            {
                siNo = value;
            }
        }

        public Bitmap Signo1
        {
            get
            {
                return signos[0];
            }

            set
            {
                signos[0] = value;
            }
        }
        public Bitmap Signo2
        {
            get
            {
                return signos[1];
            }

            set
            {
                signos[1] = value;
            }
        }
        public Bitmap Signo3
        {
            get
            {
                return signos[2];
            }

            set
            {
                signos[2] = value;
            }
        }

        public DateTime FechaInicio
        {
            get
            {
                return fechaInicio;
            }

            set
            {
                fechaInicio = value;
            }
        }

        public DateTime FechaFin
        {
            get
            {
                return fechaFin;
            }

            set
            {
                fechaFin = value;
            }
        }

        public string Estacion
        {
            get
            {
                return estacion;
            }

            set
            {
                estacion = value;
            }
        }

        public string PalabrasClave
        {
            get
            {
                return palabrasClave;
            }

            set
            {
                palabrasClave = value;
            }

        }
            public byte[] GetBytes()
        {
            object[] objs = { Nombre, Imagen, Presente, Futuro, Pasado, SiNo, PalabrasClave, Significado, Estacion, FechaInicio, FechaFin, signos };
            return formato.GetBytes(objs);
        }

        public void SetBytes(Stream bytesFile)
        {
          SetObjects(formato.GetObjects(bytesFile));
        }

        public void SetObjects(object[] campos)
        {
            Nombre = (string)campos[0];
            Imagen = (Bitmap)campos[1];
            Presente = (string)campos[2];
            Futuro = (string)campos[3];
            Pasado = (string)campos[4];
            SiNo = (string)campos[5];
            PalabrasClave = (string)campos[6];
            Significado = (string)campos[7];
            Estacion = (string)campos[8];
            FechaInicio = (DateTime)campos[9];
            FechaFin = (DateTime)campos[10];
            signos=((object[]) campos[11]).Casting<Bitmap>().ToArray();
        }

        public int CompareTo(object obj)
        {
            CartaTarot carta = obj as CartaTarot;
            if (carta != null)
            {
                if (carta.Tipo == Tipo)
                {
                    if (Tipo.Equals(CartaTarot.TipusCarta.ArcanoMayor))
                        return carta.Nombre.CompareTo(Nombre);
                    else
                        return numerosFiguras.IndicePrimeroContenido(Nombre).CompareTo(numerosFiguras.IndicePrimeroContenido(carta.Nombre));
                }
                else
                    return Tipo.CompareTo(carta.Tipo);
            }
            else
                return -1;
        }
        public static void GuardarCarta(CartaTarot carta)
        {
            if (!Directory.Exists(pathCartasCarpetaGuardado))
                Directory.CreateDirectory(pathCartasCarpetaGuardado);
            carta.GetBytes().Save(pathCartasCarpetaGuardado + Path.DirectorySeparatorChar + carta.Nombre + ExtensionCarta);
        }
        public static CartaTarot[] Cargar()
        {
            List<CartaTarot> cartas = new List<CartaTarot>();
            try
            {
                foreach (FileInfo file in new DirectoryInfo(pathCartasCargar).GetFiles())
                    try
                    {
                        cartas.Add(CargarCarta(file.FullName));
                    }
                    catch { }
            }
            catch (Exception m){

            }
            return cartas.Filtra((carta)=>carta!=null).ToArray();
        }
        public static CartaTarot CargarCarta(string fullName)
        {
            CartaTarot carta = new CartaTarot();
            Stream stream = new FileInfo(fullName).OpenRead();
            try
            {
                carta.SetBytes(stream);
            }
            finally
            {
                stream.Close();
            }
            return carta;
        }


    }
}
