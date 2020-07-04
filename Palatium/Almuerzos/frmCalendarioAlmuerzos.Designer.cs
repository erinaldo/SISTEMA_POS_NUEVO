namespace Palatium.Almuerzos
{
    partial class frmCalendarioAlmuerzos
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
            this.Panel1 = new System.Windows.Forms.Panel();
            this.lblMonthAndYear = new System.Windows.Forms.Label();
            this.Panel2 = new System.Windows.Forms.Panel();
            this.btnHoy = new System.Windows.Forms.Button();
            this.btnSiguienteMes = new System.Windows.Forms.Button();
            this.btnAnteriorMes = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbLocalidad = new System.Windows.Forms.ComboBox();
            this.Panel3 = new System.Windows.Forms.Panel();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.flDias = new System.Windows.Forms.FlowLayoutPanel();
            this.ttMensaje = new System.Windows.Forms.ToolTip(this.components);
            this.Panel1.SuspendLayout();
            this.Panel2.SuspendLayout();
            this.Panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel1
            // 
            this.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Panel1.Controls.Add(this.lblMonthAndYear);
            this.Panel1.Controls.Add(this.Panel2);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel1.Location = new System.Drawing.Point(0, 0);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(940, 72);
            this.Panel1.TabIndex = 4;
            // 
            // lblMonthAndYear
            // 
            this.lblMonthAndYear.AutoSize = true;
            this.lblMonthAndYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonthAndYear.Location = new System.Drawing.Point(12, 18);
            this.lblMonthAndYear.Name = "lblMonthAndYear";
            this.lblMonthAndYear.Size = new System.Drawing.Size(256, 42);
            this.lblMonthAndYear.TabIndex = 1;
            this.lblMonthAndYear.Text = "January, 2018";
            // 
            // Panel2
            // 
            this.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Panel2.Controls.Add(this.btnHoy);
            this.Panel2.Controls.Add(this.btnSiguienteMes);
            this.Panel2.Controls.Add(this.btnAnteriorMes);
            this.Panel2.Controls.Add(this.label8);
            this.Panel2.Controls.Add(this.cmbLocalidad);
            this.Panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.Panel2.Location = new System.Drawing.Point(587, 0);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(353, 72);
            this.Panel2.TabIndex = 0;
            // 
            // btnHoy
            // 
            this.btnHoy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.btnHoy.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Fuchsia;
            this.btnHoy.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LimeGreen;
            this.btnHoy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHoy.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHoy.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnHoy.Location = new System.Drawing.Point(158, 39);
            this.btnHoy.Name = "btnHoy";
            this.btnHoy.Size = new System.Drawing.Size(118, 30);
            this.btnHoy.TabIndex = 222;
            this.btnHoy.Text = "Fecha actual";
            this.ttMensaje.SetToolTip(this.btnHoy, "Clic aquí para cargar con la fecha actual");
            this.btnHoy.UseVisualStyleBackColor = false;
            this.btnHoy.Click += new System.EventHandler(this.btnHoy_Click);
            // 
            // btnSiguienteMes
            // 
            this.btnSiguienteMes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.btnSiguienteMes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Fuchsia;
            this.btnSiguienteMes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LimeGreen;
            this.btnSiguienteMes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSiguienteMes.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold);
            this.btnSiguienteMes.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnSiguienteMes.Location = new System.Drawing.Point(282, 39);
            this.btnSiguienteMes.Name = "btnSiguienteMes";
            this.btnSiguienteMes.Size = new System.Drawing.Size(40, 30);
            this.btnSiguienteMes.TabIndex = 221;
            this.btnSiguienteMes.Text = ">>";
            this.ttMensaje.SetToolTip(this.btnSiguienteMes, "Clic aquí para cargar información del siguiente  mes");
            this.btnSiguienteMes.UseVisualStyleBackColor = false;
            this.btnSiguienteMes.Click += new System.EventHandler(this.btnSiguienteMes_Click);
            // 
            // btnAnteriorMes
            // 
            this.btnAnteriorMes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.btnAnteriorMes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Fuchsia;
            this.btnAnteriorMes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LimeGreen;
            this.btnAnteriorMes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnteriorMes.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold);
            this.btnAnteriorMes.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnAnteriorMes.Location = new System.Drawing.Point(112, 39);
            this.btnAnteriorMes.Name = "btnAnteriorMes";
            this.btnAnteriorMes.Size = new System.Drawing.Size(40, 30);
            this.btnAnteriorMes.TabIndex = 220;
            this.btnAnteriorMes.Text = "<<";
            this.ttMensaje.SetToolTip(this.btnAnteriorMes, "Clic aquí para cargar información del mes anterior");
            this.btnAnteriorMes.UseVisualStyleBackColor = false;
            this.btnAnteriorMes.Click += new System.EventHandler(this.btnAnteriorMes_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(12, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 18);
            this.label8.TabIndex = 4;
            this.label8.Text = "Localidad:";
            // 
            // cmbLocalidad
            // 
            this.cmbLocalidad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocalidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbLocalidad.FormattingEnabled = true;
            this.cmbLocalidad.Location = new System.Drawing.Point(93, 5);
            this.cmbLocalidad.Name = "cmbLocalidad";
            this.cmbLocalidad.Size = new System.Drawing.Size(248, 24);
            this.cmbLocalidad.TabIndex = 3;
            this.cmbLocalidad.SelectedIndexChanged += new System.EventHandler(this.cmbLocalidad_SelectedIndexChanged);
            // 
            // Panel3
            // 
            this.Panel3.Controls.Add(this.Label5);
            this.Panel3.Controls.Add(this.Label6);
            this.Panel3.Controls.Add(this.Label7);
            this.Panel3.Controls.Add(this.Label4);
            this.Panel3.Controls.Add(this.Label3);
            this.Panel3.Controls.Add(this.Label2);
            this.Panel3.Controls.Add(this.Label1);
            this.Panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel3.Location = new System.Drawing.Point(0, 72);
            this.Panel3.Name = "Panel3";
            this.Panel3.Size = new System.Drawing.Size(940, 35);
            this.Panel3.TabIndex = 5;
            // 
            // Label5
            // 
            this.Label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.Label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.Location = new System.Drawing.Point(807, 3);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(128, 30);
            this.Label5.TabIndex = 6;
            this.Label5.Text = "Sábado";
            this.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label6
            // 
            this.Label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Location = new System.Drawing.Point(673, 2);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(128, 30);
            this.Label6.TabIndex = 5;
            this.Label6.Text = "Viernes";
            this.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label7
            // 
            this.Label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.Label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label7.Location = new System.Drawing.Point(539, 2);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(128, 30);
            this.Label7.TabIndex = 4;
            this.Label7.Text = "Jueves";
            this.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label4
            // 
            this.Label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.Label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.Location = new System.Drawing.Point(408, 2);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(125, 30);
            this.Label4.TabIndex = 3;
            this.Label4.Text = "Miércoles";
            this.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label3
            // 
            this.Label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(271, 2);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(128, 30);
            this.Label3.TabIndex = 2;
            this.Label3.Text = "Martes";
            this.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label2
            // 
            this.Label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(137, 2);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(127, 30);
            this.Label2.TabIndex = 1;
            this.Label2.Text = "Lunes";
            this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(-4, 2);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(135, 30);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Domingo";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flDias
            // 
            this.flDias.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.flDias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flDias.Location = new System.Drawing.Point(0, 107);
            this.flDias.Name = "flDias";
            this.flDias.Size = new System.Drawing.Size(940, 434);
            this.flDias.TabIndex = 6;
            // 
            // frmCalendarioAlmuerzos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 541);
            this.Controls.Add(this.flDias);
            this.Controls.Add(this.Panel3);
            this.Controls.Add(this.Panel1);
            this.Name = "frmCalendarioAlmuerzos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Calendarización de almuerzos";
            this.Load += new System.EventHandler(this.frmCrearCalendarizacion_Load);
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            this.Panel2.ResumeLayout(false);
            this.Panel2.PerformLayout();
            this.Panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.Label lblMonthAndYear;
        internal System.Windows.Forms.Panel Panel2;
        internal System.Windows.Forms.Panel Panel3;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.FlowLayoutPanel flDias;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbLocalidad;
        private System.Windows.Forms.Button btnHoy;
        private System.Windows.Forms.ToolTip ttMensaje;
        private System.Windows.Forms.Button btnSiguienteMes;
        private System.Windows.Forms.Button btnAnteriorMes;
    }
}