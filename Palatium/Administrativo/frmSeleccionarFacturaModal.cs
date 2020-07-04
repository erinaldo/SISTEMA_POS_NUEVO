using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.Administrativo
{
    public partial class frmSeleccionarFacturaModal : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeNuevoOk ok;
        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;

        Clases.ClaseFunciones funciones;

        string sSql;
        string sFecha;

        public int iNumeroFactura;
        public int iIdLocalidad;

        DataTable dtConsulta;

        bool bRespuesta;

        SqlParameter[] parametro;

        public frmSeleccionarFacturaModal()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA OBTENER LA FECHA
        private void fechaSistema()
        {
            try
            {
                funciones = new Clases.ClaseFunciones();

                bRespuesta = funciones.fechaSistema();

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                sFecha = funciones.sFechaRecuperada;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();

            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE LOCALIDADES
        private void llenarComboLocalidades()
        {
            try
            {
                funciones = new Clases.ClaseFunciones();

                bRespuesta = funciones.llenarComboLocalidades();

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                dtConsulta = funciones.dtConsulta;

                cmbLocalidad.ValueMember = "id_localidad";
                cmbLocalidad.DisplayMember = "nombre_localidad";
                cmbLocalidad.DataSource = dtConsulta;

                cmbLocalidad.SelectedValue = Program.iIdLocalidad;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();

            }
        }

        //FUNCION PARA LLENAR EL DATAGRID
        private bool llenarGrid()
        {
            try
            {
                dgvDatos.Rows.Clear();

                sSql = "";
                sSql += "select * from pos_vw_obtener_comprobantes_emitidos" + Environment.NewLine;
                sSql += "where id_localidad = @id_localidad" + Environment.NewLine;
                sSql += "and fecha_factura = @fecha_factura" + Environment.NewLine;
                sSql += "order by numero_factura";

                #region PARAMETROS

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_localidad";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = Convert.ToInt32(cmbLocalidad.SelectedValue);

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@fecha_factura";
                parametro[1].SqlDbType = SqlDbType.DateTime;
                parametro[1].Value = Convert.ToDateTime(dtFecha.Text);

                #endregion

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtConsulta, sSql, parametro);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return false;
                }

                string sNumeroFactura_P;

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    sNumeroFactura_P = dtConsulta.Rows[i]["establecimiento"].ToString() + "-" + dtConsulta.Rows[i]["punto_emision"].ToString() + "-" + dtConsulta.Rows[i]["numero_factura"].ToString().PadLeft(9, '0');

                    dgvDatos.Rows.Add(dtConsulta.Rows[i]["id_factura"].ToString(),
                                      dtConsulta.Rows[i]["id_localidad"].ToString(),
                                      dtConsulta.Rows[i]["numero_factura"].ToString(),
                                      Convert.ToDateTime(dtConsulta.Rows[i]["fecha_factura"].ToString()).ToString("dd-MM-yyyy"),
                                      sNumeroFactura_P,
                                      dtConsulta.Rows[i]["cliente"].ToString());
                }

                dgvDatos.ClearSelection();
                    
                return true;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return false;
            }
        }

        #endregion

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbLocalidad.SelectedValue) == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor seleccione la localidad.";
                ok.ShowDialog();
                cmbLocalidad.Focus();
                return;
            }

            DateTime time = Convert.ToDateTime(dtFecha.Text.Trim());

            TimeSpan span = (TimeSpan)(Convert.ToDateTime(sFecha) - time);

            if (span.Days > 15)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "La consulta solo se puede efectuar hasta 15 días anteriores";
                ok.ShowDialog();
                return;
            }

            llenarGrid();
        }

        private void frmAnularClonarFactura_Load(object sender, EventArgs e)
        {
            fechaSistema();
            llenarComboLocalidades();
            dtFecha.Text = sFecha;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvDatos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                iNumeroFactura = Convert.ToInt32(dgvDatos.CurrentRow.Cells["numero_factura_secuencia"].Value);
                iIdLocalidad = Convert.ToInt32(dgvDatos.CurrentRow.Cells["id_localidad"].Value);
                this.DialogResult = DialogResult.OK;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }
    }
}
