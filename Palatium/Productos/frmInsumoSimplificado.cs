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

namespace Palatium.Productos
{
    public partial class frmInsumoSimplificado : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        Clases.ClaseValidarCaracteres caracter;

        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoOk ok;
        VentanasMensajes.frmMensajeNuevoSiNo NuevoSiNo;

        string sSql;
        string sFechaListaBase;
        string sFechaListaMinorista;
        string sCodigo;
        string sFechaInicio;
        string sTabla;
        string sCampo;

        long iMaximo;

        DataTable dtConsulta;
        DataTable dtCategoriaInsumos;

        bool bRespuesta;

        int iIdListaBase;        
        int iIdListaMinorista;
        int iPagaIva;
        int iPrecioModificable;
        int iExpira;
        int iIdProducto;
        public int iIdProductoPadre;
        int iIdUnidadCompra;
        int iIdUnidadConsumo;
        int iTipoUnidadCompra;
        int iTipoUnidadConsumo;
        int iUltimo = 1;
        int iCg_tipoNombre = 5076;

        SqlParameter[] parametro;

        Decimal dPrecioCompra;
        Decimal dPresentacion;
        Decimal dPrecioUnitario;
        Decimal dIva;
        Decimal dServicio;
        Decimal dSubtotal;

        public frmInsumoSimplificado()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMBOBOX DE CATEGORIAS DE INSUMOS
        private void llenarComboCategoriaInsumo()
        {
            try
            {
                sSql = "";
                sSql += "select P.id_producto, P.codigo, NP.nombre" + Environment.NewLine;
                sSql += "from cv401_productos P,cv401_nombre_productos NP" + Environment.NewLine;
                sSql += "where P.id_producto = NP.id_producto" + Environment.NewLine;
                sSql += "and P.id_producto_padre in (" + Environment.NewLine;
                sSql += "select id_producto from cv401_productos " + Environment.NewLine;
                sSql += "where codigo = @codigo)" + Environment.NewLine;
                sSql += "and P.nivel = @nivel" + Environment.NewLine;
                sSql += "and P.estado = @estado_1" + Environment.NewLine;
                sSql += "and NP.estado = @estado_2" + Environment.NewLine;
                sSql += "and P.subcategoria = @subcategoria";

                #region PARAMETROS

                parametro = new SqlParameter[5];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@codigo";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "1";

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@nivel";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = 2;

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@estado_1";
                parametro[2].SqlDbType = SqlDbType.VarChar;
                parametro[2].Value = "A";

                parametro[3] = new SqlParameter();
                parametro[3].ParameterName = "@estado_2";
                parametro[3].SqlDbType = SqlDbType.VarChar;
                parametro[3].Value = "A";

                parametro[4] = new SqlParameter();
                parametro[4].ParameterName = "@subcategoria";
                parametro[4].SqlDbType = SqlDbType.VarChar;
                parametro[4].Value = 0;

                #endregion

                dtCategoriaInsumos = new DataTable();
                dtCategoriaInsumos.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtCategoriaInsumos, sSql, parametro);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                DataRow row = dtCategoriaInsumos.NewRow();
                row["id_producto"] = 0;
                row["nombre"] = "Seleccione...!!!";
                row["codigo"] = "0";
                dtCategoriaInsumos.Rows.InsertAt(row, 0);

                cmbCategorias.ValueMember = "id_producto";
                cmbCategorias.DisplayMember = "nombre";
                cmbCategorias.DataSource = dtCategoriaInsumos;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA OBTENER LOS DATOS DE LA LISTA BASE Y MINORISTA
        private void datosListas()
        {
            try
            {
                sSql = "";
                sSql += "select id_lista_precio, fecha_fin_validez" + Environment.NewLine;
                sSql += "from cv403_listas_precios" + Environment.NewLine;
                sSql += "where lista_base = @lista_base" + Environment.NewLine;
                sSql += "and estado = @estado_1" + Environment.NewLine;
                sSql += "union" + Environment.NewLine;
                sSql += "select id_lista_precio, fecha_fin_validez" + Environment.NewLine;
                sSql += "from cv403_listas_precios" + Environment.NewLine;
                sSql += "where lista_minorista = @lista_minorista" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARMETROS

                parametro = new SqlParameter[4];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@lista_base";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = 1;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@estado_1";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = "A";

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@lista_minorista";
                parametro[2].SqlDbType = SqlDbType.Int;
                parametro[2].Value = 1;

                parametro[3] = new SqlParameter();
                parametro[3].ParameterName = "@estado_2";
                parametro[3].SqlDbType = SqlDbType.VarChar;
                parametro[3].Value = "A";

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

                if (dtConsulta.Rows.Count > 0)
                {
                    iIdListaBase = Convert.ToInt32(dtConsulta.Rows[0]["id_lista_precio"]);
                    sFechaListaBase = Convert.ToDateTime(dtConsulta.Rows[0]["fecha_fin_validez"].ToString()).ToString("yyyy/MM/dd");
                    iIdListaMinorista = Convert.ToInt32(dtConsulta.Rows[1]["id_lista_precio"]);
                    sFechaListaMinorista = Convert.ToDateTime(dtConsulta.Rows[1]["fecha_fin_validez"].ToString()).ToString("yyyy/MM/dd");
                }
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA LLENAR EL COMBO DE COMPRA
        private void llenarComboCompra()
        {
            try
            {
                sSql = "";
                sSql += "select correlativo, valor_texto" + Environment.NewLine;
                sSql += "from tp_codigos where tabla = @tabla" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@tabla";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "SYS$00042";

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@estado";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = "A";

                #endregion

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtConsulta, sSql, parametro);

                if (bRespuesta == true)
                {
                    DataRow row = dtConsulta.NewRow();
                    row["correlativo"] = "0";
                    row["valor_texto"] = "Seleccione...!!!";
                    dtConsulta.Rows.InsertAt(row, 0);

                    cmbCompra.DisplayMember = "valor_texto";
                    cmbCompra.ValueMember = "correlativo";
                    cmbCompra.DataSource = dtConsulta;
                }

                else
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

        //FUNCION PARA LLENAR EL COMBO DE CONSUMO
        private void llenarComboConsumo()
        {
            try
            {
                sSql = "";
                sSql += "select correlativo, valor_texto" + Environment.NewLine;
                sSql += "from tp_codigos where tabla = @tabla" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@tabla";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "SYS$00042";

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@estado";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = "A";

                #endregion

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtConsulta, sSql, parametro);

                if (bRespuesta == true)
                {
                    DataRow row = dtConsulta.NewRow();
                    row["correlativo"] = "0";
                    row["valor_texto"] = "Seleccione...!!!";
                    dtConsulta.Rows.InsertAt(row, 0);

                    cmbConsumo.DisplayMember = "valor_texto";
                    cmbConsumo.ValueMember = "correlativo";
                    cmbConsumo.DataSource = dtConsulta;
                }

                else
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

        //FUNCION PARA LLENAR EL COMBO DE CLASE DE PRODUCTO
        private void llenarComboClaseProducto()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_clase_producto, descripcion" + Environment.NewLine;
                sSql += "from pos_clase_producto" + Environment.NewLine;
                sSql += "where estado = @estado" + Environment.NewLine;
                sSql += "and aplica_materia_prima = @aplica_materia_prima" + Environment.NewLine;
                sSql += "and aplica_producto_terminado = @aplica_producto_terminado";

                #region PARAMETROS

                parametro = new SqlParameter[3];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@estado";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "A";

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@aplica_materia_prima";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = 1;

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@aplica_producto_terminado";
                parametro[2].SqlDbType = SqlDbType.Int;
                parametro[2].Value = 0;

                #endregion

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtConsulta, sSql, parametro);

                if (bRespuesta == true)
                {
                    DataRow row = dtConsulta.NewRow();
                    row["id_pos_clase_producto"] = "0";
                    row["descripcion"] = "Seleccione...!!!";
                    dtConsulta.Rows.InsertAt(row, 0);

                    cmbClaseProducto.DisplayMember = "descripcion";
                    cmbClaseProducto.ValueMember = "id_pos_clase_producto";
                    cmbClaseProducto.DataSource = dtConsulta;
                }

                else
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                }

                //cmbClaseProducto.llenar(dtConsulta, sSql);
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA LLENAR EL COMBO DE TIPO DE PRODUCTO
        private void llenarComboTipoProducto()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_tipo_producto, descripcion" + Environment.NewLine;
                sSql += "from pos_tipo_producto" + Environment.NewLine;
                sSql += "where estado = @estado";

                #region PARAMETROS

                parametro = new SqlParameter[1];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@estado";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "A";

                #endregion

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtConsulta, sSql, parametro);

                if (bRespuesta == true)
                {
                    DataRow row = dtConsulta.NewRow();
                    row["id_pos_tipo_producto"] = "0";
                    row["descripcion"] = "Seleccione...!!!";
                    dtConsulta.Rows.InsertAt(row, 0);

                    cmbTipoProducto.DisplayMember = "descripcion";
                    cmbTipoProducto.ValueMember = "id_pos_tipo_producto";
                    cmbTipoProducto.DataSource = dtConsulta;
                }

                else
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                }

                //cmbTipoProducto.llenar(dtConsulta, sSql);
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA OBTENER EL PRECIO UNITARIO
        private void calcularPrecioUnitario()
        {
            try
            {
                if (txtPrecioCompra.Text.Trim() == "")
                    dPrecioCompra = 0;
                else if (Convert.ToDecimal(txtPrecioCompra.Text.Trim()) == 0)
                    dPrecioCompra = 0;
                else
                    dPrecioCompra = Convert.ToDecimal(txtPrecioCompra.Text.Trim());

                if (txtPresentacion.Text.Trim() == "")
                    dPresentacion = 0;
                else if (Convert.ToDecimal(txtPresentacion.Text.Trim()) == 0)
                    dPresentacion = 0;
                else
                    dPresentacion = Convert.ToDecimal(txtPresentacion.Text.Trim());

                if (dPresentacion == 0)
                {
                    txtPrecioUnitario.Text = "0.0000";
                    dPrecioUnitario = 0;
                }

                else
                {
                    dPrecioUnitario = dPrecioCompra / dPresentacion;
                    txtPrecioUnitario.Text = dPrecioUnitario.ToString("N4");
                }
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA CARGAR LA FECHA DEL SISTEMA
        private bool fechaSistema()
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
                    this.Cursor = Cursors.Default;
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return false;
                }

                sFechaInicio = Convert.ToDateTime(dtConsulta.Rows[0]["fecha"].ToString()).ToString("yyyy-MM-dd");
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
        private bool insertarRegistro()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                //CONSULTAR EL ID DEL REGISTRO DE LA TABLA CV401_PRODUCTOS
                sSql = "";
                sSql += "select P.codigo, NP.nombre" + Environment.NewLine;
                sSql += "from cv401_productos as P, cv401_nombre_productos as NP " + Environment.NewLine;
                sSql += "where NP.id_producto = P.id_producto" + Environment.NewLine;
                sSql += "and P.codigo = @codigo" + Environment.NewLine;
                sSql += "and P.estado = @estado_1" + Environment.NewLine;
                sSql += "and NP.estado = @estado_2";

                #region PARAMETROS

                parametro = new SqlParameter[3];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@codigo";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = sCodigo;

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
                    this.Cursor = Cursors.Default;
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return false;
                }

                if (dtConsulta.Rows.Count > 0)
                {
                    this.Cursor = Cursors.Default;
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "El código ingresado está o fue asignado para el producto " + dtConsulta.Rows[0]["nombre"].ToString() + ". Por Favor introduzca uno nuevo.";
                    ok.ShowDialog();
                    txtCodigo.Clear();
                    txtCodigo.Focus();
                    return false;
                }

                sSql = "";
                sSql += "select cg_tipo_unidad, cg_unidad, unidad_compra" + Environment.NewLine;
                sSql += "from cv401_unidades_productos" + Environment.NewLine;
                sSql += "where id_producto = @id_producto" + Environment.NewLine;
                sSql += "and estado = @estado" + Environment.NewLine;
                sSql += "order by unidad_compra desc";

                #region PARAMETROS

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_producto";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdProductoPadre;

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
                    this.Cursor = Cursors.Default;
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return false;
                }

