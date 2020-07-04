using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palatium.Clases_Crear_Comandas
{
    class ClaseEliminarComandaReabrir
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        string sSql;
        string sMotivo;
        string sFecha;
        string sTabla;
        string sCampo;
        public string sMensajeError;

        DataTable dtConsulta;
        DataTable dtAyuda;

        bool bRespuesta;

        SqlParameter[] parametro;

        int iCgTipoDocumento = 2725;
        int iNumeroPedidoOrden;
        int iCuentaDiaria;
        int iIdLocalidad;
        int iIdListaBase;
        int iIdPedido;
        int iIdPedidoOriginal;
        int iIdFactura;
        int iIdDocumentoCobrar;
        int iIdPago;
        int iIdDocumentoPago;
        int iIdEventoCobro;
        int iIdPosMovimientoCaja;
        int iIdDespachoPedido;
        int iIdCabDespacho;
        int iIdPosTarCabMovimiento;
        int iBanderaEliminarBodega;
        int iIdProducto;
        int iPagaIva;
        int iPagaServicio;
        int iIdCabDespachos;

        Decimal dbPrecio;

        long iMaximo;

        //FUNCION PARA RECIBIR LOS PARAMETROS
        public bool reciberParametroReabrir(int iIdPedido_P, int iIdLocalidad_P, string sMotivo_P)
        {
            try
            {
                this.iIdPedidoOriginal = iIdPedido_P;
                this.iIdLocalidad = iIdLocalidad_P;
                this.sMotivo = sMotivo_P;

                if (consultarProductoReabrir() == false)
                    return false;

                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    sMensajeError = "Error al iniciar la transacción.";
                    return false;
                }

                if (crearPedidoEliminar() == false)
                {
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    return false;
                }

                if (eliminarPagos() == false)
                {
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    return false;
                }

                if (consultarFacturaOriginal() == false)
                {
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    return false;
                }

                if (eliminarComandaTemporal() == false)
                {
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    return false;
                }

                //INSTRUCCION PARA INSERTAR EL MOTIVO DE ANULACIÓN DE LA FACTURA
                sSql = "";
                sSql += "insert into pos_anulacion_factura (" + Environment.NewLine;
                sSql += "id_factura, motivo_anulacion, estado, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdFactura + ", '" + sMotivo + "', 'A'," + Environment.NewLine;
                sSql += "GETDATE(), '" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "')";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    sMensajeError = conexion.sMensajeError;
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    return false;
                }

                sSql = "";
                sSql += "" + Environment.NewLine;
                sSql += "update cv403_cab_pedidos set" + Environment.NewLine;
                sSql += "estado_orden = @estado_orden," + Environment.NewLine;
                sSql += "recargo_tarjeta = @recargo_tarjeta," + Environment.NewLine;
                sSql += "remover_iva = @remover_iva" + Environment.NewLine; 
                sSql += "where id_pedido = @id_pedido" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                parametro = new SqlParameter[5];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@estado_orden";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "Abierta";

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@recargo_tarjeta";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = 0;

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@remover_iva";
                parametro[2].SqlDbType = SqlDbType.Int;
                parametro[2].Value = 0;

                parametro[3] = new SqlParameter();
                parametro[3].ParameterName = "@id_pedido";
                parametro[3].SqlDbType = SqlDbType.Int;
                parametro[3].Value = iIdPedidoOriginal;                

                parametro[4] = new SqlParameter();
                parametro[4].ParameterName = "@estado";
                parametro[4].SqlDbType = SqlDbType.VarChar;
                parametro[4].Value = "A";

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    sMensajeError = conexion.sMensajeError;
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    return false;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                return true;
            }

            catch (Exception ex)
            {
                sMensajeError = ex.Message;
                return false;
            }
        }

        //FUNCION PARA CONSULTAR EL PRODUCTO PARAMETRIZADO PARA REABRIR LA COMANDA
        private bool consultarProductoReabrir()
        {
            try
            {
                sSql = "";
                sSql += "select id_lista_precio" + Environment.NewLine;
                sSql += "from cv403_listas_precios" + Environment.NewLine;
                sSql += "where estado = @estado" + Environment.NewLine;
                sSql += "and lista_base = @id_lista_base";

                #region PARAMETROS
                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@estado";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "A";

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@id_lista_base";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = 1;

                #endregion

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtConsulta, sSql, parametro);

                if (bRespuesta == false)
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    sMensajeError = "Lista de precios base no configurada. Comuníquese con el administrador del sistema.";
                    //sMensajeError = "Producto auxiliar de reapuertura de comanda noconfigurado. Comuníquese con el administrador del sistema.";
                    return false;
                }

                iIdListaBase = Convert.ToInt32(dtConsulta.Rows[0]["id_lista_precio"].ToString());

                sSql = "";
                sSql += "select PL.id_producto_anula, P.paga_iva, P.paga_servicio, PP.valor" + Environment.NewLine;
                sSql += "from pos_parametro_localidad PL INNER JOIN" + Environment.NewLine;
                sSql += "cv401_productos P ON P.id_producto = PL.id_producto_anula" + Environment.NewLine;
                sSql += "and P.estado = @estado_1" + Environment.NewLine;
                sSql += "and PL.estado = @estado_2 INNER JOIN" + Environment.NewLine;
                sSql += "cv403_precios_productos PP ON P.id_producto = PP.id_producto" + Environment.NewLine;
                sSql += "and PP.estado = @estado_3" + Environment.NewLine;
                sSql += "where PL.id_localidad = @id_localidad" + Environment.NewLine;
                sSql += "and PP.id_lista_precio = @id_lista_precio";

                #region PARAMETROS

                parametro = new SqlParameter[5];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@estado_1";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "A";

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@estado_2";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = "A";

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@estado_3";
                parametro[2].SqlDbType = SqlDbType.VarChar;
                parametro[2].Value = "A";

                parametro[3] = new SqlParameter();
                parametro[3].ParameterName = "@id_localidad";
                parametro[3].SqlDbType = SqlDbType.VarChar;
                parametro[3].Value = iIdLocalidad;

                parametro[4] = new SqlParameter();
                parametro[4].ParameterName = "@id_lista_precio";
                parametro[4].SqlDbType = SqlDbType.Int;
                parametro[4].Value = iIdListaBase;

                #endregion

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtConsulta, sSql, parametro);

                if (bRespuesta == false)
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    sMensajeError = "Producto auxiliar de reabrir la comanda no se encuentra configurado. Comuníquese con el administrador del sistema.";
                    return false;
                }

                iIdProducto = Convert.ToInt32(dtConsulta.Rows[0]["id_producto_anula"].ToString());
                iPagaIva = Convert.ToInt32(dtConsulta.Rows[0]["paga_iva"].ToString());
                iPagaServicio = Convert.ToInt32(dtConsulta.Rows[0]["paga_servicio"].ToString());
                dbPrecio = Convert.ToDecimal(dtConsulta.Rows[0]["valor"].ToString());

                return true;
            }

            catch (Exception ex)
            {
                sMensajeError = ex.Message;
                return false;
            }
        }

        //FUNCION PARA INSERTAR UN PEDIDO AUXILIAR PARA ANULAR
        private bool crearPedidoEliminar()
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
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                sFecha = Convert.ToDateTime(dtConsulta.Rows[0]["fecha"].ToString()).ToString("yyyy-MM-dd");

                extraerNumeroCuenta();

                #region INSERTANDO UNA NUEVA COMANDA

                sSql = "";
                sSql += "Insert Into cv403_cab_pedidos (idEmpresa, cg_empresa, Id_Localidad," + Environment.NewLine;
                sSql += "Fecha_Pedido, id_persona, Cg_Tipo_Cliente, Cg_Moneda, Porcentaje_Iva," + Environment.NewLine;
                sSql += "id_vendedor, Fabricante, referencia, Comentarios, cg_estado_Pedido," + Environment.NewLine;
                sSql += "Porcentaje_Dscto, Cg_Facturado, Fecha_Ingreso, Usuario_Ingreso," + Environment.NewLine;
                sSql += "Terminal_Ingreso, Estado, numero_replica_trigger, numero_control_replica," + Environment.NewLine;
                sSql += "porcentaje_servicio) " + Environment.NewLine;
                sSql += "Values (" + Environment.NewLine;
                sSql += Program.iIdEmpresa + ", " + Program.iCgEmpresa + ", " + Program.iIdLocalidad + "," + Environment.NewLine;
                sSql += "'" + sFecha + "', " + Program.iIdPersona + ", 8032, " + Program.iMoneda + "," + Environment.NewLine;
                sSql += (Program.iva * 100) + ", " + Program.iIdVendedor + ", 78, '', '', 6967, 0, 7469," + Environment.NewLine;
                sSql += "GETDATE(), '" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "'," + Environment.NewLine;
                sSql += "'A', 0, 0, " + (Program.servicio * 100) + ")";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                sSql = "";
                sSql += "insert into cv403_cab_despachos (" + Environment.NewLine;
                sSql += "idempresa, id_persona, cg_empresa, id_localidad, fecha_despacho," + Environment.NewLine;
                sSql += "cg_motivo_despacho, id_destinatario, punto_partida, cg_ciudad_entrega," + Environment.NewLine;
                sSql += "direccion_entrega, id_transportador, fecha_inicio_transporte," + Environment.NewLine;
                sSql += "fecha_fin_transporte, cg_estado_despacho, punto_venta, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso, estado, numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += Program.iIdEmpresa + ", " + Program.iIdPersona + ", " + Program.iCgEmpresa + ", " + Program.iIdLocalidad + "," + Environment.NewLine;
                sSql += "'" + sFecha + "', " + Program.iCgMotivoDespacho + ", " + Program.iIdPersona + "," + Environment.NewLine;
                sSql += "'" + Program.sPuntoPartida + "', " + Program.iCgCiudadEntrega + ", '" + Program.sDireccionEntrega + "'," + Environment.NewLine;
                sSql += "'" + Program.iIdPersona + "', '" + sFecha + "', '" + sFecha + "', " + Program.iCgEstadoDespacho + "," + Environment.NewLine;
                sSql += "1, GETDATE(), '" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "', 'A', 1, 0)";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                sTabla = "cv403_cab_pedidos";
                sCampo = "Id_Pedido";

                iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                if (iMaximo == -1)
                {
                    //sMensajeError = "No se pudo obtener el codigo de la tabla " + sTabla;
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                iIdPedido = Convert.ToInt32(iMaximo);

                sSql = "";
                sSql += "select numero_pedido" + Environment.NewLine;
                sSql += "from tp_localidades_impresoras" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;
                sSql += "and id_localidad = " + Program.iIdLocalidad;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == false)
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                iNumeroPedidoOrden = Convert.ToInt32(dtConsulta.Rows[0]["numero_pedido"].ToString());

                sSql = "";
                sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                sSql += "numero_pedido = numero_pedido + 1" + Environment.NewLine;
                sSql += "where id_localidad = " + Program.iIdLocalidad;

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                sSql = "";
                sSql += "insert into cv403_numero_cab_pedido (" + Environment.NewLine;
                sSql += "idtipocomprobante,id_pedido, numero_pedido," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso," + Environment.NewLine;
                sSql += "estado, numero_control_replica, numero_replica_trigger)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += "1, " + iIdPedido + ", " + iNumeroPedidoOrden + ", GETDATE()," + Environment.NewLine;
                sSql += "'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "', 'A', 0, 0)";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                sTabla = "cv403_cab_despachos";
                sCampo = "id_despacho";

                iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                if (iMaximo == -1)
                {
                    //sMensajeError = "No se pudo obtener el codigo de la tabla " + sTabla;
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                iIdCabDespachos = Convert.ToInt32(iMaximo);

                sSql = "";
                sSql += "insert into cv403_despachos_pedidos (" + Environment.NewLine;
                sSql += "id_despacho, id_pedido, estado, fecha_ingreso, usuario_ingreso," + Environment.NewLine;
                sSql += "terminal_ingreso, numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdCabDespachos + "," + iIdPedido + ", 'A', GETDATE(), '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + Program.sDatosMaximo[1] + "', 1, 0)";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                sTabla = "cv403_despachos_pedidos";
                sCampo = "id_despacho_pedido";
                
                iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                if (iMaximo == -1)
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                iIdDespachoPedido = Convert.ToInt32(iMaximo);

                sSql = "";
                sSql += "insert into cv403_eventos_cobros (" + Environment.NewLine;
                sSql += "idempresa, cg_empresa, id_persona, id_localidad, cg_evento_cobro," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, estado," + Environment.NewLine;
                sSql += "numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += Program.iIdEmpresa + ", " + Program.iCgEmpresa + ", " + Program.iIdPersona + "," + Program.iIdLocalidad + "," + Environment.NewLine;
                sSql += "7466, GETDATE(), '" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "', 'A', 1, 0)";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                sTabla = "cv403_eventos_cobros";
                sCampo = "id_evento_cobro";

                iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                if (iMaximo == -1)
                {
                    //sMensajeError = "No se pudo obtener el codigo de la tabla " + sTabla;
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                iIdEventoCobro = Convert.ToInt32(iMaximo);

                sSql = "";
                sSql += "insert into cv403_dctos_por_cobrar (" + Environment.NewLine;
                sSql += "id_evento_cobro, id_pedido, cg_tipo_documento, fecha_vcto, cg_moneda," + Environment.NewLine;
                sSql += "valor, cg_estado_dcto, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso," + Environment.NewLine;
                sSql += "numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += iIdEventoCobro + ", " + iIdPedido + ", " + iCgTipoDocumento + "," + Environment.NewLine;
                sSql += "'" + sFecha + "', " + Program.iMoneda + ", " + dbPrecio + "," + Environment.NewLine;
                sSql += "7460, 'A', GETDATE(), '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + Program.sDatosMaximo[1] + "', 0, 0)";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    sMensajeError = conexion.sMensajeError;
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                sSql += "Insert Into cv403_det_pedidos(Id_Pedido, id_producto, Cg_Unidad_Medida, precio_unitario," + Environment.NewLine;
                sSql += "Cantidad, Valor_Dscto, Valor_Ice, Valor_Iva, comentario, Id_Definicion_Combo," + Environment.NewLine;
                sSql += "fecha_ingreso, Usuario_Ingreso, Terminal_ingreso, Estado, numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                sSql += "Values (" + Environment.NewLine;
                sSql += iIdPedido + ", " + iIdProducto + ", 546, " + dbPrecio + "," + Environment.NewLine;
                sSql += "1, 0, 0, 0, '', null, GETDATE(), '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "'" + Program.sDatosMaximo[1] + "', 'A', 0, 0)";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    sMensajeError = conexion.sMensajeError;
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                //INSTRUCCION PARA INSERTAR EN LA TABLA CV403_CANTIDADES_DESPACHADAS
                sSql = "";
                sSql += "Insert Into cv403_cantidades_despachadas (Id_Despacho_Pedido, id_producto," + Environment.NewLine;
                sSql += "Cantidad, Estado, numero_replica_trigger, numero_control_replica) " + Environment.NewLine;
                sSql += "Values (" + Environment.NewLine;
                sSql += iIdDespachoPedido + ", " + iIdProducto + ", 1, 'A', 0, 0)";

                #endregion

                return true;
            }

            catch (Exception ex)
            {
                sMensajeError = ex.Message;
                return false;
            }
        }

        //FUNCION PARA EXTRAER EL NUMERO DE CUENTA
        private bool extraerNumeroCuenta()
        {
            try
            {
                sSql = "";
                sSql += "select isnull(max(cuenta), 0) cuenta" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos" + Environment.NewLine;
                sSql += "where fecha_pedido = '" + sFecha + "'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == false)
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                iCuentaDiaria = Convert.ToInt32(dtConsulta.Rows[0]["cuenta"].ToString()) + 1;
                return true;
            }

            catch (Exception ex)
            {
                sMensajeError = ex.Message;
                return false;
            }
        }

        //FUNCION PARA EXTRAER LOS DATOS DE LA FACTURA A ELIMINAR
        private bool consultarFacturaOriginal()
        {
            try
            {
                sSql = "";
                sSql += "select id_factura" + Environment.NewLine;
                sSql += "from cv403_facturas_pedidos" + Environment.NewLine;
                sSql += "where id_pedido = @id_pedido" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_pedido";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdPedidoOriginal;

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
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                if (dtConsulta.Rows.Count > 0)
                {
                    if (eliminarFactura(Convert.ToInt32(dtConsulta.Rows[0]["id_factura"].ToString())) == false)
                        return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                sMensajeError = ex.Message;
                return false;
            }
        }

        //FUNCION PARA ELIMINAR LA FACTURA
        private bool eliminarFactura(int iIdFactura_P)
        {
            try
            {
                int a;

                sSql = "";
                sSql += "update cv403_facturas_pedidos set" + Environment.NewLine;
                sSql += "id_pedido = @id_pedido," + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_factura = @id_factura" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[6];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pedido";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdPedido;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_1";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "E";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@usuario_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[0];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@terminal_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[1];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_factura";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdFactura_P;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_2";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                sSql = "";
                sSql += "update cv403_facturas set" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_factura = @id_factura" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[5];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_1";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "E";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@usuario_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[0];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@terminal_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[1];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_factura";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdFactura_P;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_2";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                sSql = "";
                sSql += "update cv403_numeros_facturas set" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_factura = @id_factura" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[5];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_1";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "E";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@usuario_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[0];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@terminal_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[1];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_factura";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdFactura_P;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_2";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                sMensajeError = ex.Message;
                return false;
            }
        }

        //FUNCION PARA ELIMINAR LOS PAGOS
        private bool eliminarPagos()
        {
            try
            {
                //EXTRAER EL ID DE LA TABLA CV403_DCTOS_POR_COBRAR
                sSql = "";
                sSql += "select id_documento_cobrar" + Environment.NewLine;
                sSql += "from cv403_dctos_por_cobrar" + Environment.NewLine;
                sSql += "where id_pedido = @id_pedido" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_pedido";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdPedidoOriginal;

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
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                int iIdDocumentoCobrar = Convert.ToInt32(dtConsulta.Rows[0]["id_documento_cobrar"].ToString());

                //EXTRAER EL ID DE LA TABLA CV403_DOCUMENTOS_PAGADOS
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
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                int iIdPago_P;
                int b;

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    iIdPago_P = Convert.ToInt32(dtConsulta.Rows[i]["id_pago"].ToString());

                    sSql = "";
                    sSql += "select id_documento_pago" + Environment.NewLine;
                    sSql += "from cv403_documentos_pagos" + Environment.NewLine;
                    sSql += "where id_pago = @id_pago" + Environment.NewLine;
                    sSql += "and estado = @estado";

                    #region PARAMETROS

                    parametro = new SqlParameter[2];
                    parametro[0] = new SqlParameter();
                    parametro[0].ParameterName = "@id_pago";
                    parametro[0].SqlDbType = SqlDbType.Int;
                    parametro[0].Value = iIdPago_P;

                    parametro[1] = new SqlParameter();
                    parametro[1].ParameterName = "@estado";
                    parametro[1].SqlDbType = SqlDbType.VarChar;
                    parametro[1].Value = "A";

                    #endregion

                    dtAyuda = new DataTable();
                    dtAyuda.Clear();

                    bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtAyuda, sSql, parametro);

                    if (bRespuesta == false)
                    {
                        sMensajeError = conexion.sMensajeError;
                        return false;
                    }

                    for (int j = 0; j < dtAyuda.Rows.Count; j++)
                    {
                        int iIdDocumentoPago_P = Convert.ToInt32(dtAyuda.Rows[j]["id_documento_pago"].ToString());

                        sSql = "";
                        sSql += "select id_pos_movimiento_caja" + Environment.NewLine;
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

                        DataTable dt = new DataTable();
                        dt.Clear();

                        bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dt, sSql, parametro);

                        if (bRespuesta == false)
                        {
                            sMensajeError = conexion.sMensajeError;
                            return false;
                        }

                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            int iIdPosMovimientoCaja_P = Convert.ToInt32(dt.Rows[k]["id_pos_movimiento_caja"].ToString());

                            sSql = "";
                            sSql += "update pos_movimiento_caja set" + Environment.NewLine;
                            sSql += "estado = @estado_1," + Environment.NewLine;
                            sSql += "fecha_anula = getdate()," + Environment.NewLine;
                            sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                            sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                            sSql += "where id_pos_movimiento_caja = @id_pos_movimiento_caja" + Environment.NewLine;
                            sSql += "and estado = @estado_2";

                            #region PARAMETROS

                            int a = 0;
                            parametro = new SqlParameter[5];
                            parametro[a] = new SqlParameter();
                            parametro[a].ParameterName = "@estado_1";
                            parametro[a].SqlDbType = SqlDbType.VarChar;
                            parametro[a].Value = "E";
                            a++;

                            parametro[a] = new SqlParameter();
                            parametro[a].ParameterName = "@usuario_anula";
                            parametro[a].SqlDbType = SqlDbType.VarChar;
                            parametro[a].Value = Program.sDatosMaximo[0];
                            a++;

                            parametro[a] = new SqlParameter();
                            parametro[a].ParameterName = "@terminal_anula";
                            parametro[a].SqlDbType = SqlDbType.VarChar;
                            parametro[a].Value = Program.sDatosMaximo[1];
                            a++;

                            parametro[a] = new SqlParameter();
                            parametro[a].ParameterName = "@id_pos_movimiento_caja";
                            parametro[a].SqlDbType = SqlDbType.Int;
                            parametro[a].Value = iIdPosMovimientoCaja_P;
                            a++;

                            parametro[a] = new SqlParameter();
                            parametro[a].ParameterName = "@estado_2";
                            parametro[a].SqlDbType = SqlDbType.VarChar;
                            parametro[a].Value = "A";

                            #endregion

                            if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                            {
                                sMensajeError = conexion.sMensajeError;
                                return false;
                            }

                            sSql = "";
                            sSql += "update pos_numero_movimiento_caja set" + Environment.NewLine;
                            sSql += "estado = @estado_1," + Environment.NewLine;
                            sSql += "fecha_anula = getdate()," + Environment.NewLine;
                            sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                            sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                            sSql += "where id_pos_movimiento_caja = @id_pos_movimiento_caja" + Environment.NewLine;
                            sSql += "and estado = @estado_2";

                            #region PARAMETROS

                            a = 0;
                            parametro = new SqlParameter[5];
                            parametro[a] = new SqlParameter();
                            parametro[a].ParameterName = "@estado_1";
                            parametro[a].SqlDbType = SqlDbType.VarChar;
                            parametro[a].Value = "E";
                            a++;

                            parametro[a] = new SqlParameter();
                            parametro[a].ParameterName = "@usuario_anula";
                            parametro[a].SqlDbType = SqlDbType.VarChar;
                            parametro[a].Value = Program.sDatosMaximo[0];
                            a++;

                            parametro[a] = new SqlParameter();
                            parametro[a].ParameterName = "@terminal_anula";
                            parametro[a].SqlDbType = SqlDbType.VarChar;
                            parametro[a].Value = Program.sDatosMaximo[1];
                            a++;

                            parametro[a] = new SqlParameter();
                            parametro[a].ParameterName = "@id_pos_movimiento_caja";
                            parametro[a].SqlDbType = SqlDbType.Int;
                            parametro[a].Value = iIdPosMovimientoCaja_P;
                            a++;

                            parametro[a] = new SqlParameter();
                            parametro[a].ParameterName = "@estado_2";
                            parametro[a].SqlDbType = SqlDbType.VarChar;
                            parametro[a].Value = "A";

                            #endregion

                            if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                            {
                                sMensajeError = conexion.sMensajeError;
                                return false;
                            }
                        }
                    }

                    //ELIMINANDO LA TABLA CV403_PAGOS
                    sSql = "";
                    sSql += "update cv403_pagos set" + Environment.NewLine;
                    sSql += "estado = @estado_1," + Environment.NewLine;
                    sSql += "fecha_anula = getdate()," + Environment.NewLine;
                    sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                    sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                    sSql += "where id_pago = @id_pago" + Environment.NewLine;
                    sSql += "and estado = @estado_2";

                    #region PARAMETROS

                    b = 0;
                    parametro = new SqlParameter[5];
                    parametro[b] = new SqlParameter();
                    parametro[b].ParameterName = "@estado_1";
                    parametro[b].SqlDbType = SqlDbType.VarChar;
                    parametro[b].Value = "E";
                    b++;

                    parametro[b] = new SqlParameter();
                    parametro[b].ParameterName = "@usuario_anula";
                    parametro[b].SqlDbType = SqlDbType.VarChar;
                    parametro[b].Value = Program.sDatosMaximo[0];
                    b++;

                    parametro[b] = new SqlParameter();
                    parametro[b].ParameterName = "@terminal_anula";
                    parametro[b].SqlDbType = SqlDbType.VarChar;
                    parametro[b].Value = Program.sDatosMaximo[1];
                    b++;

                    parametro[b] = new SqlParameter();
                    parametro[b].ParameterName = "@id_pago";
                    parametro[b].SqlDbType = SqlDbType.Int;
                    parametro[b].Value = iIdPago_P;
                    b++;

                    parametro[b] = new SqlParameter();
                    parametro[b].ParameterName = "@estado_2";
                    parametro[b].SqlDbType = SqlDbType.VarChar;
                    parametro[b].Value = "A";

                    #endregion

                    if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                    {
                        sMensajeError = conexion.sMensajeError;
                        return false;
                    }

                    //ELIMINANDO LA TABLA CV403_NUMEROS_PAGOS
                    sSql = "";
                    sSql += "update cv403_numeros_pagos set" + Environment.NewLine;
                    sSql += "estado = @estado_1," + Environment.NewLine;
                    sSql += "fecha_anula = getdate()," + Environment.NewLine;
                    sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                    sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                    sSql += "where id_pago = @id_pago" + Environment.NewLine;
                    sSql += "and estado = @estado_2";

                    #region PARAMETROS

                    b = 0;
                    parametro = new SqlParameter[5];
                    parametro[b] = new SqlParameter();
                    parametro[b].ParameterName = "@estado_1";
                    parametro[b].SqlDbType = SqlDbType.VarChar;
                    parametro[b].Value = "E";
                    b++;

                    parametro[b] = new SqlParameter();
                    parametro[b].ParameterName = "@usuario_anula";
                    parametro[b].SqlDbType = SqlDbType.VarChar;
                    parametro[b].Value = Program.sDatosMaximo[0];
                    b++;

                    parametro[b] = new SqlParameter();
                    parametro[b].ParameterName = "@terminal_anula";
                    parametro[b].SqlDbType = SqlDbType.VarChar;
                    parametro[b].Value = Program.sDatosMaximo[1];
                    b++;

                    parametro[b] = new SqlParameter();
                    parametro[b].ParameterName = "@id_pago";
                    parametro[b].SqlDbType = SqlDbType.Int;
                    parametro[b].Value = iIdPago_P;
                    b++;

                    parametro[b] = new SqlParameter();
                    parametro[b].ParameterName = "@estado_2";
                    parametro[b].SqlDbType = SqlDbType.VarChar;
                    parametro[b].Value = "A";

                    #endregion

                    if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                    {
                        sMensajeError = conexion.sMensajeError;
                        return false;
                    }

                    //ELIMINANDO LA TABLA CV403_DOCUMENTOS_PAGOS
                    sSql = "";
                    sSql += "update cv403_documentos_pagos set" + Environment.NewLine;
                    sSql += "estado = @estado_1," + Environment.NewLine;
                    sSql += "fecha_anula = getdate()," + Environment.NewLine;
                    sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                    sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                    sSql += "where id_pago = @id_pago" + Environment.NewLine;
                    sSql += "and estado = @estado_2";

                    #region PARAMETROS

                    b = 0;
                    parametro = new SqlParameter[5];
                    parametro[b] = new SqlParameter();
                    parametro[b].ParameterName = "@estado_1";
                    parametro[b].SqlDbType = SqlDbType.VarChar;
                    parametro[b].Value = "E";
                    b++;

                    parametro[b] = new SqlParameter();
                    parametro[b].ParameterName = "@usuario_anula";
                    parametro[b].SqlDbType = SqlDbType.VarChar;
                    parametro[b].Value = Program.sDatosMaximo[0];
                    b++;

                    parametro[b] = new SqlParameter();
                    parametro[b].ParameterName = "@terminal_anula";
                    parametro[b].SqlDbType = SqlDbType.VarChar;
                    parametro[b].Value = Program.sDatosMaximo[1];
                    b++;

                    parametro[b] = new SqlParameter();
                    parametro[b].ParameterName = "@id_pago";
                    parametro[b].SqlDbType = SqlDbType.Int;
                    parametro[b].Value = iIdPago_P;
                    b++;

                    parametro[b] = new SqlParameter();
                    parametro[b].ParameterName = "@estado_2";
                    parametro[b].SqlDbType = SqlDbType.VarChar;
                    parametro[b].Value = "A";

                    #endregion

                    if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                    {
                        sMensajeError = conexion.sMensajeError;
                        return false;
                    }

                    //ELIMINANDO LA TABLA CV403_DOCUMENTOS_PAGADOS
                    sSql = "";
                    sSql += "update cv403_documentos_pagados set" + Environment.NewLine;
                    sSql += "estado = @estado_1," + Environment.NewLine;
                    sSql += "fecha_anula = getdate()," + Environment.NewLine;
                    sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                    sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                    sSql += "where id_pago = @id_pago" + Environment.NewLine;
                    sSql += "and estado = @estado_2";

                    #region PARAMETROS

                    b = 0;
                    parametro = new SqlParameter[5];
                    parametro[b] = new SqlParameter();
                    parametro[b].ParameterName = "@estado_1";
                    parametro[b].SqlDbType = SqlDbType.VarChar;
                    parametro[b].Value = "E";
                    b++;

                    parametro[b] = new SqlParameter();
                    parametro[b].ParameterName = "@usuario_anula";
                    parametro[b].SqlDbType = SqlDbType.VarChar;
                    parametro[b].Value = Program.sDatosMaximo[0];
                    b++;

                    parametro[b] = new SqlParameter();
                    parametro[b].ParameterName = "@terminal_anula";
                    parametro[b].SqlDbType = SqlDbType.VarChar;
                    parametro[b].Value = Program.sDatosMaximo[1];
                    b++;

                    parametro[b] = new SqlParameter();
                    parametro[b].ParameterName = "@id_pago";
                    parametro[b].SqlDbType = SqlDbType.Int;
                    parametro[b].Value = iIdPago_P;
                    b++;

                    parametro[b] = new SqlParameter();
                    parametro[b].ParameterName = "@estado_2";
                    parametro[b].SqlDbType = SqlDbType.VarChar;
                    parametro[b].Value = "A";

                    #endregion

                    if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                    {
                        sMensajeError = conexion.sMensajeError;
                        return false;
                    }
                }

                //INSTRUCCION PARA DEJAR LOS CAMPOS NULOS EN CV403_DCTOS_POR_COBRAR
                sSql = "";
                sSql += "update cv403_dctos_por_cobrar set" + Environment.NewLine;
                sSql += "id_factura = null," + Environment.NewLine;
                sSql += "numero_documento = null," + Environment.NewLine;
                sSql += "cg_estado_dcto = @cg_estado_dcto" + Environment.NewLine;
                sSql += "where id_pedido = @id_pedido" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                b = 0;
                parametro = new SqlParameter[3];
                parametro[b] = new SqlParameter();
                parametro[b].ParameterName = "@cg_estado_dcto";
                parametro[b].SqlDbType = SqlDbType.Int;
                parametro[b].Value = 7460;
                b++;
                
                parametro[b] = new SqlParameter();
                parametro[b].ParameterName = "@id_pedido";
                parametro[b].SqlDbType = SqlDbType.Int;
                parametro[b].Value = iIdPedidoOriginal;
                b++;

                parametro[b] = new SqlParameter();
                parametro[b].ParameterName = "@estado";
                parametro[b].SqlDbType = SqlDbType.VarChar;
                parametro[b].Value = "A";

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                sMensajeError = ex.Message;
                return false;
            }
        }

        //FUNCION PARA ELIMINAR LA NUEVA COMANDA
        private bool eliminarComandaTemporal()
        {
            try
            {
                int a;

                sSql = "";
                sSql += "update cv403_cab_pedidos set" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_pedido = @id_pedido" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[5];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_1";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "E";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@usuario_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[0];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@terminal_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[1];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pedido";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdPedido;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_2";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                sSql = "";
                sSql += "update cv403_cab_despachos set" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_despacho = @id_despacho" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[5];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_1";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "E";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@usuario_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[0];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@terminal_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[1];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_despacho";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdCabDespachos;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_2";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                sSql = "";
                sSql += "update cv403_numero_cab_pedido set" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_pedido = @id_pedido" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[5];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_1";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "E";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@usuario_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[0];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@terminal_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[1];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pedido";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdPedido;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_2";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                sSql = "";
                sSql += "update cv403_despachos_pedidos set" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_despacho = @id_despacho" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[5];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_1";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "E";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@usuario_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[0];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@terminal_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[1];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_despacho";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdCabDespachos;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_2";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                sSql = "";
                sSql += "update cv403_eventos_cobros set" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_evento_cobro = @id_evento_cobro" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[5];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_1";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "E";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@usuario_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[0];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@terminal_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[1];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_evento_cobro";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdEventoCobro;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_2";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                sSql = "";
                sSql += "update cv403_dctos_por_cobrar set" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_pedido = @id_pedido" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[5];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_1";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "E";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@usuario_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[0];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@terminal_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[1];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pedido";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdPedido;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_2";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                sSql = "";
                sSql += "update cv403_det_pedidos set" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_pedido = @id_pedido" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[5];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_1";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "E";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@usuario_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[0];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@terminal_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[1];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pedido";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdPedido;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_2";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                sSql = "";
                sSql += "update cv403_cantidades_despachadas set" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_despacho_pedido = @id_despacho_pedido" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[5];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_1";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "E";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@usuario_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[0];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@terminal_anula";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[1];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_despacho_pedido";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdDespachoPedido;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_2";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                sMensajeError = ex.Message;
                return false;
            }
        }
    }
}
