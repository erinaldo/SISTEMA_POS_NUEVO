using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Palatium.Formularios
{
    public partial class FInformacionPosSecMes : MaterialForm
    {
        //VARIABLES, INSTANCIAS
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoOk ok;
        VentanasMensajes.frmMensajeNuevoSiNo SiNo;

        bool bRespuesta;

        DataTable dtConsulta;

        string sSql;
        string sEstado;
        string sFecha;

        int iIdPosSeccionMesa;
        int iCuenta;
        int iHabilitado;

        SqlParameter[] parametro;

        public FInformacionPosSecMes()
        {
            InitializeComponent();
        }

        private void FInformacionPosSecMes_Load(object sender, EventArgs e)
        {
            cmbLocalidades.SelectedIndexChanged -= new EventHandler(cmbLocalidades_SelectedIndexChanged);
            llenarComboLocalidades();
            cmbLocalidades.SelectedValue = Program.iIdLocalidad;
            cmbLocalidades.SelectedIndexChanged += new EventHandler(cmbLocalidades_SelectedIndexChanged);

            llenarGrid();
            cargarColores();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMBO DE LOCALIDADES
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

                cmbLocalidades.DisplayMember = "nombre_localidad";
                cmbLocalidades.ValueMember = "id_localidad";
                cmbLocalidades.DataSource = dtConsulta;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA CARGAR LOS COLORES 
        private void cargarColores()
        {
            try
            {
                cmbPaleta.Items.Clear();
                cmbPaleta.Items.Add(Color.Azure);
                cmbPaleta.Items.Add(Color.LightSteelBlue);
                cmbPaleta.Items.Add(Color.MediumPurple);
                cmbPaleta.Items.Add(Color.LightCoral);
                cmbPaleta.Items.Add(Color.Chocolate);
                cmbPaleta.Items.Add(Color.NavajoWhite);
                cmbPaleta.Items.Add(Color.Gold);
                cmbPaleta.Items.Add(Color.GreenYellow);
                cmbPaleta.Items.Add(Color.Wheat);
                cmbPaleta.Items.Add(Color.DarkTurquoise);

                cmbPaleta.SelectedIndex = 0;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }
        
        //LIPIAR LAS CAJAS DE TEXTO
        private void limpiarTodo()
        {
            cmbLocalidades.Enabled = true;
            iIdPosSeccionMesa = 0;

            chkHabilitado.Checked = true;
            chkHabilitado.Enabled = false;

            txtBuscar.Clear();
            txtCodigo.Enabled = true;
            txtCodigo.Clear();
            txtDescripcion.Clear();
            txtRuta.Clear();
            btnAnular.Enabled = false;
            llenarGrid();
        }

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid()
        {
            try
            {
                dgvDatos.Rows.Clear();

                int iParametro_P = 2;
                int iBandera_P = 0;

                sSql = "";
                sSql += "select id_pos_seccion_mesa, id_localidad, is_active," + Environment.NewLine;
                sSql += "isnull(fondo_pantalla, '') fondo_pantalla, color, codigo, descripcion," + Environment.NewLine;
                sSql += "CASE is_active when 1 then 'ACTIVO' else 'INACTIVO' end estado" + Environment.NewLine;
                sSql += "from pos_seccion_mesa" + Environment.NewLine;
                sSql += "where estado = @estado" + Environment.NewLine;
                sSql += "and id_localidad = @id_localidad" + Environment.NewLine;

                if (txtBuscar.Text.Trim() != "")
                {
                    iParametro_P++;
                    iBandera_P = 1;
                    sSql += "and (codigo like '%' + @buscar + '%'" + Environment.NewLine;
                    sSql += "or descripcion like '%' + @buscar + '%')" + Environment.NewLine;
                    sSql += "" + Environment.NewLine;
                }

                sSql += "order by id_pos_seccion_mesa";

                #region PARAMETROS

                parametro = new SqlParameter[iParametro_P];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@estado";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "A";

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@id_localidad";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = Convert.ToInt32(cmbLocalidades.SelectedValue);

                if (iBandera_P == 1)
                {
                    parametro[2] = new SqlParameter();
                    parametro[2].ParameterName = "@buscar";
                    parametro[2].SqlDbType = SqlDbType.VarChar;
                    parametro[2].Value = txtBuscar.Text.Trim();
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
                    return;
                }


                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    dgvDatos.Rows.Add(dtConsulta.Rows[i]["id_pos_seccion_mesa"].ToString(),
                                      dtConsulta.Rows[i]["id_localidad"].ToString(),
                                      dtConsulta.Rows[i]["is_active"].ToString(),
                                      dtConsulta.Rows[i]["fondo_pantalla"].ToString(),
                                      dtConsulta.Rows[i]["color"].ToString(),
                                      dtConsulta.Rows[i]["codigo"].ToString(),
                                      dtConsulta.Rows[i]["descripcion"].ToString(),
                                      dtConsulta.Rows[i]["estado"].ToString());
                }

                dgvDatos.ClearSelection();
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA INSERTAR REGISTROS EN LA BASE DE DATOS
        private void insertarRegistro()
        {
            try
            {
                iCuenta = consultarRegistroCrear();

                if (iCuenta == -1)
                    return;

                if (iCuenta > 0)
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Ya existe un registro con el código ingresado.";
                    ok.ShowDialog();
                    txtCodigo.Clear();
                    txtCodigo.Focus();
                    return;
                }

                SiNo = new VentanasMensajes.frmMensajeNuevoSiNo();
                SiNo.lblMensaje.Text = "¿Está seguro que desea guardar el registro?";
                SiNo.ShowDialog();

                if (SiNo.DialogResult != DialogResult.OK)
                    return;

                //SE INICIA UNA TRANSACCION
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Error al abrir transacción.";
                    ok.ShowDialog();
                    return;
                }
                
                sSql = "";
                sSql += "insert into pos_seccion_mesa (" + Environment.NewLine;
                sSql += "id_localidad, codigo, descripcion, color, fondo_pantalla," + Environment.NewLine;
                sSql += "is_active, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += "@id_localidad, @codigo, @descripcion, @color, @fondo_pantalla," + Environment.NewLine;
                sSql += "@is_active, @estado, getdate(), @usuario_ingreso, @terminal_ingreso)";

                #region PARAMETROS

                int a = 0;
                parametro = new SqlParameter[9];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_localidad";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Convert.ToInt32(cmbLocalidades.SelectedValue);
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@codigo";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = txtCodigo.Text.Trim().ToUpper();
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@descripcion";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = txtDescripcion.Text.Trim();
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@color";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = cmbPaleta.Text;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@fondo_pantalla";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = txtRuta.Text.Trim().ToLower();
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
                parametro[a].Value = Program.sDatosMaximo[1];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@terminal_ingreso";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[1];

                #endregion

                //EJECUTAR LA INSTRUCCIÓN SQL
                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);

                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Registro ingresado correctamente";
                ok.ShowDialog();
                grupoDatos.Enabled = false;
                btnNuevo.Text = "Nuevo";
                limpiarTodo();
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
        
        //FUNCION PARA ACTUALIZAR REGISTROS EN LA BASE DE DATOS
        private void actualizarRegistro()
        {
            try
            {
                SiNo = new VentanasMensajes.frmMensajeNuevoSiNo();
                SiNo.lblMensaje.Text = "¿Está seguro que desea actualizar el registro?";
                SiNo.ShowDialog();

                if (SiNo.DialogResult != DialogResult.OK)
                    return;

                //SE INICIA UNA TRANSACCION
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Error al abrir transacción.";
                    ok.ShowDialog();
                    return;
                }

                sSql = "";
                sSql += "update pos_seccion_mesa set" + Environment.NewLine;
                sSql += "descripcion = @descripcion," + Environment.NewLine;
                sSql += "color = @color," + Environment.NewLine;
                sSql += "fondo_pantalla = @fondo_pantalla," + Environment.NewLine;
                sSql += "is_active = @is_active" + Environment.NewLine;
                sSql += "where id_pos_seccion_mesa = @id_pos_seccion_mesa" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                int a = 0;
                parametro = new SqlParameter[6];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@descripcion";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = txtDescripcion.Text.Trim();
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@color";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = cmbPaleta.Text;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@fondo_pantalla";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = txtRuta.Text.Trim();
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@is_active";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iHabilitado;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pos_seccion_mesa";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdPosSeccionMesa;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";

                #endregion

                //EJECUTAR LA INSTRUCCIÓN SQL
                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);

                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Registro actualizado correctamente";
                ok.ShowDialog();
                grupoDatos.Enabled = false;
                btnNuevo.Text = "Nuevo";
                limpiarTodo();
                return;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                goto reversa;
            }

            reversa: { conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION); }
        }
        
        //FUNCION PARA DAR DE BAJA EL REGISTRO
        private void eliminarRegistro()
        {
            try
            {
                SiNo = new VentanasMensajes.frmMensajeNuevoSiNo();
                SiNo.lblMensaje.Text = "¿Está seguro que desea inhabilitar el registro?";
                SiNo.ShowDialog();

                if (SiNo.DialogResult != DialogResult.OK)
                    return;

                //SE INICIA UNA TRANSACCION
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Error al abrir transacción.";
                    ok.ShowDialog();
                    return;
                }

                sSql = "";
                sSql += "update pos_seccion_mesa set" + Environment.NewLine;
                sSql += "is_active = @is_active" + Environment.NewLine;
                sSql += "where id_pos_seccion_mesa = @id_pos_seccion_mesa" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                int a = 0;
                parametro = new SqlParameter[3];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@is_active";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 0;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pos_seccion_mesa";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdPosSeccionMesa;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";

                #endregion

                //EJECUTAR LA INSTRUCCIÓN SQL
                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);

                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Registro inhabilitado correctamente";
                ok.ShowDialog();
                grupoDatos.Enabled = false;
                btnNuevo.Text = "Nuevo";
                limpiarTodo();
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

        //FUNCION PARA CONSULTAR EL REGISTRO SI ESTÁ REGISTRADO
        private int consultarRegistroCrear()
        {
            try
            {
                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from pos_seccion_mesa" + Environment.NewLine;
                sSql += "where codigo = '" + txtCodigo.Text.Trim() + "'" + Environment.NewLine;
                sSql += "and estado in ('A', 'N')";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    return Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                }

                else
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return -1;
                }
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return -1;
            }
        }

        //FUNCION PARA CONSULTAR EL REGISTRO SI ESTÁ ELIMINADO
        private int consultarRegistroEliminar()
        {
            try
            {
                sSql = "";
                sSql += "select count(*) cuenta" + Environment.NewLine;
                sSql += "from pos_mesa" + Environment.NewLine;
                sSql += "where id_pos_seccion_mesa = " + iIdPosSeccionMesa + Environment.NewLine;
                sSql += "and estado in ('A', 'N')";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    return Convert.ToInt32(dtConsulta.Rows[0].ItemArray[0].ToString());
                }

                else
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return -1;
                }
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return -1;
            }
        }

        #endregion

        private void Btn_CerrarPosSecMes_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btn_LimpiarPosSecMes_Click(object sender, EventArgs e)
        {
            grupoDatos.Enabled = false;
            btnNuevo.Text = "Nuevo";
            limpiarTodo();
        }

        private void Btn_BuscarPosSecMes_Click(object sender, EventArgs e)
        {
            try
            {
                llenarGrid();
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        private void BtnNuevoPosSecMes_Click(object sender, EventArgs e)
        {
            //SI EL BOTON ESTA EN OPCION NUEVO
            if (btnNuevo.Text == "Nuevo")
            {
                if (Convert.ToInt32(cmbLocalidades.SelectedValue) == 0)
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Favor seleccione la localidad.";
                    ok.ShowDialog();
                    cmbLocalidades.Focus();
                    return;
                }

                limpiarTodo();
                cmbLocalidades.Enabled = false;
                grupoDatos.Enabled = true;
                btnNuevo.Text = "Guardar";
                txtCodigo.Focus();
            }

            else
            {
                if ((txtCodigo.Text == "") && (txtDescripcion.Text == ""))
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Debe rellenar todos los campos obligatorios.";
                    ok.ShowDialog();
                    txtCodigo.Focus();
                    return;
                }

                if (txtCodigo.Text == "")
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Favor ingrese el código de la sección mesa.";
                    ok.ShowDialog();
                    txtCodigo.Focus();
                    return;
                }

                if (txtDescripcion.Text == "")
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Favor ingrese la descripción de la sección mesa.";
                    ok.ShowDialog();
                    txtDescripcion.Focus();
                    return;
                }

                if (btnNuevo.Text == "Guardar")
                {
                    insertarRegistro();
                }

                else if (btnNuevo.Text == "Actualizar")
                {
                    if (chkHabilitado.Checked == true)
                        iHabilitado = 1;
                    else
                        iHabilitado = 0;

                    actualizarRegistro();
                }
            }
        }

        private void Btn_AnularPosSecMes_Click(object sender, EventArgs e)
        {
            iCuenta = consultarRegistroEliminar();

            if (iCuenta > 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Existen registros de mesas asociadas a la sección a eliminar.";
                ok.ShowDialog();
                return;
            }

            else if (iCuenta == -1)
            {
                return;
            }

            else
            {
                SiNo = new VentanasMensajes.frmMensajeNuevoSiNo();
                SiNo.lblMensaje.Text = "¿Está seguro que desea inhabilitar el registro?";
                SiNo.ShowDialog();

                if (SiNo.DialogResult == DialogResult.OK)
                {
                    eliminarRegistro();
                }
            }
        }
                
        private void cmbPaleta_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            if (cmb == null) return;
            if (e.Index < 0) return;
            if (!(cmb.Items[e.Index] is Color)) return;
            Color color = (Color)cmb.Items[e.Index];
            // Dibujamos el fondo
            e.DrawBackground();
            // Creamos los objetos GDI+
            Brush brush = new SolidBrush(color);
            Pen forePen = new Pen(e.ForeColor);
            Brush foreBrush = new SolidBrush(e.ForeColor);
            // Dibujamos el borde del rectángulo
            e.Graphics.DrawRectangle(
                forePen,
                new Rectangle(e.Bounds.Left + 2, e.Bounds.Top + 2, 19,
                    e.Bounds.Size.Height - 4));
            // Rellenamos el rectángulo con el Color seleccionado
            // en la combo
            e.Graphics.FillRectangle(brush,
                new Rectangle(e.Bounds.Left + 3, e.Bounds.Top + 3, 18,
                    e.Bounds.Size.Height - 5));
            // Dibujamos el nombre del color
            e.Graphics.DrawString(color.Name, cmb.Font,
                foreBrush, e.Bounds.Left + 25, e.Bounds.Top + 2);
            // Eliminamos objetos GDI+
            brush.Dispose();
            forePen.Dispose();
            foreBrush.Dispose();
        }

        private void dgvDatos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                iIdPosSeccionMesa = Convert.ToInt32(dgvDatos.CurrentRow.Cells["id_pos_seccion_mesa"].Value);
                txtCodigo.Text = dgvDatos.CurrentRow.Cells["codigo"].Value.ToString();
                txtDescripcion.Text = dgvDatos.CurrentRow.Cells["descripcion"].Value.ToString();

                if (dgvDatos.CurrentRow.Cells[3].Value.ToString() != "color")
                    cmbPaleta.Text = dgvDatos.CurrentRow.Cells["color"].Value.ToString();
                else
                    cargarColores();

                txtRuta.Text = dgvDatos.CurrentRow.Cells["fondo_pantalla"].Value.ToString();

                if (Convert.ToInt32(dgvDatos.CurrentRow.Cells["is_active"].Value) == 1)
                    chkHabilitado.Checked = true;
                else
                    chkHabilitado.Checked = false;

                grupoDatos.Enabled = true;
                btnNuevo.Text = "Actualizar";
                txtCodigo.Enabled = false;
                chkHabilitado.Enabled = true;
                btnAnular.Enabled = true;
                cmbLocalidades.Enabled = false;
                txtDescripcion.Focus();
                dgvDatos.ClearSelection();
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            OpenFileDialog abrir = new OpenFileDialog();
            //abrir.InitialDirectory = "c:\\";
            abrir.Filter = "Archivos imagen (*.jpg; *.png; *.jpeg)|*.jpg;*.png;*.jpeg";
            abrir.Title = "Seleccionar archivo";

            if (abrir.ShowDialog() == DialogResult.OK)
            {
                txtRuta.Text = abrir.FileName;
            }
        }

        private void cmbLocalidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbLocalidades.SelectedValue) == 0)
            {
                dgvDatos.Rows.Clear();
                return;
            }

            llenarGrid();
        }
    }
}
