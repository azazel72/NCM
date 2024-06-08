using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Text.Json;
using System.Security.Permissions;
using Conexiones;
using Google.Protobuf;
using Google.Protobuf.Compiler;
using NoCocinoMas.Objetos;
using Mysqlx;
using static Common.Logging.Configuration.ArgUtils;

namespace NoCocinoMas
{
    class Servidor : ISubscriptorPermanente
    {
        private Conexiones.ServidorPermanente servidor;

        private Gestor gestor;
        private TcpListener servidorWeb;
        private Socket servidorArduino;
        private Thread hiloWeb;
        private Thread hiloArduino;
        private int puertoWeb;
        private int puertoArduino;
        private bool continuar;

        static public readonly object bloqueoAccion = new object();
        /*
         *  
reconexion_wifi: 0
reconexion_servidor: 0
identificador: 8
cantidad: 0
evento: 192.168.137.18
         * 
         * */
        // contructor
        public Servidor(Gestor gestor, int pw = 5001, int pa = 5000)
        {
            this.gestor = gestor;
            this.puertoWeb = pw;
            this.puertoArduino = pa;
            this.gestor.puertoWebTxt.Text = this.puertoWeb.ToString();
            this.gestor.puertoArduinoTxt.Text = this.puertoArduino.ToString();

            Conexiones.ServidorPermanente.IniciarHilo(7000, this);
        }

        /// <summary>
        /// Crea los hilos de los servidores y los inicia
        /// </summary>
        /// <returns></returns>
        public bool Iniciar()
        {
            try
            {
                this.hiloWeb = new Thread(new ThreadStart(IniciarServidorWeb));
                this.hiloArduino = new Thread(new ThreadStart(IniciarServidorArduinos));
                this.continuar = true;
                this.hiloWeb.Start();
                this.hiloArduino.Start();
            }
            catch (Exception e)
            {
                this.gestor.EscribirError("Error (Iniciar): " + e.StackTrace);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Finaliza los Sockets y sus hilos correspondientes.
        /// </summary>
        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        public void Apagar()
        {
            if (this.continuar) {
                try
                {
                    this.continuar = false;
                    this.servidorWeb.Stop();
                    this.servidorArduino.Close();
                    this.hiloWeb.Abort();
                    this.hiloArduino.Abort();
                    this.hiloWeb.Join();
                    this.hiloArduino.Join();
                }
                catch (Exception e)
                {
                    this.gestor.EscribirError("ERROR (ApagarHilo): " + e.Message);
                }
            }
        }

        /// <summary>
        /// Cierra los sockets. La función de control los reiniciará de manera automatica.
        /// </summary>
        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        public void ReiniciarServidores()
        {
            servidorWeb.Stop();
            servidorArduino.Close();
        }

        /// <summary>
        /// Servidor para atender las peticiones http desde los terminales Web
        /// </summary>
        private void IniciarServidorWeb()
        {
            while (this.continuar)
            {
                try
                {
                    IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, this.puertoWeb);
                    this.servidorWeb = new TcpListener(IPAddress.Any, this.puertoWeb);
                    this.servidorWeb.Start(200);
                    while (this.continuar)
                    {
                        string origen = "";
                        try
                        {
                            this.gestor.Leds(true, "web");
                            Socket conexion = this.servidorWeb.AcceptSocket();
                            new Thread(new ThreadStart(() => procesarWeb(conexion))).Start();
                        }
                        catch (Exception e)
                        {
                            this.gestor.Leds(false, "web");
                            this.gestor.EscribirError("Error (Aceptar conexion entrante Web): " + origen + " - " + e.StackTrace);
                            Thread.Sleep(1000);
                            if (!this.servidorWeb.Pending()) break;
                        }
                        this.gestor.Leds(false, "web");
                    }
                }
                catch (Exception e)
                {
                    this.gestor.EscribirError("Error (Crear Servidor Web, Bucle externo)" + e.StackTrace);
                }
                try
                {
                    this.gestor.EscribirError("Cerrando puerto del servidor Web: " + this.puertoArduino);
                    servidorArduino.Close();
                    Thread.Sleep(2000);
                    this.gestor.EscribirError("Puerto del servidor Web cerrado: " + this.puertoArduino);
                }
                catch (Exception e)
                {
                    this.gestor.EscribirError("Error (Cerrando puerto del servidor Web): " + e.StackTrace);
                }
            }
        }

        /// <summary>
        /// Servidor para atender las peticiones tcp desde las centralitas
        /// </summary>
        private void IniciarServidorArduinos()
        {
            while (this.continuar)
            {
                try
                {
                    IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, this.puertoArduino);
                    this.servidorArduino = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    this.servidorArduino.Bind(localEndPoint);
                    this.servidorArduino.Listen(100);
                    
                    while (this.continuar)
                    {
                        try
                        {
                            this.gestor.Leds(true, "plc");
                            Socket conexion = this.servidorArduino.Accept();
                            new Thread(new ThreadStart(() => procesarArduino(conexion))).Start();
                        }
                        catch (Exception e)
                        {
                            this.gestor.Leds(false, "plc");
                            this.gestor.EscribirError("Error (Aceptar conexion entrante Centralitas): " + e.StackTrace);
                            Thread.Sleep(1000);
                            if (!this.servidorArduino.Connected) break;
                        }
                        this.gestor.Leds(false, "plc");
                    }
                }
                catch (Exception e)
                {
                    this.gestor.EscribirError("Error (Crear Servidor Centralitas)" + e.StackTrace);
                }
                try
                {
                    this.gestor.EscribirError("Cerrando puerto del servidor de Centralitas: " + this.puertoArduino);
                    servidorArduino.Close();
                    Thread.Sleep(2000);
                    this.gestor.EscribirError("Puerto del servidor de Centralitas cerrado: " + this.puertoArduino);
                }
                catch (Exception e)
                {
                    this.gestor.EscribirError("Error (Cerrando puerto del servidor de Centralitas): " + e.StackTrace);
                }
            }
        }

