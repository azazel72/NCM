using Conexiones;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoCocinoMas
{
    class ConectorPLC
    {
        static private readonly object bloqueoEnviarPlc = new object();

        static private readonly SemaphoreSlim bloqueoEnviarHttp = new SemaphoreSlim(1, 1);

        static public bool notificarLeds = true;
        static public bool notificarWeb = true;

        static private string CrearMensaje(int linea, int posicion, int longitud, string rgb = "127000127", string tipo = "500", int tarea = 4)
        {
            return String.Format("GET /encender/{0}/{1}/{2}/{3}/{4}/{5}\n", linea.ToString(), posicion.ToString().PadLeft(3, '0'), longitud.ToString().PadLeft(3, '0'), rgb, tipo.ToString().PadLeft(2, '0'), tarea);
        }
        static private string CrearUrlSetModulo(string ip, int puerto, int cantidad)
        {
            return String.Format("http://{0}:{1}/set?iniciar={2}", ip, puerto, cantidad);
        }

        static private string CrearUrlTextoModulo(string ip, int puerto, string texto="..")
        {
            return String.Format("http://{0}:{1}/set?editable=0&bloquearOK=0&bloquearMas=1&bloquearMenos=1&subrayado=0&invertido=0&actualizar=0&texto={2}", ip, puerto, texto);
        }

        static public void EncenderRecogida(string ip, int linea, int posicion, int longitud)
        {
            new Thread(new ThreadStart(() => Enviar(ip, Gestor.puertoCentralita, CrearMensaje(linea, posicion, longitud)))).Start();
        }

        static public void EncenderProducto(Producto producto)
        {
            try
            {
                int index = 5;
                foreach (Posicion posicion in producto.posiciones)
                {
                    if (index > 9) return;
                    string ip = posicion.centralita.ip;
                    string m = CrearMensaje(posicion.linea, posicion.led_inicial, posicion.led_longitud, "127000127", "500", index);
                    new Thread(new ThreadStart(() => Enviar(ip, Gestor.puertoCentralita, m))).Start();
                    Productos.posicionesVerProducto.Add(new KeyValuePair<string, int>(ip, index));
                    index++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static public void ApagarProducto()
        {
            try
            {
                for (int index = 0; index < Productos.posicionesVerProducto.Count(); index++) {
                    string ip = Productos.posicionesVerProducto[index].Key;
                    int tarea = Productos.posicionesVerProducto[index].Value;
                    new Thread(new ThreadStart(() => Enviar(ip, Gestor.puertoCentralita, String.Format("GET /Apagar/{0}\n", tarea)))).Start();
                }
                Productos.posicionesVerProducto.Clear();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static public void EncenderPosicion(Tarea tarea)
        {
            //enviar aviso luminoso
            string ip = tarea.posicion.centralita.ip;
            string m = CrearMensaje(tarea.posicion.linea, tarea.posicion.led_inicial, tarea.posicion.led_longitud, tarea.GetRGB(), "500", tarea.getNumero());
            tarea.ipUltimaPosicionLeds = ip;
            //Console.WriteLine(m);
            new Thread(new ThreadStart(() => Enviar(ip, Gestor.puertoCentralita, m))).Start();

            //encender pantalla con cantidades
            string ipModulo = tarea.moduloActual.ip;
            string mM = CrearUrlSetModulo(ipModulo, Gestor.puertoModulos, tarea.cantidadParcial);
            tarea.ipUltimaPosicionDisplay = ipModulo;
            //demora
            string demora = CrearUrlTextoModulo(ipModulo, Gestor.puertoModulos, "--");
            _ = EnviarHttp(mM, demora);
        }

        static public void EncenderEspera(Tarea tarea)
        {
            //enviar aviso luminoso
            string ip = tarea.posicion.centralita.ip;
            Bloqueo b = tarea.gestor.bloqueos.BuscarMarca(tarea.posicion.modulo.alias, tarea.id);
            string m = CrearMensaje(b.linea, b.led_inicial, 2, tarea.GetRGB(), "600", tarea.getNumero());
            tarea.ipUltimaPosicionLeds = ip;
            //Console.WriteLine(m);
            new Thread(new ThreadStart(() => Enviar(ip, Gestor.puertoCentralita, m))).Start();
        }

        static public void EncenderFin(Tarea tarea, bool fin = true)
        {
            string ip = tarea.posicion.centralita.ip;
            Bloqueo b = tarea.gestor.bloqueos.BuscarMarca(tarea.posicion.modulo.alias, tarea.id);
            string m = CrearMensaje(b.primera_linea, b.led_inicial, b.ancho, fin ? "000127000" : "127000000", "520", tarea.getNumero());
            tarea.ipUltimaPosicionLeds = ip;
            new Thread(new ThreadStart(() => Enviar(ip, Gestor.puertoCentralita, m))).Start();

            string ipModulo = tarea.moduloActual.ip;
            string mM = CrearUrlTextoModulo(ipModulo, Gestor.puertoModulos, fin ? "F" : "I");
            tarea.ipUltimaPosicionDisplay = ipModulo;
            _ = EnviarHttp(mM);
        }

        static public void EncenderReposicion(string ip, int linea, int posicion, int longitud)
        {
            new Thread(new ThreadStart(() => Enviar(ip, Gestor.puertoCentralita, CrearMensaje(linea, posicion, longitud)))).Start();
        }

        static public void FinalizarPosicion(Tarea tarea)
        {
            try
            {
                string ip_c = tarea.posicion != null ? tarea.posicion.centralita.ip : tarea.ipUltimaPosicionLeds;
                string ip_m = tarea.posicion != null ? tarea.posicion.modulo.ip : tarea.ipUltimaPosicionDisplay;
                if (ip_c != null && ip_c != "")
                {
                    //notificar luces
                    new Thread(new ThreadStart(() => Enviar(ip_c, Gestor.puertoCentralita, String.Format("GET /Apagar/{0}\n", tarea.getNumero())))).Start();
                }
                if (ip_m != "" && ip_m != null)
                {
                    //notificar a pantalla
                    _ = EnviarHttp(CrearUrlTextoModulo(ip_m, Gestor.puertoModulos));
                }
            }
            catch (Exception e)
            {
                Gestor.gestor.EscribirError("(Finalizar reposicion): " + e.Message);
            }
        }

        static public void Apagar()
        {
            foreach (Centralita c in Gestor.gestor.centralitas)
            {
                new Thread(new ThreadStart(() => Enviar(Gestor.ipControlador, Gestor.puertoCentralita, "GET /Apagar/\n"))).Start();
                new Thread(new ThreadStart(() => Enviar(Gestor.ipControlador2, Gestor.puertoCentralita, "GET /Apagar/\n"))).Start();
            }
        }

        static public void Liberar(Modulo modulo)
        {
            //encender pantalla con cantidades
            string ipModulo = modulo.ip;
            string mM = CrearUrlTextoModulo(ipModulo, Gestor.puertoModulos, "..");
            //new Thread(new ThreadStart(() => EnviarHttp(mM))).Start();
            _ = EnviarHttp(mM);
        }

        static public void Informar(Modulo modulo, Modulo moduloProximo)
        {
            if (moduloProximo != null)
            {
                //encender pantalla con cantidades
                string ipModulo = modulo.ip;
                string mensaje = CrearUrlTextoModulo(ipModulo, Gestor.puertoModulos, "M" + moduloProximo.alias.ToString());
                _ = EnviarHttp(mensaje);
            }
        }

        static private void Enviar(string ip, int puerto, string mensaje)
        {
            if (!notificarLeds)
            {
                return;
            }

            lock (bloqueoEnviarPlc)
            {
                try
                {
                    IPAddress IP = IPAddress.Parse(ip);
                    TcpClient client = new TcpClient();
                    client.Connect(IP, puerto);
                    Stream conexion = client.GetStream();

                    ASCIIEncoding msg = new ASCIIEncoding();
                    byte[] ba = msg.GetBytes(mensaje);

                    conexion.Write(ba, 0, ba.Length);
                    client.Close();
                }
                catch (Exception e)
                {
                    Gestor.gestor.EscribirError("(Enviar "+mensaje+"): " + e.Message);
                }
            }
        }

        static private async Task EnviarHttp(string url, string demora = null)
        {
            if (!notificarLeds)
            {
                return;
            }
            //bloqueo peticion
            await bloqueoEnviarHttp.WaitAsync();

            Gestor.gestor.EscribirError("MODULO: " + url);
            HttpClient cliente = null;
            try
            {
                cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Clear();
                cliente.Timeout = TimeSpan.FromSeconds(5);

                if (demora != null)
                {
                    HttpResponseMessage respuesta_demora = await cliente.GetAsync(demora);
                    respuesta_demora.EnsureSuccessStatusCode();
                    await respuesta_demora.Content.ReadAsStringAsync();
                    await Task.Delay(300);
                }

                //envio de mensaje
                HttpResponseMessage respuesta = await cliente.GetAsync(url);
                respuesta.EnsureSuccessStatusCode();
                string responseBody = await respuesta.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
                cliente.Dispose();

                Gestor.gestor.EscribirError("Liberado: " + url);
            }
            catch (Exception e)
            {
                Gestor.gestor.EscribirError("(ERROR Envio Http: " + url + "): " + e.Message);
                if (cliente != null)
                {
                    cliente.CancelPendingRequests();
                    cliente.Dispose();
                    Gestor.gestor.EscribirError("Liberado con errores");
                }
            }
            bloqueoEnviarHttp.Release();
        }

        static private async Task EnviarSocket(string ip, int puerto, string postData)
        {
            if (!notificarLeds)
            {
                return;
            }
            lock (bloqueoEnviarPlc)
            {
                // Parsear la URL
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    // Conectar al servidor
                    socket.Connect(new IPEndPoint(IPAddress.Parse(ip), puerto));
                    //Gestor.gestor.EscribirEvento("Conectado al controlador");

                    // Crear el mensaje HTTP POST
                    StringBuilder request = new StringBuilder(postData);

                    // Convertir el mensaje a bytes y enviar al servidor
                    byte[] requestBytes = Encoding.UTF8.GetBytes(request.ToString());
                    socket.Send(requestBytes);

                    // Recibir la respuesta del servidor

                    byte[] buffer = new byte[1024];
                    int received = socket.Receive(buffer);
                    string response = Encoding.UTF8.GetString(buffer, 0, received);
                    Gestor.gestor.EscribirEvento("Respuesta del controlador:" + response);

                }
                catch (Exception ex)
                {
                    Gestor.gestor.EscribirError("(ERROR Envio Http: " + ip + ":" + puerto.ToString() + "): " + ex.Message);
                }
                finally
                {
                    // Cerrar el socket
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    //Gestor.gestor.EscribirEvento("Conexión cerrada");
                }
            }
        }

        static private async Task EnviarYRespuesta(string ip, int puerto, string mensaje)
        {
            //Console.Write("LEDS: " + ip + "//" + mensaje);

            lock (bloqueoEnviarPlc)
            {
                try
                {
                    IPAddress IP = IPAddress.Parse(ip);
                    TcpClient client = new TcpClient();
                    client.Connect(IP, puerto);
                    Stream conexion = client.GetStream();

                    ASCIIEncoding msg = new ASCIIEncoding();
                    byte[] ba = msg.GetBytes(mensaje);
                    conexion.Write(ba, 0, ba.Length);

                    byte[] buffer = new byte[1024];
                    int bytesRead = conexion.Read(buffer, 0, buffer.Length);
                    string respuesta = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    Gestor.gestor.EscribirEvento("Recibido: " + mensaje);

                    client.Close();
                }
                catch (Exception e)
                {
                    Gestor.gestor.EscribirError("(Enviar " + mensaje + "): " + e.Message);
                }
            }
        }


        static public void EncenderPedidos(string postData)
        {
            try
            {
                //Console.WriteLine(postData);
                _ = EnviarSocket(Gestor.ipControlador, Gestor.puertoCentralita, postData + "\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static public void EncenderPedidosBack(string postData)
        {
            try
            {
                //Console.WriteLine(postData);
                _ = EnviarSocket(Gestor.ipControlador2, Gestor.puertoCentralita, postData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
