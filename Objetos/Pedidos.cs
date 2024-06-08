using System.Collections.Generic;
using System.Linq;
using System.Collections;
using MySql.Data.MySqlClient;
using System;
using System.Text.Json.Serialization;
using System.Text;
using static iText.Svg.SvgConstants;

namespace NoCocinoMas
{
    public class Pedidos : Entidades, IEntidades
    {
        /// <summary>
        /// Agregamos el conjunto de objetos de una consulta MySql
        /// </summary>
        /// <param name="resultado"></param>
        public void Agregar(MySqlDataReader resultado)
        {
            while (resultado.Read())
            {
                this.listado.Add(new Pedido(resultado));
            }
        }

        /// <summary>
        /// Vinculamos las lineas de pedido correspondientes a cada pedido.
        /// Tambien vincula el pedido en cada una de las lineas.
        /// </summary>
        /// <param name="lineas"></param>
        public void VincularLineas(LineasPedido lineas)
        {
            if (this.listado.Count == 0 || lineas.listado.Count == 0)
            {
                return;
            }

            Pedido pedido = null;
            foreach (LineaPedido l in lineas)
            {
                if (pedido == null || pedido.numero != l.pedido_numero)
                {
                    pedido = this.BuscarNumero(l.pedido_numero);
                }
                if (pedido != null)
                {
                    l.pedido = pedido;
                    pedido.lineas.Agregar(l);
                }
                else
                {
                    Gestor.gestor.EscribirError("Linea " + l.id.ToString() + " no encuentra pedido " + l.pedido_numero.ToString());
                }
            }
        }

        /// <summary>
        /// Se eliminan las lineas duplicadas.
        /// </summary>
        public void AgruparLineas(LineasPedido listadoLineas)
        {
            //recorremos los pedidos uno a uno
            foreach (Pedido pedido in this.listado)
            {
                pedido.AgruparLineas(listadoLineas);
            }
        }


        public LineasPedido ObtenerLineasPedido(object id)
        {
            Pedido pedido = (Pedido) this.BuscarId(id);
            if (pedido != null)
            {
                return pedido.lineas;
            } else
            {
                return null;
            }
        }

        public Movimientos ObtenerMovimientos(object id)
        {
            Pedido pedido = (Pedido)this.BuscarId(id);
            if (pedido != null)
            {
                pedido.VincularMovimientos();
                return pedido.movimientos;
            }
            else
            {
                return null;
            }
        }        

        /// <summary>
        /// Crea un elemento en la BBDD y lo agrega al listado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public Respuesta InsertSQL(Parametros parametros)
        {
            Respuesta r = new Respuesta();
            int id = parametros.BuscarInt("id");
            if (id > 0) return r.Error("Comando Crear incompatible debe tener ID nulo");
            Pedido a = new Pedido(parametros);
            id = ConectorSQL.CrearEntidades(ConectorSQL.insertPedido, a.GetValoresInsertSQL());
            if (id > 0)
            {
                a.id = id;
                this.Agregar(a);
                r.entidad = a;
            }
            else
            {
                r.error = false;
                r.mensaje = "Insert SQL falló.";
            }
            return r;
        }

        public Pedido BuscarNumero(int numero)
        {
            foreach(Pedido p in this.listado)
            {
                if (p.numero == numero)
                {
                    return p;
                }
            }
            return null;
        }

        public int mayorPedido()
        {
            int i = 0;
            foreach(Pedido p in this.listado)
            {
                if (p.numero > i) {
                    i = p.numero;
                }
            }
            return i;
        }

        public void EliminarRepetidos(Pedidos pedidos, LineasPedido lineas)
        {
            Pedido pedidoBorrar = null;
            LineasPedido lineasBorrar = null;

            foreach (Pedido pedido in this.listado)
            {
                pedidoBorrar = pedidos.BuscarNumero(pedido.numero);
                if (pedidoBorrar != null)
                {
                    pedidos.listado.Remove(pedidoBorrar);
                    lineas.listado.RemoveAll((Entidad e) => {
                        return ((LineaPedido)e).pedido_numero == pedido.numero;
                    });
                    Gestor.gestor.EscribirError("Pedido ya existente, eliminando de la lista. " + pedido.numero.ToString());
                }
            }
        }

