﻿namespace Palatium.Productos
{
    partial class frmModificadores
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
            this.chkPrecioModificable = new System.Windows.Forms.CheckBox();
            this.chkPagaIva = new System.Windows.Forms.CheckBox();
            this.txtPrecioCompra = new System.Windows.Forms.TextBox();
            this.txtPrecioMinorista = new System.Windows.Forms.TextBox();
            this.lblPreCompra = new System.Windows.Forms.Label();
            this.lblPrecioMinorista = new System.Windows.Forms.Label();
            this.grupoGrid = new System.Windows.Forms.GroupBox();
            this.dgvDatos = new System.Windows.Forms.DataGridView();
            this.grupoBotones = new System.Windows.Forms.GroupBox();
            this.btnAnular = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.ttMensaje = new System.Windows.Forms.ToolTip(this.components);
            this.txtCodigoBarras = new System.Windows.Forms.TextBox();
            this.grupoOpciones = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnExaminar = new System.Windows.Forms.Button();
            this.lblEtiquetaImagen = new System.Windows.Forms.Label();
            this.imgLogo = new System.Windows.Forms.PictureBox();
            this.chkHabilitado = new System.Windows.Forms.CheckBox();
            this.chkPagaServicio = new System.Windows.Forms.CheckBox();
            this.chkExpira = new System.Windows.Forms.CheckBox();
            this.grupoPrecio = new System.Windows.Forms.GroupBox();
            this.label23 = new System.Windows.Forms.Label();
            this.lblSecuencia = new System.Windows.Forms.Label();
            this.txtSecuencia = new System.Windows.Forms.TextBox();
            this.grupoImpresion = new System.Windows.Forms.GroupBox();
            this.cmbDestinoImpresion = new ControlesPersonalizados.ComboDatos();
            this.grupoEncabezado = new System.Windows.Forms.GroupBox();
            this.dBAyudaModificadores = new ControlesPersonalizados.DB_Ayuda();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbPadre = new ControlesPersonalizados.ComboDatos();
            this.cmbEmpresa = new ControlesPersonalizados.ComboDatos();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblListaNombre = new System.Windows.Forms.Label();
            this.grupoDatos = new System.Windows.Forms.GroupBox();
            this.cmbClaseProducto = new ControlesPersonalizados.ComboDatos();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbTipoProducto = new ControlesPersonalizados.ComboDatos();
            this.label3 = new System.Windows.Forms.Label();
            this.lblDecripCategoria = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.lblCodiCategori = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.txtRuta = new System.Windows.Forms.TextBox();
            this.txtBase64 = new System.Windows.Forms.TextBox();
            this.grupoGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            this.grupoBotones.SuspendLayout();
            this.grupoOpciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).BeginInit();
            this.grupoPrecio.SuspendLayout();
            this.grupoImpresion.SuspendLayout();
            this.grupoEncabezado.SuspendLayout();
            this.grupoDatos.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkPrecioModificable
            // 
            this.chkPrecioModificable.AutoSize = true;
            this.chkPrecioModificable.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPrecioModificable.Location = new System.Drawing.Point(19, 72);
            this.chkPrecioModificable.Name = "chkPrecioModificable";
            this.chkPrecioModificable.Size = new System.Drawing.Size(128, 19);
            this.chkPrecioModificable.TabIndex = 16;
            this.chkPrecioModificable.Text = "Precio modificable";
            this.chkPrecioModificable.UseVisualStyleBackColor = true;
            // 
            // chkPagaIva
            // 
            this.chkPagaIva.AutoSize = true;
            this.chkPagaIva.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPagaIva.Location = new System.Drawing.Point(19, 22);
            this.chkPagaIva.Name = "chkPagaIva";
            this.chkPagaIva.Size = new System.Drawing.Size(73, 19);
            this.chkPagaIva.TabIndex = 15;
            this.chkPagaIva.Text = "Paga Iva";
            this.chkPagaIva.UseVisualStyleBackColor = true;
            // 
            // txtPrecioCompra
            // 
            this.txtPrecioCompra.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtPrecioCompra.Location = new System.Drawing.Point(160, 27);
            this.txtPrecioCompra.MaxLength = 20;
            this.txtPrecioCompra.Name = "txtPrecioCompra";
            this.txtPrecioCompra.Size = new System.Drawing.Size(111, 20);
            this.txtPrecioCompra.TabIndex = 11;
            this.txtPrecioCompra.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPrecioCompra_KeyPress);
            // 
            // txtPrecioMinorista
            // 
            this.txtPrecioMinorista.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtPrecioMinorista.Location = new System.Drawing.Point(160, 49);
            this.txtPrecioMinorista.MaxLength = 20;
            this.txtPrecioMinorista.Name = "txtPrecioMinorista";
            this.txtPrecioMinorista.Size = new System.Drawing.Size(111, 20);
            this.txtPrecioMinorista.TabIndex = 12;
            this.txtPrecioMinorista.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPrecioMinorista_KeyPress);
            // 
            // lblPreCompra
            // 
            this.lblPreCompra.AutoSize = true;
            this.lblPreCompra.BackColor = System.Drawing.Color.Transparent;
            this.lblPreCompra.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreCompra.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblPreCompra.Location = new System.Drawing.Point(13, 29);
            this.lblPreCompra.Name = "lblPreCompra";
            this.lblPreCompra.Size = new System.Drawing.Size(100, 15);
            this.lblPreCompra.TabIndex = 34;
            this.lblPreCompra.Text = "Precio Compra: *";
            // 
            // lblPrecioMinorista
            // 
            this.lblPrecioMinorista.AutoSize = true;
            this.lblPrecioMinorista.BackColor = System.Drawing.Color.Transparent;
            this.lblPrecioMinorista.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrecioMinorista.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblPrecioMinorista.Location = new System.Drawing.Point(13, 52);
            this.lblPrecioMinorista.Name = "lblPrecioMinorista";
            this.lblPrecioMinorista.Size = new System.Drawing.Size(141, 15);
            this.lblPrecioMinorista.TabIndex = 36;
            this.lblPrecioMinorista.Text = "Precio Minorista (PVP): *";
            // 
            // grupoGrid
            // 
            this.grupoGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.grupoGrid.Controls.Add(this.dgvDatos);
            this.grupoGrid.Location = new System.Drawing.Point(587, 95);
            this.grupoGrid.Name = "grupoGrid";
            this.grupoGrid.Size = new System.Drawing.Size(461, 256);
            this.grupoGrid.TabIndex = 46;
            this.grupoGrid.TabStop = false;
            this.grupoGrid.Text = "Lista de Registros para Búsqueda por Nombres";
            // 
            // dgvDatos
            // 
            this.dgvDatos.AllowUserToAddRows = false;
            this.dgvDatos.AllowUserToDeleteRows = false;
            this.dgvDatos.AllowUserToResizeColumns = false;
            this.dgvDatos.AllowUserToResizeRows = false;
            this.dgvDatos.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvDatos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDatos.Location = new System.Drawing.Point(16, 25);
            this.dgvDatos.MultiSelect = false;
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.ReadOnly = true;
            this.dgvDatos.RowHeadersWidth = 25;
            this.dgvDatos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDatos.Size = new System.Drawing.Size(428, 217);
            this.dgvDatos.TabIndex = 28;
            this.dgvDatos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDatos_CellDoubleClick);
            // 
            // grupoBotones
            // 
            this.grupoBotones.Controls.Add(this.btnAnular);
            this.grupoBotones.Controls.Add(this.btnLimpiar);
            this.grupoBotones.Controls.Add(this.btnNuevo);
            this.grupoBotones.Location = new System.Drawing.Point(331, 279);
            this.grupoBotones.Name = "grupoBotones";
            this.grupoBotones.Size = new System.Drawing.Size(250, 72);
            this.grupoBotones.TabIndex = 45;
            this.grupoBotones.TabStop = false;
            this.grupoBotones.Text = "Opciones";
            // 
            // btnAnular
            // 
            this.btnAnular.BackColor = System.Drawing.Color.Red;
            this.btnAnular.FlatAppearance.CheckedBackColor = System.Drawing.Color.Magenta;
            this.btnAnular.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnAnular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnular.ForeColor = System.Drawing.Color.White;
            this.btnAnular.Location = new System.Drawing.Point(163, 19);
            this.btnAnular.Name = "btnAnular";
            this.btnAnular.Size = new System.Drawing.Size(70, 39);
            this.btnAnular.TabIndex = 25;
            this.btnAnular.Text = "Eliminar";
            this.btnAnular.UseVisualStyleBackColor = false;
            this.btnAnular.Click += new System.EventHandler(this.btnAnular_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnLimpiar.FlatAppearance.CheckedBackColor = System.Drawing.Color.Magenta;
            this.btnLimpiar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnLimpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiar.ForeColor = System.Drawing.Color.White;
            this.btnLimpiar.Location = new System.Drawing.Point(87, 19);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnLimpiar.Size = new System.Drawing.Size(70, 39);
            this.btnLimpiar.TabIndex = 24;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // btnNuevo
            // 
            this.btnNuevo.BackColor = System.Drawing.Color.Blue;
            this.btnNuevo.FlatAppearance.CheckedBackColor = System.Drawing.Color.Magenta;
            this.btnNuevo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevo.ForeColor = System.Drawing.Color.White;
            this.btnNuevo.Location = new System.Drawing.Point(11, 19);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(70, 39);
            this.btnNuevo.TabIndex = 23;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = false;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            // 
            // txtCodigoBarras
            // 
            this.txtCodigoBarras.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodigoBarras.Location = new System.Drawing.Point(160, 95);
            this.txtCodigoBarras.MaxLength = 13;
            this.txtCodigoBarras.Name = "txtCodigoBarras";
            this.txtCodigoBarras.Size = new System.Drawing.Size(111, 21);
            this.txtCodigoBarras.TabIndex = 88;
            this.ttMensaje.SetToolTip(this.txtCodigoBarras, "Digite el código de barras para formato EAN-13");
            this.txtCodigoBarras.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCodigoBarras_KeyPress);
            // 
            // grupoOpciones
            // 
            this.grupoOpciones.Controls.Add(this.btnClear);
            this.grupoOpciones.Controls.Add(this.btnExaminar);
            this.grupoOpciones.Controls.Add(this.lblEtiquetaImagen);
            this.grupoOpciones.Controls.Add(this.imgLogo);
            this.grupoOpciones.Controls.Add(this.chkHabilitado);
            this.grupoOpciones.Controls.Add(this.chkPagaServicio);
            this.grupoOpciones.Controls.Add(this.chkPagaIva);
            this.grupoOpciones.Controls.Add(this.chkPrecioModificable);
            this.grupoOpciones.Controls.Add(this.chkExpira);
            this.grupoOpciones.Enabled = false;
            this.grupoOpciones.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grupoOpciones.Location = new System.Drawing.Point(331, 88);
            this.grupoOpciones.Name = "grupoOpciones";
            this.grupoOpciones.Size = new System.Drawing.Size(243, 116);
            this.grupoOpciones.TabIndex = 41;
            this.grupoOpciones.TabStop = false;
            this.grupoOpciones.Text = "Opciones";
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Red;
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Fuchsia;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(207, 79);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(29, 21);
            this.btnClear.TabIndex = 91;
            this.btnClear.Text = "X";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnExaminar
            // 
            this.btnExaminar.BackColor = System.Drawing.Color.Yellow;
            this.btnExaminar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Fuchsia;
            this.btnExaminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExaminar.Location = new System.Drawing.Point(176, 79);
            this.btnExaminar.Name = "btnExaminar";
            this.btnExaminar.Size = new System.Drawing.Size(29, 21);
            this.btnExaminar.TabIndex = 90;
            this.btnExaminar.Text = "...";
            this.btnExaminar.UseVisualStyleBackColor = false;
            this.btnExaminar.Click += new System.EventHandler(this.btnExaminar_Click);
            // 
            // lblEtiquetaImagen
            // 
            this.lblEtiquetaImagen.BackColor = System.Drawing.Color.Transparent;
            this.lblEtiquetaImagen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEtiquetaImagen.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblEtiquetaImagen.Location = new System.Drawing.Point(175, 13);
            this.lblEtiquetaImagen.Name = "lblEtiquetaImagen";
            this.lblEtiquetaImagen.Size = new System.Drawing.Size(60, 18);
            this.lblEtiquetaImagen.TabIndex = 89;
            this.lblEtiquetaImagen.Text = "Imagen:";
            this.lblEtiquetaImagen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imgLogo
            // 
            this.imgLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgLogo.Location = new System.Drawing.Point(176, 34);
            this.imgLogo.Name = "imgLogo";
            this.imgLogo.Size = new System.Drawing.Size(60, 44);
            this.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgLogo.TabIndex = 88;
            this.imgLogo.TabStop = false;
            // 
            // chkHabilitado
            // 
            this.chkHabilitado.AutoSize = true;
            this.chkHabilitado.Checked = true;
            this.chkHabilitado.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHabilitado.Enabled = false;
            this.chkHabilitado.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkHabilitado.ForeColor = System.Drawing.Color.Red;
            this.chkHabilitado.Location = new System.Drawing.Point(19, 88);
            this.chkHabilitado.Name = "chkHabilitado";
            this.chkHabilitado.Size = new System.Drawing.Size(138, 17);
            this.chkHabilitado.TabIndex = 87;
            this.chkHabilitado.Text = "Producto Habilitado";
            this.chkHabilitado.UseVisualStyleBackColor = true;
            // 
            // chkPagaServicio
            // 
            this.chkPagaServicio.AutoSize = true;
            this.chkPagaServicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPagaServicio.ForeColor = System.Drawing.Color.Red;
            this.chkPagaServicio.Location = new System.Drawing.Point(19, 40);
            this.chkPagaServicio.Name = "chkPagaServicio";
            this.chkPagaServicio.Size = new System.Drawing.Size(101, 19);
            this.chkPagaServicio.TabIndex = 86;
            this.chkPagaServicio.Text = "Paga Servicio";
            this.chkPagaServicio.UseVisualStyleBackColor = true;
            // 
            // chkExpira
            // 
            this.chkExpira.AutoSize = true;
            this.chkExpira.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkExpira.Location = new System.Drawing.Point(19, 57);
            this.chkExpira.Name = "chkExpira";
            this.chkExpira.Size = new System.Drawing.Size(61, 19);
            this.chkExpira.TabIndex = 17;
            this.chkExpira.Text = "Expira";
            this.chkExpira.UseVisualStyleBackColor = true;
            // 
            // grupoPrecio
            // 
            this.grupoPrecio.Controls.Add(this.label23);
            this.grupoPrecio.Controls.Add(this.txtCodigoBarras);
            this.grupoPrecio.Controls.Add(this.lblSecuencia);
            this.grupoPrecio.Controls.Add(this.txtPrecioCompra);
            this.grupoPrecio.Controls.Add(this.txtSecuencia);
            this.grupoPrecio.Controls.Add(this.txtPrecioMinorista);
            this.grupoPrecio.Controls.Add(this.lblPreCompra);
            this.grupoPrecio.Controls.Add(this.lblPrecioMinorista);
            this.grupoPrecio.Enabled = false;
            this.grupoPrecio.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grupoPrecio.Location = new System.Drawing.Point(12, 221);
            this.grupoPrecio.Name = "grupoPrecio";
            this.grupoPrecio.Size = new System.Drawing.Size(313, 130);
            this.grupoPrecio.TabIndex = 40;
            this.grupoPrecio.TabStop = false;
            this.grupoPrecio.Text = "Control de Precio";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(13, 98);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(88, 15);
            this.label23.TabIndex = 89;
            this.label23.Text = "Código Barras:";
            // 
            // lblSecuencia
            // 
            this.lblSecuencia.AutoSize = true;
            this.lblSecuencia.BackColor = System.Drawing.Color.Transparent;
            this.lblSecuencia.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSecuencia.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblSecuencia.Location = new System.Drawing.Point(13, 77);
            this.lblSecuencia.Name = "lblSecuencia";
            this.lblSecuencia.Size = new System.Drawing.Size(76, 15);
            this.lblSecuencia.TabIndex = 50;
            this.lblSecuencia.Text = "Secuencia: *";
            // 
            // txtSecuencia
            // 
            this.txtSecuencia.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtSecuencia.Location = new System.Drawing.Point(160, 72);
            this.txtSecuencia.MaxLength = 20;
            this.txtSecuencia.Name = "txtSecuencia";
            this.txtSecuencia.Size = new System.Drawing.Size(111, 20);
            this.txtSecuencia.TabIndex = 20;
            this.txtSecuencia.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSecuencia_KeyPress);
            // 
            // grupoImpresion
            // 
            this.grupoImpresion.Controls.Add(this.cmbDestinoImpresion);
            this.grupoImpresion.Enabled = false;
            this.grupoImpresion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grupoImpresion.Location = new System.Drawing.Point(331, 210);
            this.grupoImpresion.Name = "grupoImpresion";
            this.grupoImpresion.Size = new System.Drawing.Size(243, 63);
            this.grupoImpresion.TabIndex = 43;
            this.grupoImpresion.TabStop = false;
            this.grupoImpresion.Text = "Control de Destino  de Impresión";
            // 
            // cmbDestinoImpresion
            // 
            this.cmbDestinoImpresion.FormattingEnabled = true;
            this.cmbDestinoImpresion.Location = new System.Drawing.Point(6, 21);
            this.cmbDestinoImpresion.Name = "cmbDestinoImpresion";
            this.cmbDestinoImpresion.Size = new System.Drawing.Size(203, 24);
            this.cmbDestinoImpresion.TabIndex = 21;
            // 
            // grupoEncabezado
            // 
            this.grupoEncabezado.Controls.Add(this.dBAyudaModificadores);
            this.grupoEncabezado.Controls.Add(this.label2);
            this.grupoEncabezado.Controls.Add(this.cmbPadre);
            this.grupoEncabezado.Controls.Add(this.cmbEmpresa);
            this.grupoEncabezado.Controls.Add(this.lblCodigo);
            this.grupoEncabezado.Controls.Add(this.btnOK);
            this.grupoEncabezado.Controls.Add(this.lblListaNombre);
            this.grupoEncabezado.Location = new System.Drawing.Point(12, 13);
            this.grupoEncabezado.Name = "grupoEncabezado";
            this.grupoEncabezado.Size = new System.Drawing.Size(1036, 70);
            this.grupoEncabezado.TabIndex = 38;
            this.grupoEncabezado.TabStop = false;
            // 
            // dBAyudaModificadores
            // 
            this.dBAyudaModificadores.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dBAyudaModificadores.iId = 0;
            this.dBAyudaModificadores.Location = new System.Drawing.Point(493, 36);
            this.dBAyudaModificadores.Name = "dBAyudaModificadores";
            this.dBAyudaModificadores.sDatosConsulta = null;
            this.dBAyudaModificadores.sDescripcion = null;
            this.dBAyudaModificadores.Size = new System.Drawing.Size(461, 19);
            this.dBAyudaModificadores.TabIndex = 55;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(257, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 15);
            this.label2.TabIndex = 34;
            this.label2.Text = "Código Padre:";
            // 
            // cmbPadre
            // 
            this.cmbPadre.FormattingEnabled = true;
            this.cmbPadre.Location = new System.Drawing.Point(258, 36);
            this.cmbPadre.Name = "cmbPadre";
            this.cmbPadre.Size = new System.Drawing.Size(176, 21);
            this.cmbPadre.TabIndex = 2;
            // 
            // cmbEmpresa
            // 
            this.cmbEmpresa.FormattingEnabled = true;
            this.cmbEmpresa.Location = new System.Drawing.Point(16, 36);
            this.cmbEmpresa.Name = "cmbEmpresa";
            this.cmbEmpresa.Size = new System.Drawing.Size(214, 21);
            this.cmbEmpresa.TabIndex = 1;
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.BackColor = System.Drawing.Color.Transparent;
            this.lblCodigo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodigo.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblCodigo.Location = new System.Drawing.Point(13, 16);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(60, 15);
            this.lblCodigo.TabIndex = 32;
            this.lblCodigo.Text = "Empresa:";
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Blue;
            this.btnOK.FlatAppearance.CheckedBackColor = System.Drawing.Color.Magenta;
            this.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(960, 30);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(59, 31);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblListaNombre
            // 
            this.lblListaNombre.AutoSize = true;
            this.lblListaNombre.BackColor = System.Drawing.Color.Transparent;
            this.lblListaNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblListaNombre.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblListaNombre.Location = new System.Drawing.Point(492, 16);
            this.lblListaNombre.Name = "lblListaNombre";
            this.lblListaNombre.Size = new System.Drawing.Size(178, 15);
            this.lblListaNombre.TabIndex = 28;
            this.lblListaNombre.Text = "Listado de Ítems Modificadores";
            this.lblListaNombre.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grupoDatos
            // 
            this.grupoDatos.Controls.Add(this.cmbClaseProducto);
            this.grupoDatos.Controls.Add(this.label4);
            this.grupoDatos.Controls.Add(this.cmbTipoProducto);
            this.grupoDatos.Controls.Add(this.label3);
            this.grupoDatos.Controls.Add(this.lblDecripCategoria);
            this.grupoDatos.Controls.Add(this.txtDescripcion);
            this.grupoDatos.Controls.Add(this.lblCodiCategori);
            this.grupoDatos.Controls.Add(this.txtCodigo);
            this.grupoDatos.Enabled = false;
            this.grupoDatos.Location = new System.Drawing.Point(12, 89);
            this.grupoDatos.Name = "grupoDatos";
            this.grupoDatos.Size = new System.Drawing.Size(313, 129);
            this.grupoDatos.TabIndex = 39;
            this.grupoDatos.TabStop = false;
            this.grupoDatos.Text = "Lista de Registros";
            // 
            // cmbClaseProducto
            // 
            this.cmbClaseProducto.FormattingEnabled = true;
            this.cmbClaseProducto.Location = new System.Drawing.Point(154, 94);
            this.cmbClaseProducto.Name = "cmbClaseProducto";
            this.cmbClaseProducto.Size = new System.Drawing.Size(142, 21);
            this.cmbClaseProducto.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(6, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 15);
            this.label4.TabIndex = 68;
            this.label4.Text = "Clase de Producto";
            // 
            // cmbTipoProducto
            // 
            this.cmbTipoProducto.FormattingEnabled = true;
            this.cmbTipoProducto.Location = new System.Drawing.Point(154, 72);
            this.cmbTipoProducto.Name = "cmbTipoProducto";
            this.cmbTipoProducto.Size = new System.Drawing.Size(142, 21);
            this.cmbTipoProducto.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(5, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 15);
            this.label3.TabIndex = 66;
            this.label3.Text = "Tipo de Producto";
            // 
            // lblDecripCategoria
            // 
            this.lblDecripCategoria.AutoSize = true;
            this.lblDecripCategoria.BackColor = System.Drawing.Color.Transparent;
            this.lblDecripCategoria.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDecripCategoria.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblDecripCategoria.Location = new System.Drawing.Point(7, 52);
            this.lblDecripCategoria.Name = "lblDecripCategoria";
            this.lblDecripCategoria.Size = new System.Drawing.Size(134, 15);
            this.lblDecripCategoria.TabIndex = 32;
            this.lblDecripCategoria.Text = "Nombre del producto: *";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtDescripcion.Location = new System.Drawing.Point(154, 51);
            this.txtDescripcion.MaxLength = 30;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(142, 20);
            this.txtDescripcion.TabIndex = 8;
            // 
            // lblCodiCategori
            // 
            this.lblCodiCategori.AutoSize = true;
            this.lblCodiCategori.BackColor = System.Drawing.Color.Transparent;
            this.lblCodiCategori.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodiCategori.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblCodiCategori.Location = new System.Drawing.Point(7, 31);
            this.lblCodiCategori.Name = "lblCodiCategori";
            this.lblCodiCategori.Size = new System.Drawing.Size(128, 15);
            this.lblCodiCategori.TabIndex = 30;
            this.lblCodiCategori.Text = "Código del producto: *";
            // 
            // txtCodigo
            // 
            this.txtCodigo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCodigo.Location = new System.Drawing.Point(154, 30);
            this.txtCodigo.MaxLength = 20;
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(142, 20);
            this.txtCodigo.TabIndex = 7;
            // 
            // txtRuta
            // 
            this.txtRuta.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtRuta.Location = new System.Drawing.Point(427, 379);
            this.txtRuta.MaxLength = 20;
            this.txtRuta.Name = "txtRuta";
            this.txtRuta.ReadOnly = true;
            this.txtRuta.Size = new System.Drawing.Size(202, 20);
            this.txtRuta.TabIndex = 69;
            // 
            // txtBase64
            // 
            this.txtBase64.Enabled = false;
            this.txtBase64.Location = new System.Drawing.Point(52, 392);
            this.txtBase64.MaxLength = 20;
            this.txtBase64.Multiline = true;
            this.txtBase64.Name = "txtBase64";
            this.txtBase64.Size = new System.Drawing.Size(875, 74);
            this.txtBase64.TabIndex = 68;
            // 
            // frmModificadores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(1055, 360);
            this.Controls.Add(this.txtRuta);
            this.Controls.Add(this.txtBase64);
            this.Controls.Add(this.grupoGrid);
            this.Controls.Add(this.grupoBotones);
            this.Controls.Add(this.grupoOpciones);
            this.Controls.Add(this.grupoPrecio);
            this.Controls.Add(this.grupoImpresion);
            this.Controls.Add(this.grupoEncabezado);
            this.Controls.Add(this.grupoDatos);
            this.MaximizeBox = false;
            this.Name = "frmModificadores";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Módulo de Configuración de Modificadores";
            this.Load += new System.EventHandler(this.frmModificadores_Load);
            this.grupoGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            this.grupoBotones.ResumeLayout(false);
            this.grupoOpciones.ResumeLayout(false);
            this.grupoOpciones.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).EndInit();
            this.grupoPrecio.ResumeLayout(false);
            this.grupoPrecio.PerformLayout();
            this.grupoImpresion.ResumeLayout(false);
            this.grupoEncabezado.ResumeLayout(false);
            this.grupoEncabezado.PerformLayout();
            this.grupoDatos.ResumeLayout(false);
            this.grupoDatos.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkPrecioModificable;
        private System.Windows.Forms.CheckBox chkPagaIva;
        private System.Windows.Forms.TextBox txtPrecioCompra;
        private System.Windows.Forms.TextBox txtPrecioMinorista;
        private System.Windows.Forms.Label lblPreCompra;
        private System.Windows.Forms.Label lblPrecioMinorista;
        private System.Windows.Forms.GroupBox grupoGrid;
        private System.Windows.Forms.ToolTip ttMensaje;
        private System.Windows.Forms.DataGridView dgvDatos;
        private System.Windows.Forms.GroupBox grupoBotones;
        private System.Windows.Forms.Button btnAnular;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.GroupBox grupoOpciones;
        private System.Windows.Forms.CheckBox chkExpira;
        private System.Windows.Forms.GroupBox grupoPrecio;
        private System.Windows.Forms.GroupBox grupoImpresion;
        private ControlesPersonalizados.ComboDatos cmbDestinoImpresion;
        private System.Windows.Forms.GroupBox grupoEncabezado;
        private System.Windows.Forms.Label label2;
        private ControlesPersonalizados.ComboDatos cmbPadre;
        private ControlesPersonalizados.ComboDatos cmbEmpresa;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.Button btnOK;
        public System.Windows.Forms.Label lblListaNombre;
        private System.Windows.Forms.GroupBox grupoDatos;
        private ControlesPersonalizados.ComboDatos cmbClaseProducto;
        private System.Windows.Forms.Label label4;
        private ControlesPersonalizados.ComboDatos cmbTipoProducto;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblDecripCategoria;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblCodiCategori;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.TextBox txtSecuencia;
        private System.Windows.Forms.Label lblSecuencia;
        private ControlesPersonalizados.DB_Ayuda dBAyudaModificadores;
        private System.Windows.Forms.CheckBox chkPagaServicio;
        private System.Windows.Forms.CheckBox chkHabilitado;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox txtCodigoBarras;
        private System.Windows.Forms.Label lblEtiquetaImagen;
        private System.Windows.Forms.PictureBox imgLogo;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnExaminar;
        private System.Windows.Forms.TextBox txtRuta;
        private System.Windows.Forms.TextBox txtBase64;
    }
}