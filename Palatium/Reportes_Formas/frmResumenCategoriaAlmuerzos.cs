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
    public partial class frmResumenCategoriaAlmuerzos : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoOk ok;
        VentanasMensajes.frmMensajeNuevoSiNo SiNo;

        Clases.ClaseFunciones funciones;
        Clases.ClaseExcel exportarExcel;

        DataGridView dgvExportar;

        string sSql;
        string sFecha;
        string sFechaDesde;
        string sFechaHasta;

        int iIdLocalidad;

        DataTable dtConsulta;

        bool bRespuesta;

        SqlParameter[] parametro;

        public frmResumenCategoriaAlmuerzos()
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

        //FUNCION PARA LLENAR EL COMBOBOX DE ALMUERZOS
        private void llenarComboAlmuerzos()
        {
            try
            {
                sSql = "";
                sSql += "select P.id_producto, NP.nombre " + Environment.NewLine;
                sSql += "from cv401_productos P INNER JOIN" + Environment.NewLine;
                sSql += "cv401_nombre_productos NP ON P.id_producto = NP.id_producto" + Environment.NewLine;
                sSql += "and P.estado = @estado_1" + Environment.NewLine;
                sSql += "and NP.estado = @estado_2" + Environment.NewLine;
                sSql += "where maneja_almuerzos = @maneja_almuerzos" + Environment.NewLine;
                sSql += "and nivel = @nivel";

                #region PARAMETROS

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
                parametro[2].ParameterName = "@maneja_almuerzos";
                parametro[2].SqlDbType = SqlDbType.Int;
                parametro[2].Value = 1;

                parametro[3] = new SqlParameter();
                parametro[3].ParameterName = "@nivel";
                parametro[3].SqlDbType = SqlDbType.Int;
                parametro[3].Value = 2;

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
                row["id_producto"] = 0;
                row["nombre"] = "Seleccione...!!!";
                dtConsulta.Rows.InsertAt(row, 0);

                cmbAlmuerzos.ValueMember = "id_producto";
                cmbAlmuerzos.DisplayMember = "nombre";
                cmbAlmuerzos.DataSource = dtConsulta;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION DE LA FECHA DEL SISTEMA
        private void fechaSistema()
        {
            try
            {
                funciones = new Clases.ClaseFunciones();

                if (funciones.fechaSistema() == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = funciones.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                sFecha = funciones.sFechaRecuperada;
                return;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA LLENAR EL GRIDVIEW
        private void llenarGrid()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                dgvDatos.Rows.Clear();

                int iCantidad = 3;

                sSql = "";
                sSql += "select fecha_pedido, nombre, sum(cantidad) cantidad" + Environment.NewLine;
                sSql += "from pos_vw_cantidad_nombre_productos" + Environment.NewLine;
                sSql += "where id_producto_padre = @id_producto_padre" + Environment.NewLine;
                sSql += "and fecha_pedido between @fecha_desde" + Environment.NewLine;
                sSql += "and @fecha_hasta" + Environment.NewLine;

                if (iIdLocalidad != 0)
                {
                    iCantidad++;
                    sSql += "and id_localidad = @id_localidad" + Environment.NewLine;
                }

                sSql += "group by fecha_pedido, nombre" + Environment.NewLine;
                sSql += "order by fecha_pedido, nombre";

                #region PARAMETROS

                parametro = new SqlParameter[iCantidad];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_producto_padre";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = Convert.ToInt32(cmbAlmuerzos.SelectedValue);

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@fecha_desde";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = sFechaDesde;

                parametro[2] = new SqlParameter();
                parametro[2].ParameterName = "@fecha_hasta";
                parametro[2].SqlDbType = SqlDbType.VarChar;
                parametro[2].Value = sFechaHasta;

                if (iIdLocalidad != 0)
                {
                    parametro[3] = new SqlParameter();
                    parametro[3].ParameterName = "@id_localidad";
                    parametro[3].SqlDbType = SqlDbType.Int;
                    parametro[3].Value = iIdLocalidad;
                }                

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
                    return;
                }

                if (dtConsulta.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "No existen registros con los parámetros seleccionados.";
                    ok.ShowDialog();
                    return;
                }

                btnExportar.Visible = true;

                DateTime dtFechaRango = Convert.ToDateTime(dtConsulta.Rows[0]["fecha_pedido"].ToString());
                DateTime dtFechaComparar;

                dgvDatos.Rows.Add(dtFechaRango.ToString("dd-MM-yyyy"), "");

                int iFila = 0;
                dgvDatos.Rows[iFila].DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 192);
                iFila++;

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    dtFechaComparar = Convert.ToDateTime(dtConsulta.Rows[i]["fecha_pedido"].ToString());

                    if (dtFechaRango == dtFechaComparar)
                    {
                        dgvDatos.Rows.Add(dtConsulta.Rows[i]["nombre"].ToString(),
                                          dtConsulta.Rows[i]["cantidad"].ToString());
                    }

                    iFila++;

                    if (i + 1 != dtConsulta.Rows.Count)
                    {
                        dtFechaComparar = Convert.ToDateTime(dtConsulta.Rows[i + 1]["fecha_pedido"].ToString());

                        if (dtFechaRango != dtFechaComparar)
                        {
                            dtFechaRango = Convert.ToDateTime(dtConsulta.Rows[i + 1]["fecha_pedido"].ToString());
                            dgvDatos.Rows.Add("", "");
                            iFila++;
                            dgvDatos.Rows.Add(dtFechaRango.ToString("dd-MM-yyyy"), "");
                            dgvDatos.Rows[iFila].DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 192);
                            iFila++;
                        }
                    }
                }

                dgvDatos.ClearSelection();
                this.Cursor = Cursors.Default;
            }

            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA CREAR EL GRID PARA EXPORTAR
        private bool exportarGridExcel()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                dgvExportar = new DataGridView();
                dgvExportar.AllowUserToAddRows = false;
                dgvExportar.AllowUserToDeleteRows = false;
                dgvExportar.AllowUserToResizeColumns = false;
                dgvExportar.AllowUserToResizeRows = false;
                dgvExportar.MultiSelect = false;

                DataGridViewTextBoxColumn nombre = new DataGridViewTextBoxColumn();
                nombre.HeaderText = "PRODUCTO";
                nombre.Name = "mesero";

                DataGridViewTextBoxColumn cantidad = new DataGridViewTextBoxColumn();
                cantidad.HeaderText = "CANTIDAD";
                cantidad.Name = "ventas";

                dgvExportar.Columns.Add(nombre);
                dgvExportar.Columns.Add(cantidad);

                for (int i = 0; i < dgvDatos.Rows.Count; i++)
                {
                    dgvExportar.Rows.Add(
                                            dgvDatos.Rows[i].Cells["nombre"].Value.ToString(),
                                            dgvDatos.Rows[i].Cells["cantidad"].Value.ToString()
                                        );
                }

                exportarExcel = new Clases.ClaseExcel();

                if (exportarExcel.exportarExcelTexto(dgvExportar) == false)
                {
                    this.Cursor = Cursors.Default;
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Ocurrió un problema al exportar la información. Comuníquese con el administrador del sistema.";
                    ok.ShowDialog();
                    return false;
                }

                this.Cursor = Cursors.Default;

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

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            fechaSistema();
            llenarComboLocalidades();
            llenarComboAlmuerzos();
            dtFechaDesde.Text = sFecha;
            dtFechaHasta.Text = sFecha;
            btnExportar.Visible = false;
            dgvDatos.Rows.Clear();
        }

        #endregion

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

        private void frmResumenCategoriaAlmuerzos_Load(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.Rows.Count == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "No existe información para exportar.";
                ok.ShowDialog();
                return;
            }

            exportarGridExcel();
        }
    }
}
