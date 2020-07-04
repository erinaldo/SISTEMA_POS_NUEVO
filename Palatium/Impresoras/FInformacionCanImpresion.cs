using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Net;
using System.Security.Util;
using ConexionBD;
using System.Drawing.Printing;

namespace Palatium.Impresoras
{
    public partial class FInformacionCanImpresion : Form
    {
        //VARIABLES, INSTANCIAS
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        string sSql;
        string sEstado;
        bool bRespuesta = false;
        DataTable dtConsulta;
        string sFecha;
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();

        int iAbrirCajon;
        int iCortarPapel;

        public FInformacionCanImpresion()
        {
            InitializeComponent();
        }

        private void FInformacionCanImpresion_Load(object sender, EventArgs e)
        {
            terminalesCombo();
        }

        #region FUNCIONES DEL USUARIO

        //LLENAR COMBOBOX DE TERMINALES
        private void terminalesCombo()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_terminal, codigo + ' | ' + descripcion DESCRIPCION" + Environment.NewLine;
                sSql += "from pos_terminal where estado = 'A'";

                cmbTerminales.llenar(sSql);
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //LLENAR COMBOBOX DE IMPRESORAS
        private void impresorasCombo()
        {
            String pkInstalledPrinters;
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
                cmbImpresoras.Items.Add(pkInstalledPrinters);
            }
        }

        //Función para llenar el Combo de Localidad
        private void llenarComboLocalidad()
        {
            try
            {
                sSql = "select id_localidad, nombre_localidad from tp_vw_localidades";
                cmbLocalidad.llenar(sSql);
                cmbLocalidad.SelectedValue = Program.iIdLocalidad;
            }
            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }


        //LIMPIAR
        private void limpiarTodo()
        {
            txtBuscarCanImpre.Clear();
            txtCodigoCanImpre.Enabled = true;
            txtCodigoCanImpre.Clear();
            txtDescripCanImpre.Clear();
            TxtCantidadImpresiones.Clear();
            TxtPuertoImpresora.Clear();
            TxtIPAsignada.Clear();
            cmbEstadoCanImpre.Text = "ACTIVO";

            llenarGrid(1);
            llenarComboLocalidad();
            terminalesCombo();
        }


        //LIMPIAR SOLO LAS CAJAS DE TEXTO
        private void limpiarCajasTexto()
        {
            txtCodigoCanImpre.Enabled = true;
            txtCodigoCanImpre.Clear();
            txtDescripCanImpre.Clear();
            TxtCantidadImpresiones.Clear();
            TxtPuertoImpresora.Clear();
            TxtIPAsignada.Clear();
            cmbEstadoCanImpre.Text = "ACTIVO";
            llenarComboLocalidad();
        }

        //LIMPIAR SOLO LAS CAJAS DE TEXTO
        private void limpiarGuardar()
        {
            txtCodigoCanImpre.Enabled = true;
            txtCodigoCanImpre.Clear();
            txtDescripCanImpre.Clear();
            TxtCantidadImpresiones.Clear();
            TxtPuertoImpresora.Clear();
            TxtIPAsignada.Clear();
            cmbEstadoCanImpre.Text = "ACTIVO";

            llenarGrid(1);
        }

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid(int op)
        {
            try
            {
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                sSql = "";
                sSql += "select id_pos_canal_impresion, codigo as CODIGO, descripcion as DESCRIPCION," + Environment.NewLine;
                sSql += "numero_impresion as 'No. IMPRESIONES', isnull(nombre_impresora, 'NINGUNA') as 'NOMBRE DE IMPRESORA'," + Environment.NewLine;
                sSql += "isnull(puerto_impresora, 'NINGUNA') as PUERTO, isnull(ip_impresora, 'NINGUNA') as 'IP'," + Environment.NewLine;
                sSql += "estado as ESTADO, id_localidad as LOCALIDAD, cortar_papel, abrir_cajon" + Environment.NewLine;
                sSql += "from pos_canal_impresion" + Environment.NewLine;
                sSql += "where estado = 'A'" + Environment.NewLine;

                if (op == 1)
                {
                    sSql += "and id_pos_terminal = " + Convert.ToInt32(cmbTerminales.SelectedValue) + Environment.NewLine;
                }

                sSql += "order by id_pos_canal_impresion";

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);
                
                if (bRespuesta == false)
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                }

