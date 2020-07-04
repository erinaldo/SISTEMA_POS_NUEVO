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

namespace Palatium.RepartidorExterno
{
    public partial class frmPeriodosDelivery : Form
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
        bool bActualizar;

        SqlParameter[] parametro;

        int iBanderaDesdeHasta;
        int iHabilitado;
        int iIdRegistro;

        public frmPeriodosDelivery()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA CARGAR LOS REPARTIDORES EXTERNOS
        private void llenarComboRepartidores()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_origen_orden, descripcion" + Environment.NewLine;
                sSql += "from pos_origen_orden" + Environment.NewLine;
                sSql += "where estado = @estado" + Environment.NewLine;
                sSql += "and repartidor_externo = @repartidor_externo" + Environment.NewLine;
                sSql += "order by descripcion";

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@estado";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "A";

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@repartidor_externo";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = 1;

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
                row["id_pos_origen_orden"] = "0";
                row["descripcion"] = "Seleccione...!!!";
                dtConsulta.Rows.InsertAt(row, 0);

                cmbRepartidor.DisplayMember = "descripcion";
                cmbRepartidor.ValueMember = "id_pos_origen_orden";
                cmbRepartidor.DataSource = dtConsulta;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA CARGAR LOS REPARTIDORES EXTERNOS PARA FILTAR EN EL GRID
        private void llenarComboRepartidoresFiltro()
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_origen_orden, descripcion" + Environment.NewLine;
                sSql += "from pos_origen_orden" + Environment.NewLine;
                sSql += "where estado = @estado" + Environment.NewLine;
                sSql += "and repartidor_externo = @repartidor_externo" + Environment.NewLine;
                sSql += "order by descripcion";

                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@estado";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "A";

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@repartidor_externo";
                parametro[1].SqlDbType = SqlDbType.Int;
                parametro[1].Value = 1;

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
                row["id_pos_origen_orden"] = "0";
                row["descripcion"] = "Todos...!!!";
                dtConsulta.Rows.InsertAt(row, 0);

                cmbRepartidorFiltro.DisplayMember = "descripcion";
                cmbRepartidorFiltro.ValueMember = "id_pos_origen_orden";
                cmbRepartidorFiltro.DataSource = dtConsulta;
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

                int iCuenta = 2;

                sSql = "";
                sSql += "select PD.id_pos_periodo_delivery, PD.id_pos_origen_orden, PD.fecha_desde," + Environment.NewLine;
                sSql += "PD.fecha_hasta, PD.is_active, ORI.descripcion," + Environment.NewLine;
                sSql += "case PD.is_active when 1 then 'ACTIVO' else 'INACTIVO' end estado" + Environment.NewLine;
                sSql += "from pos_periodo_delivery PD INNER JOIN" + Environment.NewLine;
                sSql += "pos_origen_orden ORI ON ORI.id_pos_origen_orden = PD.id_pos_origen_orden" + Environment.NewLine;
                sSql += "and ORI.estado = @estado_1" + Environment.NewLine;
                sSql += "and PD.estado = @estado_2" + Environment.NewLine;

                if (Convert.ToInt32(cmbRepartidorFiltro.SelectedValue) != 0)
                {
                    iCuenta++;
                    sSql += "where PD.id_pos_origen_orden = @id_pos_origen_orden" + Environment.NewLine;
                }

                sSql += "order by ORI.descripcion";

                #region PARAMETROS

                parametro = new SqlParameter[iCuenta];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@estado_1";
                parametro[0].SqlDbType = SqlDbType.VarChar;
                parametro[0].Value = "A";

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@estado_2";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = "A";

                if (iCuenta == 3)
                {
                    parametro[2] = new SqlParameter();
                    parametro[2].ParameterName = "@id_pos_origen_orden";
                    parametro[2].SqlDbType = SqlDbType.Int;
                    parametro[2].Value = Convert.ToInt32(cmbRepartidorFiltro.SelectedValue);
                }

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

