using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palatium.Clases_Crear_Comandas
{
    class ClaseEliminarComanda_V2
    {
        ConexionBD.ConexionBD conexion;

        string sSql;
        string sMotivo;
        string sFecha;
        public string sMensajeError;

        DataTable dtConsulta;
        DataTable dtAyuda;

        bool bRespuesta;

        int iIdPedido;
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

        SqlParameter[] parametro;

        public bool procesoEliminacion(int iIdPedido_P, string sMotivo_P, int iBanderaEliminarBodega_P, string sFecha_P,
                                       int iIniciaTransaccion_P, ConexionBD.ConexionBD conexion_P)
        {
            try
            {
                this.iIdPedido = iIdPedido_P;
                this.sMotivo = sMotivo_P;
                this.iBanderaEliminarBodega = iBanderaEliminarBodega_P;
                this.sFecha = sFecha_P;
                this.conexion = conexion_P;

                if (iIniciaTransaccion_P == 0)
                {
                    if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                    {
                        return false;
                    }
                }

                if (obtenerRegistros() == false)
                {
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    return false;
                }

                sSql = "";
                sSql += "insert into pos_cancelacion (" + Environment.NewLine;
                sSql += "id_pedido, motivo_cancelacion, estado, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += "@id_pedido, @motivo_cancelacion, @estado, getdate()," + Environment.NewLine;
                sSql += "@usuario_ingreso, @terminal_ingreso)" + Environment.NewLine;

                #region PARAMETROS

                int a = 0;
                parametro = new SqlParameter[5];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pedido";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdPedido;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@motivo_cancelacion";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = sMotivo;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@usuario_ingreso";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[0];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@terminal_ingreso";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[0];

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
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                return false;
            }
        }

        //FUNCION PARA RECUPERAR LOS VALORES
        private bool obtenerRegistros()
        {
            try
            {
                //BUSCAR EL ID DE LA CABECERA DE LA VENTA DE LA TARJETA DE ALMUERZO

                sSql = "";
                sSql += "select id_pos_tar_cab_movimiento" + Environment.NewLine;
                sSql += "from pos_tar_cab_movimiento" + Environment.NewLine;
                sSql += "where id_pedido = @id_pedido" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_pedido";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdPedido;

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
                    iIdPosTarCabMovimiento = Convert.ToInt32(dtConsulta.Rows[0]["id_pos_tar_cab_movimiento"].ToString());

                    if (eliminarPedidoTarjetaAlmuerzo() == false)
                    {
                        return false;
                    }
                }

                //BUSCAR EL ID_FACTURA
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
                parametro[0].Value = iIdPedido;

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
                    iIdFactura = Convert.ToInt32(dtConsulta.Rows[0]["id_factura"].ToString());

                    if (eliminarFactura() == false)
                    {
                        return false;
                    }
                }

                //BUSCAR EL ID_DOCUMENTO_COBRAR
                sSql = "";
                sSql += "select id_documento_cobrar, id_evento_cobro" + Environment.NewLine;
                sSql += "from cv403_dctos_por_cobrar" + Environment.NewLine;
                sSql += "where id_pedido = @id_pedido" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_pedido";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdPedido;

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

                iIdDocumentoCobrar = Convert.ToInt32(dtConsulta.Rows[0]["id_documento_cobrar"].ToString());
                iIdEventoCobro = Convert.ToInt32(dtConsulta.Rows[0]["id_evento_cobro"].ToString());

                //BUSCAMOS LOS PAGOS
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

                if (dtConsulta.Rows.Count > 0)
                {
                    iIdPago = Convert.ToInt32(dtConsulta.Rows[0]["id_pago"].ToString());

                    if (eliminarPagos() == false)
                    {
                        return false;
                    }
                }

                if (eliminarPedido() == false)
                {
                    return false;
                }


                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        //FUNCION PARA ELIMINAR EL PASO DE FACTURACION
        private bool eliminarFactura()
        {
            try
            {
                int a;

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
                parametro[a].Value = "N";
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
                parametro[a].Value = iIdFactura;
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
                sSql += "update cv403_facturas set" + Environment.NewLine;
                sSql += "comentarios = @comentarios," + Environment.NewLine;
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
                parametro[a].ParameterName = "@comentarios";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = sMotivo;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_1";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "N";
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
                parametro[a].Value = iIdFactura;
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
                sSql += "update cv403_facturas_pedidos set" + Environment.NewLine;
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
                parametro[a].Value = "N";
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
                parametro[a].Value = iIdFactura;
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

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        //FUNCION PARA ELIMINAR EL PASO DE PAGOS
        private bool eliminarPagos()
        {
            try
            {
                int a;

                sSql = "";
                sSql += "update cv403_pagos set" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_pago = @id_pago" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[5];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_1";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "N";
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
                parametro[a].ParameterName = "@id_pago";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdPago;
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
                sSql += "update cv403_numeros_pagos set" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_pago = @id_pago" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[5];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_1";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "N";
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
                parametro[a].ParameterName = "@id_pago";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdPago;
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
                sSql += "update cv403_documentos_pagados set" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_pago = @id_pago" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[5];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_1";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "N";
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
                parametro[a].ParameterName = "@id_pago";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdPago;
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

                //BUSCAMOS LOS DOCUMENTOS PAGOS
                sSql = "";
                sSql += "select id_documento_pago" + Environment.NewLine;
                sSql += "from cv403_documentos_pagos" + Environment.NewLine;
                sSql += "where id_pago = @id_pago" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[2];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pago";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdPago;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";

                #endregion

                dtAyuda = new DataTable();
                dtAyuda.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtAyuda, sSql, parametro);

                if (bRespuesta == false)
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                for (int i = 0; i < dtAyuda.Rows.Count; i++)
                {
                    iIdDocumentoPago = Convert.ToInt32(dtAyuda.Rows[i]["id_documento_pago"].ToString());

                    sSql = "";
                    sSql += "select id_pos_movimiento_caja" + Environment.NewLine;
                    sSql += "from pos_movimiento_caja" + Environment.NewLine;
                    sSql += "where id_documento_pago = @id_documento_pago" + Environment.NewLine;
                    sSql += "and estado = @estado";

                    #region PARAMETROS

                    a = 0;
                    parametro = new SqlParameter[2];
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_documento_pago";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdDocumentoPago;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@estado";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = "A";

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
                        iIdPosMovimientoCaja = Convert.ToInt32(dtConsulta.Rows[0]["id_pos_movimiento_caja"].ToString());

                        sSql = "";
                        sSql += "update pos_movimiento_caja set" + Environment.NewLine;
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
                        parametro[a].Value = "N";
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
                        parametro[a].Value = iIdPosMovimientoCaja;
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
                        parametro[a].Value = "N";
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
                        parametro[a].Value = iIdPosMovimientoCaja;
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

                sSql = "";
                sSql += "update cv403_documentos_pagos set" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_pago = @id_pago" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[5];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_1";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "N";
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
                parametro[a].ParameterName = "@id_pago";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdPago;
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

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        //FUNCION PARA ELIMINAR EL PASO DE COMANDAS
        private bool eliminarPedido()
        {
            try
            {
                int a;
                //=================================================================================================================
                if (iBanderaEliminarBodega == 1)
                {
                    //SELECCIONAR EL ID MOVIMIENTO BODEGA
                    sSql = "";
                    sSql += "select id_movimiento_bodega" + Environment.NewLine;
                    sSql += "from cv402_cabecera_movimientos" + Environment.NewLine;
                    sSql += "where id_pedido = @id_pedido";

                    #region PARAMETROS

                    parametro = new SqlParameter[1];
                    parametro[0] = new SqlParameter();
                    parametro[0].ParameterName = "@id_pedido";
                    parametro[0].SqlDbType = SqlDbType.Int;
                    parametro[0].Value = iIdPedido;

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
                        int iIdCabeceraMovimiento = Convert.ToInt32(dtConsulta.Rows[0]["id_movimiento_bodega"].ToString());

                        sSql = "";
                        sSql += "update cv402_cabecera_movimientos set" + Environment.NewLine;
                        sSql += "estado = @estado_1," + Environment.NewLine;
                        sSql += "fecha_anula = getdate()," + Environment.NewLine;
                        sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                        sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                        sSql += "where id_movimiento_bodega = @id_movimiento_bodega" + Environment.NewLine;
                        sSql += "and estado = estado_2";

                        #region PARAMETROS

                        a = 0;
                        parametro = new SqlParameter[5];
                        parametro[a] = new SqlParameter();
                        parametro[a].ParameterName = "@estado_1";
                        parametro[a].SqlDbType = SqlDbType.VarChar;
                        parametro[a].Value = "N";
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
                        parametro[a].ParameterName = "@id_movimiento_bodega";
                        parametro[a].SqlDbType = SqlDbType.Int;
                        parametro[a].Value = iIdCabeceraMovimiento;
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
                        sSql += "update cv402_movimientos_bodega set" + Environment.NewLine;
                        sSql += "estado = @estado" + Environment.NewLine;
                        sSql += "where id_movimiento_bodega = @id_movimiento_bodega" + Environment.NewLine;
                        sSql += "and estado = estado_2";

                        #region PARAMETROS

                        a = 0;
                        parametro = new SqlParameter[3];
                        parametro[a] = new SqlParameter();
                        parametro[a].ParameterName = "@estado_1";
                        parametro[a].SqlDbType = SqlDbType.VarChar;
                        parametro[a].Value = "N";
                        a++;

                        parametro[a] = new SqlParameter();
                        parametro[a].ParameterName = "@id_movimiento_bodega";
                        parametro[a].SqlDbType = SqlDbType.Int;
                        parametro[a].Value = iIdCabeceraMovimiento;
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
                //=================================================================================================================

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
                parametro[a].Value = "N";
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
                sSql += "where id_documento_cobrar = @id_documento_cobrar" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[5];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_1";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "N";
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
                parametro[a].ParameterName = "@id_documento_cobrar";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdDocumentoCobrar;
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
                parametro[a].Value = "N";
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

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                //BUSCAMOS LAS CANTIDADES DESPACHADAS
                sSql = "";
                sSql += "select id_despacho_pedido, id_despacho" + Environment.NewLine;
                sSql += "from cv403_despachos_pedidos" + Environment.NewLine;
                sSql += "where id_pedido = @id_pedido" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[2];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pedido";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdPedido;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";

                #endregion

                dtAyuda = new DataTable();
                dtAyuda.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtAyuda, sSql, parametro);

                if (bRespuesta == false)
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                iIdCabDespacho = Convert.ToInt32(dtAyuda.Rows[0]["id_despacho"].ToString());

                for (int i = 0; i < dtAyuda.Rows.Count; i++)
                {
                    iIdDespachoPedido = Convert.ToInt32(dtAyuda.Rows[i]["id_despacho_pedido"].ToString());

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
                    parametro[a].Value = "N";
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

                    #endregion

                    if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                    {
                        sMensajeError = conexion.sMensajeError;
                        return false;
                    }
                }

                sSql = "";
                sSql += "update cv403_despachos_pedidos set" + Environment.NewLine;
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
                parametro[a].Value = "N";
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
                parametro[a].Value = "N";
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
                parametro[a].Value = iIdCabDespacho;
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
                parametro[a].Value = "N";
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

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                sSql = "";
                sSql += "update cv403_cab_pedidos set" + Environment.NewLine;
                sSql += "estado_orden = @estado_orden," + Environment.NewLine;
                sSql += "comentarios = @comentarios," + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_pedido = @id_pedido" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[7];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_orden";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "Cancelada";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@comentarios";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = sMotivo;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_1";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "N";
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

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        //FUNCION PARA ELIMINAR EL PASO DE LA TARJETA
        private bool eliminarPedidoTarjetaAlmuerzo()
        {
            try
            {
                int a;

                sSql = "";
                sSql += "update pos_tar_cab_movimiento set" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_pos_tar_cab_movimiento = @id_pos_tar_cab_movimiento" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[5];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_1";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "N";
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
                parametro[a].ParameterName = "@id_pos_tar_cab_movimiento";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdPosTarCabMovimiento;
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
                sSql += "update pos_tar_det_movimiento set" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_pos_tar_cab_movimiento = @id_pos_tar_cab_movimiento" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[5];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_1";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "N";
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
                parametro[a].ParameterName = "@id_pos_tar_cab_movimiento";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdPosTarCabMovimiento;
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

                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }
    }
}
