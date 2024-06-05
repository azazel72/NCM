using System.Net.Sockets;
using System.Net;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Web;

namespace Conexiones
{
    public class Servidor
    {
        private TcpListener servidor;
        private int puerto;
        private Thread hilo;
        private ISubscriptor subscriptor;

        public Servidor(int puerto, ISubscriptor subscriptor)
        {
            this.subscriptor = subscriptor;
            this.puerto = puerto;
        }

        public void CrearHilo()
        {
            hilo = new Thread(() => Iniciar());
            hilo.Start();
        }

        public async Task Iniciar()
        {
            this.servidor = new TcpListener(IPAddress.Any, this.puerto);
            this.servidor.Start(100);
            this.subscriptor.LogInformation("Servicio controlador iniciado en el puerto " + this.puerto.ToString());
            while (true)
            {
                TcpClient cliente = servidor.AcceptTcpClient();
                new Thread(() => _ = AtenderConexion(cliente)).Start();
            }
        }

        public async Task AtenderConexion(TcpClient cliente)
        {
            string respuesta = "Comando no procesado.";
            try
            {
                NetworkStream stream = cliente.GetStream();
                byte[] bytes = new byte[cliente.Available];
                if (bytes.Length > 0)
                {
                    stream.Read(bytes, 0, bytes.Length);
                    string data = Encoding.UTF8.GetString(bytes);
                    if (ObtenerDatos(data, out string[] comandos, out Dictionary<string, string> parametros, out string cuerpo))
                    {
                        respuesta = await this.subscriptor.MensajeRecibido(comandos, parametros, cuerpo);
                    }
                }
                respuesta = ResponseOK(respuesta);
                byte[] msg = Encoding.UTF8.GetBytes(respuesta);
                stream.Write(msg, 0, msg.Length);
                stream.Close();
            }
            catch (Exception e)
            {
                this.subscriptor.Excepcion(e);
            }
            finally
            {
                cliente.Close();
            }
        }

        private bool ObtenerDatos(string data, out string[] comandos, out Dictionary<string, string> parametros, out string cuerpo)
        {
            cuerpo = "";
            comandos = new string[0];
            parametros = new Dictionary<string, string>();
            bool resultado = false;

            try
            {
                //dividimos el mensaje en cabecera y cuerpo
                string[] mensaje = data.Split("\r\n\r\n");
                if (mensaje.Length > 0)
                {
                    //dividimos la cabera en lineas
                    string[] cabeceraHTTP = mensaje[0].Split("\r\n");
                    if (cabeceraHTTP.Length > 0)
                    {
                        //dividimos la primera linea
                        string[] peticion = cabeceraHTTP[0].Split(" ");
                        if (peticion.Length > 1)
                        {
                            //la peticion debe ser GET
                            if (peticion.Length > 0 && peticion[0] == "GET")
                            {

                                resultado = true;

                                //separamos la ruta de los parametros
                                string[] url = peticion[1].Split("?");

                                //obtenemos los comandos
                                comandos = url[0].Split("/");
                                this.subscriptor.LogInformation(url[0]);

                                //obtenemos los parametros si los hubiere
                                if (url.Length > 1)
                                {
                                    parametros = ParametrosUri.ExtraerParametros(url[1]);
                                }

                                //guardamos el cuerpo si lo hubiere
                                cuerpo = mensaje.Length > 1 ? mensaje[1] : "";
                            }
                        }
                    }
                }                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return resultado;
        }

        private string ResponseOK(string? datos = "")
        {
            return string.Format("HTTP/1.1 200 OK\nContent - Type: application/json\nContent - Length: {0}\nConnection: close\nAccess-Control-Allow-Origin: *\nAccess-Control-Allow-Methods: GET,POST,OPTIONS\nAccess-Control-Allow-Headers: Content-Type\nAccess-Control-Allow-Credentials: false\n\n{1}", datos.Length, datos);
        }
    }
}
