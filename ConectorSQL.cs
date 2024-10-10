using Google.Protobuf.WellKnownTypes;
using iText.StyledXmlParser.Jsoup.Select;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using static Common.Logging.Configuration.ArgUtils;

namespace NoCocinoMas
{
    class ConectorSQL
    {
        //Gestor
        static public Gestor gestor { get; set; }

        //bloqueo SQL
        static private readonly object bloqueoSQL = new object();

        //Conexion
        static private string cadenaConexion = "datasource=127.0.0.1;port=3306;database=nococinomas;username=root;password=;Allow Zero Datetime=True;Convert Zero Datetime=True;";
        static public string cadenaConexionPS = "datasource=www.nococinomas.es;port=3306;database=nococinomas;username=nokocino;password=123noko456;Allow Zero Datetime=True;Convert Zero Datetime=True;";

        //SELECTS
        static public string selectAlmacenes = "SELECT * FROM almacenes";
        static public string selectModulos = "SELECT * FROM modulos";
        static public string selectPosiciones = "SELECT * FROM posiciones WHERE modulo_id > 0 AND estado > -1";
        static public string selectProductos = "SELECT * FROM productos";
        static public string selectEnvases = "SELECT * FROM envases";
        static public string selectUbicaciones = "SELECT * FROM ubicaciones";
        static public string selectCentralitas = "SELECT * FROM centralitas";
        static public string selectOperarios = "SELECT * FROM operarios";
        static public string selectRoles = "SELECT * FROM roles";
        static public string selectPedidosFiltrado = "SELECT * FROM pedidos WHERE envio > '{0}' ORDER BY indice_recogida ASC, numero ASC";
        static public string selectLineasPedidoFiltrado = "SELECT * FROM lineas_pedido WHERE pedido_numero IN ({0}) ORDER BY pedido_numero";
        static public string selectBloqueos = "SELECT * FROM bloqueo";
        static public string selectMovimientos = "SELECT * FROM movimientos";
        static public string selectMovimientosFiltrado = "SELECT * FROM movimientos WHERE pedido_numero IN ({0}) ORDER BY pedido_numero";

        static public string selectConfiguracion = "SELECT * FROM configuracion";
        static public string selectPedido = "SELECT 1 FROM pedidos WHERE numero={0}";



        //CONSULTAS PS
        static public string obtenerPedidoPS = "SELECT o.id_order as numero, ad.postcode as cp, o.date_upd as fecha, 0 as estado, dd.tramo_date as envio, t.id_reference as transportista FROM ps_orders o join ps_cart dd on (dd.id_cart=o.id_cart) join ps_address ad on o.id_address_delivery=ad.id_address JOIN ps_carrier t on o.id_carrier=t.id_carrier WHERE o.id_order = {0} AND current_state in (2,3,4,15,16,20,23) ORDER BY numero ASC";
        static public string obtenerPedidosPS = "SELECT o.id_order as numero, ad.postcode as cp, o.date_upd as fecha, 0 as estado, dd.tramo_date as envio, t.id_reference as transportista FROM ps_orders o join ps_cart dd on (dd.id_cart=o.id_cart) join ps_address ad on o.id_address_delivery=ad.id_address JOIN ps_carrier t on o.id_carrier=t.id_carrier WHERE dd.tramo_date >= '{0}' AND dd.tramo_date <= '{1}' AND current_state in (2,3,4,15,16,20,23) ORDER BY numero ASC";
        static public string obtenerLineasPedidoPS = "SELECT 0 as id, od.id_order AS pedido_numero, od.product_id AS producto_codigo, round(od.product_quantity,2) AS cantidad, 0 as recogido, 0 as estado FROM ps_order_detail od WHERE od.product_id < 999999999 AND od.id_order in ({0}) ORDER BY od.id_order ASC";
        static public string obtenerMenusPS = "SELECT id_product_pack AS menu_id, id_product_item AS products_id, quantity AS products_quantity FROM ps_pack";
        static public string obtenerProductosPS = "SELECT pd.id_product AS id, pd.id_product AS codigo, pd.name AS nombre, 1 as envase_id, 0 as stock, ppt.posicion_clasif AS posicionRecogida, ppt.posicion_almac AS posicionAlmacenamiento FROM ps_product_lang pd join ps_product p on (pd.id_product=p.id_product and active=1) LEFT JOIN ps_product_ptl ppt ON pd.id_product = ppt.id_product;";

