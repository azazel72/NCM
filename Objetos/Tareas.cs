using System.Collections.Generic;
using System.Text.Json.Serialization;
using System;

namespace NoCocinoMas
{
    public class Tareas
    {
        public Dictionary<string, Tarea> tareas;
        static public readonly object bloqueoBusquedaPosicion = new object();
        private Gestor gestor;

        public Tareas(Gestor gestor)
        {
            tareas = new Dictionary<string, Tarea>();
            this.gestor = gestor;
            this.CrearTareas();
        }

        public void CrearTareas()
        {
            tareas.Add("a1b0", new Tarea("a1b0", gestor));
            tareas.Add("a1b1", new Tarea("a1b1", gestor));
            tareas.Add("a1b2", new Tarea("a1b2", gestor));
            tareas.Add("a1b3", new Tarea("a1b3", gestor));
        }

        public Tarea BuscarId(string id)
        {
            this.tareas.TryGetValue(id, out Tarea tarea);
            return tarea;
        }

        public Tarea BuscarOperario(int id)
        {
            foreach (Tarea t in this.tareas.Values)
            {
                if (t.operario != null && t.operario.Comparar(id))
                {
                    return t;
                }
            }
            return null;
        }
        public Tarea BuscarPedido(int numero)
        {
            foreach (Tarea t in this.tareas.Values)
            {
                if (t.pedido != null && t.pedido.numero == numero)
                {
                    return t;
                }
            }
            return null;
        }

        public void ModuloLiberado(Modulo modulo)
        {
            foreach (Tarea t in this.tareas.Values)
            {
                if (t.moduloActual == modulo)
                {
                    // SI es para recoger necesariamente debemos volver a comprobar
                    // que sigue habiendo la cantidad deseada en la posicion indicada

                    t.AccionRecoger();
                    /*

                    //vinculamos el modulo con la tarea a la espera
                    modulo.VincularTarea(t);
                    //envio de la señal de leds
                    ConectorPLC.EncenderPosicion(t);

                    */
                    return;
                }
            }
        }
    }

    public class Tarea
    {
        public string id { get; set; }
        public int accion { get; set; }
        public Almacen almacen { get; set; }
        public Operario operario { get; set; }
        public Pedido pedido { get; set; }
        public LineaPedido lineaPedidoActual { get; set; }
        public Producto producto { get; set; }
        public string lote { get; set; }
        public int cantidad { get; set; }
        //proximo modulo bloqueado
        public Posicion posicion { get; set; }
        public int estado { get; set; }
        public long version { get; set; }
        public Modulo moduloActual { get; set; }
        //cantidad a reponer o a recoger (mostrado en pantalla)
        public int cantidadParcial { get; set; }
        //cantidad que falta por completar, tanto para soltar como para recoger
        public int cantidadPendiente { get; set; }
        [JsonIgnore]
        public string ipUltimaPosicionLeds { get; set; }
        [JsonIgnore]
        public string ipUltimaPosicionDisplay { get; set; }
        [JsonIgnore]
        public Gestor gestor { get; set; }

        public Tarea(string id, Gestor gestor)
        {
            this.id = id;
            this.almacen = null;
            this.operario = null;
            this.pedido = null;
            this.lineaPedidoActual = null;
            this.producto = null;
            this.lote = "";
            this.cantidad = 0;
            this.moduloActual = null;
            this.posicion = null;
            this.cantidadParcial = 0;
            this.cantidadPendiente = 0;
            this.estado = 0;
            this.version = DateTime.Now.Ticks;
            this.gestor = gestor;
        }

        public void setModulo(Modulo m = null)
        {
            //if (this.moduloAbandonado)
        }

        /// <summary>
        /// Inicia la tarea asignando un operario, un almacen y un tipo de tarea
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="a"></param>
        /// <param name="o"></param>
        public void Iniciar(int accion, Almacen a, Operario o)
        {
            this.accion = accion;
            this.almacen = a;
            this.operario = o;
            this.pedido = null;
            this.lineaPedidoActual = null;
            this.producto = null;
            this.lote = "";
            this.cantidad = 0;
            this.moduloActual = null;
            this.posicion = null;
            this.cantidadParcial = 0;
            this.cantidadPendiente = 0;
            this.estado = 1;
            this.version = DateTime.Now.Ticks;
        }

