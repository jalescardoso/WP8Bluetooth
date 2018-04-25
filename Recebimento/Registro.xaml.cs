using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bilhar.Recebimento
{
    public partial class Registro : PhoneApplicationPage
    {
        BI_MovimentoMesa _objMov;
        BI_mesa _objMesa;
        bool fechateclado = false;

        public Registro()
        {
            InitializeComponent();
        }

        private void btn_confirma_Click(object sender, EventArgs e)
        {
            if (fechateclado)
            {
                string _obj2 = JsonConvert.SerializeObject(_objMov, Formatting.Indented);
                string _obj3 = JsonConvert.SerializeObject(_objMesa, Formatting.Indented);
                NavigationService.Navigate(new Uri("/Recebimento/Pagamento.xaml?objMov=" + _obj2 + "&reg=" + txtb_regAtual.Text + "&objM=" + _obj3, UriKind.Relative));
            }
            else
                this.Focus();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string msg;
            string msg2;
            if (NavigationContext.QueryString.TryGetValue("cod", out msg))
            {
                _objMesa = JsonConvert.DeserializeObject<BI_mesa>(msg);
            }
            if (NavigationContext.QueryString.TryGetValue("movimento", out msg2))
            {
                _objMov = JsonConvert.DeserializeObject<BI_MovimentoMesa>(msg2);
            }
            Prgs_registro.Visibility = Visibility.Visible;
            Dispatcher.BeginInvoke(() => PreencheTela(msg));
        }

        private void PreencheTela(string json)
        {

            txt_regAnterior.Text = _objMov.numero_registro_atual.ToString();
            Prgs_registro.Visibility = Visibility.Collapsed;
        }

        private void txtb_regAtual_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtb_regAtual.Text != "")
            {
                txt_totalpartidas.Text = (int.Parse(txtb_regAtual.Text) - _objMov.numero_registro_atual).ToString();
                txt_desconto.Text = ((int.Parse(txtb_regAtual.Text) - _objMov.numero_registro_atual) * (_objMesa.desconto / 100)).ToString("#0");
                txt_valorpartida.Text = "R$ " + _objMesa.valor_partida.ToString("#0.00");
                txt_totalbruto.Text = "R$ " + ((((int.Parse(txtb_regAtual.Text) - _objMov.numero_registro_atual) - Convert.ToInt16(txt_desconto.Text))) * _objMov.valor_partida).ToString("#0.00");
                txt_percRecebimento.Text = _objMesa.percentual_recebimento.ToString("#00") + "%";
            }
            if (int.Parse(txtb_regAtual.Text) < int.Parse(txt_regAnterior.Text))
                MessageBox.Show("o registro atual tem de ser superior ao anterior", "Bilhar", MessageBoxButton.OK);
            else
                fechateclado = true;
        }
    }
}