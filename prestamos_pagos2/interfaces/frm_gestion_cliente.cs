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
    public partial class frm_gestion_cliente : Telerik.WinControls.UI.RadForm
    {
        public string dni_log;
        public frm_gestion_cliente()
        {
            InitializeComponent();
        }
        public frm_gestion_cliente(string dni)
        {
            InitializeComponent();
            dni_log = dni;
        }

        private void radButton6_Click(object sender, EventArgs e)
        {

        }

        private void radTextBox20_TextChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked=true)
            {
                mostrar_clientes_apellido();
            }
            else
            {
                mostrar_clientes_dni();
            }
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

            comando.CommandText = "exec mostrar_clientes_apellido '" + radTextBox20.Text + "'";
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
                dataGridView1.Rows[renglon].Cells["column3"].Value = dr.GetString(dr.GetOrdinal("apellidos")).ToString();
                dataGridView1.Rows[renglon].Cells["column4"].Value = dr.GetString(dr.GetOrdinal("Direccion")).ToString();
                dataGridView1.Rows[renglon].Cells["column5"].Value = dr.GetInt32(dr.GetOrdinal("Telefono")).ToString();
                dataGridView1.Rows[renglon].Cells["column6"].Value = dr.GetInt32(dr.GetOrdinal("Celular")).ToString();

            }

            conn.conn.Close();
        }

        public void mostrar_clientes_dni()
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

            comando.CommandText = "exec mostrar_clientes_dni '" + radTextBox25.Text + "'";
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
                dataGridView1.Rows[renglon].Cells["column3"].Value = dr.GetString(dr.GetOrdinal("apellidos")).ToString();
                dataGridView1.Rows[renglon].Cells["column4"].Value = dr.GetString(dr.GetOrdinal("Direccion")).ToString();
                dataGridView1.Rows[renglon].Cells["column5"].Value = dr.GetInt32(dr.GetOrdinal("Telefono")).ToString();
                dataGridView1.Rows[renglon].Cells["column6"].Value = dr.GetInt32(dr.GetOrdinal("Celular")).ToString();

            }

            conn.conn.Close();
        }

        private void radTextBox25_TextChanged(object sender, EventArgs e)
        {
            mostrar_clientes_dni();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


            string dni = this.dataGridView1.CurrentRow.Cells[0].Value.ToString();
            string id_modo = "0";

            

            coneccion conn = new coneccion();

            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }
            string comand = "exec mostrar_vista_cliente '" + dni + "'";
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
                radTextBox26.Text = dr["Referencia"].ToString();
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

                radTextBox21.Text = dr["Ocupacion"].ToString();
                radTextBox22.Text = dr["Ingresos_promedio"].ToString();

                mostrar_familiares();

                MessageBox.Show("Datos obtenidos correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error al obtener los datos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            
        }

        public void mostrar_familiares()
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

            comando.CommandText = "exec mostrar_familiar '" + radTextBox1.Text + "'";
            //especificamos que es de tipo Text
            comando.CommandType = CommandType.Text;

            //limpiamos los renglones de la datagridview
            dataGridView1.Rows.Clear();
            //a la variable DataReader asignamos  el la variable de tipo SqlCommand
            dr = comando.ExecuteReader();
            //el ciclo while se ejecutará mientras lea registros en la tabla
            string apellidos;
            while (dr.Read())
            {
                //variable de tipo entero para ir enumerando los la filas del datagridview
                int renglon = dataGridView1.Rows.Add();
                // especificamos en que fila se mostrará cada registro
                // nombredeldatagrid.filas[numerodefila].celdas[nombredelacelda].valor=
                // dr.tipodedatosalmacenado(dr.getordinal(nombredelcampo_en_la_base_de_datos)conviertelo_a_string_sino_es_del_tipo_string);
                dataGridView1.Rows[renglon].Cells["column1"].Value = dr.GetString(dr.GetOrdinal("Dni_familiar")).ToString();
                dataGridView1.Rows[renglon].Cells["column2"].Value = dr.GetString(dr.GetOrdinal("Nombres")).ToString();
                apellidos = dr.GetString(dr.GetOrdinal("Ap_paterno")).ToString() + ' ' + dr.GetString(dr.GetOrdinal("Ap_materno")).ToString();
                dataGridView1.Rows[renglon].Cells["column3"].Value = apellidos;
                dataGridView1.Rows[renglon].Cells["column4"].Value = dr.GetInt32(dr.GetOrdinal("Celular")).ToString();


            }
        }
        public OpenFileDialog examinar = new OpenFileDialog();

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

        private void checkBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (checkBox1.Checked=false)
            {
                radButton1.Enabled=false;
            }
            else
            {
                radButton1.Enabled = true;
            }
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked=true)
            {
                actualizar_cliente();
                actualizar_foto();
            }
            else
            {
                actualizar_cliente();
                
            }
        }

        public void actualizar_cliente()
        {
            

            coneccion conn = new coneccion();
            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }

            SqlCommand cmd = new SqlCommand("actualizar_cliente", conn.conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Dni", radTextBox1.Text.ToString());
            cmd.Parameters.AddWithValue("@Nombres", radTextBox2.Text.ToString());
            cmd.Parameters.AddWithValue("@Ap_paterno", radTextBox3.Text);
            cmd.Parameters.AddWithValue("@Ap_materno", radTextBox4.Text.ToString());
            cmd.Parameters.AddWithValue("@Direccion", radTextBox5.Text.ToString());
            cmd.Parameters.AddWithValue("@Distrito", radTextBox6.Text.ToString());
            cmd.Parameters.AddWithValue("@Provincia", radTextBox7.Text);
            cmd.Parameters.AddWithValue("@Departamento", radTextBox8.Text.ToString());
            cmd.Parameters.AddWithValue("@Telefono", radTextBox9.Text.ToString());
            cmd.Parameters.AddWithValue("@Celular", radTextBox10.Text.ToString());
            cmd.Parameters.AddWithValue("@Ocupacion", radTextBox21.Text);
            cmd.Parameters.AddWithValue("@Ingreso_prom", radTextBox22.Text);
            cmd.Parameters.AddWithValue("@Referencia", radTextBox23.Text);
                       

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();

                MessageBox.Show("Datos actualizados corectamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {
                MessageBox.Show("Error al actualizar los datos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void actualizar_foto()
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

            SqlCommand cmd = new SqlCommand("actualizar_foto", conn.conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Dni", radTextBox1.Text.ToString());
            cmd.Parameters.AddWithValue("@Fotografia_url", binData);
            

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();

               

            }
            catch
            {
                
            }
        }
    }
}
