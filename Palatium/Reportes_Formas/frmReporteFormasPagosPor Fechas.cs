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
    public partial class frmReporteFormasPagosPor_Fechas : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoOk ok;

        string sSql;
        string sFecha;
        string sFechaDesde;
        string sFechaHasta;

        DataTable dtConsulta;
        DataTable dtFormasPagos;
        DataTable dtInformacion;
        DataTable dtValores;

        bool bRespuesta;

        int iIdLocalidad;

        SqlParameter[] parametro;

        public frmReporteFormasPagosPor_Fechas()
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

        //FUNCION PARA CONSULTAR LAS FORMAS DE PAGO
        private bool formasPagos()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_tipo_forma_cobro, descripcion" + Environment.NewLine;
                sSql += "from pos_tipo_forma_cobro" + Environment.NewLine;
                sSql += "where estado = @estado" + Environment.NewLine;
                sSql += "and is_active = @is_active";

                #region PARAMETROS

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@estado";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "A";

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@is_active";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = 1;

                #endregion

                dtFormasPagos = new DataTable();
                dtFormasPagos.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtFormasPagos, sSql, parametro);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return false;
                }

                if (dtFormasPagos.Rows.Count == 0)
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "No se encuentra información de las formas de pago.";
                    ok.ShowDialog();
                    return false;
                }

                dtInformacion = new DataTable();
                dtInformacion.Clear();

                dtInformacion.Columns.Add("FECHA", typeof(string));

                for (int i = 0; i < dtFormasPagos.Rows.Count; i++)
                {
                    dtInformacion.Columns.Add(dtFormasPagos.Rows[i]["id_pos_tipo_forma_cobro"].ToString(), typeof(Int32));
                    dtInformacion.Columns.Add(dtFormasPagos.Rows[i]["descripcion"].ToString(), typeof(string));
                }

                dtInformacion.Columns.Add("TOTAL", typeof(string));

                if (cargarFechas() == false)
                {
                    return false;
                }

                if (llenarInformacionValores() == false)
                {
                    return false;
                }

                if (eliminarColumnasDataTable() == false)
                {
                    return false;
                }

                dgvDatos.DataSource = dtInformacion;

                if (formatearGrid() == false)
                {
                    return false;
                }

                if (obtenerTotalesDataTable() == false)
                {
                    return false;
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

        //FUNCION PARA LLENAR LAS FECHAS DE CONSULTA
        private bool cargarFechas()
        {
            try
            {
                int iCantidad = 3;
                
                sSql = "";
                sSql += "select CP.fecha_pedido" + Environment.NewLine;
                sSql += "from pos_vw_pedido_forma_pago FC INNER JOIN" + Environment.NewLine;
                sSql += "cv403_cab_pedidos CP ON CP.id_pedido = FC.id_pedido" + Environment.NewLine;
                sSql += "and CP.estado = @estado" + Environment.NewLine;
                sSql += "where CP.fecha_pedido between @fecha_desde" + Environment.NewLine;
                sSql += "and @fecha_hasta" + Environment.NewLine;

                if (iIdLocalidad != 0)
                {
                    iCantidad++;
                    sSql += "and CP.id_localidad = @id_localidad" + Environment.NewLine;
                }

                sSql += "group by CP.fecha_pedido" + Environment.NewLine;
                sSql += "order by CP.fecha_pedido";

                #region PARAMETROS

                parametro = new SqlParameter[iCantidad];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@estado";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "A";

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@fecha_desde";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = sFechaDesde;

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@fecha_hasta";
                parametro[2].SqlDbType = SqlDbType.VarChar;
                parametro[2].Value = sFechaHasta;

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
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return false;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "No se encuentra información con los parámetros seleccionados.";
                    ok.ShowDialog();
                    return false;
                }

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    DataRow row = dtInformacion.NewRow();

                    row[0] = dtConsulta.Rows[i]["fecha_pedido"].ToString();

                    for (int j = 1; j < dtInformacion.Columns.Count; j++)
                    {
                        row[j] = "0";
                    }

                    dtInformacion.Rows.Add(row);
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

        //FUNCION PARA LLENAR CON DATOS
        private bool llenarInformacionValores()
        {
            try
            {
                int iCantidad = 3;

                sSql = "";
                sSql += "select CP.fecha_pedido, FC.id_pos_tipo_forma_cobro, descripcion, " + Environment.NewLine;
                sSql += "ltrim(str(isnull(sum(valor), 0), 15, 2)) valor" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos CP INNER JOIN" + Environment.NewLine;
                sSql += "pos_vw_pedido_forma_pago FC ON CP.id_pedido = FC.id_pedido" + Environment.NewLine;
                sSql += "and CP.estado = @estado" + Environment.NewLine;
                sSql += "where CP.fecha_pedido between @fecha_desde" + Environment.NewLine;
                sSql += "and @fecha_hasta" + Environment.NewLine;

                if (iIdLocalidad != 0)
                {
                    iCantidad++;
                    sSql += "and CP.id_localidad = @id_localidad" + Environment.NewLine;
                }

                sSql += "group by CP.fecha_pedido, FC.id_pos_tipo_forma_cobro, descripcion" + Environment.NewLine;
                sSql += "order by CP.fecha_pedido, FC.id_pos_tipo_forma_cobro";

                #region PARAMETROS

                parametro = new SqlParameter[iCantidad];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@estado";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "A";

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@fecha_desde";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = sFechaDesde;

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@fecha_hasta";
                parametro[2].SqlDbType = SqlDbType.VarChar;
                parametro[2].Value = sFechaHasta;

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
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return false;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "No se encuentra información con los parámetros seleccionados.";
                    ok.ShowDialog();
                    return false;
                }

                int iValorFilaRecorrer = 0;
                int iValorColumnaRecorrer;
                Decimal dbSumaTotalRecorrer;

                for (int i = 0; i < dtInformacion.Rows.Count; i++)
                {
                    DateTime dtFecha = Convert.ToDateTime(dtInformacion.Rows[i][0].ToString());
                    iValorColumnaRecorrer = 1;
                    dbSumaTotalRecorrer = 0;

                    for (int j = iValorFilaRecorrer; j < dtConsulta.Rows.Count; j++)
                    {
                        DateTime dtFechaRecorrida = Convert.ToDateTime(dtConsulta.Rows[j]["fecha_pedido"].ToString());
                        int iIdPedidoFormaCobro = Convert.ToInt32(dtConsulta.Rows[j]["id_pos_tipo_forma_cobro"].ToString());
                        Decimal dbValorRecorrido = Convert.ToDecimal(dtConsulta.Rows[j]["valor"].ToString());
                        dbSumaTotalRecorrer += dbValorRecorrido;

                        for (int k = iValorColumnaRecorrer; k < dtInformacion.Columns.Count - 1; k = k + 2)
                        {
                            string sNombreColumnaId = dtInformacion.Columns[k].ColumnName;

                            if (sNombreColumnaId == iIdPedidoFormaCobro.ToString())
                            {
                                //dtInformacion.Rows[i][k] = iIdPedidoFormaCobro;
                                dtInformacion.Rows[i][k + 1] = dbValorRecorrido;

                                if (j + 1 == dtConsulta.Rows.Count)
                                    dtInformacion.Rows[i][dtInformacion.Columns.Count - 1] = dbSumaTotalRecorrer;

                                iValorColumnaRecorrer = k + 2;
                                break;
                            }                        
                        }

                        if (dtFecha != dtFechaRecorrida)
                        {
                            dtInformacion.Rows[i][dtInformacion.Columns.Count - 1] = dbSumaTotalRecorrer;
                            iValorFilaRecorrer = j;
                            break;
                        }
                    }                        
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

        //FUNCION PARA ELIMINAR LAS COLUMNAS INNECESARAS DEL DATATABLE DE INFORMACION
        private bool eliminarColumnasDataTable()
        {
            try 
            {
                int iNumeroColumnas = dtInformacion.Columns.Count - 3;

                for (int i = iNumeroColumnas; i >= 0; i = i - 2)
                {
                    dtInformacion.Columns.RemoveAt(i);
                }

                for (int j = 0; j < dtInformacion.Rows.Count; j++)
                {
                    string sFecha_P = Convert.ToDateTime(dtInformacion.Rows[j][0].ToString()).ToString("dd-MM-yyyy");
                    dtInformacion.Rows[j][0] = sFecha_P;
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

        //FUNCION PARA FORMATEAR EL DATAGRID
        private bool formatearGrid()
        {
            try
            {
                for (int i = 0; i < dgvDatos.Rows.Count; i++)
                {
                    for (int j = 1; j < dgvDatos.Columns.Count; j++)
                    {
                        string sValor_P = Convert.ToDecimal(dgvDatos.Rows[i].Cells[j].Value).ToString("N2");
                        dgvDatos.Rows[i].Cells[j].Value = sValor_P;
                    }

                    //if (i % 2 == 0)
                    //{
                    //    dgvDatos.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 192, 192);
                    //}

                    //else
                    //{
                    //    dgvDatos.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    //}
                }

                dgvDatos.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                for (int k = 1; k < dgvDatos.Columns.Count; k++)
                {
                    dgvDatos.Columns[k].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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

        //CLONAR LA ESTRUCTURA DEL DATATABLE
        private bool obtenerTotalesDataTable()
        {
            try
            {
                dtValores = new DataTable();
                dtValores.Clear();
                dtValores = dtInformacion.Clone();

                for (int j = 0; j < dtValores.Columns.Count; j++)
                {
                    dtValores.Columns[j].DataType = typeof(string);
                }

                DataRow row = dtValores.NewRow();
                row[0] = "TOTALES";

                for (int j = 1; j < dtValores.Columns.Count; j++)
                {
                    row[j] = "0";
                }

                dtValores.Rows.Add(row);

                dgvTotales.DataSource = dtValores;

                Double dbSumaValores;

                for (int j = 1; j < dgvDatos.Columns.Count; j++)
                {
                    dbSumaValores = 0;

                    for (int k = 0; k < dgvDatos.Rows.Count; k++)
                    {
                        Double dbValor_P = Convert.ToDouble(dgvDatos.Rows[k].Cells[j].Value);
                        dbSumaValores += dbValor_P;
                    }

                    dgvTotales.Rows[0].Cells[j].Value = dbSumaValores.ToString("N2");
                }

                dgvTotales.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                for (int k = 1; k < dgvTotales.Columns.Count; k++)
                {
                    dgvTotales.Columns[k].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                //for (int j = 1; j < dgvTotales.Columns.Count; j++)
                //{
                //    string sValor_P = Convert.ToDecimal(dgvTotales.Rows[0].Cells[j].Value).ToString("N2");
                //    dgvDatos.Rows[0].Cells[j].Value = sValor_P;
                //}

                dgvTotales.Rows[0].DefaultCellStyle.BackColor = Color.FromArgb(255, 192, 192);
                dgvTotales.ClearSelection();

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

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            fechaSistema();
            llenarComboLocalidades();
            dtFechaDesde.Text = sFecha;
            dtFechaHasta.Text = sFecha;

            dtConsulta = new DataTable();
            dtConsulta.Clear();
            dgvDatos.DataSource = dtConsulta;

            dtConsulta = new DataTable();
            dtConsulta.Clear();
            dgvTotales.DataSource = dtConsulta;
        }

        #endregion

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

            formasPagos();
        }

        private void frmReporteFormasPagosPor_Fechas_Load(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void frmReporteFormasPagosPor_Fechas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
    }
}
