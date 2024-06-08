using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NoCocinoMas
{
    public abstract class Entidades : IEnumerable
    {
        //para cambiar el nombre de un atributo
        //[JsonPropertyName("slitter")]
        public List<Entidad> listado;

        public Entidades()
        {
            this.listado = new List<Entidad>();
        }

        public int Contador()
        {
            return this.listado.Count;
        }

        /// <summary>
        /// Dejamos el listado vacio
        /// </summary>
        public void Vaciar()
        {
            this.listado.Clear();
        }

        /// <summary>
        /// Agregamos un objeto al listado
        /// </summary>
        /// <param name="e"></param>
        public void Agregar(Entidad e)
        {
            if (e != null)
            {
                this.listado.Add(e);
            }
        }

        public void Eliminar(Entidad e)
        {
            if (e != null)
            {
                this.listado.Remove(e);
            }
        }

        /// <summary>
        /// Agregamos a la lista la lista pasada como parametro
        /// </summary>
        /// <param name="productos"></param>
        public void Agregar(Entidades e)
        {
            this.listado.AddRange(e.listado);
        }

        /// <summary>
        /// Agregamos el resultado de un Select Linq a la lista
        /// </summary>
        /// <param name=""></param>
        public void Agregar(IEnumerable<Entidad> e)
        {
            this.listado.AddRange(e);
        }
                
/*        /// <summary>
        /// Constructor de T a traves de una funcion static de este
        /// tambien paso de funciones como parametro -> Action<string, object, ...> funcion
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resultado"></param>
        public void Agregar<T>(MySqlDataReader resultado) where T : IEntidad, new()
        {
            while (resultado.Read())
            {
                //this.listado.Add(T.Crear(resultado));
            }
        }
*/
        /// <summary>
        /// Agrega un elemento a la lista si este no existe en ella
        /// </summary>
        /// <param name="e"></param>
        public void AgregarUnico(Entidad e)
        {
            if (!this.listado.Contains(e))
            {
                this.listado.Add(e);
            }
        }

        /// <summary>
        /// Necesario para los Selects de Linq
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return this.listado.GetEnumerator();
        }

        /// <summary>
        /// Retorna el listado de objetos
        /// </summary>
        /// <returns></returns>
        public List<Entidad> GetListado()
        {
            return this.listado;
        }

        /// <summary>
        /// Busca el identificador equivalente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entidad BuscarId(object id)
        {
            if (id != null)
            {
                foreach (Entidad e in this.listado)
                {
                    if (e.Comparar(id))
                    {
                        return e;
                    }
                }
            }
            return null;
        }

        public bool ExisteId(object id)
        {
            return this.listado?.Exists(e => e.Comparar(id)) ?? false;
        }

        /// <summary>
        /// Elimina una lista de elementos del listado
        /// </summary>
        /// <param name="es"></param>
        public void Eliminar(Entidades es)
        {
            foreach (Entidad e in es)
            {
                this.listado.Remove(e);
            }            
        }

        /// <summary>
        /// Elimina el elemento con el mismo id del listado
        /// </summary>
        /// <param name="id"></param>
        public void Eliminar(object id)
        {
            this.listado.Remove(this.BuscarId(id));
        }

        public string ValoresSQL()
        {
            string s = "";
            foreach (IEntidad e in this.listado)
            {
                s += e.GetValoresInsertSQL() + ",";
            }
            return s.TrimEnd(',');
        }

    }

    public abstract class Entidad
    {
        public object id { get; set; }

        public bool Comparar(object id)
        {
            return this.id.Equals(id);
        }
    }

}