        public void GuardarNuevosPedidos()
        {
            ConectorSQL.GuardarEntidades(ConectorSQL.guardarPedidos, this);
            foreach (Pedido pedido in this.listado)
            {
                foreach (LineaPedido linea in pedido.lineas)
                {
                    int id = ConectorSQL.GuardarEntidad(ConectorSQL.guardarLineasPedido, linea);
                    if (id > 0)
                    {
                        linea.id = id;
                    }
                }
            }
        }

        public Pedidos Incompletos()
        {
            Pedidos pedidos = new Pedidos();
            foreach (Pedido pedido in this.listado)
            {
                if (pedido.estado == 0)
                {
                    pedidos.Agregar(pedido);
                }
            }
            return pedidos;
        }

        public string ListadoNumeros()
        {
            return string.Join(",", from Pedido pedido in this.listado select pedido.numero);
            /*
            List<int> numeros = new List<int>();
            foreach (Pedido pedido in this.listado)
            {
                numeros.Add(pedido.numero);
            }
            return string.Join(",", numeros);
            */
        }

        public Pedido AvanzarPedido(int indice_modulo)
        {
            Pedido pedidoAvanzado = (Pedido)this.listado.Find(pedido => ((Pedido)pedido).indice_modulo == indice_modulo);
            pedidoAvanzado?.AvanzarPedido();
            return pedidoAvanzado;
        }
        public Pedido NuevaCaja(int transportista, string caja)
        {
            Pedido pedidoEntrante = (Pedido)this.listado.Find(pedido => ((Pedido)pedido).DisponibleRecogida(transportista));
            pedidoEntrante?.AvanzarPedido(caja);
            return pedidoEntrante;
        }
        
        public List<Pedido> PedidosCompletar()
        {
            return this.listado.ConvertAll(pedido => (Pedido)pedido).FindAll(pedido => pedido.estado == 0 && pedido.indice_modulo == 5);
        }
        public List<Pedido> PedidosTransito()
        {
            return this.listado.ConvertAll(pedido => (Pedido)pedido).FindAll(pedido => pedido.estado == 0 && pedido.indice_modulo > 0 && pedido.indice_modulo < 5 && !string.IsNullOrEmpty(pedido.caja));
        }
        public List<Pedido> PedidosPendientes()
        {
            return this.listado.ConvertAll(pedido => (Pedido)pedido).FindAll(pedido => pedido.estado == 0);
        }

        public Pedido FiltrarPedidoModulo(int indice_modulo)
        {
            return (Pedido)this.listado.Find(pedido => ((Pedido)pedido).indice_modulo == indice_modulo);
        }
    }

    public class Pedido : Entidad, IEntidad
    {
        public int numero { get; set; }
        public DateTime fecha { get; set; }
        public int estado { get; set; }
        public string cp { get; set; }
        public string envio { get; set; }
        public int transportista { get; set; }
        //objetos
        //[JsonIgnore]
        public LineasPedido lineas { get; set; }
        public Movimientos movimientos { get; set; }
        public string caja { get; set; }
        public long indice_recogida { get; set; }
        public int indice_modulo { get; set; }

        private bool movimientos_completados = false;

        public Pedido(Parametros parametros)
        {
            this.id = parametros.BuscarInt("id");
            this.numero = parametros.BuscarInt("numero");
            this.fecha = parametros.BuscarFecha("fecha");
            this.estado = parametros.BuscarInt("estado");
            this.cp = parametros.Buscar("cp");
            this.envio = parametros.Buscar("envio");
            this.transportista = parametros.BuscarInt("transportista");
            this.caja = parametros.Buscar("caja");
            this.indice_modulo = parametros.BuscarInt("indice_modulo");
            this.indice_recogida = parametros.BuscarLong("indice_recogida");
        }
        public Pedido(int id, int numero, int estado, DateTime fecha, string cp, string envio, int transportista, string caja, int indice_modulo, long indice_recogida)
        {
            this.id = id;
            this.numero = numero;
            this.fecha = fecha;
            this.estado = estado;
            this.cp = cp;
            this.envio = envio;
            this.transportista = transportista;
            this.caja = caja;
            this.indice_modulo = indice_modulo;
            this.indice_recogida = indice_recogida;
            this.lineas = new LineasPedido();
            this.movimientos = new Movimientos();
        }
        public Pedido(MySqlDataReader datos)
        {
            this.id = datos.GetInt32("numero");
            this.numero = datos.GetInt32("numero");
            this.fecha = datos.GetDateTime("fecha");
            this.estado = datos.GetInt32("estado");
            this.cp = datos.GetString("cp");
            //this.envio = datos.GetString("envio").Substring(0, 10); //formato dd/mm/aaaa
            this.envio = !datos.IsDBNull(4) ? datos.GetDateTime("envio").ToString("yyyy-MM-dd") : "";
            this.transportista = datos.GetInt32("transportista");
            this.caja = datos.GetString("caja");
            this.indice_modulo = datos.GetInt32("indice_modulo");
            this.indice_recogida = datos.GetInt64("indice_recogida");
            this.lineas = new LineasPedido();
            this.movimientos = new Movimientos();
        }

