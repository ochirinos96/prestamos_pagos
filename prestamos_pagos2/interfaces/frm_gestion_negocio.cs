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
    public partial class frm_gestion_negocio : Telerik.WinControls.UI.RadForm
    {
        public frm_gestion_negocio()
        {
            InitializeComponent();
        }

        private void radTextBox1_TextChanged(object sender, EventArgs e)
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

            comando.CommandText = "exec mostrar_clientes_apellido '" + radTextBox1.Text + "'";
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
                dataGridView1.Rows[renglon].Cells["Column1"].Value = dr.GetString(dr.GetOrdinal("Dni")).ToString();
                dataGridView1.Rows[renglon].Cells["Column2"].Value = dr.GetString(dr.GetOrdinal("Nombres")).ToString();
                dataGridView1.Rows[renglon].Cells["Column3"].Value = dr.GetString(dr.GetOrdinal("apellidos")).ToString();
                dataGridView1.Rows[renglon].Cells["Column4"].Value = dr.GetInt32(dr.GetOrdinal("Celular")).ToString();

            }

            conn.conn.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
            comando.CommandText = "exec busqueda_negocio '" + dni + "'";
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
                dataGridView2.Rows[renglon].Cells["Column5"].Value = dr.GetString(dr.GetOrdinal("Dni")).ToString();
                dataGridView2.Rows[renglon].Cells["Column6"].Value = dr.GetInt32(dr.GetOrdinal("Nombres")).ToString();
                dataGridView2.Rows[renglon].Cells["Column7"].Value = dr.GetString(dr.GetOrdinal("apellidos")).ToString();
                dataGridView2.Rows[renglon].Cells["Column8"].Value = dr.GetString(dr.GetOrdinal("Celular")).ToString();

            }

            conn.conn.Close();
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            string codigo = this.dataGridView2.CurrentRow.Cells[0].Value.ToString();

          
            coneccion conn = new coneccion();

            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }
            SqlCommand cmd = new SqlCommand("baja_negocio", conn.conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Codigo", codigo);
            

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();

                MessageBox.Show("Negocio dado de baja", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {
                MessageBox.Show("Error al dar de baja el negocio", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            conn.conn.Close();

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string codigo = this.dataGridView2.CurrentRow.Cells[0].Value.ToString();


            coneccion conn = new coneccion();

            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }

            string comand = "exec mostrar_negocio_actualizar '" + codigo + "'";
            string genero;
            SqlCommand cmd = new SqlCommand(comand, conn.conn);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                radTextBox1.Text = dr["Dni"].ToString();
                radTextBox2.Text = dr["Ruc"].ToString();
                radTextBox2.Text = dr["Nombre_negocio"].ToString();
                radTextBox3.Text = dr["Rubro_negocio"].ToString();
                radTextBox5.Text = dr["Direccion"].ToString();
                radTextBox6.Text = dr["Distrito"].ToString();
                radTextBox7.Text = dr["Provincia"].ToString();
                radTextBox8.Text = dr["Departamento"].ToString();
                radTextBox9.Text = dr["Telefono"].ToString();
                radTextBox10.Text = dr["Tipo_local"].ToString();
                radTextBox3.Text = dr["Referencia"].ToString();
                radTextBox5.Text = dr["url_foto"].ToString();
                radTextBox6.Text = dr["disp_efectivo"].ToString();
                radTextBox7.Text = dr["inventario"].ToString();
                radTextBox8.Text = dr["t_prestamos"].ToString();
                radTextBox9.Text = dr["t_ingresos"].ToString();
                radTextBox10.Text = dr["t_activos"].ToString();
                radTextBox8.Text = dr["t_pasivos"].ToString();
                radTextBox9.Text = dr["t_costo_mercaderia"].ToString();
                radTextBox10.Text = dr["u_operativa"].ToString();
                radTextBox8.Text = dr["t_costos_operativos"].ToString();
                radTextBox9.Text = dr["u_liquida"].ToString();
                

                byte[] MyData = new byte[0];
                MyData = (byte[])dr["Fotografia_url"];
                MemoryStream stream = new MemoryStream(MyData);
                pictureBox1.Image = Image.FromStream(stream);

                radTextBox21.Text = dr["Ocupacion"].ToString();
                radTextBox22.Text = dr["Ingresos_promedio"].ToString();

                
                MessageBox.Show("Datos obtenidos correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error al obtener los datos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }
    }
}