                else
                {
                    dgvCanImpre.DataSource = dtConsulta;
                    dgvCanImpre.Refresh();
                    dgvCanImpre.Columns[1].Width = 60;
                    dgvCanImpre.Columns[2].Width = 150;
                    dgvCanImpre.Columns[3].Width = 100;
                    dgvCanImpre.Columns[4].Width = 150;
                    dgvCanImpre.Columns[5].Width = 100;
                    dgvCanImpre.Columns[6].Width = 100;
                    dgvCanImpre.Columns[7].Width = 75;
                    dgvCanImpre.Columns[0].Visible = false;
                    dgvCanImpre.Columns[8].Visible = false;
                    dgvCanImpre.Columns[9].Visible = false;
                    dgvCanImpre.Columns[10].Visible = false;
                }
                
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        #endregion

        private void Txt_Codigo_LostFocus(object sender, EventArgs e)
        {
            dtConsulta = new DataTable();
            dtConsulta.Clear();
            sSql = "select descripcion, estado from pos_canal_impresion " +
                   "where codigo = '" + txtCodigoCanImpre.Text + 
                   "' and id_pos_terminal = " + Convert.ToInt32(cmbTerminales.SelectedValue);

            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

            if (bRespuesta == false)
            {
                catchMensaje.LblMensaje.Text = sSql;
                catchMensaje.ShowDialog();
            }
            else
            {
                if (dtConsulta.Rows.Count > 0) //contar cuantos registros me devuelve el datatable
                {
                    txtDescripCanImpre.Text = dtConsulta.Rows[0].ItemArray[0].ToString();
                    if (dtConsulta.Rows[0].ItemArray[1].ToString() == "A")
                    {
                        cmbEstadoCanImpre.Text = "ACTIVO";
                    }
                    else
                    {
                        cmbEstadoCanImpre.Text = "ELIMINADO";
                    }
                    btnAnularCanImpre.Enabled = true;
                    btnNuevoCanImpre.Text = "Actualizar";
                    txtCodigoCanImpre.Enabled = false;
                    txtBuscarCanImpre.Focus();
                }
                else
                {
                    txtDescripCanImpre.Focus();
                    btnNuevoCanImpre.Text = "Guardar";
                    btnAnularCanImpre.Enabled = false;
                }
            }
         

        }

        private void Txt_CodigoCanImpre_Leave(object sender, EventArgs e)
        {
            txtCodigoCanImpre.LostFocus += new EventHandler(Txt_Codigo_LostFocus);
        }

        private void Btn_CerrarCanImpre_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btn_LimpiarCanImpre_Click(object sender, EventArgs e)
        {
            Grb_DatoCanImpre.Enabled = false;
            btnNuevoCanImpre.Text = "Nuevo";
            limpiarCajasTexto();
            limpiarTodo();
        }

        private void Btn_BuscarCanImpre_Click(object sender, EventArgs e)
        {
            try
            {
                limpiarCajasTexto();
                Grb_DatoCanImpre.Enabled = false;

                llenarGrid(1);
            }

            catch (Exception)
            {
                ok.LblMensaje.Text = "Error al general la consulta.";
                ok.ShowDialog();
                Grb_DatoCanImpre.Enabled = false;
                btnNuevoCanImpre.Text = "Nuevo";
                limpiarTodo();
            }
        }

