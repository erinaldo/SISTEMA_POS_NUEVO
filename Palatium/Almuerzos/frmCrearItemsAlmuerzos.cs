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

namespace Palatium.Almuerzos
{
    public partial class frmCrearItemsAlmuerzos : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoOk ok;
        VentanasMensajes.frmMensajeNuevoSiNo SiNo;

        Modals.frmModalGrid modal;

        string sSql;
        string sFecha;
        string sTabla;
        string sCampo;
        
        int iBanderaEditar;
        int iIdLocalidad;
        int iIdProductoPadre;
        int iIdRegistro;
        int iHabilitado;
        int iDespachado;

        long iMaximo;

        DataTable dtConsulta;
        DataTable dtProductos;

        bool bRespuesta;

        SqlParameter[] parametro;

        public frmCrearItemsAlmuerzos(string sFecha_P, int iBanderaEditar_P, int iIdLocalidad_P, int iIdRegistro_P)
        {
            this.sFecha = sFecha_P;
            this.iBanderaEditar = iBanderaEditar_P;
            this.iIdLocalidad = iIdLocalidad_P;
            this.iIdRegistro = iIdRegistro_P;
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA CARGAR EL MODAL DE PROUCTOS
        private bool crearTablaProductos()
        {
            try
            {
                //sSql = "";
                //sSql += "select id_producto from cv401_productos" + Environment.NewLine;
                //sSql += "where estado = @estado" + Environment.NewLine;
                //sSql += "and nivel = @nivel" + Environment.NewLine;
                //sSql += "and codigo = @codigo";

                //#region PARAMETROS

                //parametro = new SqlParameter[3];
                //parametro[0] = new SqlParameter();
                //parametro[0].ParameterName = "@estado";
                //parametro[0].SqlDbType = SqlDbType.VarChar;
                //parametro[0].Value = "A";

                //parametro[1] = new SqlParameter();
                //parametro[1].ParameterName = "@nivel";
                //parametro[1].SqlDbType = SqlDbType.Int;
                //parametro[1].Value = 1;

                //parametro[2] = new SqlParameter();
                //parametro[2].ParameterName = "@codigo";
                //parametro[2].SqlDbType = SqlDbType.VarChar;
                //parametro[2].Value = "2";

                //#endregion

                //dtConsulta = new DataTable();
                //dtConsulta.Clear();

                //bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtConsulta, sSql, parametro);

                //if (bRespuesta == false)
                //{
                //    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                //    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                //    catchMensaje.ShowDialog();
                //    return false;
                //}

                //if (dtConsulta.Rows.Count == 0)
                //{
                //    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                //    ok.lblMensaje.Text = "No existe la configuración del código principal del producto";
                //    ok.ShowDialog();
                //    return false;
                //}

                //iIdProductoPadre = Convert.ToInt32(dtConsulta.Rows[0]["id_producto"].ToString());

                sSql = "";
                sSql += "select P.id_producto, P.codigo, NP.nombre" + Environment.NewLine;
                sSql += "from cv401_productos P INNER JOIN" + Environment.NewLine;
                sSql += "cv401_nombre_productos NP ON P.id_producto = NP.id_producto" + Environment.NewLine;
                sSql += "and P.estado = @estado_1" + Environment.NewLine;
                sSql += "and NP.estado = @estado_2" + Environment.NewLine;
                sSql += "where P.nivel in (@nivel_3, @nivel_4)" + Environment.NewLine;
                sSql += "and P.is_active = @is_active" + Environment.NewLine;
                sSql += "order by NP.nombre";

                #region PARAMETROS

                int a = 0;
                parametro = new SqlParameter[5];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_1";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_2";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@nivel_3";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 3;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@nivel_4";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 4;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@is_active";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 1;

                #endregion

                dtProductos = new DataTable();
                dtProductos.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtProductos, sSql, parametro);

                if (bRespuesta == false)
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

        //VALIDAR LOS DATOS A GUARDAR
        private void validarDatos()
        {
            int iSumaSopas = dgvSopas.Rows.Count;
            int iSumaSegundos = dgvSegundos.Rows.Count;
            int iSumaJugos = dgvJugos.Rows.Count;
            int iSumaPostres = dgvPostres.Rows.Count;

            int iSuma_P = iSumaSopas + iSumaSegundos + iSumaJugos + iSumaPostres;

            if (iSuma_P == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "No existen registros para guardar en el sistema.";
                ok.ShowDialog();
                return;
            }

            if (chkHabilitado.Checked == true)
                iHabilitado = 1;
            else
                iHabilitado = 0;

            if (chkDespachado.Checked == true)
                iDespachado = 1;
            else
                iDespachado = 0;

            SiNo = new VentanasMensajes.frmMensajeNuevoSiNo();
            SiNo.lblMensaje.Text = "¿Está seguro que desea guardar el registro?";
            SiNo.ShowDialog();

            if (SiNo.DialogResult == DialogResult.OK)
            {
                guardarRegistros();
            }
        }

        //FUNCION PARA GUARDAR LOS REGISTROS
        private void guardarRegistros()
        {
            try
            {
                int a;

                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Error al iniciar la transacción para guardar los registros.";
                    ok.ShowDialog();
                    return;
                }

                if (iBanderaEditar == 1)
                {
                    sSql = "";
                    sSql += "update pos_cab_calendario_almuerzo set" + Environment.NewLine;
                    sSql += "observaciones = @observaciones," + Environment.NewLine;
                    sSql += "despachado = @despachado," + Environment.NewLine;
                    sSql += "is_active = @is_active" + Environment.NewLine;
                    sSql += "where id_pos_cab_calendario_almuerzo = @id_pos_cab_calendario_almuerzo" + Environment.NewLine;
                    sSql += "and estado = @estado";

                    #region PARAMETROS

                    a = 0;
                    parametro = new SqlParameter[5];
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@observaciones";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = txtObservacion.Text.Trim().ToUpper();
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@despachado";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iDespachado;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@is_active";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iHabilitado;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_pos_cab_calendario_almuerzo";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdRegistro;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@estado";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = "A";

                    #endregion

                    //EJECUTAR LA INSTRUCCION SQL
                    if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                    {
                        catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                        catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                        catchMensaje.ShowDialog();
                        goto reversa;
                    }

                    #region CAMBIO A ESTADO "E" EN LA TABLA POS_ALMUERZO_SOPA
                    sSql = "";
                    sSql += "update pos_almuerzo_sopas set" + Environment.NewLine;
                    sSql += "estado = @estado_1," + Environment.NewLine;
                    sSql += "fecha_anula = getdate()," + Environment.NewLine;
                    sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                    sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                    sSql += "where id_pos_cab_calendario_almuerzo = @id_pos_cab_calendario_almuerzo" + Environment.NewLine;
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
                    parametro[a].ParameterName = "@id_pos_cab_calendario_almuerzo";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdRegistro;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@estado_2";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = "A";

                    #endregion

                    //EJECUTAR LA INSTRUCCION SQL
                    if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                    {
                        catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                        catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                        catchMensaje.ShowDialog();
                        goto reversa;
                    }

                    #endregion

                    #region CAMBIO A ESTADO "E" EN LA TABLA POS_ALMUERZO_SEGUNDOS
                    sSql = "";
                    sSql += "update pos_almuerzo_segundos set" + Environment.NewLine;
                    sSql += "estado = @estado_1," + Environment.NewLine;
                    sSql += "fecha_anula = getdate()," + Environment.NewLine;
                    sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                    sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                    sSql += "where id_pos_cab_calendario_almuerzo = @id_pos_cab_calendario_almuerzo" + Environment.NewLine;
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
                    parametro[a].ParameterName = "@id_pos_cab_calendario_almuerzo";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdRegistro;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@estado_2";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = "A";

                    #endregion

                    //EJECUTAR LA INSTRUCCION SQL
                    if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                    {
                        catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                        catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                        catchMensaje.ShowDialog();
                        goto reversa;
                    }

                    #endregion

                    #region CAMBIO A ESTADO "E" EN LA TABLA POS_ALMUERZO_JUGOS
                    sSql = "";
                    sSql += "update pos_almuerzo_jugos set" + Environment.NewLine;
                    sSql += "estado = @estado_1," + Environment.NewLine;
                    sSql += "fecha_anula = getdate()," + Environment.NewLine;
                    sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                    sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                    sSql += "where id_pos_cab_calendario_almuerzo = @id_pos_cab_calendario_almuerzo" + Environment.NewLine;
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
                    parametro[a].ParameterName = "@id_pos_cab_calendario_almuerzo";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdRegistro;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@estado_2";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = "A";

                    #endregion

                    //EJECUTAR LA INSTRUCCION SQL
                    if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                    {
                        catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                        catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                        catchMensaje.ShowDialog();
                        goto reversa;
                    }

                    #endregion

                    #region CAMBIO A ESTADO "E" EN LA TABLA POS_ALMUERZO_POSTRES
                    sSql = "";
                    sSql += "update pos_almuerzo_postres set" + Environment.NewLine;
                    sSql += "estado = @estado_1," + Environment.NewLine;
                    sSql += "fecha_anula = getdate()," + Environment.NewLine;
                    sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                    sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                    sSql += "where id_pos_cab_calendario_almuerzo = @id_pos_cab_calendario_almuerzo" + Environment.NewLine;
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
                    parametro[a].ParameterName = "@id_pos_cab_calendario_almuerzo";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdRegistro;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@estado_2";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = "A";

                    #endregion

                    //EJECUTAR LA INSTRUCCION SQL
                    if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                    {
                        catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                        catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                        catchMensaje.ShowDialog();
                        goto reversa;
                    }

                    #endregion
                }

                else
                {
                    sSql = "";
                    sSql += "insert into pos_cab_calendario_almuerzo (" + Environment.NewLine;
                    sSql += "id_localidad, fecha, observaciones, despachado, is_active," + Environment.NewLine;
                    sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                    sSql += "values (" + Environment.NewLine;
                    sSql += "@id_localidad, @fecha, @observaciones, @despachado, @is_active," + Environment.NewLine;
                    sSql += "@estado, getdate(), @usuario_ingreso, @terminal_ingreso)";

                    #region PARAMETROS

                    a = 0;
                    parametro = new SqlParameter[8];
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_localidad";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdLocalidad;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@fecha";
                    parametro[a].SqlDbType = SqlDbType.DateTime;
                    parametro[a].Value = Convert.ToDateTime(sFecha);
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@observaciones";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = txtObservacion.Text.Trim().ToUpper();
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@despachado";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iDespachado;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@is_active";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iHabilitado;
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
                    parametro[a].Value = Program.sDatosMaximo[1];

                    #endregion

                    //EJECUTAR LA INSTRUCCION SQL
                    if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                    {
                        catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                        catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                        catchMensaje.ShowDialog();
                        goto reversa;
                    }

                    sTabla = "pos_cab_calendario_almuerzo";
                    sCampo = "id_pos_cab_calendario_almuerzo";

                    iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                    if (iMaximo == -1)
                    {
                        ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                        ok.lblMensaje.Text = "Error al obtener el identificador del registro.";
                        ok.ShowDialog();
                        goto reversa;
                    }

                    iIdRegistro = Convert.ToInt32(iMaximo);
                }

                int iIdProducto_A;

                //INSERTAR EN LA TABLA POS_ALMUERZO_SOPAS
                for (int i = 0; i < dgvSopas.Rows.Count; i++)
                {
                    iIdProducto_A = Convert.ToInt32(dgvSopas.Rows[i].Cells["id_producto_sopa"].Value);

                    sSql = "";
                    sSql += "insert into pos_almuerzo_sopas (" + Environment.NewLine;
                    sSql += "id_pos_cab_calendario_almuerzo, id_producto, is_active, estado," + Environment.NewLine;
                    sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                    sSql += "values (" + Environment.NewLine;
                    sSql += "@id_pos_cab_calendario_almuerzo, @id_producto, @is_active, @estado," + Environment.NewLine;
                    sSql += "getdate(), @usuario_ingreso, @terminal_ingreso)";

                    #region PARAMETROS

                    a = 0;
                    parametro = new SqlParameter[6];
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_pos_cab_calendario_almuerzo";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdRegistro;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_producto";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdProducto_A;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@is_active";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = 1;
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
                    parametro[a].Value = Program.sDatosMaximo[1];

                    #endregion

                    //EJECUTAR LA INSTRUCCION SQL
                    if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                    {
                        catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                        catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                        catchMensaje.ShowDialog();
                        goto reversa;
                    }
                }

                //INSERTAR EN LA TABLA POS_ALMUERZO_SEGUNDOS
                for (int i = 0; i < dgvSegundos.Rows.Count; i++)
                {
                    iIdProducto_A = Convert.ToInt32(dgvSegundos.Rows[i].Cells["id_producto_segundo"].Value);

                    sSql = "";
                    sSql += "insert into pos_almuerzo_segundos (" + Environment.NewLine;
                    sSql += "id_pos_cab_calendario_almuerzo, id_producto, is_active, estado," + Environment.NewLine;
                    sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                    sSql += "values (" + Environment.NewLine;
                    sSql += "@id_pos_cab_calendario_almuerzo, @id_producto, @is_active, @estado," + Environment.NewLine;
                    sSql += "getdate(), @usuario_ingreso, @terminal_ingreso)";

                    #region PARAMETROS

                    a = 0;
                    parametro = new SqlParameter[6];
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_pos_cab_calendario_almuerzo";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdRegistro;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_producto";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdProducto_A;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@is_active";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = 1;
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
                    parametro[a].Value = Program.sDatosMaximo[1];

                    #endregion

                    //EJECUTAR LA INSTRUCCION SQL
                    if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                    {
                        catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                        catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                        catchMensaje.ShowDialog();
                        goto reversa;
                    }
                }

                //INSERTAR EN LA TABLA POS_ALMUERZO_JUGOS
                for (int i = 0; i < dgvJugos.Rows.Count; i++)
                {
                    iIdProducto_A = Convert.ToInt32(dgvJugos.Rows[i].Cells["id_producto_jugo"].Value);

                    sSql = "";
                    sSql += "insert into pos_almuerzo_jugos (" + Environment.NewLine;
                    sSql += "id_pos_cab_calendario_almuerzo, id_producto, is_active, estado," + Environment.NewLine;
                    sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                    sSql += "values (" + Environment.NewLine;
                    sSql += "@id_pos_cab_calendario_almuerzo, @id_producto, @is_active, @estado," + Environment.NewLine;
                    sSql += "getdate(), @usuario_ingreso, @terminal_ingreso)";

                    #region PARAMETROS

                    a = 0;
                    parametro = new SqlParameter[6];
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_pos_cab_calendario_almuerzo";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdRegistro;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_producto";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdProducto_A;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@is_active";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = 1;
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
                    parametro[a].Value = Program.sDatosMaximo[1];

                    #endregion

                    //EJECUTAR LA INSTRUCCION SQL
                    if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                    {
                        catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                        catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                        catchMensaje.ShowDialog();
                        goto reversa;
                    }
                }

                //INSERTAR EN LA TABLA POS_ALMUERZO_POSTRES
                for (int i = 0; i < dgvPostres.Rows.Count; i++)
                {
                    iIdProducto_A = Convert.ToInt32(dgvPostres.Rows[i].Cells["id_producto_postre"].Value);

                    sSql = "";
                    sSql += "insert into pos_almuerzo_postres (" + Environment.NewLine;
                    sSql += "id_pos_cab_calendario_almuerzo, id_producto, is_active, estado," + Environment.NewLine;
                    sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                    sSql += "values (" + Environment.NewLine;
                    sSql += "@id_pos_cab_calendario_almuerzo, @id_producto, @is_active, @estado," + Environment.NewLine;
                    sSql += "getdate(), @usuario_ingreso, @terminal_ingreso)";

                    #region PARAMETROS

                    a = 0;
                    parametro = new SqlParameter[6];
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_pos_cab_calendario_almuerzo";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdRegistro;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_producto";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdProducto_A;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@is_active";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = 1;
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
                    parametro[a].Value = Program.sDatosMaximo[1];

                    #endregion

                    //EJECUTAR LA INSTRUCCION SQL
                    if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                    {
                        catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                        catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                        catchMensaje.ShowDialog();
                        goto reversa;
                    }
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);

                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Registros guardados éxitosamente.";
                ok.ShowDialog();
                this.DialogResult = DialogResult.OK;
                return;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }

            reversa: { conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION); }
        }

