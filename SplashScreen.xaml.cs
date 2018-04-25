using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace Bilhar
{
    public partial class SplashScreen : PhoneApplicationPage
    {
        bool verf = false;

        public SplashScreen()
        {
            InitializeComponent();
            SolicitaAutorizacaoLocalizacao();
            //Persistencia.ZerarTudo();
            if(Persistencia.JsonMesas == "")
            IniciaDownload("");
            if(Persistencia.JsonMovimentoMesas == "")
            IniciaDownload2("");
            if(Persistencia.JsonMovimentoMesas != "" && Persistencia.JsonMesas != "")
                Dispatcher.BeginInvoke(() =>NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative)));
        }

        public void IniciaDownload(string downloadUrl)
        {
            txt_informacao.Text = "";
            if (Persistencia.JsonMesas == "")
            {
                txt_informacao.Text = "Starting Download.";
                WebClient webClient = new WebClient();
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
                webClient.DownloadStringAsync(new Uri(downloadUrl));

                txt_informacao.Text = "Downloading source from " + downloadUrl;
                txt_informacao.Text = string.Empty;
            }
            else
            {
                txt_informacao.Text = string.Empty;
            }
        }

        void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            txt_informacao.Text = "Downloaded " + e.BytesReceived + "/" + e.TotalBytesToReceive + "bytes, " + e.ProgressPercentage + "% completed.";
        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                txt_informacao.Text = "Download cancelled.";
                VerificaDownload();
                return;
            }

            if (e.Error != null)
            {
                txt_informacao.Text = "Download error.";
                VerificaDownload();
                return;
            }
            Persistencia.JsonMesas = e.Result;
            txt_informacao.Text = "Download completed.";
            VerificaDownload();
        }

        public void IniciaDownload2(string downloadUrl)
        {
            txt_informacaoMovMesa.Text = "";
            if (Persistencia.JsonMovimentoMesas == "")
            {
                txt_informacaoMovMesa.Text = "Starting Download.";
                WebClient webClient = new WebClient();
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted2);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged2);
                webClient.DownloadStringAsync(new Uri(downloadUrl));

                txt_informacaoMovMesa.Text = "Downloading source from " + downloadUrl;
                txt_informacaoMovMesa.Text = string.Empty;
            }
            else
            {
                txt_informacaoMovMesa.Text = string.Empty;
            }
        }

        void webClient_DownloadProgressChanged2(object sender, DownloadProgressChangedEventArgs e)
        {
            txt_informacaoMovMesa.Text = "Downloaded " + e.BytesReceived + "/" + e.TotalBytesToReceive + "bytes, " + e.ProgressPercentage + "% completed.";
        }

        void webClient_DownloadStringCompleted2(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                txt_informacaoMovMesa.Text = "Download cancelled.";
                VerificaDownload();
                return;
            }

            if (e.Error != null)
            {
                txt_informacaoMovMesa.Text = "Download error.";
                VerificaDownload();
                return;
            }
            Persistencia.JsonMovimentoMesas = e.Result;
            txt_informacaoMovMesa.Text = "Download completed.";
            VerificaDownload();
        }

        void VerificaDownload()
        {
            if (Persistencia.JsonMesas != "" && Persistencia.JsonMovimentoMesas != "")
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            else if (verf)
            {
                
                if (Persistencia.JsonMesas == "" || Persistencia.JsonMovimentoMesas == "")
                {
                    var result = MessageBox.Show("ouve um problema com o sincronismo. Deseja tentar novamente?", "Erro", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        if (Persistencia.JsonMesas == "")
                            IniciaDownload("");
                        if (Persistencia.JsonMovimentoMesas == "")
                            IniciaDownload2("");
                    }
                    else
                        NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }
            }
            else { verf = true; }

        }

        private void SolicitaAutorizacaoLocalizacao()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("LocationConsent"))
            {
                //User already gave us his agreement for using his position
                if ((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"] == true)

                    return;
                //If he didn't we ask for it
                else
                {
                    MessageBoxResult result =
                                MessageBox.Show("Bilhar deseja utilizar sua localização",
                                "Location",
                                MessageBoxButton.OKCancel);

                    if (result == MessageBoxResult.OK)
                    {
                        IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = true;
                    }
                    else
                    {
                        IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = false;
                    }

                    IsolatedStorageSettings.ApplicationSettings.Save();
                }
            }

                //Ask for user agreement in using his position
            else
            {
                MessageBoxResult result =
                            MessageBox.Show("Bilhar deseja utilizar sua localização",
                            "Location",
                            MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = true;
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = false;
                }

                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }
    }
}