        private void BtnNuevoCanImpre_Click(object sender, EventArgs e)
        {
            if (chkAbrirCajon.Checked == true)
            {
                iAbrirCajon = 1;
            }

            else
            {
                iAbrirCajon = 0;
            }


            if (chkCortarPapel.Checked == true)
            {
                iCortarPapel = 1;
            }

            else
            {
                iCortarPapel = 0;
            }
            
            //SI EL BOTON ESTA EN OPCION NUEVO
            if (btnNuevoCanImpre.Text == "Nuevo")
            {
                limpiarCajasTexto();
                Grb_DatoCanImpre.Enabled = true;
                btnNuevoCanImpre.Text = "Guardar";
                txtCodigoCanImpre.Focus();
                goto fin;
            }

            //SI EL BOTON ESTA EN OPCION GUARDAR
            else if (btnNuevoCanImpre.Text == "Guardar")
            {

                if ((txtCodigoCanImpre.Text == "") && (txtDescripCanImpre.Text == ""))
                {
                    ok.LblMensaje.Text = "Debe rellenar todos los campos obligatorios.";
                    ok.ShowDialog();
                    txtCodigoCanImpre.Focus();
                }

                else if (txtCodigoCanImpre.Text == "")
                {
                    ok.LblMensaje.Text = "Favor ingrese el código del canal de impresión.";
                    ok.ShowDialog();
                    txtCodigoCanImpre.Focus();
                }

                else if (txtDescripCanImpre.Text == "")
                {
                    ok.LblMensaje.Text = "Favor ingrese la descripción del canal de impresión.";
                    ok.ShowDialog();
                    txtDescripCanImpre.Focus();
                }

                else if (TxtCantidadImpresiones.Text == "")
                {
                    ok.LblMensaje.Text = "Favor ingrese la cantidad de impresiones que desea obtener.";
                    ok.ShowDialog();
                    txtDescripCanImpre.Focus();
                }

                else
                {
                    sFecha = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    //llamo a la funcion que iniciara un begin transAction (se graba en una tabla temporal) y Program.G_INICIA_TRANSACCION devuelve true si abrio bn la transaccion
                    if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                    {
                        ok.LblMensaje.Text = "Error al abrir transacción.";
                        ok.ShowDialog();
                        limpiarTodo();
                    }
                    else
                    {
                        //CONSULTAMOS SI EL CODIGO YA FUE INGRESADO
                        sSql = "";
                        sSql += "select * from pos_canal_impresion where codigo = '" + txtCodigoCanImpre.Text.Trim() + "'" + Environment.NewLine;
                        sSql += "and id_pos_terminal = " + Convert.ToInt32(cmbTerminales.SelectedValue) + " and estado = 'A'";
                        dtConsulta = new DataTable();
                        dtConsulta.Clear();

                        bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                        if (bRespuesta == true)
                        {
                            if (dtConsulta.Rows.Count == 0)
                            {
                                sSql = "";
                                sSql += "insert into pos_canal_impresion (" + Environment.NewLine;
                                sSql += "codigo, descripcion, numero_impresion, nombre_impresora," + Environment.NewLine;
                                sSql += "puerto_impresora, ip_impresora, id_pos_terminal," + Environment.NewLine;
                                sSql += "cortar_papel, abrir_cajon, estado, fecha_ingreso," + Environment.NewLine;
                                sSql += "usuario_ingreso, terminal_ingreso, id_localidad) " + Environment.NewLine;
                                sSql += "values(" + Environment.NewLine;
                                sSql += "'" + txtCodigoCanImpre.Text.Trim() + "', '" + txtDescripCanImpre.Text.Trim() + "'," + Environment.NewLine;
                                sSql += "'" + TxtCantidadImpresiones.Text.Trim() + "', '" + cmbImpresoras.Text.Trim() + "'," + Environment.NewLine;
                                sSql += "'" + TxtPuertoImpresora.Text.Trim() + "', '" + TxtIPAsignada.Text.Trim() + "'," + Environment.NewLine;
                                sSql += Convert.ToInt32(cmbTerminales.SelectedValue) + ", " + iCortarPapel + "," + Environment.NewLine;
                                sSql += iAbrirCajon + ", 'A', '" + sFecha + "', '" + Program.sNombreUsuario + "'," + Environment.NewLine;
                                sSql += "'" + Environment.MachineName.ToString() + "', " + cmbLocalidad.SelectedValue  + ")";

                                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                                {
                                    catchMensaje.LblMensaje.Text = sSql;
                                    catchMensaje.ShowDialog();
                                }

                                else
                                {
                                    //si no se ejecuta bien hara un commit
                                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                                    ok.LblMensaje.Text = "Registro ingresado correctamente.";
                                    ok.ShowDialog();
                                    btnNuevoCanImpre.Text = "Nuevo";
                                    Grb_DatoCanImpre.Enabled = false;
                                    limpiarGuardar();
                                    goto fin;
                                }

                            }

                            else
                            {
                                ok.LblMensaje.Text = "Ya existe un registro con el código ingresado. Favor verifique los datos.";
                                ok.ShowDialog();
                                txtCodigoCanImpre.Clear();
                                txtCodigoCanImpre.Focus();
                            }
                        }                        
                    }
                }
            }

            //SI EL BOTON ESTA EN OPCION ACTUALIZAR
            else if (btnNuevoCanImpre.Text == "Actualizar")
            {
                try
                {
                    if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                    {
                        ok.LblMensaje.Text = "Error al abrir transacción.";
                        ok.ShowDialog();
                        limpiarTodo();
                    }
                    else
                    {
                        sSql = "";
                        sSql += "update pos_canal_impresion set" + Environment.NewLine;
                        sSql += "codigo = '" + txtCodigoCanImpre.Text.Trim() + "'," + Environment.NewLine;
                        sSql += "descripcion = '" + txtDescripCanImpre.Text.Trim() + "'," + Environment.NewLine;
                        sSql += "numero_impresion = " + Convert.ToInt32(TxtCantidadImpresiones.Text.Trim()) + "," + Environment.NewLine;
                        sSql += "nombre_impresora = '" + cmbImpresoras.Text.Trim() + "'," + Environment.NewLine;
                        sSql += "puerto_impresora = '" + TxtPuertoImpresora.Text.Trim() + "'," + Environment.NewLine; 
                        sSql += "ip_impresora = '" + TxtIPAsignada.Text.Trim() + "'," + Environment.NewLine;
                        sSql += "id_localidad = " + cmbLocalidad.SelectedValue + "," + Environment.NewLine;
                        sSql += "id_pos_terminal = " + Convert.ToInt32(cmbTerminales.SelectedValue) + "," + Environment.NewLine;
                        sSql += "cortar_papel = " + iCortarPapel + "," + Environment.NewLine;
                        sSql += "abrir_cajon = " + iAbrirCajon + Environment.NewLine;
                        sSql += "where id_pos_canal_impresion = " + Convert.ToInt32(lblId.Text.Trim());

                        if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                        {
                            catchMensaje.LblMensaje.Text = sSql;
                            catchMensaje.ShowDialog();
                            goto reversa;
                        }
                        else
                        {
                            //si no se ejecuta bien hara un commit
                            conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                            ok.LblMensaje.Text = "Registro actualizado éxitosamente.";
                            ok.ShowDialog();
                            btnNuevoCanImpre.Text = "Nuevo";
                            Grb_DatoCanImpre.Enabled = false;
                            limpiarCajasTexto();
                            limpiarGuardar();
                            goto fin;
                        }

                    }
                }
                catch (Exception)
                {
                    goto reversa;
                }
            }

            reversa:
            {
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);                
                Grb_DatoCanImpre.Enabled = false;
                btnNuevoCanImpre.Text = "Nuevo";
                limpiarCajasTexto();
                limpiarTodo();
            }