                string _sFechaDesde_P;
                string _sFechaHasta_P;

                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    _sFechaDesde_P = Convert.ToDateTime(dtConsulta.Rows[i]["fecha_desde"].ToString()).ToString("dd/MM/yyyy");
                    _sFechaHasta_P = Convert.ToDateTime(dtConsulta.Rows[i]["fecha_hasta"].ToString()).ToString("dd/MM/yyyy");

                    dgvDatos.Rows.Add(dtConsulta.Rows[i]["id_pos_periodo_delivery"].ToString(),
                                      dtConsulta.Rows[i]["id_pos_origen_orden"].ToString(),
                                      _sFechaDesde_P, _sFechaHasta_P,
                                      dtConsulta.Rows[i]["is_active"].ToString(),
                                      dtConsulta.Rows[i]["descripcion"].ToString(),
                                      "DESDE: " + _sFechaDesde_P + " - HASTA: " + _sFechaHasta_P,
                                      dtConsulta.Rows[i]["estado"].ToString());
                }

                dgvDatos.ClearSelection();

                #endregion
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA INSERTAR EL REGISTRO
        private void insertarRegistro()
        {
            try
            {
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Errror al iniciar latransacción.";
                    ok.ShowDialog();
                    return;
                }

                sSql = "";
                sSql += "insert into pos_periodo_delivery (" + Environment.NewLine;
                sSql += "id_pos_origen_orden, fecha_desde, fecha_hasta, is_active," + Environment.NewLine;
                sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += "@id_pos_origen_orden, @fecha_desde, @fecha_hasta, @is_active," + Environment.NewLine;
                sSql += "@estado, getdate(), @usuario_ingreso, @terminal_ingreso)";

                #region PARAMETROS

                int a = 0;
                parametro = new SqlParameter[7];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pos_origen_orden";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Convert.ToInt32(cmbRepartidor.SelectedValue);
                a++;

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
                parametro[a].ParameterName = "@is_active";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iHabilitado;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@usuario_ingreso";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[0];
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@terminal_ingreso";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = Program.sDatosMaximo[1];

                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);

                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Registro guardado éxitosamente.";
                ok.ShowDialog();
                limpiar();

                return;
            }

            catch (Exception ex)
            {
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA INSERTAR EL REGISTRO
        private void actualizarRegistro()
        {
            try
            {
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Errror al iniciar latransacción.";
                    ok.ShowDialog();
                    return;
                }

                sSql = "";
                sSql += "update pos_periodo_delivery set" + Environment.NewLine;
                sSql += "id_pos_origen_orden = @id_pos_origen_orden," + Environment.NewLine;
                sSql += "fecha_desde = @fecha_desde," + Environment.NewLine;
                sSql += "fecha_hasta = @fecha_hasta," + Environment.NewLine;
                sSql += "is_active = @is_active" + Environment.NewLine;
                sSql += "where id_pos_periodo_delivery = @id_pos_periodo_delivery" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                int a = 0;
                parametro = new SqlParameter[6];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pos_origen_orden";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = Convert.ToInt32(cmbRepartidor.SelectedValue);
                a++;

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
                parametro[a].ParameterName = "@is_active";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iHabilitado;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pos_periodo_delivery";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdRegistro;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";


                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
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
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA INSERTAR EL REGISTRO
        private void eliminarRegistro()
        {
            try
            {
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Errror al iniciar latransacción.";
                    ok.ShowDialog();
                    return;
                }

                sSql = "";
                sSql += "update pos_periodo_delivery set" + Environment.NewLine;
                sSql += "is_active = @is_active" + Environment.NewLine;
                sSql += "where id_pos_periodo_delivery = @id_pos_periodo_delivery" + Environment.NewLine;
                sSql += "and estado = @estado";

                #region PARAMETROS

                int a = 0;
                parametro = new SqlParameter[3];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@is_active";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iHabilitado;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_pos_periodo_delivery";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdRegistro;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";


                #endregion

                if (!conexion.GFun_Lo_Ejecutar_SQL_Parametros(sSql, parametro))
                {
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);

                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Registro inhabilitado éxitosamente.";
                ok.ShowDialog();
                limpiar();

                return;
            }

            catch (Exception ex)
            {
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
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

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            fechaSistema();
            llenarComboRepartidores();

            cmbRepartidorFiltro.SelectedIndexChanged -= new EventHandler(cmbRepartidorFiltro_SelectedIndexChanged);
            llenarComboRepartidoresFiltro();
            cmbRepartidorFiltro.SelectedIndexChanged += new EventHandler(cmbRepartidorFiltro_SelectedIndexChanged);

            chkHabilitado.Checked = true;

            cmbRepartidor.Enabled = false;
            dtFechaDesde.Enabled = false;
            dtFechaHasta.Enabled = false;
            chkHabilitado.Checked = false;
            chkHabilitado.Enabled = false;

            cmbRepartidorFiltro.Enabled = true;

            bActualizar = false;

            dtFechaDesde.Text = sFecha;
            dtFechaHasta.Text = sFecha;

            btnGrabar.Text = "  Nuevo";
            btnGrabar.Focus();
            llenarGrid();
        }

        //FUNCION PARA CONSULTAR LA FECHA
        private int verificarFechas(DateTime dtFechaInicial_P, DateTime dtFechaFinal_P, int iIdPosOrigenOrden_P)
        {
            try
            {
                int a = 2;

                sSql = "";
                sSql += "select fecha_desde, fecha_hasta" + Environment.NewLine;
                sSql += "from pos_periodo_delivery" + Environment.NewLine;
                sSql += "where id_pos_origen_orden = @id_pos_origen_orden" + Environment.NewLine;
                sSql += "and estado = @estado";

                if (bActualizar == true)
                {
                    a++;
                    sSql += Environment.NewLine + "and id_pos_periodo_delivery <> @id_pos_periodo_delivery";
                }

                parametro = new SqlParameter[a];
                parametro[0] = new SqlParameter();
                parametro[0].ParameterName = "@id_pos_origen_orden";
                parametro[0].SqlDbType = SqlDbType.Int;
                parametro[0].Value = iIdPosOrigenOrden_P;

                parametro[1] = new SqlParameter();
                parametro[1].ParameterName = "@estado";
                parametro[1].SqlDbType = SqlDbType.VarChar;
                parametro[1].Value = "A";

                if (a == 3)
                {
                    parametro[2] = new SqlParameter();
                    parametro[2].ParameterName = "@id_pos_periodo_delivery";
                    parametro[2].SqlDbType = SqlDbType.Int;
                    parametro[2].Value = iIdRegistro;
                }

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro_Parametros(dtConsulta, sSql, parametro);

                if (bRespuesta == false)
                {
                    catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                    catchMensaje.lblMensaje.Text = conexion.sMensajeError;
                    catchMensaje.ShowDialog();
                    return -1;
                }

                if (dtConsulta.Rows.Count == 0)
                    return 0;

                DateTime _fechaInicial;
                DateTime _fechaFinal;

                int iContador = 0;

                //RECORRER LA FECHA INICIAL
                for (int i = 0; i < dtConsulta.Rows.Count; i++)
                {
                    _fechaInicial = Convert.ToDateTime(dtConsulta.Rows[i]["fecha_desde"].ToString());
                    _fechaFinal = Convert.ToDateTime(dtConsulta.Rows[i]["fecha_hasta"].ToString());

                    if ((dtFechaInicial_P >= _fechaInicial) && (dtFechaInicial_P <= _fechaFinal))
                    {
                        iContador++;
                        break;
                    }
                }

                if (iContador == 0)
                {
                    //RECORRER LA FECHA FINAL
                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        _fechaInicial = Convert.ToDateTime(dtConsulta.Rows[i]["fecha_desde"].ToString());
                        _fechaFinal = Convert.ToDateTime(dtConsulta.Rows[i]["fecha_hasta"].ToString());

                        if ((dtFechaFinal_P >= _fechaInicial) && (dtFechaFinal_P <= _fechaFinal))
                        {
                            iContador++;
                            break;
                        }
                    }
                }

                return iContador;
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
                return -1;
            }
        }

        #endregion

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPeriodosDelivery_Load(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (btnGrabar.Text == "  Nuevo")
            {
                btnGrabar.Text = "  Guardar";
                bActualizar = false;
                cmbRepartidor.Enabled = true;
                dtFechaDesde.Enabled = true;
                dtFechaHasta.Enabled = true;
                chkHabilitado.Checked = true;
                cmbRepartidorFiltro.Enabled = false;
                cmbRepartidor.Focus();
            }

            else
            {
                if (Convert.ToInt32(cmbRepartidor.SelectedValue) == 0)
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Favor seleccione el repartidor externo";
                    ok.ShowDialog();
                    cmbRepartidor.Focus();
                    return;
                }                

                if (Convert.ToDateTime(dtFechaDesde.Text) > Convert.ToDateTime(dtFechaHasta.Text))
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "La fecha final no debe ser superior a la fecha inicial.";
                    ok.ShowDialog();
                    dtFechaHasta.Text = sFecha;
                    return;
                }

                int iCuenta = verificarFechas(Convert.ToDateTime(dtFechaDesde.Text), Convert.ToDateTime(dtFechaHasta.Text), Convert.ToInt32(cmbRepartidor.SelectedValue));

                if (iCuenta == -1)
                    return;

                if (iCuenta > 0)
                {
                    ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                    ok.lblMensaje.Text = "Las fechas seleccionadas ya se encuentran dentro de otro rango de fechas registradas.";
                    ok.ShowDialog();
                    return;
                }

                sFechaDesde = Convert.ToDateTime(dtFechaDesde.Text).ToString("yyyy-MM-dd");
                sFechaHasta = Convert.ToDateTime(dtFechaHasta.Text).ToString("yyyy-MM-dd");

                if (chkHabilitado.Checked == true)
                    iHabilitado = 1;
                else
                    iHabilitado = 0;

                SiNo = new VentanasMensajes.frmMensajeNuevoSiNo();

                if (bActualizar == true)
                {
                    SiNo.lblMensaje.Text = "¿Está seguro que desea actualizar el registro?";
                    SiNo.ShowDialog();

                    if (SiNo.DialogResult == DialogResult.OK)
                    {
                        actualizarRegistro();
                    }
                }

                else
                {
                    SiNo.lblMensaje.Text = "¿Está seguro que desea guardar el registro?";
                    SiNo.ShowDialog();

                    if (SiNo.DialogResult == DialogResult.OK)
                    {
                        insertarRegistro();
                    }
                }
            }
        }

        private void cmbRepartidorFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenarGrid();
        }

        private void dgvDatos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                iIdRegistro = Convert.ToInt32(dgvDatos.CurrentRow.Cells["id_pos_periodo_delivery"].Value);
                cmbRepartidor.SelectedValue = Convert.ToInt32(dgvDatos.CurrentRow.Cells["id_pos_origen_orden"].Value);
                dtFechaDesde.Text = Convert.ToDateTime(dgvDatos.CurrentRow.Cells["fecha_desde"].Value.ToString()).ToString("dd/MM/yyyy");
                dtFechaHasta.Text = Convert.ToDateTime(dgvDatos.CurrentRow.Cells["fecha_hasta"].Value.ToString()).ToString("dd/MM/yyyy");

                if (Convert.ToInt32(dgvDatos.CurrentRow.Cells["is_active"].Value) == 1)
                    chkHabilitado.Checked = true;
                else
                    chkHabilitado.Checked = false;

                chkHabilitado.Enabled = true;
                bActualizar = true;
                cmbRepartidor.Enabled = true;
                dtFechaDesde.Enabled = true;
                dtFechaHasta.Enabled = true;
                chkHabilitado.Checked = true;
                btnGrabar.Text = "  Actualizar";

                cmbRepartidorFiltro.Enabled = false;

                cmbRepartidor.Focus();
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        private void frmPeriodosDelivery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