        static public string vaciarStock = "TRUNCATE ps_stock_web;";
        static public string sobreescribirStock = "INSERT INTO ps_stock_web (products_id, fechaLote, cantidad) VALUES ({0}, '{1}', {2}) ON DUPLICATE KEY UPDATE cantidad=cantidad-{2};";
        static public string insertarTrazabilidad = "INSERT INTO ps_trazabilidad (id_order, id_product, lote, cantidad) VALUES ({0}, {1}, '{2}', {3});";
        static public string obtenerTransportistasPS = "SELECT id_carrier, name FROM ps_carrier;";

        //INSERT
        static public string guardarProductos = "INSERT INTO productos (codigo, nombre, envase_id, stock) VALUES {0};";
        static public string guardarPedidos = "INSERT INTO pedidos (id, numero, fecha, estado, cp, envio, transportista, caja, indice_modulo, indice_recogida) VALUES {0};";
        static public string guardarLineasPedido = "INSERT INTO lineas_pedido (pedido_numero, producto_codigo, cantidad, recogido, estado) VALUES {0};";

        static public string insertOperario = "INSERT INTO operarios (nombre, rol_id, pin) VALUES {0};";
        static public string insertRol = "INSERT INTO roles (nombre, acceso_edicion, acceso_reposicion) VALUES {0};";
        static public string insertCentralita = "INSERT INTO centralitas (alias, nombre) VALUES {0};";
        static public string insertAlmacen = "INSERT INTO almacenes (numero, nombre) VALUES {0};";
        static public string insertModulo = "INSERT INTO modulos (alias, nombre, almacen_id, preferencia_reposicion) VALUES {0};";
        static public string insertPosicion = "INSERT INTO posiciones (nombre, balda, modulo_id, centralita_id, linea, led_inicial, led_longitud, producto_codigo, cantidad, lote, capacidad, envase, estado) VALUES {0};";
        static public string insertProducto = "INSERT INTO productos (codigo, nombre, envase_id, stock) VALUES {0};";
        static public string insertEnvase = "INSERT INTO envases (nombre, ancho, largo, alto, volumen, peso) VALUES {0};";
        static public string insertUbicacion = "INSERT INTO ubicaciones (nombre, linea, inicio, ancho) VALUES {0};";
        static public string insertPedido = "INSERT INTO pedidos (numero, estado, fecha, cp, envio, transportista, caja, indice_modulo, indice_recogida) VALUES {0};";
        static public string insertLineaPedido = "INSERT INTO lineas_pedido (pedido_numero, producto_codigo, cantidad, recogido, estado) VALUES {0};";
        static public string insertMovimiento = "INSERT INTO movimientos (operario_id, producto_codigo, lote, cantidad, posicion_nombre, pedido_numero, linea_pedido_id, accion) VALUES {0};";

        //UPDATE
        static public string updateOperario = "UPDATE operarios SET nombre='{1}', rol_id={2}, pin='{3}' WHERE id={0};";
        static public string updateRol = "UPDATE roles SET nombre='{1}', acceso_edicion={2}, acceso_reposicion={3} WHERE id={0};";
        static public string updateCentralita = "UPDATE centralitas SET alias='{1}', nombre='{2}' WHERE id={0};";
        static public string updateAlmacen = "UPDATE almacenes SET numero={1}, nombre='{2}' WHERE id={0};";
        static public string updateModulo = "UPDATE modulos SET alias='{1}', nombre='{2}', almacen_id={3}, preferencia_reposicion={4} WHERE id={0};";
        static public string updatePosicion = "UPDATE posiciones SET nombre='{1}', balda={2}, modulo_id={3}, centralita_id={4}, linea={5}, led_inicial={6}, led_longitud={7}, producto_codigo={8}, cantidad={9}, lote='{10}', capacidad={11}, envase={12}, estado='{13}' WHERE id={0};";
        static public string updateProducto = "UPDATE productos SET codigo={1}, nombre='{2}', envase_id={3}, stock={4} WHERE id={0};";
        static public string updateEnvase = "UPDATE envases SET nombre='{1}', ancho={2}, largo={3}, alto={4}, volumen={5}, peso={6} WHERE id={0};";
        static public string updateUbicacion = "UPDATE ubicaciones SET nombre='{1}', linea={2}, inicio={3}, ancho={4} WHERE id={0};";
        static public string updatePedido = "UPDATE pedidos SET numero={1}, fecha='{2}', estado={3}, cp='{4}', envio='{5}', transportista={6} WHERE id={0};";
        static public string updateLineaPedido = "UPDATE lineas_pedido SET cantidad={1}, recogido={2}, estado={3} WHERE id={0};";