                if (dtConsulta.Rows.Count >= 2)
                {
                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(dtConsulta.Rows[i]["unidad_compra"].ToString()) == true)
                        {
                            iIdUnidadCompra = Convert.ToInt32(dtConsulta.Rows[i]["cg_unidad"].ToString());
                            iTipoUnidadCompra = Convert.ToInt32(dtConsulta.Rows[i]["cg_tipo_unidad"].ToString());
                        }

                        else
                        {
                            iIdUnidadConsumo = Convert.ToInt32(dtConsulta.Rows[i]["cg_unidad"].ToString());
                            iTipoUnidadConsumo = Convert.ToInt32(dtConsulta.Rows[i]["cg_tipo_unidad"].ToString());
                        }
                    }
                }

                else
                {
                    this.Cursor = Cursors.Default;
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "No se pudo consultar la unidad de la categoría seleccionada.";
                    ok.ShowDialog();
                    return false;
                }

                dIva = Convert.ToDecimal(Program.iva);
                dServicio = Convert.ToDecimal(Program.servicio);

                if (fechaSistema() == false)
                {
                    this.Cursor = Cursors.Default;
                    return false;
                }

                if (sFechaInicio == "0001/01/01")
                {
                    this.Cursor = Cursors.Default;
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Se ha encontrado una fecha inválida. Favor reinicie la aplicación para solucionar el inconveniente. Si el problema persiste, favor comuníquese con el administrador del sistema.";
                    ok.ShowDialog();
                    return false;
                }

                //SE INICIA UNA TRANSACCION
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    this.Cursor = Cursors.Default;
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Error al iniciar la transacción para guardar el registro.";
                    ok.ShowDialog();
                    return false;
                }

                int a;

                //INSTRUCCION PARA INSERTAR EN LA TABLA CV401_PRODUCTOS
                sSql = "";
                sSql += "insert into cv401_productos (" + Environment.NewLine;
                sSql += "idempresa, codigo, id_Producto_padre, estado, Nivel," + Environment.NewLine;
                sSql += "modificable, precio_modificable, paga_iva, secuencia," + Environment.NewLine;
                sSql += "modificador, subcategoria, ultimo_nivel," + Environment.NewLine;
                sSql += "stock_min, stock_max, expira, fecha_ingreso, usuario_ingreso," + Environment.NewLine;
                sSql += "terminal_ingreso, id_pos_receta, id_pos_tipo_producto," + Environment.NewLine;
                sSql += "id_pos_clase_producto, id_bod_referencia, presentacion, rendimiento, uso_receta)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += "@idempresa, @codigo, @id_producto_padre, @estado, @nivel," + Environment.NewLine;
                sSql += "@modificable, @precio_modificable, @paga_iva, @secuencia," + Environment.NewLine;
                sSql += "@modificador, @subcategoria, @ultimo_nivel," + Environment.NewLine;
                sSql += "@stock_min, @stock_max, @expira, getdate(), @usuario_ingreso," + Environment.NewLine;
                sSql += "@terminal_ingreso, @id_pos_receta, @id_pos_tipo_producto," + Environment.NewLine;
                sSql += "@id_pos_clase_producto, @id_bod_referencia, @presentacion, @rendimiento, @uso_receta)";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[24];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@idempresa";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Program.iIdEmpresa;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@codigo";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = sCodigo;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_producto_padre";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdProductoPadre;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@nivel";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 3;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@modificable";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 0;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@precio_modificable";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iPrecioModificable;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@paga_iva";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iPagaIva;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@secuencia";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Convert.ToInt32(txtSecuencia.Text);
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@modificador";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 0;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@subcategoria";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 0;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@ultimo_nivel";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iUltimo;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@stock_min";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtStockMinimo.Text);
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@stock_max";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtStockMaximo.Text);
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@expira";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iExpira;
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
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pos_receta";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 0;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pos_tipo_producto";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Convert.ToInt32(cmbTipoProducto.SelectedValue);
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pos_clase_producto";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Convert.ToInt32(cmbClaseProducto.SelectedValue);
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_bod_referencia";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 0;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@presentacion";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtPresentacion.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@rendimiento";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtRendimiento.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@uso_receta";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 1;

                #endregion


                //EJECUTAR LA INSTRUCCIÓN SQL
                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                //PROCEDIMINTO PARA EXTRAER EL ID DEL PRODUCTO REGISTRADO
                sTabla = "cv401_productos";
                sCampo = "id_Producto"; ;

                long iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                if (iMaximo == -1)
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "No se pudo obtener el codigo de productos.";
                    ok.ShowDialog();
                    goto reversa;
                }

                iIdProducto = Convert.ToInt32(iMaximo);

                //INSTRUCCION PARA INSERTAR EN LA TABLA CV401_NOMBRE_PRODUCTOS
                sSql = "";
                sSql += "insert into cv401_nombre_productos (" + Environment.NewLine;
                sSql += "id_Producto, cg_tipo_nombre, nombre, nombre_interno," + Environment.NewLine;
                sSql += "estado, numero_replica_trigger, numero_control_replica," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += "@id_producto, @cg_tipo_nombre, @nombre, @nombre_interno," + Environment.NewLine;
                sSql += "@estado, @numero_replica_trigger, @numero_control_replica," + Environment.NewLine;
                sSql += "getdate(), @usuario_ingreso, @terminal_ingreso)";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[9];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_producto";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdProducto;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@cg_tipo_nombre";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iCg_tipoNombre;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@nombre";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = txtNombre.Text.ToString().Trim().ToUpper();
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@nombre_interno";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 1;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@numero_replica_trigger";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 0;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@numero_control_replica";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 0;
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

                //EJECUTAR LA INSTRUCCIÓN SQL
                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                //INSTRUCCION PARA NSERTAR EN LA TABLA CV403_PRECIOS_PRODUCTOS CON LISTA BASE
                if (iPagaIva == 1)
                    dSubtotal = dPrecioUnitario / (1 + (dIva + dServicio));
                else
                    dSubtotal = dPrecioUnitario;

                sSql = "";
                sSql += "insert into cv403_precios_productos (" + Environment.NewLine;
                sSql += "id_lista_precio, id_producto, valor_porcentaje, valor," + Environment.NewLine;
                sSql += "fecha_inicio, fecha_final, estado, numero_replica_trigger, numero_control_replica," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += "@id_lista_precio, @id_producto, @valor_porcentaje, @valor," + Environment.NewLine;
                sSql += "@fecha_inicio, @fecha_final, @estado, @numero_replica_trigger, @numero_control_replica," + Environment.NewLine;
                sSql += "getdate(), @usuario_ingreso, @terminal_ingreso)";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[11];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_lista_precio";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdListaBase;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_producto";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdProducto;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@valor_porcentaje";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 0;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@valor";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = dSubtotal;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@fecha_inicio";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = sFechaInicio;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@fecha_final";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = sFechaListaBase;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@numero_replica_trigger";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 0;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@numero_control_replica";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 0;
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

                //EJECUTAR LA INSTRUCCIÓN SQL
                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                //INSTRUCCION PARA NSERTAR EN LA TABLA CV403_PRECIOS_PRODUCTOS CON LISTA MINORISTA
                if (iPagaIva == 1)
                    dSubtotal = Convert.ToDecimal(txtPrecioMinorista.Text) / (1 + (dIva + dServicio));
                else
                    dSubtotal = Convert.ToDecimal(txtPrecioMinorista.Text);

                sSql = "";
                sSql += "insert into cv403_precios_productos (" + Environment.NewLine;
                sSql += "id_lista_precio, id_producto, valor_porcentaje, valor," + Environment.NewLine;
                sSql += "fecha_inicio, fecha_final, estado, numero_replica_trigger, numero_control_replica," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += "@id_lista_precio, @id_producto, @valor_porcentaje, @valor," + Environment.NewLine;
                sSql += "@fecha_inicio, @fecha_final, @estado, @numero_replica_trigger, @numero_control_replica," + Environment.NewLine;
                sSql += "getdate(), @usuario_ingreso, @terminal_ingreso)";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[11];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_lista_precio";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdListaMinorista;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_producto";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdProducto;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@valor_porcentaje";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 0;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@valor";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = dSubtotal;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@fecha_inicio";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = sFechaInicio;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@fecha_final";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = sFechaListaMinorista;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@numero_replica_trigger";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 0;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@numero_control_replica";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 0;
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

                //EJECUTAR LA INSTRUCCIÓN SQL
                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                //INSTRUCCIONES PARA INSERTAR EN  LA TABLA CV401_UNIDADES_PRODUCTOS
                //INSERTAR LA UNIDAD DE COMPRA
                sSql = "";
                sSql += "insert into cv401_unidades_productos (" + Environment.NewLine;
                sSql += "id_producto, cg_tipo_unidad, cg_unidad, unidad_compra," + Environment.NewLine;
                sSql += "estado, usuario_creacion, terminal_creacion, fecha_creacion," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += "@id_producto, @cg_tipo_unidad, @cg_unidad, @unidad_compra," + Environment.NewLine;
                sSql += "@estado, @usuario_creacion, @terminal_creacion, getdate()," + Environment.NewLine;
                sSql += "getdate(), @usuario_ingreso, @terminal_ingreso)";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[9];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_producto";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdProducto;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@cg_tipo_unidad";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iTipoUnidadCompra;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@cg_unidad";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Convert.ToInt32(cmbCompra.SelectedValue);
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@unidad_compra";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 1;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@usuario_creacion";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[0];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@terminal_creacion";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[1];
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

                //EJECUTAR LA INSTRUCCIÓN SQL
                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                //INSERTAR LA UNIDAD DE CONSUMO
                sSql = "";
                sSql += "insert into cv401_unidades_productos (" + Environment.NewLine;
                sSql += "id_producto, cg_tipo_unidad, cg_unidad, unidad_compra," + Environment.NewLine;
                sSql += "estado, usuario_creacion, terminal_creacion, fecha_creacion," + Environment.NewLine;
                sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values(" + Environment.NewLine;
                sSql += "@id_producto, @cg_tipo_unidad, @cg_unidad, @unidad_compra," + Environment.NewLine;
                sSql += "@estado, @usuario_creacion, @terminal_creacion, getdate()," + Environment.NewLine;
                sSql += "getdate(), @usuario_ingreso, @terminal_ingreso)";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[9];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_producto";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdProducto;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@cg_tipo_unidad";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iTipoUnidadConsumo;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@cg_unidad";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Convert.ToInt32(cmbConsumo.SelectedValue);
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@unidad_compra";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 0;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@usuario_creacion";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[0];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@terminal_creacion";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[1];
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

                //EJECUTAR LA INSTRUCCIÓN SQL
                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                this.Cursor = Cursors.Default;
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);

                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Registro ingresado correctamente.";
                ok.ShowDialog();
                this.DialogResult = DialogResult.OK;
                return true;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return false;
            }

        reversa: { this.Cursor = Cursors.Default; conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION); return false; }
        }

        #endregion

        private void frmInsumoSimplificado_Load(object sender, EventArgs e)
        {
            datosListas();
            llenarComboCategoriaInsumo();
            llenarComboClaseProducto();
            llenarComboTipoProducto();
            llenarComboCompra();
            llenarComboConsumo();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPrecioCompra_KeyPress(object sender, KeyPressEventArgs e)
        {
            caracter = new Clases.ClaseValidarCaracteres();
            caracter.soloDecimales(sender, e, 4);
        }

        private void txtPrecioCompra_Leave(object sender, EventArgs e)
        {
            calcularPrecioUnitario();
        }

        private void txtPresentacion_KeyPress(object sender, KeyPressEventArgs e)
        {
            caracter = new Clases.ClaseValidarCaracteres();
            caracter.soloDecimales(sender, e, 4);
        }

        private void txtPresentacion_Leave(object sender, EventArgs e)
        {
            calcularPrecioUnitario();
        }

        private void txtRendimiento_KeyPress(object sender, KeyPressEventArgs e)
        {
            caracter = new Clases.ClaseValidarCaracteres();
            caracter.soloDecimales(sender, e, 4);
        }

        private void txtPrecioUnitario_KeyPress(object sender, KeyPressEventArgs e)
        {
            caracter = new Clases.ClaseValidarCaracteres();
            caracter.soloDecimales(sender, e, 4);
        }

        private void txtPrecioMinorista_KeyPress(object sender, KeyPressEventArgs e)
        {
            caracter = new Clases.ClaseValidarCaracteres();
            caracter.soloDecimales(sender, e, 4);
        }

        private void txtStockMinimo_KeyPress(object sender, KeyPressEventArgs e)
        {
            caracter = new Clases.ClaseValidarCaracteres();
            caracter.soloDecimales(sender, e, 4);
        }

        private void txtStockMaximo_KeyPress(object sender, KeyPressEventArgs e)
        {
            caracter = new Clases.ClaseValidarCaracteres();
            caracter.soloDecimales(sender, e, 4);
        }

        private void txtSecuencia_KeyPress(object sender, KeyPressEventArgs e)
        {
            caracter = new Clases.ClaseValidarCaracteres();
            caracter.soloNumeros(e);
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbCategorias.SelectedValue) == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor seleccione la categoría del nuevo insumo";
                ok.ShowDialog();
                cmbCategorias.Focus();
                return;
            }

            if (txtCodigo.Text.Trim() == "")
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor ingrese el código del producto.";
                ok.ShowDialog();
                txtCodigo.Focus();
                return;
            }

            if (txtNombre.Text.Trim() == "")
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor ingrese el nombre del producto.";
                ok.ShowDialog();
                txtNombre.Focus();
                return;
            }

            if (Convert.ToInt32(cmbTipoProducto.SelectedValue) == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor seleccione el tipo de producto para el registro";
                ok.ShowDialog();
                cmbTipoProducto.Focus();
                return;
            }

            if (Convert.ToInt32(cmbClaseProducto.SelectedValue) == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor seleccione la clase de producto para el registro";
                ok.ShowDialog();
                cmbClaseProducto.Focus();
                return;
            }

            if ((txtPrecioCompra.Text.Trim() == "") || (Convert.ToDecimal(txtPrecioCompra.Text.Trim()) == 0))
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor ingrese un precio de compra válido para el registro.";
                ok.ShowDialog();
                txtPrecioCompra.Focus();
                return;
            }

            if ((txtPresentacion.Text.Trim() == "") || (Convert.ToDecimal(txtPresentacion.Text.Trim()) == 0))
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor ingrese la presentación del producto a registrar.";
                ok.ShowDialog();
                txtPresentacion.Focus();
                return;
            }

            if ((txtRendimiento.Text.Trim() == "") || (Convert.ToDecimal(txtRendimiento.Text.Trim()) == 0))
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor ingrese el rendimiento del producto a registrar.";
                ok.ShowDialog();
                txtRendimiento.Focus();
                return;
            }

            if (txtSecuencia.Text.Trim() == "")
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor ingrese la secuencia del registro.";
                ok.ShowDialog();
                txtSecuencia.Focus();
                return;
            }

            if (Convert.ToInt32(cmbCompra.SelectedValue) == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor seleccione unidad de compra para el registro";
                ok.ShowDialog();
                cmbClaseProducto.Focus();
                return;
            }

            if (Convert.ToInt32(cmbConsumo.SelectedValue) == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor seleccione la unidad de consumo para el registro";
                ok.ShowDialog();
                cmbClaseProducto.Focus();
                return;
            }

            if (txtStockMinimo.Text.Trim() == "")
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor ingrese el stock mínimo del producto.";
                ok.ShowDialog();
                txtStockMinimo.Focus();
                return;
            }

            if (txtStockMaximo.Text.Trim() == "")
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor ingrese el stock máximo del producto.";
                ok.ShowDialog();
                txtStockMaximo.Focus();
                return;
            }

            else
            {
                sCodigo = txtCodigo.Text.Trim();
                iIdProductoPadre = Convert.ToInt32(cmbCategorias.SelectedValue);

                if (chkPagaIva.Checked == true)
                    iPagaIva = 1;
                else 
                    iPagaIva = 0;

                if (chkPreModifProductos.Checked == true)
                    iPrecioModificable = 1;
                else 
                    iPrecioModificable = 0;

                if (chkExpiraProductos.Checked == true)
                    iExpira = 1;
                else iExpira = 0;

                NuevoSiNo = new VentanasMensajes.frmMensajeNuevoSiNo();
                NuevoSiNo.lblMensaje.Text = "¿Está seguro que desea guardar el registro?";
                NuevoSiNo.ShowDialog();

                if (NuevoSiNo.DialogResult == DialogResult.OK)
                {
                    insertarRegistro();
                }
            }
        }

        private void frmInsumoSimplificado_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
    }
}
