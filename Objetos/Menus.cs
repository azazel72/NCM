using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace NoCocinoMas
{
    public class Menus : Entidades, IEntidades
    {
        /// <summary>
        /// Agregamos el conjunto de objetos de una consulta MySql
        /// </summary>
        /// <param name="resultado"></param>
        public void Agregar(MySqlDataReader resultado)
        {
            while (resultado.Read())
            {
                this.listado.Add(new Menu(resultado));
            }
        }

        public Respuesta InsertSQL(Parametros parametros)
        {
            throw new NotImplementedException();
        }

        public LineasPedido SustituirMenus(LineaPedido linea)
        {
            LineasPedido lineas = new LineasPedido();
            foreach (Menu menu in this.listado)
            {
                if (menu.menu_id == linea.producto_codigo)
                {
                    LineaPedido l = new LineaPedido(linea, menu);
                    lineas.Agregar(l);
                }
            }
            return lineas;
        }

        public void VincularProductos(Dictionary<int, Producto> listadoProductos)
        {
            foreach (Menu menu in this.listado)
            {
                listadoProductos.TryGetValue(menu.products_id, out Producto p);
                //if (p == null) Console.WriteLine("Producto no encontrado: " + menu.products_id);
                menu.producto = p ?? Gestor.productoNoEncontrado;
            }
        }
    }

    public class Menu : Entidad, IEntidad
    {
        public int menu_id { get; set; }
        public int products_id { get; set; }
        public int products_quantity { get; set; }
        [JsonIgnore]
        public Producto producto { get; set; }

        public Menu(MySqlDataReader datos)
        {
            this.id = datos.GetInt32("menu_id");
            this.menu_id = datos.GetInt32("menu_id");
            this.products_id = datos.GetInt32("products_id");
            this.products_quantity = datos.GetInt32("products_quantity");
        }

        public string GetValoresInsertSQL()
        {
            throw new NotImplementedException();
        }

        public Respuesta UpdateSQL(Parametros parametros)
        {
            throw new NotImplementedException();
        }

        public object[] GetValores()
        {
            throw new NotImplementedException();
        }

        public object[] GetValoresTablas()
        {
            throw new NotImplementedException();
        }
    }
}
