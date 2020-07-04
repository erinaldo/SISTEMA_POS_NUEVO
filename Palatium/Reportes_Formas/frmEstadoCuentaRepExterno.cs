using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Palatium.Reportes_Formas
{
    public partial class frmEstadoCuentaRepExterno : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeNuevoOk ok;
        VentanasMensajes.frmMensajeNuevoSiNo SiNo;
        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;

        Clases_Factura_Electronica.ClaseConsultarXML consultar;
        Clases.ClaseExcel exportarExcel;

        XmlDocument xmlAut;
        XDocument xml;
        XElement autorizacion;
        XmlDocument miXML;

        DataGridView dgvExportar;

        string sSql;
        string sRutaPlantilla;
        string sFecha;
        string sFechaDesde;
        string sFechaHasta;
        string sDirGenerados;
        string sDirFirmados;
        string sDirAutorizados;
        string sDirNoAutorizados;
        string sVersion = "1.0";
        string sUTF = "utf-8";
        string sStandAlone = "yes";

        DataTable dtConsulta;
        DataTable dtLocalidades;

        bool bRespuesta;

        int iBanderaComprobantesElectronicos;

        SqlParameter[] parametro;

        //VARIABLES PARA CARGAR LOS PARAMETROS DE ENVIO DEL MAIL
        string P_St_correo_server_smtp;
        string P_St_from;
        string P_St_fromname;
        string P_St_correo_que_envia;
        string P_St_correo_con_copia;
        string P_St_correo_consumidor_final;
        string P_St_correo_ambiente_prueba;
        string P_St_correo_palabra_clave;
        long P_Ln_correo_puerto_smtp;
        int P_In_maneja_SSL;
        string P_St_telefono_empresa;
        string P_St_nombre_comercial;
        string sMensajeEnviar;
        string sTipoComprobante;
        string sMensajeRetorno;
        string sCorreoCliente;
        string sAsuntoMail;
        string srutaXML;
        string srutaRIDE;
        string sRutaAdjuntos;
        string sWSEnvioPruebas;
        string sWSConsultaPruebas;
        string sWSEnvioProduccion;
        string sWSConsultaProduccion;
        string sWebService;

        public frmEstadoCuentaRepExterno()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMBOBOX DE LOCALIDADES
        private void llenarComboLocalidades()
        {
            try
            {
                sSql = "";
                sSql += "select id_localidad, nombre_localidad, emite_comprobante_electronico" + Environment.NewLine;
                sSql += "from tp_vw_localidades";

                dtLocalidades = new DataTable();
                dtLocalidades.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtLocalidades, sSql);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                DataRow row = dtLocalidades.NewRow();
                row["id_localidad"] = "0";
                row["nombre_localidad"] = "Seleccione...!!!";
                dtLocalidades.Rows.InsertAt(row, 0);

                cmbLocalidades.DisplayMember = "nombre_localidad";
                cmbLocalidades.ValueMember = "id_localidad";
                cmbLocalidades.DataSource = dtLocalidades;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE REPARTIDRES EXTERNOS
        private void llenarComboRepartidoresExternos()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_origen_orden, descripcion" + Environment.NewLine;
                sSql += "from pos_origen_orden" + Environment.NewLine;
                sSql += "where repartidor_externo = @repartidor_externo" + Environment.NewLine;
                sSql += "and estado = @estado" + Environment.NewLine;
                sSql += "order by descripcion";

                #region PARAMETROS

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@repartidor_externo";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = 0;

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
                    return;
                }

                DataRow row = dtConsulta.NewRow();
                row["id_pos_origen_orden"] = "0";
                row["descripcion"] = "Seleccione...!!!";
                dtConsulta.Rows.InsertAt(row, 0);

                cmbRepartidores.DisplayMember = "descripcion";
                cmbRepartidores.ValueMember = "id_pos_origen_orden";
                cmbRepartidores.DataSource = dtConsulta;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid()
        {
            try
            {
                dgvDatos.Rows.Clear();

                sSql = "";
                sSql += "select id_factura, fecha_factura, numero_pedido, establecimiento, punto_emision," + Environment.NewLine;
                sSql += "numero_factura, identificacion, ltrim(isnull(nombres, '') + ' ' + apellidos) cliente," + Environment.NewLine;
                sSql += "clave_acceso, autorizacion, facturaelectronica, id_tipo_ambiente, id_tipo_emision," + Environment.NewLine;
                sSql += "isnull(sum(case paga_iva when 0 then cantidad * precio_unitario end), 0) base_iva_cero," + Environment.NewLine;
                sSql += "isnull(sum(case paga_iva when 1 then cantidad * precio_unitario end), 0) base_iva," + Environment.NewLine;
                sSql += "isnull(sum(cantidad * valor_dscto), 0) valor_descuento," + Environment.NewLine;
                sSql += "isnull(sum(cantidad * valor_iva), 0) valor_iva," + Environment.NewLine;
                sSql += "isnull(sum(cantidad * valor_otro), 0) valor_servicio," + Environment.NewLine;
                sSql += "isnull(sum(cantidad * (precio_unitario - valor_dscto + valor_iva + valor_otro)), 0) total" + Environment.NewLine;
                sSql += "from pos_vw_factura" + Environment.NewLine;
                sSql += "where id_localidad = @id_localidad" + Environment.NewLine;
                sSql += "and idtipocomprobante = @idtipocomprobante" + Environment.NewLine;
                sSql += "and id_pos_origen_orden = @id_pos_origen_orden" + Environment.NewLine;
                sSql += "and fecha_factura between @fecha_inicio" + Environment.NewLine;
                sSql += "and @fecha_final" + Environment.NewLine;
                sSql += "group by id_factura, id_localidad, fecha_factura, numero_pedido, establecimiento," + Environment.NewLine;
                sSql += "punto_emision, numero_factura, identificacion, nombres, apellidos, clave_acceso," + Environment.NewLine;
                sSql += "autorizacion, facturaelectronica, id_tipo_ambiente, id_tipo_emision" + Environment.NewLine;
                sSql += "order by numero_factura";

                #region PARAMETROS

                int a = 0;
                parametro = new SqlParameter[5];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_localidad";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Convert.ToInt32(cmbLocalidades.SelectedValue);
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@idtipocomprobante";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 1;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pos_origen_orden";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Convert.ToInt32(cmbRepartidores.SelectedValue);
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@fecha_inicio";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = sFechaDesde;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@fecha_final";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = sFechaHasta;

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
                    ok.lblMensaje.Text = "No existen registros con los parámetros ingresados.";
                    ok.ShowDialog();
                    return;
                }

                if (iBanderaComprobantesElectronicos == 1)
                    btnSincronizar.Visible = true;
                else
                    btnSincronizar.Visible = false;

                btnExcel.Visible = true;

                string sEstablecimiento_P;
                string sPuntoEmision_P;
                string sNumeroFactura_P;
                string sNumeroAutorizacion_P;
                int iFacturaElectronica_P;
                bool bEstadoFacElectronica_P;
                bool bEstadoAutorizado_P;
                int iCuentaNoEnviados = 0;

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    bEstadoFacElectronica_P = false;
                    bEstadoAutorizado_P = false;

                    sEstablecimiento_P = dtConsulta.Rows[i]["establecimiento"].ToString().Trim();
                    sPuntoEmision_P = dtConsulta.Rows[i]["punto_emision"].ToString().Trim();
                    sNumeroFactura_P = dtConsulta.Rows[i]["numero_factura"].ToString().Trim().PadLeft(9, '0');
                    sNumeroAutorizacion_P = dtConsulta.Rows[i]["autorizacion"].ToString().Trim();
                    iFacturaElectronica_P = Convert.ToInt32(dtConsulta.Rows[i]["facturaelectronica"].ToString());

                    if (iFacturaElectronica_P == 1)
                    {
                        bEstadoFacElectronica_P = true;

                        if (sNumeroAutorizacion_P != "")
                            bEstadoAutorizado_P = true;
                        else
                            iCuentaNoEnviados++;
                    }

                    dgvDatos.Rows.Add(dtConsulta.Rows[i]["id_factura"].ToString().Trim(),
                                      sEstablecimiento_P,
                                      sPuntoEmision_P,
                                      dtConsulta.Rows[i]["numero_factura"].ToString().Trim(),
                                      dtConsulta.Rows[i]["clave_acceso"].ToString().Trim(),
                                      "Fac",
                                      dtConsulta.Rows[i]["id_tipo_ambiente"].ToString().Trim(),
                                      dtConsulta.Rows[i]["id_tipo_emision"].ToString().Trim(),
                                      bEstadoFacElectronica_P,
                                      bEstadoAutorizado_P,
                                      Convert.ToDateTime(dtConsulta.Rows[i]["fecha_factura"].ToString()).ToString("dd-MM-yyyy"),
                                      dtConsulta.Rows[i]["numero_pedido"].ToString().Trim(),
                                      sEstablecimiento_P + "-" + sPuntoEmision_P + "-" + sNumeroFactura_P,
                                      dtConsulta.Rows[i]["identificacion"].ToString().Trim(),
                                      dtConsulta.Rows[i]["cliente"].ToString().Trim(),
                                      Convert.ToDecimal(dtConsulta.Rows[i]["base_iva_cero"].ToString()).ToString("N2"),
                                      Convert.ToDecimal(dtConsulta.Rows[i]["base_iva"].ToString()).ToString("N2"),
                                      Convert.ToDecimal(dtConsulta.Rows[i]["valor_descuento"].ToString()).ToString("N2"),
                                      Convert.ToDecimal(dtConsulta.Rows[i]["valor_iva"].ToString()).ToString("N2"),
                                      Convert.ToDecimal(dtConsulta.Rows[i]["valor_servicio"].ToString()).ToString("N2"),
                                      Convert.ToDecimal(dtConsulta.Rows[i]["total"].ToString()).ToString("N2"),
                                      "0.00", "0.00",
                                      Convert.ToDecimal(dtConsulta.Rows[i]["total"].ToString()).ToString("N2")
                        );
                }

                dgvDatos.ClearSelection();

                if (iCuentaNoEnviados > 0)
                {
                    SiNo = new VentanasMensajes.frmMensajeNuevoSiNo();
                    SiNo.lblMensaje.Text = "Existen comprobantes sin autorizar. ¿Desea proceder con la autorización?";
                    SiNo.ShowDialog();

                    if (SiNo.DialogResult == DialogResult.OK)
                    {

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

        //FUNCION DE LAS COLUMAS DEL DATAGRID
        private void columasGrid(bool ok)
        {
            try
            {
                dgvDatos.Columns["facturaelectronica"].Visible = ok;
                dgvDatos.Columns["autorizado"].Visible = ok;
                dgvDatos.Columns["sri"].Visible = ok;
                dgvDatos.Columns["diferencia"].Visible = ok;
                dgvDatos.Columns["total_orden"].Visible = ok;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            fechaSistema();
            llenarComboLocalidades();
            llenarComboRepartidoresExternos();
            columasGrid(false);
            dgvDatos.Rows.Clear();

            btnSincronizar.Visible = false;
            btnExcel.Visible = false;

            cmbLocalidades.Focus();
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

        //CARGAR EL DIRECTORIO DONDE SE GUARDARAN LOS XML GENERADOS
        private bool buscarDirectorio()
        {
            try
            {
                sSql = "";
                sSql += "select codigo, nombres" + Environment.NewLine;
                sSql += "from cel_directorio" + Environment.NewLine;
                sSql += "where id_tipo_comprobante = @id_tipo_comprobante" + Environment.NewLine;
                sSql += "and estado = @estado" + Environment.NewLine;
                sSql += "order by orden";

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_tipo_comprobante";
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
                    return false;
                }

                if (dtConsulta.Rows.Count > 0)
                {
                    sDirGenerados = dtConsulta.Rows[0]["nombres"].ToString();
                    sDirFirmados = dtConsulta.Rows[1]["nombres"].ToString();
                    sDirAutorizados = dtConsulta.Rows[2]["nombres"].ToString();
                    sDirNoAutorizados = dtConsulta.Rows[3]["nombres"].ToString();

                    if (!Directory.Exists(sDirGenerados))
                    {
                        DirectoryInfo generado = Directory.CreateDirectory(sDirGenerados);
                    }

                    if (!Directory.Exists(sDirFirmados))
                    {
                        DirectoryInfo firmado = Directory.CreateDirectory(sDirFirmados);
                    }

                    if (!Directory.Exists(sDirAutorizados))
                    {
                        DirectoryInfo autorizado = Directory.CreateDirectory(sDirAutorizados);
                    }

                    if (!Directory.Exists(sDirNoAutorizados))
                    {
                        DirectoryInfo no_autorizado = Directory.CreateDirectory(sDirNoAutorizados);
                    }

                    return true;
                }

                else
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "No existe una configuracion de directorio para guardar los xml genereados.";
                    ok.ShowDialog();
                    return false;
                }
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return false;
            }
        }

        //FUNCION PARA CARGAR LOS PARAMETROS DE ENVIO DEL CORREO CON LOS ARCHIVOS ADJUNTOS
        public void traerparametrosComprobantesElectronicos()
        {
            try
            {
                P_St_correo_que_envia = "";
                P_Ln_correo_puerto_smtp = 0;

                sSql = "";
                sSql += "select correo_que_envia, correo_con_copia," + Environment.NewLine;
                sSql += "correo_consumidor_final,correo_ambiente_prueba,correo_palabra_clave," + Environment.NewLine;
                sSql += "correo_smtp,correo_puerto, maneja_SSL, wsdl_pruebas," + Environment.NewLine;
                sSql += "url_pruebas, wsdl_produccion, url_produccion" + Environment.NewLine;
                sSql += "from cel_parametro" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        P_St_correo_que_envia = conexion.GFun_Va_Valor_Defecto(dtConsulta.Rows[0][0].ToString(), "");
                        P_St_from = P_St_correo_que_envia;
                        P_St_correo_con_copia = conexion.GFun_Va_Valor_Defecto(dtConsulta.Rows[0][1].ToString(), "");
                        P_St_correo_consumidor_final = conexion.GFun_Va_Valor_Defecto(dtConsulta.Rows[0][2].ToString(), "");
                        P_St_correo_ambiente_prueba = conexion.GFun_Va_Valor_Defecto(dtConsulta.Rows[0][3].ToString(), "");
                        P_St_correo_palabra_clave = conexion.GFun_Va_Valor_Defecto(dtConsulta.Rows[0][4].ToString(), "");
                        P_St_correo_server_smtp = conexion.GFun_Va_Valor_Defecto(dtConsulta.Rows[0][5].ToString(), "");
                        P_Ln_correo_puerto_smtp = Convert.ToInt64(conexion.GFun_Va_Valor_Defecto(dtConsulta.Rows[0][6].ToString(), "0"));
                        P_In_maneja_SSL = Convert.ToInt32(conexion.GFun_Va_Valor_Defecto(dtConsulta.Rows[0][7].ToString(), "0"));
                        sWSEnvioPruebas = dtConsulta.Rows[0]["wsdl_pruebas"].ToString();
                        sWSConsultaPruebas = dtConsulta.Rows[0]["url_pruebas"].ToString();
                        sWSEnvioProduccion = dtConsulta.Rows[0]["wsdl_produccion"].ToString();
                        sWSConsultaProduccion = dtConsulta.Rows[0]["url_produccion"].ToString();
                    }
                }

                else
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                //==================================================================================================
                sSql = "";
                sSql += "select telefono, nombrecomercial" + Environment.NewLine;
                sSql += "from sis_empresa" + Environment.NewLine;
                sSql += "where idempresa = " + Program.iIdEmpresa;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        P_St_telefono_empresa = conexion.GFun_Va_Valor_Defecto(dtConsulta.Rows[0][0].ToString(), "");
                        P_St_nombre_comercial = conexion.GFun_Va_Valor_Defecto(dtConsulta.Rows[0][1].ToString(), "");
                    }
                }

                else
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //DESCARGAR LOS COMPROBANTES
        private bool descargarComprobantes()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                string _sObtenerClaveAcceso;
                string _sSecuencia;
                string _sNombreDocumento;
                string _sWebServiceEnviar;
                string _sImporteTotal;

                Decimal _dbValorSistema;
                Decimal _dbValorSRI;
                Decimal _dbValorDiferencia;

                bool _bAutorizado;
                bool _bFacturaElectronica;

                for (int i = 0; i < dgvDatos.Rows.Count; i++)
                {
                    _bAutorizado = Convert.ToBoolean(dgvDatos.Rows[i].Cells["autorizado"].Value);
                    _bFacturaElectronica = Convert.ToBoolean(dgvDatos.Rows[i].Cells["facturaelectronica"].Value);

                    if (_bFacturaElectronica == false)
                        goto continuar;

                    if (_bAutorizado == false)
                        goto continuar;

                    if (Convert.ToInt32(dgvDatos.Rows[i].Cells["id_tipo_ambiente"].Value) == 1)
                        _sWebServiceEnviar = sWSConsultaPruebas;
                    else
                        _sWebServiceEnviar = sWSConsultaProduccion;

                    _dbValorSistema = Convert.ToDecimal(dgvDatos.Rows[i].Cells["total"].Value);

                    _sObtenerClaveAcceso = dgvDatos.Rows[i].Cells["clave_acceso"].Value.ToString();
                    _sSecuencia = _sObtenerClaveAcceso.Substring(24, 15);

                    _sNombreDocumento = sDirAutorizados + @"\F" + _sSecuencia + ".xml";
                    consultar = new Clases_Factura_Electronica.ClaseConsultarXML();

                    RespuestaSRI respuesta = consultar.AutorizacionComprobante(out xmlAut, _sObtenerClaveAcceso, _sWebServiceEnviar);

                    if (respuesta != null)
                    {
                        miXML = new XmlDocument();
                        miXML.LoadXml(respuesta.Comprobante);
                        XmlNodeList nodo;

                        nodo = miXML.GetElementsByTagName("infoFactura");

                        foreach (XmlNode elemento in nodo)
                        {
                            _sImporteTotal = elemento.SelectSingleNode("importeTotal").InnerText;
                            dgvDatos.Rows[i].Cells["sri"].Value = _sImporteTotal;

                            _dbValorSRI = Convert.ToDecimal(_sImporteTotal);
                            _dbValorDiferencia = _dbValorSistema - _dbValorSRI;
                            dgvDatos.Rows[i].Cells["diferencia"].Value = _dbValorDiferencia.ToString("N2");
                        }
                    }

                    continuar: { }
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

                DataGridViewTextBoxColumn fecha_factura = new DataGridViewTextBoxColumn();
                fecha_factura.HeaderText = "FECHA FACTURA";
                fecha_factura.Name = "fecha_factura";

                DataGridViewTextBoxColumn numero_pedido = new DataGridViewTextBoxColumn();
                numero_pedido.HeaderText = "No. PEDIDO";
                numero_pedido.Name = "numero_pedido";

                DataGridViewTextBoxColumn factura = new DataGridViewTextBoxColumn();
                factura.HeaderText = "No. FACTURA";
                factura.Name = "factura";

                DataGridViewTextBoxColumn identificacion = new DataGridViewTextBoxColumn();
                identificacion.HeaderText = "IDENTIFICACIÓN";
                identificacion.Name = "identificacion";

                DataGridViewTextBoxColumn cliente = new DataGridViewTextBoxColumn();
                cliente.HeaderText = "CLIENTE";
                cliente.Name = "cliente";

                DataGridViewTextBoxColumn base_iva_cero = new DataGridViewTextBoxColumn();
                base_iva_cero.HeaderText = "BASE IVA 0";
                base_iva_cero.Name = "base_iva_cero";

                DataGridViewTextBoxColumn base_iva = new DataGridViewTextBoxColumn();
                base_iva.HeaderText = "BASE IVA";
                base_iva.Name = "base_iva";

                DataGridViewTextBoxColumn valor_descuento = new DataGridViewTextBoxColumn();
                valor_descuento.HeaderText = "DESCUENTO";
                valor_descuento.Name = "valor_descuento";

                DataGridViewTextBoxColumn valor_iva = new DataGridViewTextBoxColumn();
                valor_iva.HeaderText = "IVA";
                valor_iva.Name = "valor_iva";

                DataGridViewTextBoxColumn valor_servicio = new DataGridViewTextBoxColumn();
                valor_servicio.HeaderText = "SERVICIO";
                valor_servicio.Name = "valor_servicio";

                DataGridViewTextBoxColumn total = new DataGridViewTextBoxColumn();
                total.HeaderText = "TOTAL FACTURA";
                total.Name = "total";

                DataGridViewTextBoxColumn sri = new DataGridViewTextBoxColumn();
                sri.HeaderText = "SRI";
                sri.Name = "sri";

                DataGridViewTextBoxColumn diferencia = new DataGridViewTextBoxColumn();
                diferencia.HeaderText = "DIFERENCIA";
                diferencia.Name = "diferencia";

                DataGridViewTextBoxColumn total_orden = new DataGridViewTextBoxColumn();
                total_orden.HeaderText = "TOTAL ORDEN";
                total_orden.Name = "total_orden";

                dgvExportar.Columns.Add(fecha_factura);
                dgvExportar.Columns.Add(numero_pedido);
                dgvExportar.Columns.Add(factura);
                dgvExportar.Columns.Add(identificacion);
                dgvExportar.Columns.Add(cliente);
                dgvExportar.Columns.Add(base_iva_cero);
                dgvExportar.Columns.Add(base_iva);
                dgvExportar.Columns.Add(valor_descuento);
                dgvExportar.Columns.Add(valor_iva);
                dgvExportar.Columns.Add(valor_servicio);
                dgvExportar.Columns.Add(total);
                dgvExportar.Columns.Add(sri);
                dgvExportar.Columns.Add(diferencia);
                dgvExportar.Columns.Add(total_orden);

                for (int i = 0; i < dgvDatos.Rows.Count; i++)
                {
                    dgvExportar.Rows.Add(
                                            dgvDatos.Rows[i].Cells["fecha_factura"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["numero_pedido"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["factura"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["identificacion"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["cliente"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["base_iva_cero"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["base_iva"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["valor_descuento"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["valor_iva"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["valor_servicio"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["total"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["sri"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["diferencia"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["total_orden"].Value.ToString()
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

        //FUNCION PARA LLENAR EL EXCEL
        private void exportarGridExcel_V2()
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook libro;
                Microsoft.Office.Interop.Excel.Worksheet hoja;

                libro = excel.Workbooks.Open(sRutaPlantilla, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                hoja = (Microsoft.Office.Interop.Excel.Worksheet)libro.ActiveSheet;

                int iFila = 2;

                for (int i = 0; i < dgvDatos.Rows.Count; i++)
                {
                    hoja.Cells[iFila, 1] = dgvDatos.Rows[i].Cells["fecha_factura"].Value.ToString();
                    hoja.Cells[iFila, 2] = dgvDatos.Rows[i].Cells["numero_pedido"].Value.ToString();
                    hoja.Cells[iFila, 3] = dgvDatos.Rows[i].Cells["factura"].Value.ToString();
                    hoja.Cells[iFila, 4] = dgvDatos.Rows[i].Cells["identificacion"].Value.ToString();
                    hoja.Cells[iFila, 5] = dgvDatos.Rows[i].Cells["cliente"].Value.ToString();
                    hoja.Cells[iFila, 6] = dgvDatos.Rows[i].Cells["base_iva_cero"].Value.ToString();
                    hoja.Cells[iFila, 7] = dgvDatos.Rows[i].Cells["base_iva"].Value.ToString();
                    hoja.Cells[iFila, 8] = dgvDatos.Rows[i].Cells["valor_descuento"].Value.ToString();
                    hoja.Cells[iFila, 9] = dgvDatos.Rows[i].Cells["valor_iva"].Value.ToString();
                    hoja.Cells[iFila, 10] = dgvDatos.Rows[i].Cells["valor_servicio"].Value.ToString();
                    hoja.Cells[iFila, 11] = dgvDatos.Rows[i].Cells["total"].Value.ToString();
                    hoja.Cells[iFila, 12] = dgvDatos.Rows[i].Cells["sri"].Value.ToString();
                    hoja.Cells[iFila, 13] = dgvDatos.Rows[i].Cells["diferencia"].Value.ToString();
                    hoja.Cells[iFila, 14] = dgvDatos.Rows[i].Cells["total_orden"].Value.ToString();
                    iFila++;
                }

                excel.Visible = true;
                excel.UserControl = true;
                excel.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlMaximized;
                excel = null;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        private void frmEstadoCuentaRepExterno_Load(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbLocalidades.SelectedValue) == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor seleccione la localidad.";
                ok.ShowDialog();
                cmbLocalidades.Focus();
                return;
            }

            if (Convert.ToInt32(cmbRepartidores.SelectedValue) == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor seleccione el repartidor externo.";
                ok.ShowDialog();
                cmbRepartidores.Focus();
                return;
            }

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

            int iIdLocalidad_P = Convert.ToInt32(cmbLocalidades.SelectedValue);
            DataRow[] fila = dtLocalidades.Select("id_localidad = " + iIdLocalidad_P);

            if (fila.Length != 0)
            {
                if (Convert.ToInt32(fila[0][2].ToString()) == 1)
                    iBanderaComprobantesElectronicos = 1;
                else
                    iBanderaComprobantesElectronicos = 0;
            }

            else
                iBanderaComprobantesElectronicos = 0;

            if (iBanderaComprobantesElectronicos == 1)
            {
                columasGrid(true);
                traerparametrosComprobantesElectronicos();
                buscarDirectorio();
            }
            else
                columasGrid(false);

            llenarGrid();
        }

        private void frmEstadoCuentaRepExterno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnSincronizar_Click(object sender, EventArgs e)
        {
            descargarComprobantes();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (dgvDatos.Rows.Count == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "No existen registros para realizar la exportación de datos.";
                ok.ShowDialog();
                return;
            }

            if (Program.sUrlReportes == "")
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "No se encuentra configurado la ubicación para los reportes.";
                ok.ShowDialog();
                return;
            }

            sRutaPlantilla = Program.sUrlReportes.Trim() + @"\cuadre_repartidores_externos.xls";

            if (!File.Exists(sRutaPlantilla))
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "No se encuentra el archivo para generar el informe.";
                ok.ShowDialog();
                return;
            }

            exportarGridExcel_V2();
        }
    }
}