        static public string updateRecogida = "UPDATE pedidos SET caja='{1}', indice_modulo={2}, indice_recogida={3} WHERE id={0};";


        static public string updateReposicion = "UPDATE posiciones SET producto_codigo={1}, cantidad={2}, lote='{3}' WHERE id={0};";
        static public string updateRecogidaLineas = "UPDATE lineas_pedido SET recogido={1}, estado={2} WHERE id={0};";
        static public string updateRecogidaPedido = "UPDATE pedidos SET estado={1} WHERE id={0};";

        static public string updateFabricacion = "UPDATE productos SET stock={1} WHERE id={0};";

        //static public string updateConfiguracion = "UPDATE configuracion SET carpeta_pdf_envios='{0}', impresora_envios='{1}' WHERE id=1;";
        static public string updateConfiguracion = "INSERT INTO configuracion(id, carpeta_pdf_envios, impresora_envios) VALUES(1, '{0}', '{1}') ON DUPLICATE KEY UPDATE carpeta_pdf_envios='{0}', impresora_envios='{1}';";
        //DELETE
        static public string deleteEntidad = "DELETE FROM {0} WHERE id={1};";
        static public string deleteEntidades = "DELETE FROM {0} WHERE id IN ({1});";

        /// <summary>
        /// Comprobamos si es posible realizar la conexion
        /// </summary>
        /// <returns></returns>
        static public bool Comprobar(string cadenaConexion = null)
        {
            try
            {
                if (cadenaConexion == null)
                {
                    cadenaConexion = ConectorSQL.cadenaConexion;
                }
                MySqlConnection conexion = new MySqlConnection(cadenaConexion);
                conexion.Open();
                conexion.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                string error = "Error (conexion bbdd): " + ex.Number.ToString() + " - " + ex.Message;
                gestor.EscribirError(error);
                return false;
            }
        }

        /// <summary>
        /// Funcion para cargar entidades desde la bbdd
        /// </summary>
        /// <param name="cadenaConsulta"></param>
        /// <param name="entidad"></param>
        static public void CargarEntidades(string cadenaConsulta, IEntidades entidades, bool actualizar = true, string cadenaConexion = null)
        {
            try
            {
                if (cadenaConexion == null)
                {
                    cadenaConexion = ConectorSQL.cadenaConexion;
                }
                if (actualizar)
                {
                    entidades.Vaciar();
                }
                MySqlConnection conexion = new MySqlConnection(cadenaConexion);
                MySqlCommand consulta = conexion.CreateCommand();
                consulta.CommandText = cadenaConsulta;
                conexion.Open();
                MySqlDataReader resultado = consulta.ExecuteReader();

                if (resultado.HasRows)
                {
                    entidades.Agregar(resultado);
                }
                else
                {
                    Console.WriteLine("No se encontraron entidades.");
                }
                conexion.Close();
            }
            catch (MySqlException ex)
            {
                string error = "Error (entidades): " + ex.Number.ToString() + " - " + ex.Message + "\n" + cadenaConsulta;
                gestor.EscribirError(error);
            }
        }

        static public bool ComprobarPedido(string cadenaConsulta, string cadenaConexion = null)
        {
            try
            {
                if (cadenaConexion == null)
                {
                    cadenaConexion = ConectorSQL.cadenaConexion;
                }
                MySqlConnection conexion = new MySqlConnection(cadenaConexion);
                MySqlCommand consulta = conexion.CreateCommand();
                consulta.CommandText = cadenaConsulta;
                conexion.Open();
                MySqlDataReader resultado = consulta.ExecuteReader();

                if (resultado.HasRows)
                {
                    return true;
                }
                conexion.Close();
            }
            catch (MySqlException ex)
            {
                string error = "Error (entidades): " + ex.Number.ToString() + " - " + ex.Message + "\n" + cadenaConsulta;
                gestor.EscribirError(error);
            }
            return false;
        }

