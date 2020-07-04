using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.Cancelar_Orden
{
    public partial class frmMotivoEliminacionComanda : Form
    {
        VentanasMensajes.frmMensajeNuevoOk ok;
        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoSiNoCancelar SiNo;

        public int iBanderaEliminarBodega;
        int iPeticioComanda;

        public frmMotivoEliminacionComanda(int iPeticionComanda_P)
        {
            this.iPeticioComanda = iPeticionComanda_P;
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //INGRESAR EL CURSOR AL BOTON
        private void ingresaBoton(Button btnProceso)
        {
            btnProceso.ForeColor = Color.Black;
            btnProceso.BackColor = Color.LawnGreen;
        }

        //SALIR EL CURSOR DEL BOTON
        private void salidaBoton(Button btnProceso)
        {
            btnProceso.ForeColor = Color.White;
            btnProceso.BackColor = Color.Navy;
        }

        //VALIDAR LA ANULACION
        private void validarAnulacion()
        {
            try
            {
                if (txtMotivo.Text == "")
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Debe ingresar un motivo de la cancelación del pedido.";
                    ok.ShowDialog();
                    txtMotivo.Focus();
                }

                else
                {
                    if (iPeticioComanda == 1)
                    {
                        iBanderaEliminarBodega = 0;

                        if ((Program.iDescargarReceta == 1) || (Program.iDescargarProductosNoProcesados == 1))
                        {
                            SiNo = new VentanasMensajes.frmMensajeNuevoSiNoCancelar();
                            SiNo.lblMensaje.Text = "¿Desea eliminar los movimientos de bodega realizados en la comanda?";
                            SiNo.ShowDialog();

                            if (SiNo.DialogResult == DialogResult.Yes)
                            {
                                iBanderaEliminarBodega = 1;
                            }

                            else if (SiNo.DialogResult == DialogResult.No)
                            {
                                iBanderaEliminarBodega = 0;
                            }

                            else
                                return;
                        }
                    }

                    this.DialogResult = DialogResult.OK;
                }
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        #endregion

        private void btnAceptar_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnAceptar);
        }

        private void btnAceptar_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnAceptar);
        }

        private void btnCancelar_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnCancelar);
        }

        private void btnCancelar_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnCancelar);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            validarAnulacion();
        }

        private void txtMotivo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                validarAnulacion();
            }
        }

        private void frmMotivoEliminacionComanda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void frmMotivoEliminacionComanda_Load(object sender, EventArgs e)
        {

        }
    }
}
