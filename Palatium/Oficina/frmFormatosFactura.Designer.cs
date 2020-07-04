﻿namespace Palatium.Oficina
{
    partial class frmFormatosFactura
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
            this.btnEliminar = new System.Windows.Forms.Button();
            this.Grb_listReCanImpre = new System.Windows.Forms.GroupBox();
            this.btnBuscarCanImpre = new System.Windows.Forms.Button();
            this.txtBusqueda = new System.Windows.Forms.TextBox();
            this.dgvDatos = new System.Windows.Forms.DataGridView();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.cmbEstado = new System.Windows.Forms.ComboBox();
            this.lblEstaCajero = new System.Windows.Forms.Label();
            this.lbldescrCajero = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.lblcodigoCajero = new System.Windows.Forms.Label();
            this.grupoDatos = new System.Windows.Forms.GroupBox();
            this.cmbImpresoras = new ControlesPersonalizados.ComboDatos();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.Grb_opcioCanImpre = new System.Windows.Forms.GroupBox();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.Grb_listReCanImpre.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            this.grupoDatos.SuspendLayout();
            this.Grb_opcioCanImpre.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnEliminar
            // 
            this.btnEliminar.BackColor = System.Drawing.Color.Red;
            this.btnEliminar.Enabled = false;
            this.btnEliminar.ForeColor = System.Drawing.Color.White;
            this.btnEliminar.Location = new System.Drawing.Point(143, 19);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(63, 39);
            this.btnEliminar.TabIndex = 12;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = false;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // Grb_listReCanImpre
            // 
            this.Grb_listReCanImpre.BackColor = System.Drawing.Color.Transparent;
            this.Grb_listReCanImpre.Controls.Add(this.btnBuscarCanImpre);
            this.Grb_listReCanImpre.Controls.Add(this.txtBusqueda);
            this.Grb_listReCanImpre.Controls.Add(this.dgvDatos);
            this.Grb_listReCanImpre.Location = new System.Drawing.Point(366, 78);
            this.Grb_listReCanImpre.Name = "Grb_listReCanImpre";
            this.Grb_listReCanImpre.Size = new System.Drawing.Size(336, 230);
            this.Grb_listReCanImpre.TabIndex = 11;
            this.Grb_listReCanImpre.TabStop = false;
            this.Grb_listReCanImpre.Text = "Lista de Registros";
            // 
            // btnBuscarCanImpre
            // 
            this.btnBuscarCanImpre.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnBuscarCanImpre.ForeColor = System.Drawing.Color.White;
            this.btnBuscarCanImpre.Location = new System.Drawing.Point(235, 25);
            this.btnBuscarCanImpre.Name = "btnBuscarCanImpre";
            this.btnBuscarCanImpre.Size = new System.Drawing.Size(88, 26);
            this.btnBuscarCanImpre.TabIndex = 2;
            this.btnBuscarCanImpre.Text = "Buscar";
            this.btnBuscarCanImpre.UseVisualStyleBackColor = false;
            this.btnBuscarCanImpre.Click += new System.EventHandler(this.btnBuscarCanImpre_Click);
            // 
            // txtBusqueda
            // 
            this.txtBusqueda.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBusqueda.Location = new System.Drawing.Point(13, 28);
            this.txtBusqueda.MaxLength = 20;
            this.txtBusqueda.Name = "txtBusqueda";
            this.txtBusqueda.Size = new System.Drawing.Size(216, 20);
            this.txtBusqueda.TabIndex = 1;
            this.txtBusqueda.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBusqueda_KeyPress);
            // 
            // dgvDatos
            // 
            this.dgvDatos.AllowUserToAddRows = false;
            this.dgvDatos.AllowUserToDeleteRows = false;
            this.dgvDatos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDatos.Location = new System.Drawing.Point(13, 61);
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.ReadOnly = true;
            this.dgvDatos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDatos.Size = new System.Drawing.Size(310, 155);
            this.dgvDatos.TabIndex = 0;
            this.dgvDatos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDatos_CellDoubleClick);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.BackColor = System.Drawing.Color.Lime;
            this.btnLimpiar.ForeColor = System.Drawing.Color.White;
            this.btnLimpiar.Location = new System.Drawing.Point(212, 19);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(63, 39);
            this.btnLimpiar.TabIndex = 13;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // cmbEstado
            // 
            this.cmbEstado.Enabled = false;
            this.cmbEstado.FormattingEnabled = true;
            this.cmbEstado.Items.AddRange(new object[] {
            "ACTIVO",
            "ELIMINADO"});
            this.cmbEstado.Location = new System.Drawing.Point(117, 107);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Size = new System.Drawing.Size(107, 21);
            this.cmbEstado.TabIndex = 10;
            // 
            // lblEstaCajero
            // 
            this.lblEstaCajero.AutoSize = true;
            this.lblEstaCajero.BackColor = System.Drawing.Color.Transparent;
            this.lblEstaCajero.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstaCajero.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblEstaCajero.Location = new System.Drawing.Point(22, 107);
            this.lblEstaCajero.Name = "lblEstaCajero";
            this.lblEstaCajero.Size = new System.Drawing.Size(48, 15);
            this.lblEstaCajero.TabIndex = 7;
            this.lblEstaCajero.Text = "Estado:";
            // 
            // lbldescrCajero
            // 
            this.lbldescrCajero.AutoSize = true;
            this.lbldescrCajero.BackColor = System.Drawing.Color.Transparent;
            this.lbldescrCajero.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldescrCajero.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbldescrCajero.Location = new System.Drawing.Point(22, 59);
            this.lbldescrCajero.Name = "lbldescrCajero";
            this.lbldescrCajero.Size = new System.Drawing.Size(75, 15);
            this.lbldescrCajero.TabIndex = 5;
            this.lbldescrCajero.Text = "Descripción:";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtDescripcion.Location = new System.Drawing.Point(117, 55);
            this.txtDescripcion.MaxLength = 20;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(195, 20);
            this.txtDescripcion.TabIndex = 5;
            // 
            // lblcodigoCajero
            // 
            this.lblcodigoCajero.AutoSize = true;
            this.lblcodigoCajero.BackColor = System.Drawing.Color.Transparent;
            this.lblcodigoCajero.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblcodigoCajero.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblcodigoCajero.Location = new System.Drawing.Point(22, 33);
            this.lblcodigoCajero.Name = "lblcodigoCajero";
            this.lblcodigoCajero.Size = new System.Drawing.Size(49, 15);
            this.lblcodigoCajero.TabIndex = 3;
            this.lblcodigoCajero.Text = "Código:";
            // 
            // grupoDatos
            // 
            this.grupoDatos.BackColor = System.Drawing.Color.Transparent;
            this.grupoDatos.Controls.Add(this.cmbImpresoras);
            this.grupoDatos.Controls.Add(this.label1);
            this.grupoDatos.Controls.Add(this.cmbEstado);
            this.grupoDatos.Controls.Add(this.lblEstaCajero);
            this.grupoDatos.Controls.Add(this.lbldescrCajero);
            this.grupoDatos.Controls.Add(this.txtDescripcion);
            this.grupoDatos.Controls.Add(this.lblcodigoCajero);
            this.grupoDatos.Controls.Add(this.txtCodigo);
            this.grupoDatos.Enabled = false;
            this.grupoDatos.Location = new System.Drawing.Point(12, 78);
            this.grupoDatos.Name = "grupoDatos";
            this.grupoDatos.Size = new System.Drawing.Size(342, 152);
            this.grupoDatos.TabIndex = 9;
            this.grupoDatos.TabStop = false;
            this.grupoDatos.Text = "Datos del Registro";
            // 
            // cmbImpresoras
            // 
            this.cmbImpresoras.FormattingEnabled = true;
            this.cmbImpresoras.Location = new System.Drawing.Point(117, 81);
            this.cmbImpresoras.Name = "cmbImpresoras";
            this.cmbImpresoras.Size = new System.Drawing.Size(195, 21);
            this.cmbImpresoras.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(22, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 15);
            this.label1.TabIndex = 23;
            this.label1.Text = "Impresoras:";
            // 
            // txtCodigo
            // 
            this.txtCodigo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCodigo.Location = new System.Drawing.Point(117, 31);
            this.txtCodigo.MaxLength = 20;
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(195, 20);
            this.txtCodigo.TabIndex = 4;
            // 
            // Grb_opcioCanImpre
            // 
            this.Grb_opcioCanImpre.BackColor = System.Drawing.Color.Transparent;
            this.Grb_opcioCanImpre.Controls.Add(this.btnLimpiar);
            this.Grb_opcioCanImpre.Controls.Add(this.btnEliminar);
            this.Grb_opcioCanImpre.Controls.Add(this.btnNuevo);
            this.Grb_opcioCanImpre.Location = new System.Drawing.Point(14, 236);
            this.Grb_opcioCanImpre.Name = "Grb_opcioCanImpre";
            this.Grb_opcioCanImpre.Size = new System.Drawing.Size(342, 72);
            this.Grb_opcioCanImpre.TabIndex = 10;
            this.Grb_opcioCanImpre.TabStop = false;
            this.Grb_opcioCanImpre.Text = "Opciones";
            // 
            // btnNuevo
            // 
            this.btnNuevo.BackColor = System.Drawing.Color.Blue;
            this.btnNuevo.ForeColor = System.Drawing.Color.White;
            this.btnNuevo.Location = new System.Drawing.Point(74, 19);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(63, 39);
            this.btnNuevo.TabIndex = 11;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = false;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            // 
            // frmFormatosFactura
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(718, 326);
            this.Controls.Add(this.Grb_listReCanImpre);
            this.Controls.Add(this.grupoDatos);
            this.Controls.Add(this.Grb_opcioCanImpre);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFormatosFactura";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Módulos de Formatos de Selección de Facturas";
            this.Load += new System.EventHandler(this.frmFormatosFactura_Load);
            this.Grb_listReCanImpre.ResumeLayout(false);
            this.Grb_listReCanImpre.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            this.grupoDatos.ResumeLayout(false);
            this.grupoDatos.PerformLayout();
            this.Grb_opcioCanImpre.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.GroupBox Grb_listReCanImpre;
        private System.Windows.Forms.Button btnBuscarCanImpre;
        private System.Windows.Forms.TextBox txtBusqueda;
        private System.Windows.Forms.DataGridView dgvDatos;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.ComboBox cmbEstado;
        private System.Windows.Forms.Label lblEstaCajero;
        private System.Windows.Forms.Label lbldescrCajero;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblcodigoCajero;
        private System.Windows.Forms.GroupBox grupoDatos;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.GroupBox Grb_opcioCanImpre;
        private System.Windows.Forms.Button btnNuevo;
        private ControlesPersonalizados.ComboDatos cmbImpresoras;
        private System.Windows.Forms.Label label1;
    }
}