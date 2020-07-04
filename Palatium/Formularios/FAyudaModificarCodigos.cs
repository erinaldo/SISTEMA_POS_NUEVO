﻿using System;
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

namespace Palatium.Formularios
{
    public partial class FAyudaModificarCodigos : Form
    {
        //VARIABLES, INSTANCIAS
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        string[] G_st_datos = new string[2];
        bool x = false;
        string sCodigoProducto1;
        string sNombreProducto1;
        string sCodigoBarra1;
        string sCodigoProducto2;
        string sNombreProducto2;
        string sCodigoBarra2;
        int iIdProducto1;
        int iIdProducto2;

        public FAyudaModificarCodigos()
        {
            InitializeComponent();
        }

        private void FAyudaModificarCodigos_Load(object sender, EventArgs e)
        {
            string[] t_st_datos = { "1", "adsdasdasd" };
            llenarGrid(t_st_datos);
        }

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid(string[] t_st_datos)
        {
            try
            {
                string t_st_query = "";
                if (t_st_datos[0] == "1")
                {
                    t_st_query = "select PRO.id_producto,PRO.codigo as Código, NOM.nombre as Nombre, PRO.codigo_barra as Código_Barra "+
                        "from cv401_productos PRO inner join cv401_nombre_productos  NOM on PRO.id_producto = NOM.id_producto and NOM.estado = 'A' "+
                        "where ultimo_nivel=1";
                }

                else
                {
                    t_st_query = "select PRO.id_producto,PRO.codigo as Código, NOM.nombre as Nombre, PRO.codigo_barra as Código_Barra "+
                        "from cv401_productos PRO inner join cv401_nombre_productos NOM on PRO.id_producto = NOM.id_producto and NOM.estado = 'A' "+
                        "where ultimo_nivel=1 and PRO.codigo LIKE '%' + '" + t_st_datos[1] + "' OR NOM.nombre like '%' + "+
                        "'" + t_st_datos[1] + "' + '%'";
                }

                x = conexion.GFun_Lo_Rellenar_Grid(t_st_query, "cv401_productos");
                if (x == false)
                {
                    MessageBox.Show("Error en la consulta");
                }
                else
                {
                    dgvCodigo.DataSource = conexion.ds.Tables["cv401_productos"];
                    dgvCodigo.Refresh();

                    dgvCodigo.Columns[0].Width = 100;
                    dgvCodigo.Columns[1].Width = 200;
                    dgvCodigo.Columns[2].Width = 100;
                    dgvCodigo.Columns[3].Width = 100; 

                    //NICOLE
                    dgvCodigo.Rows[0].Selected = true;
                    dgvCodigo.CurrentCell = dgvCodigo.Rows[0].Cells[1];
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void btnBuscarCodigo_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBuscaCodigo.Text == "")
                {
                    //ENVIAR A LLENAR EL GRID CON TODOS LOS DATOS
                    G_st_datos[0] = "1";
                    G_st_datos[1] = "asdsdasd";
                }

                else
                {
                    //ENVIAR A LLENAR EL GRID CON UN SOLO DATO
                    G_st_datos[0] = "2";
                    G_st_datos[1] = txtBuscaCodigo.Text.Trim();
                }

                llenarGrid(G_st_datos);
            }

            catch (Exception)
            {
                MessageBox.Show("Error al general la consulta.", "Aviso", MessageBoxButtons.OK);
                string[] t_st_datos = { "1", "adsdasdasd" };
                llenarGrid(t_st_datos);
            }
        }

        private void dgvCodigo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            iIdProducto1 = Convert.ToInt32(dgvCodigo.CurrentRow.Cells[0].Value.ToString());
            sCodigoProducto1 = dgvCodigo.CurrentRow.Cells[1].Value.ToString();
            sNombreProducto1 = dgvCodigo.CurrentRow.Cells[2].Value.ToString();
            sCodigoBarra1 = dgvCodigo.CurrentRow.Cells[3].Value.ToString();
        }

        public int idProducto //creamos un metodo 
        {
            get { return iIdProducto2; }
        }

        public string codigo //creamos un metodo 
        {
            get { return sCodigoProducto2; }
        }

        public string nombre
        {
            get { return sNombreProducto2; }
        }

        public string codigoBarra
        {
            get { return sCodigoBarra2; }
        }

        private void btnAceptarCodigo_Click(object sender, EventArgs e)
        {
            iIdProducto2 = iIdProducto1;
            sCodigoProducto2 = sCodigoProducto1;
            sNombreProducto2 = sNombreProducto1;
            sCodigoBarra2 = sCodigoBarra1;
            
            this.Close();
        }

        private void btnSalirCodigo_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}