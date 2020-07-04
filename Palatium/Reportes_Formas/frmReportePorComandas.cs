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

namespace Palatium.Reportes_Formas
{
    public partial class frmReportePorComandas : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoOk ok;
        VentanasMensajes.frmMensajeNuevoSiNo NuevoSiNo;

        Clases.ClaseValidarCaracteres caracter;

        string sSql;
        string sFecha;
        string sFechaDesde;
        string sFechaHasta;

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
        int iMostraDetalles;

        SqlParameter[] parametro;

        public frmReportePorComandas()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

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

        //FUNCION PARA LLENAR EL COMBOBOX DE JORNADAS
        private void llenarComboJornadas()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_jornada, descripcion" + Environment.NewLine;
                sSql += "from pos_jornada" + Environment.NewLine;
                sSql += "where estado = @estado" + Environment.NewLine;
                sSql += "order by orden";

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
                row["id_pos_jornada"] = "0";
                row["descripcion"] = "Todos...!!!";
                dtConsulta.Rows.InsertAt(row, 0);

                cmbJornada.DisplayMember = "descripcion";
                cmbJornada.ValueMember = "id_pos_jornada";
                cmbJornada.DataSource = dtConsulta;
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
                row["nombre"] = "Todos...!!!";
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
                row["nombre"] = "Todos...!!!";
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

        //FUNCION PARA LLENAR EL COMBOBOX DE TIPO DE ORIGEN
        private void llenarComboTipoOrigen()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_origen_orden, descripcion" + Environment.NewLine;
                sSql += "from pos_origen_orden" + Environment.NewLine;
                sSql += "where estado = @estado" + Environment.NewLine;
                sSql += "order by codigo";

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
                row["id_pos_origen_orden"] = "0";
                row["descripcion"] = "Todos...!!!";
                dtConsulta.Rows.InsertAt(row, 0);

                cmbOrigen.DisplayMember = "descripcion";
                cmbOrigen.ValueMember = "id_pos_origen_orden";
                cmbOrigen.DataSource = dtConsulta;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return;
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE MESEROS
        private void llenarComboMeseros()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_mesero, descripcion" + Environment.NewLine;
                sSql += "from pos_mesero" + Environment.NewLine;
                sSql += "where estado = @estado" + Environment.NewLine;
                sSql += "order by descripcion";

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
                row["id_pos_mesero"] = "0";
                row["descripcion"] = "Todos...!!!";
                dtConsulta.Rows.InsertAt(row, 0);

                cmbMesero.DisplayMember = "descripcion";
                cmbMesero.ValueMember = "id_pos_mesero";
                cmbMesero.DataSource = dtConsulta;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return;
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE SECCIONES DE MESAS
        private void llenarComboSeccionesMesas()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_seccion_mesa, descripcion" + Environment.NewLine;
                sSql += "from pos_seccion_mesa" + Environment.NewLine;
                sSql += "where estado = @estado" + Environment.NewLine;
                sSql += "order by descripcion";

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
                row["id_pos_seccion_mesa"] = "0";
                row["descripcion"] = "Todos...!!!";
                dtConsulta.Rows.InsertAt(row, 0);

                cmbSeccion.DisplayMember = "descripcion";
                cmbSeccion.ValueMember = "id_pos_seccion_mesa";
                cmbSeccion.DataSource = dtConsulta;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return;
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE MESAS
        private void llenarComboMesas(int iIdSeccion_P)
        {
            try
            {
                int a = 1;

                sSql = "";
                sSql += "select id_pos_mesa, descripcion" + Environment.NewLine;
                sSql += "from pos_mesa" + Environment.NewLine;
                sSql += "where estado = @estado" + Environment.NewLine;

                if (iIdSeccion_P != 0)
                {
                    a++;
                    sSql += "and id_pos_seccion_mesa = @id_pos_seccion_mesa" + Environment.NewLine;
                }

                sSql += "order by descripcion";

                parametro = new SqlParameter[a];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@estado";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "A";

                if (iIdSeccion_P != 0)
                {
                    parametro[1] = new SqlParameter();
                    parametro[1].ParameterName = "@id_pos_seccion_mesa";
                    parametro[1].SqlDbType = SqlDbType.Int;
                    parametro[1].Value = iIdSeccion_P;
                }

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
                row["id_pos_mesa"] = "0";
                row["descripcion"] = "Todos...!!!";
                dtConsulta.Rows.InsertAt(row, 0);

                cmbMesas.DisplayMember = "descripcion";
                cmbMesas.ValueMember = "id_pos_mesa";
                cmbMesas.DataSource = dtConsulta;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return;
            }
        }

