using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json.Serialization;

namespace NoCocinoMas
{
    public class Posiciones : Entidades, IEntidades
    {
        /// <summary>
        /// Filtra las posiciones con modulo coincidente
        /// </summary>
        /// <param name="modulo"></param>
        public IEnumerable<Posicion> Filtrar(Modulo modulo)
        {
            return from Posicion posicion in this.listado where modulo.Comparar(posicion.modulo_id) select posicion;
        }

        /// <summary>
        /// Devuelve el listado de posiciones que coincidan con el codigo del producto
        /// </summary>
        /// <param name="id_producto"></param>
        /// <returns></returns>
        public Posiciones BuscarProducto(int producto_codigo, string lote)
        {
            Posiciones resultado = new Posiciones();
            resultado.Agregar(from Posicion posicion in this.listado where posicion.ContieneProducto(producto_codigo, lote) select posicion);
            return resultado;
        }
        public Posiciones BuscarVacias()
        {
            Posiciones resultado = new Posiciones();
            resultado.Agregar(from Posicion posicion in this.listado where posicion.cantidad == 0 select posicion);
            return resultado;
        }

        public Posiciones BuscarEnvase(Producto producto)
        {
            Posiciones resultado = new Posiciones();
            resultado.Agregar(from Posicion posicion in this.listado where posicion.envase == producto.envase_id select posicion);
            return resultado;
        }

        public Posicion BuscarNombre(string nombre)
        {
            return (from Posicion posicion in this.listado where posicion.nombre == nombre select posicion).FirstOrDefault();
        }

        /// <summary>
        /// Crea el objeto Centralita en cada posicion existente
        /// </summary>
        /// <param name="centralitas"></param>
        public void VincularCentralitas(Centralitas centralitas)
        {
            foreach (Posicion posicion in this.listado)
            {
                posicion.VincularCentralita(centralitas);
            }
        }

        /// <summary>
        /// Crea el objeto Producto en cada posicion existente.
        /// </summary>
        /// <param name="productos"></param>
        public void VincularProductos(Productos productos)
        {
            foreach (Posicion posicion in this.listado)
            {
                posicion.VincularProducto(productos);
            }
        }

