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
    public partial class frm_registro_personal : Telerik.WinControls.UI.RadForm
    {
        public OpenFileDialog examinar = new OpenFileDialog();
        public frm_registro_personal()
        {
            InitializeComponent();
        }
        public string dni_login;
        public frm_registro_personal( string log)
        {
            InitializeComponent();
            dni_login = log;
        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            coneccion conn = new coneccion();
            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }

            SqlCommand cmd = new SqlCommand("registrar_personal_existente", conn.conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Dni", radTextBox1.Text.ToString());
            cmd.Parameters.AddWithValue("@usuario", radTextBox20.Text.ToString());
            cmd.Parameters.AddWithValue("@contrasenia", radTextBox19.Text);
            cmd.Parameters.AddWithValue("@permisos", comboBox1.Text.ToString());
            cmd.Parameters.AddWithValue("@Fecha_ingreso", radDateTimePicker2.Value.Date);
            cmd.Parameters.AddWithValue("@Remuneracion_basica", double.Parse(radTextBox16.Text));
            cmd.Parameters.AddWithValue("@Condicion_contrato", comboBox3.Text.ToString());
            cmd.Parameters.AddWithValue("@Inicio_contrato", radDateTimePicker3.Value.Date);
            cmd.Parameters.AddWithValue("@Fin_contrato", radDateTimePicker4.Value.Date);
            cmd.Parameters.AddWithValue("@Observacion", radRichTextEditor1.Text.ToString());
            try
            {
                SqlDataReader dr = cmd.ExecuteReader();

                MessageBox.Show("Contrato registrado corectamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mostrar_contratos_personal();

            }
            catch
            {
                MessageBox.Show("Error al agregar el contrato", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }



        }

        public void mostrar_contratos_personal()
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

            comando.CommandText = "exec mostrar_contratos_personal '" + radTextBox13.Text+"'";
            //especificamos que es de tipo Text
            comando.CommandType = CommandType.Text;

            //limpiamos los renglones de la datagridview
            radGridView1.Rows.Clear();
            //a la variable DataReader asignamos  el la variable de tipo SqlCommand
            dr = comando.ExecuteReader();
            //el ciclo while se ejecutará mientras lea registros en la tabla
            while (dr.Read())
            {
                //variable de tipo entero para ir enumerando los la filas del datagridview
                int renglon = radGridView1.Rows.Add();
                // especificamos en que fila se mostrará cada registro
                // nombredeldatagrid.filas[numerodefila].celdas[nombredelacelda].valor=
                // dr.tipodedatosalmacenado(dr.getordinal(nombredelcampo_en_la_base_de_datos)conviertelo_a_string_sino_es_del_tipo_string);
                radGridView1.Rows[renglon].Cells["ccontrato"].Value = dr.GetString(dr.GetOrdinal("Codigo_usuario")).ToString();
                radGridView1.Rows[renglon].Cells["column4"].Value = dr.GetString(dr.GetOrdinal("Cargo")).ToString();
                estado= dr.GetString(dr.GetOrdinal("Estado")).ToString();
                if (estado=="1")
                {
                    estado = "Activo";
                }
                else
                {
                    estado = "Inactivo";
                }
                radGridView1.Rows[renglon].Cells["column1"].Value = estado;
                radGridView1.Rows[renglon].Cells["column2"].Value = dr.GetDateTime(dr.GetOrdinal("Inicio_contrato")).ToString();
                radGridView1.Rows[renglon].Cells["column3"].Value = dr.GetString(dr.GetOrdinal("Fin_contrato")).ToString();


            }
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
                radTextBox12.Text = dr["Referencia"].ToString();
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
            }
            else
            {
                busqueda = "";
            }

            if (busqueda != "")
            {
                radTextBox1.Text = busqueda;
                MessageBox.Show("Esta persona ya esta registrada su DNI es " + busqueda + "", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                radButton4.Enabled = false;
            }
            else
            {
                radButton4.Enabled = true;
            }
            radTextBox13 = radTextBox1;
            mostrar_contratos_personal();
        }

        

        private void radButton4_Click(object sender, EventArgs e)
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

            SqlCommand cmd = new SqlCommand("registrar_personal_antiguo", conn.conn);
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
            cmd.Parameters.AddWithValue("@Referencia", radTextBox12.Text);
            cmd.Parameters.AddWithValue("@Terminal", Environment.MachineName);
            cmd.Parameters.AddWithValue("@Dni_registro", dni_login);
            try
            {
                SqlDataReader dr = cmd.ExecuteReader();

                MessageBox.Show("Datos registrados corectamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mostrar_contratos_personal();
            }
            catch
            {
                MessageBox.Show("Error al agregar los datos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
