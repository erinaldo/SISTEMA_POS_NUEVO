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

namespace Palatium.Promociones
{
    public partial class frmCrearPromociones : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoOk ok;
        VentanasMensajes.frmMensajeNuevoSiNo NuevoSiNo;

        Clases.ClaseValidarCaracteres caracter;

        string sSql;
        string sFecha;

        bool bRespuesta;
        bool bEditar;

        DataTable dtConsulta;
        DataTable dtCategorias;
        DataTable dtProductos;
        DataTable dtSubCategorias;

        int iSubcategoria_P;
        int iModificador_P;
        int iIdProducto;
        int iPagaIva;
        int iPagaServicio;
        int iFila;

        int iCantidadPromociones;

        SqlParameter[] parametro;

        public frmCrearPromociones()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMBOBX DE DÍAS
        private void llenarComboDias()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_dia, descripcion" + Environment.NewLine;
                sSql += "from pos_dias" + Environment.NewLine;
                sSql += "where estado = @estado" + Environment.NewLine;
                sSql += "order by secuencia";

                parametro = new SqlParameter[1];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@estado";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "A";

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
                row["id_pos_dia"] = "0";
                row["descripcion"] = "Seleccione...!!!";
                dtConsulta.Rows.InsertAt(row, 0);

                cmbDia.DisplayMember = "descripcion";
                cmbDia.ValueMember = "id_pos_dia";
                cmbDia.DataSource = dtConsulta;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return;
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE CATEGORÍAS
        private void llenarComboCategorias()
        {
            try
            {
                sSql = "";
                sSql += "select P.id_producto, NP.nombre, P.subcategoria, P.modificador" + Environment.NewLine;
                sSql += "from cv401_productos P INNER JOIN" + Environment.NewLine;
                sSql += "cv401_nombre_productos NP ON P.id_Producto = NP.id_Producto" + Environment.NewLine;
                sSql += "and P.estado = @estado_1" + Environment.NewLine;
                sSql += "and NP.estado = @estado_2" + Environment.NewLine;
                sSql += "where P.nivel = @nivel" + Environment.NewLine;
                sSql += "order by NP.nombre";

                parametro = new SqlParameter[3];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@estado_1";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "A";

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@estado_2";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = "A";

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@nivel";
                parametro[2].SqlDbType = SqlDbType.VarChar;
                parametro[2].Value = 2;

                dtCategorias = new DataTable();
                dtCategorias.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtCategorias, sSql, parametro);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                DataRow row = dtCategorias.NewRow();
                row["id_producto"] = "0";
                row["nombre"] = "Seleccione...!!!";
                row["subcategoria"] = "0";
                row["modificador"] = "0";
                dtCategorias.Rows.InsertAt(row, 0);

                cmbCategorias.DisplayMember = "nombre";
                cmbCategorias.ValueMember = "id_producto";
                cmbCategorias.DataSource = dtCategorias;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return;
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE SUBCATEGORÍAS
        private void llenarComboSubCategorias(int iIdProductoPadre_P)
        {
            try
            {
                sSql = "";
                sSql += "select P.id_producto, NP.nombre" + Environment.NewLine;
                sSql += "from cv401_productos P INNER JOIN" + Environment.NewLine;
                sSql += "cv401_nombre_productos NP ON P.id_Producto = NP.id_Producto" + Environment.NewLine;
                sSql += "and P.estado = @estado_1" + Environment.NewLine;
                sSql += "and NP.estado = @estado_2" + Environment.NewLine;
                sSql += "where P.nivel = @nivel" + Environment.NewLine;
                sSql += "and P.id_producto_padre = @id_producto_padre" + Environment.NewLine;
                sSql += "order by NP.nombre";

                parametro = new SqlParameter[4];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@estado_1";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "A";

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@estado_2";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = "A";

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@nivel";
                parametro[2].SqlDbType = SqlDbType.VarChar;
                parametro[2].Value = 3;

                parametro[3] = new SqlParameter();
                parametro[3].ParameterName = "@id_producto_padre";
                parametro[3].SqlDbType = SqlDbType.Int;
                parametro[3].Value = iIdProductoPadre_P;

                dtSubCategorias = new DataTable();
                dtSubCategorias.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtSubCategorias, sSql, parametro);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                DataRow row = dtSubCategorias.NewRow();
                row["id_producto"] = "0";
                row["nombre"] = "Seleccione...!!!";
                dtSubCategorias.Rows.InsertAt(row, 0);

                cmbSubCategorias.DisplayMember = "nombre";
                cmbSubCategorias.ValueMember = "id_producto";
                cmbSubCategorias.DataSource = dtSubCategorias;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return;
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE PRODUCTOS
        private void llenarComboProductos(int iIdProductoPadre_P, int iNivel_P)
        {
            try
            {
                sSql = "";
                sSql += "select P.id_producto, NP.nombre, P.paga_iva, P.paga_servicio" + Environment.NewLine;
                sSql += "from cv401_productos P INNER JOIN" + Environment.NewLine;
                sSql += "cv401_nombre_productos NP ON P.id_Producto = NP.id_Producto" + Environment.NewLine;
                sSql += "and P.estado = @estado_1" + Environment.NewLine;
                sSql += "and NP.estado = @estado_2" + Environment.NewLine;
                sSql += "where P.nivel = @nivel" + Environment.NewLine;
                sSql += "and P.id_producto_padre = @id_producto_padre" + Environment.NewLine;
                sSql += "order by NP.nombre";

                parametro = new SqlParameter[4];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@estado_1";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "A";

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@estado_2";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = "A";

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@nivel";
                parametro[2].SqlDbType = SqlDbType.VarChar;
                parametro[2].Value = iNivel_P;

                parametro[3] = new SqlParameter();
                parametro[3].ParameterName = "@id_producto_padre";
                parametro[3].SqlDbType = SqlDbType.Int;
                parametro[3].Value = iIdProductoPadre_P;

                dtProductos = new DataTable();
                dtProductos.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtProductos, sSql, parametro);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                DataRow row = dtProductos.NewRow();
                row["id_producto"] = "0";
                row["nombre"] = "Seleccione...!!!";
                dtProductos.Rows.InsertAt(row, 0);

                cmbProductos.DisplayMember = "nombre";
                cmbProductos.ValueMember = "id_producto";
                cmbProductos.DataSource = dtProductos;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return;
            }
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            iCantidadPromociones = 0;

            cmbCategorias.SelectedIndexChanged -= new EventHandler(cmbCategorias_SelectedIndexChanged);
            cmbSubCategorias.SelectedIndexChanged -= new EventHandler(cmbSubCategorias_SelectedIndexChanged);
            llenarComboCategorias();
            llenarComboSubCategorias(0);
            llenarComboProductos(0, 3);
            cmbCategorias.SelectedIndexChanged += new EventHandler(cmbCategorias_SelectedIndexChanged);
            cmbSubCategorias.SelectedIndexChanged += new EventHandler(cmbSubCategorias_SelectedIndexChanged);

            llenarComboDias();
            dgvDatos.Rows.Clear();

            txtDescripcion.Clear();
            txtPrecio.Text = "0.00";

            chkHabilitado.Checked = true;
            chkHabilitado.Enabled = false;

            btnGrabar.Visible = false;
            btnRemoverLinea.Visible = false;
            btnValidar.Visible = true;
            btnAgregar.Visible = false;

            dtHoraInicial.Text = "12:00:00";
            dtHoraFinal.Text = "12:00:00";
            grupoDatos.Enabled = false;
            grupoValidar.Enabled = true;
            cmbCategorias.Focus();
        }

        //VALIDAR LAS HORAS PARA NO REPETIR
        private int validarHoras()
        {
            try
            {
                int iBandera = 0;
                int iDia_P = Convert.ToInt32(cmbDia.SelectedValue);
                int iIdProducto_P = Convert.ToInt32(cmbProductos.SelectedValue);
                DateTime dtHoraInicial_P = Convert.ToDateTime(dtHoraInicial.Text);
                DateTime dtHoraFinal_P = Convert.ToDateTime(dtHoraFinal.Text);

                int iDiaGrid, iIdProductoGrid;
                DateTime dtHoraInicialGrid, dtHoraFinalGrid;

                for (int i = 0; i < dgvDatos.Rows.Count; i++)
                {
                    iDiaGrid = Convert.ToInt32(dgvDatos.Rows[i].Cells["id_pos_dia"].Value);
                    iIdProductoGrid = Convert.ToInt32(dgvDatos.Rows[i].Cells["id_producto"].Value);
                    dtHoraInicialGrid = Convert.ToDateTime(dgvDatos.Rows[i].Cells["hora_inicial"].Value);
                    dtHoraFinalGrid = Convert.ToDateTime(dgvDatos.Rows[i].Cells["hora_final"].Value);

                    if ((iDia_P == iDiaGrid) && (iIdProducto_P == iIdProductoGrid))
                    {
                        if ((bEditar == true) && (iFila == i))
                        {
                            goto continuar;
                        }

                        if ((dtHoraInicial_P >= dtHoraInicialGrid) && (dtHoraInicial_P <= dtHoraFinalGrid))
                        {
                            iBandera = 1;
                            break;
                        }

                        if ((dtHoraFinal_P >= dtHoraInicialGrid) && (dtHoraFinal_P <= dtHoraFinalGrid))
                        {
                            iBandera = 1;
                            break;
                        }

                        continuar: { }
                    }
                }

                return iBandera;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return -1;
            }
        }

        //FUNCION PARA LLENAR EL GRID
        private bool llenarGrid(int iIdProducto_P)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                dgvDatos.Rows.Clear();

                sSql = "";
                sSql += "select * from pos_vw_promociones_productos" + Environment.NewLine;
                sSql += "where id_producto = @id_producto";

                parametro = new SqlParameter[1];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_producto";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdProducto_P;

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

                iCantidadPromociones = dtConsulta.Rows.Count;

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    Decimal dbPrecio_P;

                    if (Program.iCobrarConSinProductos == 1)
                    {
                        if (iPagaServicio == 1)
                        {
                            if (iPagaIva == 1)
                                dbPrecio_P = Convert.ToDecimal(dtConsulta.Rows[i]["precio"].ToString()) * Convert.ToDecimal(1 + Program.iva + Program.servicio);
                            else
                                dbPrecio_P = Convert.ToDecimal(dtConsulta.Rows[i]["precio"].ToString()) * Convert.ToDecimal(1 + Program.servicio);
                        }

                        else
                        {
                            if (iPagaIva == 1)
                                dbPrecio_P = Convert.ToDecimal(dtConsulta.Rows[i]["precio"].ToString()) * Convert.ToDecimal(1 + Program.iva);
                            else
                                dbPrecio_P = Convert.ToDecimal(dtConsulta.Rows[0]["precio"].ToString());
                        }
                    }

                    else
                    {
                        dbPrecio_P = Convert.ToDecimal(dtConsulta.Rows[0]["precio"].ToString());
                    }

                    dgvDatos.Rows.Add(dtConsulta.Rows[i]["id_pos_dia"].ToString(),
                                      dtConsulta.Rows[i]["id_producto"].ToString(),
                                      dtConsulta.Rows[i]["is_active"].ToString(),
                                      dtConsulta.Rows[i]["descripcion"].ToString(),
                                      dtConsulta.Rows[i]["nombre_producto"].ToString(),
                                      dtConsulta.Rows[i]["dia"].ToString(),
                                      Convert.ToDateTime(dtConsulta.Rows[i]["hora_inicial"].ToString()).ToString("HH:mm"),
                                      Convert.ToDateTime(dtConsulta.Rows[i]["hora_final"].ToString()).ToString("HH:mm"),
                                      dbPrecio_P.ToString("N2"),
                                      dtConsulta.Rows[i]["estado"].ToString());
                }

