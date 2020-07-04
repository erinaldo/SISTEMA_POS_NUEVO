using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.VentanasMensajes
{
    public partial class frmMensajeNuevoOk : Form
    {
        int iCuenta;

        public frmMensajeNuevoOk(int iTiempoCerrarSegundos_P)
        {
            this.iCuenta = iTiempoCerrarSegundos_P;
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        private void contarSegundos()
        {
            lblVerMensaje.Text = "La ventana se cerrará en " + (iCuenta - 1).ToString() + " segundos.";
            iCuenta--;

            if (iCuenta == -1)
            {
                timer1.Enabled = false;
                timer1.Stop();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        #endregion

        private void frmMensajeNuevoOk_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                timer1.Enabled = false;
                timer1.Stop();
                this.Close();
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer1.Stop();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            contarSegundos();
        }

        private void btnAcepar_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer1.Stop();
            this.DialogResult = DialogResult.OK;
            this.Close(); 
        }

        private void btnCerrar_Click_1(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer1.Stop();
            this.Close();
        }

        private void frmMensajeNuevoOk_Load(object sender, EventArgs e)
        {
            lblVerMensaje.Text = "La ventana se cerrará en " + iCuenta.ToString() + " segundos.";
            timer1.Enabled = true;
            timer1.Start();
        }
    }
}