        /// <summary>
        /// Funcion para actualizar tablas
        /// </summary>
        /// <param name="cadenaConsulta"></param>
        /// <param name="argumentos"></param>
        static public bool ActualizarEntidades(string cadenaConsulta, object[] argumentos, string cadenaConexion = null)
        {
            try
            {
                if (cadenaConexion == null)
                {
                    cadenaConexion = ConectorSQL.cadenaConexion;
                }
                MySqlConnection conexion = new MySqlConnection(cadenaConexion);
                MySqlCommand consulta = conexion.CreateCommand();
                consulta.CommandText = string.Format(cadenaConsulta, argumentos);

                gestor.EscribirError(consulta.CommandText);

                conexion.Open();
                lock (bloqueoSQL)
                {
                    consulta.ExecuteNonQuery();
                    consulta.Dispose();
                }
                conexion.Close();
                return true;
            }
            catch (Exception ex)
            {
                string error = "Error (Actualizar entidades): " + ex.Message;
                gestor.EscribirError(error);
                return false;
            }
        }

        /// <summary>
        /// Crea una entidad en la BBDD y retorna el primer valor obtenido, presuntamente la ID.
        /// </summary>
        /// <param name="cadenaConsulta"></param>
        /// <param name="valores"></param>
        /// <returns></returns>
        static public int CrearEntidades(string cadenaConsulta, string valores, string cadenaConexion = null)
        {
            try
            {
                if (cadenaConexion == null)
                {
                    cadenaConexion = ConectorSQL.cadenaConexion;
                }
                MySqlConnection conexion = new MySqlConnection(cadenaConexion);
                MySqlCommand consulta = conexion.CreateCommand();
                consulta.CommandText = string.Format(cadenaConsulta, valores) + "SELECT LAST_INSERT_ID();";
                Console.WriteLine(consulta.CommandText);
                conexion.Open();
                MySqlDataReader resultado;
                lock (bloqueoSQL)
                {
                    resultado = consulta.ExecuteReader();
                }
                if (!resultado.HasRows)
                {
                    conexion.Close();
                }

                resultado.Read();
                int id = resultado.GetInt32(0);

                consulta.Dispose();
                conexion.Close();
                return id;
            }
            catch (Exception ex)
            {
                string error = "Error (Crear entidades): " + ex.Message;
                gestor.EscribirError(error);
                return -1;
            }
        }

        /// <summary>
        /// Funcion que eliminar una entidad de una tabla por su id
        /// </summary>
        /// <param name="entidad"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        static public bool DeleteSQL(string entidad, int id)
        {
            try
            {
                MySqlConnection conexion = new MySqlConnection(cadenaConexion);
                MySqlCommand consulta = conexion.CreateCommand();
                consulta.CommandText = string.Format(deleteEntidad, entidad, id);
                Console.WriteLine(consulta.CommandText);
                conexion.Open();
                lock (bloqueoSQL)
                {
                    consulta.ExecuteNonQuery();
                    consulta.Dispose();
                }
                conexion.Close();
                return true;
            }
            catch (Exception ex)
            {
                //string error = "Error (actualizar entidades): " + ex.Number.ToString() + " - " + ex.Message;
                string error = "Error (actualizar entidades): " + ex.Message;
                gestor.EscribirError(error);
                return false;
            }
        }

        static public void GuardarEntidades(string cadenaConsulta, IEntidades e, string cadenaConexion = null)
        {
            if (e == null)
            {
                return;
            }
            string comando = "";
            try
            {
                if (cadenaConexion == null)
                {
                    cadenaConexion = ConectorSQL.cadenaConexion;
                }
                MySqlConnection conexion = new MySqlConnection(cadenaConexion);
                MySqlCommand consulta = conexion.CreateCommand();
                //crear el listado de valores
                comando = string.Format(cadenaConsulta, e.ValoresSQL());
                consulta.CommandText = comando;
                conexion.Open();
                consulta.ExecuteNonQuery();
                consulta.Dispose();
                conexion.Close();
            }
            catch (MySqlException ex)
            {
                string error = "Error (Guardar entidades): " + ex.Number.ToString() + " - " + ex.Message;
                gestor.EscribirError(error);
                gestor.EscribirError(comando);
            }
        }