        private void procesarWeb(Socket conexion)
        {
            string origen = "";
            try
            {
                origen = ((IPEndPoint)conexion.RemoteEndPoint).Address.ToString();
                byte[] b = new byte[5000];
                conexion.DontFragment = false;
                int length = conexion.Receive(b);
                string data = Encoding.UTF8.GetString(b, 0, length);
                string response = this.responseOK("Peticion no procesada");
                //comprobacion de datos > 0
                if (data.Length > 0)
                {
                    var lista = data.Split(' ');
                    //comprobacion de parametros
                    if (lista.Length > 0)
                    {
                        switch (lista[0].Trim())
                        {
                            case "GET":
                                response = peticionesWeb(lista[1]);
                                break;
                            case "POST":
                                int init_size = data.IndexOf("Content-Length:") + 16;
                                int end_size = data.IndexOf("\n", init_size);
                                int size = Int32.Parse(data.Substring(init_size, end_size - init_size));
                                int size_real = 0;
                                int inicioBody = data.IndexOf("\r\n\r\n");
                                if (inicioBody > 0)
                                {
                                    data = data.Substring(inicioBody).Trim();
                                }
                                else
                                {
                                    data = "";
                                }
                                size_real = Encoding.UTF8.GetBytes(data).Length;
                                while (size_real < size)
                                {
                                    b = new byte[size];
                                    try
                                    {
                                        length = conexion.Receive(b, size, SocketFlags.None);
                                        data += Encoding.UTF8.GetString(b, 0, length);
                                        size_real += length;
                                        //this.gestor.EscribirEvento("Paquete POST de " + length.ToString() + " de un total de " + size.ToString());
                                        if (length == 0)
                                        {
                                            this.gestor.EscribirError("Paquete POST con 0 bytes.");
                                            response = responseOK("Error: Paquete POST con 0 bytes.");
                                            break;
                                        }
                                    }
                                    catch
                                    {
                                        this.gestor.EscribirError("Error: Error en el paquete de datos POST");
                                        response = responseOK("Error: Error en el paquete de datos POST");
                                        break;
                                    }
                                }
                                /*
                                if (size != data.Length)
                                {
                                    this.gestor.EscribirEvento("POST Fallido: " + data + "\nTamaño esperado:" + size.ToString() + " recibido:" + data.Length.ToString());
                                }
                                */
                                response = peticionesWeb(lista[1], data);
                                break;
                            default:
                                this.gestor.EscribirError("Error (procesarWeb, Tipo consulta no reconocida): " + origen + " : " + lista[0]);
                                response = "Tipo de consulta no reconocida.";
                                break;
                        }
                    }
                    else
                    {
                        this.gestor.EscribirError("Error (procesarWeb, Request vacio): " + origen);
                    }
                }
                else
                {
                    this.gestor.EscribirError("Error (procesarWeb, Request nulo): " + origen);
                }
                //Console.WriteLine(response);
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);
                conexion.Send(msg);
                conexion.Close();
            }
            catch (Exception e)
            {
                this.gestor.EscribirError("Error (Hilo Gestion Terminales): " + origen + " - " + e.StackTrace);
            }
        }

