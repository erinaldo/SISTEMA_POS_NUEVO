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

namespace Palatium.Almuerzos
{
    public partial class frmComandaAlmuerzo : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoOk ok;

        Clases.ClaseFunciones funciones;

        Button[] botonSopas = new Button[3];
        Button[] botonSegundos = new Button[3];
        Button[] botonJugos = new Button[3];
        Button[] botonPostres = new Button[3];

        string sSql;
        string sFecha;

        bool bRespuesta;

        DataTable dtConsulta;
        DataTable dtSopas;
        DataTable dtAlmuerzos;
        DataTable dtJugos;
        DataTable dtPostres;

        SqlParameter[] parametro;

        int iIdCabecera;
        int iCuentaSopas;
        int iCuentaSegundos;
        int iCuentaJugos;
        int iCuentaPostres;
        int iCuentaAyudaSopas;
        int iCuentaAyudaSegundos;
        int iCuentaAyudaJugos;
        int iCuentaAyudaPostres;
        int iPosXSopas;
        int iPosXSegundos;
        int iPosXJugos;
        int iPosXPostres;

        public frmComandaAlmuerzo()
        {
            InitializeComponent();
        }

        #region FUNCIONES PARA CREAR LOS BOTONES

        //FUNCION PARA CONSULTAR LOS DATOS DEL REGISTRO DE ALMUERZOS
        private bool consultarDatosCalendario()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_cab_calendario_almuerzo, fecha" + Environment.NewLine;
                sSql += "from pos_cab_calendario_almuerzo" + Environment.NewLine;
                sSql += "where id_localidad = @id_localidad" + Environment.NewLine;
                sSql += "and fecha = @fecha" + Environment.NewLine;
                sSql += "and estado = @estado" + Environment.NewLine;
                sSql += "and is_active = @is_active";

                #region PARAMETROS

                int a = 0;
                parametro = new SqlParameter[4];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_localidad";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Program.iIdLocalidad;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@fecha";
                parametro[a].SqlDbType = SqlDbType.DateTime;
                parametro[a].Value = Convert.ToDateTime(sFecha);
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@is_active";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 1;

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
                    iIdCabecera = 0;
                else
                    iIdCabecera = Convert.ToInt32(dtConsulta.Rows[0]["id_pos_cab_calendario_almuerzo"].ToString());

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

        //FUNCION PARA CREAR LOS BOTONES
        private bool cargarBotonesSopas()
        {
            try
            {
                sSql = "";
                sSql += "select id_producto, codigo, nombre" + Environment.NewLine;
                sSql += "from pos_vw_almuerzo_sopas" + Environment.NewLine;
                sSql += "where id_pos_cab_calendario_almuerzo = @id_pos_cab_calendario_almuerzo" + Environment.NewLine;
                sSql += "order by nombre";

                #region PARAMETROS

                parametro = new SqlParameter[1];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_pos_cab_calendario_almuerzo";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdCabecera;

                #endregion

                dtSopas = new DataTable();
                dtSopas.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtSopas, sSql, parametro);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return false;
                }

                iCuentaSopas = 0;

                if (dtSopas.Rows.Count > 0)
                {
                    if (dtSopas.Rows.Count > 3)
                    {
                        btnSiguienteSopas.Enabled = true;
                        btnAnteriorSopas.Visible = true;
                        btnSiguienteSopas.Visible = true;
                    }

                    else
                    {
                        btnSiguienteSopas.Enabled = false;
                        btnAnteriorSopas.Visible = false;
                        btnSiguienteSopas.Visible = false;
                    }

                    //CONSTRUIR BOTONES
                }

                else
                {
                    pnlSopas.Controls.Clear();
                    btnAnteriorSopas.Visible = false;
                    btnSiguienteSopas.Visible = false;
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

        //FUNCION PARA CREAR LOS BOTONES DE SOPAS
        private bool crearBotonesSopas()
        {
            try
            {
                pnlSopas.Controls.Clear();

                iPosXSopas = 0;
                iCuentaAyudaSopas = 0;

                for (int i = 0; i < 3; i++)
                {
                    botonSopas[i] = new Button();
                    botonSopas[i].Cursor = Cursors.Hand;
                    //botonSopas[i].Click += new EventHandler(boton_clic_productos);
                    botonSopas[i].Size = new Size(130, 71);
                    botonSopas[i].Location = new Point(iPosXSopas, 0);
                    botonSopas[i].BackColor = Color.FromArgb(255, 255, 128);
                    botonSopas[i].Font = new Font("Maiandra GD", 9.75f, FontStyle.Bold);
                    botonSopas[i].Name = dtSopas.Rows[iCuentaSopas]["id_producto"].ToString();
                    botonSopas[i].Text = dtSopas.Rows[iCuentaSopas]["nombre"].ToString();
                    botonSopas[i].AccessibleDescription = dtSopas.Rows[iCuentaSopas]["codigo_clase_producto"].ToString();
                    //botonSopas[i].AccessibleName = dtSopas.Rows[iCuentaSopas]["valor"].ToString();
                    botonSopas[i].FlatStyle = FlatStyle.Flat;
                    botonSopas[i].FlatAppearance.BorderSize = 1;
                    botonSopas[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(128, 255, 128);
                    botonSopas[i].FlatAppearance.MouseDownBackColor = Color.Fuchsia;
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

        #endregion

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA TRAER LA FECHA
        private void fechaSistema()
        {
            try
            {
                funciones = new Clases.ClaseFunciones();

                bRespuesta = funciones.fechaSistema();

                if (bRespuesta == false)
                {

                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = funciones.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                sFecha = funciones.sFechaRecuperada;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        #endregion

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
