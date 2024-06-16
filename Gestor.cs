using Conexiones;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Layout;
using Microsoft.VisualBasic.FileIO;
using NoCocinoMas.Objetos;
using Org.BouncyCastle.Utilities;
using RawPrint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Resources.ResXFileRef;

namespace NoCocinoMas
{
    public partial class Gestor : Form
    {
        public static Gestor gestor;
        private Servidor servidor;

        /////////////////////////  variables para No Cocino Mas //////////////////////
        public Configuracion configuracion;
        //listados de entidades
        public Almacenes almacenes;
        public Modulos modulos;
        public Posiciones posiciones;
        public Productos productos;
        public Envases envases;
        public Centralitas centralitas;
        public Operarios operarios;
        public Roles roles;
        public Pedidos pedidos;
        public Pedidos pedidosTransito;
        public Pedidos pedidosCompletar;
        public Tareas tareas;
        //tener todas las lineas es comodo para saber las cantidades totales de algo, y las pendientes de otros pedidos
        public LineasPedido lineasPedido;
        public ControlModulos controlModulos;
        public Bloqueos bloqueos;
        public Movimientos movimientos;
        public Menus menus;
        public Producto productoIluminado;
        public Transportistas transportistas;
        public Ubicaciones ubicaciones;

        public Dictionary<string, Entidades> listados;
        public Dictionary<string, DataGridView> tablas;
        public Dictionary<string, long> versiones;

        static private readonly object bloqueoArrays = new object();
        static private readonly object bloqueoListados = new object();
        static private readonly object bloqueoOK = new object();
        static private readonly object bloqueoObtenerPedidos = new object();
        static private readonly object bloqueoRecogida = new object();

        static public string ipTestModulos = "127.0.0.1";
        //static public string ipTest = "127.0.0.1";
        //static public string ipTest = "192.168.1.175";
        static public string ipTest = "192.168.1.147";
        //static public string ipTest = "";
        static public int puertoModulos = 80;

        static public string ipControlador = "192.168.1.254";
        static public int puertoCentralita = 8000;

        public Dictionary<string, ConexionPermanente> conexiones;

        public Gestor()
        {
            InitializeComponent();
            ConectorSQL.gestor = this;
            Gestor.gestor = this;

            //inicio de variables
            this.almacenes = new Almacenes();
            this.modulos = new Modulos();
            this.posiciones = new Posiciones();
            this.productos = new Productos();
            this.envases = new Envases();
            this.centralitas = new Centralitas();
            this.operarios = new Operarios();
            this.roles = new Roles();
            this.pedidos = new Pedidos();
            this.pedidosTransito = new Pedidos();
            this.pedidosCompletar = new Pedidos();
            this.lineasPedido = new LineasPedido();
            this.tareas = new Tareas(this);
            this.controlModulos = new ControlModulos();
            this.bloqueos = new Bloqueos();
            this.movimientos = new Movimientos();
            this.menus = new Menus();
            this.productoIluminado = null;

            this.configuracion = new Configuracion();

            listados = new Dictionary<string, Entidades>();

            this.listados.Add("almacenes", this.almacenes);
            this.listados.Add("modulos", this.modulos);
            this.listados.Add("posiciones", this.posiciones);
            this.listados.Add("productos", this.productos);
            this.listados.Add("envases", this.envases);
            this.listados.Add("centralitas", this.centralitas);
            this.listados.Add("operarios", this.operarios);
            this.listados.Add("roles", this.roles);
            this.listados.Add("pedidos", this.pedidos);
            this.listados.Add("lineas_pedido", this.lineasPedido);
            this.listados.Add("movimientos", this.movimientos);
            this.listados.Add("menus", this.menus);

            tablas = new Dictionary<string, DataGridView>();

            this.tablas.Add("almacenes", this.tablaAlmacenes);
            this.tablas.Add("modulos", this.tablaModulos);
            this.tablas.Add("posiciones", this.tablaPosiciones);
            this.tablas.Add("productos", this.tablaProductos);
            this.tablas.Add("centralitas", this.tablaCentralitas);
            this.tablas.Add("operarios", this.tablaOperarios);
            this.tablas.Add("roles", this.tablaRoles);
            this.tablas.Add("pedidos", this.tablaPedidos);
            this.tablas.Add("lineas_pedido", this.tablaLineasPedido);
            this.tablas.Add("movimientos", this.tablaMovimientos);

            versiones = new Dictionary<string, long>();

            this.versiones.Add("almacenes", 0);
            this.versiones.Add("modulos", 0);
            this.versiones.Add("posiciones", 0);
            this.versiones.Add("productos", 0);
            this.versiones.Add("envases", 0);
            this.versiones.Add("centralitas", 0);
            this.versiones.Add("operarios", 0);
            this.versiones.Add("roles", 0);
            this.versiones.Add("pedidos", 0);
            this.versiones.Add("lineas_pedido", 0);
            this.versiones.Add("movimientos", 0);
            this.versiones.Add("menus", 0);

            this.transportistas = new Transportistas();
            this.ubicaciones = new Ubicaciones();
            this.conexiones = new Dictionary<string, ConexionPermanente>();
        }

        private void Gestor_Load(object sender, EventArgs e)
        {
            this.Show();
            Task carga = new Task(CargarDatos);
            carga.Start();
            this.servidor = new Servidor(this);

            ConectorPLC.Apagar();
            this.temporizador.Start();


            this.pestañas.SelectedIndex = 0;
        }

        private void Gestor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (servidor != null)
            {
                servidor.Apagar();
            }
            foreach (Process proceso in Process.GetProcesses())
            {
                if (proceso.ProcessName == "NoCocinoMas")
                {
                    proceso.Kill();
                }
            }
        }

