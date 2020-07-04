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

namespace Palatium.Inventario
{
    public partial class frmInsumosConsumidos : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoOk ok;
        VentanasMensajes.frmMensajeNuevoSiNo SiNo;

        string sSql;
        string sFecha;
        string sFechaDesde;
        string sFechaHasta;

        DataTable dtConsulta;

        bool bRespuesta;

        int iIdLocalidad;
        int iTabHabilitados;

        SqlParameter[] parametro;

        public frmInsumosConsumidos()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //LLENAR EL COMBOBX DE LOCALIDADES
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

                cmbLocalidad.ValueMember = "id_localidad";
                cmbLocalidad.DisplayMember = "nombre_localidad";
                cmbLocalidad.DataSource = dtConsulta;

                cmbLocalidad.SelectedValue = Program.iIdLocalidad;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

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

        //FUNCION PARA LLENAR EL GRID
        private bool llenarGrid()
        {
            try
            {
                dgvDatos.Rows.Clear();

                int iBanderaParametro = 0;
                int iCantidadParametro = 2;

                sSql = "";
                sSql += "select id_producto, nombre, unidad, sum(cantidad) cantidad," + Environment.NewLine;
                sSql += "sum(precio_promedio) precio_promedio" + Environment.NewLine;
                sSql += "from pos_vw_insumos_consumidos" + Environment.NewLine;
                sSql += "where fecha between @fecha_desde" + Environment.NewLine;
                sSql += "and @fecha_hasta" + Environment.NewLine;

                if (iIdLocalidad != 0)
                {
                    iBanderaParametro = 1;
                    iCantidadParametro++;
                    sSql += "and id_localidad = @id_localidad" + Environment.NewLine;
                }

                sSql += "group by id_producto, nombre, unidad" + Environment.NewLine;
                sSql += "order by nombre";

                #region PARAMETROS

                int a = 0;
                parametro = new SqlParameter[iCantidadParametro];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@fecha_desde";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = sFechaDesde;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@fecha_hasta";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = sFechaHasta;
                
                if (iBanderaParametro == 1)
                {
                    a++;
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_localidad";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdLocalidad;
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
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "No existen registros con los parámetros ingresados.";
                    ok.ShowDialog();
                }

                else
                {
                    Decimal dbPrecioPromedio_R;
                    Decimal dbCantidad_R;
                    Decimal dbSubtotal_R;

                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        dbPrecioPromedio_R = Convert.ToDecimal(dtConsulta.Rows[i]["precio_promedio"].ToString());
                        dbCantidad_R = Convert.ToDecimal(dtConsulta.Rows[i]["cantidad"].ToString()) * -1;

                        if (dbPrecioPromedio_R == 0)
                            dbSubtotal_R = 0;
                        else
                            dbSubtotal_R = dbPrecioPromedio_R / dbCantidad_R;

                        dgvDatos.Rows.Add(dtConsulta.Rows[i]["id_producto"].ToString(),
                                          dtConsulta.Rows[i]["precio_promedio"].ToString(),
                                          //Convert.ToDateTime(dtConsulta.Rows[i][""].ToString()).ToString("dd-MM-yyyy"),
                                          dtConsulta.Rows[i]["nombre"].ToString().Trim().ToUpper(),
                                          dtConsulta.Rows[i]["unidad"].ToString().Trim().ToUpper(),
                                          dbCantidad_R,
                                          dbPrecioPromedio_R.ToString("N2"),
                                          dbSubtotal_R.ToString("N2"));
                    }
                }

                dgvDatos.ClearSelection();

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

        //FUNCION PARA LLENAR EL GRID DETALLADO
        private bool llenarGridDetallado(int iIdProducto_P)
        {
            try
            {
                dgvDetalle.Rows.Clear();

                int iBanderaParametro = 0;
                int iCantidadParametro = 3;

                sSql = "";
                sSql += "select numero_pedido, fecha, nombre, unidad," + Environment.NewLine;
                sSql += "cantidad, precio_promedio" + Environment.NewLine;
                sSql += "from pos_vw_insumos_consumidos" + Environment.NewLine;
                sSql += "where fecha between @fecha_desde" + Environment.NewLine;
                sSql += "and @fecha_hasta" + Environment.NewLine;
                sSql += "and id_producto = @id_producto" + Environment.NewLine;

                if (iIdLocalidad != 0)
                {
                    iBanderaParametro = 1;
                    iCantidadParametro++;
                    sSql += "and id_localidad = @id_localidad" + Environment.NewLine;
                }

                sSql += "order by numero_pedido, nombre";

                #region PARAMETROS

                int a = 0;
                parametro = new SqlParameter[iCantidadParametro];
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

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_producto";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdProducto_P;

                if (iBanderaParametro == 1)
                {
                    a++;
                    parametro[a] = new SqlParameter();
                    parametro[a].ParameterName = "@id_localidad";
                    parametro[a].SqlDbType = SqlDbType.Int;
                    parametro[a].Value = iIdLocalidad;
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
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "No existen registros con los parámetros ingresados.";
                    ok.ShowDialog();
                }

                else
                {
                    Decimal dbPrecioPromedio_R;
                    Decimal dbCantidad_R;
                    Decimal dbSubtotal_R;

                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        dbPrecioPromedio_R = Convert.ToDecimal(dtConsulta.Rows[i]["precio_promedio"].ToString());
                        dbCantidad_R = Convert.ToDecimal(dtConsulta.Rows[i]["cantidad"].ToString()) * -1;

                        if (dbPrecioPromedio_R == 0)
                            dbSubtotal_R = 0;
                        else
                            dbSubtotal_R = dbPrecioPromedio_R / dbCantidad_R;

                        dgvDetalle.Rows.Add(dtConsulta.Rows[i]["numero_pedido"].ToString(),
                                            Convert.ToDateTime(dtConsulta.Rows[i]["fecha"].ToString()).ToString("dd-MM-yyyy"),
                                            dtConsulta.Rows[i]["nombre"].ToString().Trim().ToUpper(),
                                            dtConsulta.Rows[i]["unidad"].ToString().Trim().ToUpper(),
                                            dbCantidad_R,
                                            dbPrecioPromedio_R.ToString("N2"),
                                            dbSubtotal_R.ToString("N2"));
                    }
                }

                dgvDetalle.ClearSelection();

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
            iTabHabilitados = 0;
            tabDatos.TabPages.Remove(tabDetallado);
            tabDatos.SelectedTab = tabDatos.TabPages["tabConsolidado"];

            fechaSistema();
            llenarComboLocalidades();
            
            dtFechaDesde.Text = sFecha;
            dtFechaHasta.Text = sFecha;
            btnExportar.Visible = false;
            dgvDatos.Rows.Clear();
        }

        #endregion

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmInsumosConsumidos_Load(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
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
            iIdLocalidad = Convert.ToInt32(cmbLocalidad.SelectedValue);

            llenarGrid();
        }

        private void dgvDatos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (iTabHabilitados == 0)
                {
                    tabDatos.TabPages.Add(tabDetallado);
                    iTabHabilitados = 1;
                }

                int iIdProducto_A = Convert.ToInt32(dgvDatos.CurrentRow.Cells["id_producto"].Value);

                llenarGridDetallado(iIdProducto_A);
                tabDatos.SelectedTab = tabDatos.TabPages["tabDetallado"];
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        private void tabDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabDatos.SelectedTab == tabDatos.TabPages["tabConsolidado"])
            {
                dgvDatos.ClearSelection();
                return;
            }

            if (tabDatos.SelectedTab == tabDatos.TabPages["tabDetallado"])
            {
                dgvDetalle.ClearSelection();
                return;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }
    }
}
