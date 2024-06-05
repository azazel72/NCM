using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Conexiones
{
    public class ParametrosUri
    {
        private List<KeyValuePair<string, string>> parametros;

        /// <summary>
        /// Inicializa un contenedor de parametros para una peticion GET
        /// </summary>
        public ParametrosUri()
        {
            this.parametros = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Agrega un nuevo parametro a los ya existentes
        /// </summary>
        /// <param name="clave"></param>
        /// <param name="valor"></param>
        public void AgregarParametro(string clave, string valor)
        {
            this.parametros.Add(new KeyValuePair<string, string>(clave, valor));
        }

        /// <summary>
        /// Obtiene la cadena con todos los parametros
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(this.parametros);
            string query = content.ReadAsStringAsync().Result;
            return query;
        }

        /// <summary>
        /// Obtiene la cadena incluyendo la parte inicial de la url
        /// y todos los parametros
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public string ToString(string uri)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(this.parametros);
            string query = uri + "?" + content.ReadAsStringAsync().Result;
            return query;
        }

        /// <summary>
        /// Crea una cadena con un único parametro
        /// </summary>
        /// <param name="clave"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static string CrearParametro(string clave, string valor)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(
                new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>(clave, valor),
                });
            return content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Combina una url con un parametro y lo devuelve
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="clave"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static string FormarUri(string uri, string clave, string valor)
        {
            return uri + "?" + CrearParametro(clave, valor);
        }

        public static Dictionary<string, string> ExtraerParametros(string uri)
        {
            Dictionary<string, string> parametros = new();
            uri = HttpUtility.HtmlDecode(uri);
            
            foreach (string par in uri.Split("&"))
            {
                string[] p = par.Split('=');
                if (p.Length == 2)
                {
                    parametros.Add(p[0], p[1]);
                }
            }

            return parametros;
        }
    }
}