        /// <summary>
        /// Devuelve un array de objetos con los valores.
        /// </summary>
        /// <returns></returns>
        public object[] GetValores()
        {
            object[] valores = {
                this.id,
                this.numero,
                this.fecha,
                this.estado,
                this.cp,
                this.envio,
                this.transportista,
                this.caja,
                this.indice_modulo,
                this.indice_recogida
            };
            return valores;
        }

        public object[] GetValoresRecogida()
        {
            object[] valores = {
                this.id,
                this.caja,
                this.indice_modulo,
                this.indice_recogida
            };
            return valores;
        }

        /// <summary>
        /// Devuelve un array de objetos con los valores.
        /// Adaptado para llenar las tablas.
        /// </summary>
        /// <returns></returns>
        public object[] GetValoresTablas()
        {
            object[] valores = {
                this.id,
                this.numero,
                this.cp,
                this.envio,
                this.transportista,
                this.estado,
                this.caja,
                this.indice_modulo,
                this.indice_recogida,
            };
            return valores;
        }

        /// <summary>
        /// Devuelve un array de objetos con los valores.
        /// Adaptado para crear consultas SQL
        /// </summary>
        /// <returns></returns>
        public string GetValoresInsertSQL()
        {
            return string.Format("({0}, {1},'{2}',{3},'{4}','{5}',{6},'{7}',{8},{9})",
                this.id,
                this.numero,
                this.fecha.ToString("yyyy-MM-dd HH:mm:ss"),
                this.estado,
                this.cp,
                this.envio,
                this.transportista,
                this.caja,
                this.indice_modulo,
                this.indice_recogida
                );
        }

        /// <summary>
        /// Esta funcion actualiza directamente en la BBDD
        /// Devuelve el resultado.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public Respuesta UpdateSQL(Parametros parametros)
        {
            Respuesta r = Respuesta.Crear(false, 0, (Pedido)this);
            int id = parametros.BuscarInt("id");
            int numero = parametros.BuscarInt("numero");
            //DateTime fecha = parametros.BuscarFecha("fecha");
            int estado = parametros.BuscarInt("estado");
            string cp = parametros.Buscar("cp");
            string envio = parametros.Buscar("envio");
            int transportista = parametros.BuscarInt("transportista");
            string caja = parametros.Buscar("caja");
            int indice_modulo = parametros.BuscarInt("indice_modulo");
            long indice_recogida = parametros.BuscarLong("indice_recogida");
            //Comprobamos la correccion del estado
            if (estado != 2)
            {
                this.ComprobarFinalizacion();
                estado = this.estado;
            }
            object[] valores = {
                this.id,
                this.numero,
                this.fecha.ToString("yyyy-MM-dd HH:mm:ss"),
                this.estado,
                this.cp,
                this.envio,
                this.transportista,
                this.caja,
                this.indice_modulo,
                this.indice_recogida
        };
            if (!this.Comparar(id))
            {
                r.Error("ID no coincidente");
            }
            else
            {
                bool ok = ConectorSQL.ActualizarEntidades(ConectorSQL.updatePedido, valores);
                if (ok)
                {
                    this.numero = numero;
                    this.fecha = fecha;
                    this.estado = estado;
                    this.cp = cp;
                    this.envio = envio;
                    this.transportista = transportista;
                    this.caja = caja;
                    this.indice_modulo = indice_modulo;
                    this.indice_recogida = indice_recogida;
                }
                else
                {
                    r.error = false;
                    r.mensaje = "Actualización SQL falló.";
                }
            }
            return r;
        }

