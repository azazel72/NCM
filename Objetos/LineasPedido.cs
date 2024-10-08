using System.Collections.Generic;
using System.Linq;
using System.Collections;
using MySql.Data.MySqlClient;
using System;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace NoCocinoMas
{
    public class LineasPedido : Entidades, IEntidades
    {
        /// <summary>
        /// Devuelve las lineas de pedido coincidentes con el pedido
        /// </summary>
        /// <param name="pedido"></param>
        /// <returns></returns>
        public IEnumerable<LineaPedido> Filtrar(Pedido pedido)
        {
            return from LineaPedido linea in this.listado where pedido.numero == linea.pedido_numero select linea;
        }

        /// <summary>
        /// Agregamos el conjunto de objetos de una consulta MySql
        /// </summary>
        /// <param name="resultado"></param>
        public void Agregar(MySqlDataReader resultado)
        {
            while (resultado.Read())
            {
                this.listado.Add(new LineaPedido(resultado));
            }

        }

        /// <summary>
        /// Vinculamos el producto. Se debe considerar el codigo y no el id
        /// </summary>
        /// <param name="productos"></param>
        public void VincularProductos(Dictionary<int, Producto> listadoProductos)
        {
            foreach (LineaPedido l in this.listado)
            {
                l.VincularProducto(listadoProductos);
            }
        }

        public void VincularProductosNuevos(Productos nuevosProductos)
        {
            foreach (LineaPedido l in this.listado)
            {
                if (l.producto == null)
                {
                    l.producto = nuevosProductos.BuscarCodigo(l.producto_codigo) ?? Gestor.productoNoEncontrado;
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
            LineaPedido a = new LineaPedido(parametros);
            id = ConectorSQL.CrearEntidades(ConectorSQL.insertLineaPedido, a.GetValoresInsertSQL());
            //insertLineaPedido = "INSERT INTO lineas_pedido (pedido_numero, producto_codigo, cantidad, recogido, estado) VALUES {0};";

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

        //eliminamos la linea que sea menu y agregamos en su lugar el contenido
        public void SustituirMenus(Menus menus)
        {
            LineasPedido lineasEliminar = new LineasPedido();
            LineasPedido lineasAgregar = new LineasPedido();
            foreach (LineaPedido linea in this.listado)
            {
                LineasPedido lineas = menus.SustituirMenus(linea);
                if (lineas.Contador() > 0)
                {
                    lineasEliminar.Agregar(linea);
                    lineasAgregar.Agregar(lineas);
                }
            }
            this.Eliminar(lineasEliminar);
            this.Agregar(lineasAgregar);
        }

        public void ComprobarMenus(Menus menus)
        {

            LineasPedido lineasEliminar = new LineasPedido();
            LineasPedido lineasAgregar = new LineasPedido();
            foreach (LineaPedido linea in this.listado)
            {
                LineasPedido lineas = menus.SustituirMenus(linea);

                if (lineas.Contador() > 0)
                {
                    lineasEliminar.Agregar(linea);
                    lineasAgregar.Agregar(lineas);
                }
            }
            //lineasAgregar.

            foreach (LineaPedido l in lineasEliminar)
            {
                ConectorSQL.DeleteSQL("lineas_pedido", (int)l.id);
                //eliminamos la linea del pedido de la memoria
                l.pedido.lineas.Eliminar(l);
            }
            //Vinculacion de productos a lineas y de lineas a pedidos
            Dictionary<int, Producto> listadoProductos = Gestor.gestor.ListadoProductos();
            foreach (LineaPedido l in lineasAgregar)
            {
                int id = ConectorSQL.GuardarEntidad(ConectorSQL.guardarLineasPedido, l);
                if (id > 0)
                {
                    l.id = id;
                }
                //vinculacion del producto
                l.VincularProducto(listadoProductos);
                //vinculacion del pedido
                Pedido pedido = Gestor.gestor.pedidos.BuscarNumero(l.pedido_numero);
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
            //actualizar vista
            Gestor.gestor.lineasPedido.Agregar(lineasAgregar);

            //Application.Restart();
        }

        public LineaPedido Primera()
        {
            return (LineaPedido) this.listado.First();
        }
        public LineaPedido PrimeraConProducto(int codigo)
        {
            foreach(LineaPedido linea in this.listado)
            {
                if (linea.producto_codigo.Equals(codigo)) {
                    return linea;
                }
            }
            return null;
        }
    }

    public class LineaPedido : Entidad, IEntidad
    {
        public int pedido_numero { get; set; }
        public int producto_codigo { get; set; }
        public int cantidad { get; set; }
        public int recogido { get; set; }
        public int estado { get; set; }
        //objetos
        [JsonIgnore]
        public Pedido pedido { get; set; }
        //[JsonIgnore]
        public Producto producto { get; set; }
        //[JsonIgnore]
        public string envio { get; set; }

        public LineaPedido(Parametros parametros)
        {
            this.id = parametros.BuscarInt("id");
            this.pedido_numero = parametros.BuscarInt("pedido_numero");
            this.producto_codigo = parametros.BuscarInt("producto_codigo");
            this.cantidad = parametros.BuscarInt("cantidad");
            this.recogido = parametros.BuscarInt("recogido");
            this.estado = parametros.BuscarInt("estado");
        }
        public LineaPedido()
        {
            this.pedido = null;
            this.producto = null;
        }
        public LineaPedido(MySqlDataReader datos)
        {
            this.id = datos.GetInt32("id");
            this.pedido_numero = datos.GetInt32("pedido_numero");
            this.producto_codigo = datos.GetInt32("producto_codigo");
            this.cantidad = datos.GetInt32("cantidad");
            this.recogido = datos.GetInt32("recogido");
            this.estado = datos.GetInt32("estado");
            this.pedido = null;
            this.producto = null;
        }
        public LineaPedido(LineaPedido linea, Menu menu)
        {
            this.id = 0;
            this.pedido_numero = linea.pedido_numero;
            this.producto_codigo = menu.products_id;
            this.cantidad = linea.cantidad * menu.products_quantity;
            this.recogido = 0;
            this.estado = 0;
            this.pedido = null;
            this.producto = null;

            //this.pedido = linea.pedido;
            //this.producto = menu.producto;
        }


        /// <summary>
        /// Devuelve un array de objetos con los valores.
        /// Adaptado para llenar las tablas.
        /// </summary>
        /// <returns></returns>
        public object[] GetValores()
        {
            object[] valores = {
                this.id,
                this.pedido_numero,
                this.producto_codigo,
                this.cantidad,
                this.recogido,
                this.estado
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
                this.producto_codigo,
                this.producto != null ? this.producto.nombre : "No vinculado",
                this.cantidad,
                this.recogido
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
            return string.Format("({0},{1},{2},{3},{4})",
                this.pedido_numero,
                this.producto_codigo,
                this.cantidad,
                this.recogido,
                this.estado
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
            Respuesta r = Respuesta.Crear(false, 0, (LineaPedido)this);
            try
            {
                int id = parametros.BuscarInt("id");
                int cantidad = parametros.BuscarInt("cantidad");
                int recogido = parametros.BuscarInt("recogido");
                int estado = (cantidad - recogido) == 0 ? 1 : 0;
                object[] valores = {
                    id,
                    cantidad,
                    recogido,
                    estado
                };
                if (!this.Comparar(id))
                {
                    r.Error("ID no coincidente");
                }
                else
                {
                    bool ok = ConectorSQL.ActualizarEntidades(ConectorSQL.updateLineaPedido, valores);
                    if (ok)
                    {
                        this.cantidad = cantidad;
                        this.recogido = recogido;
                        this.estado = estado;
                    }
                    else
                    {
                        r.error = false;
                        r.mensaje = "Actualización SQL falló.";
                    }
                }
            }
            catch (Exception e)
            {
                r.Error(e.Message);
            }
            return r;
        }

        public void VincularProducto(Dictionary<int, Producto> listadoProductos)
        {
            listadoProductos.TryGetValue(this.producto_codigo, out Producto p);
            this.producto = p ?? Gestor.productoNoEncontrado;
        }
        public void VincularProducto(Productos productos)
        {
            Producto p = (Producto)productos.BuscarCodigo(this.producto_codigo);
            this.producto = p ?? Gestor.productoNoEncontrado;
        }

        public int CantidadPendiente()
        {
            return this.cantidad - this.recogido;
        }

        public bool ComprobarCantidadRecogida(int c)
        {
            int nuevaCantidad = this.recogido + c;
            return nuevaCantidad <= this.cantidad && nuevaCantidad  >= 0;
        }
    }
}
