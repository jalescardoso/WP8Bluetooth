using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Phone.Shell;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Bilhar
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ApplicationBarIconButton btn_confirmar;
        private ApplicationBarIconButton btn_recebimento;
        private ApplicationBarIconButton btn_locacao;
        //private ApplicationBarMenuItem btn_reset;
        private ApplicationBarMenuItem btn_sincronizar;
        private ApplicationBarMenuItem btn_representante;
        private ApplicationBarMenuItem btn_caixa;
        BI_mesa _obj;
        string _objMovimento;
        List<BI_MovimentoMesa> _listMov;
        List<BI_mesa> _listMesas;
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            while (NavigationService.BackStack.Any())
            {
                NavigationService.RemoveBackEntry();

            }
        }

        public MainPage()
        {
            InitializeComponent();
            CarregaListas();
            
        }

        private async void CarregaListas()
        {
            this.Focus();
            ProgressOverlay.Visibility = Visibility.Visible;
            ApplicationBar.IsVisible = false;
            _listMov = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<BI_MovimentoMesa>>(Persistencia.JsonMovimentoMesas));
            _listMesas = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<BI_mesa>>(Persistencia.JsonMesas));
            ProgressOverlay.Visibility = Visibility.Collapsed;
            ApplicationBar.IsVisible = true;
            if (txtb_pesquisa.IsHitTestVisible)
            {
                txtb_pesquisa.Focus();
            }
            
        }

        private void btn_sincronizar_click(object sender, EventArgs e)
        {
            try
            {
                this.Focus();
                ProgressOverlay.Visibility = Visibility.Visible;
                ApplicationBar.IsVisible = false;
                Dispatcher.BeginInvoke(() => UploadDados());
            }
            catch
            {
                var result = MessageBox.Show("Processo não executado. deseja tentar novamente ?", "Bilhar", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                { ProgressOverlay.Visibility = Visibility.Collapsed; Dispatcher.BeginInvoke(() => UploadDados()); ApplicationBar.IsVisible = false; }
            }
        }

        private async void UploadDados()
        {
            try
            {
                List<Sincronismo> _listSincronismo = JsonConvert.DeserializeObject<List<Sincronismo>>(Persistencia.JsonSincronismo);
                if (_listSincronismo == null)
                    _listSincronismo = new List<Sincronismo>();
                bool cmdexecutado = false;
                foreach (var obj in _listSincronismo)
                {
                    if (obj.sincronizado_servidor == default(DateTime))
                    {
                        foreach (var filtro in _listMov.Where(c => c.id_mesa == obj.id_mesa && c.data_Movimento != default(DateTime)))
                        {
                            using (HttpClient client = new HttpClient())
                            {
                                string json = JsonConvert.SerializeObject(filtro, Formatting.Indented);
                                HttpContent content = new StringContent(json);
                                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                                var result = await client.PostAsync("http://api" + obj.origem, content);
                                if (result.StatusCode == HttpStatusCode.Created)
                                {
                                    var resultado = JsonConvert.DeserializeObject<BI_MovimentoMesa>(result.Content.ReadAsStringAsync().Result);
                                    obj.sincronizado_servidor = DateTime.Now;
                                    filtro.data_Movimento = default(DateTime);
                                    filtro.id_sincronismo = resultado.id_sincronismo;
                                    cmdexecutado = true;
                                    //BI_MovimentoMesa json2 = JsonConvert.DeserializeObject<BI_MovimentoMesa>(result.Content.ReadAsStringAsync().Result);
                                    //_listSincronismo.Remove(obj);
                                    //_listSincronismo.Add(new Sincronismo(obj.id_mesa, obj.data_movimento, DateTime.Now));
                                }
                                //result.EnsureSuccessStatusCode();
                            }
                        }
                    }
                }
                Persistencia.JsonMovimentoMesas = JsonConvert.SerializeObject(_listMov, Formatting.Indented);
                Persistencia.JsonSincronismo = JsonConvert.SerializeObject(_listSincronismo, Formatting.Indented);
                ProgressOverlay.Visibility = Visibility.Collapsed;
                //MessageBox.Show("Processamento realizado com sucesso", "Bilhar", MessageBoxButton.OK);
                ApplicationBar.IsVisible = true;
                if(cmdexecutado)
                    CarregaListas();
                DownloadSincronismos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ProgressOverlay.Visibility = Visibility.Collapsed;
                ApplicationBar.IsVisible = true;
            }
        }

        private async void DownloadSincronismos()
        {
            
            _listMesas.Sort((x, y) => y.id_sincronismo.CompareTo(x.id_sincronismo));
            _listMov.Sort((x, y) => y.id_sincronismo.CompareTo(x.id_sincronismo));
            int maior = _listMesas[0].id_sincronismo > _listMov[0].id_sincronismo ? _listMesas[0].id_sincronismo : _listMov[0].id_sincronismo;
            bool cmdexecutado = false;
            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetAsync("http://api" + maior.ToString() + "&pOperador=>");
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    JArray resultado = JArray.Parse(result.Content.ReadAsStringAsync().Result);
                    foreach (var obj in resultado)
                    {
                        var variaveis = obj["integracao"].ToString().Split('|');
                        string end = "http://api" + variaveis[0].ToString() + "&pData=" + variaveis[2].ToString();
                        using (HttpClient client2 = new HttpClient())
                        {
                            var result2 = await client2.GetAsync(end);
                            if (result2.StatusCode == HttpStatusCode.OK)
                            {
                                var resultado2 = JsonConvert.DeserializeObject<BI_MovimentoMesa>(result2.Content.ReadAsStringAsync().Result);
                                _listMov.Add(resultado2);
                                cmdexecutado = true;
                            }
                            //result.EnsureSuccessStatusCode();
                        }
                    }
                }
                //result.EnsureSuccessStatusCode();
            }
            if(cmdexecutado)
                CarregaListas();
        }

        private void btn_confirmar_Click(object sender, EventArgs e)
        {
            this.Focus();
            pgrs_mainpage.Visibility = Visibility.Visible;
            Dispatcher.BeginInvoke(() => PesquisaMesa());
            MontaBotoes(true);
            
        }

        private void btn_locacao_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Locacao/CadLocacao.xaml", UriKind.Relative));

        }

        private void MontaBotoes(bool confirma)
        {
            this.ApplicationBar.Buttons.Clear();
            this.ApplicationBar.MenuItems.Clear();
            if (confirma)
            {
                this.btn_recebimento = new ApplicationBarIconButton();
                this.btn_recebimento.IconUri = new Uri("/Assets/appbar.money.png", UriKind.RelativeOrAbsolute);
                this.btn_recebimento.Text = "Recebimento";
                this.btn_recebimento.Click += new EventHandler(btn_recebimento_Click);
                this.ApplicationBar.Buttons.Add(this.btn_recebimento);

                this.btn_locacao = new ApplicationBarIconButton();
                this.btn_locacao.IconUri = new Uri("/Assets/appbar.arrow.right.left.png", UriKind.RelativeOrAbsolute);
                this.btn_locacao.Text = "locação";
                this.btn_locacao.Click += new EventHandler(btn_locacao_Click);
                this.ApplicationBar.Buttons.Add(this.btn_locacao);
            }
            else
            {
                this.btn_confirmar = new ApplicationBarIconButton();
                this.btn_confirmar.IconUri = new Uri("/Assets/appbar.check.png", UriKind.RelativeOrAbsolute);
                this.btn_confirmar.Text = "Selecionar";
                this.btn_confirmar.Click += new EventHandler(btn_confirmar_Click);
                this.ApplicationBar.Buttons.Add(this.btn_confirmar);
            }

            this.btn_sincronizar = new ApplicationBarMenuItem();
            this.btn_sincronizar.Text = "sincronizar";
            this.btn_sincronizar.Click += new EventHandler(btn_sincronizar_click);
            this.ApplicationBar.MenuItems.Add(this.btn_sincronizar);

            this.btn_caixa = new ApplicationBarMenuItem();
            this.btn_caixa.Text = "Caixa";
            this.btn_caixa.Click += new EventHandler(btn_caixa_click);
            this.ApplicationBar.MenuItems.Add(this.btn_caixa);

            this.btn_representante = new ApplicationBarMenuItem();
            this.btn_representante.Text = "representante";
            this.btn_representante.Click += new EventHandler(btn_representante_click);
            this.ApplicationBar.MenuItems.Add(this.btn_representante);

            //this.btn_reset = new ApplicationBarMenuItem();
            //this.btn_reset.Text = "reset";
            //this.btn_reset.Click += new EventHandler(btn_reset_click);
            //this.ApplicationBar.MenuItems.Add(this.btn_reset);
        }

        private void btn_recebimento_Click(object sender, EventArgs e)
        {
            if (lstb_dados.Items.Count > 0)
            {
                string mesa = JsonConvert.SerializeObject(_obj, Formatting.Indented);
                NavigationService.Navigate(new Uri("/Recebimento/Registro.xaml?cod=" + mesa + "&movimento=" + _objMovimento, UriKind.Relative));
            }
        }

        private void PesquisaMesa()
        {
            lstb_dados.Items.Clear();
            try
            {
                _obj = _listMesas.Find(c => c.id == int.Parse(txtb_pesquisa.Text));
                DadosMesa _dados = new DadosMesa();
                _dados.txt_nome.Text = _obj.nome;
                _dados.txt_regiao.Text = Swifts.Regiao(_obj.id_regiao);
                _dados.txt_tipo.Text = Swifts.TipoMesa(_obj.id_tipo_mesa);
                _dados.txt_cidade.Text = _obj.cidade;
                _dados.txt_endereco.Text = _obj.endereco;
                _dados.txt_bairro.Text = _obj.bairro;
                _dados.txt_cidade.Text = _obj.cidade;
                _dados.txt_estado.Text = _obj.uf;
                _dados.txt_cep.Text = _obj.cep;
                _dados.txt_telefone.Text = _obj.telefone;
                _dados.txt_celular.Text = _obj.celular;
                _dados.txt_cpf.Text = _obj.cpf;
                _dados.txt_cnpj.Text = _obj.cnpj;
                _dados.txt_inscEstadual.Text = _obj.inscricao_estadual;
                _dados.txt_rg.Text = _obj.identidade;
                _dados.txt_datanasc.Text = _obj.data_nascimento;
                //_dados.txt_observacao.Text = _obj.observacao;
                lstb_dados.Items.Add(_dados);
                pgrs_mainpage.Visibility = Visibility.Collapsed;
                Dispatcher.BeginInvoke(() =>
                {
                    BI_MovimentoMesa _obj2;
                    var _objseilaoq = _listMov.Where(c => c.id_mesa == _obj.id);
                    if (_objseilaoq.Count() > 0)
                    {
                        BI_MovimentoMesa filtro = null;
                        foreach (var myobj in _objseilaoq)
                        {
                            if (filtro == null || filtro.numero_registro_atual < myobj.numero_registro_atual)
                                filtro = (BI_MovimentoMesa)myobj;
                        }
                        _obj2 = filtro;
                    }
                    else { _obj2 = (BI_MovimentoMesa)_objseilaoq; }
                    _objMovimento = JsonConvert.SerializeObject(_obj2, Formatting.Indented);
                });
            }
            catch { MessageBox.Show("Mesa não encontrada", "Erro", MessageBoxButton.OK); pgrs_mainpage.Visibility = Visibility.Collapsed; }
        }

        private void txtb_pesquisa_GotFocus(object sender, RoutedEventArgs e)
        {
            MontaBotoes(false);
        }

        private void btn_representante_click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Representante.xaml", UriKind.Relative));
        }

        private void btn_caixa_click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Caixa.xaml", UriKind.Relative));
        }

        private void txtb_pesquisa_LostFocus(object sender, RoutedEventArgs e)
        {
            MontaBotoes(true);
        }
    }
}