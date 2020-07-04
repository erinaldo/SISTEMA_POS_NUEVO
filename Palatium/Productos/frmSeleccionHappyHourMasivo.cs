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
    public partial class frmSeleccionHappyHourMasivo : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeNuevoOk ok;
        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoSiNo SiNo;

        Clases.ClaseFunciones funciones;

        string sSql;
        string sFecha;

        DataTable dtConsulta;
        DataTable dtCategorias;
        DataTable dtSubCategorias;

        bool bRespuesta;

        int iSubcategoria_P;
        int iModificador_P;
        int iIdProductoPadre;

        Image img;

        SqlParameter[] parametro;

        public frmSeleccionHappyHourMasivo()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

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
            }
        }

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid()
        {
            try
            {
                dgvDatos.Rows.Clear();

                sSql = "";
                sSql += "select P.id_producto, P.codigo, NP.nombre, isnull(P.maneja_happy_hour, 0) maneja_happy_hour" + Environment.NewLine;
                sSql += "from cv401_productos P INNER JOIN" + Environment.NewLine;
                sSql += "cv401_nombre_productos NP ON P.id_producto = NP.id_producto" + Environment.NewLine;
                sSql += "and P.estado = @estado_1" + Environment.NewLine;
                sSql += "and NP.estado = @estado_2" + Environment.NewLine;
                sSql += "where P.id_producto_padre = @id_producto_padre" + Environment.NewLine;
                sSql += "order by NP.nombre";

                #region PARAMETROS

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
                parametro[2].ParameterName = "@id_producto_padre";
                parametro[2].SqlDbType = SqlDbType.Int;
                parametro[2].Value = iIdProductoPadre;

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

                if (dtConsulta.Rows.Count == 0)
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "No existen registros con los parámetros ingresados";
                    ok.ShowDialog();
                    return;
                }

                bool bAsignar;

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    int iManejaHH = Convert.ToInt32(dtConsulta.Rows[i]["maneja_happy_hour"].ToString());

                    if (iManejaHH == 1)
                        bAsignar = true;
                    else
                        bAsignar = false;

                    dgvDatos.Rows.Add(dtConsulta.Rows[i]["id_producto"].ToString(),
                                      dtConsulta.Rows[i]["maneja_happy_hour"].ToString(),
                                      false,
                                      dtConsulta.Rows[i]["codigo"].ToString(),
                                      dtConsulta.Rows[i]["nombre"].ToString(),
                                      bAsignar);
                }
                                
                btnBuscar.Visible = false;
                cmbCategorias.Enabled = false;
                cmbSubCategorias.Enabled = false;
                btnGrabar.Visible = true;

                dgvDatos.ClearSelection();
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            cmbCategorias.SelectedIndexChanged -= new EventHandler(cmbCategorias_SelectedIndexChanged);
            llenarComboCategorias();
            llenarComboSubCategorias(0);
            cmbCategorias.SelectedIndexChanged += new EventHandler(cmbCategorias_SelectedIndexChanged);

            iIdProductoPadre = 0;
            iSubcategoria_P = 0;
            iModificador_P = 0;

            btnBuscar.Visible = true;
            cmbCategorias.Enabled = true;
            cmbSubCategorias.Enabled = true;
            btnGrabar.Visible = false;

            dgvDatos.Rows.Clear();
        }

        //FUNCION PARA ACTUALIZAR LOS REGISTROS
        private void actualizarRegistros()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    this.Cursor = Cursors.Default;
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Error al inicar la transacción.";
                    ok.ShowDialog();
                    return;
                }

                int iIdProducto_P;
                int iManejaHappyHour_P;
                bool bEstado_P;
                bool bSeleccion_P;

                for (int i = 0; i < dgvDatos.Rows.Count; i++)
                {
                    bEstado_P = Convert.ToBoolean(dgvDatos.Rows[i].Cells["actualizar"].Value);

                    if (bEstado_P == true)
                    {
                        iIdProducto_P = Convert.ToInt32(dgvDatos.Rows[i].Cells["id_producto"].Value);
                        bSeleccion_P = Convert.ToBoolean(dgvDatos.Rows[i].Cells["seleccion"].Value);

                        if (bSeleccion_P == true)
                            iManejaHappyHour_P = 1;
                        else
                            iManejaHappyHour_P = 0;

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
                        parametro[0].Value = iManejaHappyHour_P;

                        parametro[1] = new SqlParameter();
                        parametro[1].ParameterName = "@id_producto";
                        parametro[1].SqlDbType = SqlDbType.Int;
                        parametro[1].Value = iIdProducto_P;

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
                    }
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);

                this.Cursor = Cursors.Default;
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Los registros se han actualizado con éxito.";
                ok.ShowDialog();

                limpiar();
                return;
            }

            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
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

        private void cmbCategorias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbCategorias.SelectedValue) == 0)
            {
                cmbSubCategorias.Enabled = false;
                llenarComboSubCategorias(0);
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
                    llenarComboSubCategorias(0);
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbCategorias.SelectedValue) == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor seleccione la categoría para extraer la información.";
                ok.ShowDialog();
                return;
            }

            if (iSubcategoria_P == 0)
            {
                iIdProductoPadre = Convert.ToInt32(cmbCategorias.SelectedValue);
            }

            else
            {
                if (Convert.ToInt32(cmbSubCategorias.SelectedValue) == 0)
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Favor seleccione la subcategoría para extraer la información.";
                    ok.ShowDialog();
                    return;
                }

                iIdProductoPadre = Convert.ToInt32(cmbSubCategorias.SelectedValue);
            }

            llenarGrid();
        }

        private void frmSeleccionHappyHourMasivo_Load(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            int iBandera_P = 0;

            for (int i = 0; i < dgvDatos.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgvDatos.Rows[i].Cells["actualizar"].Value) == true)
                {
                    iBandera_P = 1;
                    break;
                }
            }

            if (iBandera_P == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "No hay modificaciones en los registros para actualizar.";
                ok.ShowDialog();
                return;
            }

            SiNo = new VentanasMensajes.frmMensajeNuevoSiNo();
            SiNo.lblMensaje.Text = "¿Está seguro que desea actualizar los registros modificados?";
            SiNo.ShowDialog();

            if (SiNo.DialogResult == DialogResult.OK)
                actualizarRegistros();
        }

        private void dgvDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int iIndice = e.RowIndex;

                if ((dgvDatos.Columns[e.ColumnIndex].Name == "seleccion") && (dgvDatos.CurrentCell is DataGridViewCheckBoxCell))
                {
                    int iManejaHBDD = Convert.ToInt32(dgvDatos.Rows[iIndice].Cells["maneja_happy_hour"].Value);
                    bool bSeleccion = Convert.ToBoolean(dgvDatos.Rows[iIndice].Cells["seleccion"].Value);
                    int iValorValidacion = 0;

                    if (bSeleccion == true)
                        iValorValidacion = 1;

                    if (iManejaHBDD == iValorValidacion)
                        dgvDatos.Rows[iIndice].Cells["actualizar"].Value = false;
                    else
                        dgvDatos.Rows[iIndice].Cells["actualizar"].Value = true;

                    dgvDatos.EndEdit(); 
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

        private void dgvDatos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dgvDatos.CommitEdit(DataGridViewDataErrorContexts.Commit); 
        }

        private void dgvDatos_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvDatos.CurrentCell is DataGridViewCheckBoxCell)
            {
                dgvDatos.CommitEdit(DataGridViewDataErrorContexts.Commit);
            } 
        }
    }
}
