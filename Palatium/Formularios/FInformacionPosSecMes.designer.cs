namespace Palatium.Formularios
{
    partial class FInformacionPosSecMes
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
            this.Grb_Registros = new System.Windows.Forms.GroupBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.dgvDatos = new System.Windows.Forms.DataGridView();
            this.Grb_Opciones = new System.Windows.Forms.GroupBox();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnAnular = new System.Windows.Forms.Button();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.grupoDatos = new System.Windows.Forms.GroupBox();
            this.chkHabilitado = new System.Windows.Forms.CheckBox();
            this.btnExaminar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbPaleta = new System.Windows.Forms.ComboBox();
            this.txtRuta = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDescrSecMes = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.lblcodigoSecMesa = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.cmbLocalidades = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.id_pos_seccion_mesa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id_localidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.is_active = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fondo_pantalla = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.color = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.estado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Grb_Registros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            this.Grb_Opciones.SuspendLayout();
            this.grupoDatos.SuspendLayout();
            this.SuspendLayout();
            // 
            // Grb_Registros
            // 
            this.Grb_Registros.BackColor = System.Drawing.Color.Transparent;
            this.Grb_Registros.Controls.Add(this.btnBuscar);
            this.Grb_Registros.Controls.Add(this.txtBuscar);
            this.Grb_Registros.Controls.Add(this.dgvDatos);
            this.Grb_Registros.Location = new System.Drawing.Point(430, 72);
            this.Grb_Registros.Name = "Grb_Registros";
            this.Grb_Registros.Size = new System.Drawing.Size(430, 281);
            this.Grb_Registros.TabIndex = 5;
            this.Grb_Registros.TabStop = false;
            this.Grb_Registros.Text = "Lista de Registros";
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnBuscar.ForeColor = System.Drawing.Color.Transparent;
            this.btnBuscar.Location = new System.Drawing.Point(235, 24);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(88, 26);
            this.btnBuscar.TabIndex = 2;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.Btn_BuscarPosSecMes_Click);
            // 
            // txtBuscar
            // 
            this.txtBuscar.Location = new System.Drawing.Point(13, 28);
            this.txtBuscar.MaxLength = 20;
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(216, 20);
            this.txtBuscar.TabIndex = 1;
            // 
            // dgvDatos
            // 
            this.dgvDatos.AllowUserToAddRows = false;
            this.dgvDatos.AllowUserToDeleteRows = false;
            this.dgvDatos.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvDatos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDatos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id_pos_seccion_mesa,
            this.id_localidad,
            this.is_active,
            this.fondo_pantalla,
            this.color,
            this.codigo,
            this.descripcion,
            this.estado});
            this.dgvDatos.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dgvDatos.Location = new System.Drawing.Point(13, 54);
            this.dgvDatos.MultiSelect = false;
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.ReadOnly = true;
            this.dgvDatos.RowHeadersVisible = false;
            this.dgvDatos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDatos.Size = new System.Drawing.Size(406, 211);
            this.dgvDatos.TabIndex = 0;
            this.dgvDatos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDatos_CellDoubleClick);
            // 
            // Grb_Opciones
            // 
            this.Grb_Opciones.BackColor = System.Drawing.Color.Transparent;
            this.Grb_Opciones.Controls.Add(this.btnCerrar);
            this.Grb_Opciones.Controls.Add(this.btnLimpiar);
            this.Grb_Opciones.Controls.Add(this.btnAnular);
            this.Grb_Opciones.Controls.Add(this.btnNuevo);
            this.Grb_Opciones.Location = new System.Drawing.Point(12, 280);
            this.Grb_Opciones.Name = "Grb_Opciones";
            this.Grb_Opciones.Size = new System.Drawing.Size(412, 73);
            this.Grb_Opciones.TabIndex = 4;
            this.Grb_Opciones.TabStop = false;
            this.Grb_Opciones.Text = "Opciones";
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnCerrar.ForeColor = System.Drawing.Color.Transparent;
            this.btnCerrar.Location = new System.Drawing.Point(288, 18);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(72, 39);
            this.btnCerrar.TabIndex = 10;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.Btn_CerrarPosSecMes_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnLimpiar.ForeColor = System.Drawing.Color.Transparent;
            this.btnLimpiar.Location = new System.Drawing.Point(210, 18);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(72, 39);
            this.btnLimpiar.TabIndex = 9;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            this.btnLimpiar.Click += new System.EventHandler(this.Btn_LimpiarPosSecMes_Click);
            // 
            // btnAnular
            // 
            this.btnAnular.BackColor = System.Drawing.Color.Red;
            this.btnAnular.Enabled = false;
            this.btnAnular.ForeColor = System.Drawing.Color.Transparent;
            this.btnAnular.Location = new System.Drawing.Point(132, 18);
            this.btnAnular.Name = "btnAnular";
            this.btnAnular.Size = new System.Drawing.Size(72, 39);
            this.btnAnular.TabIndex = 8;
            this.btnAnular.Text = "Eliminar";
            this.btnAnular.UseVisualStyleBackColor = false;
            this.btnAnular.Click += new System.EventHandler(this.Btn_AnularPosSecMes_Click);
            // 
            // btnNuevo
            // 
            this.btnNuevo.BackColor = System.Drawing.Color.Blue;
            this.btnNuevo.ForeColor = System.Drawing.Color.Transparent;
            this.btnNuevo.Location = new System.Drawing.Point(54, 18);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(72, 39);
            this.btnNuevo.TabIndex = 7;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = false;
            this.btnNuevo.Click += new System.EventHandler(this.BtnNuevoPosSecMes_Click);
            // 
            // grupoDatos
            // 
            this.grupoDatos.BackColor = System.Drawing.Color.Transparent;
            this.grupoDatos.Controls.Add(this.chkHabilitado);
            this.grupoDatos.Controls.Add(this.btnExaminar);
            this.grupoDatos.Controls.Add(this.label2);
            this.grupoDatos.Controls.Add(this.cmbPaleta);
            this.grupoDatos.Controls.Add(this.txtRuta);
            this.grupoDatos.Controls.Add(this.label1);
            this.grupoDatos.Controls.Add(this.lblDescrSecMes);
            this.grupoDatos.Controls.Add(this.txtDescripcion);
            this.grupoDatos.Controls.Add(this.lblcodigoSecMesa);
            this.grupoDatos.Controls.Add(this.txtCodigo);
            this.grupoDatos.Enabled = false;
            this.grupoDatos.Location = new System.Drawing.Point(12, 112);
            this.grupoDatos.Name = "grupoDatos";
            this.grupoDatos.Size = new System.Drawing.Size(412, 161);
            this.grupoDatos.TabIndex = 3;
            this.grupoDatos.TabStop = false;
            this.grupoDatos.Text = "Datos del Registro";
            // 
            // chkHabilitado
            // 
            this.chkHabilitado.AutoSize = true;
            this.chkHabilitado.Checked = true;
            this.chkHabilitado.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHabilitado.Enabled = false;
            this.chkHabilitado.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkHabilitado.ForeColor = System.Drawing.Color.Red;
            this.chkHabilitado.Location = new System.Drawing.Point(109, 133);
            this.chkHabilitado.Name = "chkHabilitado";
            this.chkHabilitado.Size = new System.Drawing.Size(83, 17);
            this.chkHabilitado.TabIndex = 61;
            this.chkHabilitado.Text = "Habilitado";
            this.chkHabilitado.UseVisualStyleBackColor = true;
            // 
            // btnExaminar
            // 
            this.btnExaminar.BackColor = System.Drawing.Color.Yellow;
            this.btnExaminar.Location = new System.Drawing.Point(370, 102);
            this.btnExaminar.Name = "btnExaminar";
            this.btnExaminar.Size = new System.Drawing.Size(29, 21);
            this.btnExaminar.TabIndex = 7;
            this.btnExaminar.Text = "...";
            this.btnExaminar.UseVisualStyleBackColor = false;
            this.btnExaminar.Click += new System.EventHandler(this.btnExaminar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(25, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "Ruta imagen:";
            // 
            // cmbPaleta
            // 
            this.cmbPaleta.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbPaleta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaleta.FormattingEnabled = true;
            this.cmbPaleta.Location = new System.Drawing.Point(109, 75);
            this.cmbPaleta.Name = "cmbPaleta";
            this.cmbPaleta.Size = new System.Drawing.Size(152, 21);
            this.cmbPaleta.TabIndex = 5;
            this.cmbPaleta.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbPaleta_DrawItem);
            // 
            // txtRuta
            // 
            this.txtRuta.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtRuta.Enabled = false;
            this.txtRuta.Location = new System.Drawing.Point(109, 102);
            this.txtRuta.MaxLength = 20;
            this.txtRuta.Name = "txtRuta";
            this.txtRuta.Size = new System.Drawing.Size(255, 20);
            this.txtRuta.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(25, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "Color";
            // 
            // lblDescrSecMes
            // 
            this.lblDescrSecMes.AutoSize = true;
            this.lblDescrSecMes.BackColor = System.Drawing.Color.Transparent;
            this.lblDescrSecMes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescrSecMes.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblDescrSecMes.Location = new System.Drawing.Point(25, 52);
            this.lblDescrSecMes.Name = "lblDescrSecMes";
            this.lblDescrSecMes.Size = new System.Drawing.Size(75, 15);
            this.lblDescrSecMes.TabIndex = 5;
            this.lblDescrSecMes.Text = "Descripción:";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtDescripcion.Location = new System.Drawing.Point(109, 50);
            this.txtDescripcion.MaxLength = 20;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(290, 20);
            this.txtDescripcion.TabIndex = 4;
            // 
            // lblcodigoSecMesa
            // 
            this.lblcodigoSecMesa.AutoSize = true;
            this.lblcodigoSecMesa.BackColor = System.Drawing.Color.Transparent;
            this.lblcodigoSecMesa.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblcodigoSecMesa.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblcodigoSecMesa.Location = new System.Drawing.Point(25, 26);
            this.lblcodigoSecMesa.Name = "lblcodigoSecMesa";
            this.lblcodigoSecMesa.Size = new System.Drawing.Size(49, 15);
            this.lblcodigoSecMesa.TabIndex = 3;
            this.lblcodigoSecMesa.Text = "Código:";
            // 
            // txtCodigo
            // 
            this.txtCodigo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCodigo.Location = new System.Drawing.Point(109, 24);
            this.txtCodigo.MaxLength = 20;
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(290, 20);
            this.txtCodigo.TabIndex = 3;
            // 
            // cmbLocalidades
            // 
            this.cmbLocalidades.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocalidades.FormattingEnabled = true;
            this.cmbLocalidades.Location = new System.Drawing.Point(121, 72);
            this.cmbLocalidades.Name = "cmbLocalidades";
            this.cmbLocalidades.Size = new System.Drawing.Size(290, 21);
            this.cmbLocalidades.TabIndex = 13;
            this.cmbLocalidades.SelectedIndexChanged += new System.EventHandler(this.cmbLocalidades_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(37, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 15);
            this.label3.TabIndex = 14;
            this.label3.Text = "Localidad:";
            // 
            // id_pos_seccion_mesa
            // 
            this.id_pos_seccion_mesa.HeaderText = "ID SECCION";
            this.id_pos_seccion_mesa.Name = "id_pos_seccion_mesa";
            this.id_pos_seccion_mesa.ReadOnly = true;
            this.id_pos_seccion_mesa.Visible = false;
            // 
            // id_localidad
            // 
            this.id_localidad.HeaderText = "ID LOCALIDAD";
            this.id_localidad.Name = "id_localidad";
            this.id_localidad.ReadOnly = true;
            this.id_localidad.Visible = false;
            // 
            // is_active
            // 
            this.is_active.HeaderText = "IS ACTIVE";
            this.is_active.Name = "is_active";
            this.is_active.ReadOnly = true;
            this.is_active.Visible = false;
            // 
            // fondo_pantalla
            // 
            this.fondo_pantalla.HeaderText = "FONDO PANTALLA";
            this.fondo_pantalla.Name = "fondo_pantalla";
            this.fondo_pantalla.ReadOnly = true;
            this.fondo_pantalla.Visible = false;
            // 
            // color
            // 
            this.color.HeaderText = "COLOR";
            this.color.Name = "color";
            this.color.ReadOnly = true;
            this.color.Visible = false;
            // 
            // codigo
            // 
            this.codigo.HeaderText = "CÓDIGO";
            this.codigo.Name = "codigo";
            this.codigo.ReadOnly = true;
            // 
            // descripcion
            // 
            this.descripcion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.descripcion.HeaderText = "DESCRIPCIÓN";
            this.descripcion.Name = "descripcion";
            this.descripcion.ReadOnly = true;
            // 
            // estado
            // 
            this.estado.HeaderText = "ESTADO";
            this.estado.Name = "estado";
            this.estado.ReadOnly = true;
            // 
            // FInformacionPosSecMes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(872, 368);
            this.Controls.Add(this.Grb_Registros);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Grb_Opciones);
            this.Controls.Add(this.cmbLocalidades);
            this.Controls.Add(this.grupoDatos);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FInformacionPosSecMes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Módulo de Configuración de Secciones para Mesas";
            this.Load += new System.EventHandler(this.FInformacionPosSecMes_Load);
            this.Grb_Registros.ResumeLayout(false);
            this.Grb_Registros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            this.Grb_Opciones.ResumeLayout(false);
            this.grupoDatos.ResumeLayout(false);
            this.grupoDatos.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox Grb_Registros;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.DataGridView dgvDatos;
        private System.Windows.Forms.GroupBox Grb_Opciones;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnAnular;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.GroupBox grupoDatos;
        private System.Windows.Forms.ComboBox cmbPaleta;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDescrSecMes;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblcodigoSecMesa;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRuta;
        private System.Windows.Forms.Button btnExaminar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbLocalidades;
        private System.Windows.Forms.CheckBox chkHabilitado;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_pos_seccion_mesa;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_localidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn is_active;
        private System.Windows.Forms.DataGridViewTextBoxColumn fondo_pantalla;
        private System.Windows.Forms.DataGridViewTextBoxColumn color;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn descripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn estado;

    }
}