        /// <summary>
        /// Procesa las peticiones originarias de un automata (texto plano sin cabecera http)
        /// </summary>
        /// <returns></returns>
        private void procesarArduino(Socket conexion)
        {
            try
            {
                byte[] b = new Byte[1000];
                int length = conexion.Receive(b);
                string m = Encoding.UTF8.GetString(b, 0, length);
                this.gestor.EscribirEvento(m);
                Mensaje mensaje;
                string origen = ((IPEndPoint)conexion.RemoteEndPoint).Address.ToString();
                try
                {
                    mensaje = new Mensaje(m);
                    mensaje.ip = origen;
                    //cliente.println(String("reconexion_wifi: 0\r\n") + "reconexion_servidor: " + String(reconexion_servidor) + "\r\n" + "identificador: c01\r\n" + "cantidad: 0\r\n" + "evento: Conexion controlador de luces, IP" + Ethernet.localIP() + "\r\n");
                }
                catch (Exception e)
                {
                    this.gestor.EscribirError("Error (Crear Servidor Controladores, mensaje no válido): " + origen + " -> " + e.Message);
                    return;
                }
                //Procesar evento
                lock (Servidor.bloqueoAccion)
                {
                    //comprobar si es un modulo o un controlador
                    Modulo modulo = this.gestor.modulos.BuscarAlias(mensaje.identificador);
                    if (modulo != null)
                    {
                        this.gestor.ActualizarIpModulo(modulo, origen);
                    }
                    else
                    {
                        Centralita centralita = this.gestor.centralitas.BuscarAlias(mensaje.identificador);
                        if (centralita != null)
                        {
                            this.gestor.ActualizarIpCentralita(centralita, origen);
                        }
                    }
                    switch (mensaje.evento)
                    {
                        //comando del display
                        case "OK":
                            this.gestor.PulsarOk(mensaje);
                            break;
                        case "mas":
                        case "menos":

                            break;
                        //ip de conexion
                        default:
                            break;
                    }
                }
                conexion.Close();
            }
            catch (Exception e)
            {
                this.gestor.EscribirError("Error (procesarAutomata): " + e.StackTrace);
            }
        }

