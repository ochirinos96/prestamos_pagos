using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Data.SqlClient;
using prestamos_pagos2.datos;

namespace prestamos_pagos2.interfaces
{
    public partial class frm_registro_articulos : Telerik.WinControls.UI.RadForm
    {

        public OpenFileDialog examinar = new OpenFileDialog();
        public frm_registro_articulos()
        {
            InitializeComponent();
        }
        public string dni_login;
        public frm_registro_articulos(string dni_log)
        {

            InitializeComponent();
            dni_login = dni_log;

        }


        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void radTextBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            FileStream stream2 = new FileStream(textBox2.Text, FileMode.Open, FileAccess.Read);
            //Se inicailiza un flujo de archivo con la imagen seleccionada desde el disco.
            BinaryReader br = new BinaryReader(stream2);
            FileInfo fi = new FileInfo(textBox2.Text);

            //Se inicializa un arreglo de Bytes del tamaño de la imagen
            byte[] binData2 = new byte[stream2.Length];
            //Se almacena en el arreglo de bytes la informacion que se obtiene del flujo de archivos(foto)
            //Lee el bloque de bytes del flujo y escribe los datos en un búfer dado.
            stream2.Read(binData2, 0, Convert.ToInt32(stream2.Length));

            ////Se muetra la imagen obtenida desde el flujo de datos
            pictureBox2.Image = Image.FromStream(stream2);


            coneccion conn = new coneccion();
            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }

            SqlCommand cmd = new SqlCommand("registro_articulo", conn.conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@dni_cliente",radTextBox13.Text.ToString());
            cmd.Parameters.AddWithValue("@N_serie", radTextBox16.Text.ToString());
            cmd.Parameters.AddWithValue("@Nombre", radTextBox15.Text.ToString());
            cmd.Parameters.AddWithValue("@V_contable", radTextBox14.Text.ToString());
            cmd.Parameters.AddWithValue("@F_compra", radDateTimePicker1.Value.Date);
            cmd.Parameters.AddWithValue("@Observaciones", radTextBox12.Text.ToString());
            cmd.Parameters.AddWithValue("@estado_art", comboBox1.Text.ToString());
            cmd.Parameters.AddWithValue("@Fotografia_url", binData2);
            cmd.Parameters.AddWithValue("@dni_registro", dni_login);
            cmd.Parameters.AddWithValue("@terminal", Environment.MachineName);
            
            try
            {

                SqlDataReader dr = cmd.ExecuteReader();
                MessageBox.Show("Datos registrados corectamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mostrar_articulos(radTextBox13.Text);
                limpiar_datos();
            }
            catch
            {
                MessageBox.Show("Error al agregar los datos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            conn.conn.Close();

            
        }

        public void limpiar_datos()
        {
            radTextBox16.Text = "";
            radTextBox15.Text = "";
            radTextBox14.Text = "";
            radTextBox12.Text = "";
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void radTextBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            examinar.Filter = "image files|*.jpg;*.png;*.gif;*.ico;.*;";
            DialogResult dres1 = examinar.ShowDialog();
            if (dres1 == DialogResult.Abort)
                return;
            if (dres1 == DialogResult.Cancel)
                return;
            textBox2.Text = examinar.FileName;
            pictureBox2.Image = Image.FromFile(examinar.FileName);
        }

        private void radTextBox9_TextChanged(object sender, EventArgs e)
        {
            mostrar_clientes_apellido();
        }

        public void mostrar_clientes_apellido()
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
                dataGridView1.Rows[renglon].Cells["Column1"].Value = dr.GetString(dr.GetOrdinal("Dni")).ToString();
                dataGridView1.Rows[renglon].Cells["Column2"].Value = dr.GetString(dr.GetOrdinal("Nombres")).ToString();
                dataGridView1.Rows[renglon].Cells["Column3"].Value = dr.GetString(dr.GetOrdinal("apellidos")).ToString();
                dataGridView1.Rows[renglon].Cells["Column4"].Value = dr.GetInt32(dr.GetOrdinal("Telefono")).ToString();
                dataGridView1.Rows[renglon].Cells["Column7"].Value = dr.GetInt32(dr.GetOrdinal("Celular")).ToString();

            }

            conn.conn.Close();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            string dni = this.dataGridView3.CurrentRow.Cells[0].Value.ToString();

            radTextBox13.Text = dni;

            mostrar_articulos(dni);


        }

        public void mostrar_articulos(string cliente)
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

            comando.CommandText = "exec mostrar_articulos_cliente '" + cliente+"'";
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
                dataGridView2.Rows[renglon].Cells["dataGridViewTextBoxColumn3"].Value = dr.GetInt32(dr.GetOrdinal("V_contable")).ToString();
                dataGridView2.Rows[renglon].Cells["Column8"].Value = dr.GetString(dr.GetOrdinal("N_serie")).ToString();
                dataGridView2.Rows[renglon].Cells["dataGridViewTextBoxColumn4"].Value = dr.GetString(dr.GetOrdinal("Observaciones")).ToString();

            }

            conn.conn.Close();
        }

        private void radTextBox6_TextChanged(object sender, EventArgs e)
        {
            mostrar_clientes_apellido();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string dni = this.dataGridView1.CurrentRow.Cells[0].Value.ToString();
            radTextBox4.Text = dni;
            mostrar_articulos(dni);
        }
    }
}