            fin: { }

        }

        private void Btn_AnularCanImpre_Click(object sender, EventArgs e)
        {
            try
            {
                sFecha = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                if (MessageBox.Show("Esta seguro que desea dar de bajar el registro?", "Mensaje", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                    {
                        ok.LblMensaje.Text = "Error al abrir transacción.";
                        ok.ShowDialog();
                        limpiarTodo();
                    }
                    else
                    {
                        sSql = "";
                        sSql += "update pos_canal_impresion set" + Environment.NewLine;
                        sSql += "estado = 'E'," + Environment.NewLine;
                        sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                        sSql += "usuario_anula = '" + Program.sNombreUsuario + "'," + Environment.NewLine;
                        sSql += "terminal_anula = '" + Environment.MachineName.ToString() + "'" + Environment.NewLine;
                        sSql += "where codigo = '" + txtCodigoCanImpre.Text.ToString().Trim() + "'";

                        if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                        {
                            //hara el rolBAck
                            catchMensaje.LblMensaje.Text = sSql;
                            catchMensaje.ShowDialog();
                        }
                        else
                        {
                            //si no se ejecuta bien hara un commit
                            conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                            ok.LblMensaje.Text = "Registro eliminado éxitosamente.";
                            ok.ShowDialog();
                            btnNuevoCanImpre.Text = "Nuevo";
                            Grb_DatoCanImpre.Enabled = false;
                            limpiarTodo();
                        }

                    }
                }
                else
                {
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    ok.LblMensaje.Text = "Se canceló la eliminacion.";
                    ok.ShowDialog();
                    Grb_DatoCanImpre.Enabled = false;
                    btnNuevoCanImpre.Text = "Nuevo";
                    limpiarTodo();
                }
            }

            catch (Exception)
            {
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                ok.LblMensaje.Text = "Ocurrió un problema al modificar el registro.";
                ok.ShowDialog();
                Grb_DatoCanImpre.Enabled = false;
                btnNuevoCanImpre.Text = "Nuevo";
                limpiarTodo();
            }
        }

       

        private void CmbEstadoCanImpre_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEstadoCanImpre.Text.Trim().Equals("ACTIVO"))
            {
                sEstado = "A";
            }
            else if (cmbEstadoCanImpre.Text.Trim().Equals("ELIMINADO"))
            {
                sEstado = "E";
            }
        }

        //funcion para extraer todos los datos de la tabla a cada uno de los texBox, etc.
        private void Dgv_CanImpre_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Grb_DatoCanImpre.Enabled = true;
            txtCodigoCanImpre.Enabled = false;
            btnNuevoCanImpre.Text = "Actualizar";
            dgvCanImpre.Columns[0].Visible = true;
            dgvCanImpre.Columns[8].Visible = true;

            lblId.Text = dgvCanImpre.CurrentRow.Cells[0].Value.ToString();
            txtCodigoCanImpre.Text = dgvCanImpre.CurrentRow.Cells[1].Value.ToString();
            txtDescripCanImpre.Text = dgvCanImpre.CurrentRow.Cells[2].Value.ToString();
            TxtCantidadImpresiones.Text = dgvCanImpre.CurrentRow.Cells[3].Value.ToString();

            if (dgvCanImpre.CurrentRow.Cells[4].Value.ToString() != "NINGUNA")
            {
                cmbImpresoras.Text = dgvCanImpre.CurrentRow.Cells[4].Value.ToString();
            }
            else
            {
                cmbImpresoras.Text = "";
            }

            if (dgvCanImpre.CurrentRow.Cells[5].Value.ToString() != "NINGUNA")
            {
                TxtPuertoImpresora.Text = dgvCanImpre.CurrentRow.Cells[5].Value.ToString();
            }
            else
            {
                TxtPuertoImpresora.Text = "";
            }

            if (dgvCanImpre.CurrentRow.Cells[6].Value.ToString() != "NINGUNA")
            {
                TxtIPAsignada.Text = dgvCanImpre.CurrentRow.Cells[6].Value.ToString();
            }
            else
            {
                TxtIPAsignada.Text = "";
            }

            if (dgvCanImpre.CurrentRow.Cells[7].Value.ToString() == "A")
            {
                cmbEstadoCanImpre.Text = "ACTIVO";
            }
            else
            {
                cmbEstadoCanImpre.Text = "ELIMINADO";
            }

            cmbLocalidad.SelectedValue = dgvCanImpre.CurrentRow.Cells[8].Value.ToString();


            if (dgvCanImpre.CurrentRow.Cells[9].Value.ToString() == "1")
            {
                chkCortarPapel.Checked = true;
            }

            else
            {
                chkCortarPapel.Checked = false;
            }

            if (dgvCanImpre.CurrentRow.Cells[10].Value.ToString() == "1")
            {
                chkAbrirCajon.Checked = true;
            }

            else
            {
                chkAbrirCajon.Checked = false;
            }


            dgvCanImpre.Columns[0].Visible = false;
            dgvCanImpre.Columns[0].Visible = false;
            txtDescripCanImpre.Focus();
        }

        private void cmbTerminales_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTerminales.SelectedValue.ToString() == "0")
            {
                dtConsulta = new DataTable();
                dtConsulta.Clear();
                dgvCanImpre.DataSource = dtConsulta;
            }

            else
            {
                llenarGrid(1);
                cmbEstadoCanImpre.Text = "ACTIVO";
                llenarComboLocalidad();
                impresorasCombo();
            }
        }
    }
}
