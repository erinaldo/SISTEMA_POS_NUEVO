using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.Modals
{
    public partial class frmModalGrid : Form
    {
        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoOk ok;

        DataTable dtConsulta;
        DataTable dtMostrar;

        DataRow[] dr;

        int iNumeroColumna;

        int iCol_Correlativo;
        int iCol_Datos;
        int iCol_Descripcion;

        public int iId { get; set; }
        public string sDescripcion { get; set; }
        public string sDatosConsulta { get; set; }

        public frmModalGrid(DataTable dtConsulta, int iCol_Correlativo_P, int iCol_Datos_P, int iCol_Descripcion_P)
        {
            this.dtConsulta = dtConsulta;
            this.iCol_Correlativo = iCol_Correlativo_P;
            this.iCol_Datos = iCol_Datos_P;
            this.iCol_Descripcion = iCol_Descripcion_P;
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA RECUPERAR DATOS
        private void recuperarDatos()
        {
            iId = Convert.ToInt32(dgvDatos.CurrentRow.Cells[iCol_Correlativo].Value);
            sDescripcion = dgvDatos.CurrentRow.Cells[iCol_Descripcion].Value.ToString();
            sDatosConsulta = dgvDatos.CurrentRow.Cells[iCol_Datos].Value.ToString();
            this.DialogResult = DialogResult.OK;
        }

        //LLENAR GRID
        private void llenarGrid()
        {
            try
            {
                if (txtBusqueda.Text == "")
                {
                    dtMostrar = new DataTable();
                    dtMostrar.Clear();
                    dtMostrar = dtConsulta;
                    dgvDatos.DataSource = dtMostrar;
                    //lblBusqueda.Text = dgvDatos.Columns[1].HeaderText;
                    iNumeroColumna = 1;
                }

                else
                {
                    dtMostrar = new DataTable();
                    dtMostrar.Clear();
                    dtMostrar = dtConsulta;

                    string sAyuda;

                    string sTipoDatos = dtMostrar.Columns[lblBusqueda.Text].DataType.ToString();

                    if (sTipoDatos.ToUpper() == "SYSTEM.STRING")
                        sAyuda = lblBusqueda.Text + " like '%" + txtBusqueda.Text.Trim() + "%'";
                    else
                        sAyuda = lblBusqueda.Text + " = '" + txtBusqueda.Text.Trim() + "'";

                    dr = dtMostrar.Select(sAyuda);

                    if (dr.Length > 0)
                    {
                        dtMostrar = dr.CopyToDataTable();
                        dgvDatos.DataSource = dtMostrar;
                    }

                    else
                    {
                        dgvDatos.DataSource = dtMostrar;
                        ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                        ok.lblMensaje.Text = "No existen registros.";
                        ok.ShowDialog();
                    }

                    dgvDatos.ClearSelection();
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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            llenarGrid();
        }

        private void txtBusqueda_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                llenarGrid();
            }
        }

        private void frmModalGrid_Load(object sender, EventArgs e)
        {
            dtMostrar = new DataTable();
            dtMostrar.Clear();
            dtMostrar = dtConsulta;
            dgvDatos.DataSource = dtMostrar;
            lblBusqueda.Text = dgvDatos.Columns[1].HeaderText;
            iNumeroColumna = 1;
            dgvDatos.ClearSelection();

            this.ActiveControl = txtBusqueda;
        }

        private void dgvDatos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            iNumeroColumna = e.ColumnIndex;
            lblBusqueda.Text = dgvDatos.Columns[e.ColumnIndex].HeaderText;
            txtBusqueda.Focus();
            txtBusqueda.SelectionStart = txtBusqueda.Text.Trim().Length;
        }

        private void frmModalGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAcepar_Click(object sender, EventArgs e)
        {
            recuperarDatos();
        }

        private void dgvDatos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            recuperarDatos();
        }
    }
}
