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
    public partial class frm_gestion_personal : Telerik.WinControls.UI.RadForm
    {
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

        }
    }
}