        /// <summary>
        /// Procesa las peticiones web (cabeceras http)
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        private string peticionesWeb(string datos, string data = "")
        {
            try
            {
                if (!datos.Equals("/ObtenerVersiones"))
                {
                    this.gestor.EscribirEvento(datos);
                }
                //separamos la ruta de los argumentos
                var args = datos.Split('?');
                //guardamos los comandos que vienen en la ruta
                List<string> comandos = new List<string>(args[0].Split('/'));
                //creamos el diccionario con los argumentos
                Dictionary<string, string> argumentos = new Dictionary<string, string>();
                if (args.Length > 1)
                {
                    var pares = args[1].Split('&');
                    foreach (var par in pares)
                    {
                        var p = par.Split('=');
                        argumentos.Add(p[0], (p.Length > 1 ? p[1] : ""));
                    }
                }
                Respuesta r = new Respuesta();
                lock (Servidor.bloqueoAccion)
                {
                    //Comprobamos la ruta y los parametros
                    Parametros parametros;
                    switch (comandos[1])
                    {
                        case "ObtenerDatos":
                            string operarios = JsonSerializer.Serialize<Operarios>(this.gestor.operarios);
                            string roles = JsonSerializer.Serialize<Roles>(this.gestor.roles);
                            string centralitas = JsonSerializer.Serialize<Centralitas>(this.gestor.centralitas);
                            string almacenes = JsonSerializer.Serialize<Almacenes>(this.gestor.almacenes);
                            string modulos = JsonSerializer.Serialize<Modulos>(this.gestor.modulos);
                            string posiciones = JsonSerializer.Serialize<Posiciones>(this.gestor.posiciones);
                            string productos = JsonSerializer.Serialize<Productos>(this.gestor.productos);
                            string envases = JsonSerializer.Serialize<Envases>(this.gestor.envases);
                            string tareas = JsonSerializer.Serialize<Dictionary<string, Tarea>>(this.gestor.tareas.tareas);
                            string versiones = JsonSerializer.Serialize<Dictionary<string, long>>(this.gestor.versiones);
                            string json = "{\"operarios\": " + operarios;
                            json += ", \"roles\": " + roles;
                            json += ", \"centralitas\": " + centralitas;
                            json += ", \"almacenes\": " + almacenes;
                            json += ", \"modulos\": " + modulos;
                            json += ", \"posiciones\": " + posiciones;
                            json += ", \"productos\": " + productos;
                            json += ", \"envases\": " + envases;
                            json += ", \"objeto_tareas\": " + tareas;
                            json += ", \"versiones\": " + versiones;
                            json += "}";
                            return responseOK(json);
                        case "Editar":
                        case "Crear":
                            try
                            {
                                //Console.WriteLine(data);
                                parametros = JsonSerializer.Deserialize<Parametros>(data);
                                r = this.gestor.EditarEntidad(parametros);
                            }
                            catch (Exception e)
                            {
                                r.Error("Error en JSON");
                            }
                            string resultadoEditar = JsonSerializer.Serialize<Respuesta>(r);
                            //Console.WriteLine(resultadoEditar);
                            return responseOK(resultadoEditar);
                        case "Eliminar":
                            if (argumentos.ContainsKey("id") && argumentos.ContainsKey("entidad"))
                            {
                                r = this.gestor.EliminarEntidad(argumentos);
                            }
                            else
                            {
                                r.Error("Parametros ID y Entidad requeridos");
                            }
                            string resultadoEliminar = JsonSerializer.Serialize<Respuesta>(r);
                            return responseOK(resultadoEliminar);
                        case "Tarea":
                            if (argumentos.ContainsKey("id") && argumentos.ContainsKey("almacen_id") && argumentos.ContainsKey("color") && argumentos.ContainsKey("operario_id") && argumentos.ContainsKey("accion"))
                            {
                                r = this.gestor.EditarTarea(argumentos);
                            }
                            else
                            {
                                r.Error("Parametros bloque, operario_id y accion requeridos");
                            }
                            string resultadoTarea = JsonSerializer.Serialize<Respuesta>(r);
                            return responseOK(resultadoTarea);
                        case "CambiarTarea":
                            if (argumentos.ContainsKey("id"))
                            {
                                r = this.gestor.CambiarTarea(argumentos);
                            }
                            else
                            {
                                r.Error("Parametros bloque requerido");
                            }
                            string resultadoCambiarTarea = JsonSerializer.Serialize<Respuesta>(r);
                            return responseOK(resultadoCambiarTarea);
                        case "IniciarPedido":
                            if (argumentos.ContainsKey("tarea_id") && argumentos.ContainsKey("num_pedido"))
                            {
                                r = this.gestor.IniciarPedido(argumentos);
                            }
                            else
                            {
                                r.Error("Parametros bloque, operario_id y accion requeridos");
                            }
                            string resultadoIniciarPedido = JsonSerializer.Serialize<Respuesta>(r);
                            return responseOK(resultadoIniciarPedido);
                        case "IniciarReposicion":
                            if (argumentos.ContainsKey("tarea_id") && argumentos.ContainsKey("producto_codigo") && argumentos.ContainsKey("cantidad") && argumentos.ContainsKey("lote") && argumentos.ContainsKey("forzarPosicion"))
                            {
                                r = this.gestor.IniciarReposicion(argumentos);
                            }
                            else
                            {
                                r.Error("Parametros tarea_id, producto_codigo, cantidad y lote requeridos, y forzarPosicion debe aparecer");
                            }
                            string resultadoIniciarReposicion = JsonSerializer.Serialize<Respuesta>(r);
                            return responseOK(resultadoIniciarReposicion);
                        case "Accion":
                            if (argumentos.ContainsKey("tarea_id") && argumentos.ContainsKey("accion") && argumentos.ContainsKey("cantidad"))
                            {
                                r = this.gestor.SimularAccion(argumentos);
                            }
                            else
                            {
                                r.Error("Parametros tarea_id, accion y cantidad requeridos");
                            }
                            string resultadoAccion = JsonSerializer.Serialize<Respuesta>(r);
                            return responseOK(resultadoAccion);
                        case "ObtenerVersiones":
                            Dictionary<string, long> v = JsonSerializer.Deserialize<Dictionary<string, long>>(data);
                            string res = this.gestor.ObtenerVersiones(v);
                            return responseOK(res);
                        case "TraerTarea":
                            if (argumentos.ContainsKey("pedido"))
                            {
                                r = this.gestor.ObtenerTarea(argumentos);
                            }
                            else
                            {
                                r.Error("Se requiere un numero de pedido válido.");
                            }
                            return responseOK(JsonSerializer.Serialize<Respuesta>(r));
                        case "TraerPedidos":
                            if (argumentos.ContainsKey("incompletos"))
                            {
                                return responseOK(JsonSerializer.Serialize<Pedidos>(this.gestor.pedidos.Incompletos()));
                            }
                            else
                            {
                                return responseOK(JsonSerializer.Serialize<Pedidos>(this.gestor.pedidos));
                            }
                        case "CompletarLinea":
                            if (argumentos.ContainsKey("pedido_numero") && argumentos.ContainsKey("codigo_producto") && argumentos.ContainsKey("lote") && argumentos.ContainsKey("cantidad"))
                            {
                                r = this.gestor.CompletarLinea(argumentos);
                            }
                            else
                            {
                                r.Error("Parametros pedido_numero, codigo_producto y cantidad requeridos");
                            }
                            return responseOK(JsonSerializer.Serialize<Respuesta>(r));
                        case "CompletarListado":
                            try
                            {
                                CompletarPedido lineas = JsonSerializer.Deserialize<CompletarPedido>(data);
                                r = this.gestor.CompletarListado(lineas);
                            }
                            catch (Exception e)
                            {
                                r.Error("Error en JSON");
                            }
                            return responseOK(JsonSerializer.Serialize<Respuesta>(r));
                        case "TraerStock":
                            if (argumentos.ContainsKey("producto") && argumentos.ContainsKey("lote"))
                            {
                                r = this.gestor.ObtenerStock(argumentos);
                            }
                            else
                            {
                                r.Error("Se requiere código de producto válido y opcionalmente lote.");
                            }
                            return responseOK(JsonSerializer.Serialize<Respuesta>(r));
                        case "ExtraerProducto":
                            try
                            {
                                parametros = JsonSerializer.Deserialize<Parametros>(data);
                                r = this.gestor.ExtraerProducto(parametros);
                            }
                            catch (Exception e)
                            {
                                r.Error("Error en JSON");
                            }
                            return responseOK(JsonSerializer.Serialize<Respuesta>(r));
                        case "MoverProducto":
                            try
                            {
                                parametros = JsonSerializer.Deserialize<Parametros>(data);
                                r = this.gestor.MoverProducto(parametros);
                            }
                            catch (Exception e)
                            {
                                r.Error("Error en JSON");
                            }
                            return responseOK(JsonSerializer.Serialize<Respuesta>(r));
                        case "Fabricar":
                            if (argumentos.ContainsKey("codigo_producto") && argumentos.ContainsKey("cantidad"))
                            {
                                r = this.gestor.Fabricar(argumentos);
                            }
                            else
                            {
                                r.Error("Se requiere código de producto válido y opcionalmente lote.");
                            }
                            return responseOK(JsonSerializer.Serialize<Respuesta>(r));
                        case "EncenderPosiciones":
                            if (argumentos.ContainsKey("producto_codigo"))
                            {
                                r = this.gestor.EncenderProducto(argumentos);
                            }
                            else
                            {
                                r.Error("Parametro producto_codigo debe aparecer");
                            }
                            string resultadoEncenderProducto = JsonSerializer.Serialize<Respuesta>(r);
                            return responseOK(resultadoEncenderProducto);
                        case "ApagarPosiciones":
                            r = this.gestor.ApagarProducto();
                            string resultadoApagarProducto = JsonSerializer.Serialize<Respuesta>(r);
                            return responseOK(resultadoApagarProducto);
                        case "ActualizarStock":
                            r = ConectorSQL.ActualizarStock();
                            string resultadoActualizarStock = JsonSerializer.Serialize<Respuesta>(r);
                            return responseOK(resultadoActualizarStock);
                        case "ListadoPedidos":
                            string pedidos = "[]";
                            if (argumentos.ContainsKey("incompletos"))
                            {
                                pedidos = JsonSerializer.Serialize<Pedidos>(this.gestor.pedidos.Incompletos());
                            }
                            else
                            {
                                pedidos = JsonSerializer.Serialize<Pedidos>(this.gestor.pedidos);
                            }
                            string pos = JsonSerializer.Serialize<Posiciones>(this.gestor.posiciones);
                            string jsonListado = "{\"pedidos\": " + pedidos;
                            jsonListado += ", \"posiciones\": " + pos + "}";
                            return responseOK(jsonListado);
                        case "ImportarFabricacion":
                            ListaFabricacion listaFabricacion = JsonSerializer.Deserialize<ListaFabricacion>(data);
                            r = this.gestor.ImportarFabricacion(listaFabricacion);
                            string resultadoImportarFabricacion = JsonSerializer.Serialize<Respuesta>(r);
                            return responseOK(resultadoImportarFabricacion);
                        case "ImprimirEtiqueta":
                            if (argumentos.ContainsKey("pedido_numero"))
                            {
                                r = this.gestor.ImprimirEtiqueta(argumentos["pedido_numero"]);
                            }
                            else
                            {
                                r.Error("Parametro pedido_numero debe aparecer");
                            }
                            string resultadoImprimirEtiqueta = JsonSerializer.Serialize<Respuesta>(r);
                            return responseOK(resultadoImprimirEtiqueta);
                        case "ObtenerPedidos":
                            this.gestor.ObtenerPedidos();
                            return responseOK(JsonSerializer.Serialize<Respuesta>(r));
                        case "RecargarPedido":
                            if (argumentos.ContainsKey("pedido_numero"))
                            {
                                r = this.gestor.RecargarPedido(int.Parse(argumentos["pedido_numero"]));
                            }
                            else
                            {
                                r.Error("Parametro pedido_numero debe aparecer");
                            }
                            string resultadoRecargarPedido = JsonSerializer.Serialize<Respuesta>(r);
                            return responseOK(resultadoRecargarPedido);
                        case "TraerPedido":
                            if (argumentos.ContainsKey("pedido_numero"))
                            {
                                return responseOK(JsonSerializer.Serialize<Pedido>(this.gestor.pedidos.BuscarNumero(int.Parse(argumentos["pedido_numero"]))));
                            }
                            break;
                        case "EncenderLucesProducto":
                            if (argumentos.ContainsKey("codigo_producto") && argumentos.ContainsKey("color_posicion"))
                            {
                                return responseOK(JsonSerializer.Serialize<Producto>(this.gestor.productos.BuscarCodigo(int.Parse(argumentos["codigo_producto"]))));
                            }
                            else
                            {
                                r.Error("Parametros codigo_producto y color_posicion requeridos");
                            }
                            return responseOK(r.Serializar());
                        case "ConsultarPedidosProducto":
                            if (argumentos.ContainsKey("codigo_producto"))
                            {
                                /*
                                 */
                            }
                            else
                            {
                                return responseOK("Parametro codigo_producto debe aparecer");
                            }
                            return responseOK(r.Serializar());
                        case "ApagarLucesProducto":
                            if (argumentos.ContainsKey("codigo_producto"))
                            {
                                
                            }
                            else
                            {
                                r.Error("Parametros codigo_producto y color_posicion requeridos");
                            }
                            return responseOK(r.Serializar());
                        case "BotonConinuar":
                            if (argumentos.ContainsKey("boton"))
                            {
                                MensajeClienteServidor m = new MensajeClienteServidor();
                                m.indice_modulo = int.Parse(argumentos["boton"]);
                                this.gestor.AvanzarPedido(m);
                            }
                            return responseOK();
                        default:
                            return responseOK("Comando no encontrado.");
                    }
                }
            }
            catch (Exception e)
            {
                this.gestor.EscribirError("Error (peticionesWeb): " + datos + "\n + " + data + "\n + " + e.StackTrace);
            }
            return responseOK(datos);
        }

