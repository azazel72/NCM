using System;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conexiones
{
    public class ServidorPermanente
    {
        //como parametro recibo una interfaz que contiene el callback y la lista de sockets
        public static async Task Iniciar(int puertoServidor = 7000, ISubscriptorPermanente subscriptor = null)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://*:" + puertoServidor.ToString() + "/");
            try
            {
                // Configurar el servidor WebSocket
                listener.Start();
                Console.WriteLine("Servidor iniciado");

                // Escuchar conexiones
                while (true)
                {
                    HttpListenerContext context = await listener.GetContextAsync();
                    if (context.Request.IsWebSocketRequest)
                    {
                        // Aceptar la conexión WebSocket
                        WebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
                        WebSocket socket = webSocketContext.WebSocket;
                        string origen = context.Request.RemoteEndPoint.ToString();
                        ConexionPermanente.NuevaConexion(subscriptor, socket, origen);
                        Console.WriteLine("Conexión WebSocket aceptada. " + origen);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                        context.Response.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void IniciarHilo(int puertoServidor = 7000, ISubscriptorPermanente subscriptor = null)
        {
            new Thread(() => _ = Servidor(puertoServidor, subscriptor)).Start();
        }

        private static async Task Servidor(int puertoServidor = 7000, ISubscriptorPermanente subscriptor = null)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://*:" + puertoServidor.ToString() + "/");
            try
            {
                // Configurar el servidor WebSocket
                listener.Start();
                Console.WriteLine("Servidor iniciado");

                // Escuchar conexiones
                while (true)
                {
                    HttpListenerContext context = await listener.GetContextAsync();
                    if (context.Request.IsWebSocketRequest)
                    {
                        // Aceptar la conexión WebSocket
                        WebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
                        WebSocket socket = webSocketContext.WebSocket;
                        string origen = context.Request.RemoteEndPoint.ToString();
                        ConexionPermanente.NuevaConexion(subscriptor, socket, origen);
                        Console.WriteLine("Conexión WebSocket aceptada. " + origen);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                        context.Response.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
