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

namespace Palatium.Receta
{
    public partial class frmCrearRecetas : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        Clases.ClaseValidarCaracteres caracter;

        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoOk ok;
        VentanasMensajes.frmMensajeNuevoSiNo NuevoSiNo;

        DataTable dtFamilias;
        DataTable dtCategoria;
        DataTable dtSubcategoria;
        DataTable dtProductos;
        DataTable dtCategoriaInsumos;
        DataTable dtInsumos;
        DataTable dtConsulta;

        string sSql;
        string sTabla;
        string sCampo;
        string sNombre_R;
        string sUnidad_R;
        string sRendimiento_R;
        string sCantidadBruta_R;
        string sCantidadNeta_R;
        string sCostoUnitario_R;
        string sImporte_R;
        string sIdProducto_R;
        string sIdUnidad_R;
        string sCodigo_R;

        long iMaximo;

        bool bRespuesta;
        bool bNuevoRegistro;

        SqlParameter[] parametro;

        int iIdReceta;
        int iSubcategoria_P;
        int iModificador_P;
        int iCantidadIngredientes;
        int iTabHabilitados;
        int iIdProductoActualizar;

        Decimal dbRendimientoTotal;
        Decimal dbCostoTotalTotal = 0;
        Decimal dbCostoUnitarioTotal = 0;
        Decimal dbSumaRendimiento = 0;
        Decimal dbPorcentajeParaServicios;
        Decimal dbPorcentajeUtilidadGanancias;
        Decimal dbNumeroPorciones;
        Decimal dbPreciodeVenta;
        Decimal dbUtilidadDeGanancias;
        Decimal dbUtilidadDeServicios;
        Decimal dbPorcentajeDeUtilidad;
        Decimal dbPorcentajeDeCostos;
        Decimal dbPorcentajeGananciaDeseada;
        Decimal dbPorcentajeGananciaServicioDeseado;

        //VARIABLES PARA ENVIAR A LA BASE DE DATOS
        Decimal dbRendimiento_R;
        Decimal dbImporte_R;
        Decimal dbCantidadBruta_R;
        Decimal dbCantidadNeta_R;
        Decimal dbCostoUnitario_R;
        
        int iIdUnidad_R;
        int iIdProducto_R;

