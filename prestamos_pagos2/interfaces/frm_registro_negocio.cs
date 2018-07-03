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
    public partial class frm_registro_negocio : Telerik.WinControls.UI.RadForm
    {
        public string dni_login;
        public OpenFileDialog examinar = new OpenFileDialog();
        public frm_registro_negocio()
        {
            InitializeComponent();
        }
        public frm_registro_negocio(string login)
        {
            InitializeComponent();
            dni_login = login;
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
            FileStream stream = new FileStream(textBox1.Text, FileMode.Open, FileAccess.Read);
            //Se inicailiza un flujo de archivo con la imagen seleccionada desde el disco.
            BinaryReader br = new BinaryReader(stream);
            FileInfo fi = new FileInfo(textBox1.Text);

            //Se inicializa un arreglo de Bytes del tamaño de la imagen
            byte[] binData = new byte[stream.Length];
            //Se almacena en el arreglo de bytes la informacion que se obtiene del flujo de archivos(foto)
            //Lee el bloque de bytes del flujo y escribe los datos en un búfer dado.
            stream.Read(binData, 0, Convert.ToInt32(stream.Length));

            ////Se muetra la imagen obtenida desde el flujo de datos
            pictureBox1.Image = Image.FromStream(stream);

            coneccion conn = new coneccion();
            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }

            SqlCommand cmd = new SqlCommand("registrar_negocio", conn.conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Dni", radTextBox7.Text.ToString());
            cmd.Parameters.AddWithValue("@Ruc", radTextBox2.Text.ToString());
            cmd.Parameters.AddWithValue("@Nombre_negocio", radTextBox3.Text);
            cmd.Parameters.AddWithValue("@Rubro_negocio", radTextBox4.Text.ToString());
            cmd.Parameters.AddWithValue("@Direccion", radTextBox5.Text.ToString());
            cmd.Parameters.AddWithValue("@Distrito", radTextBox6.Text.ToString());
            cmd.Parameters.AddWithValue("@Provincia", radTextBox9.Text);
            cmd.Parameters.AddWithValue("@Departamento", radTextBox8.Text.ToString());
            cmd.Parameters.AddWithValue("@Telefono", radTextBox11.Text.ToString());
            cmd.Parameters.AddWithValue("@Tipo_local", comboBox4.Text);
            cmd.Parameters.AddWithValue("@Referencia", radTextBox12.Text);
            cmd.Parameters.AddWithValue("@url_foto", binData);
            cmd.Parameters.AddWithValue("@disp_efectivo", radTextBox13.Text);
            cmd.Parameters.AddWithValue("@inventario", radTextBox14.Text);
            cmd.Parameters.AddWithValue("@t_prestamos", radTextBox17.Text);
            cmd.Parameters.AddWithValue("@t_ingresos", radTextBox18.Text);
            cmd.Parameters.AddWithValue("@t_activos", radTextBox16.Text);
            cmd.Parameters.AddWithValue("@t_pasivos", radTextBox17.Text);
            cmd.Parameters.AddWithValue("@t_costo_mercaderia", radTextBox19.Text);
            cmd.Parameters.AddWithValue("@u_operativa", radTextBox20.Text);
            cmd.Parameters.AddWithValue("@t_costos_operativos", radTextBox22.Text.ToString());
            cmd.Parameters.AddWithValue("@u_liquida", radTextBox21.Text.ToString());
            cmd.Parameters.AddWithValue("@Terminal", Environment.MachineName);
            cmd.Parameters.AddWithValue("@Dni_registro", dni_login);
            
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
                dataGridView2.Rows[renglon].Cells["Column5"].Value = dr.GetString(dr.GetOrdinal("Codigo_negocio")).ToString();
                dataGridView2.Rows[renglon].Cells["Column6"].Value = dr.GetString(dr.GetOrdinal("Ruc")).ToString();
                dataGridView2.Rows[renglon].Cells["Column7"].Value = dr.GetString(dr.GetOrdinal("Nombre_negocio")).ToString();
                dataGridView2.Rows[renglon].Cells["Column8"].Value = dr.GetString(dr.GetOrdinal("Direccion")).ToString();

            }

            conn.conn.Close();
        }
    }
}
