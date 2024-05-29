using MySql.Data.MySqlClient;
using System;

namespace NoCocinoMas
{
    public class Configuracion : Entidades, IEntidades
    {

        public string carpetaPDFEnvios { get; set; }
        public string impresoraEnvios { get; set; }


        /// <summary>
        /// Agregamos el conjunto de objetos de una consulta MySql
        /// </summary>
        /// <param name="resultado"></param>
        public void Agregar(MySqlDataReader resultado)
        {
            if (resultado.Read())
            {
                this.carpetaPDFEnvios = resultado.GetString("carpeta_pdf_envios");
                this.impresoraEnvios = resultado.GetString("impresora_envios");
            }
        }

        /// <summary>
        /// Crea un elemento en la BBDD y lo agrega al listado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public Respuesta InsertSQL(Parametros parametros)
        {
            return new Respuesta();
        }

        /// <summary>
        /// Devuelve un array de objetos con los valores.
        /// Adaptado para crear consultas SQL
        /// </summary>
        /// <returns></returns>
        public string GetValoresInsertSQL()
        {
            return string.Format("('{0}','{1}')",
                this.carpetaPDFEnvios,
                this.impresoraEnvios
            );
        }

    }
}