        private string responseOK(string datos = "")
        {
            return string.Format("HTTP/1.1 200 OK\nContent - Type: application/json\nContent - Length: {0}\nConnection: close\nAccess-Control-Allow-Origin: *\nAccess-Control-Allow-Methods: GET,POST,OPTIONS\nAccess-Control-Allow-Headers: Content-Type\nAccess-Control-Allow-Credentials: false\n\n{1}", datos.Length, datos);
        }

        public void NuevaConexion(ConexionPermanente conexion)
        {
            try
            {
                this.gestor.EscribirError(string.Format("Conexion entrante: {0}, {1}", conexion.id, conexion.descripcion));
                this.gestor.conexiones[conexion.id] = conexion;
            }
            catch (Exception e)
            {
                this.gestor.EscribirError("Error (NuevaConexion): " + e.StackTrace);
            }
        }

        public void ConexionCerrada(ConexionPermanente conexion)
        {
            try
            {
                this.gestor.EscribirError(string.Format("Conexion cerrada: {0}, {1}", conexion.id, conexion.descripcion));
                this.gestor.conexiones.Remove(conexion.id);
            }
            catch (Exception e)
            {
                this.gestor.EscribirError("Error (ConexionCerrada): " + e.StackTrace);
            }
        }

        public void MensajeEntrante(ConexionPermanente conexion, string mensaje)
        {
            try {
                this.gestor.EscribirEvento(string.Format("Mensaje de {0}: {1}", conexion.id, mensaje));
                MensajeClienteServidor m = JsonSerializer.Deserialize<MensajeClienteServidor>(mensaje);
                m.id_conexion = conexion.id;
                m.conexion = conexion;
                switch(m.accion)
                {
                    case "Refrescar":
                        this.gestor.Refrescar(m);
                        break;
                    case "NuevoContenedor":
                        this.gestor.AvanzarPedido(m);
                        break;
                    case "TraerPedidos":
                        this.gestor.TraerPedidos(m);
                        break;
                    case "ObtenerTransportistas":
                        this.gestor.ObtenerTransportistas(m);
                        break;
                    case "AvanzarPedido":
                        this.gestor.AvanzarPedido(m);
                        break;
                    case "ObtenerPosiciones":
                        m.datos = this.gestor.productos.BuscarCodigo(m.codigo_producto);
                        _ = m.conexion.Enviar(JsonSerializer.Serialize(m));
                        break;
                }
            }
            catch (Exception e)
            {
                this.gestor.EscribirError("Error (MensajeEntrante): " + e.StackTrace);
            }
        }
    }

}