        /// <summary>
        /// Recupera los datos de la BBDD, los almacena en las variables y los muestra en las tablas
        /// </summary>
        private void CargarDatos()
        {
            Estado("Conectando a BBDD...");
            while (true)
            {
                if (ConectorSQL.Comprobar())
                {
                    Estado("Conexion a BBDD establecida.");
                    //Configuracion
                    ConectorSQL.CargarEntidades(ConectorSQL.selectConfiguracion, this.configuracion);
                    pdfEnvioTxt.Text = this.configuracion.carpetaPDFEnvios;
                    impresoraEnviosTxt.Text = this.configuracion.impresoraEnvios;

                    //Almacenes
                    ConectorSQL.CargarEntidades(ConectorSQL.selectAlmacenes, this.almacenes);
                    this.MostrarEntidades(this.tablaAlmacenes, this.almacenes);
                    ValorProgreso(this.progresoAlmacenes, 100);

                    //Centralitas
                    ConectorSQL.CargarEntidades(ConectorSQL.selectCentralitas, this.centralitas);
                    this.MostrarEntidades(this.tablaCentralitas, this.centralitas);
                    ValorProgreso(this.progresoCentralitas, 100);

                    //Modulos
                    ConectorSQL.CargarEntidades(ConectorSQL.selectModulos, this.modulos);
                    this.MostrarEntidades(this.tablaModulos, this.modulos);
                    ValorProgreso(this.progresoModulos, 100);

                    //Posiciones
                    ConectorSQL.CargarEntidades(ConectorSQL.selectPosiciones, this.posiciones);
                    this.MostrarEntidades(this.tablaPosiciones, this.posiciones);
                    ValorProgreso(this.progresoPosiciones, 100);

                    //Productos
                    this.productos = ConectorJSON.CargarObjeto<Productos>("./productos.json") ?? new Productos();
                    this.MostrarEntidades(this.tablaProductos, this.productos);
                    ValorProgreso(this.progresoProductos, 100);

                    //Ubicaciones
                    ConectorSQL.CargarEntidades(ConectorSQL.selectUbicaciones, this.ubicaciones);

                    //Actualizar productos
                    ActualizarProductos();

                    //Envases
                    ConectorSQL.CargarEntidades(ConectorSQL.selectEnvases, this.envases);
                    ValorProgreso(this.progresoEnvases, 100);

                    //Operarios
                    ConectorSQL.CargarEntidades(ConectorSQL.selectOperarios, this.operarios);
                    this.MostrarEntidades(this.tablaOperarios, this.operarios);
                    ValorProgreso(this.progresoOperarios, 100);

                    //Roles
                    ConectorSQL.CargarEntidades(ConectorSQL.selectRoles, this.roles);
                    this.MostrarEntidades(this.tablaRoles, this.roles);
                    ValorProgreso(this.progresoRoles, 100);

                    //Pedidos desde hace un mes
                    //DateTime desde = DateTime.Now.AddMonths(-1);
                    DateTime desde = DateTime.Now.AddDays(-15);
                    string consultaPedidos = string.Format(ConectorSQL.selectPedidosFiltrado, desde.ToString("yyyy-MM-dd"));
                    ConectorSQL.CargarEntidades(consultaPedidos, this.pedidos);
                    this.MostrarEntidades(this.tablaPedidos, this.pedidos);
                    ValorProgreso(this.progresoOrdenes, 100);

                    if (this.pedidos.Contador() > 0)
                    {
                        //LineasPedido
                        string consultaLineasPedidos = string.Format(ConectorSQL.selectLineasPedidoFiltrado, this.pedidos.ListadoNumeros());
                        ConectorSQL.CargarEntidades(consultaLineasPedidos, this.lineasPedido);

                        //Movimientos
                        string consultaMovimientos = string.Format(ConectorSQL.selectMovimientosFiltrado, this.pedidos.ListadoNumeros());
                        ConectorSQL.CargarEntidades(consultaMovimientos, this.movimientos);

                        //Pedidos en transito
                        this.pedidosTransito.Agregar(this.pedidos.PedidosTransito());
                        this.pedidosCompletar.Agregar(this.pedidos.PedidosCompletar());
                    }

                    //Menus
                    ConectorSQL.CargarEntidades(ConectorSQL.obtenerMenusPS, this.menus, false, ConectorSQL.cadenaConexionPS);

                    /////////////// AGRUPACIONES //////////////
                    Estado("Vinculando Objetos.");

                    // agregamos los modulos a los almacenes
                    this.almacenes.VincularModulos(this.modulos);

                    // agregamos las posiciones a los modulos
                    this.modulos.VincularPosiciones(this.posiciones);

                    // agregamos los productos y las centralitas a las posiciones
                    this.posiciones.VincularProductos(this.productos);
                    this.posiciones.VincularCentralitas(this.centralitas);

                    // agregamos los envases a los productos
                    this.productos.VincularEnvases(this.envases);
                    this.productos.VincularUbicaciones(this.ubicaciones);

                    // agregamos los roles a los operarios
                    this.operarios.VincularRoles(this.roles);

                    Estado("Preparando Pedidos.");
                    //agregamos los productos a las lineas de pedido
                    Dictionary<int, Producto> listadoProductos = this.ListadoProductos();
                    this.lineasPedido.VincularProductos(listadoProductos);
                    this.menus.VincularProductos(listadoProductos);

                    Estado("Vinculando Productos.");
                    //agregamos las lineas a los pedidos
                    this.pedidos.VincularLineas(this.lineasPedido);

                    Estado("Completando Listados.");
                    //llenado de otros controles

                    this.RellenarOperariosTareas();

                    //leds de inicio de marcas de bloqueo de modulos
                    ConectorSQL.CargarEntidades(ConectorSQL.selectBloqueos, this.bloqueos);

                    //inicio de los servidores
                    this.servidor.Iniciar();

                    DesbloquearPestañas();
                    Estado("Sistema preparado.");

                    CalcularFechas();

                    break;
                }
                Estado("Reintentando conexión a BBDD...");
                Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// Modifica el valor de una barra de progreso desde otro hilo
        /// </summary>
        /// <param name="pb"></param>
        /// <param name="valor"></param>
        private void ValorProgreso(ProgressBar pb, int valor)
        {
            if (pb.InvokeRequired)
            {
                pb.BeginInvoke(
                    (System.Windows.Forms.MethodInvoker)delegate ()
                   {
                       pb.Value = valor;
                   }
                );
            }
            else
            {
                pb.Value = valor;
            }
        }

        /// <summary>
        /// Agrega un registro a una tabla desde otro hilo
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="entidades"></param>
        private void MostrarEntidades(DataGridView tabla, IEnumerable entidades, bool actualizar = false)
        {
            if (entidades == null || tabla == null) return;
            lock (bloqueoListados)
            {
                if (tabla.InvokeRequired)
                {
                    tabla.BeginInvoke(
                        (System.Windows.Forms.MethodInvoker)delegate ()
                       {
                           if (actualizar)
                           {
                               tabla.Rows.Clear();
                           }
                           foreach (IEntidad e in entidades)
                           {
                               tabla.Rows.Add(e.GetValoresTablas());
                           }
                       }
                    );
                }
                else
                {
                    if (actualizar)
                    {
                        tabla.Rows.Clear();
                    }
                    else
                    {
                        foreach (IEntidad e in entidades)
                        {
                            tabla.Rows.Add(e.GetValoresTablas());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Carga los operarios en la tabla tareas
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="entidades"></param>
        private void RellenarOperariosTareas(bool actualizar = true)
        {
            if (this.tablaTareas.InvokeRequired)
            {
                this.tablaTareas.BeginInvoke(
                    (System.Windows.Forms.MethodInvoker)delegate ()
                    {
                        if (actualizar)
                        {
                            this.tablaTareas.Rows.Clear();
                        }
                        foreach (Operario o in this.operarios)
                        {
                            this.tablaTareas.Rows.Add(o.id, o.nombre);
                        }
                    }
                );
            }
            else
            {
                if (actualizar)
                {
                    this.tablaTareas.Rows.Clear();
                }
                else
                {
                    foreach (Operario o in this.operarios)
                    {
                        this.tablaTareas.Rows.Add(o.id, o.nombre);
                    }
                }
            }
        }

        /// <summary>
        /// Lista un numero maximo de lineas de un array en un DataGrid
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="entidades"></param>
        /// <param name="maximo"></param>
        private void MostrarEntidades(DataGridView tabla, IEnumerable entidades, int maximo)
        {
            int contador = 0;
            if (tabla.InvokeRequired)
            {
                tabla.BeginInvoke(
                    (System.Windows.Forms.MethodInvoker)delegate ()
                    {
                        foreach (IEntidad e in entidades)
                        {
                            if (contador == maximo) return;
                            tabla.Rows.Add(e.GetValoresTablas());
                            contador++;
                        }
                    }
                );
            }
            else
            {
                foreach (IEntidad e in entidades)
                {
                    if (contador == maximo) return;
                    tabla.Rows.Add(e.GetValoresTablas());
                    contador++;
                }
            }
        }

        /// <summary>
        /// Desbloquea las pestañas del formulario tras completar una tarea
        /// </summary>
        private void DesbloquearPestañas()
        {
            if (this.pestañas.InvokeRequired)
            {
                this.pestañas.BeginInvoke(
                    (System.Windows.Forms.MethodInvoker)delegate ()
                    {
                        this.pestañas.Enabled = true;
                    }
                );
            }
            else
            {
                this.pestañas.Enabled = true;
            }
        }

        public void EscribirEvento(string texto)
        {
            //lock (bloqueoEscribirEvento)
            {
                if (this.eventosTxt.InvokeRequired)
                {
                    this.eventosTxt.BeginInvoke(
                        (System.Windows.Forms.MethodInvoker)delegate ()
                        {
                            string[] l = new string[100];
                            l[0] = DateTime.Now.ToString() + " -- " + texto;
                            string[] lineas = this.eventosTxt.Lines;
                            int ultimo = lineas.Length > 99 ? 98 : lineas.Length;

                            for (int i = 0; i < ultimo; i++)
                            {
                                l[i + 1] = lineas[i];
                            }

                            this.eventosTxt.Lines = l;
                        }
                    );
                }
                else
                {
                    string[] l = new string[100];
                    l[0] = DateTime.Now.ToString() + " -- " + texto;
                    string[] lineas = this.eventosTxt.Lines;
                    int ultimo = lineas.Length > 99 ? 98 : lineas.Length;

                    for (int i = 0; i < ultimo; i++)
                    {
                        l[i + 1] = lineas[i];
                    }

                    this.eventosTxt.Lines = l;
                }
            }
        }

        public void EscribirError(string texto)
        {
            //lock (bloqueoEscribirError)
            {
                try
                {
                    //ConectorSQL.InsertarError(texto);
                    if (this.erroresTxt.InvokeRequired)
                    {
                        this.erroresTxt.BeginInvoke(
                            (System.Windows.Forms.MethodInvoker)delegate ()
                            {
                                string[] l = new string[100];
                                l[0] = DateTime.Now.ToString() + " -- " + texto;
                                string[] lineas = this.erroresTxt.Lines;
                                int ultimo = lineas.Length > 99 ? 98 : lineas.Length;

                                for (int i = 0; i < ultimo; i++)
                                {
                                    l[i + 1] = lineas[i];
                                }

                                this.erroresTxt.Lines = l;
                            }
                        );
                    }
                    else
                    {
                        string[] l = new string[100];
                        l[0] = DateTime.Now.ToString() + " -- " + texto;
                        string[] lineas = this.erroresTxt.Lines;
                        int ultimo = lineas.Length > 99 ? 98 : lineas.Length;

                        for (int i = 0; i < ultimo; i++)
                        {
                            l[i + 1] = lineas[i];
                        }

                        this.erroresTxt.Lines = l;
                    }
                }
                catch
                {
                    //
                }
            }
        }



        private void ResetHilos_Click(object sender, EventArgs e)
        {
            this.ResetServidor();
        }

        public void ResetServidor()
        {
            this.servidor.ReiniciarServidores();
        }

        public void Estado(string texto)
        {
            if (textoEstado.GetCurrentParent().InvokeRequired)
            {
                this.textoEstado.GetCurrentParent().BeginInvoke(
                    (System.Windows.Forms.MethodInvoker)delegate ()
                    {
                        this.textoEstado.Text = texto;
                    });
            }
            else
            {
                this.textoEstado.Text = texto;
            }
        }


        /// <summary>
        /// Click en una celda.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tablaTareas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int columna = e.ColumnIndex;
            int fila = e.RowIndex;
            if (fila < 0) return;

            switch (columna)
            {
                case 0:
                    //boton ok
                    int.TryParse(this.tablaTareas.Rows[fila].Cells[0].Value.ToString(), out int id);
                    Tarea tarea = this.tareas.BuscarOperario(id);
                    if (tarea != null)
                    {
                        //tarea.PulsarOk(this);
                    }
                    break;
                case 2:
                    //boton iniciar
                    break;
                case 5:
                    //check
                    if (this.tablaTareas.Rows[fila].Cells[5].Value == null)
                    {
                        this.tablaTareas.Rows[fila].Cells[5].Value = true;

                    }
                    else
                    {
                        this.tablaTareas.Rows[fila].Cells[5].Value = null;
                    }

                    break;
            }
        }

        /*this.tablaTareas.Rows[fila].DefaultCellStyle.BackColor = Color.LightSeaGreen;
        this.tablaTareas.Rows[fila].Cells[1].Style.ForeColor = Color.Blue;
        this.tablaTareas.Rows[fila].Cells[columna].Style.BackColor = Color.Red;
        */

        public Respuesta EditarEntidad(Parametros parametros)
        {
            Respuesta r = new Respuesta();
            try
            {
                //si no existe id se supone que es Crear, y la variable será true
                int id = parametros.BuscarInt("id");
                bool crear = id == 0;
                string entidad = parametros.Buscar("entidad");
                //comprobacion parametro entidad
                if (entidad == null) return r.Error("Parametro Entidad no encontrado");
                //comprobacion existe la entidad
                if (!this.listados.ContainsKey(entidad)) return r.Error("Entidad no encontrada");
                IEntidad e;
                lock (bloqueoArrays)
                {
                    if (crear)
                    {
                        switch (entidad)
                        {
                            case "lineas_pedido":
                                //comprobar Producto y pedido
                                int pedido_id = parametros.BuscarInt("pedido_numero");
                                Pedido pedido = this.pedidos.BuscarNumero(pedido_id);
                                if (pedido == null)
                                {
                                    return r.Error("Pedido no encontrado");
                                }
                                int producto_id = parametros.BuscarInt("producto_codigo");
                                Producto producto = this.productos.BuscarCodigo(producto_id);
                                if (producto == null)
                                {
                                    return r.Error("Producto no encontrado");
                                }

                                r = ((IEntidades)this.listados[entidad]).InsertSQL(parametros);
                                e = (IEntidad)r.entidad;

                                pedido.lineas.Agregar((LineaPedido)e);
                                ((LineaPedido)e).producto = producto;

                                break;
                            default:
                                r = ((IEntidades)this.listados[entidad]).InsertSQL(parametros);
                                e = (IEntidad)r.entidad;
                                break;
                        }
                    }
                    else
                    {
                        switch (entidad)
                        {
                            case "lineas_pedido":
                                //no se pueden bajar de los recogidos, ni siquiera para devolverlos a stock
                                //pues no se sabe que movimiento o de que posicion, ni lote, ha sido
                                //se debe eliminar la linea y crear de nuevo, o minimo, permitir solo bajar a 0
                                //y luego subirlo, para ahorrar el crear la linea, pero no se recomienda.
                                //Eliminandola, todos los movimientos pueden eliminarse y todo iria a stock.
                                e = (IEntidad)this.listados[entidad].BuscarId(id);
                                if (e == null) return r.Error("Id " + id + " no encontrado");
                                if (((LineaPedido)e).recogido > parametros.BuscarInt("cantidad"))
                                {
                                    return r.Error("Ya se han retirado más unidades de las indicadas.");
                                }
                                r = e.UpdateSQL(parametros);
                                break;
                            case "pedido":
                                e = (IEntidad)this.listados[entidad].BuscarId(id);
                                if (e == null) return r.Error("Id " + id + " no encontrado");
                                //verificamos si es posible el cambio de estado
                                Pedido p = (Pedido)e;


                                r = e.UpdateSQL(parametros);
                                break;
                            default:
                                e = (IEntidad)this.listados[entidad].BuscarId(id);
                                if (e == null) return r.Error("Id " + id + " no encontrado");
                                r = e.UpdateSQL(parametros);
                                break;
                        }
                    }
                }
                //comprobacion error en edicion o creacion
                if (r.error) return r;
                //actualizacmos los listados
                switch (entidad)
                {
                    case "operarios":
                        ((Operario)e).VincularRol(this.roles);
                        this.MostrarEntidades(this.tablaOperarios, this.operarios, true);
                        break;
                    case "roles":
                        this.MostrarEntidades(this.tablaRoles, this.roles, true);
                        break;
                    case "centralitas":
                        this.MostrarEntidades(this.tablaCentralitas, this.centralitas, true);
                        break;
                    case "almacenes":
                        this.MostrarEntidades(this.tablaAlmacenes, this.almacenes, true);
                        break;
                    case "modulos":
                        //no se vinculan las posiciones, se hace dinamicamente o al principio
                        this.MostrarEntidades(this.tablaModulos, this.modulos, true);
                        break;
                    case "posiciones":
                        ((Posicion)e).VincularCentralita(this.centralitas);
                        ((Posicion)e).VincularProducto(this.productos);
                        this.MostrarEntidades(this.tablaPosiciones, this.posiciones, true);
                        break;
                    case "productos":
                        this.MostrarEntidades(this.tablaProductos, this.productos, true);
                        break;
                    case "pedidos":
                        this.MostrarEntidades(this.tablaPedidos, this.pedidos, true);
                        break;
                    case "lineas_pedido":
                        break;
                }
            }
            catch (Exception e)
            {
                r.Error(e.Message);
            }
            return r;
        }


        public Respuesta EliminarEntidad(Dictionary<string, string> argumentos)
        {
            Respuesta r = new Respuesta();
            try
            {
                string entidad = argumentos["entidad"];
                int id = int.Parse(argumentos["id"]);
                if (ConectorSQL.DeleteSQL(entidad, id))
                {
                    switch (entidad)
                    {
                        case "lineas_pedido":
                            LineaPedido l = (LineaPedido)this.lineasPedido.BuscarId(id);
                            l.pedido.lineas.Eliminar(l);
                            this.lineasPedido.Eliminar(l);
                            break;
                        case "pedidos":
                            Pedido p = (Pedido)this.pedidos.BuscarId(id);
                            //borramos cada linea de la BBDD
                            foreach (LineaPedido linea in p.lineas)
                            {
                                ConectorSQL.DeleteSQL("lineas_pedido", (int)linea.id);
                                this.lineasPedido.Eliminar(linea);
                            }
                            this.listados[entidad].Eliminar(id);
                            this.MostrarEntidades(this.tablas[entidad], this.listados[entidad], true);
                            break;
                        default:
                            this.listados[entidad].Eliminar(id);
                            this.MostrarEntidades(this.tablas[entidad], this.listados[entidad], true);
                            break;
                    }
                }
                else
                {
                    r.Error("Fallo en la consulta SQL para eliminar");
                }
            }
            catch
            {
                r.Error("Ocurrió un error durante la eliminación.");
            }
            return r;
        }


        public void ActualizarIpModulo(Modulo modulo, string ip)
        {
            modulo.ip = ip;
            this.MostrarEntidades(this.tablaModulos, this.modulos, true);
        }

        public void ActualizarIpCentralita(Centralita centralita, string ip)
        {
            centralita.ip = ip;
            this.MostrarEntidades(this.tablaCentralitas, this.centralitas, true);
        }

        public Respuesta EditarTarea(Dictionary<string, string> argumentos)
        {
            Respuesta r = new Respuesta();
            try
            {
                string tarea_id = argumentos["id"];
                int operario_id = int.Parse(argumentos["operario_id"]);
                int almacen_id = int.Parse(argumentos["almacen_id"]);
                int accion = int.Parse(argumentos["accion"]);
                int color = int.Parse(argumentos["color"]);

                Tarea tarea = this.tareas.BuscarId(tarea_id);
                if (tarea == null)
                {
                    r.Error("No se encontro la tarea");
                    return r;
                }

                Almacen almacen = (Almacen)this.almacenes.BuscarId(almacen_id);
                if (almacen == null)
                {
                    r.Error("No se encontro el almacen");
                    return r;
                }
                Operario operario = (Operario)this.operarios.BuscarId(operario_id);
                if (operario == null)
                {
                    r.Error("No se encontro el operario");
                    return r;
                }

                switch (accion)
                {
                    case 0:
                        operario.almacen_id = 0;
                        operario.color = 0;
                        operario.accion = 0;
                        tarea.Parar();
                        break;
                    case 1:
                    case 2:
                        if (operario.accion != 0)
                        {
                            r.Error("El operario ya está activo");
                            return r;
                        }
                        if (tarea.estado != 0)
                        {
                            r.Error("La tarea ya está activa");
                            return r;
                        }
                        operario.almacen_id = almacen_id;
                        operario.color = color;
                        operario.accion = accion;
                        tarea.Iniciar(operario.accion, almacen, operario);
                        break;
                }
                //mostrar cambios en el formulario
                r.objeto = tarea;
                this.ActualizarTablaTareas(tarea);
            }
            catch (Exception e)
            {
                r.Error("Ocurrió un error durante la edición de la tarea (EditarTarea): " + e.Message);
            }
            return r;
        }

        public Respuesta CambiarTarea(Dictionary<string, string> argumentos)
        {
            Respuesta r = new Respuesta();
            try
            {
                string tarea_id = argumentos["id"];
                Tarea tarea = this.tareas.BuscarId(tarea_id);
                if (tarea == null)
                {
                    r.Error("No se encontro la tarea");
                    return r;
                }
                if (tarea.accion == 0)
                {
                    r.Error("Tarea no iniciada");
                    return r;
                }
                tarea.Cambiar();
                //mostrar cambios en el formulario
                r.objeto = tarea;
                this.ActualizarTablaTareas(tarea);
            }
            catch (Exception e)
            {
                r.Error("Ocurrió un error durante la edición de la tarea (EditarTarea): " + e.Message);
            }
            return r;
        }

        public Respuesta IniciarReposicion(Dictionary<string, string> argumentos)
        {
            Respuesta r = new Respuesta();
            try
            {
                string tarea_id = argumentos["tarea_id"];
                int producto_codigo = int.Parse(argumentos["producto_codigo"]);
                int cantidad = int.Parse(argumentos["cantidad"]);
                string lote = argumentos["lote"];
                string forzarPosicion = argumentos["forzarPosicion"];

                Tarea tarea = this.tareas.BuscarId(tarea_id);
                if (tarea == null)
                {
                    r.Error("No se encontro la tarea");
                    return r;
                }
                if (tarea.estado == 0)
                {
                    r.Error("Tarea no iniciada");
                    return r;
                }
                if (tarea.estado == 2)
                {
                    r.Error("Tarea en estado de recogida");
                    return r;
                }
                if (tarea.almacen == null)
                {
                    r.Error("No se encontro el almacen");
                    return r;
                }
                if (tarea.operario == null)
                {
                    r.Error("No se encontro el operario");
                    return r;
                }
                Producto producto = this.productos.BuscarCodigo(producto_codigo);
                if (producto == null)
                {
                    r.Error("No se encontro el producto");
                    return r;
                }
                if (cantidad == 0)
                {
                    r.Error("No se indicó una cantidad");
                    return r;
                }
                if (lote.Trim() == "")
                {
                    r.Error("No se indicó un lote");
                    return r;
                }

                Posicion posicionForzada = null;
                if (forzarPosicion.Trim() != "")
                {
                    posicionForzada = this.posiciones.BuscarNombre(forzarPosicion);
                    if (producto == null)
                    {
                        r.Error("Posición forzada no válida");
                        return r;
                    }
                    if (posicionForzada.cantidad > 0 && posicionForzada.producto_codigo != producto.codigo)
                    {
                        r.Error("Posición con otro producto.");
                        return r;
                    }
                    if (posicionForzada.Disponible() == 0)
                    {
                        r.Error("Posición sin capacidad disponible.");
                        return r;
                    }

                    tarea.IniciarReposicion(producto, lote, cantidad, posicionForzada);
                }
                else
                {
                    tarea.IniciarReposicion(producto, lote, cantidad);
                }
                r.objeto = tarea;
                if (tarea.posicion == null)
                {
                    if (forzarPosicion.Trim() == "")
                    {
                        r.Error("Sin posición (Iniciar reposicion)");
                    }
                    else
                    {
                        r.Error("Posición forzada no válida (Iniciar reposicion)");
                    }
                }

                this.ActualizarTablaTareas(tarea);

            }
            catch (Exception e)
            {
                r.Error("Ocurrió un error durante la edición de la tarea (IniciarReposicion): " + e.Message);
            }
            return r;
        }

        public Respuesta IniciarPedido(Dictionary<string, string> argumentos)
        {
            Respuesta r = new Respuesta();
            //try
            //{
            string tarea_id = argumentos["tarea_id"];
            int num_pedido = int.Parse(argumentos["num_pedido"]);

            Tarea tarea = this.tareas.BuscarId(tarea_id);
            if (tarea == null)
            {
                r.Error("No se encontro la tarea");
                return r;
            }
            if (tarea.estado == 0)
            {
                r.Error("Tarea no iniciada");
                return r;
            }
            if (tarea.almacen == null)
            {
                r.Error("No se encontro el almacen");
                return r;
            }
            if (tarea.operario == null)
            {
                r.Error("No se encontro el operario");
                return r;
            }
            Pedido pedido = this.pedidos.BuscarNumero(num_pedido);
            if (pedido == null)
            {
                r.Error("No se encontro el pedido");
                return r;
            }
            if (pedido.estado > 1)
            {
                r.Error("Pedido ya transferido a la BBDD");
                return r;
            }
            Tarea tareaReferida = this.tareas.BuscarPedido(num_pedido);
            if (tareaReferida != null)
            {
                r.Error("El pedido ya se está recogiendo");
                //agregar datos de la tarea referida
                return r;
            }

            pedido.VincularMovimientos();
            tarea.IniciarRecogida(pedido);
            r.objeto = tarea;
            this.ActualizarTablaTareas(tarea);

            /* }
             catch (Exception e)
             {
                 r.Error("Ocurrió un error durante la edición de la tarea (IniciarPedido): " + e.Message);
             }*/
            return r;
        }

        /// <summary>
        /// Con el alias del modulo, se busca cual tarea lo tiene bloqueado
        /// </summary>
        /// <param name="alias_modulo"></param>
        public void PulsarOk(Mensaje mensaje)
        {
            Modulo modulo = this.modulos.BuscarAlias(mensaje.identificador);
            //Se comprueba que el modulo sea valido
            lock (Tareas.bloqueoBusquedaPosicion)
            {
                if (modulo == null || modulo.tareaBloqueante == null || modulo != modulo.tareaBloqueante.moduloActual)
                {
                    return;
                }
            }
            //Se comprueba la IP del modulo
            if (modulo.ip == "")
            {
                this.ActualizarIpModulo(modulo, mensaje.ip);
            }
            //Se ejecuta la aceptacion
            Tarea tarea = modulo.tareaBloqueante;
            if (tarea != null && tarea.posicion != null)
            {
                tarea.PulsarOk(mensaje.cantidad);
            }
        }

        public void ActualizarTablaTareas(Tarea tarea)
        {
            string tarea_id = tarea.id;
            object operario_id = tarea.operario != null ? tarea.operario.id : null;

            if (this.tablaTareas.InvokeRequired)
            {
                this.tablaTareas.BeginInvoke(
                    (System.Windows.Forms.MethodInvoker)delegate ()
                    {
                        foreach (DataGridViewRow row in this.tablaTareas.Rows)
                        {
                            if ((string)row.Cells[5].Value == tarea_id || row.Cells[0].Value == operario_id)
                            {
                                if (tarea.estado == 1)
                                {
                                    row.Cells[2].Value = tarea.accion;
                                    row.Cells[3].Value = (tarea.pedido != null) ? tarea.pedido.numero.ToString() : "";
                                    row.Cells[4].Value = (tarea.producto != null) ? tarea.producto.codigo.ToString() : "";
                                    row.Cells[5].Value = tarea_id;
                                    row.Cells[6].Value = tarea.moduloActual != null ? tarea.moduloActual.alias.ToString() : "";
                                }
                                else
                                {
                                    row.Cells[2].Value = "";
                                    row.Cells[3].Value = "";
                                    row.Cells[4].Value = "";
                                    row.Cells[5].Value = "";
                                    row.Cells[6].Value = tarea.moduloActual != null ? tarea.moduloActual.alias.ToString() : "";
                                    break;
                                }
                            }
                        }
                    }
                );
            }
            else
            {
                foreach (DataGridViewRow row in this.tablaTareas.Rows)
                {
                    if ((string)row.Cells[5].Value == tarea_id || row.Cells[0].Value == operario_id)
                    {
                        if (tarea.estado == 1)
                        {
                            row.Cells[2].Value = tarea.accion;
                            row.Cells[3].Value = (tarea.pedido != null) ? tarea.pedido.numero.ToString() : "";
                            row.Cells[4].Value = (tarea.producto != null) ? tarea.producto.codigo.ToString() : "";
                            row.Cells[5].Value = tarea_id;
                            row.Cells[6].Value = tarea.moduloActual != null ? tarea.moduloActual.alias.ToString() : "";
                        }
                        else
                        {
                            row.Cells[2].Value = "";
                            row.Cells[3].Value = "";
                            row.Cells[4].Value = "";
                            row.Cells[5].Value = "";
                            row.Cells[6].Value = tarea.moduloActual != null ? tarea.moduloActual.alias.ToString() : "";
                            break;
                        }
                    }
                }
            }
        }

        public void ActualizarTablaPosiciones(Posicion posicion)
        {
            if (this.tablaPosiciones.InvokeRequired)
            {
                this.tablaPosiciones.BeginInvoke(
                    (System.Windows.Forms.MethodInvoker)delegate ()
                    {
                        foreach (DataGridViewRow row in this.tablaPosiciones.Rows)
                        {
                            if (row.Cells[0].Value == posicion.id)
                            {
                                if (posicion.producto != null)
                                {
                                    row.Cells[8].Value = posicion.producto.codigo;
                                    row.Cells[9].Value = posicion.cantidad;
                                    row.Cells[10].Value = posicion.lote;
                                }
                                else
                                {
                                    row.Cells[8].Value = "";
                                    row.Cells[9].Value = 0;
                                    row.Cells[10].Value = "";
                                }
                            }
                        }
                    }
                );
            }
            else
            {
                foreach (DataGridViewRow row in this.tablaPosiciones.Rows)
                {
                    if (row.Cells[0].Value == posicion.id)
                    {
                        row.Cells[8].Value = posicion.producto.codigo;
                        row.Cells[9].Value = posicion.cantidad;
                        row.Cells[10].Value = posicion.lote;
                    }
                }
            }
        }

        public void ActualizarTablaProductos(Producto producto)
        {
            if (this.tablaProductos.InvokeRequired)
            {
                this.tablaProductos.BeginInvoke(
                    (System.Windows.Forms.MethodInvoker)delegate ()
                    {
                        foreach (DataGridViewRow row in this.tablaProductos.Rows)
                        {
                            if (row.Cells[0].Value == producto.id)
                            {
                                row.Cells[1].Value = producto.codigo;
                                row.Cells[2].Value = producto.nombre;
                                row.Cells[3].Value = producto.envase_id;
                                row.Cells[4].Value = producto.stock;
                            }
                        }
                    }
                );
            }
            else
            {
                foreach (DataGridViewRow row in this.tablaProductos.Rows)
                {
                    if (row.Cells[0].Value == producto.id)
                    {
                        row.Cells[1].Value = producto.codigo;
                        row.Cells[2].Value = producto.nombre;
                        row.Cells[3].Value = producto.envase_id;
                        row.Cells[4].Value = producto.stock;
                    }
                }
            }
        }

        public void ActualizarTablaModulos(Modulo modulo)
        {
            if (this.tablaModulos.InvokeRequired)
            {
                this.tablaModulos.BeginInvoke(
                    (System.Windows.Forms.MethodInvoker)delegate ()
                    {
                        foreach (DataGridViewRow row in this.tablaModulos.Rows)
                        {
                            if (row.Cells[0].Value.ToString() == modulo.id.ToString())
                            {
                                row.Cells[5].Value = modulo.tareaBloqueante != null ? modulo.tareaBloqueante.id : "";
                            }
                        }
                    }
                );
            }
            else
            {
                foreach (DataGridViewRow row in this.tablaModulos.Rows)
                {
                    if (row.Cells[0].Value == modulo.id)
                    {
                        row.Cells[5].Value = modulo.tareaBloqueante != null ? modulo.tareaBloqueante.id : "";
                    }
                }
            }
        }

        /// <summary>
        /// Accion de la tabla de pedidos que muestra el contenido de un pedido en la tabla de lineas de pedido
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tablaPedidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                this.MostrarLineasPedido(this.tablaPedidos.Rows[e.RowIndex].Cells[1].Value);
            }
        }
        private void tablaPedidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        /// <summary>
        /// Muestra el contenido de un pedido en la tabla de lineas de pedido
        /// </summary>
        /// <param name="id"></param>
        private void MostrarLineasPedido(object id)
        {
            this.tablaLineasPedido.Rows.Clear();
            this.MostrarEntidades(this.tablaLineasPedido, this.pedidos.ObtenerLineasPedido(id));
            this.tablaMovimientos.Rows.Clear();
            this.MostrarEntidades(this.tablaMovimientos, this.pedidos.ObtenerMovimientos(id));
        }

        /// <summary>
        /// Crea un diccionario de operarios para localizarlos rapidamente por el id
        /// </summary>
        /// <returns></returns>
        public Dictionary<object, Operario> ListadoOperarios()
        {
            Dictionary<object, Operario> os = new Dictionary<object, Operario>();
            foreach (Operario o in this.operarios)
            {
                try
                {
                    os.Add(o.id, o);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return os;
        }
        /// <summary>
        /// Crea un diccionario de productos para localizarlos rapidamente por el id
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Producto> ListadoProductos()
        {
            Dictionary<int, Producto> ps = new Dictionary<int, Producto>();
            foreach (Producto p in this.productos)
            {
                try
                {
                    ps.Add(p.codigo, p);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return ps;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConectorPLC.Apagar();
        }

        private void tablaPosiciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == 12)
            {
                int centralita_id = (int)this.tablaPosiciones.Rows[e.RowIndex].Cells[4].Value;
                string ip = ((Centralita)this.centralitas.BuscarId(centralita_id)).ip;
                int linea = (int)this.tablaPosiciones.Rows[e.RowIndex].Cells[5].Value;
                int inicio = (int)this.tablaPosiciones.Rows[e.RowIndex].Cells[6].Value;
                int longitud = (int)this.tablaPosiciones.Rows[e.RowIndex].Cells[7].Value;
                ConectorPLC.EncenderReposicion(ip, linea, inicio, longitud);
            }
        }

        public Respuesta SimularAccion(Dictionary<string, string> argumentos)
        {
            Respuesta r = new Respuesta();
            try
            {
                string tarea_id = argumentos["tarea_id"];
                string accion = argumentos["accion"];
                int cantidad = int.Parse(argumentos["cantidad"]);

                Tarea tarea = this.tareas.BuscarId(tarea_id);
                if (tarea == null)
                {
                    r.Error("No se encontro la tarea");
                    return r;
                }
                if (tarea.estado == 0)
                {
                    r.Error("Tarea no iniciada");
                    return r;
                }
                if (tarea.almacen == null)
                {
                    r.Error("No se encontró el almacen");
                    return r;
                }
                if (tarea.operario == null)
                {
                    r.Error("No se encontro el operario");
                    return r;
                }
                switch (accion)
                {
                    case "OK":
                        if (tarea != null && tarea.posicion != null)
                        {
                            tarea.PulsarOk(cantidad);
                            r.objeto = tarea;
                        }
                        else
                        {
                            r.Error("Tarea o posición nulas.");
                        }
                        break;
                    default:
                        r.Error("Acción no encontrada: " + accion);
                        return r;
                }
                //comprobar que no se finalice la tarea de reposicion, solo esta accion. //quizas cambiar accion = 0
                this.ActualizarTablaTareas(tarea);

            }
            catch (Exception e)
            {
                r.Error("Ocurrió un error durante la edición de la tarea (SimularAccion): " + e.Message);
            }
            return r;
        }

        public Respuesta CompletarLinea(Dictionary<string, string> argumentos)
        {
            Respuesta r = new Respuesta();
            try
            {
                int pedido_numero = int.Parse(argumentos["pedido_numero"]);
                int codigo_producto = int.Parse(argumentos["codigo_producto"]);
                string lote = argumentos["lote"];
                int cantidad = int.Parse(argumentos["cantidad"]);

                Pedido pedido = this.pedidos.BuscarNumero(pedido_numero);
                if (pedido == null)
                {
                    r.Error("No se encontro el pedido");
                    return r;
                }
                if (pedido.estado == 1)
                {
                    r.Error("Pedido ya finalizado");
                    return r;
                }
                string respuesta = pedido.CompletarLinea(codigo_producto, lote, cantidad);
                if (respuesta == null)
                {
                    r.entidad = pedido;
                    r.objeto = pedido;
                }
                else
                {
                    r.Error(respuesta);
                }
            }
            catch (Exception e)
            {
                r.Error("Ocurrió un error completando la linea de pedido (CompletarLinea): " + e.Message);
            }
            return r;
        }

        public Respuesta CompletarListado(CompletarPedido lineas)
        {
            Respuesta r = new Respuesta();
            try
            {
                int pedido_numero = lineas.numero_pedido;
                Pedido pedido = this.pedidos.BuscarNumero(pedido_numero);
                if (pedido == null)
                {
                    r.Error("No se encontro el pedido");
                    return r;
                }
                if (pedido.estado == 1)
                {
                    r.Error("Pedido ya finalizado");
                    return r;
                }
                string respuesta = pedido.CompletarLineas(lineas.lineas);
                if (respuesta == null)
                {
                    r.entidad = pedido;
                    r.objeto = lineas;
                }
                else
                {
                    r.Error(respuesta);
                    r.entidad = pedido;
                    r.objeto = lineas;
                }
            }
            catch (Exception e)
            {
                r.Error("Ocurrió un error completando la linea de pedido (CompletarLinea): " + e.Message);
            }
            return r;
        }

        public string ObtenerVersiones(Dictionary<string, long> v)
        {
            Dictionary<string, Tarea> actualizaciones = new Dictionary<string, Tarea>();
            foreach (Tarea t in this.tareas.tareas.Values)
            {
                this.versiones[t.id] = t.version;
                if (v.TryGetValue(t.id, out long v_version))
                {
                    if (v_version != t.version)
                    {
                        actualizaciones.Add(t.id, t);
                    }
                }
            }
            string json_productoIluminado = JsonSerializer.Serialize<Producto>(this.productoIluminado);
            string json_versiones = JsonSerializer.Serialize<Dictionary<string, long>>(this.versiones);
            string json_datos = JsonSerializer.Serialize<Dictionary<string, Tarea>>(actualizaciones);
            return "{\"versiones\":" + json_versiones + ",\"tareas\":" + json_datos + ",\"iluminado\":" + json_productoIluminado + "}";
        }

        public Respuesta ObtenerTarea(Dictionary<string, string> argumentos)
        {
            Respuesta r = new Respuesta();
            try
            {
                int numeroPedido = int.Parse(argumentos["pedido"]);

                Pedido pedido = this.pedidos.BuscarNumero(numeroPedido);
                if (pedido == null)
                {
                    pedido = this.pedidos.BuscarCaja(argumentos["pedido"]);
                }
                if (pedido == null)
                {
                    r.Error("No se encontró el pedido");
                    return r;
                }

                pedido.VincularMovimientos();
                r.objeto = pedido;
            }
            catch (Exception e)
            {
                r.Error("Ocurrió un error durante la edición de la tarea (ObtenerTarea): " + e.Message);
            }
            return r;
        }

        public Respuesta ObtenerStock(Dictionary<string, string> argumentos)
        {
            Respuesta r = new Respuesta();
            try
            {
                int codigoProducto = int.Parse(argumentos["producto"]);
                string lote = argumentos["lote"];

                Producto producto = this.productos.BuscarCodigo(codigoProducto);
                if (producto == null)
                {
                    r.Error("No se encontró el producto");
                    return r;
                }
                r.entidad = producto;
                r.objeto = producto.posiciones;
            }
            catch (Exception e)
            {
                r.Error("Ocurrió un error (ObtenerStock): " + e.Message);
            }
            return r;
        }

        private void temporizador_Tick(object sender, EventArgs e)
        {
            this.controlModulos.Liberar();
        }

        /// <summary>
        /// Importamos los nuevos pedidos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void obtenerPedidosBtn_Click(object sender, EventArgs e)
        {
            ObtenerPedidos();
        }

        public void ObtenerPedidos()
        {
            lock (bloqueoObtenerPedidos)
            {
                CambiarBoton("Actualizando", false);
                Estado("Actualizando pedidos...");
                int cantidad = 0;
                if (ConectorSQL.Comprobar(ConectorSQL.cadenaConexionPS))
                {
                    cantidad = ObtenerPedidosPS();
                }
                Estado("Actualizacion completada, " + cantidad.ToString() + " pedidos añadidos");
                CambiarBoton("Obtener Nuevos Pedidos", true);
            }
        }
        private void CambiarBoton(string titulo, bool enabled)
        {
            try
            {
                //ConectorSQL.InsertarError(texto);
                if (this.obtenerPedidosBtn.InvokeRequired)
                {
                    this.obtenerPedidosBtn.BeginInvoke(
                        (System.Windows.Forms.MethodInvoker)delegate ()
                        {
                            obtenerPedidosBtn.Text = titulo;
                            obtenerPedidosBtn.Enabled = enabled;
                        }
                    );
                }
                else
                {
                    obtenerPedidosBtn.Text = titulo;
                    obtenerPedidosBtn.Enabled = enabled;
                }
            }
            catch
            {
                //
            }
        }

        /// <summary>
        /// Importamos los productos que se hayan creado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void obtenerProductosBtn_Click(object sender, EventArgs e)
        {
            obtenerProductosBtn.Text = "Actualizando";
            obtenerProductosBtn.Enabled = false;

            ActualizarProductos();

            obtenerProductosBtn.Enabled = true;
            obtenerProductosBtn.Text = "Actualizar Productos";
        }

        private void ActualizarProductos()
        {
            Estado("Actualizando productos...");
            //creamos la nueva lista de productos
            Productos nuevosProductos = new Productos();
            //cargamos los productos de las tablas externas
            ConectorSQL.CargarEntidades(ConectorSQL.obtenerProductosPS, nuevosProductos, false, ConectorSQL.cadenaConexionPS);
            //eliminamos los productos coincidentes por codigo
            nuevosProductos.listado.RemoveAll(p => this.productos.ExisteId(p.id));
            if (nuevosProductos.Contador() > 0)
            {
                //agregamos los nuevos a la lista que tenemos
                this.productos.Agregar(nuevosProductos);
                //actualizamos la tabla
                this.MostrarEntidades(this.tablaProductos, nuevosProductos);
                //se vinculan los envases
                nuevosProductos.VincularEnvases(this.envases);
                //es necesario volver a vincular los productos a las lineas de pedido
                this.lineasPedido.VincularProductosNuevos(nuevosProductos);

                //vincular ubicaciones
                nuevosProductos.VincularUbicaciones(this.ubicaciones);
            }
            ConectorJSON.GuardarObjeto("./productos.json", this.productos);
            Estado("Actualizacion completada");
        }

        private void tablaModulos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == 1)
            {
                int centralita_id = (int)this.tablaPosiciones.Rows[e.RowIndex].Cells[4].Value;
                string ip = ((Centralita)this.centralitas.BuscarId(centralita_id)).ip;
                int linea = (int)this.tablaPosiciones.Rows[e.RowIndex].Cells[5].Value;
                int inicio = (int)this.tablaPosiciones.Rows[e.RowIndex].Cells[6].Value;
                int longitud = (int)this.tablaPosiciones.Rows[e.RowIndex].Cells[7].Value;
                ConectorPLC.EncenderReposicion(ip, linea, inicio, longitud);
            }
        }

        public Respuesta ExtraerProducto(Parametros parametros)
        {
            Respuesta r = new Respuesta();
            try
            {
                lock (bloqueoListados)
                {
                    int motivo_id = parametros.BuscarInt("motivo_extraccion");
                    int operario_id = parametros.BuscarInt("operario_extraccion");
                    string posicion_id = parametros.Buscar("posicion_extraccion");
                    int producto_id = parametros.BuscarInt("codigo_extraccion");
                    int cantidad = parametros.BuscarInt("cantidad_extraccion");
                    Operario operario = (Operario)this.operarios.BuscarId(operario_id);
                    string lote = parametros.Buscar("lote_extraccion");

                    if (operario == null)
                    {
                        return r.Error("Operario no válido");
                    }
                    if (motivo_id == 0)
                    {
                        return r.Error("Motivo no válido");
                    }
                    Producto producto = this.productos.BuscarCodigo(producto_id);
                    Posicion posicion = this.posiciones.BuscarNombre(posicion_id);
                    if (producto == null)
                    {
                        return r.Error("Producto no válido");
                    }
                    if (posicion == null)
                    {
                        return r.Error("Posición no válida");
                    }

                    //todo descuenta menos el codigo 19 -> devolución
                    if (motivo_id == 19)
                    {
                        if ((posicion.Disponible() - cantidad) < 0)
                        {
                            return r.Error("Posición sin capacidad para devolución");
                        }
                        else
                        {
                            posicion.ActualizarProducto(producto, lote, cantidad);
                        }
                    }
                    else
                    {
                        if ((posicion.cantidad - cantidad) < 0)
                        {
                            return r.Error("Posición sin stock suficiente para retirar");
                        }
                        else
                        {
                            posicion.ActualizarProducto(producto, lote, -cantidad);
                        }
                    }

                    //actualizar posicion
                    object[] valoresReposicion = {
                        posicion.id,
                        posicion.producto_codigo,
                        posicion.cantidad,
                        posicion.lote
                    };
                    if (ConectorSQL.ActualizarEntidades(ConectorSQL.updateReposicion, valoresReposicion))
                    {
                        gestor.ActualizarTablaPosiciones(posicion);
                    }
                    //insertar movimiento
                    Movimiento m = new Movimiento(parametros, posicion, producto, operario);
                    ConectorSQL.CrearEntidades(ConectorSQL.insertMovimiento, m.GetValoresInsertSQL());
                    this.movimientos.Agregar(m);

                    //actualizar producto
                    r.entidad = producto;
                    r.objeto = posicion;
                }
            }
            catch (Exception e)
            {
                r.Error("Ocurrió un error (ExtraerProducto): " + e.Message);
            }
            return r;
        }

        public Respuesta MoverProducto(Parametros parametros)
        {
            Respuesta r = new Respuesta();
            try
            {
                lock (bloqueoListados)
                {
                    //int operario_id = parametros.BuscarInt("operario_extraccion");
                    string posicion_id = parametros.Buscar("mover_posicion");
                    int producto_id = parametros.BuscarInt("mover_codigo");
                    int operario_id = parametros.BuscarInt("mover_operario");
                    string lote = parametros.Buscar("mover_lote");
                    int cantidad = parametros.BuscarInt("mover_cantidad");
                    string cambiar_posicion = parametros.Buscar("cambiar_posicion");
                    Producto producto = this.productos.BuscarCodigo(producto_id);
                    Posicion posicion = this.posiciones.BuscarNombre(posicion_id);
                    Operario operario = (Operario)this.operarios.BuscarId(operario_id);
                    Posicion nuevaPosicion = this.posiciones.BuscarNombre(cambiar_posicion);
                    if (producto == null)
                    {
                        return r.Error("Producto no válido");
                    }
                    if (posicion == null)
                    {
                        return r.Error("Posición no válida");
                    }
                    if (operario == null)
                    {
                        return r.Error("Operario no válido");
                    }
                    if (cantidad < 1)
                    {
                        return r.Error("Cantidad no válida");
                    }
                    if (cambiar_posicion != "0")
                    {
                        if (nuevaPosicion == null)
                        {
                            return r.Error("Nueva Posición no válida");
                        }
                        if (cantidad > nuevaPosicion.Disponible())
                        {
                            return r.Error("Posición sin capacidad suficiente");
                        }
                        if (nuevaPosicion.lote != "" && posicion.lote != nuevaPosicion.lote)
                        {
                            return r.Error("Lotes no coincidentes");
                        }
                    }

                    posicion.ActualizarProducto(producto, lote, -cantidad);
                    //actualizar sql posicion
                    object[] valoresResta = {
                        posicion.id,
                        posicion.producto_codigo,
                        posicion.cantidad,
                        posicion.lote
                    };
                    if (ConectorSQL.ActualizarEntidades(ConectorSQL.updateReposicion, valoresResta))
                    {
                        gestor.ActualizarTablaPosiciones(posicion);
                    }

                    if (cambiar_posicion != "0")
                    {
                        nuevaPosicion.ActualizarProducto(producto, lote, cantidad);
                        object[] valoresSuma = {
                            nuevaPosicion.id,
                            nuevaPosicion.producto_codigo,
                            nuevaPosicion.cantidad,
                            nuevaPosicion.lote
                        };
                        if (ConectorSQL.ActualizarEntidades(ConectorSQL.updateReposicion, valoresSuma))
                        {
                            gestor.ActualizarTablaPosiciones(nuevaPosicion);
                        }
                    }
                    else
                    {
                        object[] valoresFabricacion = {
                            producto.id,
                            producto.stock + cantidad
                        };
                        if (ConectorSQL.ActualizarEntidades(ConectorSQL.updateFabricacion, valoresFabricacion))
                        {
                            producto.stock += cantidad;
                            gestor.ActualizarTablaProductos(producto);
                        }
                    }

                    //insertar movimiento
                    Movimiento m = new Movimiento(posicion, cambiar_posicion, producto, lote, cantidad, operario);
                    ConectorSQL.CrearEntidades(ConectorSQL.insertMovimiento, m.GetValoresInsertSQL());
                    this.movimientos.Agregar(m);
                    r.entidad = producto;
                    r.objeto = producto.posiciones;
                }
            }
            catch (Exception e)
            {
                r.Error("Ocurrió un error (MoverProducto): " + e.Message);
            }
            return r;
        }

        public Respuesta Fabricar(Dictionary<string, string> argumentos)
        {
            Respuesta r = new Respuesta();
            try
            {
                int codigoProducto = int.Parse(argumentos["codigo_producto"]);
                int cantidad = int.Parse(argumentos["cantidad"]);

                Producto producto = this.productos.BuscarCodigo(codigoProducto);
                if (producto == null)
                {
                    r.Error("No se encontró el producto");
                    return r;
                }
                lock (bloqueoListados)
                {
                    object[] valoresFabricacion = {
                        producto.id,
                        producto.stock + cantidad
                    };
                    if (ConectorSQL.ActualizarEntidades(ConectorSQL.updateFabricacion, valoresFabricacion))
                    {
                        producto.stock += cantidad;
                        gestor.ActualizarTablaProductos(producto);
                    }
                    //insertar movimiento
                    Movimiento m = new Movimiento();
                    m.cantidad = cantidad;
                    m.producto_codigo = codigoProducto;
                    m.accion = 3;

                    ConectorSQL.CrearEntidades(ConectorSQL.insertMovimiento, m.GetValoresInsertSQL());
                    this.movimientos.Agregar(m);
                }
                r.entidad = producto;
            }
            catch (Exception e)
            {
                r.Error("Ocurrió un error (Fabricar): " + e.Message);
            }
            return r;
        }

        public Respuesta EncenderProducto(Dictionary<string, string> argumentos)
        {
            Respuesta r = new Respuesta();
            try
            {
                int codigoProducto = int.Parse(argumentos["producto_codigo"]);
                Producto producto = this.productos.BuscarCodigo(codigoProducto);
                if (producto == null)
                {
                    r.Error("No se encontró el producto");
                    return r;
                }
                if (this.productoIluminado != null)
                {
                    ApagarProducto();
                }
                ConectorPLC.EncenderProducto(producto);
                this.productoIluminado = producto;
                r.entidad = producto;
            }
            catch (Exception e)
            {
                r.Error("Ocurrió un error (EncenderProducto): " + e.Message);
            }
            return r;
        }

        public Respuesta ApagarProducto()
        {
            Respuesta r = new Respuesta();
            try
            {
                ConectorPLC.ApagarProducto();
                this.productoIluminado = null;
            }
            catch (Exception e)
            {
                r.Error("Ocurrió un error (ApagarProducto): " + e.Message);
            }
            return r;
        }

        public void Leds(bool encender, string tipo)
        {
            switch (tipo)
            {
                case "web":
                    if (this.ledPLCs.InvokeRequired)
                    {
                        this.ledPLCs.BeginInvoke(
                            (System.Windows.Forms.MethodInvoker)delegate ()
                            {
                                this.ledPLCs.Visible = encender;
                            }
                        );
                    }
                    else
                    {
                        this.ledPLCs.Visible = encender;
                    }
                    break;
                case "plc":
                    if (this.ledTerminales.InvokeRequired)
                    {
                        this.ledTerminales.BeginInvoke(
                            (System.Windows.Forms.MethodInvoker)delegate ()
                            {
                                this.ledTerminales.Visible = encender;
                            }
                        );
                    }
                    else
                    {
                        this.ledTerminales.Visible = encender;
                    }
                    break;
            }
        }

        public Respuesta ImportarFabricacion(ListaFabricacion listaFabricacion)
        {
            Respuesta r = new Respuesta();
            try
            {
                Dictionary<string, string> argumentos = new Dictionary<string, string>();
                argumentos["codigo_producto"] = "";
                argumentos["cantidad"] = "";
                foreach (ProductoFabricar p in listaFabricacion)
                {
                    argumentos["codigo_producto"] = p.codigo;
                    argumentos["cantidad"] = p.cantidad;
                    r = this.Fabricar(argumentos);
                    if (!r.error)
                    {
                        p.cantidad = "0";
                    }
                }
            }
            catch (Exception e)
            {
                r.Error("Ocurrió un error (ImportarFabricacion): " + e.Message);
            }
            r.objeto = listaFabricacion;
            return r;
        }

        private void CalcularFechas()
        {
            DateTime hoy = DateTime.Now;
            DateTime[] fechas = ObtenerFechasPendientes(hoy, hoy);
            fechaHastaPS.Value = fechas[0];
        }
        private int ObtenerPedidosPS()
        {
            Pedidos nuevosPedidos = new Pedidos();
            try
            {
                string fechaDesde = fechaDesdePS.Value.ToString("yyyy-MM-dd");
                string fechaHasta = fechaHastaPS.Value.ToString("yyyy-MM-dd");
                //Obtenemos los ultimos pedidos

                string cadena = string.Format(ConectorSQL.obtenerPedidosPS, fechaDesde, fechaHasta);
                ConectorSQL.CargarEntidades(cadena, nuevosPedidos, false, ConectorSQL.cadenaConexionPS);
                if (nuevosPedidos.Contador() == 0)
                {
                    return 0;
                }

                LineasPedido nuevasLineasPedido = new LineasPedido();
                cadena = string.Format(ConectorSQL.obtenerLineasPedidoPS, nuevosPedidos.ListadoNumeros());
                ConectorSQL.CargarEntidades(cadena, nuevasLineasPedido, false, ConectorSQL.cadenaConexionPS);

                //Eliminamos los que ya tenemos
                this.pedidos.EliminarRepetidos(nuevosPedidos, nuevasLineasPedido);
                if (nuevosPedidos.Contador() == 0)
                {
                    return 0;
                }
                //sustituimos los menus
                nuevasLineasPedido.SustituirMenus(this.menus);

                //Vinculacion de productos a lineas y de lineas a pedidos
                Dictionary<int, Producto> listadoProductos = this.ListadoProductos();
                nuevasLineasPedido.VincularProductos(listadoProductos);
                nuevosPedidos.VincularLineas(nuevasLineasPedido);
                nuevosPedidos.AgruparLineas(nuevasLineasPedido);

                //Guardamos los nuevos pedidos
                nuevosPedidos.GuardarNuevosPedidos();

                //actualizar vista
                this.lineasPedido.Agregar(nuevasLineasPedido);
                this.pedidos.Agregar(nuevosPedidos);

                this.MostrarEntidades(this.tablaPedidos, nuevosPedidos);
            }
            catch (Exception ex)
            {
                EscribirError(ex.Message);
            }
            return nuevosPedidos.Contador();
        }

        public Respuesta RecargarPedido(int pedido_numero)
        {
            Respuesta r = new Respuesta();
            Pedidos nuevosPedidos = new Pedidos();
            try
            {
                //consultamos si existe el pedido en la BBDD
                string cadena = string.Format(ConectorSQL.selectPedido, pedido_numero);
                if (ConectorSQL.ComprobarPedido(cadena))
                {
                    return r.Error("El pedido ya existe");
                }

                cadena = string.Format(ConectorSQL.obtenerPedidoPS, pedido_numero);
                ConectorSQL.CargarEntidades(cadena, nuevosPedidos, false, ConectorSQL.cadenaConexionPS);
                if (nuevosPedidos.Contador() == 0)
                {
                    return r.Error("Pedido no encontrado");
                }

                LineasPedido nuevasLineasPedido = new LineasPedido();
                cadena = string.Format(ConectorSQL.obtenerLineasPedidoPS, nuevosPedidos.ListadoNumeros());
                ConectorSQL.CargarEntidades(cadena, nuevasLineasPedido, false, ConectorSQL.cadenaConexionPS);

                //sustituimos los menus
                nuevasLineasPedido.SustituirMenus(this.menus);

                //Vinculacion de productos a lineas y de lineas a pedidos
                Dictionary<int, Producto> listadoProductos = this.ListadoProductos();
                nuevasLineasPedido.VincularProductos(listadoProductos);
                nuevosPedidos.VincularLineas(nuevasLineasPedido);
                nuevosPedidos.AgruparLineas(nuevasLineasPedido);

                //Guardamos los nuevos pedidos
                nuevosPedidos.GuardarNuevosPedidos();

                //actualizar vista
                this.lineasPedido.Agregar(nuevasLineasPedido);
                this.pedidos.Agregar(nuevosPedidos);

                this.MostrarEntidades(this.tablaPedidos, nuevosPedidos);
            }
            catch (Exception ex)
            {
                EscribirError(ex.Message);
                r.Error(ex.Message);
            }
            return r;
        }

        public DateTime[] ObtenerFechasPendientes(DateTime desdePeninsula, DateTime desdeBarcelona)
        {
            int[] sumaPeninsula = { 0, 1, 1, 1, 1, 1, 0 };
            int[] sumaBarcelona = { 0, 2, 2, 3, 2, 4, 0 };
            DateTime[] fechas = {
                desdePeninsula.AddDays(sumaPeninsula[(int)desdePeninsula.DayOfWeek]),
                desdeBarcelona.AddDays(sumaBarcelona[(int)desdeBarcelona.DayOfWeek])
            };
            return fechas;
        }

        private void buscarCarpetaBtn_Click(object sender, EventArgs e)
        {
            if (buscador.ShowDialog() == DialogResult.OK)
            {
                pdfEnvioTxt.Text = buscador.SelectedPath + "\\";
                //guardar en configuracion
                //configuracion.carpeta = buscador.SelectedPath + "\\";
            }
        }

        public Respuesta ImprimirEtiqueta(string pedido_numero)
        {
            Respuesta r = new Respuesta();
            try
            {
                Pedido pedido = this.pedidos.BuscarNumero(Int32.Parse(pedido_numero));

                if (pedido == null)
                {
                    return r.Error("No se encontro el pedido");
                }

                //prefijo del archivo
                DateTime hoy = DateTime.Now;
                string prefijo = hoy.ToString("yyyy-MM-dd");
                string numero_pedido = pedido.numero.ToString();
                string sufijo = "";
                switch (pedido.transportista)
                {
                    case 1: //GastroPartner
                        numero_pedido = string.Format("{0}9", pedido.numero.ToString().PadLeft(10, '0'));
                        sufijo = "1";
                        break;
                    case 44:
                        //numero_pedido = string.Format(" 9{0} ", pedido.numero.ToString().PadLeft(10, '0'));
                        numero_pedido = string.Format("{0}9", pedido.numero.ToString());
                        sufijo = "44";
                        break;
                    case 89:
                        numero_pedido = string.Format("{0}9", pedido.numero.ToString());
                        sufijo = "89";
                        break;
                    case 2: //Seur
                        numero_pedido = string.Format("Ref.Exp:9{0}9", pedido.numero.ToString().PadLeft(10, '0'));
                        sufijo = "2";
                        break;
                    case 17: //TIPSA
                        numero_pedido = string.Format("{0}-", pedido.numero.ToString());
                        sufijo = "17";
                        break;
                    case 67: //PAACK
                        numero_pedido = string.Format("{0}9", pedido.numero.ToString());
                        sufijo = "67";
                        break;
                    default:
                        numero_pedido = string.Format("{0}", pedido.numero.ToString());
                        sufijo = pedido.transportista.ToString();
                        break;
                }
                string archivo = string.Format("{0}{1}_{2}.pdf", pdfEnvioTxt.Text, prefijo, sufijo);
                if (File.Exists(archivo))
                {
                    new Thread(new ThreadStart(() => HiloImpresion(archivo, numero_pedido, sufijo))).Start();
                }
                else
                {
                    r.Error("El archivo de etiquetas " + archivo + " no se ha encontrado.");
                }
            }
            catch (Exception ex)
            {
                r.Error("ERROR (ImprimirEtiqueta): " + ex.Message);
            }
            return r;
        }

        private void HiloImpresion(string archivo, string numero_pedido, string sufijo)
        {
            try
            {
                PdfReader pdfReader = new PdfReader(archivo);
                PdfDocument pdfDoc = new PdfDocument(pdfReader);

                for (int index = 1; index <= pdfDoc.GetNumberOfPages(); index++)
                {
                    StringBuilder text = new StringBuilder();
                    PdfPage pagina = pdfDoc.GetPage(index);
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string currentText = PdfTextExtractor.GetTextFromPage(pagina, strategy);
                    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(
                        Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText))
                        );
                    text.Append(currentText);
                    if (text.ToString().Contains(numero_pedido))
                    {
                        string archivo_etiqueta = System.IO.Path.GetTempFileName();
                        PdfDocument etiqueta = new PdfDocument(new PdfWriter(archivo_etiqueta).SetSmartMode(true));
                        //etiqueta.SetDefaultPageSize(PageSize.A5);
                        //pagina.SetTrimBox(new iText.Kernel.Geom.Rectangle(50, 50));

                        pdfDoc.CopyPagesTo(index, index, etiqueta);
                        etiqueta.Close();

                        Imprimir.Imprime(archivo_etiqueta, impresoraEnviosTxt.Text, sufijo);
                        //Imprimir.Imprime(archivo_etiqueta, impresoraEnviosTxt.Text, sufijo);

                        //Console.WriteLine(archivo_etiqueta);
                        //FileSystem.DeleteFile(archivo_etiqueta);

                        /*
                        MemoryStream stream = new MemoryStream();
                        PdfWriter writer = new PdfWriter(stream);
                        PdfDocument pdf = new PdfDocument(new PdfReader(),writer);
                        Document document = new Document(pdf);
                        pdfDoc.CopyPagesTo(index + 1, index + 1, pdf);
                        document.Close();
                        printer.PrintRawStream(impresoraEnviosTxt.Text, stream, numero_pedido);
                        */
                    }
                }
                pdfDoc.Close();
                pdfReader.Close();
            }
            catch (Exception ex)
            {
                this.EscribirError("ERROR (HiloImpresion): " + ex.Message);
            }
        }

        private void seleccionarImpresoraBtn_Click(object sender, EventArgs e)
        {
            string impresora = SeleccionImpresora(this);
            if (impresora != "")
            {
                this.configuracion.impresoraEnvios = impresora;
                impresoraEnviosTxt.Text = impresora;
            }
        }
        public static string SeleccionImpresora(Gestor g)
        {
            PrintDialog pd = new PrintDialog();
            pd.UseEXDialog = true;
            pd.PrinterSettings = new PrinterSettings();
            if (System.Windows.Forms.DialogResult.OK == pd.ShowDialog(g))
            {
                return pd.PrinterSettings.PrinterName;
            }
            else
            {
                return "";
            }
        }

        private void guardarConfiguracionBtn_Click(object sender, EventArgs e)
        {
            try
            {
                this.configuracion.carpetaPDFEnvios = this.pdfEnvioTxt.Text;
                this.configuracion.impresoraEnvios = this.impresoraEnviosTxt.Text;
                object[] valoresConfiguracion = {
                        MySql.Data.MySqlClient.MySqlHelper.EscapeString(this.configuracion.carpetaPDFEnvios),
                        MySql.Data.MySqlClient.MySqlHelper.EscapeString(this.configuracion.impresoraEnvios)
                    };
                ConectorSQL.ActualizarEntidades(ConectorSQL.updateConfiguracion, valoresConfiguracion);
            }
            catch (Exception ex)
            {
                EscribirError("ERROR (Guardar Configuracion): " + ex.Message);
            }
        }

        private void buscador_HelpRequest(object sender, EventArgs e)
        {

        }

        public void AvanzarPedido(MensajeClienteServidor mensaje)
        {
            lock(bloqueoRecogida)
            {
                //se avanza el pedido que este en el modulo uno y se toma el siguiente de la lista
                Pedido p = this.pedidosTransito.AvanzarPedido(mensaje.indice_modulo);
                if (p?.indice_modulo == 5)
                {
                    this.pedidosTransito.Eliminar(p);
                    this.pedidosCompletar.Agregar(p);
                }

                if (mensaje.indice_modulo == 1 && !string.IsNullOrEmpty(mensaje.caja))
                {
                    //se recupera el siguiente pedido de la lista (de la mensajeria), y pasa al modulo 1
                    p = this.pedidos.NuevaCaja(mensaje.transportista, mensaje.caja);
                    this.pedidosTransito.Agregar(p);
                }

                mensaje.accion = "Refrescar";
                mensaje.datos = this.pedidosTransito;
                string m = JsonSerializer.Serialize(mensaje); ;
                _ = mensaje.conexion?.Enviar(m);

                IluminarModulos();
            }
        }

        public void Refrescar(MensajeClienteServidor mensaje)
        {
            mensaje.datos = this.pedidosTransito;
            string m = JsonSerializer.Serialize(mensaje); ;
            _ = mensaje.conexion.Enviar(m);
        }

        public void TraerPedidos(MensajeClienteServidor mensaje)
        {
            lock(bloqueoRecogida)
            {
                mensaje.datos = this.pedidos.PedidosPendientes();
                string m = JsonSerializer.Serialize(mensaje);
                _ = mensaje.conexion.Enviar(m);
            }
        }

        public void ObtenerTransportistas(MensajeClienteServidor mensaje)
        {
            ConectorSQL.CargarEntidades(ConectorSQL.obtenerTransportistasPS, this.transportistas, false, ConectorSQL.cadenaConexionPS);
            mensaje.datos = this.transportistas;
            string m = JsonSerializer.Serialize(mensaje);
            _ = mensaje.conexion.Enviar(m);
        }

        public void IluminarModulos()
        {
            try
            {
                List<string> csv = new List<string>();
                for (int i = 1; i < 5; i++)
                {
                    Pedido pedido = this.pedidosTransito.FiltrarPedidoMinModulo(i);
                    //csv.Add("<Modulo " + i + ">");
                    //csv.Add("#Pedido " + pedido?.numero ?? "0");
                    if (pedido != null)
                    {
                        pedido.ObtenerUbicacionesModulo(i).ConvertAll<string>(ubicacion => ubicacion.ToCSV()).ForEach(csv.Add);
                    }
                }

                string postData = string.Join(";", csv);
                EscribirError("EncenderPedidos: " + postData);
                ConectorPLC.EncenderPedidos("Actualizar " + postData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private void ActualizarStockBtn_Click(object sender, EventArgs e)
        {
            if (ConectorSQL.ActualizarStock()) {
                EscribirEvento("Actualizacion completada");
            } else
            {
                EscribirError("Error durante la actualizacion");
            }
        }
    }
}
