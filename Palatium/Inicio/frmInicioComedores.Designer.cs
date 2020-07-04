﻿namespace Palatium.Inicio
{
    partial class frmInicioComedores
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInicioComedores));
            this.logo = new System.Windows.Forms.PictureBox();
            this.btnVentaExpress = new System.Windows.Forms.Button();
            this.btnClienteEmpresarial = new System.Windows.Forms.Button();
            this.btnCobroAlmuerzos = new System.Windows.Forms.Button();
            this.btnSalidaCajero = new System.Windows.Forms.Button();
            this.btnMovimientoCaja = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnRevisar = new System.Windows.Forms.Button();
            this.btnTarjetaAlmuerzo = new System.Windows.Forms.Button();
            this.btnDatosClientes = new System.Windows.Forms.Button();
            this.btnGenerarFactura = new System.Windows.Forms.Button();
            this.btnDetalleClienteEmpresarial = new System.Windows.Forms.Button();
            this.btnConsumoInterno = new System.Windows.Forms.Button();
            this.btnBusquedaPorIdentificacion = new System.Windows.Forms.Button();
            this.lblVersionDemo = new System.Windows.Forms.Label();
            this.lblNombreEquipo = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            this.SuspendLayout();
            // 
            // logo
            // 
            this.logo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("logo.BackgroundImage")));
            this.logo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.logo.Location = new System.Drawing.Point(45, 76);
            this.logo.Margin = new System.Windows.Forms.Padding(2);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(457, 501);
            this.logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logo.TabIndex = 43;
            this.logo.TabStop = false;
            // 
            // btnVentaExpress
            // 
            this.btnVentaExpress.BackColor = System.Drawing.Color.Navy;
            this.btnVentaExpress.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnVentaExpress.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVentaExpress.FlatAppearance.BorderSize = 2;
            this.btnVentaExpress.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVentaExpress.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVentaExpress.ForeColor = System.Drawing.Color.White;
            this.btnVentaExpress.Image = global::Palatium.Properties.Resources.icono_comida_rapida;
            this.btnVentaExpress.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnVentaExpress.Location = new System.Drawing.Point(928, 76);
            this.btnVentaExpress.Name = "btnVentaExpress";
            this.btnVentaExpress.Size = new System.Drawing.Size(192, 138);
            this.btnVentaExpress.TabIndex = 76;
            this.btnVentaExpress.Text = "Venta Rápida\r\nHappy Hour";
            this.btnVentaExpress.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVentaExpress.UseVisualStyleBackColor = false;
            this.btnVentaExpress.Click += new System.EventHandler(this.btnVentaExpress_Click);
            this.btnVentaExpress.MouseEnter += new System.EventHandler(this.btnVentaExpress_MouseEnter);
            this.btnVentaExpress.MouseLeave += new System.EventHandler(this.btnVentaExpress_MouseLeave);
            // 
            // btnClienteEmpresarial
            // 
            this.btnClienteEmpresarial.BackColor = System.Drawing.Color.Navy;
            this.btnClienteEmpresarial.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClienteEmpresarial.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClienteEmpresarial.FlatAppearance.BorderSize = 2;
            this.btnClienteEmpresarial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClienteEmpresarial.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClienteEmpresarial.ForeColor = System.Drawing.Color.White;
            this.btnClienteEmpresarial.Image = global::Palatium.Properties.Resources.icono_cliente_empresarial;
            this.btnClienteEmpresarial.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClienteEmpresarial.Location = new System.Drawing.Point(532, 76);
            this.btnClienteEmpresarial.Name = "btnClienteEmpresarial";
            this.btnClienteEmpresarial.Size = new System.Drawing.Size(192, 138);
            this.btnClienteEmpresarial.TabIndex = 75;
            this.btnClienteEmpresarial.Text = "Cliente Empresarial\r\n ";
            this.btnClienteEmpresarial.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClienteEmpresarial.UseVisualStyleBackColor = false;
            this.btnClienteEmpresarial.Click += new System.EventHandler(this.btnClienteEmpresarial_Click);
            this.btnClienteEmpresarial.MouseEnter += new System.EventHandler(this.btnClienteEmpresarial_MouseEnter);
            this.btnClienteEmpresarial.MouseLeave += new System.EventHandler(this.btnClienteEmpresarial_MouseLeave);
            // 
            // btnCobroAlmuerzos
            // 
            this.btnCobroAlmuerzos.BackColor = System.Drawing.Color.Navy;
            this.btnCobroAlmuerzos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCobroAlmuerzos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCobroAlmuerzos.FlatAppearance.BorderSize = 2;
            this.btnCobroAlmuerzos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCobroAlmuerzos.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnCobroAlmuerzos.ForeColor = System.Drawing.Color.White;
            this.btnCobroAlmuerzos.Image = global::Palatium.Properties.Resources.cobrar_almuerzos1;
            this.btnCobroAlmuerzos.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCobroAlmuerzos.Location = new System.Drawing.Point(532, 220);
            this.btnCobroAlmuerzos.Name = "btnCobroAlmuerzos";
            this.btnCobroAlmuerzos.Size = new System.Drawing.Size(192, 138);
            this.btnCobroAlmuerzos.TabIndex = 77;
            this.btnCobroAlmuerzos.Text = "Cobro de Almuerzos\r\n ";
            this.btnCobroAlmuerzos.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCobroAlmuerzos.UseVisualStyleBackColor = false;
            this.btnCobroAlmuerzos.Click += new System.EventHandler(this.btnCobroAlmuerzos_Click);
            this.btnCobroAlmuerzos.MouseEnter += new System.EventHandler(this.btnCobroAlmuerzos_MouseEnter);
            this.btnCobroAlmuerzos.MouseLeave += new System.EventHandler(this.btnCobroAlmuerzos_MouseLeave);
            // 
            // btnSalidaCajero
            // 
            this.btnSalidaCajero.BackColor = System.Drawing.Color.Navy;
            this.btnSalidaCajero.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSalidaCajero.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSalidaCajero.FlatAppearance.BorderSize = 2;
            this.btnSalidaCajero.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalidaCajero.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalidaCajero.ForeColor = System.Drawing.Color.White;
            this.btnSalidaCajero.Image = global::Palatium.Properties.Resources.icono_abrir_caja;
            this.btnSalidaCajero.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSalidaCajero.Location = new System.Drawing.Point(1126, 439);
            this.btnSalidaCajero.Name = "btnSalidaCajero";
            this.btnSalidaCajero.Size = new System.Drawing.Size(192, 138);
            this.btnSalidaCajero.TabIndex = 81;
            this.btnSalidaCajero.Text = "Arqueo de Caja\r\n  ";
            this.btnSalidaCajero.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSalidaCajero.UseVisualStyleBackColor = false;
            this.btnSalidaCajero.Click += new System.EventHandler(this.btnSalidaCajero_Click);
            this.btnSalidaCajero.MouseEnter += new System.EventHandler(this.btnSalidaCajero_MouseEnter);
            this.btnSalidaCajero.MouseLeave += new System.EventHandler(this.btnSalidaCajero_MouseLeave);
            // 
            // btnMovimientoCaja
            // 
            this.btnMovimientoCaja.BackColor = System.Drawing.Color.Navy;
            this.btnMovimientoCaja.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnMovimientoCaja.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMovimientoCaja.FlatAppearance.BorderSize = 2;
            this.btnMovimientoCaja.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMovimientoCaja.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMovimientoCaja.ForeColor = System.Drawing.Color.White;
            this.btnMovimientoCaja.Image = global::Palatium.Properties.Resources.icono_caja_chica;
            this.btnMovimientoCaja.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMovimientoCaja.Location = new System.Drawing.Point(928, 439);
            this.btnMovimientoCaja.Name = "btnMovimientoCaja";
            this.btnMovimientoCaja.Size = new System.Drawing.Size(192, 138);
            this.btnMovimientoCaja.TabIndex = 80;
            this.btnMovimientoCaja.Text = "Movimientos de Caja\r\n ";
            this.btnMovimientoCaja.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnMovimientoCaja.UseVisualStyleBackColor = false;
            this.btnMovimientoCaja.Click += new System.EventHandler(this.btnMovimientoCaja_Click);
            this.btnMovimientoCaja.MouseEnter += new System.EventHandler(this.btnMovimientoCaja_MouseEnter);
            this.btnMovimientoCaja.MouseLeave += new System.EventHandler(this.btnMovimientoCaja_MouseLeave);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.Navy;
            this.btnCancelar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.FlatAppearance.BorderSize = 2;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Image = global::Palatium.Properties.Resources.icono_cancelar_orden;
            this.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCancelar.Location = new System.Drawing.Point(730, 439);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(192, 138);
            this.btnCancelar.TabIndex = 79;
            this.btnCancelar.Text = "Cancelar Orden\n  ";
            this.btnCancelar.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            this.btnCancelar.MouseEnter += new System.EventHandler(this.btnCancelar_MouseEnter);
            this.btnCancelar.MouseLeave += new System.EventHandler(this.btnCancelar_MouseLeave);
            // 
            // btnRevisar
            // 
            this.btnRevisar.BackColor = System.Drawing.Color.Navy;
            this.btnRevisar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnRevisar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRevisar.FlatAppearance.BorderSize = 2;
            this.btnRevisar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRevisar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRevisar.ForeColor = System.Drawing.Color.White;
            this.btnRevisar.Image = global::Palatium.Properties.Resources.icono_revisar;
            this.btnRevisar.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRevisar.Location = new System.Drawing.Point(532, 439);
            this.btnRevisar.Name = "btnRevisar";
            this.btnRevisar.Size = new System.Drawing.Size(192, 138);
            this.btnRevisar.TabIndex = 78;
            this.btnRevisar.Text = "Revisar Órdenes\r\n ";
            this.btnRevisar.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRevisar.UseVisualStyleBackColor = false;
            this.btnRevisar.Click += new System.EventHandler(this.btnRevisar_Click);
            this.btnRevisar.MouseEnter += new System.EventHandler(this.btnRevisar_MouseEnter);
            this.btnRevisar.MouseLeave += new System.EventHandler(this.btnRevisar_MouseLeave);
            // 
            // btnTarjetaAlmuerzo
            // 
            this.btnTarjetaAlmuerzo.BackColor = System.Drawing.Color.Navy;
            this.btnTarjetaAlmuerzo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnTarjetaAlmuerzo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTarjetaAlmuerzo.FlatAppearance.BorderSize = 2;
            this.btnTarjetaAlmuerzo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTarjetaAlmuerzo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTarjetaAlmuerzo.ForeColor = System.Drawing.Color.White;
            this.btnTarjetaAlmuerzo.Image = global::Palatium.Properties.Resources.tarjeta_almuerzo_inicio;
            this.btnTarjetaAlmuerzo.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnTarjetaAlmuerzo.Location = new System.Drawing.Point(248, 263);
            this.btnTarjetaAlmuerzo.Name = "btnTarjetaAlmuerzo";
            this.btnTarjetaAlmuerzo.Size = new System.Drawing.Size(192, 138);
            this.btnTarjetaAlmuerzo.TabIndex = 82;
            this.btnTarjetaAlmuerzo.Text = "Tarjetas de Almuerzo\r\n ";
            this.btnTarjetaAlmuerzo.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnTarjetaAlmuerzo.UseVisualStyleBackColor = false;
            this.btnTarjetaAlmuerzo.Visible = false;
            this.btnTarjetaAlmuerzo.Click += new System.EventHandler(this.btnTarjetaAlmuerzo_Click);
            this.btnTarjetaAlmuerzo.MouseEnter += new System.EventHandler(this.btnTarjetaAlmuerzo_MouseEnter);
            this.btnTarjetaAlmuerzo.MouseLeave += new System.EventHandler(this.btnTarjetaAlmuerzo_MouseLeave);
            // 
            // btnDatosClientes
            // 
            this.btnDatosClientes.BackColor = System.Drawing.Color.Navy;
            this.btnDatosClientes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDatosClientes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDatosClientes.FlatAppearance.BorderSize = 2;
            this.btnDatosClientes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDatosClientes.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnDatosClientes.ForeColor = System.Drawing.Color.White;
            this.btnDatosClientes.Image = global::Palatium.Properties.Resources.clientes_1_png;
            this.btnDatosClientes.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDatosClientes.Location = new System.Drawing.Point(1126, 220);
            this.btnDatosClientes.Name = "btnDatosClientes";
            this.btnDatosClientes.Size = new System.Drawing.Size(192, 138);
            this.btnDatosClientes.TabIndex = 83;
            this.btnDatosClientes.Text = "Clientes\r\n ";
            this.btnDatosClientes.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDatosClientes.UseVisualStyleBackColor = false;
            this.btnDatosClientes.Click += new System.EventHandler(this.btnDatosClientes_Click);
            this.btnDatosClientes.MouseEnter += new System.EventHandler(this.btnDatosClientes_MouseEnter);
            this.btnDatosClientes.MouseLeave += new System.EventHandler(this.btnDatosClientes_MouseLeave);
            // 
            // btnGenerarFactura
            // 
            this.btnGenerarFactura.BackColor = System.Drawing.Color.Navy;
            this.btnGenerarFactura.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGenerarFactura.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerarFactura.FlatAppearance.BorderSize = 2;
            this.btnGenerarFactura.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerarFactura.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnGenerarFactura.ForeColor = System.Drawing.Color.White;
            this.btnGenerarFactura.Image = global::Palatium.Properties.Resources.facturar_png1;
            this.btnGenerarFactura.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnGenerarFactura.Location = new System.Drawing.Point(730, 220);
            this.btnGenerarFactura.Name = "btnGenerarFactura";
            this.btnGenerarFactura.Size = new System.Drawing.Size(192, 138);
            this.btnGenerarFactura.TabIndex = 84;
            this.btnGenerarFactura.Text = "Generar Factura\r\nCliente Empresarial";
            this.btnGenerarFactura.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnGenerarFactura.UseVisualStyleBackColor = false;
            this.btnGenerarFactura.Click += new System.EventHandler(this.btnGenerarFactura_Click);
            this.btnGenerarFactura.MouseEnter += new System.EventHandler(this.btnGenerarFactura_MouseEnter);
            this.btnGenerarFactura.MouseLeave += new System.EventHandler(this.btnGenerarFactura_MouseLeave);
            // 
            // btnDetalleClienteEmpresarial
            // 
            this.btnDetalleClienteEmpresarial.BackColor = System.Drawing.Color.Navy;
            this.btnDetalleClienteEmpresarial.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDetalleClienteEmpresarial.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDetalleClienteEmpresarial.FlatAppearance.BorderSize = 2;
            this.btnDetalleClienteEmpresarial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDetalleClienteEmpresarial.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnDetalleClienteEmpresarial.ForeColor = System.Drawing.Color.White;
            this.btnDetalleClienteEmpresarial.Image = global::Palatium.Properties.Resources.icono_menu_detalle_empresas;
            this.btnDetalleClienteEmpresarial.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDetalleClienteEmpresarial.Location = new System.Drawing.Point(928, 220);
            this.btnDetalleClienteEmpresarial.Name = "btnDetalleClienteEmpresarial";
            this.btnDetalleClienteEmpresarial.Size = new System.Drawing.Size(192, 138);
            this.btnDetalleClienteEmpresarial.TabIndex = 85;
            this.btnDetalleClienteEmpresarial.Text = "Detalle\r\nCliente Empresarial";
            this.btnDetalleClienteEmpresarial.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDetalleClienteEmpresarial.UseVisualStyleBackColor = false;
            this.btnDetalleClienteEmpresarial.Click += new System.EventHandler(this.btnDetalleClienteEmpresarial_Click);
            this.btnDetalleClienteEmpresarial.MouseEnter += new System.EventHandler(this.btnDetalleClienteEmpresarial_MouseEnter);
            this.btnDetalleClienteEmpresarial.MouseLeave += new System.EventHandler(this.btnDetalleClienteEmpresarial_MouseLeave);
            // 
            // btnConsumoInterno
            // 
            this.btnConsumoInterno.BackColor = System.Drawing.Color.Navy;
            this.btnConsumoInterno.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnConsumoInterno.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConsumoInterno.FlatAppearance.BorderSize = 2;
            this.btnConsumoInterno.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConsumoInterno.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConsumoInterno.ForeColor = System.Drawing.Color.White;
            this.btnConsumoInterno.Image = global::Palatium.Properties.Resources.icono_mesa_menu_2;
            this.btnConsumoInterno.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnConsumoInterno.Location = new System.Drawing.Point(1126, 76);
            this.btnConsumoInterno.Name = "btnConsumoInterno";
            this.btnConsumoInterno.Size = new System.Drawing.Size(192, 138);
            this.btnConsumoInterno.TabIndex = 86;
            this.btnConsumoInterno.Text = "Consumo Interno\r\n ";
            this.btnConsumoInterno.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnConsumoInterno.UseVisualStyleBackColor = false;
            this.btnConsumoInterno.Click += new System.EventHandler(this.btnConsumoInterno_Click);
            this.btnConsumoInterno.MouseEnter += new System.EventHandler(this.btnMesas_MouseEnter);
            this.btnConsumoInterno.MouseLeave += new System.EventHandler(this.btnMesas_MouseLeave);
            // 
            // btnBusquedaPorIdentificacion
            // 
            this.btnBusquedaPorIdentificacion.BackColor = System.Drawing.Color.Navy;
            this.btnBusquedaPorIdentificacion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnBusquedaPorIdentificacion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBusquedaPorIdentificacion.FlatAppearance.BorderSize = 2;
            this.btnBusquedaPorIdentificacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBusquedaPorIdentificacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBusquedaPorIdentificacion.ForeColor = System.Drawing.Color.White;
            this.btnBusquedaPorIdentificacion.Image = global::Palatium.Properties.Resources.icono_boton_lector_biometrico;
            this.btnBusquedaPorIdentificacion.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBusquedaPorIdentificacion.Location = new System.Drawing.Point(730, 76);
            this.btnBusquedaPorIdentificacion.Name = "btnBusquedaPorIdentificacion";
            this.btnBusquedaPorIdentificacion.Size = new System.Drawing.Size(192, 138);
            this.btnBusquedaPorIdentificacion.TabIndex = 87;
            this.btnBusquedaPorIdentificacion.Text = "Cliente Empresarial\r\npor Identificación";
            this.btnBusquedaPorIdentificacion.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBusquedaPorIdentificacion.UseVisualStyleBackColor = false;
            this.btnBusquedaPorIdentificacion.Click += new System.EventHandler(this.btnBusquedaPorIdentificacion_Click);
            this.btnBusquedaPorIdentificacion.MouseEnter += new System.EventHandler(this.btnBusquedaPorIdentificacion_MouseEnter);
            this.btnBusquedaPorIdentificacion.MouseLeave += new System.EventHandler(this.btnBusquedaPorIdentificacion_MouseLeave);
            // 
            // lblVersionDemo
            // 
            this.lblVersionDemo.AutoSize = true;
            this.lblVersionDemo.Font = new System.Drawing.Font("Maiandra GD", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersionDemo.ForeColor = System.Drawing.Color.Red;
            this.lblVersionDemo.Location = new System.Drawing.Point(39, 30);
            this.lblVersionDemo.Name = "lblVersionDemo";
            this.lblVersionDemo.Size = new System.Drawing.Size(195, 32);
            this.lblVersionDemo.TabIndex = 88;
            this.lblVersionDemo.Text = "Versión Demo";
            // 
            // lblNombreEquipo
            // 
            this.lblNombreEquipo.AutoSize = true;
            this.lblNombreEquipo.Font = new System.Drawing.Font("Maiandra GD", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombreEquipo.Location = new System.Drawing.Point(197, 591);
            this.lblNombreEquipo.Name = "lblNombreEquipo";
            this.lblNombreEquipo.Size = new System.Drawing.Size(209, 22);
            this.lblNombreEquipo.TabIndex = 90;
            this.lblNombreEquipo.Text = "NOMBRE DEL EQUIPO";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Maiandra GD", 14.25F);
            this.label7.Location = new System.Drawing.Point(42, 591);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(149, 22);
            this.label7.TabIndex = 89;
            this.label7.Text = "Nombre Equipo:";
            // 
            // frmInicioComedores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(1360, 694);
            this.Controls.Add(this.lblNombreEquipo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblVersionDemo);
            this.Controls.Add(this.btnBusquedaPorIdentificacion);
            this.Controls.Add(this.btnConsumoInterno);
            this.Controls.Add(this.btnDetalleClienteEmpresarial);
            this.Controls.Add(this.btnGenerarFactura);
            this.Controls.Add(this.btnDatosClientes);
            this.Controls.Add(this.btnTarjetaAlmuerzo);
            this.Controls.Add(this.btnSalidaCajero);
            this.Controls.Add(this.btnMovimientoCaja);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnRevisar);
            this.Controls.Add(this.btnCobroAlmuerzos);
            this.Controls.Add(this.btnVentaExpress);
            this.Controls.Add(this.btnClienteEmpresarial);
            this.Controls.Add(this.logo);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInicioComedores";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Módulo de Comedores";
            this.Load += new System.EventHandler(this.frmInicioComedores_Load);
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox logo;
        private System.Windows.Forms.Button btnVentaExpress;
        private System.Windows.Forms.Button btnClienteEmpresarial;
        private System.Windows.Forms.Button btnCobroAlmuerzos;
        private System.Windows.Forms.Button btnSalidaCajero;
        private System.Windows.Forms.Button btnMovimientoCaja;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnRevisar;
        private System.Windows.Forms.Button btnTarjetaAlmuerzo;
        private System.Windows.Forms.Button btnDatosClientes;
        private System.Windows.Forms.Button btnGenerarFactura;
        private System.Windows.Forms.Button btnDetalleClienteEmpresarial;
        private System.Windows.Forms.Button btnConsumoInterno;
        private System.Windows.Forms.Button btnBusquedaPorIdentificacion;
        private System.Windows.Forms.Label lblVersionDemo;
        private System.Windows.Forms.Label lblNombreEquipo;
        private System.Windows.Forms.Label label7;
    }
}