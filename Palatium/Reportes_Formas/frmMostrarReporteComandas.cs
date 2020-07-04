using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.Reportes_Formas
{
    public partial class frmMostrarReporteComandas : Form
    {
        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoOk ok;

        DataTable dtConsulta;

        int iMostrarDetalles;

        string sFechaDesde;
        string sFechaHasta;
        string sUsuario;

        Clases.ClaseExcel exportarExcel;

        DataGridView dgvExportar;
        DataGridViewTextBoxColumn nombre_producto_grid;

        public frmMostrarReporteComandas(DataTable dt, int iMostrarDetalles_P, string sFechaDesde_P, string sFechaHasta_P, string sUsuario_P)
        {
            this.dtConsulta = dt;
            this.iMostrarDetalles = iMostrarDetalles_P;
            this.sFechaDesde = sFechaDesde_P;
            this.sFechaHasta = sFechaHasta_P;
            this.sUsuario = sUsuario_P;
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid()
        {
            try
            {
                if (iMostrarDetalles == 0)
                {
                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        dgvDatos.Rows.Add(Convert.ToDateTime(dtConsulta.Rows[i]["fecha_pedido"].ToString()).ToString("dd-MM-yyyy"),
                                          Convert.ToDateTime(dtConsulta.Rows[i]["fecha_apertura_orden"].ToString()).ToString("HH:mm"),
                                          dtConsulta.Rows[i]["numero_pedido"].ToString(),
                                          dtConsulta.Rows[i]["origen"].ToString(),
                                          dtConsulta.Rows[i]["mesero"].ToString(),
                                          dtConsulta.Rows[i]["jornada"].ToString(),
                                          dtConsulta.Rows[i]["seccion"].ToString(),
                                          dtConsulta.Rows[i]["numero_mesa"].ToString(),
                                          "",
                                          dtConsulta.Rows[i]["valor"].ToString());
                    }

                    dgvDatos.Columns["nombre_producto"].Visible = false;
                }

                else
                {
                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        dgvDatos.Rows.Add(Convert.ToDateTime(dtConsulta.Rows[i]["fecha_pedido"].ToString()).ToString("dd-MM-yyyy"),
                                          Convert.ToDateTime(dtConsulta.Rows[i]["fecha_apertura_orden"].ToString()).ToString("HH:mm"),
                                          dtConsulta.Rows[i]["numero_pedido"].ToString(),
                                          dtConsulta.Rows[i]["origen"].ToString(),
                                          dtConsulta.Rows[i]["mesero"].ToString(),
                                          dtConsulta.Rows[i]["jornada"].ToString(),
                                          dtConsulta.Rows[i]["seccion"].ToString(),
                                          dtConsulta.Rows[i]["numero_mesa"].ToString(),
                                          dtConsulta.Rows[i]["nombre_producto"].ToString(),
                                          dtConsulta.Rows[i]["valor"].ToString());
                    }
                }

                lblCantidad.Text = "La consulta generó " + dtConsulta.Rows.Count.ToString() + " registros.";
                dgvDatos.ClearSelection();
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA CREAR EL GRID PARA EXPORTAR
        private bool exportarGridExcel()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                dgvExportar = new DataGridView();
                dgvExportar.AllowUserToAddRows = false;
                dgvExportar.AllowUserToDeleteRows = false;
                dgvExportar.AllowUserToResizeColumns = false;
                dgvExportar.AllowUserToResizeRows = false;
                dgvExportar.MultiSelect = false;

                DataGridViewTextBoxColumn fecha_pedido = new DataGridViewTextBoxColumn();
                fecha_pedido.HeaderText = "FECHA PEDIDO";
                fecha_pedido.Name = "fecha_pedido";

                DataGridViewTextBoxColumn fecha_apertura_orden = new DataGridViewTextBoxColumn();
                fecha_apertura_orden.HeaderText = "HORA PEDIDO";
                fecha_apertura_orden.Name = "fecha_apertura_orden";

                DataGridViewTextBoxColumn numero_pedido = new DataGridViewTextBoxColumn();
                numero_pedido.HeaderText = "N° PEDIDO";
                numero_pedido.Name = "numero_pedido";

                DataGridViewTextBoxColumn origen = new DataGridViewTextBoxColumn();
                origen.HeaderText = "TIPO DE COMANDA";
                origen.Name = "origen";

                DataGridViewTextBoxColumn mesero = new DataGridViewTextBoxColumn();
                mesero.HeaderText = "MESERO";
                mesero.Name = "mesero";

                DataGridViewTextBoxColumn jornada = new DataGridViewTextBoxColumn();
                jornada.HeaderText = "JORNADA";
                jornada.Name = "jornada";

                DataGridViewTextBoxColumn seccion = new DataGridViewTextBoxColumn();
                seccion.HeaderText = "SECCIÓN";
                seccion.Name = "seccion";

                DataGridViewTextBoxColumn numero_mesa = new DataGridViewTextBoxColumn();
                numero_mesa.HeaderText = "N° MESA";
                numero_mesa.Name = "numero_mesa";

                if (iMostrarDetalles == 1)
                {
                    nombre_producto_grid = new DataGridViewTextBoxColumn();
                    nombre_producto_grid.HeaderText = "NOMBRE DEL PRODUCTO";
                    nombre_producto_grid.Name = "nombre_producto_grid";
                }

                DataGridViewTextBoxColumn valor = new DataGridViewTextBoxColumn();
                valor.HeaderText = "SUBTOTAL";
                valor.Name = "valor";
                
                dgvExportar.Columns.Add(fecha_pedido);
                dgvExportar.Columns.Add(fecha_apertura_orden);
                dgvExportar.Columns.Add(numero_pedido);
                dgvExportar.Columns.Add(origen);
                dgvExportar.Columns.Add(mesero);
                dgvExportar.Columns.Add(jornada);
                dgvExportar.Columns.Add(seccion);
                dgvExportar.Columns.Add(numero_mesa);

                if (iMostrarDetalles == 1)
                    dgvExportar.Columns.Add(nombre_producto_grid);
                
                dgvExportar.Columns.Add(valor);

                if (iMostrarDetalles == 0)
                {
                    for (int i = 0; i < dgvDatos.Rows.Count; i++)
                    {
                        dgvExportar.Rows.Add(
                                                dgvDatos.Rows[i].Cells["fecha_pedido"].Value.ToString(),
                                                dgvDatos.Rows[i].Cells["hora_pedido"].Value.ToString(),
                                                dgvDatos.Rows[i].Cells["numero_pedido"].Value.ToString(),
                                                dgvDatos.Rows[i].Cells["origen"].Value.ToString(),
                                                dgvDatos.Rows[i].Cells["mesero"].Value.ToString(),
                                                dgvDatos.Rows[i].Cells["jornada"].Value.ToString(),
                                                dgvDatos.Rows[i].Cells["seccion_mesa"].Value.ToString(),
                                                dgvDatos.Rows[i].Cells["mesa"].Value.ToString(),
                                                dgvDatos.Rows[i].Cells["total"].Value.ToString()
                                            );
                    }
                }

                else
                {
                    for (int i = 0; i < dgvDatos.Rows.Count; i++)
                    {
                        dgvExportar.Rows.Add(
                                                dgvDatos.Rows[i].Cells["fecha_pedido"].Value.ToString(),
                                                dgvDatos.Rows[i].Cells["hora_pedido"].Value.ToString(),
                                                dgvDatos.Rows[i].Cells["numero_pedido"].Value.ToString(),
                                                dgvDatos.Rows[i].Cells["origen"].Value.ToString(),
                                                dgvDatos.Rows[i].Cells["mesero"].Value.ToString(),
                                                dgvDatos.Rows[i].Cells["jornada"].Value.ToString(),
                                                dgvDatos.Rows[i].Cells["seccion_mesa"].Value.ToString(),
                                                dgvDatos.Rows[i].Cells["mesa"].Value.ToString(),
                                                dgvDatos.Rows[i].Cells["nombre_producto"].Value.ToString(),
                                                dgvDatos.Rows[i].Cells["total"].Value.ToString()
                                            );
                    }
                }

                exportarExcel = new Clases.ClaseExcel();

                if (exportarExcel.exportarExcelTexto(dgvExportar) == false)
                {
                    this.Cursor = Cursors.Default;
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Ocurrió un problema al exportar la información. Comuníquese con el administrador del sistema.";
                    ok.ShowDialog();
                    return false;
                }

                this.Cursor = Cursors.Default;

                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Archivo exportado éxitosamente.";
                ok.ShowDialog();

                return true;
            }

            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return false;
            }
        }

        #endregion

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMostrarReporteComandas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void frmMostrarReporteComandas_Load(object sender, EventArgs e)
        {
            llenarGrid();
        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.Rows.Count == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "No existen registro para visualizar en un reporte.";
                ok.ShowDialog();
                return;
            }

            Reportes_Formas.frmVistaPreviaReporteComandas vista = new Reportes_Formas.frmVistaPreviaReporteComandas(dtConsulta, sFechaDesde, sFechaHasta, sUsuario, iMostrarDetalles);
            vista.ShowDialog();
        }

        private void btnRide_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDatos.Rows.Count == 0)
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "No existen registro para exportar a PDF";
                    ok.ShowDialog();
                    return;
                }

                SaveFileDialog guardarPDF = new SaveFileDialog();
                guardarPDF.Filter = "Archivo PDF|*.pdf";
                guardarPDF.Title = "Guardar información en formato PDF";
                guardarPDF.ShowDialog();

                if (guardarPDF.FileName != "")
                {
                    this.Cursor = Cursors.WaitCursor;
                    string filename = guardarPDF.FileName;
                    dsReporte ds = new dsReporte();
                    DataTable dt = ds.Tables["dtReporteComandasConsolidado"];
                    dt.Clear();

                    dt = dtConsulta;

                    ReportViewer reporte = new ReportViewer();

                    if (iMostrarDetalles == 0)
                        reporte.LocalReport.ReportEmbeddedResource = "Palatium.Reportes.rptReporteComandasConsolidado.rdlc";
                    else
                        reporte.LocalReport.ReportEmbeddedResource = "Palatium.Reportes.rptReporteComandasDetallada.rdlc";

                    ReportParameter[] parametro = new ReportParameter[3];
                    parametro[0] = new ReportParameter("P_Fecha_Desde", Convert.ToDateTime(sFechaDesde).ToString("dd-MM-yyyy"));
                    parametro[1] = new ReportParameter("P_Fecha_Hasta", Convert.ToDateTime(sFechaHasta).ToString("dd-MM-yyyy"));
                    parametro[2] = new ReportParameter("P_Usuario", sUsuario);

                    ReportDataSource reporteSource = new ReportDataSource("dsResultado", dt);
                    reporte.LocalReport.DataSources.Clear();
                    reporte.LocalReport.DataSources.Add(reporteSource);
                    reporte.LocalReport.SetParameters(parametro);
                    reporte.LocalReport.Refresh();
                    reporte.RefreshReport();
                    File.WriteAllBytes(filename, reporte.LocalReport.Render("PDF"));
                    this.Cursor = Cursors.Default;

                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Archivo generado éxitosamente.";
                    ok.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (dgvDatos.Rows.Count == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "No existen registro para exportar a Excel";
                ok.ShowDialog();
                return;
            }

            exportarGridExcel();
        }
    }
}
