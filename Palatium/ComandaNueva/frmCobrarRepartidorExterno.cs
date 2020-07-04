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

namespace Palatium.ComandaNueva
{
    public partial class frmCobrarRepartidorExterno : Form
    {
        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoOk ok;

        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        Clases.ClaseValidarRUC validarRuc = new Clases.ClaseValidarRUC();
        Clases.ClaseAbrirCajon abrir = new Clases.ClaseAbrirCajon();
        Clases_Crear_Comandas.ClaseCrearComanda comanda;
        ValidarCedula validarCedula = new ValidarCedula();

        SqlParameter[] parametro;

        string sSql;
        string sFecha;
        string sCiudad;
        string sEstablecimiento;
        string sPuntoEmision;
        string sNumeroSecuencial;
        string sCodigoOrigenOrden;
        string sCorreoAyuda;
        string sFiltroPago;
        string sEtiquetaForma;
        string sNumeroComprobante;
        string sDescripcionFormaPago;
        string sCodigoMetodoPago;
        string sNumeroFactura;

        bool bRespuesta;

        DataTable dtConsulta;
        DataTable dtOriginal;
        DataTable dtPagos;

        int iIdPedido;
        int iIdSriFormaPago_P;
        int iIdListaMinorista_P;
        int iIdPersona;
        int iNumeroCuenta_P;
        int iNumeroPedido_P;
        int idTipoIdentificacion;
        int idTipoPersona;
        int iIdTipoFormaCobro;
        int iIdTipoComprobante;
        int iIdSriFormaPago;
        int iIdFactura;
        int iNumeroNotaEntrega;
        int iEtiqueta;
        int iTercerDigito;
        int iIdDocumentoPorCobrar;
        int iNumeroMovimientoCaja;
        int iIdLocalidadImpresora;

        Decimal dTotal;
        Decimal dbCambio;

        public frmCobrarRepartidorExterno(string sIdPedido_P)
        {
            this.iIdPedido = Convert.ToInt32(sIdPedido_P);
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //VERIFICAR SI LA CADENA ES UN NUMERO O UN STRING
        private bool esNumero(object Expression)
        {

            bool isNum;

            double retNum;

            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);

            return isNum;
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
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
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
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtMail.Clear();
            txtIdentificacion.Focus();
        }

        //FUNCION PARA CONSULTAR DATOS DEL CLIENTE
        private void consultarRegistro()
        {
            try
            {
                sSql = "";
                sSql += "select * from pos_vw_consulta_cliente_factura" + Environment.NewLine;
                //sSql += "select * from pos_vw_cargar_informacion_cliente" + Environment.NewLine;
                sSql += "where identificacion = @identificacion";

                parametro = new SqlParameter[1];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@identificacion";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = txtIdentificacion.Text.Trim();

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
                    Facturador.frmNuevoCliente frmNuevoCliente = new Facturador.frmNuevoCliente(txtIdentificacion.Text.Trim(), chkPasaporte.Checked);
                    frmNuevoCliente.ShowDialog();

                    if (frmNuevoCliente.DialogResult == DialogResult.OK)
                    {
                        iIdPersona = frmNuevoCliente.iCodigo;
                        txtIdentificacion.Text = frmNuevoCliente.sIdentificacion;
                        txtApellidos.Text = (frmNuevoCliente.sNombre + " " + frmNuevoCliente.sApellido).Trim().ToUpper();
                        txtTelefono.Text = frmNuevoCliente.sTelefono;
                        txtDireccion.Text = frmNuevoCliente.sDireccion;
                        txtMail.Text = frmNuevoCliente.sMail;
                        sCiudad = frmNuevoCliente.sCiudad;
                        frmNuevoCliente.Close();
                    }
                }

                else
                {
                    iIdPersona = Convert.ToInt32(dtConsulta.Rows[0]["id_persona"].ToString());
                    txtApellidos.Text = (dtConsulta.Rows[0]["nombres"].ToString() + " " + dtConsulta.Rows[0]["apellidos"].ToString()).Trim().ToUpper();
                    txtMail.Text = dtConsulta.Rows[0]["correo_electronico"].ToString();
                    txtDireccion.Text = dtConsulta.Rows[0]["direccion_completa"].ToString();
                    sCiudad = dtConsulta.Rows[0]["direccion"].ToString();

                    if (dtConsulta.Rows[0]["telefono_domicilio"].ToString() != "")
                        txtTelefono.Text = dtConsulta.Rows[0]["telefono_domicilio"].ToString();
                    else if (dtConsulta.Rows[0]["celular"].ToString() != "")
                        txtTelefono.Text = dtConsulta.Rows[0]["celular"].ToString();
                    else
                        txtTelefono.Text = "";
                }

                btnFacturar.Focus();
                return;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                txtIdentificacion.Clear();
                txtIdentificacion.Focus();
            }
        }

