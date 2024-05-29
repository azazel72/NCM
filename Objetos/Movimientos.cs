using System.Collections.Generic;
using System.Linq;
using System.Collections;
using MySql.Data.MySqlClient;
using System;
using System.Text.Json.Serialization;

namespace NoCocinoMas
{
    public class Movimientos : Entidades, IEntidades
    {
        /// <summary>
        /// Devuelve los movimientos coincidentes con el pedido
        /// </summary>
        /// <param name="pedido"></param>
        /// <returns></returns>
        public IEnumerable<Movimiento> Filtrar(Pedido pedido)
        {
            return from Movimiento m in this.listado where pedido.numero == m.pedido_numero select m;
        }

        /// <summary>
        /// Agregamos el conjunto de objetos de una consulta MySql
        /// </summary>
        /// <param name="resultado"></param>
        public void Agregar(MySqlDataReader resultado)
        {
            while (resultado.Read())
            {
                this.listado.Add(new Movimiento(resultado));
            }
        }

        /// <summary>
        /// Vinculamos el producto. Se debe considerar el codigo y no el id
        /// </summary>
        /// <param name="productos"></param>
        public void VincularProductos(Dictionary<int, Producto> listadoProductos)
        {
            foreach (Movimiento l in this.listado)
            {
                l.VincularProducto(listadoProductos);
            }
        }

