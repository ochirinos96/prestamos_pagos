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

            comando.CommandText = "exec mostrar_clientes_apellido '" + radTextBox6.Text + "'";
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
       

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

            string dni = this.dataGridView1.CurrentRow.Cells[0].Value.ToString();

            radTextBox7.Text = dni;
            coneccion conn = new coneccion();

            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }


            DataTable dt = new DataTable();

            SqlCommand comando = new SqlCommand();

            SqlDataReader dr;

            comando.Connection = conn.conn;

            comando.CommandText = "exec mostrar_articulos_cliente '" + dni + "'";
            //especificamos que es de tipo Text
            comando.CommandType = CommandType.Text;

            //limpiamos los renglones de la datagridview
            dataGridView2.Rows.Clear();
            //a la variable DataReader asignamos  el la variable de tipo SqlCommand
            dr = comando.ExecuteReader();
            //el ciclo while se ejecutará mientras lea registros en la tabla
            while (dr.Read())
            {
                //variable de tipo entero para ir enumerando los la filas del datagridview
                int renglon = dataGridView2.Rows.Add();
                // especificamos en que fila se mostrará cada registro
                // nombredeldatagrid.filas[numerodefila].celdas[nombredelacelda].valor=
                // dr.tipodedatosalmacenado(dr.getordinal(nombredelcampo_en_la_base_de_datos)conviertelo_a_string_sino_es_del_tipo_string);
                dataGridView2.Rows[renglon].Cells["dataGridViewTextBoxColumn1"].Value = dr.GetString(dr.GetOrdinal("codigo_articulo")).ToString();
                dataGridView2.Rows[renglon].Cells["dataGridViewTextBoxColumn2"].Value = dr.GetString(dr.GetOrdinal("Nombre")).ToString();
                dataGridView2.Rows[renglon].Cells["Column4"].Value = dr.GetString(dr.GetOrdinal("N_serie")).ToString();
                dataGridView2.Rows[renglon].Cells["dataGridViewTextBoxColumn3"].Value = dr.GetInt32(dr.GetOrdinal("V_contable")).ToString();
                dataGridView2.Rows[renglon].Cells["dataGridViewTextBoxColumn4"].Value = dr.GetString(dr.GetOrdinal("Observaciones")).ToString();

            }

            conn.conn.Close();

            ver_baja_articulo();


        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void radTextBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string codigo = this.dataGridView2.CurrentRow.Cells[0].Value.ToString();

            coneccion conn = new coneccion();

            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }
            string comand = "exec mostrar_articulos_detalle '" + codigo + "'";

            SqlCommand cmd = new SqlCommand(comand, conn.conn);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                radTextBox1.Text = dr["N_serie"].ToString();
                radTextBox2.Text = dr["Nombre"].ToString();
                radTextBox3.Text = dr["V_contable"].ToString();
                radDateTimePicker2.Value = Convert.ToDateTime(dr["F_compra"].ToString());
                radTextBox5.Text = dr["Observaciones"].ToString();

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

        private void radButton3_Click(object sender, EventArgs e)
        {

        }

        private void radButton1_Click(object sender, EventArgs e)
        {

            string codigo = this.dataGridView2.CurrentRow.Cells[0].Value.ToString();


            coneccion conn = new coneccion();
            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }

            SqlCommand cmd = new SqlCommand("actualizar_articulo", conn.conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Codigo_articulo", codigo);
            cmd.Parameters.AddWithValue("@N_serie", radTextBox1.Text.ToString());
            cmd.Parameters.AddWithValue("@Nombre", radTextBox2.Text.ToString());
            cmd.Parameters.AddWithValue("@V_contable", radTextBox3.Text);
            cmd.Parameters.AddWithValue("@F_compra", radDateTimePicker2.Value);
            cmd.Parameters.AddWithValue("@Observaciones", radTextBox5.Text.ToString());
            

            try
            {

                SqlDataReader dr = cmd.ExecuteReader();
                MessageBox.Show("Datos actualizados corectamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                
            }
            catch
            {
                MessageBox.Show("Error al agregar los datos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            conn.conn.Close();

            mostrar_articulos();
        }

        private void radButton2_Click(object sender, EventArgs e)
        {

            string codigo = this.dataGridView2.CurrentRow.Cells[0].Value.ToString();

            coneccion conn = new coneccion();
            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }

            SqlCommand cmd = new SqlCommand("baja_articulo", conn.conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Codigo_articulo", codigo);
           

            try
            {

                SqlDataReader dr = cmd.ExecuteReader();
                MessageBox.Show("Articulo dado de baja correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            catch
            {
                MessageBox.Show("Error al dar de baja los datos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            conn.conn.Close();
            ver_baja_articulo();
            mostrar_articulos();
        }


        public void mostrar_articulos()
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

            comando.CommandText = "exec mostrar_articulos_cliente '" + radTextBox7.Text + "'";
            //especificamos que es de tipo Text
            comando.CommandType = CommandType.Text;

            //limpiamos los renglones de la datagridview
            dataGridView2.Rows.Clear();
            //a la variable DataReader asignamos  el la variable de tipo SqlCommand
            dr = comando.ExecuteReader();
            //el ciclo while se ejecutará mientras lea registros en la tabla
            while (dr.Read())
            {
                //variable de tipo entero para ir enumerando los la filas del datagridview
                int renglon = dataGridView2.Rows.Add();
                // especificamos en que fila se mostrará cada registro
                // nombredeldatagrid.filas[numerodefila].celdas[nombredelacelda].valor=
                // dr.tipodedatosalmacenado(dr.getordinal(nombredelcampo_en_la_base_de_datos)conviertelo_a_string_sino_es_del_tipo_string);
                dataGridView2.Rows[renglon].Cells["dataGridViewTextBoxColumn1"].Value = dr.GetString(dr.GetOrdinal("codigo_articulo")).ToString();
                dataGridView2.Rows[renglon].Cells["dataGridViewTextBoxColumn2"].Value = dr.GetString(dr.GetOrdinal("Nombre")).ToString();
                dataGridView2.Rows[renglon].Cells["Column4"].Value = dr.GetString(dr.GetOrdinal("N_serie")).ToString();
                dataGridView2.Rows[renglon].Cells["dataGridViewTextBoxColumn3"].Value = dr.GetInt32(dr.GetOrdinal("V_contable")).ToString();
                dataGridView2.Rows[renglon].Cells["dataGridViewTextBoxColumn4"].Value = dr.GetString(dr.GetOrdinal("Observaciones")).ToString();

            }

            conn.conn.Close();

            ver_baja_articulo();
        }

        public void ver_baja_articulo()
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

            comando.CommandText = "exec ver_baja_articulo '" + radTextBox7.Text + "'";
            //especificamos que es de tipo Text
            comando.CommandType = CommandType.Text;

            //limpiamos los renglones de la datagridview
            dataGridView3.Rows.Clear();
            //a la variable DataReader asignamos  el la variable de tipo SqlCommand
            dr = comando.ExecuteReader();
            //el ciclo while se ejecutará mientras lea registros en la tabla
            while (dr.Read())
            {
                //variable de tipo entero para ir enumerando los la filas del datagridview
                int renglon = dataGridView3.Rows.Add();
                // especificamos en que fila se mostrará cada registro
                // nombredeldatagrid.filas[numerodefila].celdas[nombredelacelda].valor=
                // dr.tipodedatosalmacenado(dr.getordinal(nombredelcampo_en_la_base_de_datos)conviertelo_a_string_sino_es_del_tipo_string);
                dataGridView3.Rows[renglon].Cells["dataGridViewTextBoxColumn5"].Value = dr.GetString(dr.GetOrdinal("Codigo_articulo")).ToString();
                dataGridView3.Rows[renglon].Cells["dataGridViewTextBoxColumn6"].Value = dr.GetString(dr.GetOrdinal("Nombre")).ToString();
                dataGridView3.Rows[renglon].Cells["dataGridViewTextBoxColumn7"].Value = dr.GetInt32(dr.GetOrdinal("V_contable")).ToString();
                dataGridView3.Rows[renglon].Cells["dataGridViewTextBoxColumn8"].Value = dr.GetString(dr.GetOrdinal("Observaciones")).ToString();
                

            }

            conn.conn.Close();
        }

        private void radButton7_Click(object sender, EventArgs e)
        {
            string codigo = this.dataGridView3.CurrentRow.Cells[0].Value.ToString();

            coneccion conn = new coneccion();

            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }

            SqlCommand cmd = new SqlCommand("alta_articulo", conn.conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Codigo_articulo", codigo);


            try
            {

                SqlDataReader dr = cmd.ExecuteReader();
                MessageBox.Show("Articulo restaurado correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            catch
            {
                MessageBox.Show("Error al restaurar el articulo", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            conn.conn.Close();
            ver_baja_articulo();
            mostrar_articulos();
        }
    }
}
