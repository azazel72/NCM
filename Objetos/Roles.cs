using System.Collections.Generic;
using System.Linq;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Text.Json.Serialization;
using System;

namespace NoCocinoMas
{
    public class Roles : Entidades, IEntidades
    {

        /// <summary>
        /// Agregamos el conjunto de objetos de una consulta MySql
        /// </summary>
        /// <param name="resultado"></param>
        public void Agregar(MySqlDataReader resultado)
        {
            while (resultado.Read())
            {
                this.listado.Add(new Rol(resultado));
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
            Rol a = new Rol(parametros);
            id = ConectorSQL.CrearEntidades(ConectorSQL.insertRol, a.GetValoresInsertSQL());
            if (id > 0)
            {
                a.id = id;
                this.Agregar(a);
                r.entidad = a;
            }
            else
            {
                r.Error("Insert SQL falló.");
            }
            return r;
        }

    }

    public class Rol : Entidad, IEntidad
    {
        public string nombre { get; set; }
        public int acceso_edicion { get; set; }
        public int acceso_reposicion { get; set; }

        public Rol(Parametros parametros)
        {
            this.id = parametros.BuscarInt("id");
            this.nombre = parametros.Buscar("nombre");
            this.acceso_edicion = parametros.BuscarInt("acceso_edicion");
            this.acceso_reposicion = parametros.BuscarInt("acceso_reposicion");
        }
        public Rol(int id, string nombre, int acceso_edicion, int acceso_reposicion)
        {
            this.id = id;
            this.nombre = nombre;
            this.acceso_edicion = acceso_edicion;
            this.acceso_reposicion = acceso_reposicion;
        }
        public Rol(MySqlDataReader datos)
        {
            this.id = datos.GetInt32("id");
            this.nombre = datos.GetString("nombre");
            this.acceso_edicion = datos.GetInt32("acceso_edicion");
            this.acceso_reposicion = datos.GetInt32("acceso_reposicion");
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
                this.acceso_edicion,
                this.acceso_reposicion
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
                this.acceso_edicion,
                this.acceso_reposicion
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
            return string.Format("('{0}', {1}, {2})",
                this.nombre,
                this.acceso_edicion,
                this.acceso_reposicion
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
            Respuesta r = Respuesta.Crear(false, 0, (Rol)this);
            int id = parametros.BuscarInt("id");
            string nombre = parametros.Buscar("nombre");
            int acceso_edicion = parametros.BuscarInt("acceso_edicion");
            int acceso_reposicion = parametros.BuscarInt("acceso_reposicion");
            object[] valores = {
                id,
                nombre,
                acceso_edicion,
                acceso_reposicion
            };
            if (!this.Comparar(id))
            {
                r.Error("ID no coincidente");
            }
            else
            {
                bool ok = ConectorSQL.ActualizarEntidades(ConectorSQL.updateRol, valores);
                if (ok)
                {
                    this.nombre = nombre;
                    this.acceso_edicion = acceso_edicion;
                    this.acceso_reposicion = acceso_reposicion;
                }
                else
                {
                    r.Error("Actualización SQL falló.");
                }
            }
            return r;
        }

    }
}
