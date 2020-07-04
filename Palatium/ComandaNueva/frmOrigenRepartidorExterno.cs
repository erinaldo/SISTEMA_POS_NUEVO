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

namespace Palatium.ComandaNueva
{
    public partial class frmOrigenRepartidorExterno : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        Clases.ClaseLimpiarArreglos limpiarArreglos = new Clases.ClaseLimpiarArreglos();

        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoOk ok;
        VentanasMensajes.frmMensajeNuevoSiNo SiNo;

        string sSql;
        string sDescripcionComanda;

        DataTable dtConsulta;

        bool bRespuesta;

        SqlParameter[] parametro;

        int iIdOrigenOrden;

        public frmOrigenRepartidorExterno()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        private void llenarArregloMaximo()
        {
            Program.iIDMESA = 0;

            Program.sDatosMaximo[0] = Program.sNombreUsuario;
            Program.sDatosMaximo[1] = Environment.MachineName.ToString();
            Program.sDatosMaximo[2] = "A";
        }

        //CONSULTA ÀRA HABILITAR LAS OPCIONES
        private bool consultarDatos(string sCodigo_P, string sTipoComanda_P)
        {
            try
            {
                limpiarArreglos.limpiarArregloComentarios();

                sSql = "";
                sSql += "select id_pos_origen_orden, descripcion" + Environment.NewLine;
                sSql += "from pos_origen_orden" + Environment.NewLine;
                sSql += "where codigo = @codigo" + Environment.NewLine;
                sSql += "and estado = @estado";

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@codigo";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = sCodigo_P;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@estado";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = "A";

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
                    ok.lblMensaje.Text = "No se encuentra configurado el tipo de comanda: " + sTipoComanda_P;
                    ok.ShowDialog();
                    return false;
                }

                iIdOrigenOrden = Convert.ToInt32(dtConsulta.Rows[0]["id_pos_origen_orden"].ToString());
                sDescripcionComanda = dtConsulta.Rows[0]["descripcion"].ToString().Trim().ToUpper();
                Program.iIdOrigenOrden = Convert.ToInt32(dtConsulta.Rows[0]["id_pos_origen_orden"].ToString());
                Program.sDescripcionOrigenOrden = dtConsulta.Rows[0]["descripcion"].ToString();

                ComandaNueva.frmComanda or = new ComandaNueva.frmComanda(iIdOrigenOrden, sDescripcionComanda, 0, 0, 0, "", Program.CAJERO_ID, Program.iIdMesero, Program.nombreMesero, "NINGUNA", 0, 0, Program.iIdPersona);
                or.ShowDialog();
                this.Close();

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

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmOrigenRepartidorExterno_Load(object sender, EventArgs e)
        {
            if (Program.iComandaUberEats == 1)
                btnUberEats.Enabled = true;
            else
                btnUberEats.Enabled = false;

            if (Program.iComandaGlovo == 1)
                btnGlovo.Enabled = true;
            else
                btnGlovo.Enabled = false;

            if (Program.iComandaRappi == 1)
                btnRappi.Enabled = true;
            else
                btnRappi.Enabled = false;
        }

        private void frmOrigenRepartidorExterno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void btnUberEats_Click(object sender, EventArgs e)
        {
            llenarArregloMaximo();

            Program.sIDPERSONA = null;
            consultarDatos("16", btnUberEats.Text.Trim());
        }

        private void btnGlovo_Click(object sender, EventArgs e)
        {
            llenarArregloMaximo();

            Program.sIDPERSONA = null;
            consultarDatos("17", btnGlovo.Text.Trim());
        }

        private void btnRappi_Click(object sender, EventArgs e)
        {
            llenarArregloMaximo();

            Program.sIDPERSONA = null;
            consultarDatos("18", btnRappi.Text.Trim());            
        }
    }
}
