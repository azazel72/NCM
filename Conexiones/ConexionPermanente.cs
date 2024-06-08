using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conexiones
{
    public class ConexionPermanente
    {
        public static void NuevaConexion(ISubscriptorPermanente subscriptor, WebSocket socket, string origen)
        {
            ConexionPermanente nuevaConexion = new ConexionPermanente(subscriptor, socket, origen);
            _ = nuevaConexion.AtenderSocket();
        }

        private ISubscriptorPermanente subscriptor;
        private WebSocket socket;
        public string id { get; set; }
        public string descripcion { get; set; }
        public Dictionary<string, object> atributos { get; set; }
        
        public object Get(string atributo)
        {
            this.atributos.TryGetValue(atributo, out object retorno);
            return retorno;
        }
        public T Get<T>(string atributo)
        {
            this.atributos.TryGetValue(atributo, out object retorno);
            return (T)retorno;
        }
        public void Set(string atributo, object valor)
        {
            this.atributos[atributo] = valor;
        }

        private ConexionPermanente(ISubscriptorPermanente subscriptor, WebSocket socket, string origen)
        {
            this.subscriptor = subscriptor;
            this.socket = socket;
            id = origen;
            descripcion = origen;
            this.atributos = new Dictionary<string, object>();
        }

        private async Task AtenderSocket()
        {
            subscriptor?.NuevaConexion(this);
            //si se usa el token creado aqui, el tiempo transcurre desde el principio de la conexion
            //CancellationToken timeOut = new CancellationTokenSource(5000).Token;

            try
            {
                while (socket.State == WebSocketState.Open)
                {
                    //Hay que crear el token en cada iteracion para resetear el tiempo
                    //CancellationToken timeOut = new CancellationTokenSource(5000).Token;
                    byte[] buffer = new byte[1024];
                    WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None); //CancellationToken.None   timeOut
                    string recibido = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    subscriptor?.MensajeEntrante(this, recibido);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            _ = Desconectar();

            subscriptor?.ConexionCerrada(this);
        }

        public async Task Enviar(string mensaje = "")
        {
            try
            {
                byte[] txt = Encoding.UTF8.GetBytes(mensaje);
                await socket.SendAsync(new ArraySegment<byte>(txt), WebSocketMessageType.Text, true, CancellationToken.None);
                //Console.WriteLine("Enviado " + mensaje);
            }
            catch { }
        }

        public async Task Desconectar()
        {
            try
            {
                socket.Abort();
                socket.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