        //FUNCION PARA CONSULTAR LOS REGISTROS GUARDADOS
        private bool consultarRegistros()
        {
            try
            {
                int a;

                sSql = "";
                sSql += "select observaciones, is_active" + Environment.NewLine;
                sSql += "from pos_cab_calendario_almuerzo" + Environment.NewLine;
                sSql += "where estado = @estado" + Environment.NewLine;
                sSql += "and id_pos_cab_calendario_almuerzo = @id_pos_cab_calendario_almuerzo";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[2];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pos_cab_calendario_almuerzo";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdRegistro;

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
                    ok.lblMensaje.Text = "No se encuentra información del registro";
                    ok.ShowDialog();
                    return false;
                }

                else
                {
                    txtObservacion.Text = dtConsulta.Rows[0]["observaciones"].ToString();

                    if (Convert.ToInt32(dtConsulta.Rows[0]["is_active"].ToString()) == 1)
                        chkHabilitado.Checked = true;
                    else
                        chkHabilitado.Checked = false;
                }

                #region BUSCAR REGISTROS EN POS_ALMUERZO_SOPAS

                sSql = "";
                sSql += "select * from pos_vw_almuerzo_sopas" + Environment.NewLine;
                sSql += "where id_pos_cab_calendario_almuerzo = @id_pos_cab_calendario_almuerzo";

                #region PARAMETROS

                parametro = new SqlParameter[1];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_pos_cab_calendario_almuerzo";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdRegistro;

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

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    dgvSopas.Rows.Add(dtConsulta.Rows[i]["id_producto"].ToString(),
                                      dtConsulta.Rows[i]["codigo"].ToString().Trim().ToUpper(),
                                      dtConsulta.Rows[i]["nombre"].ToString().Trim().ToUpper());
                }

