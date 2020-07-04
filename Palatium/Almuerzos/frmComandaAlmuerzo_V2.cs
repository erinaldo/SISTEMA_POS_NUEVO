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

namespace Palatium.Almuerzos
{
    public partial class frmComandaAlmuerzo_V2 : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;
        VentanasMensajes.frmMensajeNuevoOk ok;

        Clases.ClaseFunciones funciones;

        Button[] botonSopas = new Button[4];
        Button[] botonSegundos = new Button[4];
        Button[] botonJugos = new Button[4];
        Button[] botonPostres = new Button[4];

        string sSql;
        string sFecha;

        bool bRespuesta;

        DataTable dtConsulta;
        DataTable dtPrimerPlato;
        DataTable dtSegundoPlato;
        DataTable dtJugosPostre;

        SqlParameter[] parametro;

        int iIdCabecera;
        int iCuentaPrimerPlato;
        int iCuentaSegundoPlato;
        int iCuentaJugosPostre;
        int iCuentaAyudaPrimerPlato;
        int iCuentaAyudaSegundoPlato;
        int iCuentaAyudaJugosPostres;
        int iPosXPrimerPlato;
        int iPosXSegundoPlato;
        int iPosXJugosPostre;
        int iIdListaMinorista;

        public frmComandaAlmuerzo_V2()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //EVENTO DE CONSUMIDOR FINAL
        private void cargarDatosConsumidorFinal()
        {
            //txtIdentificacion.Text = "9999999999999";
            //txtRazonSocial.Text = "CONSUMIDOR FINAL";
            //txtTelefono.Text = "9999999999";
            //txtMail.Text = Program.sCorreoElectronicoDefault;
            //txtDireccion.Text = "QUITO";
            //iIdPersona = Program.iIdPersona;
            //idTipoIdentificacion = 180;
            //idTipoPersona = 2447;
            //btnEditar.Visible = false;
        }

