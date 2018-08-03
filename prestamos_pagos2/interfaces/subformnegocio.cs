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
    public partial class subformnegocio : Telerik.WinControls.UI.RadForm
    {
        public subformnegocio()
        {
            InitializeComponent();
        }
        public string dni_cliente;
        public subformnegocio(string dni)
        {
            InitializeComponent();
            dni_cliente = dni;
            mostrar_negocios();

        }

        public void mostrar_negocios()
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
            comando.CommandText = "exec busqueda_negocio_uso '" + dni_cliente + "'";
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



        public string var1;
        public string var2;

        private void radButton1_Click(object sender, EventArgs e)
        {
            var1= this.dataGridView2.CurrentRow.Cells[0].Value.ToString();
            var2 = "PRESTAMO";
        }
    }
}