                #endregion

                #region BUSCAR REGISTROS EN POS_ALMUERZO_SEGUNDOS

                sSql = "";
                sSql += "select * from pos_vw_almuerzo_segundos" + Environment.NewLine;
                sSql += "where id_pos_cab_calendario_almuerzo = @id_pos_cab_calendario_almuerzo";

                #region PARAMETROS

                parametro = new SqlParameter[1];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_pos_cab_calendario_almuerzo";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdRegistro;

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

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    dgvSegundos.Rows.Add(dtConsulta.Rows[i]["id_producto"].ToString(),
                                         dtConsulta.Rows[i]["codigo"].ToString().Trim().ToUpper(),
                                         dtConsulta.Rows[i]["nombre"].ToString().Trim().ToUpper());
                }

                #endregion

                #region BUSCAR REGISTROS EN POS_ALMUERZO_JUGOS

                sSql = "";
                sSql += "select * from pos_vw_almuerzo_jugos" + Environment.NewLine;
                sSql += "where id_pos_cab_calendario_almuerzo = @id_pos_cab_calendario_almuerzo";

                #region PARAMETROS

                parametro = new SqlParameter[1];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_pos_cab_calendario_almuerzo";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdRegistro;

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

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    dgvJugos.Rows.Add(dtConsulta.Rows[i]["id_producto"].ToString(),
                                      dtConsulta.Rows[i]["codigo"].ToString().Trim().ToUpper(),
                                      dtConsulta.Rows[i]["nombre"].ToString().Trim().ToUpper());
                }

                #endregion

                #region BUSCAR REGISTROS EN POS_ALMUERZO_POSTRES

                sSql = "";
                sSql += "select * from pos_vw_almuerzo_postres" + Environment.NewLine;
                sSql += "where id_pos_cab_calendario_almuerzo = @id_pos_cab_calendario_almuerzo";

                #region PARAMETROS

                parametro = new SqlParameter[1];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_pos_cab_calendario_almuerzo";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdRegistro;

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

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    dgvPostres.Rows.Add(dtConsulta.Rows[i]["id_producto"].ToString(),
                                        dtConsulta.Rows[i]["codigo"].ToString().Trim().ToUpper(),
                                        dtConsulta.Rows[i]["nombre"].ToString().Trim().ToUpper());
                }

                #endregion

                dgvSopas.ClearSelection();
                dgvSegundos.ClearSelection();
                dgvJugos.ClearSelection();
                dgvPostres.ClearSelection();

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
            dgvSopas.Rows.Clear();
            dgvSegundos.Rows.Clear();
            dgvJugos.Rows.Clear();
            dgvPostres.Rows.Clear();
            txtObservacion.Clear();

        }

        #endregion

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCrearItemsAlmuerzos_Load(object sender, EventArgs e)
        {
            txtFecha.Text = Convert.ToDateTime(sFecha).ToString("dd-MM-yyyy");
            crearTablaProductos();

            if (iBanderaEditar == 1)
            {
                chkHabilitado.Enabled = true;
                consultarRegistros();
            }

            else
            {
                chkHabilitado.Checked = true;
                chkHabilitado.Enabled = false;
            }
        }

        private void btnAgregarSopas_Click(object sender, EventArgs e)
        {
            if (dtProductos.Rows.Count == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "No existen productos configurados en el sistema.";
                ok.ShowDialog();
                return;
            }

            modal = new Modals.frmModalGrid(dtProductos, 0, 1, 2);
            modal.ShowDialog();

            if (modal.DialogResult == DialogResult.OK)
            {
                int iBandera_P = 0;
                int iIdProductoGrid;
                int iIdProducto_R = modal.iId;
                string sCodigo_R = modal.sDatosConsulta;
                string sDescripcion_P = modal.sDescripcion;
                modal.Close();

                for (int i = 0; i < dgvSopas.Rows.Count; i++)
                {
                    iIdProductoGrid = Convert.ToInt32(dgvSopas.Rows[i].Cells["id_producto_sopa"].Value);

                    if (iIdProductoGrid == iIdProducto_R)
                    {
                        iBandera_P = 1;
                        break;
                    }
                }

                if (iBandera_P == 0)
                {
                    dgvSopas.Rows.Add(iIdProducto_R, sCodigo_R, sDescripcion_P);
                    dgvSopas.ClearSelection();
                }

                else
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "El producto seleccionad ya se encuentra en la lista de sopas.";
                    ok.ShowDialog();
                    dgvSopas.ClearSelection();
                }
            }
        }

        private void dgvSopas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int iIndice = e.RowIndex;

                if (dgvSopas.Columns[e.ColumnIndex].Name == "remover_sopa")
                {
                    dgvSopas.Rows.RemoveAt(iIndice);
                }

                dgvSopas.ClearSelection();
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        private void btnAgregarSegundos_Click(object sender, EventArgs e)
        {
            if (dtProductos.Rows.Count == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "No existen productos configurados en el sistema.";
                ok.ShowDialog();
                return;
            }

            modal = new Modals.frmModalGrid(dtProductos, 0, 1, 2);
            modal.ShowDialog();

            if (modal.DialogResult == DialogResult.OK)
            {
                int iBandera_P = 0;
                int iIdProductoGrid;
                int iIdProducto_R = modal.iId;
                string sCodigo_R = modal.sDatosConsulta;
                string sDescripcion_P = modal.sDescripcion;
                modal.Close();

                for (int i = 0; i < dgvSegundos.Rows.Count; i++)
                {
                    iIdProductoGrid = Convert.ToInt32(dgvSegundos.Rows[i].Cells["id_producto_segundo"].Value);

                    if (iIdProductoGrid == iIdProducto_R)
                    {
                        iBandera_P = 1;
                        break;
                    }
                }

                if (iBandera_P == 0)
                {
                    dgvSegundos.Rows.Add(iIdProducto_R, sCodigo_R, sDescripcion_P);
                    dgvSegundos.ClearSelection();
                }

                else
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "El producto seleccionad ya se encuentra en la lista de sopas.";
                    ok.ShowDialog();
                    dgvSopas.ClearSelection();
                }
            }
        }

        private void dgvSegundos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int iIndice = e.RowIndex;

                if (dgvSegundos.Columns[e.ColumnIndex].Name == "remover_segundo")
                {
                    dgvSegundos.Rows.RemoveAt(iIndice);
                }

                dgvSegundos.ClearSelection();
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        private void btnAgregarJugos_Click(object sender, EventArgs e)
        {
            if (dtProductos.Rows.Count == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "No existen productos configurados en el sistema.";
                ok.ShowDialog();
                return;
            }

            modal = new Modals.frmModalGrid(dtProductos, 0, 1, 2);
            modal.ShowDialog();

            if (modal.DialogResult == DialogResult.OK)
            {
                int iBandera_P = 0;
                int iIdProductoGrid;
                int iIdProducto_R = modal.iId;
                string sCodigo_R = modal.sDatosConsulta;
                string sDescripcion_P = modal.sDescripcion;
                modal.Close();

                for (int i = 0; i < dgvJugos.Rows.Count; i++)
                {
                    iIdProductoGrid = Convert.ToInt32(dgvJugos.Rows[i].Cells["id_producto_jugo"].Value);

                    if (iIdProductoGrid == iIdProducto_R)
                    {
                        iBandera_P = 1;
                        break;
                    }
                }

                if (iBandera_P == 0)
                {
                    dgvJugos.Rows.Add(iIdProducto_R, sCodigo_R, sDescripcion_P);
                    dgvJugos.ClearSelection();
                }

                else
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "El producto seleccionad ya se encuentra en la lista de sopas.";
                    ok.ShowDialog();
                    dgvJugos.ClearSelection();
                }
            }
        }

        private void dgvJugos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int iIndice = e.RowIndex;

                if (dgvJugos.Columns[e.ColumnIndex].Name == "remover_jugo")
                {
                    dgvJugos.Rows.RemoveAt(iIndice);
                }

                dgvJugos.ClearSelection();
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        private void btnAgregarPostres_Click(object sender, EventArgs e)
        {
            if (dtProductos.Rows.Count == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "No existen productos configurados en el sistema.";
                ok.ShowDialog();
                return;
            }

            modal = new Modals.frmModalGrid(dtProductos, 0, 1, 2);
            modal.ShowDialog();

            if (modal.DialogResult == DialogResult.OK)
            {
                int iBandera_P = 0;
                int iIdProductoGrid;
                int iIdProducto_R = modal.iId;
                string sCodigo_R = modal.sDatosConsulta;
                string sDescripcion_P = modal.sDescripcion;
                modal.Close();

                for (int i = 0; i < dgvPostres.Rows.Count; i++)
                {
                    iIdProductoGrid = Convert.ToInt32(dgvPostres.Rows[i].Cells["id_producto_postre"].Value);

                    if (iIdProductoGrid == iIdProducto_R)
                    {
                        iBandera_P = 1;
                        break;
                    }
                }

                if (iBandera_P == 0)
                {
                    dgvPostres.Rows.Add(iIdProducto_R, sCodigo_R, sDescripcion_P);
                    dgvPostres.ClearSelection();
                }

                else
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "El producto seleccionad ya se encuentra en la lista de sopas.";
                    ok.ShowDialog();
                    dgvPostres.ClearSelection();
                }
            }
        }

        private void dgvPostres_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int iIndice = e.RowIndex;

                if (dgvPostres.Columns[e.ColumnIndex].Name == "remover_postre")
                {
                    dgvPostres.Rows.RemoveAt(iIndice);
                }

                dgvPostres.ClearSelection();
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            validarDatos();
        }        
    }
}