        public Posicion ProximaPosicionRecogidaPedido(Tarea tarea)
        {
            //necesitamos la primera posicion del producto 
            //de cada linea del pedido
            Posiciones posiciones = new Posiciones();
            Console.WriteLine("Busqueda sin bloqueo");
            foreach (LineaPedido linea in this.lineas)
            {
                //Console.WriteLine(linea.producto.nombre);
                if (linea.estado == 0)
                {
                    Posicion p = linea.producto.ProximaPosicionRecogida(tarea);
                    if (p != null)
                    {
                        posiciones.Agregar(p);
                        p.lineaPedidoPendiente = linea;
                        Console.WriteLine(p.nombre);
                    }
                }
            }
            //seleccionamos la posicion mas cercana de todos los productos disponibles
            Posicion proximaPosicion = posiciones.PosicionMasCercana(tarea);

            //SI LA POSICION ES NULA; SE DEVUELVE UNA QUE PUEDA ESTAR BLOQUEADA
            if (proximaPosicion == null)
            {
                posiciones.Vaciar();
                Console.WriteLine("Busqueda con bloqueo");
                foreach (LineaPedido linea in this.lineas)
                {
                    Console.WriteLine(linea.producto.nombre);
                    if (linea.estado == 0)
                    {
                        Posicion p = linea.producto.ProximaPosicionRecogida(tarea, true);
                        posiciones.Agregar(p);
                        if (p != null)
                        {
                            p.lineaPedidoPendiente = linea;
                            Console.WriteLine(p.nombre);
                        }
                    }
                }
                //seleccionamos la posicion mas cercana de todos los productos disponibles
                proximaPosicion = posiciones.PosicionMasCercana(tarea, true);
            }
            return proximaPosicion;
        }

        public bool ComprobarFinalizacion()
        {
            foreach (LineaPedido p in this.lineas)
            {
                if (p.estado == 0)
                {
                    this.estado = 0;
                    return false;
                }
            }
            this.estado = 1;
            return true;
        }

        public string CompletarLinea(int codigo_producto, string lote, int cantidad)
        {
            foreach (LineaPedido lineaPedido in this.lineas)
            {
                if (lineaPedido.producto_codigo == codigo_producto && lineaPedido.ComprobarCantidadRecogida(cantidad))
                {
                    lineaPedido.recogido += cantidad;
                    lineaPedido.estado = lineaPedido.CantidadPendiente() == 0 ? 1 : 0;
                    //guardamos el movimiento
                    Movimiento m = new Movimiento(lineaPedido, lote, cantidad);
                    ConectorSQL.CrearEntidades(ConectorSQL.insertMovimiento, m.GetValoresInsertSQL());
                    this.movimientos.Agregar(m);
                    //guardamos la linea de pedido
                    object[] valoresRecogidaLinea = {
                            lineaPedido.id,
                            lineaPedido.recogido,
                            lineaPedido.estado,
                        };
                    ConectorSQL.ActualizarEntidades(ConectorSQL.updateRecogidaLineas, valoresRecogidaLinea);
                    //guardamos el pedido
                    int est = this.estado;
                    this.ComprobarFinalizacion();
                    if (est != this.estado)
                    {
                        object[] valoresRecogidaPedido = {
                                this.id,
                                this.estado
                            };
                        ConectorSQL.ActualizarEntidades(ConectorSQL.updateRecogidaPedido, valoresRecogidaPedido);

                        ///////////////////////////////////
                        ///////////
                        /// INSERTAR LOS LOTES EN LA BBDD DE PRESTASHOP
                        ///
                        ///////////////////////


                    }
                    //descontamos las unidades del stock no colocado
                    /*
                    object[] valoresFabricacion = {
                        lineaPedido.producto.id,
                        lineaPedido.producto.stock - cantidad
                    };
                    if (ConectorSQL.ActualizarEntidades(ConectorSQL.updateFabricacion, valoresFabricacion))
                    {
                        lineaPedido.producto.stock -= cantidad;
                        Gestor.gestor.ActualizarTablaProductos(lineaPedido.producto);
                    }
                    */
                    return null;
                }
            }
            return "No se encontró ninguna linea que coincida con el codigo de producto o requiera esa cantidad";
        }

