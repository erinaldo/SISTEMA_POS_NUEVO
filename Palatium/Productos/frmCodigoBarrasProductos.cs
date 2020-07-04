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
    public partial class frmCodigoBarrasProductos : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeNuevoSiNo SiNo;
        VentanasMensajes.frmMensajeNuevoOk ok;
        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;

        Clases.ClaseFunciones funciones;
        Clases.ClaseValidarCaracteres caracter;

        string sSql;
        string sNombreProducto;

        DataTable dtConsulta;
        DataTable dtCategorias;
        DataTable dtSubCategorias;

        bool bRespuesta;

        int iSubcategoria_P;
        int iModificador_P;
        int iIdProducto;

        SqlParameter[] parametro;

        Byte[] Logo { get; set; }

        public frmCodigoBarrasProductos()
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
                return;
            }
        }

        //FUNCION PARA LLENAR EL COMBOBOX DE PRODUCTOS
        private void llenarComboProductos(int iIdProductoPadre_P, int iNivel_P)
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
                parametro[2].Value = iNivel_P;

                parametro[3] = new SqlParameter();
                parametro[3].ParameterName = "@id_producto_padre";
                parametro[3].SqlDbType = SqlDbType.Int;
                parametro[3].Value = iIdProductoPadre_P;

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
                row["id_producto"] = "0";
                row["nombre"] = "Seleccione...!!!";
                dtConsulta.Rows.InsertAt(row, 0);

                cmbProductos.DisplayMember = "nombre";
                cmbProductos.ValueMember = "id_producto";
                cmbProductos.DataSource = dtConsulta;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA CONSULTAR EL REGISTRO
        private void consultarRegistro()
        {
            try
            {
                sSql = "";
                sSql += "select isnull(codigo_barra, '') codigo_barra" + Environment.NewLine;
                sSql += "from cv401_productos" + Environment.NewLine;
                sSql += "where estado = @estado" + Environment.NewLine;
                sSql += "and id_producto = @id_producto";

                #region PARAMETROS

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@estado";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "A";

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@id_producto";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = iIdProducto;

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
                    ok.lblMensaje.Text = "No existe información del registro seleccionado";
                    ok.ShowDialog();
                    return;
                }

                grupoDatos.Enabled = true;
                btnGrabar.Visible = true;
                btnBuscar.Visible = false;
                cmbCategorias.Enabled = false;
                cmbSubCategorias.Enabled = false;
                cmbProductos.Enabled = false;
                btnVisualizar.Visible = true;

                txtCodigoBarras.Text = dtConsulta.Rows[0]["codigo_barra"].ToString();

                if (dtConsulta.Rows[0]["codigo_barra"].ToString().Trim() != "")
                {
                    if (dtConsulta.Rows[0]["codigo_barra"].ToString().Trim().Length == 13)
                    {
                        generarCodigoBarras();
                    }
                    else
                        imgCodigo.Image = null;
                }

                else
                    imgCodigo.Image = null;

                txtCodigoBarras.Focus();
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA ACTUALIZAR EL REGISTRO
        private void actualizarRegistro()
        {
            try
            {
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    this.Cursor = Cursors.Default;
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Error al inicar la transacción.";
                    ok.ShowDialog();
                    return;
                }

                sSql = "";
                sSql += "update cv401_productos set" + Environment.NewLine;
                sSql += "codigo_barra = @codigo_barra" + Environment.NewLine;
                sSql += "where id_producto = @id_producto" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                parametro = new SqlParameter[3];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@codigo_barra";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = txtCodigoBarras.Text.Trim();

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@id_producto";
                parametro[1].SqlDbType = SqlDbType.VarChar;
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

                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Registro actualizado éxitosamente.";
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

        //FUNCION PARA GENERAR EL CODIGO DE BARRAS
        private bool generarCodigoBarras()
        {
            try
            {
                funciones = new Clases.ClaseFunciones();

                Logo = funciones.codigoBarrasEAN13(txtCodigoBarras.Text.Trim());
                imgCodigo.Image = null;
                Image foto = null;

                using (var ms = new MemoryStream(Logo, 0, Logo.Length))
                {
                    foto = Image.FromStream(ms, true);
                }

                imgCodigo.Image = foto;
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
            cmbCategorias.SelectedIndexChanged -= new EventHandler(cmbCategorias_SelectedIndexChanged);
            cmbSubCategorias.SelectedIndexChanged -= new EventHandler(cmbSubCategorias_SelectedIndexChanged);
            llenarComboCategorias();
            llenarComboSubCategorias(0);
            llenarComboProductos(0, 3);
            cmbCategorias.SelectedIndexChanged += new EventHandler(cmbCategorias_SelectedIndexChanged);
            cmbSubCategorias.SelectedIndexChanged += new EventHandler(cmbSubCategorias_SelectedIndexChanged);

            iIdProducto = 0;
            sNombreProducto = "";
            txtCodigoBarras.Clear();
            imgCodigo.Image = null;
            grupoDatos.Enabled = false;
            btnGrabar.Visible = false;
            btnBuscar.Visible = true;
            btnVisualizar.Visible = false;
            cmbCategorias.Enabled = true;
            cmbSubCategorias.Enabled = true;
            cmbProductos.Enabled = true;
            grupoDatos.Text = "Producto";
            cmbCategorias.Focus();
        }

        #endregion

        private void frmCodigoBarrasProductos_Load(object sender, EventArgs e)
        {
            limpiar();
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

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbProductos.SelectedValue) == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor seleccione el producto para buscar en las comandas.";
                ok.ShowDialog();
                cmbProductos.Focus();
                return;
            }

            iIdProducto = Convert.ToInt32(cmbProductos.SelectedValue);
            sNombreProducto = cmbProductos.Text.Trim().ToUpper();

            consultarRegistro();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (txtCodigoBarras.Text.Trim() == "")
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor ingrese el código de barras";
                ok.ShowDialog();
                txtCodigoBarras.Focus();
                return;
            }

            if (txtCodigoBarras.Text.Trim().Length != 13)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "El código de barras debe contener 13 dígitos.";
                ok.ShowDialog();
                txtCodigoBarras.Clear();
                txtCodigoBarras.Focus();
                return;
            }

            SiNo = new VentanasMensajes.frmMensajeNuevoSiNo();
            SiNo.lblMensaje.Text = "¿Está seguro que desea actualizar el registro de " + sNombreProducto + "?";
            SiNo.ShowDialog();

            if (SiNo.DialogResult == DialogResult.OK)
                actualizarRegistro();
        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            if (txtCodigoBarras.Text.Trim() == "")
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor ingrese el código de barras";
                ok.ShowDialog();
                txtCodigoBarras.Focus();
                return;
            }

            if (txtCodigoBarras.Text.Trim().Length != 13)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "El código de barras debe contener 13 dígitos.";
                ok.ShowDialog();
                txtCodigoBarras.Clear();
                txtCodigoBarras.Focus();
                return;
            }

            generarCodigoBarras();
        }

        private void txtCodigoBarras_KeyPress(object sender, KeyPressEventArgs e)
        {
            caracter = new Clases.ClaseValidarCaracteres();
            caracter.soloNumeros(e);
        }
    }
}
