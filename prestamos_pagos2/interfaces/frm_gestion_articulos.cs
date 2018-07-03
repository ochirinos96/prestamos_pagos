using prestamos_pagos2.datos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace prestamos_pagos2.interfaces
{
    public partial class frm_gestion_articulos : Telerik.WinControls.UI.RadForm
    {
        public string dni_login;
        public frm_gestion_articulos(string dni_log)
        {
            InitializeComponent();
            dni_login = dni_log;

        }

        private void radLabel4_Click(object sender, EventArgs e)
        {

        }

        private void radTextBox6_TextChanged(object sender, EventArgs e)
        {
            mostrar_usuarios_apellido();
        }

        public void mostrar_usuarios_apellido()
        {
            coneccion conn = new coneccion();

            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }


            DataTable dt = new DataTable();

            SqlCommand comando = new SqlCommand();

            SqlDataReader dr;

            comando.Connection = conn.conn;

            comando.CommandText = "exec mostrar_usuarios_apellido '" + radTextBox6.Text + "'";
            //especificamos que es de tipo Text
            comando.CommandType = CommandType.Text;

            //limpiamos los renglones de la datagridview
            dataGridView1.Rows.Clear();
            //a la variable DataReader asignamos  el la variable de tipo SqlCommand
            dr = comando.ExecuteReader();
            //el ciclo while se ejecutará mientras lea registros en la tabla
            while (dr.Read())
            {
                //variable de tipo entero para ir enumerando los la filas del datagridview
                int renglon = dataGridView1.Rows.Add();
                // especificamos en que fila se mostrará cada registro
                // nombredeldatagrid.filas[numerodefila].celdas[nombredelacelda].valor=
                // dr.tipodedatosalmacenado(dr.getordinal(nombredelcampo_en_la_base_de_datos)conviertelo_a_string_sino_es_del_tipo_string);
                dataGridView1.Rows[renglon].Cells["column1"].Value = dr.GetString(dr.GetOrdinal("Dni")).ToString();
                dataGridView1.Rows[renglon].Cells["column2"].Value = dr.GetString(dr.GetOrdinal("Nombres")).ToString();
                dataGridView1.Rows[renglon].Cells["column3"].Value = dr.GetString(dr.GetOrdinal("Apellidos")).ToString();
                dataGridView1.Rows[renglon].Cells["column5"].Value = dr.GetInt32(dr.GetOrdinal("Telefono")).ToString();
                dataGridView1.Rows[renglon].Cells["column6"].Value = dr.GetInt32(dr.GetOrdinal("Celular")).ToString();

            }

            conn.conn.Close();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

            string dni = this.dataGridView1.CurrentRow.Cells[0].Value.ToString();

            coneccion conn = new coneccion();

            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }
            string comand = "exec mostrar_vista_usuario '" + dni + "'";
            string genero;
            SqlCommand cmd = new SqlCommand(comand, conn.conn);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                radTextBox7.Text = dr["Dni"].ToString();
                radTextBox2.Text = dr["Nombres"].ToString();

                byte[] MyData = new byte[0];
                MyData = (byte[])dr["Fotografia_url"];
                MemoryStream stream = new MemoryStream(MyData);
                pictureBox1.Image = Image.FromStream(stream);


                MessageBox.Show("Datos obtenidos correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error al obtener los datos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void radTextBox7_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
