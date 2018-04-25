using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.ComponentModel;


namespace Bilhar
{
    public partial class Caixa : PhoneApplicationPage
    {
        PopupCaixa _popupCaixa;
        private Popup _popup = new Popup();
        List<ObjCaixa> _listobjCaixa;
        ListaCaixa _listacaixa;
        private ApplicationBarIconButton btn_despesa;
        private ApplicationBarIconButton btn_receita;
        private ApplicationBarIconButton btn_ok;

        public Caixa()
        {
            InitializeComponent();
            MontaInicializaObj();
            GerBotoes(false);
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (!_popup.IsOpen)
            { base.OnBackKeyPress(e); }
            else
            {
                _popup.IsOpen = false;
                e.Cancel = true;
                GerBotoes(false);
            }
        }

        private void MontaInicializaObj()
        {
            _listobjCaixa = new List<ObjCaixa>();
            _popupCaixa = new PopupCaixa();

            ProgressOverlay.Visibility = Visibility.Visible;
            ApplicationBar.IsVisible = false;
            MontaLista();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            Persistencia.JsonCaixa = JsonConvert.SerializeObject(_listobjCaixa);
        }

        private void AbrePopup(bool despesa)
        {
            _popupCaixa = new PopupCaixa();
            //ApplicationBar.IsVisible = false;
            _popupCaixa.txt_cabecalho.Text = despesa ? "Lançar despesa" : "Lançar receita";
            //_popupCaixa.btn_ok.Click += btn_ok_Click;
            _popupCaixa.txtb_valor.Loaded += txtb_valor_Loaded;
            _popupCaixa.txtb_descricao.Loaded += txtb_descricao_Loaded;
            _popup.Child = _popupCaixa;
            _popup.IsOpen = true;
            GerBotoes(false);
        }

        void txtb_descricao_Loaded(object sender, RoutedEventArgs e)
        {
            _popupCaixa.txtb_descricao.Focus();
        }

        void txtb_valor_Loaded(object sender, RoutedEventArgs e)
        {
            _popupCaixa.LayoutRoot.Margin = new Thickness(0, -50, 0, 0);
        }

        private async void MontaLista()
        {
            if (lstbcaixa.Items.Count > 0)
                lstbcaixa.Items.Clear();

            decimal valor = 0;
            _listobjCaixa = await Task.Factory.StartNew(() => (Persistencia.JsonCaixa == string.Empty) ? new List<ObjCaixa>() : JsonConvert.DeserializeObject<List<ObjCaixa>>(Persistencia.JsonCaixa));

            foreach (var itemlist in _listobjCaixa)
            {
                _listacaixa = new ListaCaixa();
                _listacaixa.LayoutRoot.Background = (itemlist.despesa) ? new SolidColorBrush(Color.FromArgb(150, 220, 46, 46)) : new SolidColorBrush(Color.FromArgb(150, 55, 160, 64));
                _listacaixa.txt_descricao.Text = itemlist.descricao;
                _listacaixa.txt_valor.Text = itemlist.valor.ToString("c");
                _listacaixa.DataContext = itemlist;
                _listacaixa.Hold += _listacaixa_Hold;
                lstbcaixa.Items.Add(_listacaixa);
                valor += (itemlist.despesa) ? itemlist.valor * -1 : itemlist.valor;
            }
            txt_saldo.Text = "Saldo " + valor.ToString("c");
            ProgressOverlay.Visibility = Visibility.Collapsed;
            ApplicationBar.IsVisible = true;
        }

        void _listacaixa_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItemEditar = new MenuItem() { Header = "Editar", Tag = "Editar" };
            MenuItem menuItemRemover = new MenuItem() { Header = "Remover", Tag = "Remover" };
            //MenuItem menuItem3 = new MenuItem() { Header = "Cancel", Tag = "Cancel" };
            menuItemRemover.DataContext = (ObjCaixa)((System.Windows.FrameworkElement)(sender)).DataContext;
            menuItemEditar.DataContext = (ObjCaixa)((System.Windows.FrameworkElement)(sender)).DataContext;
            menuItemEditar.Tap += menuItemEditar_Tap;
            menuItemRemover.Tap += menuItemRemover_Tap;
            contextMenu.Items.Add(menuItemEditar);
            contextMenu.Items.Add(menuItemRemover);
            //contextMenu.Items.Add(menuItem3);
            ContextMenuService.SetContextMenu(this.ContentPanel, contextMenu);
        }

