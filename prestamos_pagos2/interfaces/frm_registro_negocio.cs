using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace prestamos_pagos2.interfaces
{
    public partial class frm_registro_negocio : Telerik.WinControls.UI.RadForm
    {
        public string dni_login;
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

        }

        private void radButton2_Click(object sender, EventArgs e)
        {

        }

        private void radTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
