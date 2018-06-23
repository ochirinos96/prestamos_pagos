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
    public partial class frm_registro_cliente : Telerik.WinControls.UI.RadForm
    {
        public OpenFileDialog examinar = new OpenFileDialog();
        public frm_registro_cliente()
        {
            InitializeComponent();
        }
        public string dni_login;
        public frm_registro_cliente(string dni_log)
        {
            dni_login = dni_log;
            InitializeComponent();
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

        private void radButton4_Click(object sender, EventArgs e)
        {
                
                
            string genero;
            if (radioButton1.Checked==true)
            {
                genero = "M";
            }
            else
            {
                genero = "F";
            }

            insertar_c_f(genero);

            radTextBox1.Text = "";
            radTextBox2.Text = "";
            radTextBox3.Text = "";
            radTextBox4.Text = "";
            radTextBox5.Text = "";
            radTextBox6.Text = "";
            radTextBox7.Text = "";
            radTextBox8.Text = "";
            radTextBox9.Text = "";
            radTextBox10.Text = "";
            radTextBox23.Text = "";
            radTextBox21.Text = "";
            radTextBox22.Text = "";


        }
        public void insertar_c_f(string genero)
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

            SqlCommand cmd = new SqlCommand("registrar_cliente_antiguo", conn.conn);
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
            cmd.Parameters.AddWithValue("@Fotografia_url", binData);
            cmd.Parameters.AddWithValue("@Genero", genero);
            cmd.Parameters.AddWithValue("@Ocupacion", radTextBox21.Text);
            cmd.Parameters.AddWithValue("@Ingreso_prom", radTextBox22.Text);
            cmd.Parameters.AddWithValue("@Referencia", radTextBox23.Text);
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
            string busqueda;
            coneccion conn = new coneccion();

            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }

            string comand = "exec duplicado_persona '" + radTextBox1.Text + "'";

            string genero;
            SqlCommand cmd = new SqlCommand(comand, conn.conn);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {

                busqueda = dr["Dni"].ToString();
                radTextBox1.Text= dr["Dni"].ToString();
                radTextBox2.Text= dr["Nombres"].ToString();
                radTextBox3.Text = dr["Ap_paterno"].ToString();
                radTextBox4.Text = dr["Ap_materno"].ToString();
                radTextBox5.Text = dr["Direccion"].ToString();
                radTextBox6.Text = dr["Distrito"].ToString();
                radTextBox7.Text = dr["Provincia"].ToString();
                radTextBox8.Text = dr["Departamento"].ToString();
                radTextBox9.Text = dr["Telefono"].ToString();
                radTextBox10.Text = dr["Celular"].ToString();
                radTextBox23.Text = dr["Referencia"].ToString();
                genero= dr["Genero"].ToString();
                if (genero=="M")
                {
                    radioButton1.Checked = true;
                }else
                {
                    radioButton2.Checked = true;
                    radioButton1.Checked = false;
                }

                cliente_duplicado();
                byte[] MyData = new byte[0];
                MyData= (byte[])dr["Fotografia_url"];
                MemoryStream stream = new MemoryStream(MyData);
                pictureBox1.Image = Image.FromStream(stream);
            }
            else
            {
                busqueda = "";
            }

            if (busqueda != "")
            {
                radTextBox1.Text = busqueda;
                MessageBox.Show("Esta persona ya esta registrada su DNI es " + busqueda + "", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.conn.Close();
                mostrar_familiares();
            }
            else
            {
                radButton4.Enabled = true;
            }

            radTextBox24.Text = radTextBox1.Text;
            

        }

        public void cliente_duplicado() {

            coneccion conn = new coneccion();
            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }

            string comand = "exec duplicado_cliente '" + radTextBox1.Text + "'";

            
            SqlCommand cmd = new SqlCommand(comand, conn.conn);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {

                radTextBox21.Text = dr["Ocupacion"].ToString();
                radTextBox22.Text = dr["Ingresos_promedio"].ToString();

            }
            else
            {
                radTextBox21.Text = "";
                radTextBox22.Text = "";
            }

            if (radTextBox21.Text != "")
            {
                 radButton4.Enabled = false;
            }
            else
            {
                radButton4.Enabled = true;
            }
            radTextBox24.Text = radTextBox1.Text;

            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void radTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void radTextBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void radTextBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {


            string genero;
            if (radioButton1.Checked == true)
            {
                genero = "M";
            }
            else
            {
                genero = "F";
            }
            string parentesco = comboBox1.Text;

            registrar_familiar(genero,parentesco);
            mostrar_familiares();
            
            radTextBox20.Text = "";
            radTextBox19.Text = "";
            radTextBox18.Text = "";
            radTextBox17.Text = "";
            radTextBox16.Text = "";
            radTextBox15.Text = "";
            radTextBox14.Text = "";
            radTextBox13.Text = "";
            radTextBox12.Text = "";
            radTextBox11.Text = "";
            radTextBox25.Text = "";
           

        }

        public void registrar_familiar(string genero, string parentesco)
        {
            string texto;
            texto = "Foto"+"/"+"default_image.png";

            FileStream stream = new FileStream(texto, FileMode.Open, FileAccess.Read);
            //Se inicailiza un flujo de archivo con la imagen seleccionada desde el disco.
            BinaryReader br = new BinaryReader(stream);
            FileInfo fi = new FileInfo(texto);

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

            SqlCommand cmd = new SqlCommand("registrar_familiar", conn.conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Dni_titular", radTextBox24.Text.ToString());
            cmd.Parameters.AddWithValue("@Dni", radTextBox20.Text.ToString());
            cmd.Parameters.AddWithValue("@Nombres", radTextBox19.Text);
            cmd.Parameters.AddWithValue("@Ap_paterno", radTextBox18.Text.ToString());
            cmd.Parameters.AddWithValue("@Ap_materno", radTextBox17.Text.ToString());
            cmd.Parameters.AddWithValue("@Direccion", radTextBox16.Text.ToString());
            cmd.Parameters.AddWithValue("@Distrito", radTextBox15.Text);
            cmd.Parameters.AddWithValue("@Provincia", radTextBox14.Text.ToString());
            cmd.Parameters.AddWithValue("@Departamento", radTextBox13.Text.ToString());
            cmd.Parameters.AddWithValue("@Telefono", radTextBox12.Text.ToString());
            cmd.Parameters.AddWithValue("@Celular", radTextBox11.Text.ToString());
            cmd.Parameters.AddWithValue("@Genero", genero);
            cmd.Parameters.AddWithValue("@Referencia", radTextBox25.Text);
            cmd.Parameters.AddWithValue("@Parentesco", parentesco);
            cmd.Parameters.AddWithValue("@Terminal", Environment.MachineName);
            cmd.Parameters.AddWithValue("@Dni_registro", dni_login);
            cmd.Parameters.AddWithValue("@Fotografia_url", binData);
            

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


        private void radTextBox20_TextChanged(object sender, EventArgs e)
        {
            string busqueda;
            coneccion conn = new coneccion();

            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }

            string comand = "exec duplicado_persona '" + radTextBox20.Text + "'";

            string genero;
            SqlCommand cmd = new SqlCommand(comand, conn.conn);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {

                busqueda = dr["Dni"].ToString();
                radTextBox19.Text = dr["Nombres"].ToString();
                radTextBox18.Text = dr["Ap_paterno"].ToString();
                radTextBox17.Text = dr["Ap_materno"].ToString();
                radTextBox16.Text = dr["Direccion"].ToString();
                radTextBox15.Text = dr["Distrito"].ToString();
                radTextBox14.Text = dr["Provincia"].ToString();
                radTextBox13.Text = dr["Departamento"].ToString();
                radTextBox12.Text = dr["Telefono"].ToString();
                radTextBox11.Text = dr["Celular"].ToString();
                radTextBox25.Text = dr["Referencia"].ToString();
                genero = dr["Genero"].ToString();
                if (genero == "M")
                {
                    radioButton4.Checked = true;
                }
                else
                {
                    radioButton3.Checked = true;
                    radioButton4.Checked = false;
                }

            }
            else
            {
                busqueda = "";
            }

                if (busqueda != "")
                {
                    MessageBox.Show("Esta persona ya esta registrada su DNI es " + busqueda + "", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    radButton2.Enabled = false;
                    familiar_duplicado();
            }
                else
                {
                    radButton2.Enabled = true;
                }

                conn.conn.Close();
            
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

        public void familiar_duplicado()
        {

            coneccion conn = new coneccion();
            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }

            string comand = "exec duplicado_familiar '" + radTextBox24.Text + "','" + radTextBox20.Text + "'";


            SqlCommand cmd = new SqlCommand(comand, conn.conn);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {

                comboBox1.Text = dr["Parentesco"].ToString();
                conn.conn.Close();
            }
            else
            {
                radTextBox20.Text = "";
                
            }

            if (radTextBox21.Text != "")
            {
                radButton2.Enabled = false;
            }
            else
            {
                radButton2.Enabled = true;
            }
            

        }



        private void radButton5_Click(object sender, EventArgs e)
        {
            radTextBox1.Text = "";
            radTextBox2.Text = "";
            radTextBox3.Text = "";
            radTextBox4.Text = "";
            radTextBox5.Text = "";
            radTextBox6.Text = "";
            radTextBox7.Text = "";
            radTextBox8.Text = "";
            radTextBox9.Text = "";
            radTextBox10.Text = "";
            radTextBox23.Text = "";
            radTextBox21.Text = "";
            radTextBox22.Text = "";
        }

        private void radTextBox22_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            radTextBox20.Text = "";
            radTextBox19.Text = "";
            radTextBox18.Text = "";
            radTextBox17.Text = "";
            radTextBox16.Text = "";
            radTextBox15.Text = "";
            radTextBox14.Text = "";
            radTextBox13.Text = "";
            radTextBox12.Text = "";
            radTextBox11.Text = "";
            radTextBox25.Text = "";
            
        }

        private void radTextBox20_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void radTextBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void radTextBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }
    }
}
