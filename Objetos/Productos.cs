using System.Collections.Generic;
using System.Linq;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Text.Json.Serialization;
using System;
using System.Security.RightsManagement;

namespace NoCocinoMas
{
    public class Productos : Entidades, IEntidades
    {
        static public List<KeyValuePair<string, int>> posicionesVerProducto = new List<KeyValuePair<string, int>>();

        public void VincularEnvases(Envases envases)
        {
            foreach (Producto producto in this.listado)
            {
                producto.VincularEnvase(envases);
            }
        }

        public void VincularUbicaciones(Ubicaciones ubicaciones)
        {
            foreach (Producto producto in this.listado)
            {
                producto.VincularUbicacion(ubicaciones);
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
                this.listado.Add(new Producto(resultado));
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
            Producto a = new Producto(parametros);
            id = ConectorSQL.CrearEntidades(ConectorSQL.insertProducto, a.GetValoresInsertSQL());
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

        public Producto BuscarCodigo(int codigo)
        {
            foreach (Producto p in this.listado)
            {
                if (p.codigo == codigo)
                {
                    return p;
                }
            }
            return null;
        }
    }

    public class Producto : Entidad, IEntidad
    {
        public int codigo { get; set; }
        private string _nombre;
        public string nombre {
            get { return this._nombre; }
            set { 
                this._nombre = value;
                if (value.Length > 0)
                {
                    string nombre = value.ToLower();
                    this.dieta = nombre.Contains("semana");
                    this.grande = nombre.Contains("grande") || nombre.Contains("familiar");
                }
            }
        }
        public int envase_id { get; set; }
        public int stock { get; set; }
        //objetos
        [JsonIgnore]
        public Envase envase { get; set; }
        [JsonIgnore]
        public Posiciones posiciones { get; set; }
        public string posicionRecogida { get; set; }
        public int numeroPosiciones { get; set; }
        public string posicionAlmacenamiento { get; set; }
        public Ubicacion ubicacionRecogida { get; set; }
        public Ubicacion ubicacionAlmacenamiento { get; set; }
        public bool dieta { get; set; }
        public bool grande { get; set; }
        public bool activo { get; set; }

        public Producto(Parametros parametros)
        {
            this.id = parametros.BuscarInt("id");
            this.codigo = parametros.BuscarInt("codigo");
            this.nombre = parametros.Buscar("nombre");
            this.envase_id = parametros.BuscarInt("envase_id");
            this.stock = parametros.BuscarInt("stock");
            this.posicionRecogida = parametros.Buscar("posicionRecogida");
            this.posicionAlmacenamiento = parametros.Buscar("posicionAlmacenamiento");
        }
        public Producto()
        {
        }
        public Producto(MySqlDataReader datos)
        {
            this.id = datos.GetInt32("id");
            this.codigo = datos.GetInt32("codigo");
            this.nombre = datos.GetString("nombre");
            this.envase_id = datos.GetInt32("envase_id");
            this.stock = datos.GetInt32("stock");
            try {
                this.posicionRecogida = datos.GetString("posicionRecogida");
                this.posicionAlmacenamiento = datos.GetString("posicionAlmacenamiento");
            }
            catch
            {
                this.posicionRecogida = "";
                this.posicionAlmacenamiento = "";
            }
            this.envase = new Envase();
            this.posiciones = new Posiciones();
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
                this.codigo,
                this.nombre,
                this.envase_id,
                this.stock,
                this.posicionRecogida,
                this.posicionAlmacenamiento
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
                this.codigo,
                this.nombre,
                this.posicionRecogida,
                this.posicionAlmacenamiento
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
            return string.Format("({0},'{1}',{2},{3})",
                this.codigo,
                this.nombre,
                this.envase_id,
                this.stock
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
            Respuesta r = Respuesta.Crear(false, 0, (Producto)this);
            int id = parametros.BuscarInt("id");
            int codigo = parametros.BuscarInt("codigo");
            string nombre = parametros.Buscar("nombre");
            int envase_id = parametros.BuscarInt("envase_id");
            int stock = parametros.BuscarInt("stock");
            object[] valores = {
                id,
                codigo,
                nombre,
                envase_id,
                stock
            };
            if (!this.Comparar(id))
            {
                r.Error("ID no coincidente");
            }
            else
            {
                bool ok = ConectorSQL.ActualizarEntidades(ConectorSQL.updateProducto, valores);
                if (ok)
                {
                    this.codigo = codigo;
                    this.nombre = nombre;
                    this.envase_id = envase_id;
                    this.stock = stock;
                }
                else
                {
                    r.Error("Actualización SQL falló.");
                }
            }
            return r;
        }

        public void VincularEnvase(Envases envases)
        {
            this.envase = (Envase) envases.BuscarId(this.envase_id);
            //this.envase = (from envase in envases.listado where envase.id.Equals(this.envase_id) select envase).Single();
        }

        public void VincularUbicacion(Ubicaciones ubicaciones)
        {
            this.ubicacionAlmacenamiento = (Ubicacion)ubicaciones.listado.Find(ubicacion => ((Ubicacion) ubicacion).nombre == this.posicionAlmacenamiento);
            string recogida = this.posicionRecogida ?? "";
            this.numeroPosiciones = (int) recogida.Length / 4;
            recogida = recogida.Length > 4 ? recogida.Substring(0, 4) : recogida;
            this.ubicacionRecogida = (Ubicacion)ubicaciones.listado.Find(ubicacion => ((Ubicacion)ubicacion).nombre == recogida);
            if (this.ubicacionRecogida == null)
            {
                Console.WriteLine("Ubicacion de recogida no encontrada: " + this.posicionRecogida);
            }
        }

        /// <summary>
        /// Agrega la posicion a la lista de posiciones donde se encuentre el producto.
        /// No agrega psiciones duplicadas.
        /// Esta funcion es llamada automaticamente desde la posicion cuando se le asigna un producto
        /// </summary>
        /// <param name="posicion"></param>
        public void AgregarPosicion(Posicion posicion)
        {
            this.posiciones.AgregarUnico(posicion);
        }

        /// <summary>
        /// Elimina la posicion indicada de la lista de posiciones
        /// Esta funcion es llamada automaticamente desde la posicion si se elimina el producto contenido o se cambia por otro
        /// </summary>
        /// <param name="posicion"></param>
        public void EliminarPosicion(Posicion posicion)
        {
            this.posiciones.Eliminar(posicion);
        }

        /// <summary>
        /// Suma todos los stocks de todas las posiciones donde se encuentre el producto
        /// </summary>
        /// <returns></returns>
        public int RefrescarStock()
        {
            this.stock = 0;
            foreach (Posicion p in this.posiciones)
            {
                stock += p.cantidad;
            }
            return this.stock;
        }

        public Posicion ProximaPosicionReposicion(Tarea tarea)
        {
            Posiciones posiciones = new Posiciones();
            foreach (Posicion p in this.posiciones.listado)
            {
                if (p.lote.Equals(tarea.lote) && p.cantidad > 0 
                    && p.Disponible() > 0 && !p.modulo.EstaBloqueado(tarea))
                {
                    posiciones.Agregar(p);
                }
            }
            int preferencia_reposicion = 0;
            if (tarea.moduloActual != null)
            {
                preferencia_reposicion = tarea.moduloActual.alias;
            }
            //selecciono la primera cualquiera como referencia para buscar la mas cercana
            Posicion posicion = (Posicion) posiciones.listado.FirstOrDefault();
            foreach (Posicion p in posiciones)
            {
                //if (!p.modulo.EstaBloqueado(tarea) && p.modulo.alias < posicion.modulo.alias && p.modulo.alias >= alias)
                if (!p.modulo.EstaBloqueado(tarea) && p.modulo.preferencia_reposicion < posicion.modulo.preferencia_reposicion && p.modulo.preferencia_reposicion >= preferencia_reposicion)
                {
                    posicion = p;
                }
            }
            return posicion;
        }

        public Posicion ProximaPosicionRecogida(Tarea tarea, bool saltarBloqueo = false)
        {
            //obtenemos el lote mas antiguo de los modulos disponibles, incluidos los bloqueados
            string primerLote = this.posiciones.ObtenerPrimerLote();

            Posiciones posiciones = new Posiciones();
            //seleccionamos las posiciones de modulos no bloqueados y con cantidad > 0 y lote antiguo
            foreach (Posicion p in this.posiciones.listado)
            {
                if (saltarBloqueo || !p.modulo.EstaBloqueado(tarea))
                {
                    if (p.lote == primerLote && p.cantidad > 0)
                    {
                        posiciones.Agregar(p);
                        Console.WriteLine(p.nombre);
                        Console.WriteLine(p.producto.nombre);
                    }
                }
            }
            //seleccionamos la posicion mas cercana a la tarea actual
            Posicion posicion = posiciones.PosicionMasCercana(tarea);
            return posicion;
        }


        /// <summary>
        /// Devuelve el conjunto de posiciones donde se encuentra el producto del mismo lote
        /// Para obtener todas las posiciones, usar GetListado()
        /// </summary>
        /// <param name="lote"></param>
        /// <returns></returns>
        public Posiciones FiltrarPosicionesPorLotes(string lote)
        {
            Posiciones ps = new Posiciones();
            ps.Agregar(from Posicion posicion in this.posiciones.listado where (posicion.lote.Equals(lote) && posicion.cantidad > 0) select posicion);
            return ps;
        }

        /// <summary>
        /// Devuelve el conjunto de posiciones ordenadas por proximidad,
        /// con preferencia por el lote mas antiguo, que contengan al menos la cantidad requerida
        /// </summary>
        /// <param name="cantidad"></param>
        /// <returns></returns>
        public Posiciones ObtenerPosiciones(int cantidad, bool soloLoteAntiguo = false)
        {
            Posiciones posiciones = new Posiciones();
            //Obtenemos el lote mas antiguo
            List<string> lotes = this.posiciones.ObtenerLotes();
            int cantidadAcumulada = 0;
            int contadorLotes = 0;
            //recorremos las posiciones desde el mas antiguo
            foreach(string lote in lotes)
            {
                //recorremos las posiciones con el lote indicado
                foreach (Posicion pos in this.posiciones.BuscarProducto(this.codigo, lote))
                {
                    if (pos.Disponible() > 0)
                    {
                        cantidadAcumulada += pos.Disponible();
                        posiciones.Agregar(pos);
                        if (cantidadAcumulada >= cantidad)
                        {
                            return posiciones;
                        }
                    }
                }
                if (soloLoteAntiguo && contadorLotes == 0)
                {
                    return posiciones;
                }
                contadorLotes++;
            }
            //aqui va la logica de orden en funcion de la posicion actual del operario.
            return posiciones;
        }
    }
}