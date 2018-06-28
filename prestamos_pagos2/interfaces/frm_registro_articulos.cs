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

            SqlCommand cmd = new SqlCommand("registro_articulo", conn.conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@dni_cliente",radTextBox4.Text.ToString());
            cmd.Parameters.AddWithValue("@Codigo_articulo", radTextBox7.Text.ToString());
            cmd.Parameters.AddWithValue("@N_serie", radTextBox1.Text.ToString());
            cmd.Parameters.AddWithValue("@Nombre", radTextBox2.Text.ToString());
            cmd.Parameters.AddWithValue("@V_contable", radTextBox3.Text.ToString());
            cmd.Parameters.AddWithValue("@F_compra", radDateTimePicker2.Text.ToString());
            cmd.Parameters.AddWithValue("@Observaciones", radTextBox5.Text.ToString());
            cmd.Parameters.AddWithValue("@estado_art", radTextBox8.Text.ToString());
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
            textBox1.Text = examinar.FileName;
            pictureBox1.Image = Image.FromFile(examinar.FileName);
        }

        private void radTextBox9_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