        public frmCrearRecetas()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA LLENAR EL COMBOBOX DE FAMILIAS
        private void llenarComboFamilias()
        {
            try
            {
                sSql = "";
                sSql += "select * from pos_vw_informacion_productos_receta" + Environment.NewLine;
                sSql += "where nivel = @nivel" + Environment.NewLine;
                sSql += "and is_active = @is_active";

                #region PARAMETROS

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@nivel";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = 1;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@is_active";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = 1;

                #endregion

                dtFamilias = new DataTable();
                dtFamilias.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtFamilias, sSql, parametro);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                DataRow row = dtFamilias.NewRow();
                row["id_producto"] = "0";
                row["nombre"] = "Seleccione...!!!";
                dtFamilias.Rows.InsertAt(row, 0);

                cmbFamilia.ValueMember = "id_producto";
                cmbFamilia.DisplayMember = "nombre";
                cmbFamilia.DataSource = dtFamilias;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE CATEGORIAS
        private void llenarComboCategorias(int iIdProductoPadre_P)
        {
            try
            {
                sSql = "";
                sSql += "select * from pos_vw_informacion_productos_receta" + Environment.NewLine;
                sSql += "where nivel = @nivel" + Environment.NewLine;
                sSql += "and is_active = @is_active" + Environment.NewLine;
                sSql += "and id_producto_padre = @id_producto_padre";

                #region PARAMETROS

                parametro = new SqlParameter[3];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@nivel";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = 2;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@is_active";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = 1;

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@id_producto_padre";
                parametro[2].SqlDbType = SqlDbType.Int;
                parametro[2].Value = iIdProductoPadre_P;

                #endregion

                dtCategoria = new DataTable();
                dtCategoria.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtCategoria, sSql, parametro);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                DataRow row = dtCategoria.NewRow();
                row["id_producto"] = 0;
                row["nombre"] = "Seleccione...!!!";
                row["codigo"] = "0";
                row["is_active"] = 1;
                row["nivel"] = 2;
                row["id_producto_padre"] = iIdProductoPadre_P;
                row["subcategoria"] = 0;
                row["modificador"] = 0;
                dtCategoria.Rows.InsertAt(row, 0);

                cmbCategoria.ValueMember = "id_producto";
                cmbCategoria.DisplayMember = "nombre";
                cmbCategoria.DataSource = dtCategoria;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE SUBCATEGORIAS
        private void llenarComboSubcategorias(int iIdProductoPadre_P)
        {
            try
            {
                sSql = "";
                sSql += "select * from pos_vw_informacion_productos_receta" + Environment.NewLine;
                sSql += "where nivel = @nivel" + Environment.NewLine;
                sSql += "and is_active = @is_active" + Environment.NewLine;
                sSql += "and id_producto_padre = @id_producto_padre";

                #region PARAMETROS

                parametro = new SqlParameter[3];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@nivel";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = 3;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@is_active";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = 1;

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@id_producto_padre";
                parametro[2].SqlDbType = SqlDbType.Int;
                parametro[2].Value = iIdProductoPadre_P;

                #endregion

                dtSubcategoria = new DataTable();
                dtSubcategoria.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtSubcategoria, sSql, parametro);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                DataRow row = dtSubcategoria.NewRow();
                row["id_producto"] = 0;
                row["nombre"] = "Seleccione...!!!";
                row["codigo"] = "0";
                row["is_active"] = 1;
                row["nivel"] = 2;
                row["id_producto_padre"] = iIdProductoPadre_P;
                row["subcategoria"] = 0;
                row["modificador"] = 0;
                dtSubcategoria.Rows.InsertAt(row, 0);

                cmbSubcategoria.ValueMember = "id_producto";
                cmbSubcategoria.DisplayMember = "nombre";
                cmbSubcategoria.DataSource = dtSubcategoria;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE PRODUCTOS
        private void llenarComboProductos(int iIdProductoPadre_P, int iNivel_P)
        {
            try
            {
                sSql = "";
                sSql += "select * from pos_vw_informacion_productos_receta" + Environment.NewLine;
                sSql += "where nivel = @nivel" + Environment.NewLine;
                sSql += "and is_active = @is_active" + Environment.NewLine;
                sSql += "and id_producto_padre = @id_producto_padre";

                #region PARAMETROS

                parametro = new SqlParameter[3];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@nivel";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iNivel_P;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@is_active";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = 1;

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@id_producto_padre";
                parametro[2].SqlDbType = SqlDbType.Int;
                parametro[2].Value = iIdProductoPadre_P;

                #endregion

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
                row["id_producto"] = 0;
                row["nombre"] = "Seleccione...!!!";
                row["codigo"] = "0";
                row["is_active"] = 1;
                row["nivel"] = 2;
                row["id_producto_padre"] = iIdProductoPadre_P;
                row["subcategoria"] = 0;
                row["modificador"] = 0;
                dtProductos.Rows.InsertAt(row, 0);

                cmbProductos.ValueMember = "id_producto";
                cmbProductos.DisplayMember = "nombre";
                cmbProductos.DataSource = dtProductos;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

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

                cmbCategoriaInsumos.ValueMember = "id_producto";
                cmbCategoriaInsumos.DisplayMember = "nombre";
                cmbCategoriaInsumos.DataSource = dtCategoriaInsumos;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE CLASIFICACION DE RECETA
        private void llenarComboClasificacion()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_clasificacion_receta, descripcion" + Environment.NewLine;
                sSql += "from pos_clasificacion_receta" + Environment.NewLine;
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

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                DataRow row = dtConsulta.NewRow();
                row["id_pos_clasificacion_receta"] = "0";
                row["descripcion"] = "Seleccione...!!!";
                dtConsulta.Rows.InsertAt(row, 0);

                cmbClasificacion.ValueMember = "id_pos_clasificacion_receta";
                cmbClasificacion.DisplayMember = "descripcion";
                cmbClasificacion.DataSource = dtConsulta;

                if (cmbClasificacion.Items.Count > 1)
                    cmbClasificacion.SelectedIndex = 1;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE TIPOS DE RECETA
        private void llenarComboTipoReceta()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_tipo_receta, descripcion" + Environment.NewLine;
                sSql += "from pos_tipo_receta" + Environment.NewLine;
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

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                DataRow row = dtConsulta.NewRow();
                row["id_pos_tipo_receta"] = "0";
                row["descripcion"] = "Seleccione...!!!";
                dtConsulta.Rows.InsertAt(row, 0);

                cmbReceta.ValueMember = "id_pos_tipo_receta";
                cmbReceta.DisplayMember = "descripcion";
                cmbReceta.DataSource = dtConsulta;

                if (cmbReceta.Items.Count > 1)
                    cmbReceta.SelectedIndex = 1;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE ORIGEN DE RECETA
        private void llenarComboOrigenReceta()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_origen_receta, descripcion" + Environment.NewLine;
                sSql += "from pos_origen_receta" + Environment.NewLine;
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

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                DataRow row = dtConsulta.NewRow();
                row["id_pos_origen_receta"] = "0";
                row["descripcion"] = "Seleccione...!!!";
                dtConsulta.Rows.InsertAt(row, 0);

                cmbOrigen.ValueMember = "id_pos_origen_receta";
                cmbOrigen.DisplayMember = "descripcion";
                cmbOrigen.DataSource = dtConsulta;

                if (cmbOrigen.Items.Count > 1)
                    cmbOrigen.SelectedIndex = 1;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE TEMPERATURA DE SERVICIO
        private void llenarComboTemperaturaServicio()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_temperatura_de_servicio, descripcion" + Environment.NewLine;
                sSql += "from pos_temperatura_de_servicio" + Environment.NewLine;
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

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                DataRow row = dtConsulta.NewRow();
                row["id_pos_temperatura_de_servicio"] = "0";
                row["descripcion"] = "Seleccione...!!!";
                dtConsulta.Rows.InsertAt(row, 0);

                cmbTemperatura.ValueMember = "id_pos_temperatura_de_servicio";
                cmbTemperatura.DisplayMember = "descripcion";
                cmbTemperatura.DataSource = dtConsulta;

                if (cmbTemperatura.Items.Count > 1)
                    cmbTemperatura.SelectedIndex = 1;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA LLENAR EL GRID DE INSUMOS
        private void llenarGridInsumos(int iIdProductoPadre_P)
        {
            try
            {
                dgvInsumos.Rows.Clear();
                dgvInformacion.Rows.Clear();

                sSql = "";
                sSql += "select * from pos_vw_insumo_receta_V2" + Environment.NewLine;
                sSql += "where id_producto_padre = @id_producto_padre" + Environment.NewLine;
                sSql += "order by nombre";

                #region PARAMETROS

                parametro = new SqlParameter[1];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_producto_padre";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdProductoPadre_P;

                #endregion

                dtInsumos = new DataTable();
                dtInsumos.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtInsumos, sSql, parametro);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                for (int i = 0; i < dtInsumos.Rows.Count; i++)
                {
                    dgvInsumos.Rows.Add(dtInsumos.Rows[i]["id_producto"].ToString(),
                                        dtInsumos.Rows[i]["presentacion"].ToString(),
                                        dtInsumos.Rows[i]["rendimiento"].ToString(),
                                        dtInsumos.Rows[i]["id_pos_unidad"].ToString(),
                                        dtInsumos.Rows[i]["unidad_consumo"].ToString(),
                                        dtInsumos.Rows[i]["valor"].ToString(),
                                        dtInsumos.Rows[i]["codigo"].ToString(),
                                        dtInsumos.Rows[i]["nombre"].ToString()
                        );
                }

                verificarIngredientes();

                dgvInsumos.ClearSelection();
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //Función para llenar los valores de las cajas de texto
        private void llenarValoresTexto()
        {
            try
            {
                if (dgvIngredientes.Rows.Count > 0)
                {
                    dbSumaRendimiento = 0;
                    dbRendimientoTotal = 0;
                    dbCostoTotalTotal = 0;
                    dbCostoUnitarioTotal = 0;

                    if ((txtNumeroPorciones.Text.Trim() == "") || (Convert.ToDecimal(txtNumeroPorciones.Text.Trim()) == 0))
                        txtNumeroPorciones.Text = "1";

                    dbNumeroPorciones = Convert.ToDecimal(txtNumeroPorciones.Text.Trim());

                    Decimal dbSumaGramos = 0;

                    for (int i = 0; i < dgvIngredientes.Rows.Count; i++)
                    {
                        dbSumaRendimiento += Convert.ToDecimal(dgvIngredientes.Rows[i].Cells["rendimiento"].Value.ToString());
                        dbCostoTotalTotal += Convert.ToDecimal(dgvIngredientes.Rows[i].Cells["subtotal"].Value.ToString());

                        dbSumaGramos += Convert.ToDecimal(dgvIngredientes.Rows[i].Cells["cantidad_neta"].Value.ToString());
                    }

                    txtCantidadNetaGramos.Text = dbSumaGramos.ToString("N2");

                    dbCostoUnitarioTotal = dbCostoTotalTotal / dbNumeroPorciones;

                    txtCostoTotal.Text = dbCostoTotalTotal.ToString("N2");
                    txtCostoUnitario.Text = dbCostoUnitarioTotal.ToString("N2");

                    dbPorcentajeParaServicios = Convert.ToDecimal(txtPorcentajeServicioDeseado.Text.Trim()) / 100;
                    dbPreciodeVenta = (dbCostoUnitarioTotal * dbPorcentajeParaServicios) + dbCostoUnitarioTotal;
                    txtPrecioDeVenta.Text = dbPreciodeVenta.ToString("N2");

                    dbPorcentajeDeCostos = (dbCostoUnitarioTotal * 100) / dbPreciodeVenta;
                    txtPorcentajeDeCosto.Text = dbPorcentajeDeCostos.ToString("N2");

                    dbPorcentajeGananciaDeseada = Convert.ToDecimal(txtPorcentajeGananciaDeseada.Text.Trim());
                    dbPorcentajeUtilidadGanancias = dbPreciodeVenta * (dbPorcentajeGananciaDeseada / 100);
                    dbUtilidadDeGanancias = dbPorcentajeUtilidadGanancias + dbPreciodeVenta;
                    txtUtilidadDeGanancias.Text = dbUtilidadDeGanancias.ToString("N2");

                    dbUtilidadDeServicios = dbCostoTotalTotal * dbPorcentajeParaServicios;
                    txtUtilidadDeServicios.Text = dbUtilidadDeServicios.ToString("N2");

                    dbPorcentajeDeUtilidad = (dbUtilidadDeServicios * 100) / dbPreciodeVenta;
                    txtPorcentajeDeUtilidad.Text = dbPorcentajeDeUtilidad.ToString("N2");

                    iCantidadIngredientes = dgvIngredientes.Rows.Count;
                    dbSumaRendimiento = dbSumaRendimiento / 100;
                    dbSumaRendimiento = dbSumaRendimiento / iCantidadIngredientes;

                    txtRendimiento.Text = dbSumaRendimiento.ToString("N2");

                    if (Convert.ToDecimal(txtPesoGramos.Text.Trim()) == 0)
                        txtCostoPorGramo.Text = "0";
                    else
                        txtCostoPorGramo.Text = (Convert.ToDecimal(txtCostoTotal.Text.Trim()) / Convert.ToDecimal(txtPesoGramos.Text.Trim())).ToString("N6");
                }

                else
                {
                    txtCostoTotal.Text = "0";
                    txtCostoUnitario.Text = "0";
                    txtPorcentajeDeCosto.Text = "0";
                    txtPorcentajeDeUtilidad.Text = "0";
                    txtPrecioDeVenta.Text = "0";
                    txtRendimiento.Text = "0";
                    txtUtilidadDeGanancias.Text = "0";
                    txtUtilidadDeServicios.Text = "0";
                    txtCostoPorGramo.Text = "0";
                    txtCantidadNetaGramos.Text = "0";
                    txtPesoGramos.Text = "0";
                }
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA LIMPIAR EL FORMULARIO
        private void iniciarReceta(int iBanderaTabPrincipal_P)
        {
            if (iTabHabilitados == 0)
            {
                tabControl.TabPages.Add(tabInsumo);
                tabControl.TabPages.Add(tabInformacion);
                iTabHabilitados = 1;
            }

            bNuevoRegistro = true;
            btnNuevo.Enabled = false;
            txtNumeroPorciones.Text = "1";
            dgvInsumos.Rows.Clear();
            dgvInformacion.Rows.Clear();
            dgvIngredientes.Rows.Clear();

            grupoSeleccion.Enabled = true;
            grupoInsumos.Enabled = false;

            btnGrabar.Enabled = false;
            btnGrabar_2.Enabled = false;
            chkProductoTerminado.Checked = false;

            lblPlatillo.Text = "Sin asignar";
            lblCodigo.Text = "Sin asignar";
            lblCodigo.Text = "1";
            txtCostoTotal.Text = "0";
            txtCostoUnitario.Text = "0";
            txtPorcentajeDeCosto.Text = "0";
            txtPorcentajeDeUtilidad.Text = "0";
            txtPrecioDeVenta.Text = "0";
            txtRendimiento.Text = "0";
            txtUtilidadDeGanancias.Text = "0";
            txtUtilidadDeServicios.Text = "0";
            txtNumeroPorciones.Text = "1";
            txtPesoGramos.Text = "0";
            txtCostoPorGramo.Text = "0";
            txtCantidadNetaGramos.Text = "0";
            txtPorcentajeGananciaDeseada.Text = "100";
            txtPorcentajeServicioDeseado.Text = "10";

            cmbFamilia.SelectedIndexChanged -= new EventHandler(cmbFamilia_SelectedIndexChanged);
            cmbCategoria.SelectedIndexChanged -= new EventHandler(cmbCategoria_SelectedIndexChanged);
            cmbSubcategoria.SelectedIndexChanged -= new EventHandler(cmbSubcategoria_SelectedIndexChanged);
            llenarComboFamilias();
            llenarComboCategorias(Convert.ToInt32(cmbFamilia.SelectedValue));
            llenarComboSubcategorias(0);
            llenarComboProductos(0, 3);
            cmbFamilia.SelectedIndexChanged += new EventHandler(cmbFamilia_SelectedIndexChanged);
            cmbCategoria.SelectedIndexChanged += new EventHandler(cmbCategoria_SelectedIndexChanged);
            cmbSubcategoria.SelectedIndexChanged += new EventHandler(cmbSubcategoria_SelectedIndexChanged);

            llenarComboCategoriaInsumo();
            llenarComboClasificacion();
            llenarComboTipoReceta();
            llenarComboOrigenReceta();
            llenarComboTemperaturaServicio();
            
            if (iBanderaTabPrincipal_P == 1)
                tabControl.SelectedTab = tabControl.TabPages["tabInsumo"];

            cmbFamilia.Focus();
        }

        private void limpiarTodo()
        {
            iTabHabilitados = 0;
            iIdProductoActualizar = 0;
            lblTituloReceta.Text = "";
            tabControl.TabPages.Remove(tabInsumo);
            tabControl.TabPages.Remove(tabInformacion);
            listarRecetas();
            btnNuevo.Enabled = true;
            tabControl.SelectedTab = tabControl.TabPages["tabListado"];
            this.ActiveControl = txtBuscar;
        }

        //FUNCION PARA VALIDAR LA INFORMACION PARA ENVIAR A LA BASE DE DATOS
        private void validarGuardarInformacion()
        {
            if (dgvIngredientes.Rows.Count > 0)
            {
                NuevoSiNo = new VentanasMensajes.frmMensajeNuevoSiNo();
                NuevoSiNo.lblMensaje.Text = "¿Desea grabar el registro?";
                NuevoSiNo.ShowDialog();

                if (NuevoSiNo.DialogResult == DialogResult.OK)
                {
                    if (bNuevoRegistro == true)
                        insertarRegistro();
                    else
                        actualizarRegistro();
                }
            }

            else
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Por favor, ingrese registros para almacenar la receta.";
                ok.ShowDialog();
            }
        }

        //Función para grabar un Registro
        private void insertarRegistro()
        {
            try
            {
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Error al iniciar transacción.";
                    ok.ShowDialog();
                    return;
                }

                llenarValoresTexto();
                int a;

                sSql = "";
                sSql += "insert into pos_receta(" + Environment.NewLine;
                sSql += "idempresa, descripcion, codigo, num_porciones, id_pos_clasificacion_receta," + Environment.NewLine;
                sSql += "id_pos_tipo_receta, id_pos_origen_receta, id_pos_temperatura_de_servicio," + Environment.NewLine;
                sSql += "peso_en_gramos, porcentaje_utilidad_deseada, utilidad_de_ganancias," + Environment.NewLine;
                sSql += "porcentaje_servicio_deseado, utilidad_de_servicios, rendimiento, porcentaje_costo," + Environment.NewLine;
                sSql += "porcentaje_utilidad, costo_unitario, costo_total, precio_de_venta, id_producto," + Environment.NewLine;
                sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += "@idempresa, @descripcion, @codigo, @num_porciones, @id_pos_clasificacion_receta," + Environment.NewLine;
                sSql += "@id_pos_tipo_receta, @id_pos_origen_receta, @id_pos_temperatura_de_servicio," + Environment.NewLine;
                sSql += "@peso_en_gramos, @porcentaje_utilidad_deseada, @utilidad_de_ganancias," + Environment.NewLine;
                sSql += "@porcentaje_servicio_deseado, @utilidad_de_servicios, @rendimiento, @porcentaje_costo," + Environment.NewLine;
                sSql += "@porcentaje_utilidad, @costo_unitario, @costo_total, @precio_de_venta, @id_producto," + Environment.NewLine;
                sSql += "@estado, getdate(), @usuario_ingreso, @terminal_ingreso)";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[23];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@idempresa";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Program.iIdEmpresa;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@descripcion";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = lblPlatillo.Text.Trim().ToUpper();
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@codigo";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@num_porciones";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Convert.ToInt32(txtNumeroPorciones.Text);
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pos_clasificacion_receta";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Convert.ToInt32(cmbClasificacion.SelectedValue);
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pos_tipo_receta";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Convert.ToInt32(cmbReceta.SelectedValue);
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pos_origen_receta";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Convert.ToInt32(cmbOrigen.SelectedValue);
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pos_temperatura_de_servicio";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Convert.ToInt32(cmbTemperatura.SelectedValue);
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@peso_en_gramos";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtPesoGramos.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@porcentaje_utilidad_deseada";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtPorcentajeGananciaDeseada.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@utilidad_de_ganancias";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtUtilidadDeGanancias.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@porcentaje_servicio_deseado";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtPorcentajeServicioDeseado.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@utilidad_de_servicios";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtUtilidadDeServicios.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@rendimiento";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtRendimiento.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@porcentaje_costo";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtPorcentajeDeCosto.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@porcentaje_utilidad";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtPorcentajeDeUtilidad.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@costo_unitario";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtCostoUnitario.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@costo_total";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtCostoTotal.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@precio_de_venta";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtPrecioDeVenta.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_producto";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdProductoActualizar;
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

                //PROCEDIMINTO PARA EXTRAER EL ID DE LA TABLA POS_RECETA
                sTabla = "pos_receta";
                sCampo = "id_pos_receta";

                iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                if (iMaximo == -1)
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                    ok.ShowDialog();
                    goto reversa;
                }

                else
                {
                    iIdReceta = Convert.ToInt32(iMaximo);
                }

                //ACTUALIZAR EL CODIGO DE LA RECETA
                sSql = "";
                sSql += "update pos_receta set" + Environment.NewLine;
                sSql += "codigo = @codigo" + Environment.NewLine;
                sSql += "where id_pos_receta = @id_pos_receta";

                #region PARAMETROS

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@codigo";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "REC" + iIdReceta.ToString();

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@id_pos_receta";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = iIdReceta;

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                //=================================================================================

                for (int i = 0; i < dgvIngredientes.Rows.Count; i++)
                {
                    iIdProducto_R = Convert.ToInt32(dgvIngredientes.Rows[i].Cells["id_producto"].Value);
                    dbCantidadBruta_R = Convert.ToDecimal(dgvIngredientes.Rows[i].Cells["cantidad_bruta"].Value);
                    dbCantidadNeta_R = Convert.ToDecimal(dgvIngredientes.Rows[i].Cells["cantidad_neta"].Value);
                    dbRendimiento_R = Convert.ToDecimal(dgvIngredientes.Rows[i].Cells["rendimiento"].Value);
                    iIdUnidad_R = Convert.ToInt32(dgvIngredientes.Rows[i].Cells["id_pos_unidad"].Value);
                    dbCostoUnitario_R = Convert.ToDecimal(dgvIngredientes.Rows[i].Cells["costo"].Value);
                    dbImporte_R = dbCantidadBruta_R * dbCostoUnitario_R;

                    sSql = "";
                    sSql += "insert into pos_detalle_receta (" + Environment.NewLine;
                    sSql += "id_pos_receta, id_producto, cantidad_bruta, cantidad_neta," + Environment.NewLine;
                    sSql += "id_pos_unidad, costo_unitario, importe, rendimiento, estado," + Environment.NewLine;
                    sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                    sSql += "values (" + Environment.NewLine;
                    sSql += "@id_pos_receta, @id_producto, @cantidad_bruta, @cantidad_neta," + Environment.NewLine;
                    sSql += "@id_pos_unidad, @costo_unitario, @importe, @rendimiento, @estado," + Environment.NewLine;
                    sSql += "getdate(), @usuario_ingreso, @terminal_ingreso)";

                    #region PARAMETROS

                    a = 0;
                    parametro = new SqlParameter[11];
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_pos_receta";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdReceta;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_producto";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdProducto_R;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@cantidad_bruta";
                    parametro[a].SqlDbType = SqlDbType.Decimal;
                    parametro[a].Value = dbCantidadBruta_R;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@cantidad_neta";
                    parametro[a].SqlDbType = SqlDbType.Decimal;
                    parametro[a].Value = dbCantidadNeta_R;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_pos_unidad";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdUnidad_R;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@costo_unitario";
                    parametro[a].SqlDbType = SqlDbType.Decimal;
                    parametro[a].Value = dbCostoUnitario_R;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@importe";
                    parametro[a].SqlDbType = SqlDbType.Decimal;
                    parametro[a].Value = dbImporte_R;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@rendimiento";
                    parametro[a].SqlDbType = SqlDbType.Decimal;
                    parametro[a].Value = dbRendimiento_R;
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
                }

                //if (iIdProductoActualizar != 0)
                //{
                //    //ACTUALIZAR EL ID DE LA RECETA EN EL PRODUCTO
                //    sSql = "";
                //    sSql += "update cv401_productos set" + Environment.NewLine;
                //    sSql += "id_pos_receta = @id_pos_receta" + Environment.NewLine;
                //    sSql += "where id_producto = @id_producto" + Environment.NewLine;
                //    sSql += "and estado = @estado";

                //    #region PARAMETROS

                //    parametro = new SqlParameter[3];
                //    parametro[0] = new SqlParameter();
                //    parametro[0].ParameterName = "@id_pos_receta";
                //    parametro[0].SqlDbType = SqlDbType.Int;
                //    parametro[0].Value = iIdReceta;

                //    parametro[1] = new SqlParameter();
                //    parametro[1].ParameterName = "@id_producto";
                //    parametro[1].SqlDbType = SqlDbType.Int;
                //    parametro[1].Value = iIdProductoActualizar;

                //    parametro[2] = new SqlParameter();
                //    parametro[2].ParameterName = "@estado";
                //    parametro[2].SqlDbType = SqlDbType.VarChar;
                //    parametro[2].Value = "A";

                //    #endregion

                //    if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                //    {
                //        catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                //        catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                //        catchMensaje.ShowDialog();
                //        goto reversa;
                //    }
                //}

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);

                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Registro Guardado Correctamente.";
                ok.ShowDialog();

                //if (iBanderaAsignacion == 1)
                //    this.DialogResult = DialogResult.OK;

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

        //Función para actualizar un registro
        private void actualizarRegistro()
        {
            try
            {
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Error al iniciar la transacción.";
                    ok.ShowDialog();
                    return;
                }

                llenarValoresTexto();
                int a;

                sSql = "";
                sSql += "update pos_receta set" + Environment.NewLine;
                sSql += "descripcion = @descripcion," + Environment.NewLine;
                sSql += "num_porciones = @num_porciones," + Environment.NewLine;
                sSql += "id_pos_clasificacion_receta = @id_pos_clasificacion_receta," + Environment.NewLine;
                sSql += "id_pos_tipo_receta = @id_pos_tipo_receta," + Environment.NewLine;
                sSql += "id_pos_origen_receta = @id_pos_origen_receta," + Environment.NewLine;
                sSql += "id_pos_temperatura_de_servicio = @id_pos_temperatura_de_servicio," + Environment.NewLine;
                sSql += "peso_en_gramos = @peso_en_gramos," + Environment.NewLine;
                sSql += "porcentaje_utilidad_deseada = @porcentaje_utilidad_deseada," + Environment.NewLine;
                sSql += "utilidad_de_ganancias = @utilidad_de_ganancias," + Environment.NewLine;
                sSql += "porcentaje_servicio_deseado = @porcentaje_servicio_deseado," + Environment.NewLine;
                sSql += "utilidad_de_servicios = @utilidad_de_servicios," + Environment.NewLine;
                sSql += "rendimiento = @rendimiento," + Environment.NewLine;
                sSql += "porcentaje_costo = @porcentaje_costo," + Environment.NewLine;
                sSql += "porcentaje_utilidad = @porcentaje_utilidad," + Environment.NewLine;
                sSql += "costo_unitario = @costo_unitario," + Environment.NewLine;
                sSql += "costo_total = @costo_total," + Environment.NewLine;
                sSql += "precio_de_venta = @precio_de_venta," + Environment.NewLine;
                sSql += "id_producto = @id_producto" + Environment.NewLine;
                sSql += "where id_pos_receta = @id_pos_receta" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                a = 0;
                parametro = new SqlParameter[20];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@descripcion";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = lblPlatillo.Text.Trim().ToUpper();
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@num_porciones";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Convert.ToInt32(txtNumeroPorciones.Text);
                a++;                

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pos_clasificacion_receta";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Convert.ToInt32(cmbClasificacion.SelectedValue);
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pos_tipo_receta";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Convert.ToInt32(cmbReceta.SelectedValue);
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pos_origen_receta";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Convert.ToInt32(cmbOrigen.SelectedValue);
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pos_temperatura_de_servicio";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Convert.ToInt32(cmbTemperatura.SelectedValue);
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@peso_en_gramos";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtPesoGramos.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@porcentaje_utilidad_deseada";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtPorcentajeGananciaDeseada.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@utilidad_de_ganancias";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtUtilidadDeGanancias.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@porcentaje_servicio_deseado";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtPorcentajeServicioDeseado.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@utilidad_de_servicios";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtUtilidadDeServicios.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@rendimiento";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtRendimiento.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@porcentaje_costo";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtPorcentajeDeCosto.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@porcentaje_utilidad";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtPorcentajeDeUtilidad.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@costo_unitario";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtCostoUnitario.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@costo_total";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtCostoTotal.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@precio_de_venta";
                parametro[a].SqlDbType = SqlDbType.Decimal;
                parametro[a].Value = Convert.ToDecimal(txtPrecioDeVenta.Text.Trim());
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_producto";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdProductoActualizar;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pos_receta";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdReceta;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado";
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
                
                //==============================================================================================================
                sSql = "";
                sSql += "update pos_detalle_receta set" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_pos_receta = @id_pos_receta" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARMETROS

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
                parametro[a].ParameterName = "@id_pos_receta";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdReceta;
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

                //==============================================================================================================

                for (int i = 0; i < dgvIngredientes.Rows.Count; i++)
                {
                    iIdProducto_R = Convert.ToInt32(dgvIngredientes.Rows[i].Cells["id_producto"].Value);
                    dbCantidadBruta_R = Convert.ToDecimal(dgvIngredientes.Rows[i].Cells["cantidad_bruta"].Value);
                    dbCantidadNeta_R = Convert.ToDecimal(dgvIngredientes.Rows[i].Cells["cantidad_neta"].Value);
                    dbRendimiento_R = Convert.ToDecimal(dgvIngredientes.Rows[i].Cells["rendimiento"].Value);
                    iIdUnidad_R = Convert.ToInt32(dgvIngredientes.Rows[i].Cells["id_pos_unidad"].Value);
                    dbCostoUnitario_R = Convert.ToDecimal(dgvIngredientes.Rows[i].Cells["costo"].Value);
                    dbImporte_R = dbCantidadBruta_R * dbCostoUnitario_R;

                    sSql = "";
                    sSql += "insert into pos_detalle_receta (" + Environment.NewLine;
                    sSql += "id_pos_receta, id_producto, cantidad_bruta, cantidad_neta," + Environment.NewLine;
                    sSql += "id_pos_unidad, costo_unitario, importe, rendimiento, estado," + Environment.NewLine;
                    sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                    sSql += "values (" + Environment.NewLine;
                    sSql += "@id_pos_receta, @id_producto, @cantidad_bruta, @cantidad_neta," + Environment.NewLine;
                    sSql += "@id_pos_unidad, @costo_unitario, @importe, @rendimiento, @estado," + Environment.NewLine;
                    sSql += "getdate(), @usuario_ingreso, @terminal_ingreso)";

                    #region PARAMETROS

                    a = 0;
                    parametro = new SqlParameter[11];
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_pos_receta";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdReceta;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_producto";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdProducto_R;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@cantidad_bruta";
                    parametro[a].SqlDbType = SqlDbType.Decimal;
                    parametro[a].Value = dbCantidadBruta_R;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@cantidad_neta";
                    parametro[a].SqlDbType = SqlDbType.Decimal;
                    parametro[a].Value = dbCantidadNeta_R;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_pos_unidad";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdUnidad_R;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@costo_unitario";
                    parametro[a].SqlDbType = SqlDbType.Decimal;
                    parametro[a].Value = dbCostoUnitario_R;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@importe";
                    parametro[a].SqlDbType = SqlDbType.Decimal;
                    parametro[a].Value = dbImporte_R;
                    a++;

                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@rendimiento";
                    parametro[a].SqlDbType = SqlDbType.Decimal;
                    parametro[a].Value = dbRendimiento_R;
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
                }

                //if (iIdProductoActualizar != 0)
                //{
                //    //ACTUALIZAR EL ID DE LA RECETA EN EL PRODUCTO
                //    sSql = "";
                //    sSql += "update cv401_productos set" + Environment.NewLine;
                //    sSql += "id_pos_receta = @id_pos_receta" + Environment.NewLine;
                //    sSql += "where id_producto = @id_producto" + Environment.NewLine;
                //    sSql += "and estado = @estado";

                //    #region PARAMETROS

                //    parametro = new SqlParameter[3];
                //    parametro[0] = new SqlParameter();
                //    parametro[0].ParameterName = "@id_pos_receta";
                //    parametro[0].SqlDbType = SqlDbType.Int;
                //    parametro[0].Value = iIdReceta;

                //    parametro[1] = new SqlParameter();
                //    parametro[1].ParameterName = "@id_producto";
                //    parametro[1].SqlDbType = SqlDbType.Int;
                //    parametro[1].Value = iIdProductoActualizar;

                //    parametro[2] = new SqlParameter();
                //    parametro[2].ParameterName = "@estado";
                //    parametro[2].SqlDbType = SqlDbType.VarChar;
                //    parametro[2].Value = "A";

                //    #endregion

                //    if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                //    {
                //        catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                //        catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                //        catchMensaje.ShowDialog();
                //        goto reversa;
                //    }
                //}

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);

                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Registro Actualizado Éxitosamente.";
                ok.ShowDialog();

                //if (iBanderaAsignacion == 1)
                //    this.DialogResult = DialogResult.OK;

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

        //Función para anular un registro
        private void anularRegistro(int ibandera)
        {
            try
            {
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Error al iniciar la transacción.";
                    ok.ShowDialog();
                    return;
                }

                int a;

                sSql = "";
                sSql += "update pos_receta set" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula," + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula" + Environment.NewLine;
                sSql += "where id_pos_receta = @id_pos_receta" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                #region PARMETROS

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
                parametro[a].ParameterName = "@id_pos_receta";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdReceta;
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

                sSql = "";
                sSql += "update pos_detalle_receta set" + Environment.NewLine;
                sSql += "estado = @estado_1," + Environment.NewLine;
                sSql += "fecha_anula = getdate()," + Environment.NewLine;
                sSql += "usuario_anula = @usuario_anula,'" + Environment.NewLine;
                sSql += "terminal_anula = @terminal_anula," + Environment.NewLine;
                sSql += "where id_pos_receta = @id_pos_receta" + Environment.NewLine;
                sSql += "and estado = @estado_2";

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);

                if (ibandera != 1)
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Registro Eliminado Correctamente.";
                    ok.ShowDialog();
                    limpiarTodo();
                }

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

        //FUNCION PARA VALIDAR EL GRID DE INSUMOS RECORRIENDO LOS INGREDIENTES PARA NO REPETIR
        private bool verificarIngredientes()
        {
            try
            {
                for (int i = dgvInsumos.Rows.Count - 1; i >= 0; i--)
                {
                    int iIdProductoInsumo = Convert.ToInt32(dgvInsumos.Rows[i].Cells["id_producto_insumo"].Value);

                    for (int j = 0; j < dgvIngredientes.Rows.Count; j++)
                    {
                        int iIdProductoIngrediente = Convert.ToInt32(dgvIngredientes.Rows[j].Cells["id_producto"].Value);

                        if (iIdProductoInsumo == iIdProductoIngrediente)
                        {
                            dgvInsumos.Rows.Remove(dgvInsumos.Rows[i]);
                            break;
                        }
                    }
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

        //FUNCION PARA LISTAR LAS RECETAS
        private bool listarRecetas()
        {
            try
            {
                dgvRecetas.Rows.Clear();

                int b = 1;

                sSql = "";
                sSql += "select id_pos_receta, codigo, descripcion" + Environment.NewLine;
                sSql += "from pos_receta" + Environment.NewLine;
                sSql += "where estado = @estado" + Environment.NewLine;

                if (txtBuscar.Text.Trim() != "")
                {
                    b++;
                    sSql += "and descripcion like '%' + @buscar + '%'" + Environment.NewLine;
                }

                sSql += "order by descripcion";

                #region PARAMETROS

                parametro = new SqlParameter[b];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@estado";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "A";

                if (b == 2)
                {
                    parametro[1] = new SqlParameter();
                    parametro[1].ParameterName = "@buscar";
                    parametro[1].SqlDbType = SqlDbType.VarChar;
                    parametro[1].Value = txtBuscar.Text.Trim();
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
                    return false;
                }

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    dgvRecetas.Rows.Add(dtConsulta.Rows[i]["id_pos_receta"].ToString(),
                                        dtConsulta.Rows[i]["codigo"].ToString(),
                                        dtConsulta.Rows[i]["descripcion"].ToString());
                }

                dgvRecetas.ClearSelection();
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

        //Función para recuperar información
        private bool recuperarInformacion()
        {
            try
            {
                //if (iBanderaAsignacion == 0)
                //    iIdReceta = dbAyudaReceta.iId;

                //habilitarControles();

                //sSql = "";
                //sSql += "select id_producto" + Environment.NewLine;
                //sSql += "from cv401_productos" + Environment.NewLine;
                //sSql += "where id_pos_receta = @id_pos_receta" + Environment.NewLine;
                //sSql += "and estado = @estado";

                //#region PARAMETROS 

                //parametro = new SqlParameter[2];
                //parametro[0] = new SqlParameter();
                //parametro[0].ParameterName = "@id_pos_receta";
                //parametro[0].SqlDbType = SqlDbType.Int;
                //parametro[0].Value = iIdReceta;

                //parametro[1] = new SqlParameter();
                //parametro[1].ParameterName = "@estado";
                //parametro[1].SqlDbType = SqlDbType.VarChar;
                //parametro[1].Value = "A";

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
                //    iIdProductoActualizar = 0;
                //else
                //    iIdProductoActualizar = Convert.ToInt32(dtConsulta.Rows[0]["id_producto"].ToString());

                sSql = "";
                sSql += "select * from pos_vw_receta" + Environment.NewLine;
                sSql += "where id_pos_receta = @id_pos_receta";

                parametro = new SqlParameter[1];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_pos_receta";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdReceta;

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

                lblPlatillo.Text = dtConsulta.Rows[0]["descripcion"].ToString().ToUpper();
                lblTituloReceta.Text = "RECETA: " + dtConsulta.Rows[0]["descripcion"].ToString().ToUpper();
                lblCodigo.Text = dtConsulta.Rows[0]["codigo"].ToString().ToUpper();
                txtNumeroPorciones.Text = dtConsulta.Rows[0]["num_porciones"].ToString();
                cmbClasificacion.SelectedValue = dtConsulta.Rows[0]["id_pos_clasificacion_receta"].ToString();
                cmbReceta.SelectedValue = dtConsulta.Rows[0]["id_pos_tipo_receta"].ToString();
                cmbOrigen.SelectedValue = dtConsulta.Rows[0]["id_pos_origen_receta"].ToString();
                cmbTemperatura.SelectedValue = dtConsulta.Rows[0]["id_pos_temperatura_de_servicio"].ToString();
                txtPesoGramos.Text = dtConsulta.Rows[0]["peso_en_gramos"].ToString();
                txtPorcentajeGananciaDeseada.Text = dtConsulta.Rows[0]["porcentaje_utilidad_deseada"].ToString();
                txtUtilidadDeGanancias.Text = dtConsulta.Rows[0]["utilidad_de_ganancias"].ToString();
                txtPorcentajeServicioDeseado.Text = dtConsulta.Rows[0]["porcentaje_servicio_deseado"].ToString();
                txtUtilidadDeServicios.Text = dtConsulta.Rows[0]["utilidad_de_servicios"].ToString();
                txtRendimiento.Text = dtConsulta.Rows[0]["rendimiento"].ToString();
                txtPorcentajeDeCosto.Text = dtConsulta.Rows[0]["porcentaje_costo"].ToString();
                txtPorcentajeDeUtilidad.Text = dtConsulta.Rows[0]["porcentaje_utilidad"].ToString();
                txtCostoUnitario.Text = dtConsulta.Rows[0]["costo_unitario"].ToString();
                txtCostoTotal.Text = dtConsulta.Rows[0]["costo_total"].ToString();
                txtPrecioDeVenta.Text = dtConsulta.Rows[0]["precio_de_venta"].ToString();

                iIdProductoActualizar = Convert.ToInt32(dtConsulta.Rows[0]["id_producto"].ToString());

                if (Convert.ToDecimal(txtPesoGramos.Text.Trim()) == 0)
                    txtCostoPorGramo.Text = "0";
                else
                    txtCostoPorGramo.Text = (Convert.ToDecimal(txtCostoTotal.Text.Trim()) / Convert.ToDecimal(txtPesoGramos.Text.Trim())).ToString("N6");

                if (completarDetalleReceta(iIdReceta) == false)
                    return false;

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

        //Función para completar el detalle de la receta
        private bool completarDetalleReceta(int iIdReceta_P)
        {
            try
            {
                sSql = "";
                sSql += "select * from pos_vw_detalle_receta_normal" + Environment.NewLine;
                sSql += "where id_pos_receta = @id_pos_receta";

                parametro = new SqlParameter[1];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_pos_receta";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdReceta;

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

                Decimal dbSumaGramos = 0;

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    sCodigo_R = dtConsulta.Rows[i]["codigo"].ToString();
                    sNombre_R = dtConsulta.Rows[i]["nombre"].ToString();
                    sCantidadBruta_R = dtConsulta.Rows[i]["cantidad_bruta"].ToString();
                    sRendimiento_R = dtConsulta.Rows[i]["rendimiento"].ToString();
                    sCantidadNeta_R = dtConsulta.Rows[i]["cantidad_neta"].ToString();
                    sUnidad_R = dtConsulta.Rows[i]["abreviatura"].ToString();
                    sCostoUnitario_R = dtConsulta.Rows[i]["costo_unitario"].ToString();
                    sImporte_R = dtConsulta.Rows[i]["importe"].ToString();
                    sIdProducto_R = dtConsulta.Rows[i]["id_producto"].ToString();
                    sIdUnidad_R = dtConsulta.Rows[i]["id_pos_unidad"].ToString();

                    dbSumaGramos += Convert.ToDecimal(sCantidadNeta_R);

                    dgvIngredientes.Rows.Add(sIdProducto_R, sIdUnidad_R, sRendimiento_R, sCodigo_R, sNombre_R, 
                                             sCantidadBruta_R, sCantidadNeta_R, sUnidad_R,
                                             sCostoUnitario_R, sImporte_R);
                }

                txtCantidadNetaGramos.Text = dbSumaGramos.ToString("N2");

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

        private void frmCrearRecetas_Load(object sender, EventArgs e)
        {
            limpiarTodo();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbFamilia_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenarComboCategorias(Convert.ToInt32(cmbFamilia.SelectedValue));
        }

        private void cmbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(cmbCategoria.SelectedValue) == 0)
                {
                    cmbSubcategoria.Enabled = false;
                    llenarComboSubcategorias(0);
                    llenarComboProductos(0, 3);
                }

                int iIdProducto_P = Convert.ToInt32(cmbCategoria.SelectedValue);

                DataRow[] dFila = dtCategoria.Select("id_producto = " + iIdProducto_P);

                if (dFila.Length != 0)
                {
                    iSubcategoria_P = Convert.ToInt32(dFila[0][6].ToString());
                    iModificador_P = Convert.ToInt32(dFila[0][7].ToString());

                    if (iModificador_P == 0)
                    {
                        if (iSubcategoria_P == 0)
                        {
                            cmbSubcategoria.Enabled = false;
                            llenarComboSubcategorias(0);
                            llenarComboProductos(iIdProducto_P, 3);
                        }

                        else
                        {
                            cmbSubcategoria.Enabled = true;
                            cmbSubcategoria.SelectedIndexChanged -= new EventHandler(cmbSubcategoria_SelectedIndexChanged);
                            llenarComboSubcategorias(iIdProducto_P);
                            cmbSubcategoria.SelectedIndexChanged += new EventHandler(cmbSubcategoria_SelectedIndexChanged);
                            llenarComboProductos(Convert.ToInt32(cmbSubcategoria.SelectedValue), 4);
                        }
                    }

                    else
                    {
                        cmbSubcategoria.Enabled = false;
                        llenarComboSubcategorias(0);
                        llenarComboProductos(iIdProducto_P, 3);
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

        private void cmbSubcategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int iIdProducto_P = Convert.ToInt32(cmbSubcategoria.SelectedValue);
                llenarComboProductos(iIdProducto_P, 4);
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbProductos.SelectedValue) == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor seleccione el producto para ingresar la receta.";
                ok.ShowDialog();
                cmbProductos.Focus();
                return;
            }

            if (txtNumeroPorciones.Text.Trim() == "")
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor ingrese el número de porciones de la receta.";
                ok.ShowDialog();
                txtNumeroPorciones.Focus();
                return;
            }

            grupoInsumos.Enabled = true;
            grupoSeleccion.Enabled = false;
            btnGrabar.Enabled = true;
            btnGrabar_2.Enabled = true;
            chkProductoTerminado.Checked = false;

            iIdProductoActualizar = Convert.ToInt32(cmbProductos.SelectedValue);
            lblPlatillo.Text = cmbProductos.Text.Trim().ToUpper();
            lblTituloReceta.Text = "RECETA: " + cmbProductos.Text.Trim().ToUpper();
            lblCodigo.Text = txtNumeroPorciones.Text.Trim();

            cmbCategoriaInsumos.Focus();
        }

        private void cmbCategoriaInsumos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbCategoriaInsumos.SelectedValue) == 0)
            {
                dgvInsumos.Rows.Clear();
                dgvInformacion.Rows.Clear();
                return;
            }

            llenarGridInsumos(Convert.ToInt32(cmbCategoriaInsumos.SelectedValue));
        }

        private void dgvInsumos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int iIdProducto_P = Convert.ToInt32(dgvInsumos.CurrentRow.Cells["id_producto_insumo"].Value);
                int iIdUnidad_P = Convert.ToInt32(dgvInsumos.CurrentRow.Cells["id_unidad_insumo"].Value);

                string sCodigoProducto_P = dgvInsumos.CurrentRow.Cells["codigo_insumo"].Value.ToString().Trim().ToUpper();
                string sNombreProducto_P = dgvInsumos.CurrentRow.Cells["descripcion_insumo"].Value.ToString().Trim().ToUpper();
                string sUnidadConsumo_P = dgvInsumos.CurrentRow.Cells["unidad_consumo"].Value.ToString().Trim().ToUpper();

                Decimal dbPresentacion_P = Convert.ToDecimal(dgvInsumos.CurrentRow.Cells["presentacion_insumo"].Value);
                Decimal dbRendimiento_P = Convert.ToDecimal(dgvInsumos.CurrentRow.Cells["rendimiento_insumo"].Value);
                Decimal dbValorUnitario_P = Convert.ToDecimal(dgvInsumos.CurrentRow.Cells["precio_unitario_insumo"].Value);
                Decimal dbPorcentaje_P = (dbRendimiento_P * 100) / dbPresentacion_P;

                dgvInformacion.Rows.Clear();
                dgvInformacion.Rows.Add();
                dgvInformacion.Rows[0].Cells["costo_1"].Value = dbValorUnitario_P;
                dgvInformacion.Rows[0].Cells["unidad_medida_1"].Value = sUnidadConsumo_P;
                dgvInformacion.ClearSelection();

                Receta.frmCantidadesIngredientes ingrediente = new frmCantidadesIngredientes(1, 1, sUnidadConsumo_P, dbPorcentaje_P);
                ingrediente.ShowDialog();

                if (ingrediente.DialogResult == DialogResult.OK)
                {
                    Decimal dbCantBruta = ingrediente.dbCantidadBruta;
                    Decimal dbCantNeta = ingrediente.dbCantidadNeta;
                    Decimal dbTotal_P = dbCantBruta * dbValorUnitario_P;

                    dgvIngredientes.Rows.Add(iIdProducto_P.ToString(),
                                             iIdUnidad_P,
                                             dbPorcentaje_P,
                                             sCodigoProducto_P,
                                             sNombreProducto_P,
                                             dbCantBruta,
                                             dbCantNeta,
                                             sUnidadConsumo_P,
                                             dbValorUnitario_P,
                                             dbTotal_P
                                    );
                }

                dgvInsumos.Rows.Remove(dgvInsumos.CurrentRow);

                dgvInsumos.ClearSelection();
                dgvIngredientes.ClearSelection();
                llenarValoresTexto();
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        private void txtNumeroPorciones_KeyPress(object sender, KeyPressEventArgs e)
        {
            caracter = new Clases.ClaseValidarCaracteres();
            caracter.soloNumeros(e);
        }

        private void txtPorcentajeGananciaDeseada_KeyPress(object sender, KeyPressEventArgs e)
        {
            caracter = new Clases.ClaseValidarCaracteres();
            caracter.soloNumeros(e);
        }

        private void txtPorcentajeServicioDeseado_KeyPress(object sender, KeyPressEventArgs e)
        {
            caracter = new Clases.ClaseValidarCaracteres();
            caracter.soloNumeros(e);
        }

        private void txtPesoGramos_KeyPress(object sender, KeyPressEventArgs e)
        {
            caracter = new Clases.ClaseValidarCaracteres();
            caracter.soloDecimales(sender, e, 2);
        }

        private void txtPesoGramos_Leave(object sender, EventArgs e)
        {
            if (txtPesoGramos.Text.Trim() == "")
                txtPesoGramos.Text = "0";

            else if (Convert.ToDecimal(txtPesoGramos.Text.Trim()) > Convert.ToDecimal(txtCantidadNetaGramos.Text.Trim()))
            {
                Decimal dbDiferenciaPesos = Convert.ToDecimal(txtPesoGramos.Text.Trim()) - Convert.ToDecimal(txtCantidadNetaGramos.Text.Trim());
                Decimal dbPorcentaje = (dbDiferenciaPesos * 100) / Convert.ToDecimal(txtCantidadNetaGramos.Text.Trim());

                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "El peso total en gramos supera en el " + dbPorcentaje.ToString("N2") + "% a la cantidad neta ingresada en la receta.";
                ok.ShowDialog();
            }

            if (Convert.ToDecimal(txtPesoGramos.Text.Trim()) == 0)
                txtCostoPorGramo.Text = "0";
            else
                txtCostoPorGramo.Text = (Convert.ToDecimal(txtCostoTotal.Text.Trim()) / Convert.ToDecimal(txtPesoGramos.Text.Trim())).ToString("N6");
        }

        private void btnRemoverLinea_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvIngredientes.SelectedRows.Count == 0)
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
                    dgvIngredientes.Rows.Remove(dgvIngredientes.CurrentRow);
                    llenarGridInsumos(Convert.ToInt32(cmbCategoriaInsumos.SelectedValue));
                }

                if (dgvIngredientes.Rows.Count > 0)
                {
                    llenarValoresTexto();
                }

                else
                {
                    txtCantidadNetaGramos.Text = "0";
                    txtCostoTotal.Text = "0";
                    txtCostoPorGramo.Text = "0";
                    txtPesoGramos.Text = "0";
                    txtPrecioDeVenta.Text = "0";
                    txtCostoUnitario.Text = "0";
                    txtPorcentajeDeCosto.Text = "0";
                    txtPorcentajeDeUtilidad.Text = "0";
                    txtUtilidadDeServicios.Text = "0";
                    txtUtilidadDeGanancias.Text = "0";
                    txtRendimiento.Text = "0";
                }

                dgvIngredientes.ClearSelection();
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        private void dgvIngredientes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int iFila = dgvIngredientes.CurrentRow.Index;

                if (iFila == -1)
                    return;

                Decimal dbCantBruta_P = Convert.ToDecimal(dgvIngredientes.CurrentRow.Cells["cantidad_bruta"].Value);
                Decimal dbCantNeta_P = Convert.ToDecimal(dgvIngredientes.CurrentRow.Cells["cantidad_neta"].Value);
                Decimal dbPorcentaje_P = Convert.ToDecimal(dgvIngredientes.CurrentRow.Cells["rendimiento"].Value);
                Decimal dbValorUnitario_P = Convert.ToDecimal(dgvIngredientes.CurrentRow.Cells["costo"].Value);
                string sUnidadConsumo_P = dgvIngredientes.CurrentRow.Cells["unidad"].Value.ToString().Trim().ToUpper();

                Receta.frmCantidadesIngredientes ingrediente = new frmCantidadesIngredientes(dbCantBruta_P, dbCantNeta_P, sUnidadConsumo_P, dbPorcentaje_P);
                ingrediente.ShowDialog();

                if (ingrediente.DialogResult == DialogResult.OK)
                {
                    dbCantBruta_P = ingrediente.dbCantidadBruta;
                    dbCantNeta_P = ingrediente.dbCantidadNeta;
                    Decimal dbTotal_P = dbCantBruta_P * dbValorUnitario_P;

                    dgvIngredientes.Rows[iFila].Cells["cantidad_bruta"].Value = dbCantBruta_P;
                    dgvIngredientes.Rows[iFila].Cells["cantidad_neta"].Value = dbCantNeta_P;
                    dgvIngredientes.Rows[iFila].Cells["subtotal"].Value = dbTotal_P;
                }

                dgvIngredientes.ClearSelection();
                llenarValoresTexto();
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        private void btnCancelar_2_Click(object sender, EventArgs e)
        {
            limpiarTodo();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiarTodo();
        }

        private void btnAgregarInsumo_Click(object sender, EventArgs e)
        {
            Productos.frmInsumoSimplificado insumo = new Productos.frmInsumoSimplificado();
            insumo.ShowDialog();

            if (insumo.DialogResult == DialogResult.OK)
            {
                int iIdProductoCategoria_P = insumo.iIdProductoPadre;
                insumo.Close();
                cmbCategoriaInsumos.SelectedValue = iIdProductoCategoria_P;
                //llenarGridInsumos(iIdProductoCategoria_P);
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabControl.TabPages["tabInsumo"];
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            validarGuardarInformacion();
        }

        private void btnGrabar_2_Click(object sender, EventArgs e)
        {
            validarGuardarInformacion();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listarRecetas();
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                listarRecetas();
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            iniciarReceta(1);
            iTabHabilitados = 1;
        }

        private void btnCancelar_3_Click(object sender, EventArgs e)
        {
            limpiarTodo();
        }

        private void dgvRecetas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                iIdReceta = Convert.ToInt32(dgvRecetas.CurrentRow.Cells["id_receta_lista"].Value);
                iniciarReceta(0);

                if (recuperarInformacion() == false)
                {
                    limpiarTodo();
                    return;
                }

                bNuevoRegistro = false;
                grupoInsumos.Enabled = true;
                grupoSeleccion.Enabled = false;
                btnGrabar.Enabled = true;
                btnGrabar_2.Enabled = true;
                chkProductoTerminado.Checked = false;
                btnNuevo.Enabled = false;
                tabControl.SelectedTab = tabControl.TabPages["tabInformacion"];

                dgvIngredientes.ClearSelection();
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabControl.TabPages["tabListado"])
            {
                dgvRecetas.ClearSelection();
                return;
            }

            if (tabControl.SelectedTab == tabControl.TabPages["tabInsumo"])
            {
                dgvInsumos.ClearSelection();
                dgvIngredientes.ClearSelection();
                return;
            }
        }
    }
}
