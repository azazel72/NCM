using System.Net.WebSockets;

namespace Conexiones
{
    public class ClientePermanente
    {
        //como parametro recibo una interfaz que contiene el callback y la lista de sockets
        public static async Task Iniciar(string endpoint, string origen, ISubscriptorPermanente? subscriptor = null)
        {
            try
            {
                Uri url = new Uri(endpoint);
                ClientWebSocket client = new ClientWebSocket();
                Console.WriteLine("Conexión WebSocketClient iniciada");
                CancellationToken timeOut = new CancellationTokenSource(5000).Token;
                await client.ConnectAsync(url, timeOut);
                WebSocket c = client;
                Console.WriteLine("Conexión WebSocketClient aceptada");
                ConexionPermanente.NuevaConexion(subscriptor, c, origen);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}