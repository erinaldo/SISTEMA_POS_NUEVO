using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.Promociones
{
    public partial class frmComandaPromociones : Form
    {
        public frmComandaPromociones()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clases.ClaseFunciones funciones = new Clases.ClaseFunciones();

            if (funciones.fechaSistema() == false)
                return;

            DateTime dt = funciones.dtFechaHoraRecuperada;

            textBox1.Text = dt.ToString("dd-MM-yyyy");
            byte dia1 = (byte)dt.DayOfWeek;

            textBox2.Text = dia1.ToString();
        }
    }
}
