using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.Productos
{
    public partial class frmModificacionIconosProductos : Form
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

        public frmModificacionIconosProductos()
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
                sSql += "select P.id_producto, P.codigo, NP.nombre, isnull(P.imagen_categoria, '') imagen_categoria" + Environment.NewLine;
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

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    //if (dtConsulta.Rows[i]["imagen_categoria"].ToString().Trim() == "")
                    //    img = null;
                    //else
                    //{
                    //    byte[] imageBytes;

                    //    imageBytes = Convert.FromBase64String(dtConsulta.Rows[i]["imagen_categoria"].ToString().Trim());

                    //    using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                    //    {
                    //        img = Image.FromStream(ms, true);
                    //    }
                    //}

                    dgvDatos.Rows.Add(dtConsulta.Rows[i]["id_producto"].ToString(),
                                      dtConsulta.Rows[i]["imagen_categoria"].ToString(),
                                      false,
                                      dtConsulta.Rows[i]["codigo"].ToString(),
                                      dtConsulta.Rows[i]["nombre"].ToString());
                }

                for (int i = 0; i < dgvDatos.Rows.Count; i++)
                {
                    if (dgvDatos.Rows[i].Cells["imagen_categoria"].Value.ToString().Trim() != "")
                    {
                        byte[] imageBytes;

                        imageBytes = Convert.FromBase64String(dgvDatos.Rows[i].Cells["imagen_categoria"].Value.ToString().Trim());

                        using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                        {
                            img = Image.FromStream(ms, true);
                        }

                        dgvDatos.Rows[i].Cells["imagen"].Value = img;
                    }
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

        private string ConvertirImagenToBase64(Image file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.Save(memoryStream, file.RawFormat);
                byte[] imageBytes = memoryStream.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
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
                bool bEstado_P;
                String sCodigoBase64;

                for (int i = 0; i < dgvDatos.Rows.Count; i++)
                {
                    bEstado_P = Convert.ToBoolean(dgvDatos.Rows[i].Cells["actualizar"].Value);

                    if (bEstado_P == true)
                    {
                        iIdProducto_P = Convert.ToInt32(dgvDatos.Rows[i].Cells["id_producto"].Value);
                        sCodigoBase64 = dgvDatos.Rows[i].Cells["imagen_categoria"].Value.ToString().Trim();

                        sSql = "";
                        sSql += "update cv401_productos set" + Environment.NewLine;
                        sSql += "imagen_categoria = @imagen_categoria" + Environment.NewLine;
                        sSql += "where id_producto = @id_producto" + Environment.NewLine;
                        sSql += "and estado = @estado";

                        #region PARAMETROS

                        parametro = new SqlParameter[3];
                        parametro[0] = new SqlParameter();
                        parametro[0].ParameterName = "@imagen_categoria";
                        parametro[0].SqlDbType = SqlDbType.VarChar;
                        parametro[0].Value = sCodigoBase64;

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

        private void frmModificacionIconosProductos_Load(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void dgvDatos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int iIndice = e.RowIndex;

                if (dgvDatos.Columns[e.ColumnIndex].Name == "buscar")
                {
                    OpenFileDialog abrir = new OpenFileDialog();
                    abrir.Filter = "Archivos imagen (*.jpg; *.png; *.jpeg)|*.jpg;*.png;*.jpeg";
                    abrir.Title = "Seleccionar archivo";

                    if (abrir.ShowDialog() == DialogResult.OK)
                    {
                        img = null;
                        string sRuta_P = abrir.FileName;
                        img = Image.FromFile(sRuta_P);
                        String sBase64_P = ConvertirImagenToBase64(img);

                        if (sBase64_P.Trim().Length > 8000)
                        {
                            ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                            ok.lblMensaje.Text = "La imagen supera el tamaño permitido.";
                            ok.ShowDialog();
                            abrir.Dispose();
                            return;
                        }

                        dgvDatos.Rows[iIndice].Cells["actualizar"].Value = true;
                        dgvDatos.Rows[iIndice].Cells["imagen_categoria"].Value = sBase64_P;
                        dgvDatos.Rows[iIndice].Cells["imagen"].Value = img;
                    }

                    abrir.Dispose();
                }

                else if (dgvDatos.Columns[e.ColumnIndex].Name == "remover")
                {
                    if (dgvDatos.Rows[iIndice].Cells["imagen_categoria"].Value.ToString().Trim() == "")
                    {
                        ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                        ok.lblMensaje.Text = "El registro no contiene una imagen a ser removida.";
                        ok.ShowDialog();
                        return;
                    }

                    dgvDatos.Rows[iIndice].Cells["actualizar"].Value = true;
                    dgvDatos.Rows[iIndice].Cells["imagen_categoria"].Value = "";
                    dgvDatos.Rows[iIndice].Cells["imagen"].Value = null;
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
    }
}