        static public int GuardarEntidad(string cadenaConsulta, IEntidad e, string cadenaConexion = null)
        {
            if (e == null)
            {
                return 0;
            }
            string comando = "";
            try
            {
                if (cadenaConexion == null)
                {
                    cadenaConexion = ConectorSQL.cadenaConexion;
                }
                MySqlConnection conexion = new MySqlConnection(cadenaConexion);
                MySqlCommand consulta = conexion.CreateCommand();
                MySqlDataReader resultado;
                //crear el listado de valores
                comando = string.Format(cadenaConsulta, e.GetValoresInsertSQL()) + "SELECT LAST_INSERT_ID();";
                consulta.CommandText = comando;
                conexion.Open();
                lock (bloqueoSQL)
                {
                    resultado = consulta.ExecuteReader();
                }
                if (!resultado.HasRows)
                {
                    conexion.Close();
                }

                resultado.Read();
                int id = resultado.GetInt32(0);

                consulta.Dispose();
                conexion.Close();
                return id;
            }
            catch (MySqlException ex)
            {
                string error = "Error (Guardar entidad): " + ex.Number.ToString() + " - " + ex.Message;
                gestor.EscribirError(error);
                gestor.EscribirError(comando);
            }
            return 0;
        }

        static public void EjecutarComando(string cadenaConexion, string comando)
        {
            gestor.EscribirError(comando);
            try
            {
                MySqlConnection conexion = new MySqlConnection(cadenaConexion);
                MySqlCommand consulta = conexion.CreateCommand();
                consulta.CommandText = comando;
                conexion.Open();
                consulta.ExecuteNonQuery();
                consulta.Dispose();
                conexion.Close();
            }
            catch (MySqlException ex)
            {
                string error = "Error (GuardarTrazabilidad): " + ex.Number.ToString() + " - " + ex.Message;
                gestor.EscribirError(error);
                gestor.EscribirError(comando);
            }
        }

        public static string GetFecha(string lote)
        {
            if (lote.Length != 7)
            {
                return "0000-00-00";
            }

            //1260319

            char[] charArray = lote.ToCharArray();
            Array.Reverse(charArray);
            string fecha = new string(charArray);
            fecha = "20" + fecha.Substring(5, 2) + "-" + fecha.Substring(3, 2) + "-" + fecha.Substring(1, 2);
            return fecha;
        }

        static public bool ActualizarStock()
        {
            try
            {
                // Crear una lista para almacenar los resultados
                var movimientos = new List<(int pedidoNumero, int productoCodigo, string lote, int totalCantidad)>();

                using (MySqlConnection conn = new MySqlConnection(ConectorSQL.cadenaConexion))
                {
                    try
                    {
                        conn.Open();

                        // Inicia la transacción
                        MySqlCommand startTransactionCmd = new MySqlCommand("START TRANSACTION;", conn);
                        startTransactionCmd.ExecuteNonQuery();

                        // Selecciona y bloquea los registros no leídos
                        string selectQuery = @"
                            SELECT m.pedido_numero, m.producto_codigo, m.lote, SUM(m.cantidad) AS total_cantidad
                            FROM movimientos m
                            WHERE m.leido = FALSE
                            GROUP BY m.pedido_numero, m.producto_codigo, m.lote
                            FOR UPDATE;";

                        MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn);

                        using (MySqlDataReader reader = selectCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int pedidoNumero = reader.GetInt32("pedido_numero");
                                int productoCodigo = reader.GetInt32("producto_codigo");
                                string fecha_lote = GetFecha(reader.GetString("lote"));
                                string lote = fecha_lote;
                                int totalCantidad = reader.GetInt32("total_cantidad");

                                movimientos.Add((pedidoNumero, productoCodigo, lote, totalCantidad));
                            }
                        }

                        using (MySqlConnection destinationConn = new MySqlConnection(ConectorSQL.cadenaConexionPS))
                        {
                            destinationConn.Open();
                            foreach (var movimiento in movimientos)
                            {
                                string insertQuery = "INSERT INTO ps_trazabilidad (id_order, id_product, lote, cantidad) VALUES (@id_order, @id_product, @lote, @cantidad)";
                                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, destinationConn))
                                {
                                    insertCmd.Parameters.AddWithValue("@id_order", movimiento.pedidoNumero);
                                    insertCmd.Parameters.AddWithValue("@id_product", movimiento.productoCodigo);
                                    insertCmd.Parameters.AddWithValue("@lote", movimiento.lote);
                                    insertCmd.Parameters.AddWithValue("@cantidad", movimiento.totalCantidad);

                                    insertCmd.ExecuteNonQuery();
                                }
                            }
                        }

