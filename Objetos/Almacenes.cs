using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace NoCocinoMas
{
    public class Almacenes : Entidades, IEntidades
    {

        /// <summary>
        /// Vincula los modulos correspondientes a cada almacen
        /// </summary>
        /// <param name="modulos"></param>
        public void VincularModulos(Modulos modulos)
        {
            foreach(Almacen a in this.listado)
            {
                a.VincularModulos(modulos.Filtrar(a));
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
                this.listado.Add(new Almacen(resultado));
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
            Almacen a = new Almacen(parametros);
            id = ConectorSQL.CrearEntidades(ConectorSQL.insertAlmacen, a.GetValoresInsertSQL());
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

    public class Almacen : Entidad, IEntidad
    {
        public int numero { get; set; }
        public string nombre { get; set; }
        //objetos
        [JsonIgnore]
        public Modulos modulos { get; set; }

        public Almacen(Parametros parametros)
        {
            this.id = parametros.BuscarInt("id");
            this.numero = parametros.BuscarInt("numero");
            this.nombre = parametros.Buscar("nombre");
        }
        public Almacen(int id, int numero, string nombre)
        {
            this.id = id;
            this.numero = numero;
            this.nombre = nombre;
            this.modulos = new Modulos();
        }
        public Almacen(MySqlDataReader datos)
        {
            this.id = datos.GetInt32("id");
            this.numero = datos.GetInt32("numero");
            this.nombre = datos.GetString("nombre");
            this.modulos = new Modulos();
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
                this.nombre
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
                this.nombre
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
            return string.Format("({0},'{1}')", this.numero, this.nombre);
        }

        /// <summary>
        /// Esta funcion actualiza directamente en la BBDD
        /// Devuelve el resultado.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public Respuesta UpdateSQL(Parametros parametros)
        {
            Respuesta r = Respuesta.Crear(false, 0, (Almacen)this);
            int id = parametros.BuscarInt("id");
            int numero = parametros.BuscarInt("numero");
            string nombre = parametros.Buscar("nombre");
            object[] valores = {
                id,
                numero,
                nombre
            };
            if (!this.Comparar(id))
            {
                r.Error("ID no coincidente");
            }
            else
            {
                bool ok = ConectorSQL.ActualizarEntidades(ConectorSQL.updateAlmacen, valores);
                if (ok)
                {
                    this.numero = numero;
                    this.nombre = nombre;
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
        /// Agrega el listado de modulos enviado a los existentes
        /// </summary>
        /// <param name="modulos"></param>
        public void VincularModulos(IEnumerable<Modulo> modulos)
        {
            this.modulos.Agregar(modulos);
            foreach (Modulo m in modulos)
            {
                m.almacen = this;
            }
        }

        public Posiciones BuscarProducto(int id_producto, string lote)
        {
            return this.modulos.BuscarProducto(id_producto, lote);
        }
        public Posiciones BuscarVacias()
        {
            return this.modulos.BuscarVacias();
        }

    }
}
