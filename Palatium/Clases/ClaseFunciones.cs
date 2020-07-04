using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palatium.Clases
{
    class ClaseFunciones
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        string sSql;
        public string sMensajeError;
        public string sFechaRecuperada;
        
        public DateTime dtFechaHoraRecuperada;

        bool bRespuesta;

        public DataTable dtConsulta;

        SqlParameter[] parametro;

        //FUNCION PARA CONSULTAR LA FECHA DEL SISTEMA
        public bool fechaSistema()
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

                sFechaRecuperada = Convert.ToDateTime(dtConsulta.Rows[0]["fecha"].ToString()).ToString("dd-MM-yyyy");
                dtFechaHoraRecuperada = Convert.ToDateTime(dtConsulta.Rows[0]["fecha"].ToString());
                return true;
            }

            catch (Exception ex)
            {
                sFechaRecuperada = ex.Message;
                return false;
            }
        }

        //LLENAR EL COMBOBX DE LOCALIDADES
        public bool llenarComboLocalidades()
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
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }

                DataRow row = dtConsulta.NewRow();
                row["id_localidad"] = 0;
                row["nombre_localidad"] = "Seleccione...!!!";
                dtConsulta.Rows.InsertAt(row, 0);

                return true;
            }

            catch (Exception ex)
            {
                sFechaRecuperada = ex.Message;
                return false;
            }
        }

        //FUNCION PARA EXTRAER EL MENSAJE DE PRECUENTA
        public bool mensajePrecuenta(int iIdLocalidad_P)
        {
            try
            {
                sSql = ""; 
                sSql += "select mensaje" + Environment.NewLine;
                sSql += "from pos_mensaje_reportes" + Environment.NewLine;
                sSql += "where estado = @estado" + Environment.NewLine;
                sSql += "and id_localidad = @id_localidad" + Environment.NewLine;
                sSql += "and precuenta = @precuenta";

                #region PARAMETROS  

                parametro = new SqlParameter[3];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@estado";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "A";

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@id_localidad";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = iIdLocalidad_P;

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@precuenta";
                parametro[2].SqlDbType = SqlDbType.Int;
                parametro[2].Value = 1;

                #endregion

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtConsulta, sSql, parametro);

                if (bRespuesta == false)
                {
                    sMensajeError = conexion.sMensajeError;
                    return false;
                }
                
                return true;
            }

            catch (Exception ex)
            {
                sFechaRecuperada = ex.Message;
                return false;
            }
        }

        public byte[] codigoBarrasEAN13(string sTexto_P)
        {
            BarcodeLib.Barcode codigo = new BarcodeLib.Barcode();
            codigo.IncludeLabel = true;

            var ms = new MemoryStream();

            Bitmap imgOK = new Bitmap(codigo.Encode(BarcodeLib.TYPE.EAN13, sTexto_P.ToString(), Color.Black, Color.White, 500, 150));

            imgOK.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            return ms.ToArray();
        }

        //FUNCION PARA CONSULTAR DATOS DEL CLIENTE
        public bool consultarRegistroPersona(string sIdentificacion_P, int iIdPersona_P, int iBanderaTipoConsulta_P)
        {
            try
            {
                // iBanderaTipoConsulta_P
                //  0 - Consultar con identificacion
                //  1 - Consultar con el id_persona

                sSql = "";
                sSql += "select * from pos_vw_buscar_cliente_facturacion" + Environment.NewLine;

                if (iBanderaTipoConsulta_P == 0)
                    sSql += "where identificacion = @identificacion";
                else
                    sSql += "where id_persona = @id_persona";

                #region PARAMETROS

                parametro = new SqlParameter[1];
                parametro[0] = new SqlParameter();

                if (iBanderaTipoConsulta_P == 0)
                {
                    parametro[0].ParameterName = "@identificacion";
                    parametro[0].SqlDbType = SqlDbType.VarChar;
                    parametro[0].Value = sIdentificacion_P;
                }

                else
                {
                    parametro[0].ParameterName = "@id_persona";
                    parametro[0].SqlDbType = SqlDbType.Int;
                    parametro[0].Value = iIdPersona_P;
                }

                #endregion


                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtConsulta, sSql, parametro);

                if (bRespuesta == false)
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

        //VERIFICAR SI LA CADENA ES UN NUMERO O UN STRING
        public bool esNumero(object Expression)
        {

            bool isNum;

            double retNum;

            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);

            return isNum;
        }
    }
}
