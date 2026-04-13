/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Diseño visual del formulario de consulta de vendedores del sistema AutoMarket con estilo clásico de escritorio y visualización en DataGridView.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-02-12
*/

namespace AutoMarket.Servidor.Presentacion
{
    partial class FrmConsultaVendedor
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
            grpConsulta = new GroupBox();
            dgvVendedores = new DataGridView();
            pnlResumen = new Panel();
            lblCantidadRegistrosValor = new Label();
            lblCantidadRegistros = new Label();
            pnlAcciones = new Panel();
            btnCerrar = new Button();
            btnActualizar = new Button();
            pnlEncabezado.SuspendLayout();
            grpConsulta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVendedores).BeginInit();
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
            pnlEncabezado.Size = new Size(1024, 76);
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
            lblSubtitulo.Size = new Size(427, 13);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "👔 Consulta administrativa de vendedores registrados en la base de datos de AutoMarket";
            // 
            // lblTituloPrincipal
            // 
            lblTituloPrincipal.AutoSize = true;
            lblTituloPrincipal.BackColor = Color.Transparent;
            lblTituloPrincipal.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold);
            lblTituloPrincipal.ForeColor = Color.White;
            lblTituloPrincipal.Location = new Point(12, 12);
            lblTituloPrincipal.Name = "lblTituloPrincipal";
            lblTituloPrincipal.Size = new Size(248, 24);
            lblTituloPrincipal.TabIndex = 0;
            lblTituloPrincipal.Text = "👔 Consulta de Vendedor";
            // 
            // grpConsulta
            // 
            grpConsulta.Controls.Add(dgvVendedores);
            grpConsulta.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            grpConsulta.Location = new Point(12, 92);
            grpConsulta.Name = "grpConsulta";
            grpConsulta.Size = new Size(1000, 408);
            grpConsulta.TabIndex = 1;
            grpConsulta.TabStop = false;
            grpConsulta.Text = "📋 Vendedores registrados";
            // 
            // dgvVendedores
            // 
            dgvVendedores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVendedores.Location = new Point(16, 24);
            dgvVendedores.Name = "dgvVendedores";
            dgvVendedores.Size = new Size(968, 366);
            dgvVendedores.TabIndex = 0;
            // 
            // pnlResumen
            // 
            pnlResumen.BorderStyle = BorderStyle.Fixed3D;
            pnlResumen.Controls.Add(lblCantidadRegistrosValor);
            pnlResumen.Controls.Add(lblCantidadRegistros);
            pnlResumen.Location = new Point(12, 514);
            pnlResumen.Name = "pnlResumen";
            pnlResumen.Size = new Size(1000, 46);
            pnlResumen.TabIndex = 2;
            // 
            // lblCantidadRegistrosValor
            // 
            lblCantidadRegistrosValor.AutoSize = true;
            lblCantidadRegistrosValor.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            lblCantidadRegistrosValor.Location = new Point(177, 15);
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
            pnlAcciones.Location = new Point(12, 574);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Size = new Size(1000, 56);
            pnlAcciones.TabIndex = 3;
            // 
            // btnCerrar
            // 
            btnCerrar.BackColor = Color.FromArgb(200, 0, 0);
            btnCerrar.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(874, 12);
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
            // FrmConsultaVendedor
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1024, 642);
            Controls.Add(pnlAcciones);
            Controls.Add(pnlResumen);
            Controls.Add(grpConsulta);
            Controls.Add(pnlEncabezado);
            Font = new Font("Microsoft Sans Serif", 8.25F);
            Name = "FrmConsultaVendedor";
            Text = "AutoMarket - Consulta de Vendedor";
            Load += FrmConsultaVendedor_Load;
            pnlEncabezado.ResumeLayout(false);
            pnlEncabezado.PerformLayout();
            grpConsulta.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvVendedores).EndInit();
            pnlResumen.ResumeLayout(false);
            pnlResumen.PerformLayout();
            pnlAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlEncabezado;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Label lblTituloPrincipal;
        private System.Windows.Forms.GroupBox grpConsulta;
        private System.Windows.Forms.DataGridView dgvVendedores;
        private System.Windows.Forms.Panel pnlResumen;
        private System.Windows.Forms.Label lblCantidadRegistrosValor;
        private System.Windows.Forms.Label lblCantidadRegistros;
        private System.Windows.Forms.Panel pnlAcciones;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Button btnActualizar;
    }
}