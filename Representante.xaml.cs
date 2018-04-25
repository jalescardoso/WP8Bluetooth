using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Bilhar
{
    public partial class Representante : PhoneApplicationPage
    {
        public Representante()
        {
            InitializeComponent();
            txt_dinamico.Text = "SENHA:";
            txt_usuarioAtual.Text = "";
        }

        private void Senha()
        {
            
            if (txtb_tela.Text == "753")
                AlteraRepresentante();
            else
                MessageBox.Show("Senha incorreta");
        }
        private void AlteraRepresentante()
        {
            this.Focus();
            txtb_tela.Text = "";
            txt_dinamico.Text = "Insira nome do representante";
            txt_usuarioAtual.Text = "Usuário atual: " + Persistencia.Representante;
        }

        private void txtb_tela_Loaded(object sender, RoutedEventArgs e)
        {
            txtb_tela.Focus();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (txt_usuarioAtual.Text == "")
            { Senha(); }
            else
            { Persistencia.Representante = txtb_tela.Text; MessageBox.Show("salvo com sucesso"); NavigationService.GoBack(); }
        }
    }
}