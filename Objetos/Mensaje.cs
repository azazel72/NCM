using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoCocinoMas
{
    public class Mensaje
    {
        private static string[] separador = new string[] { "\r\n" };

        public int reconexionWifi { get; set; }
        public int reconexionServidor { get; set; }
        public string identificador { get; set; }
        public int cantidad { get; set; }
        public string evento { get; set; }
        public string ip { get; set; }

        public Mensaje(string mensaje)
        {
            string[] pares = mensaje.Split(separador, StringSplitOptions.None);
            Dictionary<string, string> diccionario = new Dictionary<string, string>();
            foreach (string par in pares)
            {
                string[] p = par.Split(':');
                if (p.Length == 2)
                {
                    diccionario.Add(p[0].Trim(), p[1].Trim());
                }
            }
            this.reconexionWifi = Int32.Parse(diccionario["reconexion_wifi"]);
            this.reconexionServidor = Int32.Parse(diccionario["reconexion_servidor"]);
            this.identificador = diccionario["identificador"];
            this.cantidad = Int32.Parse(diccionario["cantidad"]);
            this.evento = diccionario["evento"];
            this.ip = "";
        }
    }

}
