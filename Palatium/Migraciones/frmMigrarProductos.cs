using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Palatium.Migraciones
{
    public partial class frmMigrarProductos : Form
    {
        string sConexion;

        OleDbCommand cmd;

        int iEliminarColumnas;

        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoOk ok;

        public frmMigrarProductos()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        DataView importar(string sNombreArchivo_P)
        {
            try
            {
                sConexion = string.Format("Provider = Microsoft.ACE.OLEDB.12.0; Data Source = {0}; Extended Properties = 'Excel 12.0;'", sNombreArchivo_P);
                OleDbConnection conectar = new OleDbConnection(sConexion);
                conectar.Open();
                
                if (rdbPalatium.Checked == true)
                    cmd = new OleDbCommand("select * from [FORMATO_1$]", conectar);
                if (rdbPractisis.Checked == true)
                    cmd = new OleDbCommand("select * from [FORMATO_2$]", conectar);

                OleDbDataAdapter adapter = new OleDbDataAdapter()
                {
                    SelectCommand = cmd
                };

                DataSet ds = new DataSet();
                adapter.Fill(ds);
                conectar.Close();

                return ds.Tables[0].DefaultView;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return null;
            }
        }

        //FUNCION PARA CREAR LAS COLUMNAS DEL DATAGRIDVIEW 
        private bool crearColumnasGrid()
        {
            try
            {
                dgvDatos.Columns.Add("colFamilia", "FAMILIA");
                dgvDatos.Columns.Add("colSubFamilia", "SUBFAMILIA");
                dgvDatos.Columns.Add("colCodigoProducto", "CODIGO PRODUCTO");
                dgvDatos.Columns.Add("colDescripcion", "DESCRIPCION");
                dgvDatos.Columns.Add("colStockMin", "STOCK MINIMO");
                dgvDatos.Columns.Add("colPrecioCompra", "PRECIO COMPRA");
                dgvDatos.Columns.Add("colPVP", "PVP");
                dgvDatos.Columns.Add("colUnidades", "UNIDADES");
                dgvDatos.Columns.Add("colCodigoBarra", "CODIGO DE BARRAS");
                dgvDatos.Columns.Add("colPorcentajeIVA", "% IVA");
                dgvDatos.Columns.Add("colPorcentajeServicio", "% SERVICIO");

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

        //FUNCION PARA LEER EL ARCHIVO CSV
        private bool leerArchivoCSV(string sNombreArchivo_P)
        {
            try
            {
                iEliminarColumnas = 1;

                //if (crearColumnasGrid() == false)
                //    return false;

                StreamReader reader = new StreamReader(sNombreArchivo_P);
                string sContenido = null;
                string[] sContenidoTemp = null;

                sContenido = reader.ReadLine();

                //CREAR ENCABEZADO
                sContenidoTemp = sContenido.Split(';');

                for (int i = 0; i < sContenidoTemp.Length; i++)
                {
                    dgvDatos.Columns.Add("columna_" + i.ToString(), sContenidoTemp[i]);
                }

                sContenido = reader.ReadLine();

                while (sContenido != null)
                {
                    sContenidoTemp = sContenido.Split(';');
                    dgvDatos.Rows.Add(sContenidoTemp);
                    sContenido = reader.ReadLine();
                }

                reader.Close();

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
            if (iEliminarColumnas == 1)
            {
                dgvDatos.Rows.Clear();

                for (int i = dgvDatos.Columns.Count - 1; i >= 0; i--)
                {
                    dgvDatos.Columns.RemoveAt(i);
                }
            }

            iEliminarColumnas = 0;
            txtRuta.Clear();
            DataTable dt = new DataTable();
            dt.Clear();
            dgvDatos.DataSource = dt;
            dgvDatos.DataSource = null;
            rdbPalatium.Checked = true;
            rdbExcel.Checked = true;
            rdbExcel.Enabled = true;
            rdbCSV.Enabled = true;
        }

        #endregion

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog abrir = new OpenFileDialog();
                abrir.Title = "Seleccione el archivo";

                if (rdbExcel.Checked == true)
                    abrir.Filter = "Excel | *.xls;*.xlsx";
                else if (rdbCSV.Checked == true)
                    abrir.Filter = "Archivo CSV | *.csv";

                if (abrir.ShowDialog() == DialogResult.OK)
                {
                    rdbExcel.Enabled = false;
                    rdbCSV.Enabled = false;

                    txtRuta.Text = abrir.FileName;

                    if (rdbExcel.Checked == true)
                        dgvDatos.DataSource = importar(txtRuta.Text.Trim());
                    else if (rdbCSV.Checked == true)
                        leerArchivoCSV(txtRuta.Text.Trim());
                }
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            if (txtRuta.Text.Trim() == "")
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor seleccione el archivo.";
                ok.ShowDialog();
                return;
            }

            dgvDatos.DataSource = importar(txtRuta.Text.Trim());
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void rdbExcel_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbExcel.Checked == true)
                pnlFormatos.Enabled = true;
        }

        private void rdbCSV_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCSV.Checked == true)
                pnlFormatos.Enabled = false;
        }
    }
}
