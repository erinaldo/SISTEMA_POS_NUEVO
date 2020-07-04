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

namespace Palatium.Reportes_Formas
{
    public partial class frmReporteVentasMeseroRangoFechas : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoOk ok;

        Clases.ClaseExcel exportarExcel;

        DataGridView dgvExportar;

        string sSql;
        string sFecha;
        string sFechaDesde;
        string sFechaHasta;

        DataTable dtConsulta;
        
        bool bRespuesta;

        int iIdLocalidad;
        int iIdOrigenOrden;

        SqlParameter[] parametro;

        public frmReporteVentasMeseroRangoFechas()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //LLENAR EL COMBOBX DE LOCALIDADES
        private void llenarComboLocalidades()
        {
            try
            {
                sSql = "";
                sSql += "select id_localidad, nombre_localidad" + Environment.NewLine;
                sSql += "from tp_vw_localidades";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                DataRow row = dtConsulta.NewRow();
                row["id_localidad"] = 0;
                row["nombre_localidad"] = "Seleccione...!!!";
                dtConsulta.Rows.InsertAt(row, 0);

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

        //FUNCION PARA CONSULTAR LA FECHA DEL SISTEMA
        private void fechaSistema()
        {
            try
            {
                //EXTRAER LA FECHA DEL SISTEMA
                sSql = "";
                sSql += "select getdate() fecha";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                sFecha = Convert.ToDateTime(dtConsulta.Rows[0]["fecha"].ToString()).ToString("dd-MM-yyyy");
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //OBTENER EL ID DEL ORIGEN MESA
        private void obtenerIdOrigenMesas()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_origen_orden" + Environment.NewLine;
                sSql += "from pos_origen_orden" + Environment.NewLine;
                sSql += "where estado = @estado" + Environment.NewLine;
                sSql += "and codigo = @codigo";

                #region PARAMETROS

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@estado";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "A";

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@codigo";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = "01";

                #endregion

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtConsulta, sSql,parametro);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "No se encuentra información del origen mesas.";
                    ok.ShowDialog();
                    return;
                }

                iIdOrigenOrden = Convert.ToInt32(dtConsulta.Rows[0]["id_pos_origen_orden"].ToString());
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA LLENAR EL GRID
        private bool llenarGrid()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                dgvDatos.Rows.Clear();
                int iCantidad = 3;

                sSql = "";
                sSql += "select id_pos_mesero, descripcion," + Environment.NewLine;
                sSql += "ltrim(str(sum(cantidad * (precio_unitario - valor_dscto)), 15, 2)) ventas" + Environment.NewLine;
                sSql += "from pos_vw_reporte_ventas_para_meseros" + Environment.NewLine;
                sSql += "where fecha_pedido between @fecha_desde" + Environment.NewLine;
                sSql += "and @fecha_hasta" + Environment.NewLine;
                sSql += "and id_pos_origen_orden = @id_pos_origen_orden" + Environment.NewLine;

                if (iIdLocalidad != 0)
                {
                    iCantidad++;
                    sSql += "and id_localidad = @id_localidad" + Environment.NewLine;
                }

                sSql += "group by id_pos_mesero, descripcion" + Environment.NewLine;
                sSql += "order by descripcion";

                #region PARAMETROS

                parametro = new SqlParameter[iCantidad];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@fecha_desde";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = sFechaDesde;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@fecha_hasta";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = sFechaHasta;

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@id_pos_origen_orden";
                parametro[2].SqlDbType = SqlDbType.Int;
                parametro[2].Value = iIdOrigenOrden;

                if (iCantidad == 4)
                {
                    parametro[3] = new SqlParameter();
                    parametro[3].ParameterName = "@id_localidad";
                    parametro[3].SqlDbType = SqlDbType.Int;
                    parametro[3].Value = iIdLocalidad;
                }

                #endregion

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtConsulta, sSql, parametro);

                if (bRespuesta == false)
                {
                    this.Cursor = Cursors.Default;
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return false;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    btnExportar.Visible = false;
                    this.Cursor = Cursors.Default;
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "No se encuentra información con los parámetros seleccionados.";
                    ok.ShowDialog();
                    return false;
                }

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    dgvDatos.Rows.Add(dtConsulta.Rows[i]["id_pos_mesero"].ToString(),
                                      dtConsulta.Rows[i]["descripcion"].ToString(),
                                      dtConsulta.Rows[i]["ventas"].ToString(),
                                      "0", "0", "0");
                }