        public void IniciarRecogida(Pedido p)
        {
            this.pedido = p;
            this.producto = null;
            this.lote = "";
            this.cantidad = 0;
            this.cantidadParcial = 0;
            this.cantidadPendiente = 0;
            this.AccionRecoger();
            this.version = DateTime.Now.Ticks;
        }

        /// <summary>
        /// Preparamos la tarea para iniciar la recogida, y activamos la primera posicion
        /// </summary>
        /// <param name="p"></param>
        /// <param name="lote"></param>
        /// <param name="cantidad"></param>
        /// <param name="posicionForzada"></param>
        public void IniciarReposicion(Producto p, string lote, int cantidad, Posicion posicionForzada=null)
        {
            //comprobamos si hay huecos vacios para reponer
            this.pedido = null;
            this.producto = p;
            this.lote = lote;
            this.cantidad = cantidad;
            this.cantidadParcial = 0;
            //la pendiente de momento es el total indicado
            this.cantidadPendiente = cantidad;
            //solicitamos la primera posicion
            this.AccionReponer(posicionForzada);
            this.version = DateTime.Now.Ticks;
        }

        /// <summary>
        /// Finaliza una accion de reposicion/recogida y prepara la siguiente
        /// Similar al estado tras Iniciar
        /// </summary>
        public void CancelarReposicion()
        {
            ConectorPLC.FinalizarPosicion(this);
            this.producto = null;
            this.lote = "";
            this.cantidad = 0;
            if (this.moduloActual != null)
            {
                this.moduloActual.DesvincularTarea(this);
            }
            this.posicion = null;
            this.cantidadParcial = 0;
            this.cantidadPendiente = 0;
            this.version = DateTime.Now.Ticks;
        }

        public void Parar()
        {
            lock (Tareas.bloqueoBusquedaPosicion)
            {
                //primero enviamos el apagado para disponer de la posicion
                ConectorPLC.FinalizarPosicion(this);
                this.accion = 0;
                this.almacen = null;
                this.operario = null;
                this.pedido = null;
                this.producto = null;
                this.lote = "";
                this.cantidad = 0;
                if (this.moduloActual != null)
                {
                    this.moduloActual.DesvincularTarea(this);
                }
                this.posicion = null;
                this.cantidadParcial = 0;
                this.cantidadPendiente = 0;
                this.estado = 0;
                this.version = DateTime.Now.Ticks;
            }
        }

        public void Cambiar()
        {
            lock (Tareas.bloqueoBusquedaPosicion)
            {
                //primero enviamos el apagado para disponer de la posicion
                ConectorPLC.FinalizarPosicion(this);
                this.accion = this.accion == 1 ? 2 : 1;
                this.operario.accion = this.accion;
                this.pedido = null;
                this.lineaPedidoActual = null;
                this.producto = null;
                this.lote = "";
                this.cantidad = 0;
                if (this.moduloActual != null)
                {
                    this.moduloActual.DesvincularTarea(this);
                }
                this.posicion = null;
                this.cantidadParcial = 0;
                this.cantidadPendiente = 0;
                this.version = DateTime.Now.Ticks;
            }
        }

        public void AccionReponer(Posicion pos = null)
        {
            //si no tenemos posicion, buscamos una
            lock (Tareas.bloqueoBusquedaPosicion)
            {
                if (this.cantidadPendiente < 1)
                {
                    this.CancelarReposicion();
                    return;
                }
                //buscamos una posicion acorde
                if (pos == null)
                {
                    pos = this.BuscarPosicionReposicion(this.producto, this.lote);
                }
                //comprobamos que la posicion entregada no este bloqueada
                if (pos == null || pos.modulo.EstaBloqueado(this))
                {
                    CancelarReposicion();
                    return;
                }
                this.posicion = pos;

                int disponible = pos.Disponible();
                if (this.cantidadPendiente > disponible)
                {
                    this.cantidadParcial = disponible;
                    this.cantidadPendiente -= disponible;
                }
                else
                {
                    this.cantidadParcial = this.cantidadPendiente;
                    this.cantidadPendiente = 0;
                }

                //vincula la tarea para bloquear el modulo
                if (!pos.modulo.VincularTarea(this))
                {
                    Console.WriteLine("REPOSICION PENDIENTE DE UN MODULO");
                    ConectorPLC.EncenderEspera(this);
                }
                else
                {
                    ConectorPLC.EncenderPosicion(this);
                }
            }
            this.version = DateTime.Now.Ticks;
        }

