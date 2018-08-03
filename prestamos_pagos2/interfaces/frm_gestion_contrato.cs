using prestamos_pagos2.datos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace prestamos_pagos2.interfaces
{
    public partial class frm_gestion_contrato : Telerik.WinControls.UI.RadForm
    {
        public frm_gestion_contrato()
        {
            InitializeComponent();
        }
        public string dni_login;
        public string tipo;
        
        public frm_gestion_contrato(string dni)
        {
            InitializeComponent();
            dni_login = dni;
            tipo = "PRESTAMO";
        }

        private void radTextBox1_TextChanged(object sender, EventArgs e)
        {
            mostrar_cliente_contrato();
        }

        public void mostrar_cliente_contrato()
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
                dataGridView1.Rows[renglon].Cells["column4"].Value = dr.GetInt32(dr.GetOrdinal("Celular")).ToString();

            }

            conn.conn.Close();
        }

        private void radButton5_Click(object sender, EventArgs e)
        {
            coneccion conn = new coneccion();
            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }

            SqlCommand cmd = new SqlCommand("registrar_contrato ", conn.conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Codigo_contrato", radTextBox4.Text.ToString());
            cmd.Parameters.AddWithValue("@Dni_cliente", radTextBox20.Text.ToString());
            cmd.Parameters.AddWithValue("@Dni_aval", radTextBox2.Text);
            cmd.Parameters.AddWithValue("@F_inicio", radDateTimePicker1.Value.Date);
            cmd.Parameters.AddWithValue("@F_fin", radDateTimePicker2.Value.Date);
            cmd.Parameters.AddWithValue("@T_prestamo", tipo);
            cmd.Parameters.AddWithValue("@Codigo_elemento", radTextBox6.Text);
            cmd.Parameters.AddWithValue("@monto_prestado", radTextBox5.Text.ToString());
            cmd.Parameters.AddWithValue("@N_cuotas", radTextBox9.Text.ToString());
            cmd.Parameters.AddWithValue("@Periodo", radTextBox10.Text.ToString());
            cmd.Parameters.AddWithValue("@P_cuota", binData);
            cmd.Parameters.AddWithValue("@Dni_registro", dni_login);
            cmd.Parameters.AddWithValue("@Terminal", Environment.MachineName);
           

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();

                MessageBox.Show("Datos registrados corectamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {
                MessageBox.Show("Error al agregar los datos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }



        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            string dni_cliente = this.dataGridView1.CurrentRow.Cells[0].Value.ToString();

            try
            {
                mostrar_contratos(dni_cliente);
                mostrar_familiares(dni_cliente);
                obtener_contrato();
                radTextBox20.Text = dni_cliente;
            }catch
            {
                MessageBox.Show("Error al obtener los datos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        public void mostrar_contratos(string dni)
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

            comando.CommandText = "exec mostrar_contratos '" + dni + "'";
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
                dataGridView2.Rows[renglon].Cells["dataGridViewTextBoxColumn1"].Value = dr.GetString(dr.GetOrdinal("Codigo_contrato")).ToString();
                dataGridView2.Rows[renglon].Cells["dataGridViewTextBoxColumn2"].Value = dr.GetDateTime(dr.GetOrdinal("F_inicio")).ToString();
                dataGridView2.Rows[renglon].Cells["dataGridViewTextBoxColumn3"].Value = dr.GetDateTime(dr.GetOrdinal("F_fin")).ToString();
                dataGridView2.Rows[renglon].Cells["dataGridViewTextBoxColumn4"].Value = dr.GetString(dr.GetOrdinal("T_prestamo")).ToString();
                dataGridView2.Rows[renglon].Cells["Column5"].Value = dr.GetInt32(dr.GetOrdinal("N_cuotas")).ToString();
            }

            conn.conn.Close();
        }

        public void mostrar_familiares(string dni)
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

            comando.CommandText = "exec mostrar_familiar '" + dni + "'";
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
                dataGridView3.Rows[renglon].Cells["dataGridViewTextBoxColumn5"].Value = dr.GetString(dr.GetOrdinal("Dni_familiar")).ToString();
                dataGridView3.Rows[renglon].Cells["dataGridViewTextBoxColumn6"].Value = dr.GetString(dr.GetOrdinal("Nombres")).ToString();
                string apellido_p, apellido_m;
                apellido_p= dr.GetString(dr.GetOrdinal("Ap_paterno")).ToString();
                apellido_m = dr.GetString(dr.GetOrdinal("Ap_materno")).ToString();


                dataGridView3.Rows[renglon].Cells["dataGridViewTextBoxColumn7"].Value = apellido_p + " "+ apellido_m;
                dataGridView3.Rows[renglon].Cells["dataGridViewTextBoxColumn8"].Value = dr.GetInt32(dr.GetOrdinal("Celular")).ToString();
            }

            conn.conn.Close();
        }

        public void obtener_contrato()
        {
          
            coneccion conn = new coneccion();

            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }

            string comand = "exec generar_codigo_contrato_nuevo ";

            SqlCommand cmd = new SqlCommand(comand, conn.conn);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                radTextBox4.Text = dr["codigo"].ToString();
                
            }
            

        }


        private void radButton7_Click(object sender, EventArgs e)
        {

        }

        private void radButton6_Click(object sender, EventArgs e)
        {
            string dni_aval = this.dataGridView3.CurrentRow.Cells[0].Value.ToString();
            radTextBox2.Text = dni_aval;
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            subfrmarticulos agregar = new subfrmarticulos(radTextBox20.Text);
            DialogResult res = agregar.ShowDialog();
            if (res == DialogResult.OK)
            {
                radTextBox6.Text = agregar.var1;
                tipo = agregar.var2;
            }

        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            subformnegocio agregar = new subformnegocio(radTextBox20.Text);
            DialogResult res = agregar.ShowDialog();
            if (res == DialogResult.OK)
            {
                radTextBox6.Text = agregar.var1;
                tipo = agregar.var2;
            }
            
        }
    }
}
