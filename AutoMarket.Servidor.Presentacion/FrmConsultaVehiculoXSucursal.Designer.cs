/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Diseño visual del formulario de consulta de vehículo por sucursal del sistema AutoMarket con estilo clásico de escritorio y visualización en DataGridView.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-02-12
*/

namespace AutoMarket.Servidor.Presentacion
{
    partial class FrmConsultaVehiculoXSucursal
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
            grpFiltro = new GroupBox();
            cmbSucursal = new ComboBox();
            lblSucursal = new Label();
            grpConsulta = new GroupBox();
            dgvVehiculosXSucursal = new DataGridView();
            pnlResumen = new Panel();
            lblCantidadRegistrosValor = new Label();
            lblCantidadRegistros = new Label();
            pnlAcciones = new Panel();
            btnCerrar = new Button();
            btnActualizar = new Button();
            pnlEncabezado.SuspendLayout();
            grpFiltro.SuspendLayout();
            grpConsulta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVehiculosXSucursal).BeginInit();
            pnlResumen.SuspendLayout();
            pnlAcciones.SuspendLayout();
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
            pnlEncabezado.Size = new Size(1284, 76);
            pnlEncabezado.TabIndex = 0;
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.BackColor = Color.Transparent;
            lblSubtitulo.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblSubtitulo.ForeColor = Color.White;
            lblSubtitulo.Location = new Point(16, 42);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(539, 13);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "🚗 Consulta administrativa del inventario de vehículos por sucursal registrado en la base de datos de AutoMarket";
            // 
            // lblTituloPrincipal
            // 
            lblTituloPrincipal.AutoSize = true;
            lblTituloPrincipal.BackColor = Color.Transparent;
            lblTituloPrincipal.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold);
            lblTituloPrincipal.ForeColor = Color.White;
            lblTituloPrincipal.Location = new Point(12, 12);
            lblTituloPrincipal.Name = "lblTituloPrincipal";
            lblTituloPrincipal.Size = new Size(362, 24);
            lblTituloPrincipal.TabIndex = 0;
            lblTituloPrincipal.Text = "🚗 Consulta de Vehículo por Sucursal";
            // 
            // grpFiltro
            // 
            grpFiltro.Controls.Add(cmbSucursal);
            grpFiltro.Controls.Add(lblSucursal);
            grpFiltro.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            grpFiltro.Location = new Point(12, 92);
            grpFiltro.Name = "grpFiltro";
            grpFiltro.Size = new Size(1260, 82);
            grpFiltro.TabIndex = 1;
            grpFiltro.TabStop = false;
            grpFiltro.Text = "🔎 Filtro de consulta";
            // 
            // cmbSucursal
            // 
            cmbSucursal.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSucursal.Font = new Font("Microsoft Sans Serif", 8.25F);
            cmbSucursal.FormattingEnabled = true;
            cmbSucursal.Location = new Point(24, 41);
            cmbSucursal.Name = "cmbSucursal";
            cmbSucursal.Size = new Size(420, 21);
            cmbSucursal.TabIndex = 1;
            cmbSucursal.SelectedIndexChanged += cmbSucursal_SelectedIndexChanged;
            // 
            // lblSucursal
            // 
            lblSucursal.AutoSize = true;
            lblSucursal.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblSucursal.Location = new Point(21, 23);
            lblSucursal.Name = "lblSucursal";
            lblSucursal.Size = new Size(134, 13);
            lblSucursal.TabIndex = 0;
            lblSucursal.Text = "🏢 Sucursal a consultar (*):";
            // 
            // grpConsulta
            // 
            grpConsulta.Controls.Add(dgvVehiculosXSucursal);
            grpConsulta.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            grpConsulta.Location = new Point(12, 188);
            grpConsulta.Name = "grpConsulta";
            grpConsulta.Size = new Size(1260, 398);
            grpConsulta.TabIndex = 2;
            grpConsulta.TabStop = false;
            grpConsulta.Text = "📋 Inventario de vehículos por sucursal";
            // 
            // dgvVehiculosXSucursal
            // 
            dgvVehiculosXSucursal.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVehiculosXSucursal.Location = new Point(16, 24);
            dgvVehiculosXSucursal.Name = "dgvVehiculosXSucursal";
            dgvVehiculosXSucursal.Size = new Size(1228, 356);
            dgvVehiculosXSucursal.TabIndex = 0;
            // 
            // pnlResumen
            // 
            pnlResumen.BorderStyle = BorderStyle.Fixed3D;
            pnlResumen.Controls.Add(lblCantidadRegistrosValor);
            pnlResumen.Controls.Add(lblCantidadRegistros);
            pnlResumen.Location = new Point(12, 600);
            pnlResumen.Name = "pnlResumen";
            pnlResumen.Size = new Size(1260, 46);
            pnlResumen.TabIndex = 3;
            // 
            // lblCantidadRegistrosValor
            // 
            lblCantidadRegistrosValor.AutoSize = true;
            lblCantidadRegistrosValor.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            lblCantidadRegistrosValor.Location = new Point(176, 15);
            lblCantidadRegistrosValor.Name = "lblCantidadRegistrosValor";
            lblCantidadRegistrosValor.Size = new Size(14, 13);
            lblCantidadRegistrosValor.TabIndex = 1;
            lblCantidadRegistrosValor.Text = "0";
            // 
            // lblCantidadRegistros
            // 
            lblCantidadRegistros.AutoSize = true;
            lblCantidadRegistros.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblCantidadRegistros.Location = new Point(14, 15);
            lblCantidadRegistros.Name = "lblCantidadRegistros";
            lblCantidadRegistros.Size = new Size(161, 13);
            lblCantidadRegistros.TabIndex = 0;
            lblCantidadRegistros.Text = "📌 Cantidad de registros visibles:";
            // 
            // pnlAcciones
            // 
            pnlAcciones.BorderStyle = BorderStyle.Fixed3D;
            pnlAcciones.Controls.Add(btnCerrar);
            pnlAcciones.Controls.Add(btnActualizar);
            pnlAcciones.Location = new Point(12, 660);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Size = new Size(1260, 56);
            pnlAcciones.TabIndex = 4;
            // 
            // btnCerrar
            // 
            btnCerrar.BackColor = Color.FromArgb(200, 0, 0);
            btnCerrar.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(1134, 12);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(108, 28);
            btnCerrar.TabIndex = 1;
            btnCerrar.Text = "❌ Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // btnActualizar
            // 
            btnActualizar.BackColor = Color.FromArgb(120, 200, 120);
            btnActualizar.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            btnActualizar.Location = new Point(14, 12);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(128, 28);
            btnActualizar.TabIndex = 0;
            btnActualizar.Text = "🔄 Actualizar";
            btnActualizar.UseVisualStyleBackColor = false;
            btnActualizar.Click += btnActualizar_Click;
            // 
            // FrmConsultaVehiculoXSucursal
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1284, 728);
            Controls.Add(pnlAcciones);
            Controls.Add(pnlResumen);
            Controls.Add(grpConsulta);
            Controls.Add(grpFiltro);
            Controls.Add(pnlEncabezado);
            Font = new Font("Microsoft Sans Serif", 8.25F);
            Name = "FrmConsultaVehiculoXSucursal";
            Text = "AutoMarket - Consulta de Vehículo por Sucursal";
            Load += FrmConsultaVehiculoXSucursal_Load;
            pnlEncabezado.ResumeLayout(false);
            pnlEncabezado.PerformLayout();
            grpFiltro.ResumeLayout(false);
            grpFiltro.PerformLayout();
            grpConsulta.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvVehiculosXSucursal).EndInit();
            pnlResumen.ResumeLayout(false);
            pnlResumen.PerformLayout();
            pnlAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlEncabezado;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Label lblTituloPrincipal;
        private System.Windows.Forms.GroupBox grpFiltro;
        private System.Windows.Forms.ComboBox cmbSucursal;
        private System.Windows.Forms.Label lblSucursal;
        private System.Windows.Forms.GroupBox grpConsulta;
        private System.Windows.Forms.DataGridView dgvVehiculosXSucursal;
        private System.Windows.Forms.Panel pnlResumen;
        private System.Windows.Forms.Label lblCantidadRegistrosValor;
        private System.Windows.Forms.Label lblCantidadRegistros;
        private System.Windows.Forms.Panel pnlAcciones;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Button btnActualizar;
    }
}