        /// <summary>
        /// Agregamos el conjunto de objetos de una consulta MySql
        /// </summary>
        /// <param name="resultado"></param>
        public void Agregar(MySqlDataReader resultado)
        {
            while (resultado.Read())
            {
                this.listado.Add(new Posicion(resultado));
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
            Posicion a = new Posicion(parametros);
            id = ConectorSQL.CrearEntidades(ConectorSQL.insertPosicion, a.GetValoresInsertSQL());
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

        public string ObtenerPrimerLote()
        {
            List<string> lotes = this.ObtenerLotes();
            if (lotes.Count > 0)
            {
                return RevertirLote(lotes.First());
            }
            return "";
        }

        /// <summary>
        /// Funcion para devolver los lotes ordenados.
        /// Pensado para un objeto Posiciones con el producto comun
        /// </summary>
        /// <returns></returns>
        public List<string> ObtenerLotes()
        {
            try
            {
                List<string> lotes = new List<string>();
                foreach (Posicion p in this.listado)
                {
                    string lote = p.lote;
                    if (lote.Trim() != "" && !lotes.Contains(lote))
                    {
                        lote = InvertirLote(lote);
                        lotes.Add(lote);
                    }
                }
                lotes.Sort();
                return lotes;
            }
            catch (Exception e)
            {
                Gestor.gestor.EscribirError("ObtenerLotes: " + e.Message);
                return null;
            }
        }
        private string InvertirLote(string lote)
        {
            try
            {
                if (lote.Length != 7)
                {
                    return null;
                }
                char[] charArray = lote.ToCharArray();
                Array.Reverse(charArray);
                lote = new string(charArray);
                lote = lote.Substring(5, 2) + lote.Substring(3, 2) + lote.Substring(1, 2) + lote.Substring(0, 1);
                return lote;
            }
            catch (Exception e)
            {
                Gestor.gestor.EscribirError("InvertirLote: " + e.Message);
                return null;
            }
        }
        private string RevertirLote(string lote)
        {
            try
            {
                if (lote == null || lote.Length != 7)
                {
                    return null;
                }
                char[] charArray = lote.ToCharArray();
                Array.Reverse(charArray);
                lote = new string(charArray);
                lote = lote.Substring(5, 2) + lote.Substring(3, 2) + lote.Substring(1, 2) + lote.Substring(0, 1);
                return lote;
            }
            catch (Exception e)
            {
                Gestor.gestor.EscribirError("InvertirLote: " + e.Message);
                return null;
            }
        }

        /// <summary>
        /// Posicion mas cerca a la tarea dada.
        /// Opcionalmente se comprueba el bloqueo.
        /// </summary>
        /// <param name="tarea"></param>
        /// <returns></returns>
        public Posicion PosicionMasCercana(Tarea tarea, bool saltarBloqueo = false)
        {
            int preferencia = 0;
            if (tarea.moduloActual != null)
            {
                preferencia = tarea.moduloActual.preferencia_reposicion;
            }
            Posicion posicion = null;
            Posicion primeraDeTodas = (Posicion)this.listado.FirstOrDefault();
            foreach (Posicion p in this.listado)
            {
                //el modulo está libre
                if (saltarBloqueo || !p.modulo.EstaBloqueado(tarea))
                {
                    //el modulo esta en o a continuacion de nuestra posicion
                    if (p.modulo.preferencia_reposicion >= preferencia)
                    {
                        //si es el primero que nos encontramos nos lo quedamos
                        if (posicion == null)
                        {
                            posicion = p;
                        }
                        //si no, ademas debe ser menor que la posicion seleccionada anteriormente
                        else
                        {
                            if (p.modulo.preferencia_reposicion < posicion.modulo.preferencia_reposicion)
                            {
                                posicion = p;
                            }
                            else if (p.modulo.preferencia_reposicion == posicion.modulo.preferencia_reposicion && p.nombre.CompareTo(posicion.nombre) < 0)
                            {
                                //si la posicion es menor pero esta en una balda mas alta o mas a la izquierda se escoge esa
                                posicion = p;
                            }
                        }
                    }
                    //paralelamente guardamos la posicion mas baja
                    if (p.modulo.preferencia_reposicion < primeraDeTodas.modulo.preferencia_reposicion)
                    {
                        primeraDeTodas = p;
                    }
                }
            }
            if (posicion == null)
            {
                return primeraDeTodas;
            }
            return posicion;
            /*
            int alias = 0;
            if (tarea.moduloActual != null)
            {
                alias = tarea.moduloActual.alias;
            }
            Posicion posicion = (Posicion)this.listado.FirstOrDefault();
            Posicion primeraDeTodas = (Posicion)this.listado.FirstOrDefault();
            foreach (Posicion p in this.listado)
            {
                if (saltarBloqueo || !p.modulo.EstaBloqueado(tarea))
                {
                    if (p.modulo.alias < posicion.modulo.alias && p.modulo.alias >= alias)
                    {
                        posicion = p;
                    }
                    else if (p.modulo.alias == posicion.modulo.alias && p.nombre.CompareTo(posicion.nombre) < 0)
                    {
                        //si la posicion es menor pero esta en una balda mas alta o mas a la izquierda se escoge esa
                        posicion = p;
                    }
                    if (p.modulo.alias < primeraDeTodas.modulo.alias)
                    {
                        primeraDeTodas = p;
                    }
                }
            }
            if (posicion == null)
            {
                return primeraDeTodas;
            }
            return posicion;
            */
        }

        public Posicion ProximaPosicion()
        {
            Posicion proximaPosicion = null;
            foreach (Posicion p in this.listado)
            {
                if (proximaPosicion == null)
                {
                    proximaPosicion = p;
                }
                else
                {
                    if (p.modulo_id < proximaPosicion.modulo_id)
                    {
                        proximaPosicion = p;
                    }
                    else if (p.modulo_id == proximaPosicion.modulo_id && p.nombre.CompareTo(proximaPosicion.nombre) < 0)
                    {
                        proximaPosicion = p;
                    }
                }
            }
            return proximaPosicion;
        }
        /// <summary>
        /// Proxima posicion disponible desde la inicial. No se tiene en cuenta la balda ni la posicion inicial, solo el modulo.
        /// </summary>
        /// <param name="posicionInicial"></param>
        /// <returns></returns>
        public Posicion ProximaPosicion(Posicion posicionInicial)
        {
            try
            {
                Posicion proximaPosicion = null;
                foreach (Posicion p in this.listado)
                {
                    if (p.modulo_id >= posicionInicial.modulo_id)
                    {
                        if (proximaPosicion == null)
                        {
                            proximaPosicion = p;
                        }
                        else
                        {
                            if (p.modulo_id < proximaPosicion.modulo_id)
                            {
                                proximaPosicion = p;
                            }
                            else if (p.modulo_id == proximaPosicion.modulo_id && p.nombre.CompareTo(proximaPosicion.nombre) < 0)
                            {
                                proximaPosicion = p;
                            }
                        }
                    }
                }
                if (proximaPosicion == null)
                {
                    return this.ProximaPosicion();
                }
                else
                {
                    return proximaPosicion;
                }
            }
            catch (Exception e)
            {
                Gestor.gestor.EscribirError("ProximaPosicion: " + e.Message);
                return null;
            }
        }

        public Posiciones NoCompletas()
        {
            Posiciones resultado = new Posiciones();
            resultado.Agregar(from Posicion posicion in this.listado where posicion.Disponible() > 0 select posicion);
            return resultado;
        }

        public Posiciones NoBloqueadas(Tarea tarea)
        {
            Posiciones resultado = new Posiciones();
            resultado.Agregar(from Posicion posicion in this.listado where !posicion.modulo.EstaBloqueado(tarea) select posicion);
            return resultado;
        }

        /// <summary>
        /// Encuantra la posicion mas cercana a la tarea dada
        /// </summary>
        /// <param name="tarea"></param>
        /// <returns></returns>
        public Posicion MasCercano(Tarea tarea)
        {
            int preferencia_reposicion = 0;
            if (tarea.moduloActual != null)
            {
                preferencia_reposicion = tarea.moduloActual.preferencia_reposicion;
            }
            Posicion posicion = (Posicion) this.listado.FirstOrDefault();
            foreach (Posicion p in this.listado)
            {
                if (p.modulo.preferencia_reposicion < posicion.modulo.preferencia_reposicion && p.modulo.preferencia_reposicion >= preferencia_reposicion)
                {
                    posicion = p;
                }
            }
            return posicion;
        }
    }

    public class Posicion : Entidad, IEntidad
    {
        public string nombre { get; set; }
        public int balda { get; set; }
        public int modulo_id { get; set; }
        //variables del led
        public int centralita_id { get; set; }
        public int linea { get; set; }
        public int led_inicial { get; set; }
        public int led_longitud { get; set; }
        //variables del producto contenido
        public int producto_codigo { get; set; }
        public int cantidad { get; set; }
        public string lote { get; set; }
        //atributos
        public int capacidad { get; set; }
        public int envase { get; set; }
        public int estado { get; set; }
        //objetos
        [JsonIgnore]
        public Centralita centralita { get; set; }
        [JsonIgnore]
        public Producto producto { get; set; }
        public Modulo modulo { get; set; }
        
        public LineaPedido lineaPedidoPendiente { get; set; }


        public Posicion(Parametros parametros)
        {
            this.id = parametros.BuscarInt("id");
            this.nombre = parametros.Buscar("nombre");
            this.balda = parametros.BuscarInt("balda");
            this.modulo_id = parametros.BuscarInt("modulo_id");
            this.centralita_id = parametros.BuscarInt("centralita_id");
            this.linea = parametros.BuscarInt("linea");
            this.led_inicial = parametros.BuscarInt("led_inicial");
            this.led_longitud = parametros.BuscarInt("led_longitud");
            this.producto_codigo = parametros.BuscarInt("producto_codigo");
            this.cantidad = parametros.BuscarInt("cantidad");
            this.lote = parametros.Buscar("lote");
            this.capacidad = parametros.BuscarInt("capacidad");
            this.envase = parametros.BuscarInt("envase");
            this.estado = parametros.BuscarInt("estado");
            this.lineaPedidoPendiente = null;
        }
        public Posicion(MySqlDataReader datos)
        {
            this.id = datos.GetInt32("id");
            this.nombre = datos.GetString("nombre");
            this.balda = datos.GetInt32("balda");
            this.modulo_id = datos.GetInt32("modulo_id");
            this.centralita_id = datos.GetInt32("centralita_id");
            this.linea = datos.GetInt32("linea");
            this.led_inicial = datos.GetInt32("led_inicial");
            this.led_longitud = datos.GetInt32("led_longitud");
            this.producto_codigo = datos.GetInt32("producto_codigo");
            this.cantidad = datos.GetInt32("cantidad");
            this.lote = datos.GetString("lote");
            this.capacidad = datos.GetInt32("capacidad");
            this.envase = datos.GetInt32("envase");
            this.estado = datos.GetInt32("estado");
            this.centralita = new Centralita();
            this.producto = new Producto();
            this.lineaPedidoPendiente = null;
        }

        /// <summary>
        /// Devuelve un array de objetos con los valores.
        /// </summary>
        /// <returns></returns>
        public object[] GetValores()
        {
            object[] valores = {
                this.id,
                this.nombre,
                this.balda,
                this.modulo_id,
                this.centralita_id,
                this.linea,
                this.led_inicial,
                this.led_longitud,
                this.producto_codigo,
                this.cantidad,
                this.lote,
                this.capacidad,
                this.envase,
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
                this.id,
                this.nombre,
                this.balda,
                this.modulo_id,
                this.centralita_id,
                this.linea,
                this.led_inicial,
                this.led_longitud,
                this.producto_codigo,
                this.cantidad,
                this.lote,
                this.capacidad,
                this.envase,
                this.estado
            };
            return valores;
        }

        public object[] GetValoresActualizarStock()
        {
            object[] valores = {
                this.producto_codigo,
                GetFechaLote(),
                this.cantidad                
            };
            return valores;
        }
        private string GetFechaLote()
        {
            string fechaLote = "";
            try
            {

                if (this.lote.Length == 7)
                {
                    char[] charArray = this.lote.ToCharArray();
                    Array.Reverse(charArray);
                    fechaLote = new string(charArray);
                    fechaLote = string.Format("{0}/{1}/{2}", fechaLote.Substring(1, 2), fechaLote.Substring(3, 2), fechaLote.Substring(5, 2));
                    fechaLote = Convert.ToDateTime(fechaLote).ToString("yyyy-MM-dd");
                }
            }
            catch (Exception e)
            {
                Gestor.gestor.EscribirError("GetFechaLote: " + e.Message);
            }
            return fechaLote;
        }

        /// <summary>
        /// Devuelve un array de objetos con los valores.
        /// Adaptado para crear consultas SQL
        /// </summary>
        /// <returns></returns>
        public string GetValoresInsertSQL()
        {
            return string.Format("('{0}', {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, '{9}', {10}, {11}, {12})",
                this.nombre,
                this.balda,
                this.modulo_id,
                this.centralita_id,
                this.linea,
                this.led_inicial,
                this.led_longitud,
                this.producto_codigo,
                this.cantidad,
                this.lote,
                this.capacidad,
                this.envase,
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
            Respuesta r = Respuesta.Crear(false, 0, (Posicion)this);
            int id = parametros.BuscarInt("id");
            string nombre = parametros.Buscar("nombre");
            int balda = parametros.BuscarInt("balda");
            int modulo_id = parametros.BuscarInt("modulo_id");
            int centralita_id = parametros.BuscarInt("centralita_id");
            int linea = parametros.BuscarInt("linea");
            int led_inicial = parametros.BuscarInt("led_inicial");
            int led_longitud = parametros.BuscarInt("led_longitud");
            int producto_codigo = parametros.BuscarInt("producto_codigo");
            int cantidad = parametros.BuscarInt("cantidad");
            string lote = parametros.Buscar("lote");
            int capacidad = parametros.BuscarInt("capacidad");
            int envase = parametros.BuscarInt("envase");
            int estado = parametros.BuscarInt("estado");
            object[] valores = {
                id,
                nombre,
                balda,
                modulo_id,
                centralita_id,
                linea,
                led_inicial,
                led_longitud,
                producto_codigo,
                cantidad,
                lote,
                capacidad,
                envase,
                estado
            };
            if (!this.Comparar(id))
            {
                r.Error("ID no coincidente");
            }
            else
            {
                bool ok = ConectorSQL.ActualizarEntidades(ConectorSQL.updatePosicion, valores);
                if (ok)
                {
                    this.nombre = nombre;
                    this.balda = balda;
                    this.modulo_id = modulo_id;
                    this.centralita_id = centralita_id;
                    this.linea = linea;
                    this.led_inicial = led_inicial;
                    this.led_longitud = led_longitud;
                    this.producto_codigo = producto_codigo;
                    this.cantidad = cantidad;
                    this.lote = lote;
                    this.capacidad = capacidad;
                    this.envase = envase;
                    this.estado = estado;
                }
                else
                {
                    r.error = false;
                    r.mensaje = "Actualización SQL falló.";
                }
            }
            return r;
        }

        /// <summary>
        /// Asocia el producto correspondiente de un listado
        /// </summary>
        /// <param name="productos"></param>
        public void VincularProducto(Productos productos)
        {
            Producto p = (Producto)productos.BuscarCodigo(this.producto_codigo);
            //si el producto es distinto
            if (p != this.producto)
            {
                //Eliminamos la posicion del producto anterior
                if (this.producto != null && this.producto.nombre != null)
                {
                    this.producto.EliminarPosicion(this);
                }
                //Agregamos la posicion a la lista del producto.
                if (p != null)
                {
                    this.producto = p;
                    this.producto.AgregarPosicion(this);
                }
                else
                {
                    this.producto = null;
                    this.producto_codigo = 0;
                    this.lote = "";
                }
            }
        }

        /// <summary>
        /// Asocia la centralita correspondiente de un listado
        /// </summary>
        /// <param name="centralitas"></param>
        public void VincularCentralita(Centralitas centralitas)
        {
            this.centralita = (Centralita) centralitas.BuscarId(this.centralita_id);
            //this.centralita = (Centralita) (from centralita in centralitas.listado where centralita.id.Equals(this.centralita_id) select centralita).Single();
        }

        public bool ContieneProducto(int producto_codigo, string lote)
        {
            if (this.producto == null)
            {
                return false;
            }
            if (lote != null && lote != "")
            {
                return this.producto.codigo.Equals(producto_codigo) && this.cantidad > 0 && this.lote.Trim().Equals(lote.Trim());
            }
            else
            {
                return this.producto.codigo.Equals(producto_codigo) && this.cantidad > 0;
            }
        }
        public bool ContieneProducto(int producto_codigo)
        {
            return this.producto_codigo.Equals(producto_codigo);
        }


        public void ActualizarProducto(Producto producto, string lote, int cantidadParcial)
        {
            if (this.producto != null && (this.producto.codigo != producto.codigo || this.lote != lote))
            {
                Gestor.gestor.EscribirError("Error actualizando el producto");
            }
            else
            {
                this.cantidad += cantidadParcial;
                if (this.cantidad < 0)
                {
                    this.cantidad = 0;
                }
                if (this.cantidad > this.capacidad)
                {
                    this.cantidad = this.capacidad;
                }
                if (this.cantidad == 0)
                {                    
                    this.producto = null;
                    this.producto_codigo = 0;
                    this.lote = "";
                    producto.EliminarPosicion(this);
                }
                else
                {
                    this.producto = producto;
                    this.producto_codigo = producto.codigo;
                    this.lote = lote;
                    producto.AgregarPosicion(this);
                }
            }
        }

        /// <summary>
        /// Atencion: indica la cantidad disponible
        /// para Reponer
        /// </summary>
        /// <returns></returns>
        public int Disponible()
        {
            return this.capacidad - this.cantidad;
        }
    }
}
