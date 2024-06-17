namespace NoCocinoMas
{
    partial class Gestor
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Gestor));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ResetHilos = new System.Windows.Forms.Button();
            this.ledTerminales = new System.Windows.Forms.Label();
            this.ledPLCs = new System.Windows.Forms.Label();
            this.puertoWebTxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.puertoArduinoTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.contenedorTablas = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.datos1 = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.tablaModulos = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aliasModulo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.almacen_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ipModulo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.moduloBloqueante = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tablaCentralitas = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aliasCentralita = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ipCentralita = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tablaAlmacenes = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numeroAlmacen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.datos2 = new System.Windows.Forms.TabPage();
            this.label21 = new System.Windows.Forms.Label();
            this.tablaPosiciones = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.balda = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modulo_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.centralita_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.linea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.led_inicial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.led_longitud = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.producto_codigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.capacidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.datos3 = new System.Windows.Forms.TabPage();
            this.label24 = new System.Windows.Forms.Label();
            this.obtenerProductosBtn = new System.Windows.Forms.Button();
            this.tablaProductos = new System.Windows.Forms.DataGridView();
            this.productos_codigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productos_nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productos_clasificacion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productos_almacenamiento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.datos4 = new System.Windows.Forms.TabPage();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.tablaOperarios = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rol_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tablaRoles = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.datos5 = new System.Windows.Forms.TabPage();
            this.ActualizarStockBtn = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.fechaHastaPS = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.fechaDesdePS = new System.Windows.Forms.DateTimePicker();
            this.tablaMovimientos = new System.Windows.Forms.DataGridView();
            this.codigo_movimiento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.posicion_movimiento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cantidad_movimiento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.obtenerPedidosBtn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.tablaPedidos = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numero = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.envio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transportista = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.estato_pedido = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label5 = new System.Windows.Forms.Label();
            this.tablaLineasPedido = new System.Windows.Forms.DataGridView();
            this.codigo_producto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.producto_pedido = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cantidad_producto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cantidad_recogida = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pestañas = new System.Windows.Forms.TabControl();
            this.tabConfiguracion = new System.Windows.Forms.TabPage();
            this.cuadroCargaBBDD = new System.Windows.Forms.GroupBox();
            this.progresoOrdenes = new System.Windows.Forms.ProgressBar();
            this.label20 = new System.Windows.Forms.Label();
            this.progresoEnvases = new System.Windows.Forms.ProgressBar();
            this.label18 = new System.Windows.Forms.Label();
            this.progresoProductos = new System.Windows.Forms.ProgressBar();
            this.label19 = new System.Windows.Forms.Label();
            this.progresoPosiciones = new System.Windows.Forms.ProgressBar();
            this.label16 = new System.Windows.Forms.Label();
            this.progresoModulos = new System.Windows.Forms.ProgressBar();
            this.label17 = new System.Windows.Forms.Label();
            this.progresoRoles = new System.Windows.Forms.ProgressBar();
            this.label14 = new System.Windows.Forms.Label();
            this.progresoOperarios = new System.Windows.Forms.ProgressBar();
            this.label15 = new System.Windows.Forms.Label();
            this.progresoCentralitas = new System.Windows.Forms.ProgressBar();
            this.label13 = new System.Windows.Forms.Label();
            this.progresoAlmacenes = new System.Windows.Forms.ProgressBar();
            this.label10 = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tablaTareas = new System.Windows.Forms.DataGridView();
            this.id_tareas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.operario_tarea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accion_tarea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pedido_tarea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.producto_tarea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.color_tarea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.moduloTarea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabEtiquetas = new System.Windows.Forms.TabPage();
            this.guardarConfiguracionBtn = new System.Windows.Forms.Button();
            this.grupoConfiguracion = new System.Windows.Forms.GroupBox();
            this.impresoraEnviosTxt = new System.Windows.Forms.Label();
            this.nombreImpresoraTxt = new System.Windows.Forms.Label();
            this.seleccionarImpresoraBtn = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.buscarCarpetaBtn = new System.Windows.Forms.Button();
            this.pdfEnvioTxt = new System.Windows.Forms.TextBox();
            this.tabEventos = new System.Windows.Forms.TabPage();
            this.eventosTxt = new System.Windows.Forms.TextBox();
            this.tabErrores = new System.Windows.Forms.TabPage();
            this.erroresTxt = new System.Windows.Forms.TextBox();
            this.tVersion = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.textoEstado = new System.Windows.Forms.ToolStripStatusLabel();
            this.temporizador = new System.Windows.Forms.Timer(this.components);
            this.buscador = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.contenedorTablas.SuspendLayout();
            this.datos1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablaModulos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaCentralitas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaAlmacenes)).BeginInit();
            this.datos2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablaPosiciones)).BeginInit();
            this.datos3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablaProductos)).BeginInit();
            this.datos4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablaOperarios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaRoles)).BeginInit();
            this.datos5.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablaMovimientos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaPedidos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaLineasPedido)).BeginInit();
            this.pestañas.SuspendLayout();
            this.tabConfiguracion.SuspendLayout();
            this.cuadroCargaBBDD.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablaTareas)).BeginInit();
            this.tabEtiquetas.SuspendLayout();
            this.grupoConfiguracion.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tabEventos.SuspendLayout();
            this.tabErrores.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ResetHilos);
            this.groupBox1.Controls.Add(this.ledTerminales);
            this.groupBox1.Controls.Add(this.ledPLCs);
            this.groupBox1.Controls.Add(this.puertoWebTxt);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.puertoArduinoTxt);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(6, 307);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(325, 231);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datos de Conexión";
            // 
            // ResetHilos
            // 
            this.ResetHilos.Location = new System.Drawing.Point(8, 185);
            this.ResetHilos.Name = "ResetHilos";
            this.ResetHilos.Size = new System.Drawing.Size(205, 36);
            this.ResetHilos.TabIndex = 13;
            this.ResetHilos.Text = "Resetear Servidores";
            this.ResetHilos.UseVisualStyleBackColor = true;
            this.ResetHilos.Click += new System.EventHandler(this.ResetHilos_Click);
            // 
            // ledTerminales
            // 
            this.ledTerminales.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.ledTerminales.Location = new System.Drawing.Point(191, 58);
            this.ledTerminales.Name = "ledTerminales";
            this.ledTerminales.Size = new System.Drawing.Size(22, 20);
            this.ledTerminales.TabIndex = 11;
            this.ledTerminales.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ledTerminales.Visible = false;
            // 
            // ledPLCs
            // 
            this.ledPLCs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.ledPLCs.Location = new System.Drawing.Point(191, 32);
            this.ledPLCs.Name = "ledPLCs";
            this.ledPLCs.Size = new System.Drawing.Size(22, 20);
            this.ledPLCs.TabIndex = 10;
            this.ledPLCs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ledPLCs.Visible = false;
            // 
            // puertoWebTxt
            // 
            this.puertoWebTxt.Enabled = false;
            this.puertoWebTxt.Location = new System.Drawing.Point(109, 32);
            this.puertoWebTxt.Name = "puertoWebTxt";
            this.puertoWebTxt.Size = new System.Drawing.Size(76, 20);
            this.puertoWebTxt.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Puerto Web";
            // 
            // puertoArduinoTxt
            // 
            this.puertoArduinoTxt.Enabled = false;
            this.puertoArduinoTxt.Location = new System.Drawing.Point(109, 58);
            this.puertoArduinoTxt.Name = "puertoArduinoTxt";
            this.puertoArduinoTxt.Size = new System.Drawing.Size(76, 20);
            this.puertoArduinoTxt.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Puerto Centralitas";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.contenedorTablas);
            this.groupBox2.Location = new System.Drawing.Point(337, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(826, 652);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Datos Principales";
            // 
            // contenedorTablas
            // 
            this.contenedorTablas.Controls.Add(this.tabPage1);
            this.contenedorTablas.Controls.Add(this.datos1);
            this.contenedorTablas.Controls.Add(this.datos2);
            this.contenedorTablas.Controls.Add(this.datos3);
            this.contenedorTablas.Controls.Add(this.datos4);
            this.contenedorTablas.Controls.Add(this.datos5);
            this.contenedorTablas.Location = new System.Drawing.Point(6, 19);
            this.contenedorTablas.Name = "contenedorTablas";
            this.contenedorTablas.SelectedIndex = 0;
            this.contenedorTablas.Size = new System.Drawing.Size(814, 627);
            this.contenedorTablas.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(806, 601);
            this.tabPage1.TabIndex = 5;
            this.tabPage1.Text = "Nueva version";
            // 
            // datos1
            // 
            this.datos1.Controls.Add(this.label8);
            this.datos1.Controls.Add(this.tablaModulos);
            this.datos1.Controls.Add(this.label9);
            this.datos1.Controls.Add(this.label11);
            this.datos1.Controls.Add(this.tablaCentralitas);
            this.datos1.Controls.Add(this.tablaAlmacenes);
            this.datos1.Location = new System.Drawing.Point(4, 22);
            this.datos1.Name = "datos1";
            this.datos1.Padding = new System.Windows.Forms.Padding(3);
            this.datos1.Size = new System.Drawing.Size(806, 601);
            this.datos1.TabIndex = 0;
            this.datos1.Text = "Almacenes, Centralitas y Modulos";
            this.datos1.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 127);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Modulos";
            // 
            // tablaModulos
            // 
            this.tablaModulos.AllowUserToAddRows = false;
            this.tablaModulos.AllowUserToDeleteRows = false;
            this.tablaModulos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tablaModulos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn11,
            this.aliasModulo,
            this.dataGridViewTextBoxColumn12,
            this.almacen_id,
            this.ipModulo,
            this.moduloBloqueante});
            this.tablaModulos.Location = new System.Drawing.Point(6, 143);
            this.tablaModulos.MultiSelect = false;
            this.tablaModulos.Name = "tablaModulos";
            this.tablaModulos.ReadOnly = true;
            this.tablaModulos.RowHeadersWidth = 51;
            this.tablaModulos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tablaModulos.Size = new System.Drawing.Size(794, 447);
            this.tablaModulos.TabIndex = 14;
            this.tablaModulos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tablaModulos_CellClick);
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "ID";
            this.dataGridViewTextBoxColumn11.MaxInputLength = 10;
            this.dataGridViewTextBoxColumn11.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            this.dataGridViewTextBoxColumn11.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn11.Width = 50;
            // 
            // aliasModulo
            // 
            this.aliasModulo.HeaderText = "Alias";
            this.aliasModulo.MinimumWidth = 6;
            this.aliasModulo.Name = "aliasModulo";
            this.aliasModulo.ReadOnly = true;
            this.aliasModulo.Width = 150;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.HeaderText = "Nombre";
            this.dataGridViewTextBoxColumn12.MaxInputLength = 6;
            this.dataGridViewTextBoxColumn12.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            this.dataGridViewTextBoxColumn12.Width = 200;
            // 
            // almacen_id
            // 
            this.almacen_id.HeaderText = "Almacen";
            this.almacen_id.MinimumWidth = 6;
            this.almacen_id.Name = "almacen_id";
            this.almacen_id.ReadOnly = true;
            this.almacen_id.Width = 125;
            // 
            // ipModulo
            // 
            this.ipModulo.HeaderText = "IP";
            this.ipModulo.MinimumWidth = 6;
            this.ipModulo.Name = "ipModulo";
            this.ipModulo.ReadOnly = true;
            this.ipModulo.Width = 125;
            // 
            // moduloBloqueante
            // 
            this.moduloBloqueante.HeaderText = "Modulo";
            this.moduloBloqueante.MinimumWidth = 6;
            this.moduloBloqueante.Name = "moduloBloqueante";
            this.moduloBloqueante.ReadOnly = true;
            this.moduloBloqueante.Width = 75;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(337, 7);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Centralitas";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 7);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 13);
            this.label11.TabIndex = 13;
            this.label11.Text = "Almacenes";
            // 
            // tablaCentralitas
            // 
            this.tablaCentralitas.AllowUserToAddRows = false;
            this.tablaCentralitas.AllowUserToDeleteRows = false;
            this.tablaCentralitas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tablaCentralitas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn7,
            this.aliasCentralita,
            this.dataGridViewTextBoxColumn8,
            this.ipCentralita});
            this.tablaCentralitas.Location = new System.Drawing.Point(366, 23);
            this.tablaCentralitas.MultiSelect = false;
            this.tablaCentralitas.Name = "tablaCentralitas";
            this.tablaCentralitas.ReadOnly = true;
            this.tablaCentralitas.RowHeadersWidth = 51;
            this.tablaCentralitas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tablaCentralitas.Size = new System.Drawing.Size(434, 93);
            this.tablaCentralitas.TabIndex = 9;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "ID";
            this.dataGridViewTextBoxColumn7.MaxInputLength = 10;
            this.dataGridViewTextBoxColumn7.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn7.Width = 35;
            // 
            // aliasCentralita
            // 
            this.aliasCentralita.HeaderText = "Alias";
            this.aliasCentralita.MinimumWidth = 6;
            this.aliasCentralita.Name = "aliasCentralita";
            this.aliasCentralita.ReadOnly = true;
            this.aliasCentralita.Width = 70;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Nombre";
            this.dataGridViewTextBoxColumn8.MaxInputLength = 6;
            this.dataGridViewTextBoxColumn8.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Width = 165;
            // 
            // ipCentralita
            // 
            this.ipCentralita.HeaderText = "IP";
            this.ipCentralita.MinimumWidth = 6;
            this.ipCentralita.Name = "ipCentralita";
            this.ipCentralita.ReadOnly = true;
            this.ipCentralita.Width = 125;
            // 
            // tablaAlmacenes
            // 
            this.tablaAlmacenes.AllowUserToAddRows = false;
            this.tablaAlmacenes.AllowUserToDeleteRows = false;
            this.tablaAlmacenes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tablaAlmacenes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn9,
            this.numeroAlmacen,
            this.dataGridViewTextBoxColumn10});
            this.tablaAlmacenes.Location = new System.Drawing.Point(6, 23);
            this.tablaAlmacenes.MultiSelect = false;
            this.tablaAlmacenes.Name = "tablaAlmacenes";
            this.tablaAlmacenes.ReadOnly = true;
            this.tablaAlmacenes.RowHeadersWidth = 51;
            this.tablaAlmacenes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tablaAlmacenes.Size = new System.Drawing.Size(354, 93);
            this.tablaAlmacenes.TabIndex = 12;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "ID";
            this.dataGridViewTextBoxColumn9.MaxInputLength = 10;
            this.dataGridViewTextBoxColumn9.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn9.Width = 35;
            // 
            // numeroAlmacen
            // 
            this.numeroAlmacen.HeaderText = "Número";
            this.numeroAlmacen.MinimumWidth = 6;
            this.numeroAlmacen.Name = "numeroAlmacen";
            this.numeroAlmacen.ReadOnly = true;
            this.numeroAlmacen.Width = 55;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "Nombre";
            this.dataGridViewTextBoxColumn10.MaxInputLength = 6;
            this.dataGridViewTextBoxColumn10.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Width = 200;
            // 
            // datos2
            // 
            this.datos2.Controls.Add(this.label21);
            this.datos2.Controls.Add(this.tablaPosiciones);
            this.datos2.Location = new System.Drawing.Point(4, 22);
            this.datos2.Name = "datos2";
            this.datos2.Padding = new System.Windows.Forms.Padding(3);
            this.datos2.Size = new System.Drawing.Size(806, 601);
            this.datos2.TabIndex = 1;
            this.datos2.Text = "Posiciones";
            this.datos2.UseVisualStyleBackColor = true;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(6, 9);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(58, 13);
            this.label21.TabIndex = 14;
            this.label21.Text = "Posiciones";
            // 
            // tablaPosiciones
            // 
            this.tablaPosiciones.AllowUserToAddRows = false;
            this.tablaPosiciones.AllowUserToDeleteRows = false;
            this.tablaPosiciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tablaPosiciones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn4,
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
            this.estatus});
            this.tablaPosiciones.Location = new System.Drawing.Point(6, 25);
            this.tablaPosiciones.MultiSelect = false;
            this.tablaPosiciones.Name = "tablaPosiciones";
            this.tablaPosiciones.ReadOnly = true;
            this.tablaPosiciones.RowHeadersWidth = 51;
            this.tablaPosiciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tablaPosiciones.Size = new System.Drawing.Size(794, 565);
            this.tablaPosiciones.TabIndex = 8;
            this.tablaPosiciones.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tablaPosiciones_CellContentClick);
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "ID";
            this.dataGridViewTextBoxColumn4.MaxInputLength = 10;
            this.dataGridViewTextBoxColumn4.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn4.Width = 35;
            // 
            // nombre
            // 
            this.nombre.HeaderText = "Nombre";
            this.nombre.MaxInputLength = 50;
            this.nombre.MinimumWidth = 6;
            this.nombre.Name = "nombre";
            this.nombre.ReadOnly = true;
            this.nombre.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.nombre.Width = 125;
            // 
            // balda
            // 
            this.balda.HeaderText = "Balda";
            this.balda.MaxInputLength = 10;
            this.balda.MinimumWidth = 6;
            this.balda.Name = "balda";
            this.balda.ReadOnly = true;
            this.balda.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.balda.Width = 50;
            // 
            // modulo_id
            // 
            this.modulo_id.HeaderText = "Modulo";
            this.modulo_id.MaxInputLength = 20;
            this.modulo_id.MinimumWidth = 6;
            this.modulo_id.Name = "modulo_id";
            this.modulo_id.ReadOnly = true;
            this.modulo_id.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.modulo_id.Width = 50;
            // 
            // centralita_id
            // 
            this.centralita_id.HeaderText = "Centralita";
            this.centralita_id.MaxInputLength = 4;
            this.centralita_id.MinimumWidth = 6;
            this.centralita_id.Name = "centralita_id";
            this.centralita_id.ReadOnly = true;
            this.centralita_id.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.centralita_id.Width = 50;
            // 
            // linea
            // 
            this.linea.HeaderText = "Linea";
            this.linea.MinimumWidth = 6;
            this.linea.Name = "linea";
            this.linea.ReadOnly = true;
            this.linea.Width = 50;
            // 
            // led_inicial
            // 
            this.led_inicial.HeaderText = "Inicio";
            this.led_inicial.MinimumWidth = 6;
            this.led_inicial.Name = "led_inicial";
            this.led_inicial.ReadOnly = true;
            this.led_inicial.Width = 35;
            // 
            // led_longitud
            // 
            this.led_longitud.HeaderText = "Longitud";
            this.led_longitud.MinimumWidth = 6;
            this.led_longitud.Name = "led_longitud";
            this.led_longitud.ReadOnly = true;
            this.led_longitud.Width = 50;
            // 
            // producto_codigo
            // 
            this.producto_codigo.HeaderText = "Producto";
            this.producto_codigo.MinimumWidth = 6;
            this.producto_codigo.Name = "producto_codigo";
            this.producto_codigo.ReadOnly = true;
            this.producto_codigo.Width = 50;
            // 
            // cantidad
            // 
            this.cantidad.HeaderText = "Cantidad";
            this.cantidad.MinimumWidth = 6;
            this.cantidad.Name = "cantidad";
            this.cantidad.ReadOnly = true;
            this.cantidad.Width = 50;
            // 
            // lote
            // 
            this.lote.HeaderText = "Lote";
            this.lote.MinimumWidth = 6;
            this.lote.Name = "lote";
            this.lote.ReadOnly = true;
            this.lote.Width = 90;
            // 
            // capacidad
            // 
            this.capacidad.HeaderText = "Capac.";
            this.capacidad.MinimumWidth = 6;
            this.capacidad.Name = "capacidad";
            this.capacidad.ReadOnly = true;
            this.capacidad.Width = 50;
            // 
            // estatus
            // 
            this.estatus.HeaderText = "Estado";
            this.estatus.MinimumWidth = 6;
            this.estatus.Name = "estatus";
            this.estatus.ReadOnly = true;
            this.estatus.Width = 50;
            // 
            // datos3
            // 
            this.datos3.Controls.Add(this.label24);
            this.datos3.Controls.Add(this.obtenerProductosBtn);
            this.datos3.Controls.Add(this.tablaProductos);
            this.datos3.Location = new System.Drawing.Point(4, 22);
            this.datos3.Name = "datos3";
            this.datos3.Size = new System.Drawing.Size(806, 601);
            this.datos3.TabIndex = 3;
            this.datos3.Text = "Productos y envases";
            this.datos3.UseVisualStyleBackColor = true;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(3, 16);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(81, 20);
            this.label24.TabIndex = 15;
            this.label24.Text = "Productos";
            // 
            // obtenerProductosBtn
            // 
            this.obtenerProductosBtn.Location = new System.Drawing.Point(476, 7);
            this.obtenerProductosBtn.Name = "obtenerProductosBtn";
            this.obtenerProductosBtn.Size = new System.Drawing.Size(319, 39);
            this.obtenerProductosBtn.TabIndex = 21;
            this.obtenerProductosBtn.Text = "Actualizar Productos";
            this.obtenerProductosBtn.UseVisualStyleBackColor = true;
            this.obtenerProductosBtn.Click += new System.EventHandler(this.obtenerProductosBtn_Click);
            // 
            // tablaProductos
            // 
            this.tablaProductos.AllowUserToAddRows = false;
            this.tablaProductos.AllowUserToDeleteRows = false;
            this.tablaProductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tablaProductos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.productos_codigo,
            this.productos_nombre,
            this.productos_clasificacion,
            this.productos_almacenamiento});
            this.tablaProductos.Location = new System.Drawing.Point(6, 52);
            this.tablaProductos.MultiSelect = false;
            this.tablaProductos.Name = "tablaProductos";
            this.tablaProductos.ReadOnly = true;
            this.tablaProductos.RowHeadersWidth = 51;
            this.tablaProductos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tablaProductos.Size = new System.Drawing.Size(789, 546);
            this.tablaProductos.TabIndex = 14;
            // 
            // productos_codigo
            // 
            this.productos_codigo.HeaderText = "Codigo";
            this.productos_codigo.MinimumWidth = 6;
            this.productos_codigo.Name = "productos_codigo";
            this.productos_codigo.ReadOnly = true;
            this.productos_codigo.Width = 150;
            // 
            // productos_nombre
            // 
            this.productos_nombre.HeaderText = "Nombre";
            this.productos_nombre.MaxInputLength = 6;
            this.productos_nombre.MinimumWidth = 6;
            this.productos_nombre.Name = "productos_nombre";
            this.productos_nombre.ReadOnly = true;
            this.productos_nombre.Width = 325;
            // 
            // productos_clasificacion
            // 
            this.productos_clasificacion.HeaderText = "Posicion";
            this.productos_clasificacion.MinimumWidth = 6;
            this.productos_clasificacion.Name = "productos_clasificacion";
            this.productos_clasificacion.ReadOnly = true;
            this.productos_clasificacion.Width = 85;
            // 
            // productos_almacenamiento
            // 
            this.productos_almacenamiento.HeaderText = "Reserva";
            this.productos_almacenamiento.MinimumWidth = 6;
            this.productos_almacenamiento.Name = "productos_almacenamiento";
            this.productos_almacenamiento.ReadOnly = true;
            this.productos_almacenamiento.Width = 150;
            // 
            // datos4
            // 
            this.datos4.Controls.Add(this.label22);
            this.datos4.Controls.Add(this.label23);
            this.datos4.Controls.Add(this.tablaOperarios);
            this.datos4.Controls.Add(this.tablaRoles);
            this.datos4.Location = new System.Drawing.Point(4, 22);
            this.datos4.Name = "datos4";
            this.datos4.Size = new System.Drawing.Size(806, 601);
            this.datos4.TabIndex = 2;
            this.datos4.Text = "Operarios y Roles";
            this.datos4.UseVisualStyleBackColor = true;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(3, 7);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(52, 13);
            this.label22.TabIndex = 15;
            this.label22.Text = "Operarios";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(475, 7);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(34, 13);
            this.label23.TabIndex = 17;
            this.label23.Text = "Roles";
            // 
            // tablaOperarios
            // 
            this.tablaOperarios.AllowUserToAddRows = false;
            this.tablaOperarios.AllowUserToDeleteRows = false;
            this.tablaOperarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tablaOperarios.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.rol_id,
            this.pin});
            this.tablaOperarios.Location = new System.Drawing.Point(6, 23);
            this.tablaOperarios.MultiSelect = false;
            this.tablaOperarios.Name = "tablaOperarios";
            this.tablaOperarios.ReadOnly = true;
            this.tablaOperarios.RowHeadersWidth = 51;
            this.tablaOperarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tablaOperarios.Size = new System.Drawing.Size(466, 567);
            this.tablaOperarios.TabIndex = 14;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "ID";
            this.dataGridViewTextBoxColumn5.MaxInputLength = 10;
            this.dataGridViewTextBoxColumn5.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn5.Width = 35;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Nombre";
            this.dataGridViewTextBoxColumn6.MaxInputLength = 6;
            this.dataGridViewTextBoxColumn6.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 245;
            // 
            // rol_id
            // 
            this.rol_id.HeaderText = "Rol";
            this.rol_id.MinimumWidth = 6;
            this.rol_id.Name = "rol_id";
            this.rol_id.ReadOnly = true;
            this.rol_id.Width = 60;
            // 
            // pin
            // 
            this.pin.HeaderText = "Pin";
            this.pin.MinimumWidth = 6;
            this.pin.Name = "pin";
            this.pin.ReadOnly = true;
            this.pin.Visible = false;
            this.pin.Width = 60;
            // 
            // tablaRoles
            // 
            this.tablaRoles.AllowUserToAddRows = false;
            this.tablaRoles.AllowUserToDeleteRows = false;
            this.tablaRoles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tablaRoles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn14,
            this.dataGridViewTextBoxColumn15});
            this.tablaRoles.Location = new System.Drawing.Point(478, 23);
            this.tablaRoles.MultiSelect = false;
            this.tablaRoles.Name = "tablaRoles";
            this.tablaRoles.ReadOnly = true;
            this.tablaRoles.RowHeadersWidth = 51;
            this.tablaRoles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tablaRoles.Size = new System.Drawing.Size(319, 169);
            this.tablaRoles.TabIndex = 16;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.HeaderText = "ID";
            this.dataGridViewTextBoxColumn14.MaxInputLength = 10;
            this.dataGridViewTextBoxColumn14.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            this.dataGridViewTextBoxColumn14.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn14.Width = 50;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.HeaderText = "Nombre";
            this.dataGridViewTextBoxColumn15.MaxInputLength = 6;
            this.dataGridViewTextBoxColumn15.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.ReadOnly = true;
            this.dataGridViewTextBoxColumn15.Width = 200;
            // 
            // datos5
            // 
            this.datos5.Controls.Add(this.ActualizarStockBtn);
            this.datos5.Controls.Add(this.groupBox7);
            this.datos5.Controls.Add(this.tablaMovimientos);
            this.datos5.Controls.Add(this.obtenerPedidosBtn);
            this.datos5.Controls.Add(this.label6);
            this.datos5.Controls.Add(this.tablaPedidos);
            this.datos5.Controls.Add(this.label5);
            this.datos5.Controls.Add(this.tablaLineasPedido);
            this.datos5.Location = new System.Drawing.Point(4, 22);
            this.datos5.Name = "datos5";
            this.datos5.Size = new System.Drawing.Size(806, 601);
            this.datos5.TabIndex = 4;
            this.datos5.Text = "Pedidos y Contenido";
            this.datos5.UseVisualStyleBackColor = true;
            // 
            // ActualizarStockBtn
            // 
            this.ActualizarStockBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActualizarStockBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ActualizarStockBtn.Location = new System.Drawing.Point(477, 293);
            this.ActualizarStockBtn.Name = "ActualizarStockBtn";
            this.ActualizarStockBtn.Size = new System.Drawing.Size(319, 93);
            this.ActualizarStockBtn.TabIndex = 31;
            this.ActualizarStockBtn.Text = "Actualizar Stock";
            this.ActualizarStockBtn.UseVisualStyleBackColor = true;
            this.ActualizarStockBtn.Click += new System.EventHandler(this.ActualizarStockBtn_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label2);
            this.groupBox7.Controls.Add(this.fechaHastaPS);
            this.groupBox7.Controls.Add(this.label1);
            this.groupBox7.Controls.Add(this.fechaDesdePS);
            this.groupBox7.Location = new System.Drawing.Point(477, 127);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(319, 79);
            this.groupBox7.TabIndex = 30;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Peninsula";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Hasta:";
            // 
            // fechaHastaPS
            // 
            this.fechaHastaPS.Location = new System.Drawing.Point(53, 47);
            this.fechaHastaPS.Name = "fechaHastaPS";
            this.fechaHastaPS.Size = new System.Drawing.Size(260, 20);
            this.fechaHastaPS.TabIndex = 30;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Desde:";
            // 
            // fechaDesdePS
            // 
            this.fechaDesdePS.Location = new System.Drawing.Point(53, 19);
            this.fechaDesdePS.Name = "fechaDesdePS";
            this.fechaDesdePS.Size = new System.Drawing.Size(260, 20);
            this.fechaDesdePS.TabIndex = 28;
            // 
            // tablaMovimientos
            // 
            this.tablaMovimientos.AllowUserToAddRows = false;
            this.tablaMovimientos.AllowUserToDeleteRows = false;
            this.tablaMovimientos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tablaMovimientos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.codigo_movimiento,
            this.posicion_movimiento,
            this.cantidad_movimiento});
            this.tablaMovimientos.Location = new System.Drawing.Point(506, 411);
            this.tablaMovimientos.MultiSelect = false;
            this.tablaMovimientos.Name = "tablaMovimientos";
            this.tablaMovimientos.ReadOnly = true;
            this.tablaMovimientos.RowHeadersWidth = 51;
            this.tablaMovimientos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tablaMovimientos.Size = new System.Drawing.Size(290, 180);
            this.tablaMovimientos.TabIndex = 23;
            // 
            // codigo_movimiento
            // 
            this.codigo_movimiento.HeaderText = "Codigo";
            this.codigo_movimiento.MaxInputLength = 10;
            this.codigo_movimiento.MinimumWidth = 6;
            this.codigo_movimiento.Name = "codigo_movimiento";
            this.codigo_movimiento.ReadOnly = true;
            this.codigo_movimiento.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.codigo_movimiento.Width = 75;
            // 
            // posicion_movimiento
            // 
            this.posicion_movimiento.HeaderText = "Posicion";
            this.posicion_movimiento.MaxInputLength = 6;
            this.posicion_movimiento.MinimumWidth = 6;
            this.posicion_movimiento.Name = "posicion_movimiento";
            this.posicion_movimiento.ReadOnly = true;
            this.posicion_movimiento.Width = 75;
            // 
            // cantidad_movimiento
            // 
            this.cantidad_movimiento.HeaderText = "Cantidad";
            this.cantidad_movimiento.MinimumWidth = 6;
            this.cantidad_movimiento.Name = "cantidad_movimiento";
            this.cantidad_movimiento.ReadOnly = true;
            this.cantidad_movimiento.Width = 75;
            // 
            // obtenerPedidosBtn
            // 
            this.obtenerPedidosBtn.Location = new System.Drawing.Point(477, 27);
            this.obtenerPedidosBtn.Name = "obtenerPedidosBtn";
            this.obtenerPedidosBtn.Size = new System.Drawing.Size(319, 93);
            this.obtenerPedidosBtn.TabIndex = 22;
            this.obtenerPedidosBtn.Text = "Obtener Nuevos Pedidos";
            this.obtenerPedidosBtn.UseVisualStyleBackColor = true;
            this.obtenerPedidosBtn.Click += new System.EventHandler(this.obtenerPedidosBtn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Pedidos pendientes";
            // 
            // tablaPedidos
            // 
            this.tablaPedidos.AllowUserToAddRows = false;
            this.tablaPedidos.AllowUserToDeleteRows = false;
            this.tablaPedidos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tablaPedidos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.numero,
            this.CP,
            this.envio,
            this.transportista,
            this.estato_pedido});
            this.tablaPedidos.Location = new System.Drawing.Point(9, 27);
            this.tablaPedidos.MultiSelect = false;
            this.tablaPedidos.Name = "tablaPedidos";
            this.tablaPedidos.ReadOnly = true;
            this.tablaPedidos.RowHeadersWidth = 51;
            this.tablaPedidos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tablaPedidos.Size = new System.Drawing.Size(462, 359);
            this.tablaPedidos.TabIndex = 14;
            this.tablaPedidos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tablaPedidos_CellClick);
            this.tablaPedidos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tablaPedidos_CellContentClick);
            // 
            // id
            // 
            this.id.HeaderText = "ID";
            this.id.MaxInputLength = 10;
            this.id.MinimumWidth = 6;
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.id.Width = 50;
            // 
            // numero
            // 
            this.numero.HeaderText = "Numero";
            this.numero.MaxInputLength = 6;
            this.numero.MinimumWidth = 6;
            this.numero.Name = "numero";
            this.numero.ReadOnly = true;
            this.numero.Width = 75;
            // 
            // CP
            // 
            this.CP.HeaderText = "CP";
            this.CP.MinimumWidth = 6;
            this.CP.Name = "CP";
            this.CP.ReadOnly = true;
            this.CP.Width = 70;
            // 
            // envio
            // 
            this.envio.HeaderText = "Envio";
            this.envio.MinimumWidth = 6;
            this.envio.Name = "envio";
            this.envio.ReadOnly = true;
            this.envio.Width = 125;
            // 
            // transportista
            // 
            this.transportista.HeaderText = "Transp.";
            this.transportista.MinimumWidth = 6;
            this.transportista.Name = "transportista";
            this.transportista.ReadOnly = true;
            this.transportista.Width = 50;
            // 
            // estato_pedido
            // 
            this.estato_pedido.HeaderText = "Estado";
            this.estato_pedido.MinimumWidth = 6;
            this.estato_pedido.Name = "estato_pedido";
            this.estato_pedido.ReadOnly = true;
            this.estato_pedido.Width = 50;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 394);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Contenido del pedido";
            // 
            // tablaLineasPedido
            // 
            this.tablaLineasPedido.AllowUserToAddRows = false;
            this.tablaLineasPedido.AllowUserToDeleteRows = false;
            this.tablaLineasPedido.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tablaLineasPedido.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.codigo_producto,
            this.producto_pedido,
            this.cantidad_producto,
            this.cantidad_recogida});
            this.tablaLineasPedido.Location = new System.Drawing.Point(9, 411);
            this.tablaLineasPedido.MultiSelect = false;
            this.tablaLineasPedido.Name = "tablaLineasPedido";
            this.tablaLineasPedido.ReadOnly = true;
            this.tablaLineasPedido.RowHeadersWidth = 51;
            this.tablaLineasPedido.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tablaLineasPedido.Size = new System.Drawing.Size(491, 180);
            this.tablaLineasPedido.TabIndex = 14;
            // 
            // codigo_producto
            // 
            this.codigo_producto.HeaderText = "Codigo";
            this.codigo_producto.MaxInputLength = 10;
            this.codigo_producto.MinimumWidth = 6;
            this.codigo_producto.Name = "codigo_producto";
            this.codigo_producto.ReadOnly = true;
            this.codigo_producto.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.codigo_producto.Width = 75;
            // 
            // producto_pedido
            // 
            this.producto_pedido.HeaderText = "Producto";
            this.producto_pedido.MaxInputLength = 6;
            this.producto_pedido.MinimumWidth = 6;
            this.producto_pedido.Name = "producto_pedido";
            this.producto_pedido.ReadOnly = true;
            this.producto_pedido.Width = 250;
            // 
            // cantidad_producto
            // 
            this.cantidad_producto.HeaderText = "Cant.";
            this.cantidad_producto.MinimumWidth = 6;
            this.cantidad_producto.Name = "cantidad_producto";
            this.cantidad_producto.ReadOnly = true;
            this.cantidad_producto.Width = 50;
            // 
            // cantidad_recogida
            // 
            this.cantidad_recogida.HeaderText = "Rec.";
            this.cantidad_recogida.MinimumWidth = 6;
            this.cantidad_recogida.Name = "cantidad_recogida";
            this.cantidad_recogida.ReadOnly = true;
            this.cantidad_recogida.Width = 50;
            // 
            // pestañas
            // 
            this.pestañas.Controls.Add(this.tabConfiguracion);
            this.pestañas.Controls.Add(this.tabControl);
            this.pestañas.Controls.Add(this.tabEtiquetas);
            this.pestañas.Controls.Add(this.tabEventos);
            this.pestañas.Controls.Add(this.tabErrores);
            this.pestañas.Enabled = false;
            this.pestañas.Location = new System.Drawing.Point(4, 12);
            this.pestañas.Name = "pestañas";
            this.pestañas.SelectedIndex = 0;
            this.pestañas.Size = new System.Drawing.Size(1177, 692);
            this.pestañas.TabIndex = 10;
            // 
            // tabConfiguracion
            // 
            this.tabConfiguracion.Controls.Add(this.cuadroCargaBBDD);
            this.tabConfiguracion.Controls.Add(this.groupBox2);
            this.tabConfiguracion.Controls.Add(this.groupBox1);
            this.tabConfiguracion.Location = new System.Drawing.Point(4, 22);
            this.tabConfiguracion.Name = "tabConfiguracion";
            this.tabConfiguracion.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfiguracion.Size = new System.Drawing.Size(1169, 666);
            this.tabConfiguracion.TabIndex = 0;
            this.tabConfiguracion.Text = "Configuración";
            this.tabConfiguracion.UseVisualStyleBackColor = true;
            // 
            // cuadroCargaBBDD
            // 
            this.cuadroCargaBBDD.Controls.Add(this.progresoOrdenes);
            this.cuadroCargaBBDD.Controls.Add(this.label20);
            this.cuadroCargaBBDD.Controls.Add(this.progresoEnvases);
            this.cuadroCargaBBDD.Controls.Add(this.label18);
            this.cuadroCargaBBDD.Controls.Add(this.progresoProductos);
            this.cuadroCargaBBDD.Controls.Add(this.label19);
            this.cuadroCargaBBDD.Controls.Add(this.progresoPosiciones);
            this.cuadroCargaBBDD.Controls.Add(this.label16);
            this.cuadroCargaBBDD.Controls.Add(this.progresoModulos);
            this.cuadroCargaBBDD.Controls.Add(this.label17);
            this.cuadroCargaBBDD.Controls.Add(this.progresoRoles);
            this.cuadroCargaBBDD.Controls.Add(this.label14);
            this.cuadroCargaBBDD.Controls.Add(this.progresoOperarios);
            this.cuadroCargaBBDD.Controls.Add(this.label15);
            this.cuadroCargaBBDD.Controls.Add(this.progresoCentralitas);
            this.cuadroCargaBBDD.Controls.Add(this.label13);
            this.cuadroCargaBBDD.Controls.Add(this.progresoAlmacenes);
            this.cuadroCargaBBDD.Controls.Add(this.label10);
            this.cuadroCargaBBDD.Location = new System.Drawing.Point(6, 8);
            this.cuadroCargaBBDD.Name = "cuadroCargaBBDD";
            this.cuadroCargaBBDD.Size = new System.Drawing.Size(325, 293);
            this.cuadroCargaBBDD.TabIndex = 10;
            this.cuadroCargaBBDD.TabStop = false;
            this.cuadroCargaBBDD.Text = "Carga de BBDD";
            // 
            // progresoOrdenes
            // 
            this.progresoOrdenes.Location = new System.Drawing.Point(119, 246);
            this.progresoOrdenes.Name = "progresoOrdenes";
            this.progresoOrdenes.Size = new System.Drawing.Size(200, 13);
            this.progresoOrdenes.TabIndex = 27;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 246);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(94, 13);
            this.label20.TabIndex = 26;
            this.label20.Text = "Carga de ordenes:";
            // 
            // progresoEnvases
            // 
            this.progresoEnvases.Location = new System.Drawing.Point(119, 168);
            this.progresoEnvases.Name = "progresoEnvases";
            this.progresoEnvases.Size = new System.Drawing.Size(200, 13);
            this.progresoEnvases.TabIndex = 25;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 168);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(96, 13);
            this.label18.TabIndex = 24;
            this.label18.Text = "Carga de envases:";
            // 
            // progresoProductos
            // 
            this.progresoProductos.Location = new System.Drawing.Point(119, 142);
            this.progresoProductos.Name = "progresoProductos";
            this.progresoProductos.Size = new System.Drawing.Size(200, 13);
            this.progresoProductos.TabIndex = 23;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 142);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(103, 13);
            this.label19.TabIndex = 22;
            this.label19.Text = "Carga de productos:";
            // 
            // progresoPosiciones
            // 
            this.progresoPosiciones.Location = new System.Drawing.Point(119, 116);
            this.progresoPosiciones.Name = "progresoPosiciones";
            this.progresoPosiciones.Size = new System.Drawing.Size(200, 13);
            this.progresoPosiciones.TabIndex = 21;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 116);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(106, 13);
            this.label16.TabIndex = 20;
            this.label16.Text = "Carga de posiciones:";
            // 
            // progresoModulos
            // 
            this.progresoModulos.Location = new System.Drawing.Point(119, 90);
            this.progresoModulos.Name = "progresoModulos";
            this.progresoModulos.Size = new System.Drawing.Size(200, 13);
            this.progresoModulos.TabIndex = 19;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 90);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(95, 13);
            this.label17.TabIndex = 18;
            this.label17.Text = "Carga de modulos:";
            // 
            // progresoRoles
            // 
            this.progresoRoles.Location = new System.Drawing.Point(119, 220);
            this.progresoRoles.Name = "progresoRoles";
            this.progresoRoles.Size = new System.Drawing.Size(200, 13);
            this.progresoRoles.TabIndex = 17;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 220);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(78, 13);
            this.label14.TabIndex = 16;
            this.label14.Text = "Carga de roles:";
            // 
            // progresoOperarios
            // 
            this.progresoOperarios.Location = new System.Drawing.Point(119, 194);
            this.progresoOperarios.Name = "progresoOperarios";
            this.progresoOperarios.Size = new System.Drawing.Size(200, 13);
            this.progresoOperarios.TabIndex = 15;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 194);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(99, 13);
            this.label15.TabIndex = 14;
            this.label15.Text = "Carga de operarios:";
            // 
            // progresoCentralitas
            // 
            this.progresoCentralitas.Location = new System.Drawing.Point(119, 64);
            this.progresoCentralitas.Name = "progresoCentralitas";
            this.progresoCentralitas.Size = new System.Drawing.Size(200, 13);
            this.progresoCentralitas.TabIndex = 13;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 64);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(104, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "Carga de centralitas:";
            // 
            // progresoAlmacenes
            // 
            this.progresoAlmacenes.Location = new System.Drawing.Point(119, 38);
            this.progresoAlmacenes.Name = "progresoAlmacenes";
            this.progresoAlmacenes.Size = new System.Drawing.Size(200, 13);
            this.progresoAlmacenes.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 38);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(107, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Carga de almacenes:";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.groupBox3);
            this.tabControl.Controls.Add(this.groupBox4);
            this.tabControl.Location = new System.Drawing.Point(4, 22);
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Windows.Forms.Padding(3);
            this.tabControl.Size = new System.Drawing.Size(1169, 666);
            this.tabControl.TabIndex = 1;
            this.tabControl.Text = "Control Recogida y Reposicion";
            this.tabControl.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox1);
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Location = new System.Drawing.Point(745, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(418, 654);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Control Recogida";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(15, 161);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(131, 17);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "Priorizar lotes antiguos";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.radioButton2);
            this.groupBox5.Controls.Add(this.radioButton1);
            this.groupBox5.Location = new System.Drawing.Point(15, 37);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(386, 100);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Orden de recogida";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(22, 63);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(204, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Orden alternativo (Campo preferencia)";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(22, 30);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(172, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "Orden númerico (modulo 1 a N)";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 580);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(172, 55);
            this.button1.TabIndex = 6;
            this.button1.Text = "Apagar Leds";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tablaTareas);
            this.groupBox4.Location = new System.Drawing.Point(6, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(733, 654);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Operarios activos";
            // 
            // tablaTareas
            // 
            this.tablaTareas.AllowUserToAddRows = false;
            this.tablaTareas.AllowUserToDeleteRows = false;
            this.tablaTareas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tablaTareas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id_tareas,
            this.operario_tarea,
            this.accion_tarea,
            this.pedido_tarea,
            this.producto_tarea,
            this.color_tarea,
            this.moduloTarea});
            this.tablaTareas.Location = new System.Drawing.Point(6, 19);
            this.tablaTareas.Name = "tablaTareas";
            this.tablaTareas.ReadOnly = true;
            this.tablaTareas.RowHeadersWidth = 51;
            this.tablaTareas.RowTemplate.Height = 40;
            this.tablaTareas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tablaTareas.Size = new System.Drawing.Size(722, 629);
            this.tablaTareas.TabIndex = 1;
            this.tablaTareas.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tablaTareas_CellClick);
            // 
            // id_tareas
            // 
            this.id_tareas.HeaderText = "Id";
            this.id_tareas.MinimumWidth = 6;
            this.id_tareas.Name = "id_tareas";
            this.id_tareas.ReadOnly = true;
            this.id_tareas.Width = 50;
            // 
            // operario_tarea
            // 
            this.operario_tarea.HeaderText = "Operario";
            this.operario_tarea.MinimumWidth = 6;
            this.operario_tarea.Name = "operario_tarea";
            this.operario_tarea.ReadOnly = true;
            this.operario_tarea.Width = 225;
            // 
            // accion_tarea
            // 
            this.accion_tarea.FillWeight = 80F;
            this.accion_tarea.HeaderText = "Acción";
            this.accion_tarea.MinimumWidth = 6;
            this.accion_tarea.Name = "accion_tarea";
            this.accion_tarea.ReadOnly = true;
            this.accion_tarea.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.accion_tarea.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.accion_tarea.Width = 80;
            // 
            // pedido_tarea
            // 
            this.pedido_tarea.HeaderText = "Pedido actual";
            this.pedido_tarea.MinimumWidth = 6;
            this.pedido_tarea.Name = "pedido_tarea";
            this.pedido_tarea.ReadOnly = true;
            this.pedido_tarea.Width = 75;
            // 
            // producto_tarea
            // 
            this.producto_tarea.HeaderText = "Producto";
            this.producto_tarea.MinimumWidth = 6;
            this.producto_tarea.Name = "producto_tarea";
            this.producto_tarea.ReadOnly = true;
            this.producto_tarea.Width = 75;
            // 
            // color_tarea
            // 
            this.color_tarea.HeaderText = "Color";
            this.color_tarea.MinimumWidth = 6;
            this.color_tarea.Name = "color_tarea";
            this.color_tarea.ReadOnly = true;
            this.color_tarea.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.color_tarea.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.color_tarea.Width = 50;
            // 
            // moduloTarea
            // 
            this.moduloTarea.HeaderText = "Modulo";
            this.moduloTarea.MinimumWidth = 6;
            this.moduloTarea.Name = "moduloTarea";
            this.moduloTarea.ReadOnly = true;
            this.moduloTarea.Width = 125;
            // 
            // tabEtiquetas
            // 
            this.tabEtiquetas.Controls.Add(this.guardarConfiguracionBtn);
            this.tabEtiquetas.Controls.Add(this.grupoConfiguracion);
            this.tabEtiquetas.Controls.Add(this.groupBox6);
            this.tabEtiquetas.Location = new System.Drawing.Point(4, 22);
            this.tabEtiquetas.Name = "tabEtiquetas";
            this.tabEtiquetas.Padding = new System.Windows.Forms.Padding(3);
            this.tabEtiquetas.Size = new System.Drawing.Size(1169, 666);
            this.tabEtiquetas.TabIndex = 4;
            this.tabEtiquetas.Text = "Etiquetas";
            this.tabEtiquetas.UseVisualStyleBackColor = true;
            // 
            // guardarConfiguracionBtn
            // 
            this.guardarConfiguracionBtn.Location = new System.Drawing.Point(12, 240);
            this.guardarConfiguracionBtn.Name = "guardarConfiguracionBtn";
            this.guardarConfiguracionBtn.Size = new System.Drawing.Size(168, 77);
            this.guardarConfiguracionBtn.TabIndex = 34;
            this.guardarConfiguracionBtn.Text = "Guardar Configuración";
            this.guardarConfiguracionBtn.UseVisualStyleBackColor = true;
            this.guardarConfiguracionBtn.Click += new System.EventHandler(this.guardarConfiguracionBtn_Click);
            // 
            // grupoConfiguracion
            // 
            this.grupoConfiguracion.Controls.Add(this.impresoraEnviosTxt);
            this.grupoConfiguracion.Controls.Add(this.nombreImpresoraTxt);
            this.grupoConfiguracion.Controls.Add(this.seleccionarImpresoraBtn);
            this.grupoConfiguracion.Location = new System.Drawing.Point(388, 6);
            this.grupoConfiguracion.Name = "grupoConfiguracion";
            this.grupoConfiguracion.Size = new System.Drawing.Size(226, 101);
            this.grupoConfiguracion.TabIndex = 35;
            this.grupoConfiguracion.TabStop = false;
            this.grupoConfiguracion.Text = "Impresora por defecto";
            // 
            // impresoraEnviosTxt
            // 
            this.impresoraEnviosTxt.AutoSize = true;
            this.impresoraEnviosTxt.Location = new System.Drawing.Point(9, 33);
            this.impresoraEnviosTxt.Name = "impresoraEnviosTxt";
            this.impresoraEnviosTxt.Size = new System.Drawing.Size(0, 13);
            this.impresoraEnviosTxt.TabIndex = 19;
            // 
            // nombreImpresoraTxt
            // 
            this.nombreImpresoraTxt.AutoSize = true;
            this.nombreImpresoraTxt.Location = new System.Drawing.Point(9, 26);
            this.nombreImpresoraTxt.Name = "nombreImpresoraTxt";
            this.nombreImpresoraTxt.Size = new System.Drawing.Size(0, 13);
            this.nombreImpresoraTxt.TabIndex = 18;
            // 
            // seleccionarImpresoraBtn
            // 
            this.seleccionarImpresoraBtn.Location = new System.Drawing.Point(6, 55);
            this.seleccionarImpresoraBtn.Name = "seleccionarImpresoraBtn";
            this.seleccionarImpresoraBtn.Size = new System.Drawing.Size(214, 32);
            this.seleccionarImpresoraBtn.TabIndex = 17;
            this.seleccionarImpresoraBtn.Text = "Seleccionar Impresora";
            this.seleccionarImpresoraBtn.UseVisualStyleBackColor = true;
            this.seleccionarImpresoraBtn.Click += new System.EventHandler(this.seleccionarImpresoraBtn_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.buscarCarpetaBtn);
            this.groupBox6.Controls.Add(this.pdfEnvioTxt);
            this.groupBox6.Location = new System.Drawing.Point(6, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(376, 101);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Carpeta PDF Etiquetas de envío";
            // 
            // buscarCarpetaBtn
            // 
            this.buscarCarpetaBtn.Location = new System.Drawing.Point(6, 26);
            this.buscarCarpetaBtn.Name = "buscarCarpetaBtn";
            this.buscarCarpetaBtn.Size = new System.Drawing.Size(137, 31);
            this.buscarCarpetaBtn.TabIndex = 33;
            this.buscarCarpetaBtn.Text = "Buscar Carpeta";
            this.buscarCarpetaBtn.UseVisualStyleBackColor = true;
            this.buscarCarpetaBtn.Click += new System.EventHandler(this.buscarCarpetaBtn_Click);
            // 
            // pdfEnvioTxt
            // 
            this.pdfEnvioTxt.Location = new System.Drawing.Point(6, 67);
            this.pdfEnvioTxt.Name = "pdfEnvioTxt";
            this.pdfEnvioTxt.Size = new System.Drawing.Size(364, 20);
            this.pdfEnvioTxt.TabIndex = 32;
            // 
            // tabEventos
            // 
            this.tabEventos.Controls.Add(this.eventosTxt);
            this.tabEventos.Location = new System.Drawing.Point(4, 22);
            this.tabEventos.Name = "tabEventos";
            this.tabEventos.Size = new System.Drawing.Size(1169, 666);
            this.tabEventos.TabIndex = 2;
            this.tabEventos.Text = "Eventos";
            this.tabEventos.UseVisualStyleBackColor = true;
            // 
            // eventosTxt
            // 
            this.eventosTxt.Location = new System.Drawing.Point(3, 3);
            this.eventosTxt.Multiline = true;
            this.eventosTxt.Name = "eventosTxt";
            this.eventosTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.eventosTxt.Size = new System.Drawing.Size(1161, 660);
            this.eventosTxt.TabIndex = 12;
            // 
            // tabErrores
            // 
            this.tabErrores.Controls.Add(this.erroresTxt);
            this.tabErrores.Location = new System.Drawing.Point(4, 22);
            this.tabErrores.Name = "tabErrores";
            this.tabErrores.Padding = new System.Windows.Forms.Padding(3);
            this.tabErrores.Size = new System.Drawing.Size(1169, 666);
            this.tabErrores.TabIndex = 3;
            this.tabErrores.Text = "Errores";
            this.tabErrores.UseVisualStyleBackColor = true;
            // 
            // erroresTxt
            // 
            this.erroresTxt.Location = new System.Drawing.Point(3, 3);
            this.erroresTxt.Multiline = true;
            this.erroresTxt.Name = "erroresTxt";
            this.erroresTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.erroresTxt.Size = new System.Drawing.Size(1160, 657);
            this.erroresTxt.TabIndex = 8;
            // 
            // tVersion
            // 
            this.tVersion.AutoSize = true;
            this.tVersion.Location = new System.Drawing.Point(1123, 9);
            this.tVersion.Name = "tVersion";
            this.tVersion.Size = new System.Drawing.Size(43, 13);
            this.tVersion.TabIndex = 11;
            this.tVersion.Text = "v 2.003";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.textoEstado});
            this.statusStrip1.Location = new System.Drawing.Point(0, 720);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1184, 22);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // textoEstado
            // 
            this.textoEstado.Name = "textoEstado";
            this.textoEstado.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.textoEstado.Size = new System.Drawing.Size(128, 17);
            this.textoEstado.Text = "toolStripStatusLabel1";
            // 
            // temporizador
            // 
            this.temporizador.Interval = 500;
            this.temporizador.Tick += new System.EventHandler(this.temporizador_Tick);
            // 
            // buscador
            // 
            this.buscador.HelpRequest += new System.EventHandler(this.buscador_HelpRequest);
            // 
            // Gestor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1184, 742);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tVersion);
            this.Controls.Add(this.pestañas);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1200, 780);
            this.Name = "Gestor";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestor Pedidos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Gestor_FormClosing);
            this.Load += new System.EventHandler(this.Gestor_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.contenedorTablas.ResumeLayout(false);
            this.datos1.ResumeLayout(false);
            this.datos1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablaModulos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaCentralitas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaAlmacenes)).EndInit();
            this.datos2.ResumeLayout(false);
            this.datos2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablaPosiciones)).EndInit();
            this.datos3.ResumeLayout(false);
            this.datos3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablaProductos)).EndInit();
            this.datos4.ResumeLayout(false);
            this.datos4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablaOperarios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaRoles)).EndInit();
            this.datos5.ResumeLayout(false);
            this.datos5.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablaMovimientos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaPedidos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaLineasPedido)).EndInit();
            this.pestañas.ResumeLayout(false);
            this.tabConfiguracion.ResumeLayout(false);
            this.cuadroCargaBBDD.ResumeLayout(false);
            this.cuadroCargaBBDD.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablaTareas)).EndInit();
            this.tabEtiquetas.ResumeLayout(false);
            this.grupoConfiguracion.ResumeLayout(false);
            this.grupoConfiguracion.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tabEventos.ResumeLayout(false);
            this.tabEventos.PerformLayout();
            this.tabErrores.ResumeLayout(false);
            this.tabErrores.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TextBox puertoWebTxt;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox puertoArduinoTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabControl pestañas;
        private System.Windows.Forms.TabPage tabConfiguracion;
        private System.Windows.Forms.TabPage tabControl;
        public System.Windows.Forms.TextBox erroresTxt;
        private System.Windows.Forms.DataGridView tablaPosiciones;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridView tablaCentralitas;
        private System.Windows.Forms.TabPage tabEventos;
        public System.Windows.Forms.TextBox eventosTxt;
        private System.Windows.Forms.Label tVersion;
        private System.Windows.Forms.TabPage tabErrores;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DataGridView tablaAlmacenes;
        private System.Windows.Forms.Label ledTerminales;
        private System.Windows.Forms.Label ledPLCs;
        private System.Windows.Forms.Button ResetHilos;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel textoEstado;
        private System.Windows.Forms.GroupBox cuadroCargaBBDD;
        private System.Windows.Forms.ProgressBar progresoAlmacenes;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ProgressBar progresoEnvases;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ProgressBar progresoProductos;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ProgressBar progresoPosiciones;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ProgressBar progresoModulos;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ProgressBar progresoRoles;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ProgressBar progresoOperarios;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ProgressBar progresoCentralitas;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ProgressBar progresoOrdenes;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TabControl contenedorTablas;
        private System.Windows.Forms.TabPage datos1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView tablaModulos;
        private System.Windows.Forms.TabPage datos2;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TabPage datos4;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.DataGridView tablaOperarios;
        private System.Windows.Forms.DataGridView tablaRoles;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.TabPage datos3;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.DataGridView tablaProductos;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView tablaTareas;
        private System.Windows.Forms.TabPage datos5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView tablaPedidos;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView tablaLineasPedido;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button obtenerPedidosBtn;
        private System.Windows.Forms.Button obtenerProductosBtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn numeroAlmacen;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn aliasCentralita;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn ipCentralita;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn balda;
        private System.Windows.Forms.DataGridViewTextBoxColumn modulo_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn centralita_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn linea;
        private System.Windows.Forms.DataGridViewTextBoxColumn led_inicial;
        private System.Windows.Forms.DataGridViewTextBoxColumn led_longitud;
        private System.Windows.Forms.DataGridViewTextBoxColumn producto_codigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn cantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn lote;
        private System.Windows.Forms.DataGridViewTextBoxColumn capacidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn estatus;
        private System.Windows.Forms.Timer temporizador;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn aliasModulo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn almacen_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn ipModulo;
        private System.Windows.Forms.DataGridViewTextBoxColumn moduloBloqueante;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_tareas;
        private System.Windows.Forms.DataGridViewTextBoxColumn operario_tarea;
        private System.Windows.Forms.DataGridViewTextBoxColumn accion_tarea;
        private System.Windows.Forms.DataGridViewTextBoxColumn pedido_tarea;
        private System.Windows.Forms.DataGridViewTextBoxColumn producto_tarea;
        private System.Windows.Forms.DataGridViewTextBoxColumn color_tarea;
        private System.Windows.Forms.DataGridViewTextBoxColumn moduloTarea;
        private System.Windows.Forms.DataGridView tablaMovimientos;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigo_movimiento;
        private System.Windows.Forms.DataGridViewTextBoxColumn posicion_movimiento;
        private System.Windows.Forms.DataGridViewTextBoxColumn cantidad_movimiento;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigo_producto;
        private System.Windows.Forms.DataGridViewTextBoxColumn producto_pedido;
        private System.Windows.Forms.DataGridViewTextBoxColumn cantidad_producto;
        private System.Windows.Forms.DataGridViewTextBoxColumn cantidad_recogida;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn rol_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn pin;
        private System.Windows.Forms.DateTimePicker fechaDesdePS;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn numero;
        private System.Windows.Forms.DataGridViewTextBoxColumn CP;
        private System.Windows.Forms.DataGridViewTextBoxColumn envio;
        private System.Windows.Forms.DataGridViewTextBoxColumn transportista;
        private System.Windows.Forms.DataGridViewTextBoxColumn estato_pedido;
        private System.Windows.Forms.TabPage tabEtiquetas;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button guardarConfiguracionBtn;
        private System.Windows.Forms.Button buscarCarpetaBtn;
        private System.Windows.Forms.TextBox pdfEnvioTxt;
        private System.Windows.Forms.GroupBox grupoConfiguracion;
        private System.Windows.Forms.Label nombreImpresoraTxt;
        private System.Windows.Forms.Button seleccionarImpresoraBtn;
        private System.Windows.Forms.Label impresoraEnviosTxt;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker fechaHastaPS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn productos_codigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn productos_nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn productos_clasificacion;
        private System.Windows.Forms.DataGridViewTextBoxColumn productos_almacenamiento;
        private System.Windows.Forms.FolderBrowserDialog buscador;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button ActualizarStockBtn;
    }
}

