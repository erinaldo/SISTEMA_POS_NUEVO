namespace Palatium.Reportes_Formas
{
    partial class frmMostrarReporteComandas
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMostrarReporteComandas));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblTituloReceta = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dgvDatos = new System.Windows.Forms.DataGridView();
            this.lblCantidad = new System.Windows.Forms.Label();
            this.btnVisualizar = new System.Windows.Forms.Button();
            this.ttMensaje = new System.Windows.Forms.ToolTip(this.components);
            this.btnExcel = new System.Windows.Forms.Button();
            this.btnRide = new System.Windows.Forms.Button();
            this.fecha_pedido = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hora_pedido = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numero_pedido = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.origen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mesero = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jornada = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seccion_mesa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mesa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombre_producto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.lblTituloReceta);
            this.panel2.Controls.Add(this.panel8);
            this.panel2.Controls.Add(this.panel7);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btnCerrar);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1180, 51);
            this.panel2.TabIndex = 8;
            // 
            // lblTituloReceta
            // 
            this.lblTituloReceta.AutoSize = true;
            this.lblTituloReceta.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloReceta.Location = new System.Drawing.Point(368, 21);
            this.lblTituloReceta.Name = "lblTituloReceta";
            this.lblTituloReceta.Size = new System.Drawing.Size(0, 19);
            this.lblTituloReceta.TabIndex = 156;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(8)))), ((int)(((byte)(55)))));
            this.panel8.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel8.Location = new System.Drawing.Point(1170, 10);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(10, 41);
            this.panel8.TabIndex = 28;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(8)))), ((int)(((byte)(55)))));
            this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel7.Location = new System.Drawing.Point(0, 10);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(10, 41);
            this.panel7.TabIndex = 27;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(400, 19);
            this.label1.TabIndex = 6;
            this.label1.Text = "Visualización de registros - REPORTE DE COMANDAS";
            // 
            // btnCerrar
            // 
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Image = ((System.Drawing.Image)(resources.GetObject("btnCerrar.Image")));
            this.btnCerrar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCerrar.Location = new System.Drawing.Point(1132, 14);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(32, 35);
            this.btnCerrar.TabIndex = 5;
            this.btnCerrar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(8)))), ((int)(((byte)(55)))));
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1180, 10);
            this.panel4.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(8)))), ((int)(((byte)(55)))));
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 51);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 550);
            this.panel1.TabIndex = 215;
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(8)))), ((int)(((byte)(55)))));
            this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel9.Location = new System.Drawing.Point(0, 601);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(1170, 10);
            this.panel9.TabIndex = 214;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(8)))), ((int)(((byte)(55)))));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(1170, 51);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(10, 560);
            this.panel3.TabIndex = 213;
            // 
            // dgvDatos
            // 
            this.dgvDatos.AllowUserToAddRows = false;
            this.dgvDatos.AllowUserToDeleteRows = false;
            this.dgvDatos.AllowUserToResizeColumns = false;
            this.dgvDatos.AllowUserToResizeRows = false;
            this.dgvDatos.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvDatos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDatos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fecha_pedido,
            this.hora_pedido,
            this.numero_pedido,
            this.origen,
            this.mesero,
            this.jornada,
            this.seccion_mesa,
            this.mesa,
            this.nombre_producto,
            this.total});
            this.dgvDatos.Location = new System.Drawing.Point(16, 67);
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.ReadOnly = true;
            this.dgvDatos.RowHeadersVisible = false;
            this.dgvDatos.Size = new System.Drawing.Size(1148, 473);
            this.dgvDatos.TabIndex = 216;
            // 
            // lblCantidad
            // 
            this.lblCantidad.AutoSize = true;
            this.lblCantidad.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCantidad.Location = new System.Drawing.Point(16, 565);
            this.lblCantidad.Name = "lblCantidad";
            this.lblCantidad.Size = new System.Drawing.Size(71, 17);
            this.lblCantidad.TabIndex = 233;
            this.lblCantidad.Text = "Cantidad";
            // 
            // btnVisualizar
            // 
            this.btnVisualizar.AccessibleDescription = "";
            this.btnVisualizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.btnVisualizar.FlatAppearance.BorderSize = 0;
            this.btnVisualizar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Fuchsia;
            this.btnVisualizar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnVisualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVisualizar.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVisualizar.ForeColor = System.Drawing.Color.White;
            this.btnVisualizar.Image = ((System.Drawing.Image)(resources.GetObject("btnVisualizar.Image")));
            this.btnVisualizar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnVisualizar.Location = new System.Drawing.Point(772, 549);
            this.btnVisualizar.Name = "btnVisualizar";
            this.btnVisualizar.Size = new System.Drawing.Size(146, 46);
            this.btnVisualizar.TabIndex = 258;
            this.btnVisualizar.Text = "   Visualizar";
            this.btnVisualizar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnVisualizar.UseVisualStyleBackColor = false;
            this.btnVisualizar.Click += new System.EventHandler(this.btnVisualizar_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.AccessibleDescription = "";
            this.btnExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.btnExcel.FlatAppearance.BorderSize = 0;
            this.btnExcel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Fuchsia;
            this.btnExcel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcel.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExcel.ForeColor = System.Drawing.Color.White;
            this.btnExcel.Image = ((System.Drawing.Image)(resources.GetObject("btnExcel.Image")));
            this.btnExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExcel.Location = new System.Drawing.Point(924, 549);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(117, 46);
            this.btnExcel.TabIndex = 259;
            this.btnExcel.Text = "   Excel";
            this.btnExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExcel.UseVisualStyleBackColor = false;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnRide
            // 
            this.btnRide.AccessibleDescription = "";
            this.btnRide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.btnRide.FlatAppearance.BorderSize = 0;
            this.btnRide.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Fuchsia;
            this.btnRide.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnRide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRide.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRide.ForeColor = System.Drawing.Color.White;
            this.btnRide.Image = ((System.Drawing.Image)(resources.GetObject("btnRide.Image")));
            this.btnRide.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRide.Location = new System.Drawing.Point(1047, 549);
            this.btnRide.Name = "btnRide";
            this.btnRide.Size = new System.Drawing.Size(117, 46);
            this.btnRide.TabIndex = 260;
            this.btnRide.Text = "   PDF";
            this.btnRide.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRide.UseVisualStyleBackColor = false;
            this.btnRide.Click += new System.EventHandler(this.btnRide_Click);
            // 
            // fecha_pedido
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fecha_pedido.DefaultCellStyle = dataGridViewCellStyle1;
            this.fecha_pedido.HeaderText = "FECHA PEDIDO";
            this.fecha_pedido.Name = "fecha_pedido";
            this.fecha_pedido.ReadOnly = true;
            this.fecha_pedido.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // hora_pedido
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Century Gothic", 9F);
            this.hora_pedido.DefaultCellStyle = dataGridViewCellStyle2;
            this.hora_pedido.HeaderText = "HORA PEDIDO";
            this.hora_pedido.Name = "hora_pedido";
            this.hora_pedido.ReadOnly = true;
            this.hora_pedido.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.hora_pedido.Width = 80;
            // 
            // numero_pedido
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Century Gothic", 9F);
            this.numero_pedido.DefaultCellStyle = dataGridViewCellStyle3;
            this.numero_pedido.HeaderText = "N° PEDIDO";
            this.numero_pedido.Name = "numero_pedido";
            this.numero_pedido.ReadOnly = true;
            this.numero_pedido.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // origen
            // 
            this.origen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Century Gothic", 9F);
            this.origen.DefaultCellStyle = dataGridViewCellStyle4;
            this.origen.HeaderText = "TIPO DE COMANDA";
            this.origen.Name = "origen";
            this.origen.ReadOnly = true;
            this.origen.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // mesero
            // 
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Century Gothic", 9F);
            this.mesero.DefaultCellStyle = dataGridViewCellStyle5;
            this.mesero.HeaderText = "MESERO";
            this.mesero.Name = "mesero";
            this.mesero.ReadOnly = true;
            this.mesero.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.mesero.Width = 130;
            // 
            // jornada
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Century Gothic", 9F);
            this.jornada.DefaultCellStyle = dataGridViewCellStyle6;
            this.jornada.HeaderText = "JORNADA";
            this.jornada.Name = "jornada";
            this.jornada.ReadOnly = true;
            this.jornada.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.jornada.Width = 110;
            // 
            // seccion_mesa
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Century Gothic", 9F);
            this.seccion_mesa.DefaultCellStyle = dataGridViewCellStyle7;
            this.seccion_mesa.HeaderText = "SECCIÓN";
            this.seccion_mesa.Name = "seccion_mesa";
            this.seccion_mesa.ReadOnly = true;
            this.seccion_mesa.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // mesa
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Century Gothic", 9F);
            this.mesa.DefaultCellStyle = dataGridViewCellStyle8;
            this.mesa.HeaderText = "N° MESA";
            this.mesa.Name = "mesa";
            this.mesa.ReadOnly = true;
            this.mesa.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.mesa.Width = 85;
            // 
            // nombre_producto
            // 
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Century Gothic", 9F);
            this.nombre_producto.DefaultCellStyle = dataGridViewCellStyle9;
            this.nombre_producto.HeaderText = "NOMBRE PRODUCTO";
            this.nombre_producto.Name = "nombre_producto";
            this.nombre_producto.ReadOnly = true;
            this.nombre_producto.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.nombre_producto.Width = 200;
            // 
            // total
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Century Gothic", 9F);
            this.total.DefaultCellStyle = dataGridViewCellStyle10;
            this.total.HeaderText = "SUBTOTAL";
            this.total.Name = "total";
            this.total.ReadOnly = true;
            this.total.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.total.Width = 80;
            // 
            // frmMostrarReporteComandas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 611);
            this.Controls.Add(this.btnRide);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.btnVisualizar);
            this.Controls.Add(this.lblCantidad);
            this.Controls.Add(this.dgvDatos);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmMostrarReporteComandas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmMostrarReporteComandas";
            this.Load += new System.EventHandler(this.frmMostrarReporteComandas_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMostrarReporteComandas_KeyDown);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblTituloReceta;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dgvDatos;
        private System.Windows.Forms.Label lblCantidad;
        private System.Windows.Forms.Button btnVisualizar;
        private System.Windows.Forms.ToolTip ttMensaje;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Button btnRide;
        private System.Windows.Forms.DataGridViewTextBoxColumn fecha_pedido;
        private System.Windows.Forms.DataGridViewTextBoxColumn hora_pedido;
        private System.Windows.Forms.DataGridViewTextBoxColumn numero_pedido;
        private System.Windows.Forms.DataGridViewTextBoxColumn origen;
        private System.Windows.Forms.DataGridViewTextBoxColumn mesero;
        private System.Windows.Forms.DataGridViewTextBoxColumn jornada;
        private System.Windows.Forms.DataGridViewTextBoxColumn seccion_mesa;
        private System.Windows.Forms.DataGridViewTextBoxColumn mesa;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre_producto;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
    }
}