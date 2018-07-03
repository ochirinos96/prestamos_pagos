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
    public partial class frm_gestion_personal : Telerik.WinControls.UI.RadForm
    {
        public OpenFileDialog examinar = new OpenFileDialog();
        public frm_gestion_personal()
        {
            InitializeComponent();
        }
        public string dni_login;
        public frm_gestion_personal(string login)
        {
            InitializeComponent();
            dni_login = login;
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            string codigo = this.dataGridView2.CurrentRow.Cells[0].Value.ToString();

            coneccion conn = new coneccion();
            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }

            SqlCommand cmd = new SqlCommand("actualizar_personal_contrato", conn.conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigo", codigo);
            cmd.Parameters.AddWithValue("@Cargo", comboBox4.Text);
            cmd.Parameters.AddWithValue("@Fecha_ingreso", radDateTimePicker2.Value.Date);
            cmd.Parameters.AddWithValue("@Remuneracion_basica", radTextBox16.Text);
            cmd.Parameters.AddWithValue("@Condicion_contrato", comboBox3.Text.ToString());
            cmd.Parameters.AddWithValue("@Inicio_contrato", radDateTimePicker3.Value.Date);
            cmd.Parameters.AddWithValue("@Fin_contrato", radDateTimePicker4.Value.Date);
            cmd.Parameters.AddWithValue("@Observacion", radRichTextEditor1.Text.ToString());
            
            SqlDataReader dr = cmd.ExecuteReader();
            try

            {

                MessageBox.Show("Contrato actualizado corectamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mostrar_contratos(radTextBox13.Text);
                conn.conn.Close();
            }
            catch
            {
                MessageBox.Show("Error al actualizar el contrato", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void radTextBox12_TextChanged(object sender, EventArgs e)
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

            comando.CommandText = "exec mostrar_usuarios_apellido '" + radTextBox12.Text + "'";
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
                dataGridView1.Rows[renglon].Cells["column4"].Value = dr.GetString(dr.GetOrdinal("Direccion")).ToString();
                dataGridView1.Rows[renglon].Cells["column5"].Value = dr.GetInt32(dr.GetOrdinal("Telefono")).ToString();
                dataGridView1.Rows[renglon].Cells["column6"].Value = dr.GetInt32(dr.GetOrdinal("Celular")).ToString();

            }

            conn.conn.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
                radTextBox1.Text = dr["Dni"].ToString();
                radTextBox2.Text = dr["Nombres"].ToString();
                radTextBox3.Text = dr["Ap_paterno"].ToString();
                radTextBox4.Text = dr["Ap_materno"].ToString();
                radTextBox5.Text = dr["Direccion"].ToString();
                radTextBox6.Text = dr["Distrito"].ToString();
                radTextBox7.Text = dr["Provincia"].ToString();
                radTextBox8.Text = dr["Departamento"].ToString();
                radTextBox9.Text = dr["Telefono"].ToString();
                radTextBox10.Text = dr["Celular"].ToString();
                radTextBox11.Text = dr["Referencia"].ToString();
                genero = dr["Genero"].ToString();
                if (genero == "M")
                {
                    radioButton1.Checked = true;
                }
                else
                {
                    radioButton2.Checked = true;
                    radioButton1.Checked = false;
                }

                byte[] MyData = new byte[0];
                MyData = (byte[])dr["Fotografia_url"];
                MemoryStream stream = new MemoryStream(MyData);
                pictureBox1.Image = Image.FromStream(stream);
                
                mostrar_contratos(dni);

                MessageBox.Show("Datos obtenidos correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.conn.Close();
            }
            else
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

            string estado;
            DataTable dt = new DataTable();

            SqlCommand comando = new SqlCommand();

            SqlDataReader dr;

            comando.Connection = conn.conn;

            comando.CommandText = "exec mostrar_registros_personal '" + dni + "'";
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
                dataGridView2.Rows[renglon].Cells["dataGridViewTextBoxColumn1"].Value = dr.GetString(dr.GetOrdinal("Codigo_usuario")).ToString();
                dataGridView2.Rows[renglon].Cells["dataGridViewTextBoxColumn2"].Value = dr.GetString(dr.GetOrdinal("Cargo")).ToString();
                dataGridView2.Rows[renglon].Cells["dataGridViewTextBoxColumn3"].Value = dr.GetString(dr.GetOrdinal("Condicion_contrato")).ToString();
                dataGridView2.Rows[renglon].Cells["dataGridViewTextBoxColumn4"].Value = dr.GetInt32(dr.GetOrdinal("Remuneracion_basica")).ToString();
                dataGridView2.Rows[renglon].Cells["dataGridViewTextBoxColumn5"].Value = dr.GetDateTime(dr.GetOrdinal("Fin_contrato")).ToString();
                estado = dr.GetString(dr.GetOrdinal("Estado")).ToString();
                if (estado == "1")
                {
                    estado = "Activo";
                }
                else
                {
                    estado = "Caduco";
                }
                dataGridView2.Rows[renglon].Cells["dataGridViewTextBoxColumn6"].Value = estado;



            }
            conn.conn.Close();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            examinar.Filter = "image files|*.jpg;*.png;*.gif;*.ico;.*;";
            DialogResult dres1 = examinar.ShowDialog();
            if (dres1 == DialogResult.Abort)
                return;
            if (dres1 == DialogResult.Cancel)
                return;
            textBox1.Text = examinar.FileName;
            pictureBox1.Image = Image.FromFile(examinar.FileName);
        }

        private void radButton2_Click(object sender, EventArgs e)
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
            string comand = "exec mostrar_vista_usuarios '" + codigo + "'";
            
            SqlCommand cmd = new SqlCommand(comand, conn.conn);

            SqlDataReader dr = cmd.ExecuteReader();
            
            if (dr.Read())
            {
                radTextBox13.Text = dr["Dni"].ToString();
                comboBox4.Text = dr["Cargo"].ToString();
                radDateTimePicker2.Value = Convert.ToDateTime(dr["Fecha_ingreso"].ToString());
                radTextBox16.Text = dr["Remuneracion_basica"].ToString();
                comboBox3.Text = dr["Condicion_contrato"].ToString();
                radDateTimePicker3.Value = Convert.ToDateTime(dr["Inicio_contrato"].ToString());
                radDateTimePicker4.Value = Convert.ToDateTime(dr["Fin_contrato"].ToString());
                radRichTextEditor1.Text = dr["Observacion"].ToString();
                
                MessageBox.Show("Datos obtenidos correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error al obtener los datos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            string codigo = this.dataGridView2.CurrentRow.Cells[0].Value.ToString();

            coneccion conn = new coneccion();
            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }

            SqlCommand cmd = new SqlCommand("truncar_contrato_personal", conn.conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigo", codigo);
            
            
            try

            {
                SqlDataReader dr = cmd.ExecuteReader();
                MessageBox.Show("Contrato cancelado corectamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mostrar_contratos(radTextBox13.Text);
                conn.conn.Close();
            }
            catch
            {
                MessageBox.Show("Error al cancelar el contrato", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