        void menuItemRemover_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            _listobjCaixa.Remove((ObjCaixa)((System.Windows.FrameworkElement)(sender)).DataContext);
            Atualizalista();
        }

        void menuItemEditar_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ObjCaixa _oobj = (ObjCaixa)((System.Windows.FrameworkElement)(sender)).DataContext;
            _popupCaixa = new PopupCaixa();
            _popupCaixa.txt_cabecalho.Text = _oobj.despesa ? "Lançar despesa" : "Lançar receita";
            _popupCaixa.txtb_descricao.Text = _oobj.descricao;
            _popupCaixa.txtb_valor.Text = _oobj.valor.ToString();
            _popupCaixa.txtb_valor.Loaded += txtb_valor_Loaded;
            _popup.Child = _popupCaixa;
            _listobjCaixa.Remove((ObjCaixa)((System.Windows.FrameworkElement)(sender)).DataContext);
            _popup.IsOpen = true;
            GerBotoes(true);
        }

        private void Atualizalista()
        {
            if (lstbcaixa.Items.Count > 0)
                lstbcaixa.Items.Clear();


            decimal valor = 0;

            foreach (var itemlist in _listobjCaixa)
            {
                _listacaixa = new ListaCaixa();
                _listacaixa.LayoutRoot.Background = (itemlist.despesa) ? new SolidColorBrush(Color.FromArgb(150, 220, 46, 46)) : new SolidColorBrush(Color.FromArgb(150, 55, 160, 64));
                _listacaixa.txt_descricao.Text = itemlist.descricao;
                _listacaixa.txt_valor.Text = itemlist.valor.ToString("c");
                _listacaixa.DataContext = itemlist;
                _listacaixa.Hold += _listacaixa_Hold;
                lstbcaixa.Items.Add(_listacaixa);
                valor += (itemlist.despesa) ? itemlist.valor * -1 : itemlist.valor;
            }
            txt_saldo.Text = "Saldo " + valor.ToString("c");
        }

        private bool ValidaCampos()
        {
            if (_popupCaixa.txtb_descricao.Text == "")
            { MessageBox.Show("Preencha a descrição"); _popupCaixa.txtb_descricao.Focus(); return false; }
            else if (_popupCaixa.txtb_valor.Text == "")
            { _popupCaixa.txtb_valor.Focus(); return false; }
            else
                return true;
        }

        private void btn_despesa_Click(object sender, EventArgs e)
        {
            AbrePopup(true);
            GerBotoes(true);
        }

        private void btn_receita_Click(object sender, EventArgs e)
        {
            AbrePopup(false);
            GerBotoes(true);
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (ValidaCampos())
            {
                ObjCaixa _obj = new ObjCaixa(decimal.Parse(_popupCaixa.txtb_valor.Text), (_popupCaixa.txt_cabecalho.Text == "Lançar despesa") ? true : false, _popupCaixa.txtb_descricao.Text, DateTime.Now);
                _listobjCaixa.Add(_obj);

                _popup.IsOpen = false;
                ApplicationBar.IsVisible = true;
                Atualizalista();
                GerBotoes(false);
            }

        }

        private void GerBotoes(bool popup)
        {
            this.ApplicationBar.Buttons.Clear();
            this.ApplicationBar.MenuItems.Clear();
            if (!popup)
            {
                this.btn_despesa = new ApplicationBarIconButton();
                this.btn_despesa.IconUri = new Uri("/Assets/AppBar/appbar.cabinet.out.png", UriKind.RelativeOrAbsolute);
                this.btn_despesa.Text = "despesa";
                this.btn_despesa.Click += new EventHandler(btn_despesa_Click);
                this.ApplicationBar.Buttons.Add(this.btn_despesa);
                this.btn_receita = new ApplicationBarIconButton();
                this.btn_receita.IconUri = new Uri("/Assets/AppBar/appbar.cabinet.in.png", UriKind.RelativeOrAbsolute);
                this.btn_receita.Text = "Receita";
                this.btn_receita.Click += new EventHandler(btn_receita_Click);
                this.ApplicationBar.Buttons.Add(this.btn_receita);
            }
            else
            {
                this.btn_ok = new ApplicationBarIconButton();
                this.btn_ok.IconUri = new Uri("/Assets/appbar.check.png", UriKind.RelativeOrAbsolute);
                this.btn_ok.Text = "ok";
                this.btn_ok.Click += new EventHandler(btn_ok_Click);
                this.ApplicationBar.Buttons.Add(this.btn_ok);
            }

            //this.btn_gambis = new ApplicationBarMenuItem();
            //this.btn_gambis.Text = "sincronizar";
            //this.btn_gambis.Click += new EventHandler(btn_gambis_click);
            //this.ApplicationBar.MenuItems.Add(this.btn_gambis);

            //this.btn_caixa = new ApplicationBarMenuItem();
            //this.btn_caixa.Text = "Caixa";
            //this.btn_caixa.Click += new EventHandler(btn_caixa_click);
            //this.ApplicationBar.MenuItems.Add(this.btn_caixa);

            //this.btn_representante = new ApplicationBarMenuItem();
            //this.btn_representante.Text = "representante";
            //this.btn_representante.Click += new EventHandler(btn_representante_click);
            //this.ApplicationBar.MenuItems.Add(this.btn_representante);
        }
    }
}