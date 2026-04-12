namespace AutoMarket.Servidor.Presentacion
{
    partial class FrmServidorPrincipal
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlEncabezado = new Panel();
            lblSubtitulo = new Label();
            lblTituloPrincipal = new Label();
            pnlEstadoServidor = new Panel();
            btnDetenerServidor = new Button();
            btnIniciarServidor = new Button();
            lblPuertoValor = new Label();
            lblPuertoTitulo = new Label();
            lblDireccionValor = new Label();
            lblDireccionTitulo = new Label();
            lblConexionesValor = new Label();
            lblConexionesTitulo = new Label();
            lblEstadoValor = new Label();
            lblEstadoTitulo = new Label();
            grpRegistros = new GroupBox();
            tlpRegistros = new TableLayoutPanel();
            btnRegistroCategoriaVehiculo = new Button();
            btnRegistroVehiculo = new Button();
            btnRegistroVendedor = new Button();
            btnRegistroSucursal = new Button();
            btnRegistroCliente = new Button();
            btnRegistroVehiculoXSucursal = new Button();
            grpConsultas = new GroupBox();
            tlpConsultas = new TableLayoutPanel();
            btnConsultaSucursal = new Button();
            btnConsultaVehiculo = new Button();
            btnConsultaVehiculoXSucursal = new Button();
            btnConsultaCategoriaVehiculo = new Button();
            btnConsultaVendedor = new Button();
            btnConsultaCliente = new Button();
            btnConsultaVenta = new Button();
            grpBitacora = new GroupBox();
            txtBitacora = new TextBox();
            pnlBitacoraAcciones = new Panel();
            btnSalir = new Button();
            btnLimpiarBitacora = new Button();
            pnlInfoServidor = new Panel();
            pnlEncabezado.SuspendLayout();
            pnlEstadoServidor.SuspendLayout();
            grpRegistros.SuspendLayout();
            tlpRegistros.SuspendLayout();
            grpConsultas.SuspendLayout();
            tlpConsultas.SuspendLayout();
            grpBitacora.SuspendLayout();
            pnlBitacoraAcciones.SuspendLayout();
            pnlInfoServidor.SuspendLayout();
            SuspendLayout();
            // 
            // pnlEncabezado
            // 
            pnlEncabezado.BackColor = Color.Navy;
            pnlEncabezado.BorderStyle = BorderStyle.Fixed3D;
            pnlEncabezado.Controls.Add(lblSubtitulo);
            pnlEncabezado.Controls.Add(lblTituloPrincipal);
            pnlEncabezado.Dock = DockStyle.Top;
            pnlEncabezado.Location = new Point(0, 0);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Size = new Size(1284, 72);
            pnlEncabezado.TabIndex = 0;
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.BackColor = Color.Transparent;
            lblSubtitulo.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblSubtitulo.ForeColor = Color.White;
            lblSubtitulo.Location = new Point(16, 40);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(376, 13);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "🖥️ Administración del servidor, monitoreo del servicio TCP y acceso a módulos";
            // 
            // lblTituloPrincipal
            // 
            lblTituloPrincipal.AutoSize = true;
            lblTituloPrincipal.BackColor = Color.Transparent;
            lblTituloPrincipal.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold);
            lblTituloPrincipal.ForeColor = Color.White;
            lblTituloPrincipal.Location = new Point(12, 12);
            lblTituloPrincipal.Name = "lblTituloPrincipal";
            lblTituloPrincipal.Size = new Size(227, 24);
            lblTituloPrincipal.TabIndex = 0;
            lblTituloPrincipal.Text = "🚘 AutoMarket Servidor";
            // 
            // pnlEstadoServidor
            // 
            pnlEstadoServidor.BackColor = SystemColors.Control;
            pnlEstadoServidor.BorderStyle = BorderStyle.Fixed3D;
            pnlEstadoServidor.Controls.Add(btnDetenerServidor);
            pnlEstadoServidor.Controls.Add(btnIniciarServidor);
            pnlEstadoServidor.Location = new Point(12, 84);
            pnlEstadoServidor.Name = "pnlEstadoServidor";
            pnlEstadoServidor.Size = new Size(250, 110);
            pnlEstadoServidor.TabIndex = 1;
            // 
            // btnDetenerServidor
            // 
            btnDetenerServidor.BackColor = Color.FromArgb(210, 80, 80);
            btnDetenerServidor.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            btnDetenerServidor.Location = new Point(18, 58);
            btnDetenerServidor.Name = "btnDetenerServidor";
            btnDetenerServidor.Size = new Size(220, 28);
            btnDetenerServidor.TabIndex = 1;
            btnDetenerServidor.Text = "⏹️ Detener servidor";
            btnDetenerServidor.UseVisualStyleBackColor = false;
            btnDetenerServidor.Click += btnDetenerServidor_Click;
            // 
            // btnIniciarServidor
            // 
            btnIniciarServidor.BackColor = Color.FromArgb(110, 205, 110);
            btnIniciarServidor.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            btnIniciarServidor.Location = new Point(18, 24);
            btnIniciarServidor.Name = "btnIniciarServidor";
            btnIniciarServidor.Size = new Size(220, 28);
            btnIniciarServidor.TabIndex = 0;
            btnIniciarServidor.Text = "▶️ Iniciar servidor";
            btnIniciarServidor.UseVisualStyleBackColor = false;
            btnIniciarServidor.Click += btnIniciarServidor_Click;
            // 
            // lblPuertoValor
            // 
            lblPuertoValor.AutoSize = true;
            lblPuertoValor.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            lblPuertoValor.ForeColor = SystemColors.ControlText;
            lblPuertoValor.Location = new Point(584, 50);
            lblPuertoValor.Name = "lblPuertoValor";
            lblPuertoValor.Size = new Size(42, 13);
            lblPuertoValor.TabIndex = 9;
            lblPuertoValor.Text = "15500";
            // 
            // lblPuertoTitulo
            // 
            lblPuertoTitulo.AutoSize = true;
            lblPuertoTitulo.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblPuertoTitulo.Location = new Point(450, 50);
            lblPuertoTitulo.Name = "lblPuertoTitulo";
            lblPuertoTitulo.Size = new Size(80, 13);
            lblPuertoTitulo.TabIndex = 8;
            lblPuertoTitulo.Text = "🔌 Puerto TCP:";
            // 
            // lblDireccionValor
            // 
            lblDireccionValor.AutoSize = true;
            lblDireccionValor.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            lblDireccionValor.ForeColor = SystemColors.ControlText;
            lblDireccionValor.Location = new Point(565, 20);
            lblDireccionValor.Name = "lblDireccionValor";
            lblDireccionValor.Size = new Size(61, 13);
            lblDireccionValor.TabIndex = 7;
            lblDireccionValor.Text = "127.0.0.1";
            // 
            // lblDireccionTitulo
            // 
            lblDireccionTitulo.AutoSize = true;
            lblDireccionTitulo.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblDireccionTitulo.Location = new Point(450, 20);
            lblDireccionTitulo.Name = "lblDireccionTitulo";
            lblDireccionTitulo.Size = new Size(83, 13);
            lblDireccionTitulo.TabIndex = 6;
            lblDireccionTitulo.Text = "🌐 Dirección IP:";
            // 
            // lblConexionesValor
            // 
            lblConexionesValor.AutoSize = true;
            lblConexionesValor.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            lblConexionesValor.ForeColor = SystemColors.ControlText;
            lblConexionesValor.Location = new Point(180, 50);
            lblConexionesValor.Name = "lblConexionesValor";
            lblConexionesValor.Size = new Size(14, 13);
            lblConexionesValor.TabIndex = 5;
            lblConexionesValor.Text = "0";
            // 
            // lblConexionesTitulo
            // 
            lblConexionesTitulo.AutoSize = true;
            lblConexionesTitulo.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblConexionesTitulo.Location = new Point(20, 50);
            lblConexionesTitulo.Name = "lblConexionesTitulo";
            lblConexionesTitulo.Size = new Size(121, 13);
            lblConexionesTitulo.TabIndex = 4;
            lblConexionesTitulo.Text = "👥 Clientes conectados:";
            // 
            // lblEstadoValor
            // 
            lblEstadoValor.AutoSize = true;
            lblEstadoValor.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            lblEstadoValor.ForeColor = Color.Maroon;
            lblEstadoValor.Location = new Point(180, 20);
            lblEstadoValor.Name = "lblEstadoValor";
            lblEstadoValor.Size = new Size(124, 13);
            lblEstadoValor.TabIndex = 3;
            lblEstadoValor.Text = "🔴 Servidor detenido";
            // 
            // lblEstadoTitulo
            // 
            lblEstadoTitulo.AutoSize = true;
            lblEstadoTitulo.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblEstadoTitulo.Location = new Point(20, 20);
            lblEstadoTitulo.Name = "lblEstadoTitulo";
            lblEstadoTitulo.Size = new Size(115, 13);
            lblEstadoTitulo.TabIndex = 2;
            lblEstadoTitulo.Text = "📡 Estado del servidor:";
            // 
            // grpRegistros
            // 
            grpRegistros.Controls.Add(tlpRegistros);
            grpRegistros.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            grpRegistros.Location = new Point(12, 208);
            grpRegistros.Name = "grpRegistros";
            grpRegistros.Size = new Size(360, 332);
            grpRegistros.TabIndex = 2;
            grpRegistros.TabStop = false;
            grpRegistros.Text = "\U0001f9fe Módulos de registro";
            // 
            // tlpRegistros
            // 
            tlpRegistros.ColumnCount = 1;
            tlpRegistros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRegistros.Controls.Add(btnRegistroCategoriaVehiculo, 0, 0);
            tlpRegistros.Controls.Add(btnRegistroVehiculo, 0, 1);
            tlpRegistros.Controls.Add(btnRegistroVendedor, 0, 2);
            tlpRegistros.Controls.Add(btnRegistroSucursal, 0, 3);
            tlpRegistros.Controls.Add(btnRegistroCliente, 0, 4);
            tlpRegistros.Controls.Add(btnRegistroVehiculoXSucursal, 0, 5);
            tlpRegistros.Dock = DockStyle.Fill;
            tlpRegistros.Location = new Point(3, 16);
            tlpRegistros.Name = "tlpRegistros";
            tlpRegistros.Padding = new Padding(8);
            tlpRegistros.RowCount = 6;
            tlpRegistros.RowStyles.Add(new RowStyle(SizeType.Percent, 16.66667F));
            tlpRegistros.RowStyles.Add(new RowStyle(SizeType.Percent, 16.66667F));
            tlpRegistros.RowStyles.Add(new RowStyle(SizeType.Percent, 16.66667F));
            tlpRegistros.RowStyles.Add(new RowStyle(SizeType.Percent, 16.66667F));
            tlpRegistros.RowStyles.Add(new RowStyle(SizeType.Percent, 16.66667F));
            tlpRegistros.RowStyles.Add(new RowStyle(SizeType.Percent, 16.66667F));
            tlpRegistros.Size = new Size(354, 313);
            tlpRegistros.TabIndex = 0;
            // 
            // btnRegistroCategoriaVehiculo
            // 
            btnRegistroCategoriaVehiculo.BackColor = Color.FromArgb(155, 180, 215);
            btnRegistroCategoriaVehiculo.Dock = DockStyle.Fill;
            btnRegistroCategoriaVehiculo.Font = new Font("Microsoft Sans Serif", 8.25F);
            btnRegistroCategoriaVehiculo.Location = new Point(11, 11);
            btnRegistroCategoriaVehiculo.Name = "btnRegistroCategoriaVehiculo";
            btnRegistroCategoriaVehiculo.Size = new Size(332, 43);
            btnRegistroCategoriaVehiculo.TabIndex = 0;
            btnRegistroCategoriaVehiculo.Text = "🗂️ Registrar categoría de vehículo";
            btnRegistroCategoriaVehiculo.UseVisualStyleBackColor = false;
            btnRegistroCategoriaVehiculo.Click += btnRegistroCategoriaVehiculo_Click;
            // 
            // btnRegistroVehiculo
            // 
            btnRegistroVehiculo.BackColor = Color.FromArgb(155, 180, 215);
            btnRegistroVehiculo.Dock = DockStyle.Fill;
            btnRegistroVehiculo.Font = new Font("Microsoft Sans Serif", 8.25F);
            btnRegistroVehiculo.Location = new Point(11, 60);
            btnRegistroVehiculo.Name = "btnRegistroVehiculo";
            btnRegistroVehiculo.Size = new Size(332, 43);
            btnRegistroVehiculo.TabIndex = 1;
            btnRegistroVehiculo.Text = "🚗 Registrar vehículo";
            btnRegistroVehiculo.UseVisualStyleBackColor = false;
            btnRegistroVehiculo.Click += btnRegistroVehiculo_Click;
            // 
            // btnRegistroVendedor
            // 
            btnRegistroVendedor.BackColor = Color.FromArgb(155, 180, 215);
            btnRegistroVendedor.Dock = DockStyle.Fill;
            btnRegistroVendedor.Font = new Font("Microsoft Sans Serif", 8.25F);
            btnRegistroVendedor.Location = new Point(11, 109);
            btnRegistroVendedor.Name = "btnRegistroVendedor";
            btnRegistroVendedor.Size = new Size(332, 43);
            btnRegistroVendedor.TabIndex = 2;
            btnRegistroVendedor.Text = "\U0001f9d1‍💼 Registrar vendedor";
            btnRegistroVendedor.UseVisualStyleBackColor = false;
            btnRegistroVendedor.Click += btnRegistroVendedor_Click;
            // 
            // btnRegistroSucursal
            // 
            btnRegistroSucursal.BackColor = Color.FromArgb(155, 180, 215);
            btnRegistroSucursal.Dock = DockStyle.Fill;
            btnRegistroSucursal.Font = new Font("Microsoft Sans Serif", 8.25F);
            btnRegistroSucursal.Location = new Point(11, 158);
            btnRegistroSucursal.Name = "btnRegistroSucursal";
            btnRegistroSucursal.Size = new Size(332, 43);
            btnRegistroSucursal.TabIndex = 3;
            btnRegistroSucursal.Text = "🏢 Registrar sucursal";
            btnRegistroSucursal.UseVisualStyleBackColor = false;
            btnRegistroSucursal.Click += btnRegistroSucursal_Click;
            // 
            // btnRegistroCliente
            // 
            btnRegistroCliente.BackColor = Color.FromArgb(155, 180, 215);
            btnRegistroCliente.Dock = DockStyle.Fill;
            btnRegistroCliente.Font = new Font("Microsoft Sans Serif", 8.25F);
            btnRegistroCliente.Location = new Point(11, 207);
            btnRegistroCliente.Name = "btnRegistroCliente";
            btnRegistroCliente.Size = new Size(332, 43);
            btnRegistroCliente.TabIndex = 4;
            btnRegistroCliente.Text = "👤 Registrar cliente";
            btnRegistroCliente.UseVisualStyleBackColor = false;
            btnRegistroCliente.Click += btnRegistroCliente_Click;
            // 
            // btnRegistroVehiculoXSucursal
            // 
            btnRegistroVehiculoXSucursal.BackColor = Color.FromArgb(155, 180, 215);
            btnRegistroVehiculoXSucursal.Dock = DockStyle.Fill;
            btnRegistroVehiculoXSucursal.Font = new Font("Microsoft Sans Serif", 8.25F);
            btnRegistroVehiculoXSucursal.Location = new Point(11, 256);
            btnRegistroVehiculoXSucursal.Name = "btnRegistroVehiculoXSucursal";
            btnRegistroVehiculoXSucursal.Size = new Size(332, 46);
            btnRegistroVehiculoXSucursal.TabIndex = 5;
            btnRegistroVehiculoXSucursal.Text = "🏢🚗 Registrar vehículo por sucursal";
            btnRegistroVehiculoXSucursal.UseVisualStyleBackColor = false;
            btnRegistroVehiculoXSucursal.Click += btnRegistroVehiculoXSucursal_Click;
            // 
            // grpConsultas
            // 
            grpConsultas.Controls.Add(tlpConsultas);
            grpConsultas.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            grpConsultas.Location = new Point(388, 208);
            grpConsultas.Name = "grpConsultas";
            grpConsultas.Size = new Size(360, 332);
            grpConsultas.TabIndex = 3;
            grpConsultas.TabStop = false;
            grpConsultas.Text = "📊 Módulos de consulta";
            // 
            // tlpConsultas
            // 
            tlpConsultas.ColumnCount = 1;
            tlpConsultas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpConsultas.Controls.Add(btnConsultaSucursal, 0, 0);
            tlpConsultas.Controls.Add(btnConsultaVehiculo, 0, 1);
            tlpConsultas.Controls.Add(btnConsultaVehiculoXSucursal, 0, 2);
            tlpConsultas.Controls.Add(btnConsultaCategoriaVehiculo, 0, 3);
            tlpConsultas.Controls.Add(btnConsultaVendedor, 0, 4);
            tlpConsultas.Controls.Add(btnConsultaCliente, 0, 5);
            tlpConsultas.Controls.Add(btnConsultaVenta, 0, 6);
            tlpConsultas.Dock = DockStyle.Fill;
            tlpConsultas.Location = new Point(3, 16);
            tlpConsultas.Name = "tlpConsultas";
            tlpConsultas.Padding = new Padding(8);
            tlpConsultas.RowCount = 7;
            tlpConsultas.RowStyles.Add(new RowStyle(SizeType.Percent, 14.28571F));
            tlpConsultas.RowStyles.Add(new RowStyle(SizeType.Percent, 14.28571F));
            tlpConsultas.RowStyles.Add(new RowStyle(SizeType.Percent, 14.28571F));
            tlpConsultas.RowStyles.Add(new RowStyle(SizeType.Percent, 14.28571F));
            tlpConsultas.RowStyles.Add(new RowStyle(SizeType.Percent, 14.28571F));
            tlpConsultas.RowStyles.Add(new RowStyle(SizeType.Percent, 14.28571F));
            tlpConsultas.RowStyles.Add(new RowStyle(SizeType.Percent, 14.28571F));
            tlpConsultas.Size = new Size(354, 313);
            tlpConsultas.TabIndex = 0;
            // 
            // btnConsultaSucursal
            // 
            btnConsultaSucursal.BackColor = Color.FromArgb(135, 185, 225);
            btnConsultaSucursal.Dock = DockStyle.Fill;
            btnConsultaSucursal.Font = new Font("Microsoft Sans Serif", 8.25F);
            btnConsultaSucursal.Location = new Point(11, 11);
            btnConsultaSucursal.Name = "btnConsultaSucursal";
            btnConsultaSucursal.Size = new Size(332, 36);
            btnConsultaSucursal.TabIndex = 0;
            btnConsultaSucursal.Text = "🏢 Consultar sucursales";
            btnConsultaSucursal.UseVisualStyleBackColor = false;
            btnConsultaSucursal.Click += btnConsultaSucursal_Click;
            // 
            // btnConsultaVehiculo
            // 
            btnConsultaVehiculo.BackColor = Color.FromArgb(135, 185, 225);
            btnConsultaVehiculo.Dock = DockStyle.Fill;
            btnConsultaVehiculo.Font = new Font("Microsoft Sans Serif", 8.25F);
            btnConsultaVehiculo.Location = new Point(11, 53);
            btnConsultaVehiculo.Name = "btnConsultaVehiculo";
            btnConsultaVehiculo.Size = new Size(332, 36);
            btnConsultaVehiculo.TabIndex = 1;
            btnConsultaVehiculo.Text = "🚗 Consultar vehículos";
            btnConsultaVehiculo.UseVisualStyleBackColor = false;
            btnConsultaVehiculo.Click += btnConsultaVehiculo_Click;
            // 
            // btnConsultaVehiculoXSucursal
            // 
            btnConsultaVehiculoXSucursal.BackColor = Color.FromArgb(135, 185, 225);
            btnConsultaVehiculoXSucursal.Dock = DockStyle.Fill;
            btnConsultaVehiculoXSucursal.Font = new Font("Microsoft Sans Serif", 8.25F);
            btnConsultaVehiculoXSucursal.Location = new Point(11, 95);
            btnConsultaVehiculoXSucursal.Name = "btnConsultaVehiculoXSucursal";
            btnConsultaVehiculoXSucursal.Size = new Size(332, 36);
            btnConsultaVehiculoXSucursal.TabIndex = 2;
            btnConsultaVehiculoXSucursal.Text = "🏢🚗 Consultar vehículos por sucursal";
            btnConsultaVehiculoXSucursal.UseVisualStyleBackColor = false;
            btnConsultaVehiculoXSucursal.Click += btnConsultaVehiculoXSucursal_Click;
            // 
            // btnConsultaCategoriaVehiculo
            // 
            btnConsultaCategoriaVehiculo.BackColor = Color.FromArgb(135, 185, 225);
            btnConsultaCategoriaVehiculo.Dock = DockStyle.Fill;
            btnConsultaCategoriaVehiculo.Font = new Font("Microsoft Sans Serif", 8.25F);
            btnConsultaCategoriaVehiculo.Location = new Point(11, 137);
            btnConsultaCategoriaVehiculo.Name = "btnConsultaCategoriaVehiculo";
            btnConsultaCategoriaVehiculo.Size = new Size(332, 36);
            btnConsultaCategoriaVehiculo.TabIndex = 3;
            btnConsultaCategoriaVehiculo.Text = "🗂️ Consultar categorías";
            btnConsultaCategoriaVehiculo.UseVisualStyleBackColor = false;
            btnConsultaCategoriaVehiculo.Click += btnConsultaCategoriaVehiculo_Click;
            // 
            // btnConsultaVendedor
            // 
            btnConsultaVendedor.BackColor = Color.FromArgb(135, 185, 225);
            btnConsultaVendedor.Dock = DockStyle.Fill;
            btnConsultaVendedor.Font = new Font("Microsoft Sans Serif", 8.25F);
            btnConsultaVendedor.Location = new Point(11, 179);
            btnConsultaVendedor.Name = "btnConsultaVendedor";
            btnConsultaVendedor.Size = new Size(332, 36);
            btnConsultaVendedor.TabIndex = 4;
            btnConsultaVendedor.Text = "\U0001f9d1‍💼 Consultar vendedores";
            btnConsultaVendedor.UseVisualStyleBackColor = false;
            btnConsultaVendedor.Click += btnConsultaVendedor_Click;
            // 
            // btnConsultaCliente
            // 
            btnConsultaCliente.BackColor = Color.FromArgb(135, 185, 225);
            btnConsultaCliente.Dock = DockStyle.Fill;
            btnConsultaCliente.Font = new Font("Microsoft Sans Serif", 8.25F);
            btnConsultaCliente.Location = new Point(11, 221);
            btnConsultaCliente.Name = "btnConsultaCliente";
            btnConsultaCliente.Size = new Size(332, 36);
            btnConsultaCliente.TabIndex = 5;
            btnConsultaCliente.Text = "👤 Consultar clientes";
            btnConsultaCliente.UseVisualStyleBackColor = false;
            btnConsultaCliente.Click += btnConsultaCliente_Click;
            // 
            // btnConsultaVenta
            // 
            btnConsultaVenta.BackColor = Color.FromArgb(135, 185, 225);
            btnConsultaVenta.Dock = DockStyle.Fill;
            btnConsultaVenta.Font = new Font("Microsoft Sans Serif", 8.25F);
            btnConsultaVenta.Location = new Point(11, 263);
            btnConsultaVenta.Name = "btnConsultaVenta";
            btnConsultaVenta.Size = new Size(332, 39);
            btnConsultaVenta.TabIndex = 6;
            btnConsultaVenta.Text = "💰 Consultar ventas";
            btnConsultaVenta.UseVisualStyleBackColor = false;
            btnConsultaVenta.Click += btnConsultaVenta_Click;
            // 
            // grpBitacora
            // 
            grpBitacora.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            grpBitacora.Controls.Add(txtBitacora);
            grpBitacora.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            grpBitacora.Location = new Point(760, 208);
            grpBitacora.Name = "grpBitacora";
            grpBitacora.Size = new Size(512, 491);
            grpBitacora.TabIndex = 4;
            grpBitacora.TabStop = false;
            grpBitacora.Text = "📝 Bitácora de eventos del servidor";
            // 
            // txtBitacora
            // 
            txtBitacora.BackColor = SystemColors.Window;
            txtBitacora.Dock = DockStyle.Fill;
            txtBitacora.Font = new Font("Courier New", 9F);
            txtBitacora.Location = new Point(3, 16);
            txtBitacora.Multiline = true;
            txtBitacora.Name = "txtBitacora";
            txtBitacora.ReadOnly = true;
            txtBitacora.ScrollBars = ScrollBars.Vertical;
            txtBitacora.Size = new Size(506, 472);
            txtBitacora.TabIndex = 0;
            // 
            // pnlBitacoraAcciones
            // 
            pnlBitacoraAcciones.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            pnlBitacoraAcciones.Controls.Add(btnSalir);
            pnlBitacoraAcciones.Controls.Add(btnLimpiarBitacora);
            pnlBitacoraAcciones.Location = new Point(12, 705);
            pnlBitacoraAcciones.Name = "pnlBitacoraAcciones";
            pnlBitacoraAcciones.Size = new Size(1260, 36);
            pnlBitacoraAcciones.TabIndex = 5;
            // 
            // btnSalir
            // 
            btnSalir.BackColor = Color.FromArgb(200, 0, 0);
            btnSalir.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            btnSalir.ForeColor = Color.White;
            btnSalir.Location = new Point(0, 5);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(140, 26);
            btnSalir.TabIndex = 0;
            btnSalir.Text = "❌ Salir";
            btnSalir.UseVisualStyleBackColor = false;
            btnSalir.Click += btnSalir_Click;
            // 
            // btnLimpiarBitacora
            // 
            btnLimpiarBitacora.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLimpiarBitacora.BackColor = Color.FromArgb(230, 210, 120);
            btnLimpiarBitacora.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            btnLimpiarBitacora.Location = new Point(940, 5);
            btnLimpiarBitacora.Name = "btnLimpiarBitacora";
            btnLimpiarBitacora.Size = new Size(140, 26);
            btnLimpiarBitacora.TabIndex = 1;
            btnLimpiarBitacora.Text = "\U0001f9f9 Limpiar bitácora";
            btnLimpiarBitacora.UseVisualStyleBackColor = false;
            btnLimpiarBitacora.Click += btnLimpiarBitacora_Click;
            // 
            // pnlInfoServidor
            // 
            pnlInfoServidor.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pnlInfoServidor.BackColor = SystemColors.ControlLight;
            pnlInfoServidor.BorderStyle = BorderStyle.Fixed3D;
            pnlInfoServidor.Controls.Add(lblPuertoValor);
            pnlInfoServidor.Controls.Add(lblPuertoTitulo);
            pnlInfoServidor.Controls.Add(lblDireccionValor);
            pnlInfoServidor.Controls.Add(lblDireccionTitulo);
            pnlInfoServidor.Controls.Add(lblConexionesValor);
            pnlInfoServidor.Controls.Add(lblConexionesTitulo);
            pnlInfoServidor.Controls.Add(lblEstadoValor);
            pnlInfoServidor.Controls.Add(lblEstadoTitulo);
            pnlInfoServidor.Location = new Point(616, 84);
            pnlInfoServidor.Name = "pnlInfoServidor";
            pnlInfoServidor.Size = new Size(656, 110);
            pnlInfoServidor.TabIndex = 6;
            // 
            // FrmServidorPrincipal
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1284, 741);
            Controls.Add(pnlBitacoraAcciones);
            Controls.Add(grpBitacora);
            Controls.Add(grpConsultas);
            Controls.Add(grpRegistros);
            Controls.Add(pnlEstadoServidor);
            Controls.Add(pnlInfoServidor);
            Controls.Add(pnlEncabezado);
            Font = new Font("Microsoft Sans Serif", 8.25F);
            MaximizeBox = false;
            Name = "FrmServidorPrincipal";
            Text = "AutoMarket - Servidor Principal";
            FormClosing += FrmServidorPrincipal_FormClosing;
            pnlEncabezado.ResumeLayout(false);
            pnlEncabezado.PerformLayout();
            pnlEstadoServidor.ResumeLayout(false);
            grpRegistros.ResumeLayout(false);
            tlpRegistros.ResumeLayout(false);
            grpConsultas.ResumeLayout(false);
            tlpConsultas.ResumeLayout(false);
            grpBitacora.ResumeLayout(false);
            grpBitacora.PerformLayout();
            pnlBitacoraAcciones.ResumeLayout(false);
            pnlInfoServidor.ResumeLayout(false);
            pnlInfoServidor.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlEncabezado;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Label lblTituloPrincipal;
        private System.Windows.Forms.Panel pnlEstadoServidor;
        private System.Windows.Forms.Label lblPuertoValor;
        private System.Windows.Forms.Label lblPuertoTitulo;
        private System.Windows.Forms.Label lblDireccionValor;
        private System.Windows.Forms.Label lblDireccionTitulo;
        private System.Windows.Forms.Label lblConexionesValor;
        private System.Windows.Forms.Label lblConexionesTitulo;
        private System.Windows.Forms.Label lblEstadoValor;
        private System.Windows.Forms.Label lblEstadoTitulo;
        private System.Windows.Forms.Button btnDetenerServidor;
        private System.Windows.Forms.Button btnIniciarServidor;
        private System.Windows.Forms.GroupBox grpRegistros;
        private System.Windows.Forms.TableLayoutPanel tlpRegistros;
        private System.Windows.Forms.Button btnRegistroVehiculoXSucursal;
        private System.Windows.Forms.Button btnRegistroCliente;
        private System.Windows.Forms.Button btnRegistroSucursal;
        private System.Windows.Forms.Button btnRegistroVendedor;
        private System.Windows.Forms.Button btnRegistroVehiculo;
        private System.Windows.Forms.Button btnRegistroCategoriaVehiculo;
        private System.Windows.Forms.GroupBox grpConsultas;
        private System.Windows.Forms.TableLayoutPanel tlpConsultas;
        private System.Windows.Forms.Button btnConsultaVenta;
        private System.Windows.Forms.Button btnConsultaCliente;
        private System.Windows.Forms.Button btnConsultaVendedor;
        private System.Windows.Forms.Button btnConsultaCategoriaVehiculo;
        private System.Windows.Forms.Button btnConsultaVehiculoXSucursal;
        private System.Windows.Forms.Button btnConsultaVehiculo;
        private System.Windows.Forms.Button btnConsultaSucursal;
        private System.Windows.Forms.GroupBox grpBitacora;
        private System.Windows.Forms.TextBox txtBitacora;
        private System.Windows.Forms.Panel pnlBitacoraAcciones;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Button btnLimpiarBitacora;
        private System.Windows.Forms.Panel pnlInfoServidor;
    }
}