        private Posicion BuscarPosicionReposicion(Producto producto, string lote)
        {
            //buscamos un modulo con el mismo producto y lote.
            //Posicion posicion = producto.FiltrarPosicionesPorLotes(lote).NoCompletas().NoBloqueadas(this).MasCercano(this);
            Posicion posicion = producto.ProximaPosicionReposicion(this);
            if (posicion == null) 
            {
                //si no hay similar, se busca un hueco vacio.
                //posicion = this.almacen.BuscarVacias().BuscarEnvase(producto).NoBloqueadas(this).MasCercano(this);
                posicion = this.almacen.BuscarVacias().NoBloqueadas(this).MasCercano(this);
            }
            return posicion;
        }

        public string GetColor()
        {
            try
            {
                return this.id.Substring(3);
            }
            catch
            {
                return "0";
            }
        }

        public string GetRGB()
        {
            string rgb = "127000127";
            try
            {
                switch(this.id.Substring(3))
                {
                    case "0":
                        rgb = "127032000";
                        break;
                    case "1":
                        rgb = "000127000";
                        break;
                    case "2":
                        rgb = "127000000";
                        break;
                    case "3":
                        rgb = "000000127";
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                return "0";
            }
            return rgb;
        }

        public int getNumero()
        {
            return int.Parse(this.id.Substring(3));
        }

        public void PulsarOk(int cantidadIndicada)
        {
            if (cantidad <= 0 || (cantidadIndicada > (this.cantidadPendiente + this.cantidadParcial)))
            {
                return;
            }
            //actualizamos la cantidad recogida con respecto a la esperada
            int diferencia = this.cantidadParcial - cantidadIndicada;
            this.cantidadPendiente += diferencia;
            this.cantidadParcial = cantidadIndicada;
            //preparar el movimiento
            Movimiento m = new Movimiento(this);
            ConectorSQL.CrearEntidades(ConectorSQL.insertMovimiento, m.GetValoresInsertSQL());
                
            switch (this.accion)
            {
                case 1: //RECOGER
                    if (this.pedido == null || this.lineaPedidoActual == null)
                    {
                        return;
                    }
                    //actualizamos la posicion con la cantidad restada
                    //...en la recogida no se verifica que el producto no sea nulo en la posicion...
                    this.posicion.ActualizarProducto(this.producto, this.lote, -(this.cantidadParcial));
                    //agregamos el movimiento al pedido
                    this.pedido.movimientos.Agregar(m);
                    //actualizamos la linea del pedido y el pedido
                    this.lineaPedidoActual.recogido = this.lineaPedidoActual.cantidad - this.cantidadPendiente;
                    if (this.cantidadPendiente == 0)
                    {
                        this.lineaPedidoActual.estado = 1;
                    }
                    object[] valoresRecogidaLinea = {
                        this.lineaPedidoActual.id,
                        this.lineaPedidoActual.recogido,
                        this.lineaPedidoActual.estado,
                    };
                    if (ConectorSQL.ActualizarEntidades(ConectorSQL.updateRecogidaLineas, valoresRecogidaLinea))
                    {
                        //actualizamos el pedido si ha cambiado de estado
                        if (this.pedido.ComprobarFinalizacion())
                        {
                            object[] valoresRecogidaPedido = {
                                this.pedido.id,
                                1
                            };
                            ConectorSQL.ActualizarEntidades(ConectorSQL.updateRecogidaPedido, valoresRecogidaPedido);
                        }
                        object[] valoresRecogida = {
                            this.posicion.id,
                            this.posicion.producto_codigo,
                            this.posicion.cantidad,
                            this.posicion.lote
                        };
                        if (ConectorSQL.ActualizarEntidades(ConectorSQL.updateReposicion, valoresRecogida))
                        {
                            gestor.ActualizarTablaPosiciones(this.posicion);
                            this.AccionRecoger();
                        }
                    }
                    break;
                case 2: //REPONER
                    //actualizar posicion con producto repuesto
                    this.posicion.ActualizarProducto(this.producto, this.lote, this.cantidadParcial);
                    //actualizar sql
                    object[] valoresReposicion = {
                        this.posicion.id,
                        this.producto.codigo,
                        this.posicion.cantidad,
                        this.lote
                    };
                    if (ConectorSQL.ActualizarEntidades(ConectorSQL.updateReposicion, valoresReposicion))
                    {
                        //se descuenta del stock
                        object[] valoresFabricacion = {
                            this.producto.id,
                            this.producto.stock - this.cantidadParcial
                        };
                        if (ConectorSQL.ActualizarEntidades(ConectorSQL.updateFabricacion, valoresFabricacion))
                        {
                            this.producto.stock -= this.cantidadParcial;
                            gestor.ActualizarTablaProductos(producto);
                        }
                        gestor.ActualizarTablaPosiciones(this.posicion);
                        this.AccionReponer();
                    }
                break;
            }
            this.version = DateTime.Now.Ticks;
        }

        /// <summary>
        /// Encontramos la posicion mas cercana para recoger productos
        /// </summary>
        public void AccionRecoger()
        {
            lock (Tareas.bloqueoBusquedaPosicion)
            {

                if (this.pedido == null || this.pedido.lineas.Contador() == 0)
                {
                    this.CancelarRecogida();
                    return;
                }
                if (this.pedido.ComprobarFinalizacion())
                {
                    this.FinalizarRecogida(true);
                    return;
                }
                //seleccionamos la posicion mas cercana de todos los productos
                //disponibles, primero desbloqueada y si no existe, de un modulo bloqueado
                Posicion proximaPosicion = this.pedido.ProximaPosicionRecogidaPedido(this);

                //comprobamos que la posicion entregada no este bloqueada
                if (proximaPosicion == null)
                {
                    if (this.pedido.ComprobarFinalizacion())
                    {
                        Console.WriteLine("PEDIDO TERMINADO");
                        this.FinalizarRecogida(true);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("PEDIDO INCOMPLETO");
                        this.FinalizarRecogida(false);
                        return;
                    }
                }
                Console.WriteLine("Seleccionada");
                Console.WriteLine(proximaPosicion.producto.nombre);
                Console.WriteLine(proximaPosicion.nombre);

                this.producto = proximaPosicion.producto;
                this.lote = proximaPosicion.lote;
                this.posicion = proximaPosicion;
                //poner la linea de pedido pendiente detras de la vinculacion, o asociar la posicion a
                //la linea de pedido, o a la propia tarea
                this.lineaPedidoActual = proximaPosicion.lineaPedidoPendiente;
                this.cantidad = proximaPosicion.lineaPedidoPendiente.cantidad;
                int cantidadRequerida = proximaPosicion.lineaPedidoPendiente.CantidadPendiente();
                if (cantidadRequerida > proximaPosicion.cantidad)
                {
                    this.cantidadParcial = proximaPosicion.cantidad;
                    this.cantidadPendiente = cantidadRequerida - this.cantidadParcial;
                }
                else
                {
                    this.cantidadParcial = cantidadRequerida;
                    this.cantidadPendiente = 0;
                }

                //el pedido continua activo, en la pantalla indica la posicion
                //el pedido carece de moduloActual, pero contiene posicion
                //el boton ok no debe aparecer para ese lugar sino para el modulo bloqueado
                //se debe evitar el boton ok del modulo que quede pendiente que no lo actualice
                //vincula la tarea para bloquear el nuevo modulo
                if (!proximaPosicion.modulo.VincularTarea(this))
                {
                    Console.WriteLine("PEDIDO PENDIENTE DE UN MODULO");
                    ConectorPLC.EncenderEspera(this);
                }
                else
                {
                    ConectorPLC.EncenderPosicion(this);
                }
                this.version = DateTime.Now.Ticks;
            }
        }

        /// <summary>
        /// Finaliza una accion de reposicion/recogida y prepara la siguiente
        /// Similar al estado tras Iniciar
        /// </summary>
        public void CancelarRecogida()
        {
            ConectorPLC.FinalizarPosicion(this);
            this.pedido = null;
            this.producto = null;
            this.lote = "";
            this.cantidad = 0;
            if (this.moduloActual != null)
            {
                this.moduloActual.DesvincularTarea(this);
            }
            this.posicion = null;
            this.cantidadParcial = 0;
            this.cantidadPendiente = 0;
            this.version = DateTime.Now.Ticks;
        }

        public void FinalizarRecogida(bool fin = true)
        {
            this.pedido = null;
            this.producto = null;
            this.lote = "";
            this.cantidad = 0;
            if (this.moduloActual != null)
            {
                ConectorPLC.EncenderFin(this, fin);
                this.gestor.controlModulos.PosponerLiberacion(this.moduloActual, this, null);
                this.moduloActual = null;
            }
            this.posicion = null;
            this.cantidadParcial = 0;
            this.cantidadPendiente = 0;
            this.version = DateTime.Now.Ticks;
        }

    }
}
