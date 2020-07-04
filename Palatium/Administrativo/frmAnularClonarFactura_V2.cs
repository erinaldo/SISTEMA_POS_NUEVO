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
    public partial class frmAnularClonarFactura_V2 : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeNuevoOk ok;
        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoSiNo SiNo;

        Clases.ClaseFunciones funciones;
        Clases.ClaseLimpiarArreglos limpiar;
        Clases.ClaseValidarCaracteres caracter;
        Clases.ClaseValidarRUC validarRuc = new Clases.ClaseValidarRUC();
        Clases_Crear_Comandas.ClaseEliminarComanda_V2 eliminar;
        ValidarCedula validarCedula = new ValidarCedula();

        string sSql;
        string sFecha;
        string sFechaFactura;
        string sNumeroFactura;
        string sEstablecimiento;
        string sPuntoEmision;
        string sCiudad;
        string sCorreoAyuda;
        string sObservacionesComanda;
        string sAlergias;
        string sNombreParaMesa;
        string sNumeroComprobante;
        string sFechaAperturaOrden;
        string sFechaCierreOrden;
        
        DataTable dtConsulta;
        DataTable dtItems;
        DataTable dtDetalleItems;
        DataTable dtPagos;

        bool bRespuesta;

        int iIdPersonaAnterior;
        int iIdFactura;
        int iIdPedido;
        int iIdFacturaAnterior;
        int iIdPedidoAnterior;
        int iNumeroMovimientoCaja;
        int iIdLocalidadImpresora;
        int iIdPersona;
        int idTipoIdentificacion;
        int idTipoPersona;
        int iTercerDigito;
        int iIdOrigenOrden;
        int iNumeroPersonas;
        int iIdMesa;
        int iIdMesero;
        int iIdCajero;
        int iConsumoAlimentos;
        int iIdPromotor;
        int iIdRepartidor;
        int iFacturaElectronica_A;
        int iIdPosCierreCajero;
        int iIdLocalidadFactura;
        int iIdTipoComprobante;
        int iBanderaRecargoBoton;
        int iBanderaRemoverIvaBoton;
        int iBanderaComandaPendiente;
        int iIdDocumentoCobrar;
        int iNumeroPedidoAnterior;
        int iNumeroPedidoNuevo;

        Decimal dPorcentajeDescuento;
        Decimal dbTotalDebido_REC;

        SqlParameter[] parametro;

        public frmAnularClonarFactura_V2()
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

        //FUNCION PARA BUSCAR LA FACTURA
        private bool buscarFactura()
        {
            try
            {
                sSql = "";
                sSql += "select * from pos_vw_factura" + Environment.NewLine;
                sSql += "where numero_factura = @numero_factura" + Environment.NewLine;
                sSql += "and id_localidad = @id_localidad" + Environment.NewLine;
                sSql += "and idtipocomprobante = @idtipocomprobante";

                #region FUNCIONES DEL USUARIO

                parametro = new SqlParameter[3];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@numero_factura";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = Convert.ToInt32(txtNumeroFactura.Text.Trim());

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@id_localidad";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = Convert.ToInt32(cmbLocalidad.SelectedValue);

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@idtipocomprobante";
                parametro[2].SqlDbType = SqlDbType.Int;
                parametro[2].Value = 1;

                #endregion

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtConsulta, sSql, parametro);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "No existen registros con los parámetros ingresados.";
                    ok.ShowDialog();
                }

                else
                {
                    txtIdentificacionRecuperado.Text = dtConsulta.Rows[0]["identificacion"].ToString();
                    txtClienteRecuperado.Text = (dtConsulta.Rows[0]["nombres"].ToString() + " " + dtConsulta.Rows[0]["apellidos"].ToString()).Trim().ToUpper();
                    dtFecha.Text = Convert.ToDateTime(dtConsulta.Rows[0]["fecha_factura"].ToString()).ToString("dd/MM/yyyy");
                    txtEstablecimiento.Text = dtConsulta.Rows[0]["establecimiento"].ToString() + "-" + dtConsulta.Rows[0]["punto_emision"].ToString();

                    iIdPersonaAnterior = Convert.ToInt32(dtConsulta.Rows[0]["id_persona"].ToString());
                    iIdFacturaAnterior = Convert.ToInt32(dtConsulta.Rows[0]["id_factura"].ToString());
                    iIdPedidoAnterior = Convert.ToInt32(dtConsulta.Rows[0]["id_pedido"].ToString());
                    iIdOrigenOrden = Convert.ToInt32(dtConsulta.Rows[0]["id_pos_origen_orden"].ToString());
                    iNumeroPersonas = Convert.ToInt32(dtConsulta.Rows[0]["numero_personas"].ToString());
                    iIdMesa = Convert.ToInt32(dtConsulta.Rows[0]["id_pos_mesa"].ToString());
                    iIdCajero = Convert.ToInt32(dtConsulta.Rows[0]["id_pos_cajero"].ToString());
                    iIdMesero = Convert.ToInt32(dtConsulta.Rows[0]["id_pos_mesero"].ToString());
                    iConsumoAlimentos = Convert.ToInt32(dtConsulta.Rows[0]["consumo_alimentos"].ToString());
                    iIdRepartidor = Convert.ToInt32(dtConsulta.Rows[0]["id_pos_repartidor"].ToString());
                    iIdPromotor = Convert.ToInt32(dtConsulta.Rows[0]["id_pos_promotor"].ToString());
                    iFacturaElectronica_A = Convert.ToInt32(dtConsulta.Rows[0]["facturaelectronica"].ToString());
                    iIdLocalidadFactura = Convert.ToInt32(dtConsulta.Rows[0]["id_localidad"].ToString());
                    iIdTipoComprobante = Convert.ToInt32(dtConsulta.Rows[0]["idtipocomprobante"].ToString());
                    iIdPosCierreCajero = Convert.ToInt32(dtConsulta.Rows[0]["id_pos_cierre_cajero"].ToString());
                    iBanderaRecargoBoton = Convert.ToInt32(dtConsulta.Rows[0]["recargo_tarjeta"].ToString());
                    iBanderaRemoverIvaBoton = Convert.ToInt32(dtConsulta.Rows[0]["remover_iva"].ToString());
                    iBanderaComandaPendiente = Convert.ToInt32(dtConsulta.Rows[0]["bandera_cuenta_por_cobrar"].ToString());
                    iNumeroPedidoAnterior = Convert.ToInt32(dtConsulta.Rows[0]["numero_pedido"].ToString());
                    sNombreParaMesa = dtConsulta.Rows[0]["nombre_mesa"].ToString();
                    sFechaAperturaOrden = Convert.ToDateTime(dtConsulta.Rows[0]["fecha_apertura_orden"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    sFechaCierreOrden = Convert.ToDateTime(dtConsulta.Rows[0]["fecha_cierre_orden"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    sObservacionesComanda = dtConsulta.Rows[0]["observacion_comanda"].ToString();
                    sAlergias = dtConsulta.Rows[0]["alergias"].ToString();
                    sFechaFactura = dtConsulta.Rows[0]["fecha_factura"].ToString();

                    if (cargarDetalleGrid() == false)
                        return false;

                    if (cargarPagosRealizados() == false)
                        return false;

                    recalcularValores();
                    pintarDataGridView();

                    dgvPagos.ClearSelection();
                    dgvPedido.ClearSelection();

                    DateTime time = Convert.ToDateTime(dtFecha.Text.Trim());

                    TimeSpan span = (TimeSpan)(Convert.ToDateTime(sFecha) - time);

                    if (span.Days > 15)
                    {
                        ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                        ok.lblMensaje.Text = "El sistema permite la modificación del beneficinar dentro de los 15 días antes de su emisión.";
                        ok.ShowDialog();
                        grupoCliente.Enabled = false;
                        btnGuardar.Visible = false;
                    }

                    else
                    {
                        grupoCliente.Enabled = true;
                        btnGuardar.Visible = true;
                        txtIdentificacion.Focus();
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

        //FUNCION PARA CARGAR EL DETALLE DE LA ORDEN EN EL DATAGRID
        private bool cargarDetalleGrid()
        {
            try
            {
                sSql = "";
                sSql += "select * from pos_vw_detalle_comanda" + Environment.NewLine;
                sSql += "where id_pedido = " + iIdPedidoAnterior + Environment.NewLine;
                sSql += "order by id_det_pedido";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return false;
                }

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    Decimal dbTotalVer_P;

                    if (Program.iMostrarTotalLineaComanda == 1)
                    {
                        dbTotalVer_P = Convert.ToDecimal(dtConsulta.Rows[i]["precio_total"].ToString());
                    }

                    else
                    {
                        dbTotalVer_P = Convert.ToDecimal(dtConsulta.Rows[i]["precio_unitario"].ToString()) -
                                       Convert.ToDecimal(dtConsulta.Rows[i]["valor_dscto"].ToString());
                    }

                    dgvPedido.Rows.Add(Convert.ToDouble(dtConsulta.Rows[i]["cantidad"].ToString()).ToString(),
                                       dtConsulta.Rows[i]["nombre"].ToString().Trim().ToUpper(),
                                       dtConsulta.Rows[i]["precio_unitario"].ToString().Trim(),
                        //dtConsulta.Rows[i]["precio_total"].ToString().Trim(),
                                       dbTotalVer_P.ToString("N2"),
                                       dtConsulta.Rows[i]["id_producto"].ToString().Trim(),
                                       dtConsulta.Rows[i]["paga_iva"].ToString().Trim(),
                                       dtConsulta.Rows[i]["codigo_producto"].ToString().Trim(),
                                       dtConsulta.Rows[i]["secuencia"].ToString().Trim(),
                                       dtConsulta.Rows[i]["bandera_cortesia"].ToString().Trim(),
                                       dtConsulta.Rows[i]["motivo_cortesia"].ToString().Trim(),
                                       dtConsulta.Rows[i]["bandera_descuento"].ToString().Trim(),
                                       dtConsulta.Rows[i]["motivo_descuento"].ToString().Trim(),
                                       dtConsulta.Rows[i]["id_pos_mascara_item"].ToString().Trim(),
                                       dtConsulta.Rows[i]["id_pos_secuencia_entrega"].ToString().Trim(),
                                       dtConsulta.Rows[i]["ordenamiento"].ToString().Trim(),
                                       dtConsulta.Rows[i]["porcentaje_descuento_info"].ToString().Trim(),
                                       dtConsulta.Rows[i]["bandera_comentario"].ToString().Trim(),
                                       dtConsulta.Rows[i]["valor_dscto"].ToString().Trim(),
                                       dtConsulta.Rows[i]["paga_servicio"].ToString().Trim()
                                       );

                    //LLENAR LA MATRIZ DE DETALLE ITEMS CON LOS DATOS INGRESADOS EN LOS DETALLES EN CASO DE QUE SI HAYA
                    sSql = "";
                    sSql += "select PD.detalle, P.id_producto" + Environment.NewLine;
                    sSql += "from pos_det_pedido_detalle PD, cv403_det_pedidos DP, cv401_productos P" + Environment.NewLine;
                    sSql += "where PD.id_det_pedido = DP.id_det_pedido " + Environment.NewLine;
                    sSql += "and DP.id_producto = P.id_producto " + Environment.NewLine;
                    sSql += "and PD.id_det_pedido = " + Convert.ToInt32(dtConsulta.Rows[i]["id_det_pedido"].ToString()) + Environment.NewLine;
                    sSql += "and P.estado = 'A'" + Environment.NewLine;
                    sSql += "and DP.estado = 'A'" + Environment.NewLine;
                    sSql += "and PD.estado = 'A'";

                    dtItems = new DataTable();
                    dtItems.Clear();

                    bRespuesta = conexion.GFun_Lo_Busca_Registro(dtItems, sSql);

                    if (bRespuesta == false)
                    {
                        catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                        catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                        catchMensaje.ShowDialog();
                        return false;
                    }

                    if (dtItems.Rows.Count > 0)
                    {
                        Program.sDetallesItems[Program.iContadorDetalle, 0] = dtItems.Rows[0][1].ToString();

                        for (int j = 1; j <= dtItems.Rows.Count; j++)
                        {
                            Program.sDetallesItems[Program.iContadorDetalle, j] = dtItems.Rows[j - 1][0].ToString();
                        }

                        Program.iContadorDetalle++;
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

        //FUNCION PARA CARGAR EL DETALLE DE LA ORDEN EN EL DATAGRID
        private bool cargarPagosRealizados()
        {
            try
            {
                sSql = "";
                sSql += "select * from pos_vw_pedido_forma_pago" + Environment.NewLine;
                sSql += "where id_pedido = @id_pedido" + Environment.NewLine;
                sSql += "order by descripcion";

                parametro = new SqlParameter[1];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_pedido";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdPedidoAnterior;

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

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    dgvPagos.Rows.Add(dtConsulta.Rows[i]["id_pos_tipo_forma_cobro"].ToString(),
                                      dtConsulta.Rows[i]["descripcion"].ToString(),
                                      Convert.ToDecimal(dtConsulta.Rows[i]["valor"].ToString()).ToString("N2"),
                                      dtConsulta.Rows[i]["id_sri_forma_pago"].ToString());
                }

                dgvPagos.ClearSelection();

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

        //FUNCION PARA EXTRAER LA INFORMACION PARA FACTURAR
        private void datosFactura()
        {
            try
            {
                sSql = "";
                sSql += "select L.id_localidad, L.establecimiento, L.punto_emision, " + Environment.NewLine;
                sSql += "P.numero_factura, P.numeromovimientocaja, P.id_localidad_impresora" + Environment.NewLine;
                sSql += "from tp_localidades L, tp_localidades_impresoras P " + Environment.NewLine;
                sSql += "where L.id_localidad = P.id_localidad" + Environment.NewLine;
                sSql += "and L.id_localidad = @id_localidad" + Environment.NewLine;
                sSql += "and L.estado = @estado_1" + Environment.NewLine;
                sSql += "and P.estado = @estado_2";

                #region PARAMETROS

                parametro = new SqlParameter[3];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_localidad";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = Convert.ToInt32(cmbLocalidad.SelectedValue);

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@estado_1";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = "A";

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@estado_2";
                parametro[2].SqlDbType = SqlDbType.VarChar;
                parametro[2].Value = "A";

                #endregion

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtConsulta, sSql, parametro);

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
                        ok.lblMensaje.Text = "No se encuentran registros de la localidad seleccionada.";
                        ok.ShowDialog();
                    }

                else
                {
                    txtEstablecimiento.Text = dtConsulta.Rows[0]["establecimiento"].ToString() + "-" + dtConsulta.Rows[0]["punto_emision"].ToString();

                    sEstablecimiento = dtConsulta.Rows[0]["establecimiento"].ToString();
                    sPuntoEmision = dtConsulta.Rows[0]["punto_emision"].ToString();
                    txtSecuencialFactura.Text = dtConsulta.Rows[0]["numero_factura"].ToString();

                    iNumeroMovimientoCaja = Convert.ToInt32(dtConsulta.Rows[0]["numeromovimientocaja"].ToString());
                    iIdLocalidadImpresora = Convert.ToInt32(dtConsulta.Rows[0]["id_localidad_impresora"].ToString());
                }               
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA PINTAR EL DATAGRID
        private void pintarDataGridView()
        {
            try
            {
                for (int i = 0; i < dgvPedido.Rows.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        dgvPedido.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(192, 255, 192);
                    }

                    else
                    {
                        dgvPedido.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA RECALCULAR
        private void recalcularValores()
        {
            try
            {
                int iPagaIva_REC;
                int iPagaServicio_REC;

                Decimal dbCantidad_REC;
                Decimal dbPrecioUnitario_REC;
                Decimal dbValorDescuento_REC;
                Decimal dbValorIva_REC;
                Decimal dbValorServicio_REC;

                Decimal dbSumaSubtotalConIva_REC = 0;
                Decimal dbSumaSubtotalSinIva_REC = 0;
                Decimal dbSumaDescuentoConIva_REC = 0;
                Decimal dbSumaDescuentoSinIva_REC = 0;

                Decimal dbSumaSubtotales_REC;
                Decimal dbSumaDescuentos_REC;

                Decimal dbSubtotalNeto_REC = 0;
                Decimal dbSumaIva_REC = 0;
                Decimal dbSumaServicio_REC = 0;

                for (int i = 0; i < dgvPedido.Rows.Count; i++)
                {
                    iPagaIva_REC = Convert.ToInt32(dgvPedido.Rows[i].Cells["paga_iva"].Value);
                    iPagaServicio_REC = Convert.ToInt32(dgvPedido.Rows[i].Cells["paga_servicio"].Value);

                    dbCantidad_REC = Convert.ToDecimal(dgvPedido.Rows[i].Cells["cantidad"].Value);
                    dbPrecioUnitario_REC = Convert.ToDecimal(dgvPedido.Rows[i].Cells["valor_unitario"].Value);
                    dbValorDescuento_REC = Convert.ToDecimal(dgvPedido.Rows[i].Cells["valor_descuento"].Value);

                    if (iPagaIva_REC == 0)
                    {
                        dbSumaSubtotalSinIva_REC += dbCantidad_REC * dbPrecioUnitario_REC;
                        dbSumaDescuentoSinIva_REC += dbCantidad_REC * dbValorDescuento_REC;
                    }

                    else
                    {
                        dbSumaSubtotalConIva_REC += dbCantidad_REC * dbPrecioUnitario_REC;
                        dbSumaDescuentoConIva_REC += dbCantidad_REC * dbValorDescuento_REC;
                        dbValorIva_REC = (dbPrecioUnitario_REC - dbValorDescuento_REC) * Convert.ToDecimal(Program.iva);
                        dbSumaIva_REC += dbCantidad_REC * dbValorIva_REC;
                    }

                    if (iPagaServicio_REC == 1)
                    {
                        dbValorServicio_REC = (dbPrecioUnitario_REC - dbValorDescuento_REC) * Convert.ToDecimal(Program.servicio);
                        dbSumaServicio_REC += dbCantidad_REC * dbValorServicio_REC;
                    }
                }

                dbSumaSubtotales_REC = dbSumaSubtotalConIva_REC + dbSumaSubtotalSinIva_REC;
                dbSumaDescuentos_REC = dbSumaDescuentoConIva_REC + dbSumaDescuentoSinIva_REC;

                dbSubtotalNeto_REC = dbSumaSubtotalConIva_REC + dbSumaSubtotalSinIva_REC - dbSumaDescuentoConIva_REC - dbSumaDescuentoSinIva_REC;
                dbTotalDebido_REC = dbSubtotalNeto_REC + dbSumaIva_REC + dbSumaServicio_REC;

                lblSubtotal.Text = "$ " + dbSumaSubtotales_REC.ToString("N2");
                lblDescuento.Text = "$ " + dbSumaDescuentos_REC.ToString("N2");
                lblImpuestos.Text = "$ " + (dbSumaIva_REC + dbSumaServicio_REC).ToString("N2");
                lblTotal.Text = "$ " + dbTotalDebido_REC.ToString("N2");

                //FUNCION PARA OBTENER EL PORCENTAJE DE DESCUENTO
                Decimal dbSumaPrecioUnitario_D = 0;
                Decimal dbSumaDescuentos_D = 0;
                Decimal dbCantidad_D;
                Decimal dbPrecioUnitario_D;
                Decimal dbValorDescuento_D;
                //Decimal dbResultado_D;

                for (int i = 0; i < dgvPedido.Rows.Count; i++)
                {
                    dbCantidad_D = Convert.ToDecimal(dgvPedido.Rows[i].Cells["cantidad"].Value);
                    dbPrecioUnitario_D = Convert.ToDecimal(dgvPedido.Rows[i].Cells["valor_unitario"].Value);
                    dbValorDescuento_D = Convert.ToDecimal(dgvPedido.Rows[i].Cells["valor_descuento"].Value);

                    dbSumaPrecioUnitario_D += dbCantidad_D * dbPrecioUnitario_D;
                    dbSumaDescuentos_D += dbCantidad_D * dbValorDescuento_D;
                }

                dPorcentajeDescuento = Convert.ToDecimal((dbSumaDescuentos_D * 100) / dbSumaPrecioUnitario_D);

                lblPorcentajeDescuento.Text = dPorcentajeDescuento.ToString("N2") + "%";
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA CANCELAR
        private void limpiarFormulario()
        {
            fechaSistema();

            dtFecha.Text = sFecha;

            dgvPedido.Rows.Clear();
            dgvPagos.Rows.Clear();

            lblSubtotal.Text = "$ 0.00";
            lblDescuento.Text = "$ 0.00";
            lblImpuestos.Text = "$ 0.00";
            lblTotal.Text = "$ 0.00";

            dPorcentajeDescuento = 0;

            lblPorcentajeDescuento.Text = dPorcentajeDescuento.ToString("N2") + "%";

            txtNumeroFactura.Clear();
            txtIdentificacion.Clear();
            txtIdentificacionRecuperado.Clear();
            txtClienteRecuperado.Clear();
            txtApellidos.Clear();
            txtNombres.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtMail.Clear();
            txtEstablecimiento.Clear();
            txtSecuencialFactura.Clear();

            chkPasaporte.Checked = false;
            grupoCliente.Enabled = false;

            iIdFactura = 0;
            iIdPedido = 0;
            iIdPersona = 0;
            iIdPersonaAnterior = 0;
            iNumeroPedidoAnterior = 0;
            iNumeroPedidoNuevo = 0;
            btnGuardar.Visible = false;
            txtNumeroFactura.Focus();
        }

        //FUNCION PARA VALIDAR LA CONSULTA
        private void validarConsulta()
        {
            iIdPersona = 0;
            txtIdentificacion.Clear();
            txtApellidos.Clear();
            txtNombres.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtMail.Clear();
            chkPasaporte.Checked = false;

            if (Convert.ToInt32(cmbLocalidad.SelectedValue) == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor seleccione la localidad";
                ok.ShowDialog();
                cmbLocalidad.Focus();
                return;
            }

            if (txtNumeroFactura.Text.Trim() == "")
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor ingrese el número de factura a buscar.";
                ok.ShowDialog();
                txtNumeroFactura.Focus();
                return;
            }

            dgvPedido.Rows.Clear();
            dgvPagos.Rows.Clear();
            datosFactura();
            buscarFactura();
        }

        //FUNCION PARA CONSULTAR DATOS DEL CLIENTE
        private void consultarRegistro()
        {
            try
            {
                funciones = new Clases.ClaseFunciones();

                bRespuesta = funciones.consultarRegistroPersona(txtIdentificacion.Text.Trim(), 0, 0);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = funciones.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                dtConsulta = funciones.dtConsulta;

                if (dtConsulta.Rows.Count > 0)
                {
                    iIdPersona = Convert.ToInt32(dtConsulta.Rows[0]["id_persona"].ToString());
                    txtNombres.Text = dtConsulta.Rows[0]["nombres"].ToString();
                    txtApellidos.Text = dtConsulta.Rows[0]["apellidos"].ToString();
                    txtMail.Text = dtConsulta.Rows[0]["correo_electronico"].ToString();
                    txtDireccion.Text = dtConsulta.Rows[0]["direccion_completa"].ToString();
                    sCiudad = dtConsulta.Rows[0]["direccion"].ToString();

                    if (dtConsulta.Rows[0]["domicilio"].ToString() != "")
                        txtTelefono.Text = dtConsulta.Rows[0]["domicilio"].ToString();
                    else if (dtConsulta.Rows[0]["celular"].ToString() != "")
                        txtTelefono.Text = dtConsulta.Rows[0]["celular"].ToString();
                    else
                        txtTelefono.Text = "";

                    btnGuardar.Focus();
                }

                else
                {
                    Facturador.frmNuevoCliente frmNuevoCliente = new Facturador.frmNuevoCliente(txtIdentificacion.Text.Trim(), chkPasaporte.Checked);
                    frmNuevoCliente.ShowDialog();

                    if (frmNuevoCliente.DialogResult == DialogResult.OK)
                    {
                        iIdPersona = frmNuevoCliente.iCodigo;
                        txtIdentificacion.Text = frmNuevoCliente.sIdentificacion;
                        txtNombres.Text = frmNuevoCliente.sNombre;
                        txtApellidos.Text = frmNuevoCliente.sApellido;
                        txtTelefono.Text = frmNuevoCliente.sTelefono;
                        txtDireccion.Text = frmNuevoCliente.sDireccion;
                        txtMail.Text = frmNuevoCliente.sMail;
                        sCiudad = frmNuevoCliente.sCiudad;
                        frmNuevoCliente.Close();
                        btnGuardar.Focus();
                    }
                }

                btnEditar.Visible = true;
                return;               
            }

            catch (Exception ex)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = ex.Message;
                ok.ShowDialog();
                txtIdentificacion.Clear();
                txtIdentificacion.Focus();
            }
        }

        //FUNCION PARA VALIDAR LA IDENTIFICACION
        private void validarIdentificacion()
        {
            try
            {
                if (txtIdentificacion.Text.Length >= 10)
                {
                    iTercerDigito = Convert.ToInt32(txtIdentificacion.Text.Substring(2, 1));

                    if (txtIdentificacion.Text.Length == 10)
                    {
                        if (validarCedula.validarCedulaConsulta(txtIdentificacion.Text.Trim()) == "SI")
                        {
                            consultarRegistro();
                            return;
                        }

                        else
                        {
                            mensajeValidarCedula();
                            return;
                        }
                    }

                    else if (txtIdentificacion.Text.Length == 13)
                    {
                        if (iTercerDigito == 9)
                        {
                            if (validarRuc.validarRucPrivado(txtIdentificacion.Text.Trim()) == true)
                            {
                                consultarRegistro();
                                return;
                            }

                            else
                            {
                                mensajeValidarCedula();
                                return;
                            }
                        }

                        else if (iTercerDigito == 6)
                        {
                            if (validarRuc.validarRucPublico(txtIdentificacion.Text.Trim()) == true)
                            {
                                consultarRegistro();
                                return;
                            }

                            else
                            {
                                mensajeValidarCedula();
                                return;
                            }
                        }

                        else if (iTercerDigito <= 5 || iTercerDigito >= 0)
                        {
                            if (validarRuc.validarRucNatural(txtIdentificacion.Text.Trim()) == true)
                            {
                                consultarRegistro();
                                return;
                            }

                            else
                            {
                                mensajeValidarCedula();
                                return;
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "El número de identificación ingresado es incorrecto.";
                ok.ShowDialog();
                txtIdentificacion.Clear();
                txtIdentificacion.Focus();
            }
        }

        //FUNCION MENSAJE DE VALIDACION DE CEDULA
        private void mensajeValidarCedula()
        {
            ok = new VentanasMensajes.frmMensajeNuevoOk(3);
            ok.lblMensaje.Text = "El número de identificación ingresado es incorrecto.";
            ok.ShowDialog();
            txtIdentificacion.Clear();
            txtApellidos.Clear();
            txtNombres.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtMail.Clear();
            txtIdentificacion.Focus();
        }

        #endregion

        #region FUNCIONES DE CREACION DE LA NUEVA COMANDA

        //FUNCION PARA ENVIAR LOS PARAMETROS A LA COMANDA - INSERTAR NUEVA COMANDA
        private bool procesoActualizacion()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (crearTablaItems() == false)
                {
                    this.Cursor = Cursors.Default;
                    return false;
                }

                if (crearTableDetalleItems() == false)
                {
                    this.Cursor = Cursors.Default;
                    return false;
                }

                if (crearTablaPagos() == false)
                {
                    this.Cursor = Cursors.Default;
                    return false;
                }

                Clases_Crear_Comandas.ClaseCrearComanda comanda = new Clases_Crear_Comandas.ClaseCrearComanda();

                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    this.Cursor = Cursors.Default;
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Error al abrir transacción";
                    ok.ShowDialog();
                    return false;
                }

                bRespuesta = comanda.insertarComanda(0, iIdPersona, 0, iIdOrigenOrden, dbTotalDebido_REC, "Abierta",
                                                    Convert.ToDecimal(dPorcentajeDescuento), iIdMesa, iIdCajero,
                                                    iNumeroPersonas, "", iIdMesero, Program.iIdPosTerminal,
                                                    Convert.ToDecimal(Program.servicio * 100), iConsumoAlimentos,
                                                    iIdPromotor, iIdRepartidor, iIdPosCierreCajero, 1,
                                                    dtItems, dtDetalleItems, 0, iIdLocalidadFactura,
                                                    sNombreParaMesa, sObservacionesComanda, sAlergias, 
                                                    sFechaFactura, sFechaAperturaOrden, sFechaCierreOrden, conexion);

                if (bRespuesta == false)
                {
                    this.Cursor = Cursors.Default;
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = comanda.sMensajeError;
                    catchMensaje.ShowDialog();
                    return false;
                }

                iIdPedido = comanda.iIdPedido;
                iNumeroPedidoNuevo = comanda.iNumeroPedidoOrden;

                if (cambiarDocumentoPorCobrar(iIdPedido) == false)
                {
                    this.Cursor = Cursors.Default;
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    return false;
                }

                comanda = new Clases_Crear_Comandas.ClaseCrearComanda();

                bRespuesta = comanda.insertarFactura(iIdPedido, iIdTipoComprobante, iFacturaElectronica_A,
                                                     iIdPersona, iIdLocalidadFactura, dtPagos, dbTotalDebido_REC, iBanderaRecargoBoton,
                                                     iBanderaRemoverIvaBoton, iBanderaComandaPendiente, 0,
                                                     Convert.ToDateTime(sFechaFactura).ToString("yyyy-MM-dd"),
                                                     iIdDocumentoCobrar, conexion);

                if (bRespuesta == false)
                {
                    this.Cursor = Cursors.Default;
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = comanda.sMensajeError;
                    catchMensaje.ShowDialog();
                    return false;
                }

                sEstablecimiento = comanda.sEstablecimiento;
                sPuntoEmision = comanda.sPuntoEmision;
                sNumeroComprobante = comanda.sNumeroComprobante;
                iIdFactura = comanda.iIdFactura;

                eliminar = new Clases_Crear_Comandas.ClaseEliminarComanda_V2();

                bRespuesta = eliminar.procesoEliminacion(iIdPedidoAnterior, "CAMBIO DE BENEFICIARIO", 0, sFecha, 1, conexion);

                if (bRespuesta == false)
                {
                    this.Cursor = Cursors.Default;
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = comanda.sMensajeError;
                    catchMensaje.ShowDialog();
                    return false;
                }

                if (actualizarPagos() == false)
                {
                    this.Cursor = Cursors.Default;
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    return false;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);

                limpiar = new Clases.ClaseLimpiarArreglos();
                limpiar.limpiarArregloComentarios();
                this.Cursor = Cursors.Default;
                ok = new VentanasMensajes.frmMensajeNuevoOk(10);
                ok.lblMensaje.Text = "El registro ha sido modificado éxitosamente." + Environment.NewLine +
                                     "N° Factura: " + sEstablecimiento + "-" + sPuntoEmision + "-" + sNumeroComprobante.PadLeft(9, '0');
                ok.ShowDialog();
                limpiarFormulario();
                return true;
            }

            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return false;
            }
        }

        //FUNCION PARA CREAR LA TABLA DE PAGOS PARA ENVIAR POR PARAMETRO
        private bool crearTablaPagos()
        {
            try
            {
                dtPagos = new DataTable();
                dtPagos.Clear();

                dtPagos.Columns.Add("id_pos_tipo_forma_cobro");
                dtPagos.Columns.Add("forma_pago");
                dtPagos.Columns.Add("valor");
                dtPagos.Columns.Add("id_sri_forma_pago");

                for (int i = 0; i < dgvPagos.Rows.Count; i++)
                {
                    dtPagos.Rows.Add(dgvPagos.Rows[i].Cells["ID"].Value,
                                     dgvPagos.Rows[i].Cells["fpago"].Value,
                                     dgvPagos.Rows[i].Cells["valor"].Value,
                                     dgvPagos.Rows[i].Cells["id_sri"].Value
                                    );
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

        //FUNCION PARA CREAR LA TABLA DE ITEMS PARA ENVIAR POR PARAMETRO
        private bool crearTablaItems()
        {
            try
            {
                dtItems = new DataTable();
                dtItems.Clear();

                dtItems.Columns.Add("id_producto");
                dtItems.Columns.Add("valor_unitario");
                dtItems.Columns.Add("cantidad");
                dtItems.Columns.Add("valor_descuento");
                dtItems.Columns.Add("paga_iva");
                dtItems.Columns.Add("bandera_cortesia");
                dtItems.Columns.Add("bandera_descuento");
                dtItems.Columns.Add("bandera_comentario");
                dtItems.Columns.Add("id_mascara");
                dtItems.Columns.Add("id_ordenamiento");
                dtItems.Columns.Add("secuencia_impresion");
                dtItems.Columns.Add("motivo_cortesia");
                dtItems.Columns.Add("motivo_descuento");
                dtItems.Columns.Add("codigo_producto");
                dtItems.Columns.Add("nombre_producto");
                dtItems.Columns.Add("porcentaje_descuento");
                dtItems.Columns.Add("paga_servicio");

                for (int i = 0; i < dgvPedido.Rows.Count; i++)
                {
                    dtItems.Rows.Add(dgvPedido.Rows[i].Cells["id_producto"].Value,
                                     dgvPedido.Rows[i].Cells["valor_unitario"].Value,
                                     dgvPedido.Rows[i].Cells["cantidad"].Value,
                                     dgvPedido.Rows[i].Cells["valor_descuento"].Value,
                                     dgvPedido.Rows[i].Cells["paga_iva"].Value,
                                     dgvPedido.Rows[i].Cells["bandera_cortesia"].Value,
                                     dgvPedido.Rows[i].Cells["bandera_descuento"].Value,
                                     dgvPedido.Rows[i].Cells["bandera_comentario"].Value,
                                     dgvPedido.Rows[i].Cells["id_mascara"].Value,
                                     dgvPedido.Rows[i].Cells["id_ordenamiento"].Value,
                                     dgvPedido.Rows[i].Cells["secuencia_impresion"].Value,
                                     dgvPedido.Rows[i].Cells["motivo_cortesia"].Value,
                                     dgvPedido.Rows[i].Cells["motivo_descuento"].Value,
                                     dgvPedido.Rows[i].Cells["codigo_producto"].Value,
                                     dgvPedido.Rows[i].Cells["nombre_producto"].Value,
                                     dgvPedido.Rows[i].Cells["porcentaje_descuento"].Value,
                                     dgvPedido.Rows[i].Cells["paga_servicio"].Value
                                    );
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

        //FUNCION PARA CREAR LA TABLA DE DETALLE DE LOS ITEMS
        private bool crearTableDetalleItems()
        {
            try
            {
                int iIdDetalle_A = 0;
                string sDetalle_A;

                dtDetalleItems = new DataTable();
                dtDetalleItems.Clear();

                dtDetalleItems.Columns.Add("id_producto");
                dtDetalleItems.Columns.Add("detalle");

                for (int i = 0; i < Program.sDetallesItems.GetLength(0); i++)
                {
                    if (Program.sDetallesItems[i, 0] != null)
                    {
                        iIdDetalle_A = Convert.ToInt32(Program.sDetallesItems[i, 0]);
                        sDetalle_A = Program.sDetallesItems[i, 1];

                        dtDetalleItems.Rows.Add(iIdDetalle_A, sDetalle_A);
                    }

                    else
                        break;
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

        //FUNCION PARA CAMBIAR EL DOCUMENTO POR COBRAR DE LAS FORMAS PAGOS
        private bool cambiarDocumentoPorCobrar(int iIdPedidoActual_P)
        {
            try
            {
                //AQUI CONSULTAR EL ID_DOCUMENTO_PAGO NUEVO
                sSql = "";
                sSql += "select id_documento_cobrar " + Environment.NewLine;
                sSql += "from cv403_dctos_por_cobrar" + Environment.NewLine;
                sSql += "where id_pedido = @id_pedido" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_pedido";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdPedidoActual_P;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@estado";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = "A";

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

                iIdDocumentoCobrar = Convert.ToInt32(dtConsulta.Rows[0]["id_documento_cobrar"].ToString());

                //AQUI CONSULTAR EL ID_DOCUMENTO_PAGO ANTIGUO
                sSql = "";
                sSql += "select id_documento_cobrar " + Environment.NewLine;
                sSql += "from cv403_dctos_por_cobrar" + Environment.NewLine;
                sSql += "where id_pedido = @id_pedido" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_pedido";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdPedidoAnterior;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@estado";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = "A";

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

                int iIdDocumentoCobrarAntiguo = Convert.ToInt32(dtConsulta.Rows[0]["id_documento_cobrar"].ToString());

                //ACTUALIZAMOS LA TABLA CV403_DOCUMENTOS_PAGADOS
                sSql = "";
                sSql += "update cv403_documentos_pagados set" + Environment.NewLine;
                sSql += "id_documento_cobrar = @id_documento_cobrar_nuevo" + Environment.NewLine;
                sSql += "where id_documento_cobrar = @id_documento_cobrar_antiguo" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                parametro = new SqlParameter[3];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_documento_cobrar_nuevo";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdDocumentoCobrar;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@id_documento_cobrar_antiguo";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = iIdDocumentoCobrarAntiguo;

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@estado";
                parametro[2].SqlDbType = SqlDbType.VarChar;
                parametro[2].Value = "A";

                #endregion 

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return false;
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

        //FUNCION PARA REEMPLAZAR LOS CAMPOS DE LOS MOVIMIENTOS DE CAJA
        private bool actualizarPagos()
        {
            try
            {
                int a;

                //OBTENER EL ID_PAGO
                sSql = "";
                sSql += "select id_pago" + Environment.NewLine;
                sSql += "from cv403_documentos_pagados" + Environment.NewLine;
                sSql += "where id_documento_cobrar = @id_documento_cobrar" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_documento_cobrar";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdDocumentoCobrar;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@estado";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = "A";

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

                int iIdPago_REC = Convert.ToInt32(dtConsulta.Rows[0]["id_pago"].ToString());

                //OBTENER LOS REGISTROS DE CV403_DOCUMENTOS_PAGOS
                sSql = "";
                sSql += "select *" + Environment.NewLine;
                sSql += "from cv403_documentos_pagos" + Environment.NewLine;
                sSql += "where id_pago = @id_pago" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_pago";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdPago_REC;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@estado";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = "A";

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

                string sNumeroComprobante_P = "FACT. No. " + sEstablecimiento + "-" + sPuntoEmision + "-" + sNumeroComprobante.Trim().PadLeft(9, '0');

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    int iIdDocumentoPago_P = Convert.ToInt32(dtConsulta.Rows[i]["id_documento_pago"].ToString());

                    sSql = "";
                    sSql += "select id_pos_movimiento_caja, concepto" + Environment.NewLine;
                    sSql += "from pos_movimiento_caja" + Environment.NewLine;
                    sSql += "where id_documento_pago = @id_documento_pago" + Environment.NewLine;
                    sSql += "and estado = @estado";

                    #region PARAMETROS

                    parametro = new SqlParameter[2];
                    parametro[0] = new SqlParameter();
                    parametro[0].ParameterName = "@id_documento_pago";
                    parametro[0].SqlDbType = SqlDbType.Int;
                    parametro[0].Value = iIdDocumentoPago_P;

                    parametro[1] = new SqlParameter();
                    parametro[1].ParameterName = "@estado";
                    parametro[1].SqlDbType = SqlDbType.VarChar;
                    parametro[1].Value = "A";

                    #endregion

                    DataTable dtAux = new DataTable();
                    dtAux.Clear();

                    bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtAux, sSql, parametro);

                    if (bRespuesta == false)
                    {
                        catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                        catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                        catchMensaje.ShowDialog();
                        return false;
                    }

                    int iIdPosMovimientoCaja_P = Convert.ToInt32(dtAux.Rows[0]["id_pos_movimiento_caja"].ToString());
                    string sConcepto_P = dtAux.Rows[0]["concepto"].ToString();
                    string sNuevoConcepto_P = sConcepto_P.Replace(iNumeroPedidoAnterior.ToString(), iNumeroPedidoNuevo.ToString());

                    sSql = "";
                    sSql += "update pos_movimiento_caja set" + Environment.NewLine;
                    sSql += "concepto = @concepto," + Environment.NewLine;
                    sSql += "documento_venta = @documento_venta" + Environment.NewLine;
                    sSql += "where id_pos_movimiento_caja = @id_pos_movimiento_caja" + Environment.NewLine;
                    sSql += "and estado = @estado";

                    #region PARAMETROS

                    a = 0;
                    parametro = new SqlParameter[4];
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@concepto";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = sNuevoConcepto_P;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@documento_venta";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = sNumeroComprobante_P;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_pos_movimiento_caja";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdPosMovimientoCaja_P;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@estado";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = "A";

                    #endregion

                    if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                    {
                        catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                        catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                        catchMensaje.ShowDialog();
                        return false;
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

        #endregion

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAnularClonarFactura_V2_Load(object sender, EventArgs e)
        {
            fechaSistema();
            llenarComboLocalidades();
            this.ActiveControl = txtNumeroFactura;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            validarConsulta();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiarFormulario();
        }

        private void txtNumeroFactura_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                validarConsulta();
                return;
            }

            caracter = new Clases.ClaseValidarCaracteres();
            caracter.soloNumeros(e);
        }

        private void btnEditarFactura_Click(object sender, EventArgs e)
        {
            if (txtSecuencialFactura.ReadOnly == true)
            {
                sNumeroFactura = txtSecuencialFactura.Text.Trim();
                txtSecuencialFactura.ReadOnly = false;
                txtSecuencialFactura.Focus();
            }

            else
            {
                txtSecuencialFactura.Text = sNumeroFactura;
                txtSecuencialFactura.ReadOnly = true;
                txtIdentificacion.Focus();
            }
        }

        private void btnEditar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Facturador.frmNuevoCliente nuevoCliente = new Facturador.frmNuevoCliente(txtIdentificacion.Text.Trim(), chkPasaporte.Checked);
            nuevoCliente.ShowDialog();

            if (nuevoCliente.DialogResult == DialogResult.OK)
            {
                iIdPersona = nuevoCliente.iCodigo;
                txtIdentificacion.Text = nuevoCliente.sIdentificacion;
                consultarRegistro();
            }
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            Facturador.frmControlDatosCliente controlDatosCliente = new Facturador.frmControlDatosCliente();
            controlDatosCliente.ShowDialog();

            if (controlDatosCliente.DialogResult == DialogResult.OK)
            {
                iIdPersona = controlDatosCliente.iCodigo;
                txtIdentificacion.Text = controlDatosCliente.sIdentificacion;
                controlDatosCliente.Close();
                consultarRegistro();
            }
        }

        private void btnConsumidorFinal_Click(object sender, EventArgs e)
        {
            txtIdentificacion.Text = "9999999999999";
            txtApellidos.Text = "CONSUMIDOR FINAL";
            txtNombres.Text = "CONSUMIDOR FINAL";
            txtTelefono.Text = "9999999999";
            txtMail.Text = Program.sCorreoElectronicoDefault;
            txtDireccion.Text = "QUITO";
            iIdPersona = Program.iIdPersona;
            idTipoIdentificacion = 180;
            idTipoPersona = 2447;
            btnEditar.Visible = false;
        }

        private void btnCorreoElectronicoDefault_Click(object sender, EventArgs e)
        {
            if (btnCorreoElectronicoDefault.AccessibleName == "0")
            {
                sCorreoAyuda = txtMail.Text.Trim();
                btnCorreoElectronicoDefault.AccessibleName = "1";
                txtMail.ReadOnly = false;
                txtMail.Focus();
            }

            else
            {
                txtMail.Text = sCorreoAyuda;
                btnCorreoElectronicoDefault.AccessibleName = "0";
                txtMail.ReadOnly = true;
                btnCorreoElectronicoDefault.Focus();
            }
        }

        private void txtIdentificacion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtIdentificacion.Text != "")
                {
                    funciones = new Clases.ClaseFunciones();
                    //AQUI INSTRUCCIONES PARA CONSULTAR Y VALIDAR LA CEDULA
                    if ((funciones.esNumero(txtIdentificacion.Text.Trim()) == true) && (chkPasaporte.Checked == false))
                    {
                        //INSTRUCCIONES PARA VALIDAR
                        validarIdentificacion();
                    }
                    else
                    {
                        //CONSULTAR EN LA BASE DE DATOS
                        consultarRegistro();
                    }
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (iIdPersona == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(5);
                ok.lblMensaje.Text = "Favor seleccione el cliente.";
                ok.ShowDialog();
                txtIdentificacion.Focus();
                return;
            }

            if (iIdPersonaAnterior == iIdPersona)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(5);
                ok.lblMensaje.Text = "La factura a modificar contiene el mismo beneficiario. Se recomienda revisar la información.";
                ok.ShowDialog();
                return;
            }

            SiNo = new VentanasMensajes.frmMensajeNuevoSiNo();
            SiNo.lblMensaje.Text = "¿Está seguro que desea guardar los cambios?";
            SiNo.ShowDialog();

            if (SiNo.DialogResult == DialogResult.OK)
            {
                procesoActualizacion();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Administrativo.frmSeleccionarFacturaModal modal = new frmSeleccionarFacturaModal();
            modal.ShowInTaskbar = false;
            modal.ShowDialog();

            if (modal.DialogResult == DialogResult.OK)
            {
                int iNumeroFacturaAux = modal.iNumeroFactura;
                int iIdLocalidadAux = modal.iIdLocalidad;
                modal.Close();

                cmbLocalidad.SelectedValue = iIdLocalidadAux;
                txtNumeroFactura.Text = iNumeroFacturaAux.ToString();

                validarConsulta();
            }
        }
    }
}
