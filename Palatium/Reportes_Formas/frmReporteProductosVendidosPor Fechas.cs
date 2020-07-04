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
    public partial class frmReporteProductosVendidosPor_Fechas : Form
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
        DataTable dtFormasPagos;
        DataTable dtInformacion;
        DataTable dtValores;

        bool bRespuesta;

        int iIdLocalidad;

        SqlParameter[] parametro;

        public frmReporteProductosVendidosPor_Fechas()
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

        //FUNCION PARA LLENAR EL GRID
        private bool llenarGrid()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                dgvDatos.Rows.Clear();
                int iCantidad = 2;

                sSql = "";
                sSql += "select id_producto, codigo, nombre, categoria, sum(cantidad) cantidad" + Environment.NewLine;
                //sSql += "ltrim(str(isnull(sum(valor), 0), 10, 2)) total" + Environment.NewLine;
                sSql += "from pos_vw_resumen_productos_rango_fechas" + Environment.NewLine;
                sSql += "where fecha_pedido between @fecha_desde" + Environment.NewLine;
                sSql += "and @fecha_hasta" + Environment.NewLine;

                if (iIdLocalidad != 0)
                {
                    iCantidad++;
                    sSql += "and id_localidad = @id_localidad" + Environment.NewLine;
                }

                sSql += "group by id_producto, codigo, nombre, categoria" + Environment.NewLine;
                sSql += "order by nombre";

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

                if (iCantidad == 3)
                {
                    parametro[2] = new SqlParameter();
                    parametro[2].ParameterName = "@id_localidad";
                    parametro[2].SqlDbType = SqlDbType.Int;
                    parametro[2].Value = iIdLocalidad;
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
                    dgvDatos.Rows.Add(dtConsulta.Rows[i]["id_producto"].ToString(),
                                      dtConsulta.Rows[i]["codigo"].ToString(),
                                      dtConsulta.Rows[i]["nombre"].ToString(),
                                      dtConsulta.Rows[i]["categoria"].ToString(),
                                      dtConsulta.Rows[i]["cantidad"].ToString(),
                                      "0", "0", "0", "0", "0", "0");
                }

                dgvDatos.ClearSelection();

                if (completarInformacionGrid_V2() == false)
                {
                    this.Cursor = Cursors.Default;
                    return false;
                }

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

        //FUNCION PARA RECORRER EL GRID Y COMPLETAR LA INFORMACIÓN
        private bool completarInformacionGrid()
        {
            try
            {
                int iCuenta;
                int a;

                for (int i = 0; i < dgvDatos.Rows.Count; i++)
                {
                    int iIdProducto = Convert.ToInt32(dgvDatos.Rows[i].Cells["id_producto"].Value);
                    Decimal dbCantidadProducida = Convert.ToDecimal(dgvDatos.Rows[i].Cells["cantidad_producida"].Value);
                    Decimal dbCantidadRecaudada;
                    Decimal dbCantidadPorCobrar;
                    Decimal dbCantidadDiferencia;

                    //CONSULTAR LA CANTIDAD RECAUDADA
                    iCuenta = 6;
                    a = 0;

                    sSql = "";
                    sSql += "select isnull(sum(cantidad), 0) cantidad" + Environment.NewLine;
                    sSql += "from pos_vw_resumen_productos_rango_fechas" + Environment.NewLine;
                    sSql += "where fecha_pedido between @fecha_desde" + Environment.NewLine;
                    sSql += "and @fecha_hasta" + Environment.NewLine;
                    sSql += "and id_producto = @id_producto" + Environment.NewLine;
                    sSql += "and genera_factura = @genera_factura" + Environment.NewLine;
                    sSql += "and cuenta_por_cobrar = @cuenta_por_cobrar" + Environment.NewLine;
                    sSql += "and repartidor_externo = @repartidor_externo" + Environment.NewLine;

                    if (iIdLocalidad != 0)
                    {
                        iCuenta++;
                        sSql += "and id_localidad = @id_localidad" + Environment.NewLine;
                    }

                    #region PARAMETROS

                    parametro =new SqlParameter[iCuenta];
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@fecha_desde";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = sFechaDesde;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@fecha_hasta";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = sFechaHasta;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_producto";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = iIdProducto;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@genera_factura";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = 1;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@cuenta_por_cobrar";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = 0;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@repartidor_externo";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = 0;                    

                    if (iCuenta == 7)
                    {
                        a++;
                        parametro[a] = new SqlParameter();
                        parametro[a].ParameterName = "@id_localidad";
                        parametro[a].SqlDbType = SqlDbType.Int;
                        parametro[a].Value = iIdLocalidad;
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

                    dbCantidadRecaudada = Convert.ToDecimal(dtConsulta.Rows[0]["cantidad"].ToString());

                    //CONSULTAR LA CANTIDAD POR COBRAR
                    iCuenta = 4;
                    a = 0;

                    sSql = "";
                    sSql += "select isnull(sum(cantidad), 0) cantidad" + Environment.NewLine;
                    sSql += "from pos_vw_resumen_productos_rango_fechas" + Environment.NewLine;
                    sSql += "where fecha_pedido between @fecha_desde" + Environment.NewLine;
                    sSql += "and @fecha_hasta" + Environment.NewLine;
                    sSql += "and id_producto = @id_producto" + Environment.NewLine;
                    sSql += "and cuenta_por_cobrar = @cuenta_por_cobrar" + Environment.NewLine;

                    if (iIdLocalidad != 0)
                    {
                        iCuenta++;
                        sSql += "and id_localidad = @id_localidad" + Environment.NewLine;
                    }

                    #region PARAMETROS

                    parametro = new SqlParameter[iCuenta];
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@fecha_desde";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = sFechaDesde;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@fecha_hasta";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = sFechaHasta;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_producto";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = iIdProducto;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@cuenta_por_cobrar";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = 1;

                    if (iCuenta == 5)
                    {
                        a++;
                        parametro[a] = new SqlParameter();
                        parametro[a].ParameterName = "@id_localidad";
                        parametro[a].SqlDbType = SqlDbType.Int;
                        parametro[a].Value = iIdLocalidad;
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

                    dbCantidadPorCobrar = Convert.ToDecimal(dtConsulta.Rows[0]["cantidad"].ToString());

                    dbCantidadDiferencia = dbCantidadProducida - dbCantidadRecaudada - dbCantidadPorCobrar;

                    dgvDatos.Rows[i].Cells["cantidad_recaudada"].Value = dbCantidadRecaudada;
                    dgvDatos.Rows[i].Cells["cantidad_por_cobrar"].Value = dbCantidadPorCobrar;
                    dgvDatos.Rows[i].Cells["cantidad_diferencia"].Value = dbCantidadDiferencia;
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

        //FUNCION PARA RECORRER EL GRID Y COMPLETAR LA INFORMACIÓN
        private bool completarInformacionGrid_V2()
        {
            try
            {
                int iCuenta;
                int a;

                for (int i = 0; i < dgvDatos.Rows.Count; i++)
                {
                    int iIdProducto = Convert.ToInt32(dgvDatos.Rows[i].Cells["id_producto"].Value);
                    Decimal dbCantidadProducida = Convert.ToDecimal(dgvDatos.Rows[i].Cells["cantidad_producida"].Value);
                    Decimal dbCantidadRecaudada;
                    Decimal dbCantidadPorCobrar;
                    Decimal dbCantidadPagoAnticipado;
                    Decimal dbCantidadDiferencia;
                    Decimal dbTotalRecaudado;
                    Decimal dbPrecioUnitario;
                    Decimal dbSumaCantidades;

                    //CONSULTAR LA CANTIDAD RECAUDADA
                    iCuenta = 20;

                    if (iIdLocalidad != 0)
                        iCuenta = iCuenta + 4;

                    a = 0;

                    sSql = "";
                    sSql += "select 1 'ORDEN', 0 cantidad, ltrim(str(isnull(sum(valor), 0), 10, 2)) valor" + Environment.NewLine;
                    sSql += "from pos_vw_resumen_productos_rango_fechas" + Environment.NewLine;
                    sSql += "where fecha_pedido between @fecha_desde_1" + Environment.NewLine;
                    sSql += "and @fecha_hasta_1" + Environment.NewLine;
                    sSql += "and id_producto = @id_producto_1" + Environment.NewLine;
                    sSql += "and pago_anticipado = @pago_anticipado_1" + Environment.NewLine;

                    if (iIdLocalidad != 0)
                        sSql += "and id_localidad = @id_localidad_1" + Environment.NewLine;

                    sSql += "union" + Environment.NewLine;
                    sSql += "select 2 'ORDEN', isnull(sum(cantidad), 0) cantidad, '0' valor" + Environment.NewLine;
                    sSql += "from pos_vw_resumen_productos_rango_fechas" + Environment.NewLine;
                    sSql += "where fecha_pedido between @fecha_desde_2" + Environment.NewLine;
                    sSql += "and @fecha_hasta_2" + Environment.NewLine;
                    sSql += "and id_producto = @id_producto_2" + Environment.NewLine;
                    sSql += "and genera_factura = @genera_factura_2" + Environment.NewLine;
                    sSql += "and cuenta_por_cobrar = @cuenta_por_cobrar_2" + Environment.NewLine;
                    sSql += "and repartidor_externo = @repartidor_externo_2" + Environment.NewLine;
                    sSql += "and pago_anticipado = @pago_anticipado_2" + Environment.NewLine;

                    if (iIdLocalidad != 0)
                        sSql += "and id_localidad = @id_localidad_2" + Environment.NewLine;

                    sSql += "union" + Environment.NewLine;
                    sSql += "select 3 'ORDEN', isnull(sum(cantidad), 0) cantidad, '0' valor" + Environment.NewLine;
                    sSql += "from pos_vw_resumen_productos_rango_fechas" + Environment.NewLine;
                    sSql += "where fecha_pedido between @fecha_desde_3" + Environment.NewLine;
                    sSql += "and @fecha_hasta_3" + Environment.NewLine;
                    sSql += "and id_producto = @id_producto_3" + Environment.NewLine;
                    sSql += "and cuenta_por_cobrar = @cuenta_por_cobrar_3" + Environment.NewLine;
                    sSql += "and pago_anticipado = @pago_anticipado_3" + Environment.NewLine;

                    if (iIdLocalidad != 0)
                        sSql += "and id_localidad = @id_localidad_3" + Environment.NewLine;

                    sSql += "union" + Environment.NewLine;
                    sSql += "select 4 'ORDEN', isnull(sum(cantidad), 0) cantidad, '0' valor" + Environment.NewLine;
                    sSql += "from pos_vw_resumen_productos_rango_fechas" + Environment.NewLine;
                    sSql += "where fecha_pedido between @fecha_desde_4" + Environment.NewLine;
                    sSql += "and @fecha_hasta_4" + Environment.NewLine;
                    sSql += "and id_producto = @id_producto_4" + Environment.NewLine;
                    sSql += "and pago_anticipado = @pago_anticipado_4" + Environment.NewLine;

                    if (iIdLocalidad != 0)
                        sSql += "and id_localidad = @id_localidad_4";

                    #region PARAMETROS

                    parametro = new SqlParameter[iCuenta];
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@fecha_desde_1";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = sFechaDesde;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@fecha_hasta_1";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = sFechaHasta;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_producto_1";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdProducto;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@pago_anticipado_1";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = 0;
                    a++;

                    if (iIdLocalidad != 0)
                    {
                        parametro[a] = new SqlParameter();
                        parametro[a].ParameterName = "@id_localidad_1";
                        parametro[a].SqlDbType = SqlDbType.Int;
                        parametro[a].Value = iIdLocalidad;
                        a++;
                    }

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@fecha_desde_2";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = sFechaDesde;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@fecha_hasta_2";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = sFechaHasta;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_producto_2";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdProducto;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@genera_factura_2";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = 1;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@cuenta_por_cobrar_2";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = 0;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@repartidor_externo_2";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = 0;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@pago_anticipado_2";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = 0;
                    a++;

                    if (iIdLocalidad != 0)
                    {
                        parametro[a] = new SqlParameter();
                        parametro[a].ParameterName = "@id_localidad_2";
                        parametro[a].SqlDbType = SqlDbType.Int;
                        parametro[a].Value = iIdLocalidad;
                        a++;
                    }

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@fecha_desde_3";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = sFechaDesde;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@fecha_hasta_3";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = sFechaHasta;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_producto_3";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdProducto;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@cuenta_por_cobrar_3";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = 1;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@pago_anticipado_3";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = 0;
                    a++;

                    if (iIdLocalidad != 0)
                    {
                        parametro[a] = new SqlParameter();
                        parametro[a].ParameterName = "@id_localidad_3";
                        parametro[a].SqlDbType = SqlDbType.Int;
                        parametro[a].Value = iIdLocalidad;
                        a++;
                    }

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@fecha_desde_4";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = sFechaDesde;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@fecha_hasta_4";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = sFechaHasta;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_producto_4";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdProducto;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@pago_anticipado_4";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = 1;
                    a++;

                    if (iIdLocalidad != 0)
                    {
                        parametro[a] = new SqlParameter();
                        parametro[a].ParameterName = "@id_localidad_4";
                        parametro[a].SqlDbType = SqlDbType.Int;
                        parametro[a].Value = iIdLocalidad;
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

                    dbTotalRecaudado = Convert.ToDecimal(dtConsulta.Rows[0]["valor"].ToString());
                    dbCantidadRecaudada = Convert.ToDecimal(dtConsulta.Rows[1]["cantidad"].ToString());
                    dbCantidadPorCobrar = Convert.ToDecimal(dtConsulta.Rows[2]["cantidad"].ToString());
                    dbCantidadPagoAnticipado = Convert.ToDecimal(dtConsulta.Rows[3]["cantidad"].ToString());

                    //dbSumaCantidades = dbCantidadRecaudada + dbCantidadPorCobrar;
                    dbPrecioUnitario = dbTotalRecaudado / dbCantidadProducida;

                    dbCantidadDiferencia = dbCantidadProducida - dbCantidadRecaudada - dbCantidadPorCobrar - dbCantidadPagoAnticipado;

                    //if (dbSumaCantidades == 0)
                    //    dbPrecioUnitario = 0;
                    //else
                    //    dbPrecioUnitario = dbTotalRecaudado / dbCantidadRecaudada;

                    dgvDatos.Rows[i].Cells["precio_unitario"].Value = dbPrecioUnitario.ToString("N2");
                    dgvDatos.Rows[i].Cells["cantidad_recaudada"].Value = dbCantidadRecaudada;
                    dgvDatos.Rows[i].Cells["cantidad_por_cobrar"].Value = dbCantidadPorCobrar;
                    dgvDatos.Rows[i].Cells["cantidad_prepagada"].Value = dbCantidadPagoAnticipado;
                    dgvDatos.Rows[i].Cells["cantidad_diferencia"].Value = dbCantidadDiferencia;
                    dgvDatos.Rows[i].Cells["total"].Value = dtConsulta.Rows[0]["valor"].ToString();
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

                DataGridViewTextBoxColumn codigo = new DataGridViewTextBoxColumn();
                codigo.HeaderText = "CÓDIGO";
                codigo.Name = "codigo";

                DataGridViewTextBoxColumn nombre = new DataGridViewTextBoxColumn();
                nombre.HeaderText = "PRODUCTO";
                nombre.Name = "nombre";

                DataGridViewTextBoxColumn categoria = new DataGridViewTextBoxColumn();
                categoria.HeaderText = "CATEGORÍA";
                categoria.Name = "categoria";

                DataGridViewTextBoxColumn cantidad_producida = new DataGridViewTextBoxColumn();
                cantidad_producida.HeaderText = "CANTIDAD PRODUCIDA";
                cantidad_producida.Name = "cantidad_producida";

                DataGridViewTextBoxColumn precio_unitario = new DataGridViewTextBoxColumn();
                precio_unitario.HeaderText = "PRECIO UNITARIO";
                precio_unitario.Name = "precio_unitario";

                DataGridViewTextBoxColumn cantidad_recaudada = new DataGridViewTextBoxColumn();
                cantidad_recaudada.HeaderText = "CANTIDAD RECAUDADA";
                cantidad_recaudada.Name = "cantidad_recaudada";

                DataGridViewTextBoxColumn cantidad_por_cobrar = new DataGridViewTextBoxColumn();
                cantidad_por_cobrar.HeaderText = "CANTIDAD POR COBRAR";
                cantidad_por_cobrar.Name = "cantidad_por_cobrar";

                DataGridViewTextBoxColumn cantidad_prepagada = new DataGridViewTextBoxColumn();
                cantidad_prepagada.HeaderText = "CANTIDAD PREPAGADA";
                cantidad_prepagada.Name = "cantidad_prepagada";

                DataGridViewTextBoxColumn cantidad_diferencia = new DataGridViewTextBoxColumn();
                cantidad_diferencia.HeaderText = "DIFERENCIA";
                cantidad_diferencia.Name = "cantidad_diferencia";

                DataGridViewTextBoxColumn total = new DataGridViewTextBoxColumn();
                total.HeaderText = "TOTAL";
                total.Name = "total";

                dgvExportar.Columns.Add(codigo);
                dgvExportar.Columns.Add(nombre);
                dgvExportar.Columns.Add(categoria);
                dgvExportar.Columns.Add(cantidad_producida);
                dgvExportar.Columns.Add(precio_unitario);
                dgvExportar.Columns.Add(cantidad_recaudada);
                dgvExportar.Columns.Add(cantidad_por_cobrar);
                dgvExportar.Columns.Add(cantidad_prepagada);
                dgvExportar.Columns.Add(cantidad_diferencia);
                dgvExportar.Columns.Add(total);

                for (int i = 0; i < dgvDatos.Rows.Count; i++)
                {
                    dgvExportar.Rows.Add(
                                            dgvDatos.Rows[i].Cells["codigo"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["nombre"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["categoria"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["cantidad_producida"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["precio_unitario"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["cantidad_recaudada"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["cantidad_por_cobrar"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["cantidad_prepagada"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["cantidad_diferencia"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["total"].Value.ToString()
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
            dtFechaDesde.Text = sFecha;
            dtFechaHasta.Text = sFecha;
            btnExportar.Visible = false;
            dgvDatos.Rows.Clear();
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

            llenarGrid();
        }

        private void frmReporteProductosVendidosPor_Fechas_Load(object sender, EventArgs e)
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

        private void frmReporteProductosVendidosPor_Fechas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
    }
}
