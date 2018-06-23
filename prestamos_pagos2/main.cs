using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using prestamos_pagos2.datos;
using prestamos_pagos2.interfaces;

namespace prestamos_pagos2
{
    public partial class main : Telerik.WinControls.UI.RadForm
    {
        public main()
        {
            InitializeComponent();
        }
        
        private void radButton1_Click(object sender, EventArgs e)
        {

            string usuario, contrasenia,codigo, nivel_acceso,estado;
            usuario = radTextBox1.Text;
            contrasenia = radTextBox2.Text;

            coneccion conn = new coneccion();
            if (ConnectionState.Closed == conn.conn.State)
            {
                conn.conn.Open();
            }

            SqlCommand cmd = new SqlCommand("verificar_usuario", conn.conn);
           cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Parameters.AddWithValue("@contrasenia", contrasenia);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                codigo = dr["Dni"].ToString();
                nivel_acceso = dr["permisos"].ToString();
                estado = dr["Estado"].ToString();
            }
            else
            {
                codigo = "";
                estado = "";
                nivel_acceso = "";
            }

            if (codigo == "")
            {
                MessageBox.Show("Corrija sus datos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            else if (estado == "0")
            {
                MessageBox.Show("Usuario inhabilitado, consulte con el administrador", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {

                frmprincipal principal = new frmprincipal(codigo, nivel_acceso);
                principal.Show();
                this.Hide();


            }


        }

        private void chkmostrarPass_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }
    }
}