        /// <summary>
        /// Vinculamos el Operario
        /// </summary>
        /// <param name="operarios"></param>
        public void VincularOperarios(Operarios operarios)
        {
            foreach (Movimiento l in this.listado)
            {
                if (l.operario_id > 0)
                {
                    l.VincularOperario(operarios);
                }                
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
            Movimiento a = new Movimiento(parametros);
            id = ConectorSQL.CrearEntidades(ConectorSQL.insertMovimiento, a.GetValoresInsertSQL());
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

    }

    public class Movimiento : Entidad, IEntidad
    {
        public int operario_id { get; set; }
        public int producto_codigo { get; set; }
        public string lote { get; set; }
        public int cantidad { get; set; }
        public string posicion_nombre { get; set; }
        public int pedido_numero { get; set; }
        public int linea_pedido_id { get; set; }
        public int accion { get; set; }
        public DateTime fecha { get; set; }
        public string comentario { get; set; }
        //objetos
        [JsonIgnore]
        public Pedido pedido { get; set; }
        [JsonIgnore]
        public LineaPedido lineaPedido { get; set; }
        [JsonIgnore]
        public Producto producto { get; set; }
        [JsonIgnore]
        public Operario operario { get; set; }
        [JsonIgnore]
        public Posicion posicion { get; set; }

        public Movimiento(Tarea tarea)
        {
            this.operario_id = (int) tarea.operario.id;
            this.operario = tarea.operario;
            this.producto_codigo = tarea.producto == null ? 0 : tarea.producto.codigo;
            this.producto = tarea.producto;
            this.lote = tarea.lote;
            this.cantidad = tarea.cantidadParcial;
            this.posicion_nombre = tarea.posicion == null ? "" : tarea.posicion.nombre;
            this.posicion = tarea.posicion;
            this.pedido_numero = tarea.pedido == null ? 0 : tarea.pedido.numero;
            this.pedido = tarea.pedido;
            this.lineaPedido = tarea.lineaPedidoActual;
            this.linea_pedido_id = tarea.lineaPedidoActual == null ? 0 : (int) tarea.lineaPedidoActual.id;
            this.accion = tarea.accion;
            this.fecha = DateTime.Now;
            this.comentario = "";
        }

        public Movimiento(Parametros parametros)
        {
            this.id = parametros.BuscarInt("id");
            this.operario_id = parametros.BuscarInt("operario_id");
            this.producto_codigo = parametros.BuscarInt("producto_codigo");
            this.lote = parametros.Buscar("lote");
            this.cantidad = parametros.BuscarInt("cantidad");
            this.posicion_nombre = parametros.Buscar("posicion_nombre");
            this.pedido_numero = parametros.BuscarInt("pedido_numero");
            this.linea_pedido_id = parametros.BuscarInt("linea_pedido_id");
            this.accion = parametros.BuscarInt("accion");
            this.fecha = parametros.BuscarFecha("fecha");
            this.comentario = "";
        }
        public Movimiento()
        {
            this.pedido = null;
            this.producto = null;
            this.operario = null;
            this.posicion = null;
            this.fecha = default(DateTime);
        }
        public Movimiento(MySqlDataReader datos)
        {
            this.id = datos.GetInt32("id");
            this.operario_id = datos.GetInt32("operario_id");
            this.producto_codigo = datos.GetInt32("producto_codigo");
            this.lote = datos.GetString("lote");
            this.cantidad = datos.GetInt32("cantidad");
            this.posicion_nombre = datos.GetString("posicion_nombre");
            this.pedido_numero = datos.GetInt32("pedido_numero");
            this.linea_pedido_id = datos.GetInt32("linea_pedido_id");
            this.accion = datos.GetInt32("accion");
            this.fecha = datos.GetDateTime("fecha");
            this.comentario = "";
            this.pedido = null;
            this.producto = null;
            this.operario = null;
            this.posicion = null;
        }

        public Movimiento(LineaPedido lineaPedido, string lote, int cantidad)
        {
            this.operario_id = 0;
            this.operario = null;
            this.producto_codigo = lineaPedido.producto_codigo;
            this.producto = lineaPedido.producto;
            this.lote = lote;
            this.cantidad = cantidad;
            this.posicion_nombre = "";
            this.posicion = null;
            this.pedido_numero = lineaPedido.pedido_numero;
            this.pedido = lineaPedido.pedido;
            this.lineaPedido = lineaPedido;
            this.linea_pedido_id = (int) lineaPedido.id;
            this.accion = 1;
            this.fecha = DateTime.Now;
            this.comentario = "";
        }

        public Movimiento(Parametros parametros, Posicion posicion, Producto producto, Operario operario)
        {
            this.operario_id = parametros.BuscarInt("operario_extraccion");
            this.operario = operario;
            this.producto_codigo = parametros.BuscarInt("codigo_extraccion");
            this.producto = producto;
            this.lote = parametros.Buscar("lote_extraccion");
            this.cantidad = parametros.BuscarInt("cantidad_extraccion");
            this.posicion_nombre = parametros.Buscar("posicion_extraccion");
            this.posicion = posicion;
            this.pedido_numero = 0;
            this.pedido = null;
            this.lineaPedido = null;
            this.linea_pedido_id = 0;
            this.accion = parametros.BuscarInt("motivo_extraccion");
            this.fecha = DateTime.Now;
            this.comentario = parametros.Buscar("comentario_extraccion");
        }

        public Movimiento(Posicion posicion, string nuevaPosicion, Producto producto, string lote, int cantidad, Operario operario)
        {
            this.operario_id = (int)operario.id;
            this.operario = operario;
            this.producto_codigo = producto.codigo;
            this.producto = producto;
            this.lote = lote;
            this.cantidad = cantidad;
            this.posicion_nombre = posicion.nombre;
            this.posicion = posicion;
            this.pedido_numero = 0;
            this.pedido = null;
            this.lineaPedido = null;
            this.linea_pedido_id = 0;
            this.accion = 4;
            this.fecha = DateTime.Now;
            this.comentario = nuevaPosicion;
        }

        /// <summary>
        /// Devuelve un array de objetos con los valores.
        /// </summary>
        /// <returns></returns>
        public object[] GetValores()
        {
            object[] valores = {
                this.id,
                this.operario_id,
                this.producto_codigo,
                this.lote,
                this.cantidad,
                this.posicion_nombre,
                this.pedido_numero,
                this.linea_pedido_id,
                this.accion,
                this.fecha
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
/*            object[] valores = {
                this.id,
                this.operario_id,
                this.producto_codigo,
                this.producto != null ? this.producto.nombre : "No vinculado",
                this.lote,
                this.cantidad,
                this.posicion_nombre,
                this.pedido_numero,
                this.linea_pedido_id,
                this.accion,
                this.fecha
            };*/
            object[] valores = {
                this.producto_codigo,
                this.posicion_nombre,
                this.cantidad,
            };
            return valores;
        }

        /// <summary>
        /// Devuelve un array de objetos con los valores.
        /// Adaptado para crear consultas SQL
        /// </summary>
        /// <returns></returns>
        /// static public string insertMovimiento = "INSERT INTO movimientos (operario_id, producto_codigo, lote, cantidad, posicion_nombre, pedido_numero, linea_pedido_id, accion) VALUES {0};";
        public string GetValoresInsertSQL()
        {
            return string.Format("({0},{1},'{2}',{3},'{4}',{5},{6},{7},'{8}')",
                    this.operario_id,
                    this.producto_codigo,
                    this.lote,
                    this.cantidad,
                    this.posicion_nombre,
                    this.pedido_numero,
                    this.linea_pedido_id,
                    this.accion,
                    this.comentario
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
            Respuesta r = Respuesta.Crear(false, 0, (Movimiento)this);
            r.Error("Funcion no implementada.");
            return r;
        }

        public void VincularProducto(Dictionary<int, Producto> listadoProductos)
        {
            listadoProductos.TryGetValue(this.producto_codigo, out Producto p);
            this.producto = p;
        }

        public void VincularOperario(Operarios operarios)
        {
            this.operario = (Operario) operarios.BuscarId(this.operario_id);
        }

        public string GetFecha()
        {
            if (this.lote.Length != 7)
            {
                return this.lote;
            }

            //1260319

            char[] charArray = this.lote.ToCharArray();
            Array.Reverse(charArray);
            string fecha = new string(charArray);
            fecha = "20" + fecha.Substring(5, 2) + "-" + fecha.Substring(3, 2) + "-" + fecha.Substring(1, 2);
            return fecha;
        }

    }
}