                if (completarInformacionGrid() == false)
                {
                    this.Cursor = Cursors.Default;
                    return false;
                }

                dgvDatos.ClearSelection();
                btnExportar.Visible = true;
                this.Cursor = Cursors.Default;
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

        private bool completarInformacionGrid()
        {
            try
            {
                int iCantidad;
                int a;
                int iCantidadMesas;
                int iDiasTrabajados;
                int iIdMesero;
                Decimal dbVentas;
                Decimal dbPromedio;

                for (int i = 0; i < dgvDatos.Rows.Count; i++)
                {
                    iIdMesero = Convert.ToInt32(dgvDatos.Rows[i].Cells["id_pos_mesero"].Value);
                    dbVentas = Convert.ToDecimal(dgvDatos.Rows[i].Cells["ventas"].Value);
                    iCantidad = 5;

                    sSql = "";
                    sSql += "select count(*) cuenta" + Environment.NewLine;
                    sSql += "from cv403_cab_pedidos" + Environment.NewLine;
                    sSql += "where fecha_pedido between @fecha_desde" + Environment.NewLine;
                    sSql += "and @fecha_hasta" + Environment.NewLine;
                    sSql += "and id_pos_origen_orden = @id_pos_origen_orden" + Environment.NewLine;
                    sSql += "and estado = @estado" + Environment.NewLine;
                    sSql += "and id_pos_mesero = @id_pos_mesero" + Environment.NewLine;

                    if (iIdLocalidad != 0)
                    {
                        iCantidad++;
                        sSql += "and id_localidad = @id_localidad" + Environment.NewLine;
                    }

                    #region PARAMETROS

                    parametro = new SqlParameter[iCantidad];
                    parametro[0] = new SqlParameter();
                    parametro[0].ParameterName = "@fecha_desde";
                    parametro[0].SqlDbType = SqlDbType.VarChar;
                    parametro[0].Value = sFechaDesde;

                    parametro[1] = new SqlParameter();
                    parametro[1].ParameterName = "@fecha_hasta";
                    parametro[1].SqlDbType = SqlDbType.VarChar;
                    parametro[1].Value = sFechaHasta;

                    parametro[2] = new SqlParameter();
                    parametro[2].ParameterName = "@id_pos_origen_orden";
                    parametro[2].SqlDbType = SqlDbType.Int;
                    parametro[2].Value = iIdOrigenOrden;

                    parametro[3] = new SqlParameter();
                    parametro[3].ParameterName = "@estado";
                    parametro[3].SqlDbType = SqlDbType.VarChar;
                    parametro[3].Value = "A";

                    parametro[4] = new SqlParameter();
                    parametro[4].ParameterName = "@id_pos_mesero";
                    parametro[4].SqlDbType = SqlDbType.Int;
                    parametro[4].Value = iIdMesero;

                    if (iCantidad == 6)
                    {
                        parametro[5] = new SqlParameter();
                        parametro[5].ParameterName = "@id_localidad";
                        parametro[5].SqlDbType = SqlDbType.Int;
                        parametro[5].Value = iIdLocalidad;
                    }

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

                    iCantidadMesas = Convert.ToInt32(dtConsulta.Rows[0]["cuenta"].ToString());

                    iCantidad = 4;

                    sSql = "";
                    sSql += "select count(*) cuenta, fecha_pedido" + Environment.NewLine;
                    sSql += "from cv403_cab_pedidos" + Environment.NewLine;
                    sSql += "where fecha_pedido between @fecha_desde" + Environment.NewLine;
                    sSql += "and @fecha_hasta" + Environment.NewLine;
                    sSql += "and estado = @estado" + Environment.NewLine;
                    sSql += "and id_pos_mesero = @id_pos_mesero" + Environment.NewLine;

                    if (iIdLocalidad != 0)
                    {
                        iCantidad++;
                        sSql += "and id_localidad = @id_localidad" + Environment.NewLine;
                    }

                    sSql += "group by fecha_pedido";

                    #region PARAMETROS

                    parametro = new SqlParameter[iCantidad];
                    parametro[0] = new SqlParameter();
                    parametro[0].ParameterName = "@fecha_desde";
                    parametro[0].SqlDbType = SqlDbType.VarChar;
                    parametro[0].Value = sFechaDesde;

                    parametro[1] = new SqlParameter();
                    parametro[1].ParameterName = "@fecha_hasta";
                    parametro[1].SqlDbType = SqlDbType.VarChar;
                    parametro[1].Value = sFechaHasta;

                    parametro[2] = new SqlParameter();
                    parametro[2].ParameterName = "@estado";
                    parametro[2].SqlDbType = SqlDbType.VarChar;
                    parametro[2].Value = "A";

                    parametro[3] = new SqlParameter();
                    parametro[3].ParameterName = "@id_pos_mesero";
                    parametro[3].SqlDbType = SqlDbType.Int;
                    parametro[3].Value = iIdMesero;

                    if (iCantidad == 5)
                    {
                        parametro[4] = new SqlParameter();
                        parametro[4].ParameterName = "@id_localidad";
                        parametro[4].SqlDbType = SqlDbType.Int;
                        parametro[4].Value = iIdLocalidad;
                    }

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

                    iDiasTrabajados = dtConsulta.Rows.Count;

                    if (iCantidadMesas == 0)
                        dbPromedio = 0;
                    else
                        dbPromedio = dbVentas / iCantidadMesas;

                    dgvDatos.Rows[i].Cells["mesas_atendidas"].Value = iCantidadMesas.ToString();
                    dgvDatos.Rows[i].Cells["cheque_promedio"].Value = dbPromedio.ToString("N2");
                    dgvDatos.Rows[i].Cells["dias_trabajados"].Value = iDiasTrabajados.ToString();
                }

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

                DataGridViewTextBoxColumn mesero = new DataGridViewTextBoxColumn();
                mesero.HeaderText = "MESERO";
                mesero.Name = "mesero";

                DataGridViewTextBoxColumn ventas = new DataGridViewTextBoxColumn();
                ventas.HeaderText = "VENTAS";
                ventas.Name = "ventas";

                DataGridViewTextBoxColumn mesas_atendidas = new DataGridViewTextBoxColumn();
                mesas_atendidas.HeaderText = "MESAS ATENDIDAS";
                mesas_atendidas.Name = "mesas_atendidas";

                DataGridViewTextBoxColumn cheque_promedio = new DataGridViewTextBoxColumn();
                cheque_promedio.HeaderText = "CHEQUE PROMEDIO";
                cheque_promedio.Name = "cheque_promedio";

                DataGridViewTextBoxColumn dias_trabajados = new DataGridViewTextBoxColumn();
                dias_trabajados.HeaderText = "DÍAS TRABAJADOS";
                dias_trabajados.Name = "dias_trabajados";


                dgvExportar.Columns.Add(mesero);
                dgvExportar.Columns.Add(ventas);
                dgvExportar.Columns.Add(mesas_atendidas);
                dgvExportar.Columns.Add(cheque_promedio);
                dgvExportar.Columns.Add(dias_trabajados);

                for (int i = 0; i < dgvDatos.Rows.Count; i++)
                {
                    dgvExportar.Rows.Add(
                                            dgvDatos.Rows[i].Cells["mesero"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["ventas"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["mesas_atendidas"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["cheque_promedio"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["dias_trabajados"].Value.ToString()
                                        );
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

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            fechaSistema();
            llenarComboLocalidades();
            obtenerIdOrigenMesas();
            dtFechaDesde.Text = sFecha;
            dtFechaHasta.Text = sFecha;
            btnExportar.Visible = false;
            dgvDatos.Rows.Clear();
        }

        #endregion

        private void frmReporteVentasMeseroRangoFechas_Load(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            if (Convert.ToDateTime(dtFechaDesde.Text) > Convert.ToDateTime(dtFechaHasta.Text))
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "La fecha final no debe ser superior a la fecha inicial.";
                ok.ShowDialog();
                dtFechaHasta.Text = sFecha;
                return;
            }

            sFechaDesde = Convert.ToDateTime(dtFechaDesde.Text).ToString("yyyy-MM-dd");
            sFechaHasta = Convert.ToDateTime(dtFechaHasta.Text).ToString("yyyy-MM-dd");
            iIdLocalidad = Convert.ToInt32(cmbLocalidad.SelectedValue);

            llenarGrid();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.Rows.Count == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "No existe información para exportar.";
                ok.ShowDialog();
                return;
            }

            exportarGridExcel();
        }

        private void frmReporteVentasMeseroRangoFechas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
    }
}
