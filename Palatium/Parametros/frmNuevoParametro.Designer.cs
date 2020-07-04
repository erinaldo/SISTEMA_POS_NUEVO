﻿namespace Palatium.Parametros
{
    partial class frmNuevoParametro
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
            this.tbControl = new System.Windows.Forms.TabControl();
            this.tabPorcentajes = new System.Windows.Forms.TabPage();
            this.chkIncluirImpuestos = new System.Windows.Forms.CheckBox();
            this.chkNomina = new System.Windows.Forms.CheckBox();
            this.chkMostrarNombreMesa = new System.Windows.Forms.CheckBox();
            this.chkUsuariosLogin = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtIce = new System.Windows.Forms.TextBox();
            this.txtIva = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtPorcentajeServicio = new System.Windows.Forms.TextBox();
            this.chkManejaServicio = new System.Windows.Forms.CheckBox();
            this.tabValores = new System.Windows.Forms.TabPage();
            this.BtnLimpiarMovilizacion = new System.Windows.Forms.Button();
            this.BtnLimpiarModificadores = new System.Windows.Forms.Button();
            this.dBAyudaNuevoItem = new ControlesPersonalizados.DB_Ayuda();
            this.dBAyudaMovilizacion = new ControlesPersonalizados.DB_Ayuda();
            this.dBAyudaModificadores = new ControlesPersonalizados.DB_Ayuda();
            this.BtnLimpiarNuevoItem = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabParametros = new System.Windows.Forms.TabPage();
            this.btnLimpiarRespaldo = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btnExaminarDirectorio = new System.Windows.Forms.Button();
            this.txtUrlRespaldoBDD = new System.Windows.Forms.TextBox();
            this.btnRemoverContable = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.btnExaminarContable = new System.Windows.Forms.Button();
            this.txtUrlContable = new System.Windows.Forms.TextBox();
            this.txtSitioWeb = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtTelefono = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbTipoComprobante = new ControlesPersonalizados.ComboDatos();
            this.label30 = new System.Windows.Forms.Label();
            this.tabComanda = new System.Windows.Forms.TabPage();
            this.chkUsarIconosProductos = new System.Windows.Forms.CheckBox();
            this.chkUsarIconosCategorias = new System.Windows.Forms.CheckBox();
            this.tabHuellas = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkHuellasMeseros = new System.Windows.Forms.CheckBox();
            this.chkHuellasCajeros = new System.Windows.Forms.CheckBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.fbRuta = new System.Windows.Forms.FolderBrowserDialog();
            this.chkMostrarTotalLineaComanda = new System.Windows.Forms.CheckBox();
            this.tbControl.SuspendLayout();
            this.tabPorcentajes.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabValores.SuspendLayout();
            this.tabParametros.SuspendLayout();
            this.tabComanda.SuspendLayout();
            this.tabHuellas.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbControl
            // 
            this.tbControl.Controls.Add(this.tabPorcentajes);
            this.tbControl.Controls.Add(this.tabValores);
            this.tbControl.Controls.Add(this.tabParametros);
            this.tbControl.Controls.Add(this.tabComanda);
            this.tbControl.Controls.Add(this.tabHuellas);
            this.tbControl.Location = new System.Drawing.Point(0, 60);
            this.tbControl.Name = "tbControl";
            this.tbControl.SelectedIndex = 0;
            this.tbControl.Size = new System.Drawing.Size(592, 184);
            this.tbControl.TabIndex = 1;
            this.tbControl.SelectedIndexChanged += new System.EventHandler(this.tbControl_SelectedIndexChanged);
            // 
            // tabPorcentajes
            // 
            this.tabPorcentajes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tabPorcentajes.Controls.Add(this.chkIncluirImpuestos);
            this.tabPorcentajes.Controls.Add(this.chkNomina);
            this.tabPorcentajes.Controls.Add(this.chkMostrarNombreMesa);
            this.tabPorcentajes.Controls.Add(this.chkUsuariosLogin);
            this.tabPorcentajes.Controls.Add(this.groupBox3);
            this.tabPorcentajes.Controls.Add(this.groupBox2);
            this.tabPorcentajes.Location = new System.Drawing.Point(4, 22);
            this.tabPorcentajes.Name = "tabPorcentajes";
            this.tabPorcentajes.Padding = new System.Windows.Forms.Padding(3);
            this.tabPorcentajes.Size = new System.Drawing.Size(584, 158);
            this.tabPorcentajes.TabIndex = 0;
            this.tabPorcentajes.Text = "Porcentajes del Sistema";
            // 
            // chkIncluirImpuestos
            // 
            this.chkIncluirImpuestos.AutoSize = true;
            this.chkIncluirImpuestos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.chkIncluirImpuestos.Location = new System.Drawing.Point(284, 133);
            this.chkIncluirImpuestos.Name = "chkIncluirImpuestos";
            this.chkIncluirImpuestos.Size = new System.Drawing.Size(198, 19);
            this.chkIncluirImpuestos.TabIndex = 119;
            this.chkIncluirImpuestos.Text = "Incluir impuestos en los precios";
            this.chkIncluirImpuestos.UseVisualStyleBackColor = true;
            // 
            // chkNomina
            // 
            this.chkNomina.AutoSize = true;
            this.chkNomina.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.chkNomina.Location = new System.Drawing.Point(284, 114);
            this.chkNomina.Name = "chkNomina";
            this.chkNomina.Size = new System.Drawing.Size(189, 19);
            this.chkNomina.TabIndex = 118;
            this.chkNomina.Text = "Usar nómina para el personal";
            this.chkNomina.UseVisualStyleBackColor = true;
            // 
            // chkMostrarNombreMesa
            // 
            this.chkMostrarNombreMesa.AutoSize = true;
            this.chkMostrarNombreMesa.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.chkMostrarNombreMesa.Location = new System.Drawing.Point(57, 114);
            this.chkMostrarNombreMesa.Name = "chkMostrarNombreMesa";
            this.chkMostrarNombreMesa.Size = new System.Drawing.Size(200, 19);
            this.chkMostrarNombreMesa.TabIndex = 117;
            this.chkMostrarNombreMesa.Text = "Mostrar Descripción de la Mesa";
            this.chkMostrarNombreMesa.UseVisualStyleBackColor = true;
            // 
            // chkUsuariosLogin
            // 
            this.chkUsuariosLogin.AutoSize = true;
            this.chkUsuariosLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.chkUsuariosLogin.Location = new System.Drawing.Point(57, 133);
            this.chkUsuariosLogin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkUsuariosLogin.Name = "chkUsuariosLogin";
            this.chkUsuariosLogin.Size = new System.Drawing.Size(171, 19);
            this.chkUsuariosLogin.TabIndex = 115;
            this.chkUsuariosLogin.Text = "Mostrar Usuarios en Login";
            this.chkUsuariosLogin.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.txtIce);
            this.groupBox3.Controls.Add(this.txtIva);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.groupBox3.Location = new System.Drawing.Point(57, 18);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(214, 90);
            this.groupBox3.TabIndex = 114;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Valores de configuración";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 28);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(89, 15);
            this.label16.TabIndex = 3;
            this.label16.Text = "Porcentaje IVA:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Porcentaje ICE:";
            // 
            // txtIce
            // 
            this.txtIce.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIce.Location = new System.Drawing.Point(134, 50);
            this.txtIce.Name = "txtIce";
            this.txtIce.Size = new System.Drawing.Size(53, 22);
            this.txtIce.TabIndex = 6;
            this.txtIce.Text = "0";
            this.txtIce.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtIce.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtIce_KeyPress);
            this.txtIce.Leave += new System.EventHandler(this.txtIce_Leave);
            // 
            // txtIva
            // 
            this.txtIva.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIva.Location = new System.Drawing.Point(134, 25);
            this.txtIva.Name = "txtIva";
            this.txtIva.Size = new System.Drawing.Size(53, 22);
            this.txtIva.TabIndex = 4;
            this.txtIva.Text = "0";
            this.txtIva.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtIva.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtIva_KeyPress);
            this.txtIva.Leave += new System.EventHandler(this.txtIva_Leave);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.txtPorcentajeServicio);
            this.groupBox2.Controls.Add(this.chkManejaServicio);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.groupBox2.Location = new System.Drawing.Point(277, 18);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(214, 90);
            this.groupBox2.TabIndex = 112;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Servicio (Categoría A)";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(9, 57);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(132, 15);
            this.label18.TabIndex = 113;
            this.label18.Text = "Porcentaje de Servicio:";
            // 
            // txtPorcentajeServicio
            // 
            this.txtPorcentajeServicio.Location = new System.Drawing.Point(147, 54);
            this.txtPorcentajeServicio.MaxLength = 5;
            this.txtPorcentajeServicio.Name = "txtPorcentajeServicio";
            this.txtPorcentajeServicio.Size = new System.Drawing.Size(53, 21);
            this.txtPorcentajeServicio.TabIndex = 47;
            this.txtPorcentajeServicio.Text = "0";
            this.txtPorcentajeServicio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPorcentajeServicio.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPorcentajeServicio_KeyPress);
            this.txtPorcentajeServicio.Leave += new System.EventHandler(this.txtPorcentajeServicio_Leave);
            // 
            // chkManejaServicio
            // 
            this.chkManejaServicio.AutoSize = true;
            this.chkManejaServicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkManejaServicio.Location = new System.Drawing.Point(7, 25);
            this.chkManejaServicio.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkManejaServicio.Name = "chkManejaServicio";
            this.chkManejaServicio.Size = new System.Drawing.Size(114, 19);
            this.chkManejaServicio.TabIndex = 111;
            this.chkManejaServicio.Text = "Maneja Servicio";
            this.chkManejaServicio.UseVisualStyleBackColor = true;
            this.chkManejaServicio.CheckedChanged += new System.EventHandler(this.chkManejaServicio_CheckedChanged);
            // 
            // tabValores
            // 
            this.tabValores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.tabValores.Controls.Add(this.BtnLimpiarMovilizacion);
            this.tabValores.Controls.Add(this.BtnLimpiarModificadores);
            this.tabValores.Controls.Add(this.dBAyudaNuevoItem);
            this.tabValores.Controls.Add(this.dBAyudaMovilizacion);
            this.tabValores.Controls.Add(this.dBAyudaModificadores);
            this.tabValores.Controls.Add(this.BtnLimpiarNuevoItem);
            this.tabValores.Controls.Add(this.label4);
            this.tabValores.Controls.Add(this.label3);
            this.tabValores.Controls.Add(this.label2);
            this.tabValores.Location = new System.Drawing.Point(4, 22);
            this.tabValores.Name = "tabValores";
            this.tabValores.Padding = new System.Windows.Forms.Padding(3);
            this.tabValores.Size = new System.Drawing.Size(584, 158);
            this.tabValores.TabIndex = 1;
            this.tabValores.Text = "Configuración de Ítems";
            // 
            // BtnLimpiarMovilizacion
            // 
            this.BtnLimpiarMovilizacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnLimpiarMovilizacion.ForeColor = System.Drawing.Color.Red;
            this.BtnLimpiarMovilizacion.Location = new System.Drawing.Point(549, 73);
            this.BtnLimpiarMovilizacion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnLimpiarMovilizacion.Name = "BtnLimpiarMovilizacion";
            this.BtnLimpiarMovilizacion.Size = new System.Drawing.Size(28, 25);
            this.BtnLimpiarMovilizacion.TabIndex = 122;
            this.BtnLimpiarMovilizacion.Text = "X";
            this.BtnLimpiarMovilizacion.UseVisualStyleBackColor = true;
            // 
            // BtnLimpiarModificadores
            // 
            this.BtnLimpiarModificadores.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnLimpiarModificadores.ForeColor = System.Drawing.Color.Red;
            this.BtnLimpiarModificadores.Location = new System.Drawing.Point(549, 30);
            this.BtnLimpiarModificadores.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnLimpiarModificadores.Name = "BtnLimpiarModificadores";
            this.BtnLimpiarModificadores.Size = new System.Drawing.Size(28, 25);
            this.BtnLimpiarModificadores.TabIndex = 119;
            this.BtnLimpiarModificadores.Text = "X";
            this.BtnLimpiarModificadores.UseVisualStyleBackColor = true;
            // 
            // dBAyudaNuevoItem
            // 
            this.dBAyudaNuevoItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.dBAyudaNuevoItem.iId = 0;
            this.dBAyudaNuevoItem.Location = new System.Drawing.Point(13, 119);
            this.dBAyudaNuevoItem.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dBAyudaNuevoItem.Name = "dBAyudaNuevoItem";
            this.dBAyudaNuevoItem.sDatosConsulta = null;
            this.dBAyudaNuevoItem.sDescripcion = null;
            this.dBAyudaNuevoItem.Size = new System.Drawing.Size(528, 22);
            this.dBAyudaNuevoItem.TabIndex = 108;
            // 
            // dBAyudaMovilizacion
            // 
            this.dBAyudaMovilizacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.dBAyudaMovilizacion.iId = 0;
            this.dBAyudaMovilizacion.Location = new System.Drawing.Point(13, 76);
            this.dBAyudaMovilizacion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dBAyudaMovilizacion.Name = "dBAyudaMovilizacion";
            this.dBAyudaMovilizacion.sDatosConsulta = null;
            this.dBAyudaMovilizacion.sDescripcion = null;
            this.dBAyudaMovilizacion.Size = new System.Drawing.Size(528, 22);
            this.dBAyudaMovilizacion.TabIndex = 107;
            // 
            // dBAyudaModificadores
            // 
            this.dBAyudaModificadores.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.dBAyudaModificadores.iId = 0;
            this.dBAyudaModificadores.Location = new System.Drawing.Point(13, 33);
            this.dBAyudaModificadores.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dBAyudaModificadores.Name = "dBAyudaModificadores";
            this.dBAyudaModificadores.sDatosConsulta = null;
            this.dBAyudaModificadores.sDescripcion = null;
            this.dBAyudaModificadores.Size = new System.Drawing.Size(528, 22);
            this.dBAyudaModificadores.TabIndex = 106;
            // 
            // BtnLimpiarNuevoItem
            // 
            this.BtnLimpiarNuevoItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnLimpiarNuevoItem.ForeColor = System.Drawing.Color.Red;
            this.BtnLimpiarNuevoItem.Location = new System.Drawing.Point(549, 119);
            this.BtnLimpiarNuevoItem.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnLimpiarNuevoItem.Name = "BtnLimpiarNuevoItem";
            this.BtnLimpiarNuevoItem.Size = new System.Drawing.Size(28, 25);
            this.BtnLimpiarNuevoItem.TabIndex = 96;
            this.BtnLimpiarNuevoItem.Text = "X";
            this.BtnLimpiarNuevoItem.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(13, 101);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 15);
            this.label4.TabIndex = 93;
            this.label4.Text = "Nuevo Ítem en Pos:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 58);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 15);
            this.label3.TabIndex = 92;
            this.label3.Text = "Ítem Movilización:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 15);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 15);
            this.label2.TabIndex = 91;
            this.label2.Text = "Modificadores:";
            // 
            // tabParametros
            // 
            this.tabParametros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tabParametros.Controls.Add(this.btnLimpiarRespaldo);
            this.tabParametros.Controls.Add(this.label7);
            this.tabParametros.Controls.Add(this.btnExaminarDirectorio);
            this.tabParametros.Controls.Add(this.txtUrlRespaldoBDD);
            this.tabParametros.Controls.Add(this.btnRemoverContable);
            this.tabParametros.Controls.Add(this.label14);
            this.tabParametros.Controls.Add(this.btnExaminarContable);
            this.tabParametros.Controls.Add(this.txtUrlContable);
            this.tabParametros.Controls.Add(this.txtSitioWeb);
            this.tabParametros.Controls.Add(this.label12);
            this.tabParametros.Controls.Add(this.txtTelefono);
            this.tabParametros.Controls.Add(this.label5);
            this.tabParametros.Controls.Add(this.cmbTipoComprobante);
            this.tabParametros.Controls.Add(this.label30);
            this.tabParametros.Location = new System.Drawing.Point(4, 22);
            this.tabParametros.Name = "tabParametros";
            this.tabParametros.Padding = new System.Windows.Forms.Padding(3);
            this.tabParametros.Size = new System.Drawing.Size(584, 158);
            this.tabParametros.TabIndex = 2;
            this.tabParametros.Text = "Parametrización Adicional";
            // 
            // btnLimpiarRespaldo
            // 
            this.btnLimpiarRespaldo.BackColor = System.Drawing.Color.Red;
            this.btnLimpiarRespaldo.ForeColor = System.Drawing.Color.Black;
            this.btnLimpiarRespaldo.Location = new System.Drawing.Point(517, 94);
            this.btnLimpiarRespaldo.Name = "btnLimpiarRespaldo";
            this.btnLimpiarRespaldo.Size = new System.Drawing.Size(27, 25);
            this.btnLimpiarRespaldo.TabIndex = 128;
            this.btnLimpiarRespaldo.Text = "X";
            this.btnLimpiarRespaldo.UseVisualStyleBackColor = false;
            this.btnLimpiarRespaldo.Click += new System.EventHandler(this.btnLimpiarRespaldo_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label7.Location = new System.Drawing.Point(38, 99);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 15);
            this.label7.TabIndex = 129;
            this.label7.Text = "URL Backup:";
            // 
            // btnExaminarDirectorio
            // 
            this.btnExaminarDirectorio.BackColor = System.Drawing.Color.Yellow;
            this.btnExaminarDirectorio.ForeColor = System.Drawing.Color.Black;
            this.btnExaminarDirectorio.Location = new System.Drawing.Point(484, 94);
            this.btnExaminarDirectorio.Name = "btnExaminarDirectorio";
            this.btnExaminarDirectorio.Size = new System.Drawing.Size(27, 25);
            this.btnExaminarDirectorio.TabIndex = 127;
            this.btnExaminarDirectorio.Text = "...";
            this.btnExaminarDirectorio.UseVisualStyleBackColor = false;
            this.btnExaminarDirectorio.Click += new System.EventHandler(this.btnExaminarDirectorio_Click);
            // 
            // txtUrlRespaldoBDD
            // 
            this.txtUrlRespaldoBDD.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtUrlRespaldoBDD.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUrlRespaldoBDD.Location = new System.Drawing.Point(152, 96);
            this.txtUrlRespaldoBDD.Name = "txtUrlRespaldoBDD";
            this.txtUrlRespaldoBDD.ReadOnly = true;
            this.txtUrlRespaldoBDD.Size = new System.Drawing.Size(326, 22);
            this.txtUrlRespaldoBDD.TabIndex = 126;
            // 
            // btnRemoverContable
            // 
            this.btnRemoverContable.BackColor = System.Drawing.Color.Red;
            this.btnRemoverContable.ForeColor = System.Drawing.Color.Black;
            this.btnRemoverContable.Location = new System.Drawing.Point(517, 65);
            this.btnRemoverContable.Name = "btnRemoverContable";
            this.btnRemoverContable.Size = new System.Drawing.Size(27, 25);
            this.btnRemoverContable.TabIndex = 124;
            this.btnRemoverContable.Text = "X";
            this.btnRemoverContable.UseVisualStyleBackColor = false;
            this.btnRemoverContable.Click += new System.EventHandler(this.btnRemoverContable_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label14.Location = new System.Drawing.Point(38, 71);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(87, 15);
            this.label14.TabIndex = 125;
            this.label14.Text = "URL Palatium:";
            // 
            // btnExaminarContable
            // 
            this.btnExaminarContable.BackColor = System.Drawing.Color.Yellow;
            this.btnExaminarContable.ForeColor = System.Drawing.Color.Black;
            this.btnExaminarContable.Location = new System.Drawing.Point(484, 66);
            this.btnExaminarContable.Name = "btnExaminarContable";
            this.btnExaminarContable.Size = new System.Drawing.Size(27, 25);
            this.btnExaminarContable.TabIndex = 123;
            this.btnExaminarContable.Text = "...";
            this.btnExaminarContable.UseVisualStyleBackColor = false;
            this.btnExaminarContable.Click += new System.EventHandler(this.btnExaminarContable_Click);
            // 
            // txtUrlContable
            // 
            this.txtUrlContable.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtUrlContable.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUrlContable.Location = new System.Drawing.Point(152, 68);
            this.txtUrlContable.Name = "txtUrlContable";
            this.txtUrlContable.ReadOnly = true;
            this.txtUrlContable.Size = new System.Drawing.Size(326, 22);
            this.txtUrlContable.TabIndex = 122;
            // 
            // txtSitioWeb
            // 
            this.txtSitioWeb.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtSitioWeb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSitioWeb.Location = new System.Drawing.Point(152, 40);
            this.txtSitioWeb.MaxLength = 100;
            this.txtSitioWeb.Name = "txtSitioWeb";
            this.txtSitioWeb.Size = new System.Drawing.Size(392, 22);
            this.txtSitioWeb.TabIndex = 120;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label12.Location = new System.Drawing.Point(38, 43);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 15);
            this.label12.TabIndex = 121;
            this.label12.Text = "Sitio Web:";
            // 
            // txtTelefono
            // 
            this.txtTelefono.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTelefono.Location = new System.Drawing.Point(152, 12);
            this.txtTelefono.MaxLength = 15;
            this.txtTelefono.Name = "txtTelefono";
            this.txtTelefono.Size = new System.Drawing.Size(157, 22);
            this.txtTelefono.TabIndex = 118;
            this.txtTelefono.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTelefono_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label5.Location = new System.Drawing.Point(38, 15);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 15);
            this.label5.TabIndex = 119;
            this.label5.Text = "Teléfono:";
            // 
            // cmbTipoComprobante
            // 
            this.cmbTipoComprobante.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTipoComprobante.FormattingEnabled = true;
            this.cmbTipoComprobante.Location = new System.Drawing.Point(152, 124);
            this.cmbTipoComprobante.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbTipoComprobante.Name = "cmbTipoComprobante";
            this.cmbTipoComprobante.Size = new System.Drawing.Size(241, 23);
            this.cmbTipoComprobante.TabIndex = 116;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label30.Location = new System.Drawing.Point(38, 122);
            this.label30.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(99, 30);
            this.label30.TabIndex = 117;
            this.label30.Text = "Registro de Nota\r\nde Entrega";
            // 
            // tabComanda
            // 
            this.tabComanda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tabComanda.Controls.Add(this.chkMostrarTotalLineaComanda);
            this.tabComanda.Controls.Add(this.chkUsarIconosProductos);
            this.tabComanda.Controls.Add(this.chkUsarIconosCategorias);
            this.tabComanda.Location = new System.Drawing.Point(4, 22);
            this.tabComanda.Name = "tabComanda";
            this.tabComanda.Padding = new System.Windows.Forms.Padding(3);
            this.tabComanda.Size = new System.Drawing.Size(584, 158);
            this.tabComanda.TabIndex = 3;
            this.tabComanda.Text = "Configurar Comanda";
            // 
            // chkUsarIconosProductos
            // 
            this.chkUsarIconosProductos.AutoSize = true;
            this.chkUsarIconosProductos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.chkUsarIconosProductos.Location = new System.Drawing.Point(24, 44);
            this.chkUsarIconosProductos.Name = "chkUsarIconosProductos";
            this.chkUsarIconosProductos.Size = new System.Drawing.Size(268, 19);
            this.chkUsarIconosProductos.TabIndex = 119;
            this.chkUsarIconosProductos.Text = "Usar íconos en productos del punto de venta";
            this.chkUsarIconosProductos.UseVisualStyleBackColor = true;
            // 
            // chkUsarIconosCategorias
            // 
            this.chkUsarIconosCategorias.AutoSize = true;
            this.chkUsarIconosCategorias.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.chkUsarIconosCategorias.Location = new System.Drawing.Point(24, 19);
            this.chkUsarIconosCategorias.Name = "chkUsarIconosCategorias";
            this.chkUsarIconosCategorias.Size = new System.Drawing.Size(274, 19);
            this.chkUsarIconosCategorias.TabIndex = 118;
            this.chkUsarIconosCategorias.Text = "Usar íconos en las categorías de la Comanda";
            this.chkUsarIconosCategorias.UseVisualStyleBackColor = true;
            // 
            // tabHuellas
            // 
            this.tabHuellas.BackColor = System.Drawing.Color.Teal;
            this.tabHuellas.Controls.Add(this.groupBox1);
            this.tabHuellas.Location = new System.Drawing.Point(4, 22);
            this.tabHuellas.Name = "tabHuellas";
            this.tabHuellas.Padding = new System.Windows.Forms.Padding(3);
            this.tabHuellas.Size = new System.Drawing.Size(584, 158);
            this.tabHuellas.TabIndex = 4;
            this.tabHuellas.Text = "Huellas Dactilares";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkHuellasMeseros);
            this.groupBox1.Controls.Add(this.chkHuellasCajeros);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Location = new System.Drawing.Point(8, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(225, 75);
            this.groupBox1.TabIndex = 121;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Uso de lector de huellas";
            // 
            // chkHuellasMeseros
            // 
            this.chkHuellasMeseros.AutoSize = true;
            this.chkHuellasMeseros.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.chkHuellasMeseros.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkHuellasMeseros.Location = new System.Drawing.Point(25, 45);
            this.chkHuellasMeseros.Name = "chkHuellasMeseros";
            this.chkHuellasMeseros.Size = new System.Drawing.Size(185, 19);
            this.chkHuellasMeseros.TabIndex = 122;
            this.chkHuellasMeseros.Text = "Acceso al sistema a meseros";
            this.chkHuellasMeseros.UseVisualStyleBackColor = true;
            // 
            // chkHuellasCajeros
            // 
            this.chkHuellasCajeros.AutoSize = true;
            this.chkHuellasCajeros.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.chkHuellasCajeros.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkHuellasCajeros.Location = new System.Drawing.Point(25, 20);
            this.chkHuellasCajeros.Name = "chkHuellasCajeros";
            this.chkHuellasCajeros.Size = new System.Drawing.Size(177, 19);
            this.chkHuellasCajeros.TabIndex = 121;
            this.chkHuellasCajeros.Text = "Acceso al sistema a cajeros";
            this.chkHuellasCajeros.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            this.btnGuardar.BackColor = System.Drawing.Color.Blue;
            this.btnGuardar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Location = new System.Drawing.Point(508, 12);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(70, 39);
            this.btnGuardar.TabIndex = 6;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Maiandra GD", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label6.Location = new System.Drawing.Point(24, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(353, 19);
            this.label6.TabIndex = 7;
            this.label6.Text = "CONFIGURACIÓN GENERAL DEL SISTEMA";
            // 
            // chkMostrarTotalLineaComanda
            // 
            this.chkMostrarTotalLineaComanda.AutoSize = true;
            this.chkMostrarTotalLineaComanda.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.chkMostrarTotalLineaComanda.Location = new System.Drawing.Point(24, 69);
            this.chkMostrarTotalLineaComanda.Name = "chkMostrarTotalLineaComanda";
            this.chkMostrarTotalLineaComanda.Size = new System.Drawing.Size(294, 19);
            this.chkMostrarTotalLineaComanda.TabIndex = 120;
            this.chkMostrarTotalLineaComanda.Text = "Mostrar total en línea de producto en la comanda";
            this.chkMostrarTotalLineaComanda.UseVisualStyleBackColor = true;
            // 
            // frmNuevoParametro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(590, 244);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.tbControl);
            this.MaximizeBox = false;
            this.Name = "frmNuevoParametro";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuración general de parámetros";
            this.Load += new System.EventHandler(this.frmNuevoParametro_Load);
            this.tbControl.ResumeLayout(false);
            this.tabPorcentajes.ResumeLayout(false);
            this.tabPorcentajes.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabValores.ResumeLayout(false);
            this.tabValores.PerformLayout();
            this.tabParametros.ResumeLayout(false);
            this.tabParametros.PerformLayout();
            this.tabComanda.ResumeLayout(false);
            this.tabComanda.PerformLayout();
            this.tabHuellas.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tbControl;
        private System.Windows.Forms.TabPage tabPorcentajes;
        private System.Windows.Forms.TabPage tabValores;
        private System.Windows.Forms.Button BtnLimpiarMovilizacion;
        private System.Windows.Forms.Button BtnLimpiarModificadores;
        private ControlesPersonalizados.DB_Ayuda dBAyudaNuevoItem;
        private ControlesPersonalizados.DB_Ayuda dBAyudaMovilizacion;
        private ControlesPersonalizados.DB_Ayuda dBAyudaModificadores;
        private System.Windows.Forms.Button BtnLimpiarNuevoItem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabParametros;
        private ControlesPersonalizados.ComboDatos cmbTipoComprobante;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TextBox txtIce;
        private System.Windows.Forms.TextBox txtIva;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtPorcentajeServicio;
        private System.Windows.Forms.CheckBox chkManejaServicio;
        private System.Windows.Forms.CheckBox chkUsuariosLogin;
        private System.Windows.Forms.CheckBox chkMostrarNombreMesa;
        private System.Windows.Forms.TextBox txtSitioWeb;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtTelefono;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnRemoverContable;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnExaminarContable;
        private System.Windows.Forms.TextBox txtUrlContable;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkNomina;
        private System.Windows.Forms.CheckBox chkIncluirImpuestos;
        private System.Windows.Forms.Button btnLimpiarRespaldo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnExaminarDirectorio;
        private System.Windows.Forms.TextBox txtUrlRespaldoBDD;
        private System.Windows.Forms.FolderBrowserDialog fbRuta;
        private System.Windows.Forms.TabPage tabComanda;
        private System.Windows.Forms.CheckBox chkUsarIconosCategorias;
        private System.Windows.Forms.TabPage tabHuellas;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkHuellasMeseros;
        private System.Windows.Forms.CheckBox chkHuellasCajeros;
        private System.Windows.Forms.CheckBox chkUsarIconosProductos;
        private System.Windows.Forms.CheckBox chkMostrarTotalLineaComanda;
    }
}