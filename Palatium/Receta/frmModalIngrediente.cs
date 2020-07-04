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
    public partial class frmModalIngrediente : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeNuevoOk ok;
        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;

        string sSql;        

        bool bRespuesta;

        DataTable dtConsulta;

        //VARIABLES PUBLICAS
        public int iIdProducto;
        public int iIdUnidad;
        public string sNombreProducto;
        public string sUnidadConsumo;
        public Decimal dbPresentacion;
        public Decimal dbRendimiento;
        public Decimal dbValorUnitario;

        public frmModalIngrediente()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA CARGAR EL GRID
        private void llenarGrid()
        {
            try
            {
                sSql = "";
                sSql += "select * from pos_vw_insumo_receta" + Environment.NewLine;

                if (txtBusqueda.Text.Trim() != "")
                {
                    sSql += "where (codigo like '%" + txtBusqueda.Text.Trim() + "%'" + Environment.NewLine;
                    sSql += "or Nombre like '%" + txtBusqueda.Text.Trim() + "%')" + Environment.NewLine;
                }

                sSql += "order by nombre" + Environment.NewLine;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    dgvDatos.DataSource = dtConsulta;
                    dgvDatos.Columns[0].Visible = false;
                    dgvDatos.Columns[4].Visible = false;
                }

                else
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA ENVIAR LOS DATOS
        private void enviarDatos()
        {
            try
            {
                iIdProducto = Convert.ToInt32(dgvDatos.CurrentRow.Cells[0].Value);
                sNombreProducto = dgvDatos.CurrentRow.Cells[1].Value.ToString().Trim().ToUpper();
                dbPresentacion = Convert.ToDecimal(dgvDatos.CurrentRow.Cells[2].Value);
                dbRendimiento = Convert.ToDecimal(dgvDatos.CurrentRow.Cells[3].Value);
                iIdUnidad = Convert.ToInt32(dgvDatos.CurrentRow.Cells[4].Value);
                sUnidadConsumo = dgvDatos.CurrentRow.Cells[7].Value.ToString().Trim().ToUpper();
                dbValorUnitario = Convert.ToDecimal(dgvDatos.CurrentRow.Cells[6].Value);

                this.DialogResult = DialogResult.OK;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        #endregion

        private void frmModalIngrediente_Load(object sender, EventArgs e)
        {
            llenarGrid();
        }

        private void btnCerrarInformeVentas_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.Rows.Count == 0)
            {
                this.Close();
                return;
            }

            if (dgvDatos.SelectedRows.Count > 0)
            {
                enviarDatos();
            }

            else
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "No ha seleccionado ningún registro";
                ok.ShowDialog();
            }
        }

        private void dgvDatos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDatos.Rows.Count == 0)
            {
                this.Close();
                return;
            }

            else
            {
                enviarDatos();
            }
        }

        private void txtBusqueda_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                llenarGrid();
            }
        }

        private void frmModalIngrediente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
