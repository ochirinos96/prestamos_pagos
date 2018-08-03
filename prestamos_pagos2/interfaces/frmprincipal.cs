using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace prestamos_pagos2.interfaces
{
    public partial class frmprincipal : Telerik.WinControls.UI.RadRibbonForm
    {
        public string dni_login;
        public string permisos;
        public frmprincipal()
        {
            InitializeComponent();
        }
        public frmprincipal(string dni_l,string per)
        {
            
            InitializeComponent();
            dni_login = dni_l;
            permisos = per;
        }

        private void radRibbonBarGroup1_Click(object sender, EventArgs e)
        {
            
        }

        private void radButtonElement1_Click(object sender, EventArgs e)
        {
            frm_registro_cliente cliente = new frm_registro_cliente(dni_login);
            cliente.MdiParent = this;
            cliente.Show();
        }

        private void radButtonElement2_Click(object sender, EventArgs e)
        {
            frm_gestion_cliente cliente = new frm_gestion_cliente(dni_login);
            cliente.MdiParent = this;
            cliente.Show();
        }

        private void radButtonElement3_Click(object sender, EventArgs e)
        {
            frm_registro_negocio cliente = new frm_registro_negocio(dni_login);
            cliente.MdiParent = this;
            cliente.Show();
        }

        private void radButtonElement4_Click(object sender, EventArgs e)
        {
            frm_registro_articulos cliente = new frm_registro_articulos(dni_login);
            cliente.MdiParent = this;
            cliente.Show();
        }

        private void radButtonElement5_Click(object sender, EventArgs e)
        {
            
        }

        private void frmprincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void radButtonElement6_Click(object sender, EventArgs e)
        {
            frm_registro_personal personal = new frm_registro_personal(dni_login);
            personal.MdiParent = this;
            personal.Show();
        }

        private void radButtonElement7_Click(object sender, EventArgs e)
        {
            frm_gestion_personal personal = new frm_gestion_personal(dni_login);
            personal.MdiParent = this;
            personal.Show();
        }

        private void radButtonElement8_Click(object sender, EventArgs e)
        {
            frm_gestion_articulos personal = new frm_gestion_articulos(dni_login);
            personal.MdiParent = this;
            personal.Show();
        }

        private void radButtonElement5_Click_1(object sender, EventArgs e)
        {
            frm_gestion_contrato personal = new frm_gestion_contrato(dni_login);
            personal.MdiParent = this;
            personal.Show();
        }
    }
}
