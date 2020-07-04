using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.Reportes_Formas
{
    public partial class frmVistaPreviaReporteComandas : Form
    {
        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoOk ok;

        DataTable dtConsulta;

        string sFechaDesde;
        string sFechaHasta;
        string sUsuario;

        int iMostraDetalle;

        public frmVistaPreviaReporteComandas(DataTable dt, string sFechaDesde_P, string sFechaHasta_P, string sUsuario_P, int iMostraDetalle_P)
        {
            this.dtConsulta = dt;
            this.sFechaDesde = Convert.ToDateTime(sFechaDesde_P).ToString("dd-MM-yyyy");
            this.sFechaHasta = Convert.ToDateTime(sFechaHasta_P).ToString("dd-MM-yyyy");
            this.sUsuario = sUsuario_P;
            this.iMostraDetalle = iMostraDetalle_P;
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA CREAR EL REPORTE
        private void crearReporte()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                dsReporte ds = new dsReporte();
                DataTable dt = ds.Tables["dtReporteComandasConsolidado"];
                dt.Clear();

                dt = dtConsulta;

                if (iMostraDetalle == 0)
                    rptVisor.LocalReport.ReportEmbeddedResource = "Palatium.Reportes.rptReporteComandasConsolidado.rdlc";
                else
                    rptVisor.LocalReport.ReportEmbeddedResource = "Palatium.Reportes.rptReporteComandasDetallada.rdlc";

                ReportParameter[] parametro = new ReportParameter[3];
                parametro[0] = new ReportParameter("P_Fecha_Desde", sFechaDesde);
                parametro[1] = new ReportParameter("P_Fecha_Hasta", sFechaHasta);
                parametro[2] = new ReportParameter("P_Usuario", sUsuario);

                ReportDataSource reporte = new ReportDataSource("dsResultado", dt);
                rptVisor.LocalReport.DataSources.Clear();
                rptVisor.LocalReport.DataSources.Add(reporte);
                rptVisor.LocalReport.SetParameters(parametro);
                rptVisor.LocalReport.Refresh();
                rptVisor.RefreshReport();
                this.Cursor = Cursors.Default;
            }

            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        #endregion

        private void frmVistaPreviaReporteComandas_Load(object sender, EventArgs e)
        {
            crearReporte();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