        public string CompletarLineas(List<CompletarLinea> listado)
        {
            foreach (CompletarLinea completarLinea in listado)
            {
                //comprobamos la linea
                LineaPedido lineaPedido = (LineaPedido) this.lineas.BuscarId(completarLinea.id);
                if (lineaPedido == null)
                {
                    return "Error, linea de pedido inexistente";
                }
                if (lineaPedido.producto_codigo == completarLinea.codigo && lineaPedido.CantidadPendiente() < completarLinea.completar)
                {
                    return "Error, producto o cantidad erroneo";
                }
                lineaPedido.recogido += completarLinea.completar;
                if (lineaPedido.CantidadPendiente() == 0)
                {
                    lineaPedido.estado = 1;
                }
                //guardamos la linea de pedido
                object[] valoresRecogidaLinea = {
                        lineaPedido.id,
                        lineaPedido.recogido,
                        lineaPedido.estado,
                    };
                ConectorSQL.ActualizarEntidades(ConectorSQL.updateRecogidaLineas, valoresRecogidaLinea);
                //guardamos el movimiento
                Movimiento m = new Movimiento(lineaPedido, completarLinea.lote, completarLinea.completar);
                ConectorSQL.CrearEntidades(ConectorSQL.insertMovimiento, m.GetValoresInsertSQL());
                this.movimientos.Agregar(m);
                //descontamos las unidades del stock no colocado
                object[] valoresFabricacion = {
                    lineaPedido.producto.id,
                    lineaPedido.producto.stock - completarLinea.completar
                };
                if (ConectorSQL.ActualizarEntidades(ConectorSQL.updateFabricacion, valoresFabricacion))
                {
                    lineaPedido.producto.stock -= completarLinea.completar;
                    Gestor.gestor.ActualizarTablaProductos(lineaPedido.producto);
                }
                //marcamos a cero la cantidad de la linea para notificar al terminal de cuales se han completo correctamente
                completarLinea.completar = 0;
            }
            //guardamos el pedido
            if (this.ComprobarFinalizacion())
            {
                object[] valoresRecogidaPedido = {
                            this.id,
                            1
                        };
                ConectorSQL.ActualizarEntidades(ConectorSQL.updateRecogidaPedido, valoresRecogidaPedido);
            }
            return null;
        }

        public void VincularMovimientos() {
            if (!this.movimientos_completados)
            {
                this.movimientos.Vaciar();
                this.movimientos.Agregar(Gestor.gestor.movimientos.Filtrar(this));
                this.movimientos_completados = true;
            }
        }

        public void AgruparLineas(LineasPedido listadoLineas)
        {
            LineasPedido lineasAgrupadas = new LineasPedido();
            //comprobamos todas las lineas
            foreach (LineaPedido linea in this.lineas)
            {
                //Si el producto no se encuentra, se agrega la linea a las definitivas
                LineaPedido lineaDefinitiva = lineasAgrupadas.PrimeraConProducto(linea.producto_codigo);
                if (lineaDefinitiva == null)
                {
                    lineasAgrupadas.Agregar(linea);
                }
                //Si el producto ya se encuentra se agrega la cantidad
                else
                {
                    lineaDefinitiva.cantidad += linea.cantidad;
                    //opcionalmente, eliminamos las lineas del listado global
                    listadoLineas.Eliminar(linea);
                }
            }
            this.lineas = lineasAgrupadas;
        }

        public void AvanzarPedido(string caja = null)
        {
            int indiceActual = this.indice_modulo;
            string cajaActual = this.caja;
            this.indice_modulo = indiceActual + 1;
            if (!string.IsNullOrEmpty(caja) && caja != this.caja)
            {
                this.caja = caja;
                this.indice_recogida = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds();
            }
            if (indiceActual != this.indice_modulo || cajaActual != this.caja)
            {
                //actualizamos BBDD
                ConectorSQL.ActualizarEntidades(ConectorSQL.updateRecogida, this.GetValoresRecogida());
            }
        }

        public bool DisponibleRecogida(int transportista)
        {
            return this.estado == 0 && this.transportista == transportista && this.indice_modulo == 0 && string.IsNullOrEmpty(this.caja);
        }

        public List<Ubicacion> ObtenerUbicacionesModulo(int indice = -1)
        {
            if (indice == -1) indice = this.indice_modulo;
            List<Ubicacion> ubicaciones = new List<Ubicacion>();
            if (indice > 0)
            {
                string letra = indice == 1 ? "A" : indice == 2 ? "B" : indice == 3 ? "C" : "D";
                foreach (LineaPedido linea in this.lineas)
                {
                    if (linea.producto.posicionRecogida.StartsWith(letra))
                    {
                        linea.producto.ubicacionRecogida.ancho = linea.cantidad;
                        ubicaciones.Add(linea.producto.ubicacionRecogida);
                    }
                }
            }
            return ubicaciones;
        }

    }
}
