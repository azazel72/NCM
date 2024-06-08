using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Conexiones
{
    public class Cliente<T>
    {
        public string url { get; set; }
        public HttpContent cuerpo { get; set; }
        public string usuario { get; set; }
        public string clave { get; set; }
        public T respuesta { get; set; }
        public bool error { get; set; }
        public Exception excepcion { get; set; }
        public string respuestaOriginal { get; set; }
        public HttpClient cliente { get; set; }

        public Cliente(string url, string usuario = "", string clave = "")
        {
            this.url = url;
            this.usuario = usuario;
            this.clave = clave;
        }

        private void CrearCliente()
        {
            this.cliente = new HttpClient();

            this.cliente.DefaultRequestHeaders.Accept.Clear();
            this.cliente.DefaultRequestHeaders.ConnectionClose = true;
            this.cliente.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
                );
            //agregamos la autorizacion si existe
            if (this.usuario != null && this.usuario != "")
            {
                this.cliente.DefaultRequestHeaders.Authorization =
                    new BasicAuthenticationHeaderValue(this.usuario, this.clave);
            }
        }

        /// <summary>
        /// Creamos una conexion de tipo POST
        /// y se devuelve el objeto esperado
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="cuerpo"></param>
        /// <returns></returns>
        public async Task ConectarPost(string cuerpo)
        {
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;

            try
            {
                CrearCliente();
                StringContent queryString = cuerpo != null ? new StringContent(cuerpo) : null;
                HttpResponseMessage response = await this.cliente.PostAsync(url, queryString);

                string buffer = await response.Content.ReadAsStringAsync();

                byte[] bytes = await response.Content.ReadAsByteArrayAsync();
                byte[] isoBytes = Encoding.Convert(utf8, iso, bytes);
                this.respuestaOriginal = iso.GetString(isoBytes);
                this.respuesta = JsonConvert.DeserializeObject<T>(this.respuestaOriginal);
            }
            catch (Exception e)
            {
                this.error = true;
                this.excepcion = e;
            }
            finally
            {
                this.cliente?.Dispose();
            }
        }

        /// <summary>
        /// Creamos una conexion de tipo POST
        /// y se devuelve el objeto esperado
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="cuerpo"></param>
        /// <returns></returns>
        public async Task ConectarGet()
        {
            try
            {
                CrearCliente();
                HttpResponseMessage response = await this.cliente.GetAsync(url);

                this.respuestaOriginal = await response.Content.ReadAsStringAsync();
                this.respuesta = JsonConvert.DeserializeObject<T>(this.respuestaOriginal);
            }
            catch (Exception e)
            {
                this.error = true;
                this.excepcion = e;
            }
            finally
            {
                this.cliente?.Dispose();
            }
        }
    }
}