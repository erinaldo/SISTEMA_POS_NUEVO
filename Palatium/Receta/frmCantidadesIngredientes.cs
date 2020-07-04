using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.Receta
{
    public partial class frmCantidadesIngredientes : Form
    {
        Clases.ClaseValidarCaracteres caracter;

        public Decimal dbCantidadBruta;
        public Decimal dbCantidadNeta;
        Decimal dbPorcentaje;

        string sUnidad;

        public frmCantidadesIngredientes(Decimal dbCantidadBruta_P, Decimal dbCantidadNeta_P, string sUnidad_P, Decimal dbPorcentaje_P)
        {
            this.dbCantidadBruta = dbCantidadBruta_P;
            this.dbCantidadNeta = dbCantidadNeta_P;
            this.sUnidad = sUnidad_P;
            this.dbPorcentaje = dbPorcentaje_P;
            InitializeComponent();
        }

        private void frmCantidadesIngredientes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCantidadNeta_KeyPress(object sender, KeyPressEventArgs e)
        {
            caracter = new Clases.ClaseValidarCaracteres();
            caracter.soloDecimales(sender, e, 4);
        }

        private void txtCantidadBruta_KeyPress(object sender, KeyPressEventArgs e)
        {
            caracter = new Clases.ClaseValidarCaracteres();
            caracter.soloDecimales(sender, e, 4);
        }

        private void frmCantidadesIngredientes_Load(object sender, EventArgs e)
        {
            txtCantidadBruta.Text = dbCantidadBruta.ToString();
            txtCantidadNeta.Text = dbCantidadNeta.ToString();
            lblUnidad.Text = sUnidad.Trim().ToUpper();
            lblRendimiento.Text = dbPorcentaje.ToString() + " %";

            Decimal dbCantidad_R = dbCantidadBruta;
            dbCantidad_R = (dbCantidad_R * dbPorcentaje) / 100;

            txtCantidadNeta.Text = dbCantidad_R.ToString();

            this.ActiveControl = txtCantidadBruta;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            dbCantidadBruta = Convert.ToDecimal(txtCantidadBruta.Text);
            dbCantidadNeta = Convert.ToDecimal(txtCantidadNeta.Text);
            this.DialogResult = DialogResult.OK;
        }

        private void txtCantidadBruta_Leave(object sender, EventArgs e)
        {
            if (txtCantidadBruta.Text.Trim() == "")
                txtCantidadBruta.Text = "1";
            
            else if (Convert.ToDecimal(txtCantidadBruta.Text) <= 0)
                txtCantidadBruta.Text = "1";

            Decimal dbCantidad_R = Convert.ToDecimal(txtCantidadBruta.Text);
            dbCantidad_R = (dbCantidad_R * dbPorcentaje) / 100;

            txtCantidadNeta.Text = dbCantidad_R.ToString();
        }

        private void txtCantidadNeta_Leave(object sender, EventArgs e)
        {
            if (txtCantidadNeta.Text.Trim() == "")
                txtCantidadNeta.Text = "1";

            else if (Convert.ToDecimal(txtCantidadNeta.Text) == 0)
                txtCantidadNeta.Text = "1";

            Decimal dbCantidad_R = Convert.ToDecimal(txtCantidadNeta.Text);
            dbCantidad_R = (dbCantidad_R * 100) / dbPorcentaje;

            txtCantidadBruta.Text = dbCantidad_R.ToString();
        }
    }
}
