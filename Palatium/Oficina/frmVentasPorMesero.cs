using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Palatium.Oficina
{
    public partial class frmVentasPorMesero : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoOk ok;

        DataTable dtConsulta;

        string sSql;

        bool bRespuesta;

        int iIdCierreCajero;

        Double dSuma;

        SqlParameter[] parametro;

        public frmVentasPorMesero(int iIdCierreCajero_P)
        {   
            this.iIdCierreCajero = iIdCierreCajero_P;
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        private void llenarGrid()
        {
            try
            {
                dgvDatos.Rows.Clear();

                dSuma = 0;

                sSql = "";
                sSql += "select mesero, " + Environment.NewLine;
                sSql += "ltrim(str(sum(cantidad * (precio_unitario - valor_dscto + valor_iva + valor_otro)), 10,2)) total" + Environment.NewLine;
                sSql += "from pos_vw_factura" + Environment.NewLine;
                sSql += "where id_pos_cierre_cajero = @id_pos_cierre_cajero" + Environment.NewLine;
                sSql += "group by mesero";

                parametro = new SqlParameter[1];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_pos_cierre_cajero";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdCierreCajero;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtConsulta, sSql, parametro);

                if (bRespuesta == false)
                {
                    txtTotal.Text = "0.00";
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    dgvDatos.Rows.Add(dtConsulta.Rows[i]["mesero"].ToString(),
                                      dtConsulta.Rows[i]["total"].ToString());

                    dSuma += Convert.ToDouble(dtConsulta.Rows[i]["total"].ToString());
                }

                txtTotal.Text = dSuma.ToString("N2");
                dgvDatos.ClearSelection();
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        #endregion

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (dgvDatos.Rows.Count == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "No hay registrios para imprimir.";
                ok.ShowDialog();
            }

            else
            {
                ReportesTextbox.frmVentasMesero mesero = new ReportesTextbox.frmVentasMesero(iIdCierreCajero);
                mesero.ShowDialog();
            }
        }

        private void frmVentasPorMesero_Load(object sender, EventArgs e)
        {
            llenarGrid();
        }

        private void frmVentasPorMesero_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }        
    }
}