        //FUNCION PARA OBTENER LOS DATOS DE LA LISTA BASE Y MINORISTA
        private void datosListas()
        {
            try
            {
                sSql = "";
                sSql += "select id_lista_precio" + Environment.NewLine;
                sSql += "from cv403_listas_precios" + Environment.NewLine;
                sSql += "where lista_minorista = @lista_minorista" + Environment.NewLine;
                sSql += "and estado = @estado";

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@lista_minorista";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = 1;

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
                    return;
                }

                if (dtConsulta.Rows.Count > 0)
                    iIdListaMinorista = Convert.ToInt32(dtConsulta.Rows[0]["id_lista_precio"]);
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        #endregion

        #region FUNCIONES PARA CREAR LOS BOTONES DE LA COMANDA

        //FUNCION PARA CARGAR LOS BOTONES DE PRIMER PLATO
        private bool cargarBotonesPrimerPlato()
        {
            try
            {
                sSql = "";
                sSql += "select * from pos_vw_menu_platos" + Environment.NewLine;
                sSql += "where id_lista_precio = @id_lista_precio" + Environment.NewLine;
                sSql += "and secuencia = @secuencia" + Environment.NewLine;
                sSql += "order by nombre";

                #region PARAMETROS

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_lista_precio";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdListaMinorista;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@secuencia";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = 1;

                #endregion

                dtPrimerPlato = new DataTable();
                dtPrimerPlato.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtPrimerPlato, sSql, parametro);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return false;
                }

                iCuentaPrimerPlato = 0;

                if (dtPrimerPlato.Rows.Count > 0)
                {
                    if (dtPrimerPlato.Rows.Count > 4)
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
                    crearBotonesPrimerPlato();
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

        //FUNCION PARA CREAR LOS BOTONES DE PRIMER PLATO
        private bool crearBotonesPrimerPlato()
        {
            try
            {
                pnlSopas.Controls.Clear();

                iPosXPrimerPlato = 0;
                iCuentaAyudaPrimerPlato = 0;

                for (int i = 0; i < 4; i++)
                {
                    botonSopas[i] = new Button();
                    botonSopas[i].Cursor = Cursors.Hand;
                    //botonSopas[i].Click += new EventHandler(boton_clic_productos);
                    botonSopas[i].Size = new Size(130, 71);
                    botonSopas[i].Location = new Point(iPosXPrimerPlato, 0);
                    botonSopas[i].BackColor = Color.FromArgb(255, 255, 128);
                    botonSopas[i].Font = new Font("Maiandra GD", 9.75f, FontStyle.Bold);
                    botonSopas[i].Name = dtPrimerPlato.Rows[iCuentaPrimerPlato]["id_producto"].ToString();
                    botonSopas[i].Text = dtPrimerPlato.Rows[iCuentaPrimerPlato]["nombre"].ToString();
                    botonSopas[i].AccessibleDescription = dtPrimerPlato.Rows[iCuentaPrimerPlato]["codigo"].ToString();
                    botonSopas[i].AccessibleName = dtPrimerPlato.Rows[iCuentaPrimerPlato]["valor"].ToString();
                    botonSopas[i].FlatStyle = FlatStyle.Flat;
                    botonSopas[i].FlatAppearance.BorderSize = 1;
                    botonSopas[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(128, 255, 128);
                    botonSopas[i].FlatAppearance.MouseDownBackColor = Color.Fuchsia;

                    if (Program.iUsarIconosProductos == 1)
                    {
                        if (dtPrimerPlato.Rows[iCuentaPrimerPlato]["imagen_categoria"].ToString().Trim() != "")
                        {
                            Image foto;
                            byte[] imageBytes;

                            imageBytes = Convert.FromBase64String(dtPrimerPlato.Rows[iCuentaPrimerPlato]["imagen_categoria"].ToString().Trim());

                            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                            {
                                foto = Image.FromStream(ms, true);
                            }

                            botonSopas[i].TextAlign = ContentAlignment.BottomCenter;
                            botonSopas[i].Image = foto;
                            botonSopas[i].ImageAlign = ContentAlignment.TopCenter;
                            botonSopas[i].BackgroundImageLayout = ImageLayout.Stretch;
                            botonSopas[i].Font = new Font("Maiandra GD", 7.25F, FontStyle.Bold);
                        }
                    }

                    pnlSopas.Controls.Add(botonSopas[i]);
                    iCuentaPrimerPlato++;
                    iCuentaAyudaPrimerPlato++;
                    iPosXPrimerPlato += 130;

                    if (dtPrimerPlato.Rows.Count == iCuentaPrimerPlato)
                    {
                        btnSiguienteSopas.Enabled = false;
                        break;
                    }
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

        //FUNCION PARA CARGAR LOS BOTONES DE SEGUNDO PLATO
        private bool cargarBotonesSegundoPlato()
        {
            try
            {
                sSql = "";
                sSql += "select * from pos_vw_menu_platos" + Environment.NewLine;
                sSql += "where id_lista_precio = @id_lista_precio" + Environment.NewLine;
                sSql += "and secuencia = @secuencia" + Environment.NewLine;
                sSql += "order by nombre";

                #region PARAMETROS

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_lista_precio";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdListaMinorista;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@secuencia";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = 2;

                #endregion

                dtSegundoPlato = new DataTable();
                dtSegundoPlato.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtSegundoPlato, sSql, parametro);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return false;
                }

                iCuentaSegundoPlato = 0;

                if (dtSegundoPlato.Rows.Count > 0)
                {
                    if (dtSegundoPlato.Rows.Count > 4)
                    {
                        btnSiguienteSegundos.Enabled = true;
                        btnAnteriorSegundos.Visible = true;
                        btnSiguienteSegundos.Visible = true;
                    }

                    else
                    {
                        btnSiguienteSegundos.Enabled = false;
                        btnAnteriorSegundos.Visible = false;
                        btnSiguienteSegundos.Visible = false;
                    }

                    //CONSTRUIR BOTONES
                    crearBotonesSegundoPlato();
                }

                else
                {
                    pnlSegundos.Controls.Clear();
                    btnAnteriorSegundos.Visible = false;
                    btnSiguienteSegundos.Visible = false;
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

        //FUNCION PARA CREAR LOS BOTONES DE SEGUNDO PLATO
        private bool crearBotonesSegundoPlato()
        {
            try
            {
                pnlSegundos.Controls.Clear();

                iPosXSegundoPlato = 0;
                iCuentaAyudaSegundoPlato = 0;

                for (int i = 0; i < 4; i++)
                {
                    botonSegundos[i] = new Button();
                    botonSegundos[i].Cursor = Cursors.Hand;
                    //botonSegundos[i].Click += new EventHandler(boton_clic_productos);
                    botonSegundos[i].Size = new Size(130, 71);
                    botonSegundos[i].Location = new Point(iPosXSegundoPlato, 0);
                    botonSegundos[i].BackColor = Color.FromArgb(255, 255, 128);
                    botonSegundos[i].Font = new Font("Maiandra GD", 9.75f, FontStyle.Bold);
                    botonSegundos[i].Name = dtSegundoPlato.Rows[iCuentaSegundoPlato]["id_producto"].ToString();
                    botonSegundos[i].Text = dtSegundoPlato.Rows[iCuentaSegundoPlato]["nombre"].ToString();
                    botonSegundos[i].AccessibleDescription = dtSegundoPlato.Rows[iCuentaSegundoPlato]["codigo"].ToString();
                    botonSegundos[i].AccessibleName = dtSegundoPlato.Rows[iCuentaSegundoPlato]["valor"].ToString();
                    botonSegundos[i].FlatStyle = FlatStyle.Flat;
                    botonSegundos[i].FlatAppearance.BorderSize = 1;
                    botonSegundos[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(128, 255, 128);
                    botonSegundos[i].FlatAppearance.MouseDownBackColor = Color.Fuchsia;

                    if (Program.iUsarIconosProductos == 1)
                    {
                        if (dtSegundoPlato.Rows[iCuentaSegundoPlato]["imagen_categoria"].ToString().Trim() != "")
                        {
                            Image foto;
                            byte[] imageBytes;

                            imageBytes = Convert.FromBase64String(dtSegundoPlato.Rows[iCuentaSegundoPlato]["imagen_categoria"].ToString().Trim());

                            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                            {
                                foto = Image.FromStream(ms, true);
                            }

                            botonSegundos[i].TextAlign = ContentAlignment.BottomCenter;
                            botonSegundos[i].Image = foto;
                            botonSegundos[i].ImageAlign = ContentAlignment.TopCenter;
                            botonSegundos[i].BackgroundImageLayout = ImageLayout.Stretch;
                            botonSegundos[i].Font = new Font("Maiandra GD", 7.25F, FontStyle.Bold);
                        }
                    }

                    pnlSegundos.Controls.Add(botonSegundos[i]);
                    iCuentaSegundoPlato++;
                    iCuentaAyudaSegundoPlato++;
                    iPosXSegundoPlato += 130;

                    if (dtSegundoPlato.Rows.Count == iCuentaSegundoPlato)
                    {
                        btnSiguienteSegundos.Enabled = false;
                        break;
                    }
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

        //FUNCION PARA CARGAR LOS BOTONES DE POSTRES Y JUGOS
        private bool cargarBotonesJugosPostres()
        {
            try
            {
                sSql = "";
                sSql += "select * from pos_vw_menu_platos" + Environment.NewLine;
                sSql += "where id_lista_precio = @id_lista_precio" + Environment.NewLine;
                sSql += "and secuencia = @secuencia" + Environment.NewLine;
                sSql += "order by nombre";

                #region PARAMETROS

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_lista_precio";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdListaMinorista;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@secuencia";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = 3;

                #endregion

                dtJugosPostre = new DataTable();
                dtJugosPostre.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtJugosPostre, sSql, parametro);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return false;
                }

                iCuentaJugosPostre = 0;

                if (dtJugosPostre.Rows.Count > 0)
                {
                    if (dtJugosPostre.Rows.Count > 4)
                    {
                        btnSiguienteJugos.Enabled = true;
                        btnAnteriorJugos.Visible = true;
                        btnSiguienteJugos.Visible = true;
                    }

                    else
                    {
                        btnSiguienteJugos.Enabled = false;
                        btnAnteriorJugos.Visible = false;
                        btnSiguienteJugos.Visible = false;
                    }

                    //CONSTRUIR BOTONES
                    crearBotonesJugosPostres();
                }

                else
                {
                    pnlJugos.Controls.Clear();
                    btnAnteriorJugos.Visible = false;
                    btnSiguienteJugos.Visible = false;
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
        private bool crearBotonesJugosPostres()
        {
            try
            {
                pnlJugos.Controls.Clear();

                iPosXJugosPostre = 0;
                iCuentaAyudaJugosPostres = 0;

                for (int i = 0; i < 4; i++)
                {
                    botonJugos[i] = new Button();
                    botonJugos[i].Cursor = Cursors.Hand;
                    //botonJugos[i].Click += new EventHandler(boton_clic_productos);
                    botonJugos[i].Size = new Size(130, 71);
                    botonJugos[i].Location = new Point(iPosXJugosPostre, 0);
                    botonJugos[i].BackColor = Color.FromArgb(255, 255, 128);
                    botonJugos[i].Font = new Font("Maiandra GD", 9.75f, FontStyle.Bold);
                    botonJugos[i].Name = dtJugosPostre.Rows[iCuentaJugosPostre]["id_producto"].ToString();
                    botonJugos[i].Text = dtJugosPostre.Rows[iCuentaJugosPostre]["nombre"].ToString();
                    botonJugos[i].AccessibleDescription = dtJugosPostre.Rows[iCuentaJugosPostre]["codigo"].ToString();
                    botonJugos[i].AccessibleName = dtJugosPostre.Rows[iCuentaJugosPostre]["valor"].ToString();
                    botonJugos[i].FlatStyle = FlatStyle.Flat;
                    botonJugos[i].FlatAppearance.BorderSize = 1;
                    botonJugos[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(128, 255, 128);
                    botonJugos[i].FlatAppearance.MouseDownBackColor = Color.Fuchsia;

                    if (Program.iUsarIconosProductos == 1)
                    {
                        if (dtJugosPostre.Rows[iCuentaJugosPostre]["imagen_categoria"].ToString().Trim() != "")
                        {
                            Image foto;
                            byte[] imageBytes;

                            imageBytes = Convert.FromBase64String(dtJugosPostre.Rows[iCuentaJugosPostre]["imagen_categoria"].ToString().Trim());

                            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                            {
                                foto = Image.FromStream(ms, true);
                            }

                            botonJugos[i].TextAlign = ContentAlignment.BottomCenter;
                            botonJugos[i].Image = foto;
                            botonJugos[i].ImageAlign = ContentAlignment.TopCenter;
                            botonJugos[i].BackgroundImageLayout = ImageLayout.Stretch;
                            botonJugos[i].Font = new Font("Maiandra GD", 7.25F, FontStyle.Bold);
                        }
                    }

                    pnlJugos.Controls.Add(botonJugos[i]);
                    iCuentaJugosPostre++;
                    iCuentaAyudaJugosPostres++;
                    iPosXJugosPostre += 130;

                    if (dtJugosPostre.Rows.Count == iCuentaJugosPostre)
                    {
                        btnSiguienteJugos.Enabled = false;
                        break;
                    }
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

        private void frmComandaAlmuerzo_V2_Load(object sender, EventArgs e)
        {
            datosListas();
            cargarDatosConsumidorFinal();
            cargarBotonesPrimerPlato();
            cargarBotonesSegundoPlato();
            cargarBotonesJugosPostres();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
