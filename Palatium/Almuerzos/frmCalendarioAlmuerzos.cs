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
    public partial class frmCalendarioAlmuerzos : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();

        VentanasMensajes.frmMensajeNuevoOk ok;
        VentanasMensajes.frmMensajeNuevoCatch catchMensaje;

        Clases.ClaseFunciones funciones;

        ToolTip ttMensajeDia;

        string sSql;
        string sFecha;
        string sAnio;
        string sMes;

        DataTable dtConsulta;

        bool bRespuesta;

        int iIdLocalidad;
        int firstDayAtFlNumber;
        int totalDay;

        SqlParameter[] parametro;

        Image imgAlmuerzo;

        //List<FlowLayoutPanel> listaFLDias = new List<FlowLayoutPanel>();
        List<Button> listaFLDias = new List<Button>();
        DateTime fechaHoy;

        public frmCalendarioAlmuerzos()
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

        //FUNCION PARA OBTENER LA FECHA ACTUAL
        private bool fechaSistema()
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
                    return false;
                }

                sFecha = funciones.sFechaRecuperada;

                fechaHoy = Convert.ToDateTime(sFecha);

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

        private void generarPanelDias(int iDias_P)
        {
            try
            {
                flDias.Controls.Clear();
                listaFLDias.Clear();

                for (int i = 0; i < iDias_P; i++)
                {
                    Button fl = new Button();
                    fl.Name = "flDay" + (i + 1).ToString();
                    fl.Size = new Size(128, 65);
                    //fl.BackColor = Color.FromArgb(255, 192, 255);
                    fl.FlatAppearance.MouseDownBackColor = Color.FromArgb(192, 192, 255);
                    fl.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 255, 192);
                    fl.FlatStyle = FlatStyle.Flat;
                    fl.Font = new Font("Maiandra GD", 12F, FontStyle.Bold);
                    fl.TextAlign = ContentAlignment.TopLeft;
                    fl.ImageAlign = ContentAlignment.MiddleCenter;
                    fl.Cursor = Cursors.Hand;

                    ttMensajeDia = new ToolTip();
                    ttMensajeDia.SetToolTip(fl, "Sin fecha");

                    fl.Click += opcion_click;
                    flDias.Controls.Add(fl);
                    listaFLDias.Add(fl);
                }
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        private void opcion_click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbLocalidad.SelectedValue) == 0)
            {
                ok = new VentanasMensajes.frmMensajeNuevoOk(3);
                ok.lblMensaje.Text = "Favor seleccione una localidad.";
                ok.ShowDialog();
                cmbLocalidad.Focus();
                return;
            }

            Button flOK = sender as Button;
            int iIdRegistro_P = 0;
            int iBanderaActualizar = 0;

            if (Convert.ToInt32(flOK.Tag) != 0)
            {
                if (esNumero(flOK.Name) == true)
                {
                    iIdRegistro_P = Convert.ToInt32(flOK.Name);
                    iBanderaActualizar = 1;
                }

                Almuerzos.frmCrearItemsAlmuerzos crear = new frmCrearItemsAlmuerzos(flOK.AccessibleDescription, iBanderaActualizar, Convert.ToInt32(cmbLocalidad.SelectedValue), iIdRegistro_P);
                crear.ShowInTaskbar = false;
                crear.ShowDialog();

                if (crear.DialogResult == DialogResult.OK)
                {
                    AgregarEtiquetaDia(firstDayAtFlNumber, totalDay);
                    mostrarRegistros(firstDayAtFlNumber);
                }
            }
        }

        //VERIFICAR SI LA CADENA ES UN NUMERO O UN STRING
        private bool esNumero(object Expression)
        {

            bool isNum;

            double retNum;

            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);

            return isNum;
        }

        private void DisplayCurrentDate()
        {
            lblMonthAndYear.Text = fechaHoy.ToString("MMMM, yyyy");
            sAnio = fechaHoy.ToString("yyyy");
            sMes = fechaHoy.ToString("MM");
            firstDayAtFlNumber = ObtenerPrimerDiaDeSemanaDeLaFecha();
            totalDay = ObtenerTotalDiasFechaActual();   //ELVIS
            AgregarEtiquetaDia(firstDayAtFlNumber, totalDay); //ELVIS
            mostrarRegistros(firstDayAtFlNumber);
        }

        private int ObtenerPrimerDiaDeSemanaDeLaFecha()
        {
            DateTime primerDia = new DateTime(fechaHoy.Year, fechaHoy.Month, 1);
            return Convert.ToInt32(primerDia.DayOfWeek) + 1;
        }

        private int ObtenerTotalDiasFechaActual()
        {
            DateTime primerDiaFechaActual = new DateTime(fechaHoy.Year, fechaHoy.Month, 1);
            return primerDiaFechaActual.AddMonths(1).AddDays(-1).Day;
        }

        private void AgregarEtiquetaDia(int startDayAtFlNumber, int totalDaysInMonth)
        {
            try
            {
                foreach (Button fl in listaFLDias)
                {
                    fl.Controls.Clear();
                    fl.Tag = 0;
                    fl.BackColor = Color.White;
                    fl.Text = "";
                    fl.AccessibleDescription = "Sin fecha";
                }

                for (int i = 0; i < totalDaysInMonth; i++)
                {
                    listaFLDias[i + (startDayAtFlNumber - 1)].Text = (i + 1).ToString();
                    listaFLDias[i + (startDayAtFlNumber - 1)].Tag = i + 1;
                    listaFLDias[i + (startDayAtFlNumber - 1)].AccessibleDescription = (i + 1).ToString().PadLeft(2, '0') + "-" + sMes + "-" + sAnio;
                    listaFLDias[i + (startDayAtFlNumber - 1)].BackColor = Color.FromArgb(255, 192, 255);
                    ttMensajeDia = new ToolTip();
                    ttMensajeDia.SetToolTip(listaFLDias[i + (startDayAtFlNumber - 1)], listaFLDias[i + (startDayAtFlNumber - 1)].AccessibleDescription);

                    if (new DateTime(fechaHoy.Year, fechaHoy.Month, i + 1) == DateTime.Today)
                        listaFLDias[i + (startDayAtFlNumber - 1)].BackColor = Color.Aqua;
                }
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        private void mostrarRegistros(int startDayAtFlNumber)
        {
            try
            {
                DateTime dtFechaInicial = new DateTime(fechaHoy.Year, fechaHoy.Month, 1);
                DateTime dtFechaFinal = dtFechaInicial.AddMonths(1).AddDays(-1);

                iIdLocalidad = Convert.ToInt32(cmbLocalidad.SelectedValue);

                sSql = "";
                sSql += "select id_pos_cab_calendario_almuerzo, is_active, fecha, count(*) cuenta" + Environment.NewLine;
                sSql += "from pos_cab_calendario_almuerzo" + Environment.NewLine;
                sSql += "where estado = @estado" + Environment.NewLine;
                sSql += "and is_active = @is_active" + Environment.NewLine;
                sSql += "and fecha between @fecha_desde" + Environment.NewLine;
                sSql += "and @fecha_hasta" + Environment.NewLine;
                sSql += "and id_localidad = @id_localidad" + Environment.NewLine;
                sSql += "group by id_pos_cab_calendario_almuerzo, is_active, fecha" + Environment.NewLine;
                sSql += "order by fecha";

                #region PARAMETROS

                int a = 0;
                parametro = new SqlParameter[5];
                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@estado";
                parametro[a].SqlDbType = SqlDbType.VarChar;
                parametro[a].Value = "A";
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@is_active";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = 1;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@fecha_desde";
                parametro[a].SqlDbType = SqlDbType.DateTime;
                parametro[a].Value = dtFechaInicial;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@fecha_hasta";
                parametro[a].SqlDbType = SqlDbType.DateTime;
                parametro[a].Value = dtFechaFinal;
                a++;

                parametro[a] = new SqlParameter();
                parametro[a].ParameterName = "@id_localidad";
                parametro[a].SqlDbType = SqlDbType.Int;
                parametro[a].Value = iIdLocalidad;

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

                for (int i = 0; i < listaFLDias.Count; i++)
                {
                    listaFLDias[i].Image = null;
                    listaFLDias[i].AccessibleName = "0";
                    listaFLDias[i].Name = "btn_" + i.ToString();
                }

                foreach (DataRow row in dtConsulta.Rows)
                {
                    DateTime appDay = Convert.ToDateTime(row["fecha"]);
                    listaFLDias[(appDay.Day - 1) + (startDayAtFlNumber - 1)].Image = imgAlmuerzo;
                    listaFLDias[(appDay.Day - 1) + (startDayAtFlNumber - 1)].AccessibleName = row["is_active"].ToString();
                    listaFLDias[(appDay.Day - 1) + (startDayAtFlNumber - 1)].Name = row["id_pos_cab_calendario_almuerzo"].ToString();
                }    


                //string sql = "select * from appointment where AppDate between #{startDate.ToShortDateString()}# and #{endDate.ToShortDateString()}#";
                //DataTable dt = QueryAsDataTable(sql);

                //foreach (DataRow row in dt.Rows)
                //{
                //    DateTime appDay = DateTime.Parse(row("AppDate"));
                //    LinkLabel link = new LinkLabel();
                //    link.Tag = row("ID");
                //    // link.Name = link{row("ID")}
                //    link.Text = row("ContactName");
                //    link.Click += ShowAppointmentDetail;
                //    listFlDay((appDay.Day - 1) + (startDayAtFlNumber - 1)).Controls.Add(link);
                //}
            }

            catch (Exception ex)
            {
                catchMensaje = new VentanasMensajes.frmMensajeNuevoCatch();
                catchMensaje.lblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }


        #endregion

        private void frmCrearCalendarizacion_Load(object sender, EventArgs e)
        {
            imgAlmuerzo = Properties.Resources.icono_almuerzo_calendario_2;
            fechaSistema();
            cmbLocalidad.SelectedIndexChanged -= new EventHandler(cmbLocalidad_SelectedIndexChanged);
            llenarComboLocalidades();
            cmbLocalidad.SelectedIndexChanged += new EventHandler(cmbLocalidad_SelectedIndexChanged);
            generarPanelDias(42);
            DisplayCurrentDate();
        }

        private void btnHoy_Click(object sender, EventArgs e)
        {
            fechaSistema(); ;
            DisplayCurrentDate();
        }

        private void btnSiguienteMes_Click(object sender, EventArgs e)
        {
            fechaHoy = fechaHoy.AddMonths(1);
            DisplayCurrentDate();
        }

        private void btnAnteriorMes_Click(object sender, EventArgs e)
        {
            fechaHoy = fechaHoy.AddMonths(-1);
            DisplayCurrentDate();
        }

        private void cmbLocalidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayCurrentDate();
        }
    }
}