                        // Marca los registros como leídos
                        string updateQuery = "UPDATE movimientos SET leido = TRUE WHERE leido = FALSE;";
                        MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                        updateCmd.ExecuteNonQuery();

                        // Confirma la transacción
                        MySqlCommand commitCmd = new MySqlCommand("COMMIT;", conn);
                        commitCmd.ExecuteNonQuery();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        // En caso de error, revertir la transacción
                        MySqlCommand rollbackCmd = new MySqlCommand("ROLLBACK;", conn);
                        rollbackCmd.ExecuteNonQuery();
                    }
                }
                
                using (MySqlConnection conn = new MySqlConnection(ConectorSQL.cadenaConexionPS))
                {
                    conn.Open();

                    //.CompareTo("2024-06-13") < 0 ? "0000-00-00" : fecha_lote;
                    // Crear la sentencia SQL
                    string sql = @"
                        START TRANSACTION;
                        INSERT INTO ps_stock_web (products_id, fechaLote, cantidad)
                        SELECT subquery.id_product, subquery.fecha_lote, subquery.total
                        FROM (
                            SELECT t.id_product,
                            CASE 
                               WHEN t.lote < '2024-06-13' THEN '0000-00-00' 
                               ELSE t.lote 
                            END AS fecha_lote, 
                            SUM(t.cantidad) AS total
                            FROM ps_trazabilidad t
                            WHERE t.leido = FALSE
                            GROUP BY t.id_product, t.lote
                            FOR UPDATE
                        ) AS subquery
                        ON DUPLICATE KEY UPDATE cantidad = cantidad - subquery.total;

                        UPDATE ps_trazabilidad
                        SET leido = TRUE
                        WHERE leido = FALSE;

                        COMMIT;";

                    // Crear el comando para ejecutar la sentencia SQL
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    // Ejecutar el comando
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Operación completada con éxito.");
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
            return true;
        }

        static public string FiltrarConsultaLineasPedido(DateTime hoy)
        {
            DateTime semanaPasada = hoy.AddDays(-7);
            DateTime semanaProxima = hoy.AddDays(7);
            string resultado = string.Format(ConectorSQL.selectLineasPedidoFiltrado, semanaPasada.ToString("yyyy-MM-dd"), semanaProxima.ToString("yyyy-MM-dd"));
            return resultado;
        }

        /*
        static public Respuesta ActualizarStock() {
            Respuesta r = new Respuesta();
            try
            {
                //truncate tabla stock
                ConectorSQL.EjecutarComando(ConectorSQL.cadenaConexionPS, ConectorSQL.vaciarStock);
                //guardamos la trazabilidad
                foreach (Producto producto in Gestor.gestor.productos)
                {
                    //recorrer las posiciones
                    foreach (Posicion posicion in producto.posiciones)
                    {
                        string query = string.Format(ConectorSQL.sobreescribirStock, posicion.GetValoresActualizarStock());
                        ConectorSQL.EjecutarComando(ConectorSQL.cadenaConexionPS, query);
                    }
                    //stock sin colocar
                    if (producto.stock != 0)
                    {
                        string query = string.Format(ConectorSQL.sobreescribirStock, producto.codigo, "", producto.stock);
                        ConectorSQL.EjecutarComando(ConectorSQL.cadenaConexionPS, query);
                    }
                }
            }
            catch (MySqlException ex)
            {
                string error = "Error (ActualizarStock): " + ex.Number.ToString() + " - " + ex.Message;
                gestor.EscribirError(error);
            }
            return r;
        }
        */
    }
}