        //FUNCION PARA EXTRAER LA LISTA MINORISTA
        private void extraerListaMinorista()
        {
            try
            {
                sSql = "";
                sSql += "select id_lista_precio from cv403_listas_precios" + Environment.NewLine;
                sSql += "where lista_minorista = @lista_minorista" + Environment.NewLine;
                sSql += "and estado = @estado";

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@lista_minorista";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = 1;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@estado";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = "A";

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
                    iIdListaMinorista_P = 4;
                else
                    iIdListaMinorista_P = Convert.ToInt32(dtConsulta.Rows[0]["id_lista_precio"].ToString());

                sSql = "";
                sSql += "select DP.id_det_pedido, DP.id_producto, DP.precio_unitario, DP.valor_dscto," + Environment.NewLine;
                sSql += "DP.valor_iva, DP.valor_otro, P.paga_iva, PP.valor, CP.porcentaje_iva" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos CP INNER JOIN" + Environment.NewLine;
                sSql += "cv403_det_pedidos DP ON CP.id_pedido = DP.id_pedido" + Environment.NewLine;
                sSql += "and CP.estado = 'A'" + Environment.NewLine;
                sSql += "and DP.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "cv401_productos P ON P.id_producto = DP.id_producto" + Environment.NewLine;
                sSql += "and P.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "cv403_precios_productos PP ON P.id_producto = PP.id_producto" + Environment.NewLine;
                sSql += "and PP.estado = 'A'" + Environment.NewLine;
                sSql += "where CP.id_pedido = @id_pedido" + Environment.NewLine;
                sSql += "and PP.id_lista_precio = @id_lista_precio";

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_pedido";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdPedido;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@id_lista_precio";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = iIdListaMinorista_P;

                dtOriginal = new DataTable();
                dtOriginal.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtOriginal, sSql, parametro);

                if (bRespuesta == false)
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

        //FUNCION PARA OBTENER EL TOTAL
        private void obtenerTotal()
        {
            try
            {
                sSql = "";
                sSql += "select ltrim(str(sum(cantidad * (precio_unitario + valor_iva + valor_otro - valor_dscto)), 10, 2)) total," + Environment.NewLine;
                sSql += "ltrim(str(sum(cantidad * (precio_unitario - valor_dscto)), 10, 2)) subtotal," + Environment.NewLine;
                sSql += "ltrim(str(sum(cantidad * valor_iva), 10, 2)) iva" + Environment.NewLine;
                sSql += "from pos_vw_det_pedido" + Environment.NewLine;
                sSql += "where id_pedido = @id_pedido";

                parametro = new SqlParameter[1];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_pedido";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdPedido;

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

                dTotal = Convert.ToDecimal(dtConsulta.Rows[0]["total"].ToString());
                lblTotal.Text = "$ " + dTotal.ToString("N2");
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA OBTENER EL ID DE LA FORMA DE PAGO
        private void consultarIdFormaPago()
        {
            try
            {
                sSql = "";
                sSql += "select * from pos_vw_datos_forma_pago_facturacion" + Environment.NewLine;
                sSql += "where id_pedido = @id_pedido";

                parametro = new SqlParameter[1];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_pedido";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdPedido;

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
                    btnFacturar.Enabled = false;
                    iIdSriFormaPago = 0;
                    iIdTipoFormaCobro = 0;
                }

                else
                {
                    btnFacturar.Enabled = true;
                    iIdTipoFormaCobro = Convert.ToInt32(dtConsulta.Rows[0]["id_pos_tipo_forma_cobro"].ToString());
                    lblTipoComanda.Text = dtConsulta.Rows[0]["descripcion"].ToString();
                    sCodigoOrigenOrden = dtConsulta.Rows[0]["codigo"].ToString().Trim();
                    iIdSriFormaPago = Convert.ToInt32(dtConsulta.Rows[0]["id_sri_forma_pago"].ToString());
                }
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA CARGAR LA INFORMACION DEL CLIENTE
        private void cargarInformacionCliente()
        {
            try
            {
                sSql = "";
                sSql += "select * from pos_vw_cargar_informacion_cliente" + Environment.NewLine;
                sSql += "where id_pedido = @id_pedido";

                parametro = new SqlParameter[1];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_pedido";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdPedido;

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

                iIdPersona = Convert.ToInt32(dtConsulta.Rows[0]["id_persona"].ToString());
                iNumeroCuenta_P = Convert.ToInt32(dtConsulta.Rows[0]["cuenta"].ToString());
                iNumeroPedido_P = Convert.ToInt32(dtConsulta.Rows[0]["numero_pedido"].ToString());

                if (iIdPersona == Program.iIdPersona)
                {
                    txtIdentificacion.Text = "9999999999999";
                    txtApellidos.Text = "CONSUMIDOR FINAL";
                    txtTelefono.Text = "9999999999";
                    txtMail.Text = dtConsulta.Rows[0]["correo_electronico"].ToString();
                    txtDireccion.Text = "QUITO";
                    iIdPersona = Program.iIdPersona;
                    idTipoIdentificacion = 180;
                    idTipoPersona = 2447;
                    btnEditar.Visible = false;
                }

                else
                {
                    txtIdentificacion.Text = dtConsulta.Rows[0]["identificacion"].ToString();
                    txtApellidos.Text = (dtConsulta.Rows[0]["nombres"].ToString() + ' ' + dtConsulta.Rows[0]["apellidos"].ToString()).Trim().ToUpper();
                    txtMail.Text = dtConsulta.Rows[0]["correo_electronico"].ToString();
                    txtDireccion.Text = dtConsulta.Rows[0]["direccion_completa"].ToString();
                    sCiudad = dtConsulta.Rows[0]["direccion"].ToString();

                    if (dtConsulta.Rows[0]["telefono_domicilio"].ToString() != "")
                        txtTelefono.Text = dtConsulta.Rows[0]["telefono_domicilio"].ToString();
                    else if (dtConsulta.Rows[0]["celular"].ToString() != "")
                        txtTelefono.Text = dtConsulta.Rows[0]["celular"].ToString();
                    else
                        txtTelefono.Text = "";

                    btnEditar.Visible = true;
                }
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA EXTRAER LA INFORMACION PARA FACTURAR
        private void datosFactura()
        {
            try
            {
                sSql = "";
                sSql += "select L.id_localidad, L.establecimiento, L.punto_emision, " + Environment.NewLine;
                sSql += "P.numero_factura, P.numeronotaentrega, P.numeromovimientocaja, P.id_localidad_impresora" + Environment.NewLine;
                sSql += "from tp_localidades L, tp_localidades_impresoras P " + Environment.NewLine;
                sSql += "where L.id_localidad = P.id_localidad" + Environment.NewLine;
                sSql += "and L.id_localidad = @id_localidad" + Environment.NewLine;
                sSql += "and L.estado = @estado_1" + Environment.NewLine;
                sSql += "and P.estado = @estado_2";

                parametro = new SqlParameter[3];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_localidad";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = Program.iIdLocalidad;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@estado_1";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = "A";

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@estado_2";
                parametro[2].SqlDbType = SqlDbType.VarChar;
                parametro[2].Value = "A";

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
                    ok.lblMensaje.Text = "No se encuentran registros en la consulta.";
                    ok.ShowDialog();
                }

                else
                {
                    txtfacturacion.Text = dtConsulta.Rows[0]["establecimiento"].ToString() + "-" + dtConsulta.Rows[0]["punto_emision"].ToString();

                    sEstablecimiento = dtConsulta.Rows[0]["establecimiento"].ToString();
                    sPuntoEmision = dtConsulta.Rows[0]["punto_emision"].ToString();

                    if (rdbFactura.Checked)
                    {
                        TxtNumeroFactura.Text = dtConsulta.Rows[0]["numero_factura"].ToString();
                    }

                    else if (rdbNotaVenta.Checked)
                    {
                        TxtNumeroFactura.Text = dtConsulta.Rows[0]["numeronotaentrega"].ToString();
                    }

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

        #endregion

        #region FUNCIONES PARA INTEGRAR LA SEGUNDA VERSION DE GUARDAR LA COMANDA

        //FUNCION PARA CONTROLAR LA GENERACION DE COMANDAS
        private bool cobrarComanda_V2()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                int iFacturaElectronica_A = 0;

                if (Program.iFacturacionElectronica == 1)
                    iFacturaElectronica_A = 1;

                if (extraerFecha() == false)
                {
                    Cursor = Cursors.Default;
                    return false;
                }

                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    Cursor = Cursors.Default;
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Error al abrir transacción";
                    ok.ShowDialog();
                    return false;
                }

                sSql = "";
                sSql += "update tp_personas set" + Environment.NewLine;
                sSql += "correo_electronico = @correo_electronico" + Environment.NewLine;
                sSql += "where id_persona = @id_persona" + Environment.NewLine;
                sSql += "and estado = 'A'";

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@correo_electronico";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = txtMail.Text.Trim().ToLower();

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@id_persona";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = iIdPersona;

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    Cursor = Cursors.Default;
                    return false;
                }

                if (insertarPagos_V2() == false)
                {
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    Cursor = Cursors.Default;
                    return false;
                }

                if (insertarFactura_V2(iFacturaElectronica_A) == false)
                {
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    Cursor = Cursors.Default;
                    return false;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                crearReporte();

                return true;
            }

            catch (Exception ex)
            {
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                Cursor = Cursors.Default;
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return false;
            }
        }

        //FUNCION PARA CREAR EL REPORTE
        private void crearReporte()
        {
            try
            {
                Program.iCortar = 1;
                dbCambio = 0;

                if (rdbFactura.Checked)
                {
                    if (Program.iEjecutarImpresion == 1)
                    {
                        ReportesTextBox.frmVistaFactura frmVistaFactura = new ReportesTextBox.frmVistaFactura(iIdFactura, 1, 1);
                        frmVistaFactura.ShowDialog();

                        if (frmVistaFactura.DialogResult == DialogResult.OK)
                        {
                            this.DialogResult = DialogResult.OK;
                            Cambiocs cambiocs = new Cambiocs("$ " + dbCambio.ToString("N2"));
                            cambiocs.lblVerMensaje.Text = "FACTURA GENERADA" + Environment.NewLine + "ÉXITOSAMENTE";
                            cambiocs.ShowDialog();
                            Program.sIDPERSONA = null;
                            Program.dbValorPorcentaje = 0;
                            Program.dbDescuento = 0.0;
                            frmVistaFactura.Close();

                            this.DialogResult = DialogResult.OK;
                        }
                    }

                    else
                    {
                        this.DialogResult = DialogResult.OK;
                        Cambiocs cambiocs = new Cambiocs("$ " + dbCambio.ToString("N2"));
                        cambiocs.lblVerMensaje.Text = "FACTURA GENERADA" + Environment.NewLine + "ÉXITOSAMENTE";
                        cambiocs.ShowDialog();

                        Program.sIDPERSONA = null;
                        Program.dbValorPorcentaje = 0;
                        Program.dbDescuento = 0;

                        this.DialogResult = DialogResult.OK;
                    }
                }

                else if (rdbNotaVenta.Checked == true)
                {
                    if (Program.iEjecutarImpresion == 1)
                    {
                        ReportesTextBox.frmVerNotaVentaFactura notaVenta = new ReportesTextBox.frmVerNotaVentaFactura(iIdPedido.ToString(), 1);
                        notaVenta.ShowDialog();

                        if (notaVenta.DialogResult == DialogResult.OK)
                        {
                            this.DialogResult = DialogResult.OK;

                            Cambiocs ok = new Cambiocs("$ " + Program.dCambioPantalla.ToString("N2"));
                            ok.lblVerMensaje.Text = "NOTA DE ENTREGA GENERADA";
                            ok.ShowDialog();

                            Program.sIDPERSONA = null;
                            Program.dbValorPorcentaje = 0;
                            Program.dbDescuento = 0;
                            notaVenta.Close();

                            this.DialogResult = DialogResult.OK;
                        }
                    }

                    else
                    {
                        this.DialogResult = DialogResult.OK;

                        Cambiocs ok = new Cambiocs("$ " + Program.dCambioPantalla.ToString("N2"));
                        ok.lblVerMensaje.Text = "NOTA DE ENTREGA GENERADA";
                        ok.ShowDialog();

                        Program.sIDPERSONA = null;
                        Program.dbValorPorcentaje = 0;
                        Program.dbDescuento = 0;

                        this.DialogResult = DialogResult.OK;
                    }
                }
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();

                if (ok.DialogResult == DialogResult.OK)
                {
                    Program.sIDPERSONA = null;
                    //actualizarNumeroFactura();
                    Program.dbValorPorcentaje = 0;
                    Program.dbDescuento = 0;

                    this.DialogResult = DialogResult.OK;
                }
            }
        }

        //FUNCION PARA EXTRAER LA FECHA DEL SISTEMA
        private bool extraerFecha()
        {
            try
            {
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
                    return false;
                }

                sFecha = Convert.ToDateTime(dtConsulta.Rows[0]["fecha"].ToString()).ToString("yyyy-MM-dd");
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

        //FUNCION PARA ENVIAR LOS PARAMETROS- INSERTAR NUEVOS PAGOS
        private bool insertarPagos_V2()
        {
            try
            {
                if (crearTablaPagos() == false)
                    return false;

                comanda = new Clases_Crear_Comandas.ClaseCrearComanda();

                bRespuesta = comanda.insertarPagos(iIdPedido, dtPagos, dTotal, 0, 0,
                                                   iIdPersona, sFecha, Program.iIdLocalidad, 0, conexion);

                if (bRespuesta == false)
                {
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = comanda.sMensajeError;
                    catchMensaje.ShowDialog();
                    return false;
                }

                iIdDocumentoPorCobrar = comanda.iIdDocumentoCobrar;

                return true;
            }

            catch (Exception ex)
            {
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return false;
            }
        }

        //FUNCION PARA ENVIAR LOS PARAMETROS- INSERTAR FACTURA
        private bool insertarFactura_V2(int iFacturaElectronica_P)
        {
            try
            {
                comanda = new Clases_Crear_Comandas.ClaseCrearComanda();

                bRespuesta = comanda.insertarFactura(iIdPedido, iIdTipoComprobante, iFacturaElectronica_P,
                                                     iIdPersona, Program.iIdLocalidad, dtPagos, dTotal, 0,
                                                     0, 0, 1,  sFecha, iIdDocumentoPorCobrar, conexion);

                if (bRespuesta == false)
                {
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

                return true;
            }

            catch (Exception ex)
            {
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
                dtPagos.Columns.Add("conciliacion");
                dtPagos.Columns.Add("id_operador_tarjeta");
                dtPagos.Columns.Add("id_tipo_tarjeta");
                dtPagos.Columns.Add("numero_lote");
                dtPagos.Columns.Add("bandera_insertar_lote");
                dtPagos.Columns.Add("propina");
                dtPagos.Columns.Add("codigo_metodo_pago");
                dtPagos.Columns.Add("numero_documento");
                dtPagos.Columns.Add("fecha_vcto");
                dtPagos.Columns.Add("cg_banco");
                dtPagos.Columns.Add("numero_cuenta");
                dtPagos.Columns.Add("titular");

                dtPagos.Rows.Add(iIdTipoFormaCobro, sDescripcionFormaPago, dTotal, iIdSriFormaPago_P, 0, 0, 0, "", 0, 0,
                                 sCodigoMetodoPago, "", "", "0", "", "");

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

        //FUNCION PARA OBTENER LOS VALORES PARA INSERTAR EN LA SECCION DE PAGOS
        private bool obtenerDatosFormaPagoRealizada(int iIdPosTipoFormaCobro)
        {
            try
            {
                sSql = "";
                sSql += "select * from pos_vw_obtener_datos_formas_pagos" + Environment.NewLine;
                sSql += "where id_localidad = @id_localidad" + Environment.NewLine;
                sSql += "and id_pos_tipo_forma_cobro = @id_pos_tipo_forma_cobro";

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_localidad";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = Program.iIdLocalidad;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@id_pos_tipo_forma_cobro";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = iIdPosTipoFormaCobro;

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
                    ok.lblMensaje.Text = "No se encuentran configurados los registros de cobros. Favor comuníquese con el administrador.";
                    ok.ShowDialog();
                    return false;
                }

                iIdTipoFormaCobro = Convert.ToInt32(dtConsulta.Rows[0]["id_pos_tipo_forma_cobro"].ToString());
                sDescripcionFormaPago = dtConsulta.Rows[0]["descripcion"].ToString().Trim().ToUpper();
                iIdSriFormaPago_P = Convert.ToInt32(dtConsulta.Rows[0]["id_sri_forma_pago"].ToString());
                sCodigoMetodoPago = dtConsulta.Rows[0]["codigo"].ToString().Trim().ToUpper();

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

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCobrarRepartidorExterno_Load(object sender, EventArgs e)
        {
            extraerListaMinorista();
            obtenerTotal();
            cargarInformacionCliente();
            consultarIdFormaPago();
            datosFactura();
            this.ActiveControl = txtIdentificacion;
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

        private void btnFacturar_Click(object sender, EventArgs e)
        {
            if (iIdPersona == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor ingrese el número de identificación.";
                ok.ShowDialog();
                txtIdentificacion.Focus();
                return;
            }

            if (txtIdentificacion.Text.Trim() == "")
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor ingrese el número de identificación.";
                ok.ShowDialog();
                txtIdentificacion.Focus();
                return;
            }

            if (txtMail.Text.Trim() == "")
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor ingrese el correo electrónico.";
                ok.ShowDialog();
                txtMail.ReadOnly = false;
                txtMail.Focus();
                return;
            }

            if (obtenerDatosFormaPagoRealizada(iIdTipoFormaCobro) == false)
                return;

            if (rdbFactura.Checked)
                iIdTipoComprobante = 1;
            else if (rdbNotaVenta.Checked)
                iIdTipoComprobante = Program.iComprobanteNotaEntrega;

            cobrarComanda_V2();
        }

        private void frmCobrarRepartidorExterno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }

            if (Program.iPermitirAbrirCajon == 1)
            {
                if (e.KeyCode == Keys.F7)
                {
                    if (Program.iPuedeCobrar == 1)
                    {
                        abrir.consultarImpresoraAbrirCajon();
                    }
                }
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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Facturador.frmControlDatosCliente controlDatosCliente = new Facturador.frmControlDatosCliente();
            controlDatosCliente.ShowDialog();

            if (controlDatosCliente.DialogResult == DialogResult.OK)
            {
                iIdPersona = controlDatosCliente.iCodigo;
                txtIdentificacion.Text = controlDatosCliente.sIdentificacion;
                consultarRegistro();
                controlDatosCliente.Close();
            }
        }

        private void btnConsumidorFinal_Click(object sender, EventArgs e)
        {
            txtIdentificacion.Text = "9999999999999";
            txtApellidos.Text = "CONSUMIDOR FINAL";
            txtTelefono.Text = "9999999999";
            txtMail.Text = Program.sCorreoElectronicoDefault;
            txtDireccion.Text = "QUITO";
            iIdPersona = Program.iIdPersona;
            idTipoIdentificacion = 180;
            idTipoPersona = 2447;
            btnEditar.Visible = false;
        }

        private void txtIdentificacion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtIdentificacion.Text != "")
                {
                    //AQUI INSTRUCCIONES PARA CONSULTAR Y VALIDAR LA CEDULA
                    if ((esNumero(txtIdentificacion.Text.Trim()) == true) && (chkPasaporte.Checked == false))
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

        private void btnPrecuenta_Click(object sender, EventArgs e)
        {
            Pedidos.frmVerPrecuentaTextBox precuenta = new Pedidos.frmVerPrecuentaTextBox(iIdPedido.ToString(), 1, "Pre-Cuenta");
            precuenta.ShowDialog();
            return;
        }

        private void rdbFactura_CheckedChanged(object sender, EventArgs e)
        {
            datosFactura();
        }

        private void rdbNotaVenta_CheckedChanged(object sender, EventArgs e)
        {
            datosFactura();
        }

        private void btnEditarFactura_Click(object sender, EventArgs e)
        {
            if (TxtNumeroFactura.ReadOnly == true)
            {
                sNumeroFactura = TxtNumeroFactura.Text.Trim();
                TxtNumeroFactura.ReadOnly = false;
                TxtNumeroFactura.Focus();
            }

            else
            {
                TxtNumeroFactura.Text = sNumeroFactura;
                TxtNumeroFactura.ReadOnly = true;
                txtIdentificacion.Focus();
            }
        }
    }
}
