using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Bilhar.usercontrool;
using System.Linq.Expressions;

namespace Bilhar.Locacao
{
    public partial class CadLocacao : PhoneApplicationPage
    {
        FormLocMesa _formLocMesa;
        public class objlocação
        {
            public int id;
            public string nome;
            public objlocação(int _id, string _nome)
            {
                id = _id;
                nome = _nome;
            }
            public static List<objlocação> MontaRegioes()
            {
                List<objlocação> retorno = new List<objlocação>();
                retorno.Add(new objlocação(1, "GOIÂNIA"));
                retorno.Add(new objlocação(2, "NORDESTE GOIANO"));
                retorno.Add(new objlocação(3, "BAHIA"));
                retorno.Add(new objlocação(4, "PIAUÍ"));
                retorno.Add(new objlocação(7, "BAHIA 4"));
                retorno.Add(new objlocação(8, "APARECIDA DE GOIÂNIA"));
                retorno.Add(new objlocação(9, "MESA NO DEPÓSITO PARA LOCAÇÃO"));
                retorno.Add(new objlocação(10, "MESA DESMANCHADA"));
                retorno.Add(new objlocação(20, "PIAUI 2"));
                retorno.Add(new objlocação(22, "BAHIA 5"));
                retorno.Add(new objlocação(23, "MESAS PROCURADAS"));
                retorno.Add(new objlocação(24, "MESA DEPOSITO P/ PAULO AUGUSTO"));
                retorno.Add(new objlocação(25, "NÃO EXISTE NO LOCAL PIAUI"));
                retorno.Add(new objlocação(26, "PIAUI FICHA DEPOSITO CORRENTE"));
                retorno.Add(new objlocação(27, "DEPOSITO CORRENTE POR PAULO"));
                return retorno;
            }

            public static List<objlocação> MontaTamanhos()
            {
                List<objlocação> retorno = new List<objlocação>();
                retorno.Add(new objlocação(2, "GRANDE"));
                retorno.Add(new objlocação(1, "MÉDIA"));
                retorno.Add(new objlocação(243, "MESA DE PEDRA"));
                retorno.Add(new objlocação(3, "PEQUENA"));
                return retorno;
            }
        }
        public CadLocacao()
        {
            InitializeComponent();
            _formLocMesa = new FormLocMesa();
            _formLocMesa.KeyUp += _formLocMesa_KeyUp;
            _formLocMesa.ltpk_regiao.Items.Add("--selecione--");
            _formLocMesa.ltpk_tipo.Items.Add("--selecione--");

            _formLocMesa.ltpk_tipo.DataContext = objlocação.MontaTamanhos();
            _formLocMesa.ltpk_regiao.DataContext = objlocação.MontaRegioes();
            foreach (var regiao in objlocação.MontaRegioes())
                _formLocMesa.ltpk_regiao.Items.Add(regiao.nome);
            foreach (var tamanho in objlocação.MontaTamanhos())
                _formLocMesa.ltpk_tipo.Items.Add(tamanho.nome);
            
            _formLocMesa.ltpk_regiao.SelectedIndex = 0;
            _formLocMesa.ltpk_tipo.SelectedIndex = 0;
            Lstbcadloc.Items.Add(_formLocMesa);

        }

        private bool ValidaCampos()
        {
            if (_formLocMesa.txtb_codigo.Text == "")
            {
                MessageBox.Show("Informe o código da mesa");
                _formLocMesa.txtb_codigo.Focus();
                return false;
            }
            else if (_formLocMesa.ltpk_tipo.SelectedIndex == 0)
            {
                MessageBox.Show("Informe o tipo de mesa");
                _formLocMesa.ltpk_tipo.Focus();
                return false;
            }
            else if (_formLocMesa.ltpk_regiao.SelectedIndex == 0)
            {
                MessageBox.Show("Informe a região");
                _formLocMesa.ltpk_regiao.Focus();
                return false;
            }
            else if (_formLocMesa.txtb_nome.Text == "")
            {
                MessageBox.Show("Informe o nome");
                _formLocMesa.txtb_nome.Focus();
                return false;
            }
            else if (_formLocMesa.txtb_telefone.Text == "")
            {
                MessageBox.Show("Informe o telefone");
                _formLocMesa.txtb_telefone.Focus();
                return false;
            }
            else if (_formLocMesa.txtb_celular.Text == "")
            {
                MessageBox.Show("Informe o celular");
                _formLocMesa.txtb_celular.Focus();
                return false;
            }
            else if (_formLocMesa.txtb_CEP.Text == "")
            {
                MessageBox.Show("Informe o CEP");
                _formLocMesa.txtb_CEP.Focus();
                return false;
            }
            else if (_formLocMesa.txtb_endereco.Text == "")
            {
                MessageBox.Show("Informe o endereço");
                _formLocMesa.txtb_endereco.Focus();
                return false;
            }
            else if (_formLocMesa.txtb_bairro.Text == "")
            {
                MessageBox.Show("Informe o bairro");
                _formLocMesa.txtb_bairro.Focus();
                return false;
            }
            else if (_formLocMesa.txtb_cidade.Text == "")
            {
                MessageBox.Show("Informe a cidade");
                _formLocMesa.txtb_cidade.Focus();
                return false;
            }
            else if (_formLocMesa.txtb_estado.Text == "")
            {
                MessageBox.Show("Informe CPF");
                _formLocMesa.txtb_estado.Focus();
                return false;
            }
            else if (_formLocMesa.txtb_CNPJ.Text == "")
            {
                MessageBox.Show("Informe CNPJ");
                _formLocMesa.txtb_CNPJ.Focus();
                return false;
            }
            else if (_formLocMesa.txtb_insEstadual.Text == "")
            {
                MessageBox.Show("Informe a inscrição estadual");
                _formLocMesa.txtb_insEstadual.Focus();
                return false;
            }
            else if (_formLocMesa.txtb_RG.Text == "")
            {
                MessageBox.Show("Informe o RG");
                _formLocMesa.txtb_RG.Focus();
                return false;
            }
            else
            {
                return true;
            }

        }

        void _formLocMesa_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.PlatformKeyCode == 13)
            {

            }
        }

        private void PreencheListPiker()
        {

            //_formLocMesacurrentUICulture_listpicker.ItemsSource = this._UICultureList;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (ValidaCampos())
            {
                int idTamanho = ((List<objlocação>)this._formLocMesa.ltpk_tipo.DataContext).Where(o => o.nome == this._formLocMesa.ltpk_tipo.Header).First().id;
                //Locacao obj = new Locacao(0, int.Parse(_formLocMesa.txtb_codigo.Text), DateTime.Parse(_formLocMesa.txtb_datalocacao.ToString()), 

            }
        }


    }
}