                this.Cursor = Cursors.Default;
                dgvDatos.ClearSelection();
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

        //FUNCION PARA INSERTAR EN LA BASE DE DATOS
        private bool insertarRegistros()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    this.Cursor = Cursors.Default;
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Error al iniciar transacción.";
                    ok.ShowDialog();
                    return false;
                }

                int a;

                sSql = "";
                sSql += "update pos_promocion set" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_producto = @id_producto" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region FUNCIONES DEL USUARIO

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
                parametro[a].ParameterName = "@id_producto";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdProducto;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_2";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    this.Cursor = Cursors.Default;
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                for (int i = 0; i < dgvDatos.Rows.Count; i++)
                {
                    int iIdDia_P = Convert.ToInt32(dgvDatos.Rows[i].Cells["id_pos_dia"].Value);
                    string sDescripcion_P = dgvDatos.Rows[i].Cells["descripcion"].Value.ToString().Trim().ToUpper();
                    DateTime dtHoraInicial_P = Convert.ToDateTime(dgvDatos.Rows[i].Cells["hora_inicial"].Value);
                    DateTime dtHoraFinal_P = Convert.ToDateTime(dgvDatos.Rows[i].Cells["hora_final"].Value);
                    Decimal dbPrecio_P = Convert.ToDecimal(dgvDatos.Rows[i].Cells["precio"].Value);
                    int iHabilitado_P = Convert.ToInt32(dgvDatos.Rows[i].Cells["is_active"].Value);

                    Decimal dbSubtotal_P;

                    //INSTRUCCION PARA NSERTAR EN LA TABLA CV403_PRECIOS_PRODUCTOS CON LISTA MINORISTA
                    if (Program.iCobrarConSinProductos == 1)
                    {
                        if (iPagaServicio == 1)
                        {
                            if (iPagaIva == 1)
                                dbSubtotal_P = dbPrecio_P / Convert.ToDecimal(1 + Program.iva + Program.servicio);
                            else
                                dbSubtotal_P = dbPrecio_P / Convert.ToDecimal(1 + Program.servicio);
                        }

                        else
                        {
                            if (iPagaIva == 1)
                                dbSubtotal_P = dbPrecio_P / Convert.ToDecimal(1 + Program.iva);
                            else
                                dbSubtotal_P = dbPrecio_P;
                        }
                    }

                    else
                    {
                        dbSubtotal_P = dbPrecio_P;
                    }

                    sSql = "";
                    sSql += "insert into pos_promocion (" + Environment.NewLine;
                    sSql += "id_producto, id_pos_dia, descripcion, hora_inicial," + Environment.NewLine;
                    sSql += "hora_final, precio, is_active, estado, fecha_ingreso," + Environment.NewLine;
                    sSql += "usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                    sSql += "values (" + Environment.NewLine;
                    sSql += "@id_producto, @id_pos_dia, @descripcion, @hora_inicial," + Environment.NewLine;
                    sSql += "@hora_final, @precio, @is_active, @estado, getdate()," + Environment.NewLine;
                    sSql += "@usuario_ingreso, @terminal_ingreso)";

                    #region PARAMETROS

                    a = 0;
                    parametro = new SqlParameter[10];
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_producto";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdProducto;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_pos_dia";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdDia_P;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@descripcion";
                    parametro[a].SqlDbType = SqlDbType.VarChar;
                    parametro[a].Value = sDescripcion_P;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@hora_inicial";
                    parametro[a].SqlDbType = SqlDbType.DateTime;
                    parametro[a].Value = Convert.ToDateTime(dtHoraInicial_P.ToString("HH:mm"));
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@hora_final";
                    parametro[a].SqlDbType = SqlDbType.DateTime;
                    parametro[a].Value = Convert.ToDateTime(dtHoraFinal_P.ToString("HH:mm"));
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@precio";
                    parametro[a].SqlDbType = SqlDbType.Decimal;
                    parametro[a].Value = dbSubtotal_P;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@is_active";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iHabilitado_P;
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
                        this.Cursor = Cursors.Default;
                        catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                        catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                        catchMensaje.ShowDialog();
                        goto reversa;
                    }

                }

                sSql = "";
                sSql += "update cv401_productos set" + Environment.NewLine;
                sSql += "maneja_happy_hour = @maneja_happy_hour" + Environment.NewLine;
                sSql += "where id_producto = @id_producto" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                parametro = new SqlParameter[3];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@maneja_happy_hour";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = 1;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@id_producto";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = iIdProducto;

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@estado";
                parametro[2].SqlDbType = SqlDbType.VarChar;
                parametro[2].Value = "A";

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    this.Cursor = Cursors.Default;
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);

                this.Cursor = Cursors.Default;
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Los registros se han guardado con éxito.";
                ok.ShowDialog();
                limpiar();

                return true;
            }

            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                goto reversa;
            }

            reversa: { conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION); return false; }
        }

        //FUNCION PARA ELIMINAR TODOS LOS REGISTROS
        private bool eliminarRegistros()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    this.Cursor = Cursors.Default;
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Error al iniciar transacción.";
                    ok.ShowDialog();
                    return false;
                }

                int a;

                sSql = "";
                sSql += "update pos_promocion set" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_producto = @id_producto" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region FUNCIONES DEL USUARIO

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
                parametro[a].ParameterName = "@id_producto";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdProducto;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado_2";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    this.Cursor = Cursors.Default;
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }                

                sSql = "";
                sSql += "update cv401_productos set" + Environment.NewLine;
                sSql += "maneja_happy_hour = @maneja_happy_hour" + Environment.NewLine;
                sSql += "where id_producto = @id_producto" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                parametro = new SqlParameter[3];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@maneja_happy_hour";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = 0;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@id_producto";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = iIdProducto;

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@estado";
                parametro[2].SqlDbType = SqlDbType.VarChar;
                parametro[2].Value = "A";

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    this.Cursor = Cursors.Default;
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);

                this.Cursor = Cursors.Default;
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Los registros se han eliminado con éxito.";
                ok.ShowDialog();
                limpiar();

                return true;
            }

            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                goto reversa;
            }

        reversa: { conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION); return false; }
        }

        #endregion

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbCategorias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbCategorias.SelectedValue) == 0)
            {
                cmbSubCategorias.Enabled = false;
                llenarComboSubCategorias(0);
                llenarComboProductos(0, 3);
            }

            int iIdProducto_P = Convert.ToInt32(cmbCategorias.SelectedValue);

            DataRow[] dFila = dtCategorias.Select("id_producto = " + iIdProducto_P);

            if (dFila.Length != 0)
            {
                iSubcategoria_P = Convert.ToInt32(dFila[0][2].ToString());
                iModificador_P = Convert.ToInt32(dFila[0][3].ToString());

                if (iModificador_P == 0)
                {
                    if (iSubcategoria_P == 0)
                    {
                        cmbSubCategorias.Enabled = false;
                        llenarComboSubCategorias(0);
                        llenarComboProductos(iIdProducto_P, 3);
                    }

                    else
                    {
                        cmbSubCategorias.Enabled = true;
                        cmbSubCategorias.SelectedIndexChanged -= new EventHandler(cmbSubCategorias_SelectedIndexChanged);
                        llenarComboSubCategorias(iIdProducto_P);
                        cmbSubCategorias.SelectedIndexChanged += new EventHandler(cmbSubCategorias_SelectedIndexChanged);
                        llenarComboProductos(Convert.ToInt32(cmbSubCategorias.SelectedValue), 4);
                    }
                }

                else
                {
                    cmbSubCategorias.Enabled = false;
                    llenarComboSubCategorias(0);
                    llenarComboProductos(iIdProducto_P, 3);
                }
            }
        }

        private void cmbSubCategorias_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iIdProducto_P = Convert.ToInt32(cmbSubCategorias.SelectedValue);
            llenarComboProductos(iIdProducto_P, 4);
        }

        private void frmCrearPromociones_Load(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbDia.SelectedValue) == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor seleccione el día.";
                ok.ShowDialog();
                cmbDia.Focus();
                return;
            }

            if (txtDescripcion.Text.Trim() == "")
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor ingrese la descripción de la promoción.";
                ok.ShowDialog();
                txtDescripcion.Focus();
                return;
            }

            if (Convert.ToDecimal(txtPrecio.Text) == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor ingrese un precio diferente a cero.";
                ok.ShowDialog();
                txtPrecio.Focus();
                return;
            }

            if (Convert.ToDateTime(dtHoraInicial.Text) == Convert.ToDateTime(dtHoraFinal.Text))
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "El rango de horas no pueden ser iguales.";
                ok.ShowDialog();
                dtHoraFinal.Text = "23:00";
                return;
            }

            if (Convert.ToDateTime(dtHoraInicial.Text) > Convert.ToDateTime(dtHoraFinal.Text))
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "La hora final no debe ser superior a la fecha inicial.";
                ok.ShowDialog();
                dtHoraFinal.Text = "23:00";
                return;
            }

            int iCuenta = validarHoras();

            if (iCuenta == -1)
                return;

            if (iCuenta == 1)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Los datos seleccionados ya constan o están dentro de un registro de la lista.";
                ok.ShowDialog();
                cmbDia.Focus();
                return;
            }

            if (bEditar == false)
            {
                dgvDatos.Rows.Add(cmbDia.SelectedValue,
                                  cmbProductos.SelectedValue,
                                  1,
                                  txtDescripcion.Text.Trim().ToUpper(),
                                  cmbProductos.Text.Trim().ToUpper(),
                                  cmbDia.Text.Trim(),
                                  dtHoraInicial.Text,
                                  dtHoraFinal.Text,
                                  Convert.ToDecimal(txtPrecio.Text).ToString("N2"),
                                  "ACTIVO"
                                  );
            }

            else
            {
                dgvDatos.Rows[iFila].Cells["id_pos_dia"].Value = cmbDia.SelectedValue;
                dgvDatos.Rows[iFila].Cells["descripcion"].Value = txtDescripcion.Text.Trim().ToUpper();
                dgvDatos.Rows[iFila].Cells["hora_inicial"].Value = dtHoraInicial.Text;
                dgvDatos.Rows[iFila].Cells["hora_final"].Value = dtHoraFinal.Text;
                dgvDatos.Rows[iFila].Cells["precio"].Value = Convert.ToDecimal(txtPrecio.Text).ToString("N2");
            }

            bEditar = false;
            txtDescripcion.Clear();
            txtPrecio.Text = "0.00";
            dgvDatos.ClearSelection();
            cmbDia.Focus();
        }

        private void btnRemoverLinea_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDatos.SelectedRows.Count == 0)
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "No se ha seleccionado una línea para remover.";
                    ok.ShowDialog();
                    return;
                }

                NuevoSiNo = new VentanasMensajes.frmMensajeNuevoSiNo();
                NuevoSiNo.lblMensaje.Text = "¿Desea eliminar la línea...?";
                NuevoSiNo.ShowDialog();

                if (NuevoSiNo.DialogResult == DialogResult.OK)
                {
                    dgvDatos.Rows.Remove(dgvDatos.CurrentRow);
                }

                dgvDatos.ClearSelection();
                cmbDia.Focus();
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

        private void btnValidar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(cmbProductos.SelectedValue) == 0)
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Favor seleccione el producto.";
                    ok.ShowDialog();
                    cmbProductos.Focus();
                    return;
                }

                grupoValidar.Enabled = false;
                grupoDatos.Enabled = true;
                btnGrabar.Visible = true;
                btnRemoverLinea.Visible = true;
                btnValidar.Visible = false;
                btnAgregar.Visible = true;

                iIdProducto = Convert.ToInt32(cmbProductos.SelectedValue);

                DataRow[] fila = dtProductos.Select("id_producto = " + iIdProducto);

                if (fila.Length != 0)
                {
                    iPagaIva = Convert.ToInt32(fila[0]["paga_iva"].ToString());
                    iPagaServicio = Convert.ToInt32(fila[0]["paga_servicio"].ToString());
                }

                else
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "No existen parámetros de iva y servicio en el producto.";
                    ok.ShowDialog();
                    cmbProductos.Focus();
                    return;
                }

                llenarGrid(iIdProducto);
                cmbDia.Focus();
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            caracter = new Clases.ClaseValidarCaracteres();
            caracter.soloDecimales(sender, e, 2);
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (iCantidadPromociones == 0)
            {
                if (dgvDatos.Rows.Count == 0)
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "No existen registros para guardar.";
                    ok.ShowDialog();
                    cmbDia.Focus();
                    return;
                }

                NuevoSiNo = new VentanasMensajes.frmMensajeNuevoSiNo();
                NuevoSiNo.lblMensaje.Text = "¿Está seguro que desea guardar los registros?";
                NuevoSiNo.ShowDialog();

                if (NuevoSiNo.DialogResult == DialogResult.OK)
                {
                    insertarRegistros();
                }
            }

            else
            {
                if (dgvDatos.Rows.Count == 0)
                {
                    NuevoSiNo = new VentanasMensajes.frmMensajeNuevoSiNo();
                    NuevoSiNo.lblMensaje.Text = "¿Está seguro que desea eliminar los registros?";
                    NuevoSiNo.ShowDialog();

                    if (NuevoSiNo.DialogResult == DialogResult.OK)
                    {
                        eliminarRegistros();
                    }
                }

                else
                {
                    NuevoSiNo = new VentanasMensajes.frmMensajeNuevoSiNo();
                    NuevoSiNo.lblMensaje.Text = "¿Está seguro que desea guardar los registros?";
                    NuevoSiNo.ShowDialog();

                    if (NuevoSiNo.DialogResult == DialogResult.OK)
                    {
                        insertarRegistros();
                    }
                }
            }

            //if (dgvDatos.Rows.Count == 0)
            //{
            //    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
            //    ok.lblMensaje.Text = "No existen registros para guardar.";
            //    ok.ShowDialog();
            //    cmbDia.Focus();
            //    return;
            //}

            //NuevoSiNo = new VentanasMensajes.frmMensajeNuevoSiNo();
            //NuevoSiNo.lblMensaje.Text = "¿Está seguro que desea guardar los registros?";
            //NuevoSiNo.ShowDialog();

            //if (NuevoSiNo.DialogResult == DialogResult.OK)
            //{
            //    insertarRegistros();
            //}
        }

        private void dgvDatos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                iFila = dgvDatos.CurrentRow.Index;

                cmbDia.SelectedValue = dgvDatos.CurrentRow.Cells["id_pos_dia"].Value;
                txtDescripcion.Text = dgvDatos.CurrentRow.Cells["descripcion"].Value.ToString().Trim().ToUpper();
                txtPrecio.Text = dgvDatos.CurrentRow.Cells["precio"].Value.ToString().Trim();
                dtHoraInicial.Text = dgvDatos.CurrentRow.Cells["hora_inicial"].Value.ToString().Trim();
                dtHoraFinal.Text = dgvDatos.CurrentRow.Cells["hora_final"].Value.ToString().Trim();

                bEditar = true;
                cmbDia.Focus();
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }
    }
}
