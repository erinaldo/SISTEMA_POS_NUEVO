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

namespace Palatium.Publicidad
{
    public partial class frmPublicidadReportes : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        Clases.ClaseFunciones funciones;

        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoOk ok;
        VentanasMensajes.frmMensajeNuevoSiNo SiNo;

        string sSql;
        string sFecha;

        DataTable dtConsulta;

        bool bRespuesta;

        int iIdLocalidad;
        int iCantidad;
        int iBanderaPrecuentaFactura;
        int iIdRegistro;

        SqlParameter[] parametro;

        public frmPublicidadReportes()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMBOBOX DE LOCALIDADES
        private void llenarComboLocalidades()
        {
            try
            {
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                funciones = new Clases.ClaseFunciones();

                if (funciones.llenarComboLocalidades() == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = funciones.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                dtConsulta = funciones.dtConsulta;
                cmbLocalidad.ValueMember = "id_localidad";
                cmbLocalidad.DisplayMember = "nombre_localidad";
                cmbLocalidad.DataSource = dtConsulta;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA CONSULTAR LA INFORMACIÓN
        private void consultarRegistro()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_mensaje_reportes, mensaje" + Environment.NewLine;
                sSql += "from pos_mensaje_reportes" + Environment.NewLine;
                sSql += "where id_localidad = @id_localidad" + Environment.NewLine;
                sSql += "and estado = @estado" + Environment.NewLine;

                if (rdbPrecuenta.Checked == true)
                    sSql += "and precuenta = @filtro";
                else
                    sSql += "and factura = @filtro";

                #region PARAMETROS

                parametro = new SqlParameter[3];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_localidad";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdLocalidad;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@estado";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = "A";

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@filtro";
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
                    return;
                }

                iCantidad = dtConsulta.Rows.Count;
                btnGrabar.Visible = true;
                btnBuscar.Visible = false;
                cmbLocalidad.Enabled = false;
                rdbPrecuenta.Enabled = false;
                rdbFactura.Enabled = false;
                iIdRegistro = 0;
                txtInfoPublicitaria.Enabled = true;
                txtInfoPublicitaria.Clear();

                if (iCantidad > 0)
                {
                    btnEliminar.Visible = true;
                    iIdRegistro = Convert.ToInt32(dtConsulta.Rows[0]["id_pos_mensaje_reportes"].ToString());
                    txtInfoPublicitaria.Text = dtConsulta.Rows[0]["mensaje"].ToString();
                }

                txtInfoPublicitaria.Focus();
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION LIMPIAR
        private void limpiar()
        {
            llenarComboLocalidades();
            rdbPrecuenta.Checked = true;
            txtInfoPublicitaria.Clear();
            iCantidad = 0;
            iIdLocalidad = 0;
            iIdRegistro = 0;
            btnGrabar.Visible = false;
            btnBuscar.Visible = true;
            btnEliminar.Visible = false;
            cmbLocalidad.Enabled = true;
            rdbPrecuenta.Enabled = true;
            rdbFactura.Enabled = true;
            txtInfoPublicitaria.Enabled = false;
            cmbLocalidad.Focus();
        }

        //FUNCION PARA GUARDAR EL REGISTRO
        private void guardarRegistro()
        {
            try
            {
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Error al inicar la transacción.";
                    ok.ShowDialog();
                    return;
                }

                int a;

                if (iCantidad > 0)
                {
                    sSql = "";
                    sSql += "update pos_mensaje_reportes" + Environment.NewLine;
                    sSql += "estado = @estado_1," + Environment.NewLine;
                    sSql += "fecha_anula = getdate()," + Environment.NewLine;
                    sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                    sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                    sSql += "where id_pos_mensaje_reportes = @id_pos_mensaje_reportes" + Environment.NewLine;
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
                    parametro[a].ParameterName = "@id_pos_mensaje_reportes";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdRegistro;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@estado_2";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = "A";

                    #endregion

                    if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                    {
                        catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                        catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                        catchMensaje.ShowDialog();
                        goto reversa;
                    }
                }

                int iPrecuenta, iFactura;

                if (iBanderaPrecuentaFactura == 0)
                {
                    iPrecuenta = 1;
                    iFactura = 0;
                }

                else
                {
                    iPrecuenta = 0;
                    iFactura = 1;
                }

                sSql = "";
                sSql += "insert into pos_mensaje_reportes (" + Environment.NewLine;
                sSql += "id_localidad, mensaje, precuenta, factura, estado, fecha_ingreso," + Environment.NewLine;
                sSql += "usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += "@id_localidad, @mensaje, @precuenta, @factura, @estado, getdate()," + Environment.NewLine;
                sSql += "@usuario_ingreso, @terminal_ingreso)";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[7];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_localidad";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdLocalidad;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@mensaje";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = txtInfoPublicitaria.Text.Trim().ToUpper();
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@precuenta";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iPrecuenta;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@factura";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iFactura;
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

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);

                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "El registro se ha guardado con éxito.";
                ok.ShowDialog();

                limpiar();
                return;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                goto reversa;
            }

            reversa: { conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION); return; }
        }

        //FUNCION PARA GUARDAR EL REGISTRO
        private void eliminarRegistro()
        {
            try
            {
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Error al inicar la transacción.";
                    ok.ShowDialog();
                    return;
                }

                int a;

                sSql = "";
                sSql += "update pos_mensaje_reportes" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_pos_mensaje_reportes = @id_pos_mensaje_reportes" + Environment.NewLine;
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
                parametro[a].ParameterName = "@id_pos_mensaje_reportes";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdRegistro;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_2";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);

                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "El registro se ha eliminado con éxito.";
                ok.ShowDialog();

                limpiar();
                return;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                goto reversa;
            }

        reversa: { conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION); return; }
        }
        
        #endregion

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPublicidadReportes_Load(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbLocalidad.SelectedValue) == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor seleccione la localidad.";
                ok.ShowDialog();
                cmbLocalidad.Focus();
                return;
            }

            iIdLocalidad = Convert.ToInt32(cmbLocalidad.SelectedValue);

            if (rdbPrecuenta.Checked == true)
                iBanderaPrecuentaFactura = 0;
            else if (rdbFactura.Checked == true)
                iBanderaPrecuentaFactura = 1;

            consultarRegistro();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (txtInfoPublicitaria.Text.Trim() == "")
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor ingrese un mensaje de publicidad.";
                ok.ShowDialog();
                txtInfoPublicitaria.Focus();
                return;
            }

            SiNo = new VentanasMensajes.frmMensajeNuevoSiNo();
            SiNo.lblMensaje.Text = "¿Está seguro que desea guardar el registro?";
            SiNo.ShowDialog();

            if (SiNo.DialogResult == DialogResult.OK)
                guardarRegistro();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            SiNo = new VentanasMensajes.frmMensajeNuevoSiNo();
            SiNo.lblMensaje.Text = "¿Está seguro que desea eliminar el registro?";
            SiNo.ShowDialog();

            if (SiNo.DialogResult == DialogResult.OK)
                eliminarRegistro();
        }
    }
}
