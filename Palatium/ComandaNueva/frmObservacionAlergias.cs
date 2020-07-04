using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.ComandaNueva
{
    public partial class frmObservacionAlergias : Form
    {
        public string sObservaciones;
        public string sAlergias;

        public frmObservacionAlergias(string sObservaciones_P, string sAlergias_P)
        {
            this.sObservaciones = sObservaciones_P;
            this.sAlergias = sAlergias_P;
            InitializeComponent();
        }

        private void frmObservacionAlergias_Load(object sender, EventArgs e)
        {
            txtObservaciones.Text = sObservaciones;
            txtAlergias.Text = sAlergias;
            this.ActiveControl = txtObservaciones;
            txtObservaciones.SelectionStart = txtObservaciones.Text.Trim().Length;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmObservacionAlergias_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            sObservaciones = txtObservaciones.Text.Trim();
            sAlergias = txtAlergias.Text.Trim();
            this.DialogResult = DialogResult.OK;
        }
    }
}
