using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace NoCocinoMas
{
    public class Modulos : Entidades, IEntidades
    {
        /// <summary>
        /// Filtra los modulos con almacen coincidente
        /// </summary>
        /// <param name="almacen"></param>
        public IEnumerable<Modulo> Filtrar(Almacen almacen)
        {
            return from Modulo modulo in this.listado where modulo.almacen_id.Equals(almacen.id) select modulo;
        }

        /// <summary>
        /// Vincula las posiciones correspondientes a cada modulo
        /// </summary>
        /// <param name="posiciones"></param>
        public void VincularPosiciones(Posiciones posiciones)
        {
            foreach (Modulo m in this.listado)
            {
                m.VincularPosiciones(posiciones.Filtrar(m));
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
                this.listado.Add(new Modulo(resultado));
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
            Modulo a = new Modulo(parametros);
            id = ConectorSQL.CrearEntidades(ConectorSQL.insertModulo, a.GetValoresInsertSQL());
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

        public Modulo BuscarAlias(string alias)
        {
            int.TryParse(alias, out int a);
            if (a == 0)
            {
                return null;
            }
            foreach (Modulo m in this.listado)
            {
                if (m.alias == a)
                {
                    return m;
                }
            }
            return null;
        }

        /// <summary>
        /// Esta funcion devuelve las posiciones donde se encuentre un producto,
        /// </summary>
        /// <param name="producto"></param>
        /// <param name="cantidad"></param>
        /// <param name="vacio"></param>
        /// <returns></returns>
        public Posiciones BuscarProducto(int id_producto, string lote = "")
        {
            Posiciones posiciones = new Posiciones();
            foreach (Modulo modulo in this.listado)
            {
                posiciones.Agregar(modulo.BuscarProducto(id_producto, lote));
            }
            return posiciones;
        }
        public Posiciones BuscarVacias()
        {
            Posiciones posiciones = new Posiciones();
            foreach (Modulo modulo in this.listado)
            {
                posiciones.Agregar(modulo.BuscarVacias());
            }
            return posiciones;
        }
    }

    public class Modulo : Entidad, IEntidad
    {
        public int alias { get; set; }
        public string nombre { get; set; }
        public int almacen_id { get; set; }
        public string ip { get; set; }
        public int preferencia_reposicion { get; set; }
        //objetos
        [JsonIgnore]
        public Posiciones posiciones { get; set; }
        [JsonIgnore]
        public Almacen almacen { get; set; }
        [JsonIgnore]
        public Tarea tareaBloqueante { get; set; }
        private bool bloqueado { get; set; }

        public Modulo(Parametros parametros)
        {
            this.id = parametros.BuscarInt("id");
            this.alias = parametros.BuscarInt("alias");
            this.nombre = parametros.Buscar("nombre");
            this.almacen_id = parametros.BuscarInt("almacen_id");
            this.ip = "";
            this.preferencia_reposicion = parametros.BuscarInt("preferencia_reposicion");
        }
        public Modulo(int id, int alias, string nombre, int almacen_id)
        {
            this.id = id;
            this.alias = alias;
            this.nombre = nombre;
            this.almacen_id = almacen_id;
            this.ip = "";
            this.posiciones = new Posiciones();
        }
        public Modulo(MySqlDataReader datos)
        {
            this.id = datos.GetInt32("id");
            this.alias = datos.GetInt32("alias");
            this.nombre = datos.GetString("nombre");
            this.almacen_id = datos.GetInt32("almacen_id");
            this.ip = "";
            this.preferencia_reposicion = datos.GetInt32("preferencia_reposicion");
            this.posiciones = new Posiciones();
        }

        /// <summary>
        /// Devuelve un array de objetos con los valores.
        /// </summary>
        /// <returns></returns>
        public object[] GetValores()
        {
            object[] valores = {
                this.id,
                this.alias,
                this.nombre,
                this.almacen_id,
                this.ip,
                this.preferencia_reposicion
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
                this.alias,
                this.nombre,
                this.almacen_id,
                this.ip,
                this.preferencia_reposicion
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
            return string.Format("('{0}','{1}',{2},{3})",
                this.alias,
                this.nombre,
                this.almacen_id,
                this.preferencia_reposicion
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
            Respuesta r = Respuesta.Crear(false, 0, (Modulo)this);
            int id = parametros.BuscarInt("id");
            int alias = parametros.BuscarInt("alias");
            string nombre = parametros.Buscar("nombre");
            int almacen_id = parametros.BuscarInt("almacen_id");
            int preferencia_reposicion = parametros.BuscarInt("preferencia_reposicion");
            object[] valores = {
                id,
                alias,
                nombre,
                almacen_id,
                preferencia_reposicion
            };
            if (!this.Comparar(id))
            {
                r.Error("ID no coincidente");
            }
            else
            {
                bool ok = ConectorSQL.ActualizarEntidades(ConectorSQL.updateModulo, valores);
                if (ok)
                {
                    this.alias = alias;
                    this.nombre = nombre;
                    this.almacen_id = almacen_id;
                    this.preferencia_reposicion = preferencia_reposicion;
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
        /// Agrega el listado de posiciones enviado a las existentes
        /// </summary>
        /// <param name="posiciones"></param>
        public void VincularPosiciones(IEnumerable<Posicion> posiciones)
        {
            this.posiciones.Agregar(posiciones);
            foreach(Posicion p in posiciones)
            {
                p.modulo = this;
            }
        }

        public Posiciones BuscarProducto(int id_producto, string lote)
        {
            return this.posiciones.BuscarProducto(id_producto, lote);
        }
        public Posiciones BuscarVacias()
        {
            return this.posiciones.BuscarVacias();
        }

        /// <summary>
        /// Vincula la tarea que bloquea el modulo, si éste está libre
        /// y desvincula el modulo que estuviera previamente vinculado a esa tarea
        /// </summary>
        /// <param name="tarea"></param>
        public bool VincularTarea(Tarea tarea)
        {
            if (this.tareaBloqueante == tarea)
            {
                return true;
            }
            if (tarea.moduloActual != null && tarea.moduloActual != this)
            {
                Console.WriteLine("> Vincular Tarea (modulo tarea, esta tarea)<");
                Console.WriteLine(tarea.moduloActual.id);
                Console.WriteLine(this.id);
                Console.WriteLine("><");
                //latencia para desvincular la tarea
                tarea.gestor.controlModulos.PosponerLiberacion(tarea.moduloActual, tarea, this);
            }
            tarea.moduloActual = this;
            tarea.gestor.ActualizarTablaTareas(tarea);
            if (this.bloqueado)
            {
                return false;
            }
            this.tareaBloqueante = tarea;
            this.bloqueado = true;
            tarea.gestor.ActualizarTablaModulos(this);
            return true;
        }

        /// <summary>
        /// Desconecta la tarea que bloquea el modulo si el parametro es nulo y no esta bloqueada
        /// </summary>
        /// <param name="tarea"></param>
        public void DesvincularTarea(Tarea tarea)
        {
            if (tarea.moduloActual == this)
            {
                tarea.moduloActual = null;
                tarea.gestor.ActualizarTablaTareas(tarea);
            }
            if (this.tareaBloqueante == tarea)
            {
                this.tareaBloqueante = null;
                this.bloqueado = false;
                tarea.gestor.ActualizarTablaModulos(this);
                //la tareaActual es la presente, ya que se llama desde this.tareaActual.Desvincular(this)
                tarea.gestor.tareas.ModuloLiberado(this);
            }
        }

        public bool EstaBloqueado(Tarea tarea)
        {
            return this.bloqueado && this.tareaBloqueante != tarea;
        }

    }
}