        //LLENAR COMBOBOX DE FILTROS
        private void llenarComboFiltros()
        {
            try
            {
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                dtConsulta.Columns.Add("valor");
                dtConsulta.Columns.Add("mostrar");

                DataRow row = dtConsulta.NewRow();
                row["valor"] = "0";
                row["mostrar"] = "Ninguno";
                dtConsulta.Rows.Add(row);
                    
                row = dtConsulta.NewRow();
                row["valor"] = "fecha_pedido";
                row["mostrar"] = "Fecha";
                dtConsulta.Rows.Add(row);

                row = dtConsulta.NewRow();
                row["valor"] = "numero_pedido";
                row["mostrar"] = "Número de pedido";
                dtConsulta.Rows.Add(row);

                row = dtConsulta.NewRow();
                row["valor"] = "mesero";
                row["mostrar"] = "Mesero";
                dtConsulta.Rows.Add(row);

                cmbOrdenar.ValueMember = "valor";
                cmbOrdenar.DisplayMember = "mostrar";
                cmbOrdenar.DataSource = dtConsulta;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                dtConsulta.Columns.Add("valor");
                dtConsulta.Columns.Add("mostrar");

                row = dtConsulta.NewRow();
                row["valor"] = "0";
                row["mostrar"] = "Ninguno";
                dtConsulta.Rows.Add(row);

                row = dtConsulta.NewRow();
                row["valor"] = "asc";
                row["mostrar"] = "De menor a mayor";
                dtConsulta.Rows.Add(row);

                row = dtConsulta.NewRow();
                row["valor"] = "desc";
                row["mostrar"] = "De mayor a menor";
                dtConsulta.Rows.Add(row);

                cmbEnOrden.ValueMember = "valor";
                cmbEnOrden.DisplayMember = "mostrar";
                cmbEnOrden.DataSource = dtConsulta;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                dtConsulta.Columns.Add("valor");
                dtConsulta.Columns.Add("mostrar");

                row = dtConsulta.NewRow();
                row["valor"] = "0";
                row["mostrar"] = "Ninguno";
                dtConsulta.Rows.Add(row);

                row = dtConsulta.NewRow();
                row["valor"] = "fecha_pedido";
                row["mostrar"] = "Fecha";
                dtConsulta.Rows.Add(row);

                row = dtConsulta.NewRow();
                row["valor"] = "numero_mesa";
                row["mostrar"] = "Mesa";
                dtConsulta.Rows.Add(row);

                row = dtConsulta.NewRow();
                row["valor"] = "mesero";
                row["mostrar"] = "Mesero";
                dtConsulta.Rows.Add(row);

                row = dtConsulta.NewRow();
                row["valor"] = "descripcion";
                row["mostrar"] = "Sección";
                dtConsulta.Rows.Add(row);                

                cmbAgrupar.ValueMember = "valor";
                cmbAgrupar.DisplayMember = "mostrar";
                cmbAgrupar.DataSource = dtConsulta;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA CREAR LA CONSULTA
        private bool consultarRegistros()
        {
            try
            {
                int iIdProducto_P = 0;
                int iParametros_P = 2;
                int iBanderaJornada = 0;
                int iBanderaOrigenOrden = 0;
                int iBanderaMesero = 0;
                int iBanderaSeccion = 0;
                int iBanderaMesa = 0;
                int iBanderaCategoria = 0;

                if (Convert.ToInt32(cmbCategorias.SelectedValue) != 0)
                {
                    if (iSubcategoria_P == 1)
                    {
                        if (Convert.ToInt32(cmbSubCategorias.SelectedValue) == 0)
                        {
                            ok = new VentanasMensajes.frmMensajeNuevoOk(5);
                            ok.lblMensaje.Text = "Favor seleccione la subcategoría para proceder con la consulta.";
                            ok.ShowDialog();
                            cmbSubCategorias.Focus();
                            return false;
                        }

                        else
                            iIdProducto_P = Convert.ToInt32(cmbSubCategorias.SelectedValue);
                    }

                    else
                        iIdProducto_P = Convert.ToInt32(cmbCategorias.SelectedValue);
                }

                if (chkMostrarDetalles.Checked == false)
                    iMostraDetalles = 0;
                else
                    iMostraDetalles = 1;

                sSql = "";
                sSql += "select fecha_pedido, fecha_apertura_orden, numero_pedido, origen, mesero, jornada," + Environment.NewLine;
                sSql += "seccion, numero_mesa, ltrim( str(sum(cantidad * (precio_unitario - valor_dscto)), 10, 2)) valor";

                if (iMostraDetalles == 1)
                    sSql += ", nombre_producto" + Environment.NewLine;
                else
                    sSql += Environment.NewLine;

                sSql += "from pos_vw_reporte_de_comandas" + Environment.NewLine;
                sSql += "where fecha_pedido between @fecha_desde" + Environment.NewLine;
                sSql += "and @fecha_hasta" + Environment.NewLine;

                if (Convert.ToInt32(cmbJornada.SelectedValue) != 0)
                {
                    iBanderaJornada = 1;
                    iParametros_P++;
                    sSql += "and id_pos_jornada = @id_pos_jornada" + Environment.NewLine;
                }

                if (iIdProducto_P != 0)
                {
                    iBanderaCategoria = 1;
                    iParametros_P++;
                    sSql += "and id_producto_padre = @id_producto_padre" + Environment.NewLine;
                }

                if (Convert.ToInt32(cmbOrigen.SelectedValue) != 0)
                {
                    iBanderaOrigenOrden = 1;
                    iParametros_P++;
                    sSql += "and id_pos_origen_orden = @id_pos_origen_orden" + Environment.NewLine;
                }

                if (Convert.ToInt32(cmbMesero.SelectedValue) != 0)
                {
                    iBanderaMesero = 1;
                    iParametros_P++;
                    sSql += "and id_pos_mesero = @id_pos_mesero" + Environment.NewLine;
                }

                if (Convert.ToInt32(cmbSeccion.SelectedValue) != 0)
                {
                    iBanderaSeccion = 1;
                    iParametros_P++;
                    sSql += "and id_pos_seccion_mesa = @id_pos_seccion_mesa" + Environment.NewLine;
                }

                if (Convert.ToInt32(cmbMesas.SelectedValue) != 0)
                {
                    iBanderaMesa = 1;
                    iParametros_P++;
                    sSql += "and id_pos_mesa = @id_pos_mesa" + Environment.NewLine;
                }

                sSql += "group by fecha_pedido, fecha_apertura_orden, numero_pedido," + Environment.NewLine;
                sSql += "origen, mesero, jornada, seccion, numero_mesa";

                if (iMostraDetalles == 1)
                {
                    sSql += ", nombre_producto";
                }

                if (cmbOrdenar.SelectedValue != "0")
                {
                    sSql += Environment.NewLine;
                    sSql += "order by " + cmbOrdenar.SelectedValue;

                    if (cmbEnOrden.SelectedValue != "0")
                    {
                        sSql += " " + cmbEnOrden.SelectedValue;
                    }
                }

                #region PARAMETROS

                int a = 0;
                parametro = new SqlParameter[iParametros_P];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@fecha_desde";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = sFechaDesde;
                a++;
                
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@fecha_hasta";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = sFechaHasta;
                a++;

                if (iBanderaJornada == 1)
                {
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_pos_jornada";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = Convert.ToInt32(cmbJornada.SelectedValue);
                    a++;
                }

                if (iBanderaCategoria == 1)
                {
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_producto_padre";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdProducto_P;
                    a++;
                }

                if (iBanderaOrigenOrden == 1)
                {
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_pos_origen_orden";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = Convert.ToInt32(cmbOrigen.SelectedValue);
                    a++;
                }

                if (iBanderaMesero == 1)
                {
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_pos_mesero";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = Convert.ToInt32(cmbMesero.SelectedValue);
                    a++;
                }

                if (iBanderaSeccion == 1)
                {
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_pos_seccion_mesa";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = Convert.ToInt32(cmbSeccion.SelectedValue);
                    a++;
                }

                if (iBanderaMesa == 1)
                {
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_pos_mesa";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = Convert.ToInt32(cmbMesas.SelectedValue);
                    a++;
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

                if (dtConsulta.Rows.Count == 0)
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(5);
                    ok.lblMensaje.Text = "No se encuentran registros con los parámetros ingresados.";
                    ok.ShowDialog();
                }

                else
                {
                    Reportes_Formas.frmMostrarReporteComandas ver = new frmMostrarReporteComandas(dtConsulta, iMostraDetalles, sFechaDesde, sFechaHasta, Program.sDatosMaximo[0]);
                    ver.ShowInTaskbar = false;
                    ver.ShowDialog();
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

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            fechaSistema();
            dtFechaDesde.Text = sFecha;
            dtFechaHasta.Text = sFecha;

            llenarComboJornadas();

            cmbCategorias.SelectedIndexChanged -= new EventHandler(cmbCategorias_SelectedIndexChanged);
            llenarComboCategorias();
            cmbCategorias.SelectedIndexChanged += new EventHandler(cmbCategorias_SelectedIndexChanged);

            llenarComboSubCategorias(0);
            llenarComboTipoOrigen();
            llenarComboMeseros();

            cmbSeccion.SelectedIndexChanged -= new EventHandler(cmbSeccion_SelectedIndexChanged);
            llenarComboSeccionesMesas();
            cmbSeccion.SelectedIndexChanged += new EventHandler(cmbSeccion_SelectedIndexChanged);

            llenarComboMesas(0);
            llenarComboFiltros();
        }

        #endregion

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmReportePorComandas_Load(object sender, EventArgs e)
        {
            limpiar();
        }

        private void cmbCategorias_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(cmbCategorias.SelectedValue) == 0)
                {
                    cmbSubCategorias.Enabled = false;
                    iSubcategoria_P = 0;
                    iModificador_P = 0;
                    llenarComboSubCategorias(0);
                }

                int iIdProducto_P = Convert.ToInt32(cmbCategorias.SelectedValue);

                DataRow[] dFila = dtCategorias.Select("id_producto = " + iIdProducto_P);

                if (dFila.Length != 0)
                {
                    iSubcategoria_P = Convert.ToInt32(dFila[0]["subcategoria"].ToString());
                    iModificador_P = Convert.ToInt32(dFila[0]["modificador"].ToString());

                    if (iModificador_P == 0)
                    {
                        if (iSubcategoria_P == 0)
                        {
                            cmbSubCategorias.Enabled = false;
                            llenarComboSubCategorias(0);
                        }

                        else
                        {
                            cmbSubCategorias.Enabled = true;
                            llenarComboSubCategorias(iIdProducto_P);
                        }
                    }

                    else
                    {
                        cmbSubCategorias.Enabled = false;
                        iSubcategoria_P = 0;
                        iModificador_P = 0;
                        llenarComboSubCategorias(0);
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

        private void cmbSeccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenarComboMesas(Convert.ToInt32(cmbSeccion.SelectedValue));
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
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
            consultarRegistros();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }
    }
}
