using Bilhar.Resources;
using Datecs.Api.IO;
using Datecs.Api.Printer;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
//REMEMBER: For make your app works always add this 3 using statement
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using Windows.Devices.Geolocation;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;



namespace Bilhar.Recebimento
{
    public partial class Pagamento : PhoneApplicationPage
    {
        BI_MovimentoMesa _mov;
        int regAtual;
        BI_mesa _mesa;
        decimal latitude;
        decimal longitude;
        bool fechateclado = false;
        string desconto;
        private Popup _popup = new Popup();
        PopupBluetooth _popupBluetooth;
        DateTime dataPagamentoAnterior;
        string conexaobluetooh = string.Empty;

        public Pagamento()
        {
            InitializeComponent();
            Persistencia.NumerodoRecibo++;
        }

        private void IniciaBluetooth()
        {
            _popupBluetooth = new PopupBluetooth();
            _popupBluetooth.PrinterList.Loaded += PrinterList_Loaded;
            CapturaCoordenadas();
        }

        private void btn_confirma_Click(object sender, EventArgs e)
        {

            if (fechateclado)
            {
                Prgs_pagamento.Visibility = Visibility.Visible;
                ApplicationBar.IsVisible = false;
                if (txtb_valorPago.Text == "")
                { txtb_valorPago.Text = "0"; txt_valorRestante.Text = txt_totalPagar.Text; }
                
                _popupBluetooth.btn_Cancelar.Click += btn_Cancelar_Click;
                _popupBluetooth.btn_conectar.Click += btn_conectar_Click;
                _popup.Child = _popupBluetooth;
                _popup.IsOpen = true;
                ShowPrinterSelectionDialog();

                

                //SalvoBanco();
            }
            else
            {
                this.Focus();
                _popupBluetooth.btn_Cancelar.Click += btn_Cancelar_Click;
                _popupBluetooth.btn_conectar.Click += btn_conectar_Click;
                _popup.Child = _popupBluetooth;
                _popup.IsOpen = true;
            }
        }

        private void PrinterList_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowPrinterSelectionDialog();
        }

        void btn_conectar_Click(object sender, RoutedEventArgs e)
        {
            
            ApplicationBar.IsVisible = false;
            Connect();
        }

        void btn_Cancelar_Click(object sender, RoutedEventArgs e)
        {
               var resposta =  MessageBox.Show("Deseja salvar movimento sem impressao", "Bilhar", MessageBoxButton.OKCancel);
               if (resposta == MessageBoxResult.OK)
               {
                   _popup.IsOpen = false;
                   Prgs_pagamento.Visibility = Visibility.Visible;
                   ApplicationBar.IsVisible = false;
                   Dispatcher.BeginInvoke(() => SalvoBanco());
               }
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (!_popup.IsOpen)
            { base.OnBackKeyPress(e); }
            else
            {
                _popup.IsOpen = false;
                fechateclado = false;
                e.Cancel = true;
            }
        }

        private void SalvoBanco()
        {
            
            if (txtb_valorPago.Text == "")
            { txtb_valorPago.Text = "0"; txt_valorRestante.Text = txt_totalPagar.Text; }
            BI_MovimentoMesa movimento = new BI_MovimentoMesa(_mesa.id, DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"), _mov.numero_registro_atual, regAtual, (regAtual - _mov.numero_registro_atual), int.Parse(desconto), _mesa.valor_partida, Math.Truncate((((regAtual - _mov.numero_registro_atual) - int.Parse(desconto)) * _mesa.valor_partida) * _mesa.percentual_recebimento / 100), Math.Truncate(_mov.valor_restante), (decimal.Parse(txtb_valorPago.Text.Replace(".", ",")) == 0 ? null : DateTime.Now.ToString("yyyy/MM/ddTHH:mm:ss")), 0, decimal.Parse(txtb_valorPago.Text.Replace(".", ",")), Math.Truncate(((((((regAtual - _mov.numero_registro_atual) - int.Parse(desconto)) * _mesa.valor_partida) * _mesa.percentual_recebimento / 100) + Math.Truncate(_mov.valor_restante)) - decimal.Parse(txtb_valorPago.Text.Replace(".", ",")))), latitude, longitude, _mov.id_regiao, _mov.identidade, _mov.endereco, _mov.bairro, _mov.cidade, _mov.uf, _mov.cep, _mov.telefone, _mov.celular, 0, 0, DateTime.Now);
            List<BI_MovimentoMesa> listMovMesa = JsonConvert.DeserializeObject<List<BI_MovimentoMesa>>(Persistencia.JsonMovimentoMesas);
            //listMovMesa.Remove(listMovMesa.Where(c => c.id_mesa == movimento.id_mesa).First());
            listMovMesa.Add(movimento);
            Persistencia.JsonMovimentoMesas = JsonConvert.SerializeObject(listMovMesa, Formatting.Indented);

            Sincronismo sincronismo = new Sincronismo(movimento.id_mesa, DateTime.Now, default(DateTime));
            List<Sincronismo> listSincronismo = JsonConvert.DeserializeObject<List<Sincronismo>>(Persistencia.JsonSincronismo);
            if (listSincronismo == null)
                listSincronismo = new List<Sincronismo>();
            listSincronismo.Add(sincronismo);
            Persistencia.JsonSincronismo = JsonConvert.SerializeObject(listSincronismo, Formatting.Indented);

            if(movimento.valor_pago > 0)
            {
                List<ObjCaixa> _listobjcaixa = JsonConvert.DeserializeObject<List<ObjCaixa>>(Persistencia.JsonCaixa);
                if (Persistencia.JsonCaixa == "")
                    _listobjcaixa = new List<ObjCaixa>();
                ObjCaixa objCaixa = new ObjCaixa(movimento.valor_pago, false, "Recebimento mesa: " + movimento.id_mesa, DateTime.Now);
                _listobjcaixa.Add(objCaixa);
                Persistencia.JsonCaixa = JsonConvert.SerializeObject(_listobjcaixa, Formatting.Indented);
            }

            Prgs_pagamento.Visibility = Visibility.Collapsed;

            while (true)
            {
                var rsult = MessageBox.Show("Deseja imprimir novamente?", "Bilhar", MessageBoxButton.OKCancel);
                if (rsult == MessageBoxResult.OK)
                {
                    PrintViaClient(); 
                    
                }
                else
                {
                    MessageBox.Show("Recebimento salvo com sucesso", "Bilhar", MessageBoxButton.OK);
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                    try
                    {
                        this.mSocket.Dispose();
                        this.mSocket = null;
                        break;
                    }
                    catch { break; }
                    
                }
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string json;
            string cod;
            string json2;
            if (NavigationContext.QueryString.TryGetValue("objMov", out json))
                _mov = JsonConvert.DeserializeObject<BI_MovimentoMesa>(json);
            if (NavigationContext.QueryString.TryGetValue("reg", out cod))
                regAtual = int.Parse(cod);
            if (NavigationContext.QueryString.TryGetValue("objM", out json2))
                _mesa = JsonConvert.DeserializeObject<BI_mesa>(json2);
            dataPagamentoAnterior = Convert.ToDateTime(_mov.data);
            IniciaBluetooth();
            PreencheTela();
        }

        private void PreencheTela()
        {
            desconto = ((regAtual - _mov.numero_registro_atual) * _mesa.desconto / 100).ToString("#0");
            txt_subTotal.Text = Math.Truncate(((((regAtual - _mov.numero_registro_atual) - int.Parse(desconto)) * _mesa.valor_partida) * _mesa.percentual_recebimento / 100)).ToString("c");
            txt_debitoAnterior.Text = Math.Truncate(_mov.valor_restante).ToString("c");
            txt_totalPagar.Text = (Math.Truncate(((((regAtual - _mov.numero_registro_atual) - int.Parse(desconto)) * _mesa.valor_partida) * _mesa.percentual_recebimento / 100)) + Math.Truncate(_mov.valor_restante)).ToString("c");
            txt_dataPagamento.Text = DateTime.Now.ToString();
        }

        private void txtb_valorPago_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (decimal.Parse(txtb_valorPago.Text) > (((((regAtual - _mov.numero_registro_atual) - ((regAtual - _mov.numero_registro_atual) * _mesa.desconto / 100)) * _mesa.valor_partida) * _mesa.percentual_recebimento / 100) + _mov.valor_debito_anterior))
                //{ MessageBox.Show("O valor deve ser menos que o débito"); txtb_valorPago.Focus(); }
                if (txtb_valorPago.Text != "")
                { txt_valorRestante.Text = Math.Truncate(((((((regAtual - _mov.numero_registro_atual) - int.Parse(desconto)) * _mesa.valor_partida) * _mesa.percentual_recebimento / 100) + Math.Truncate(_mov.valor_restante)) - decimal.Parse(txtb_valorPago.Text.Replace(".",",")))).ToString("c"); fechateclado = true; }
                else { txtb_valorPago.Text = "0"; txt_valorRestante.Text = Math.Truncate(((((((regAtual - _mov.numero_registro_atual) - int.Parse(desconto)) * _mesa.valor_partida) * _mesa.percentual_recebimento / 100) + Math.Truncate(_mov.valor_restante)) - decimal.Parse(txtb_valorPago.Text))).ToString("c"); }
            }
            catch { }
        }

        private void txtb_valorPago_GotFocus(object sender, RoutedEventArgs e)
        {
            fechateclado = false;
        }

        #region Localização

        private async void CapturaCoordenadas()
        {
            if ((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"] != true)
            {
                return;
            }
            Geolocator geolocator = new Geolocator();

            geolocator.DesiredAccuracyInMeters = 50;
            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10)
                    );
                latitude = decimal.Parse(geoposition.Coordinate.Latitude.ToString());
                longitude = decimal.Parse(geoposition.Coordinate.Longitude.ToString());
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    MessageBox.Show("Status= Location  is disabled in phone settings.");
                }
            }
        }

        #endregion

        #region Impressora

        private delegate void FunctionCallBack();

        private class Function
        {
            public String IconType { get; set; }
            public String Name { get; set; }
            public String Description { get; set; }

            private FunctionCallBack mCallback;

            public Function(String iconType, String name, String description, FunctionCallBack callback)
            {
                this.IconType = iconType;
                this.Name = name;
                this.Description = description;
                this.mCallback = callback;
            }

            public void Run()
            {
                mCallback();
            }
        }

        // Helper method to convert byte array to hex string
        private static string ByteArrayToHexString(byte[] data, int offset, int length)
        {
            char[] hex = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            char[] buf = new char[length * 3];
            int offs = 0;

            for (int i = 0; i < length; i++)
            {
                buf[offs++] = hex[(data[offset + i] >> 4) & 0xf];
                buf[offs++] = hex[(data[offset + i] >> 0) & 0xf];
                buf[offs++] = ' ';
            }

            return new String(buf, 0, offs);
        }
       
        // Helper method to log 
        static void Log(String text, byte[] buffer, int offset, int count)
        {
            if (Debugger.IsAttached)
            {
                string s = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.") + (System.Environment.TickCount % 1000).ToString("D3") + " " + text;

                if (buffer != null) s += ByteArrayToHexString(buffer, offset, count);

                System.Diagnostics.Debug.WriteLine(s);
            }
        }

        // Helper method to log 
        static void Log(String text)
        {
            Log(text, null, 0, 0);
        }

        // Helper class that implement input stream
        private class ConnectionInputStream : IInputStream
        {
            private Windows.Storage.Streams.IInputStream mInputStream;            
            private byte[] mBuffer;
            private int mBufferCount;
            private IOException mLastError;
            
            public ConnectionInputStream(StreamSocket socket)
            {
                this.mInputStream = socket.InputStream;   
                this.mBuffer = new byte[2048];
                mBufferCount = 0;

                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        int chunk = mBuffer.Length - mBufferCount;

                        if (chunk > 0)
                        {
                            Task<Windows.Storage.Streams.IBuffer> task = null;

                            try
                            {
                                task = ReadAsync(chunk);
                                task.Wait();
                                if (task.Exception != null) throw task.Exception;
                            }
                            catch (Exception ex)
                            {
                                mLastError = new System.IO.IOException("Error reading bytes: " + ex.Message);
                                break;
                            }
                           
                            lock (mBuffer)
                            {
                                int count = (int)task.Result.Length;

                                for (int i = 0; i < count; i++)
                                {
                                    mBuffer[mBufferCount++] = task.Result.GetByte((uint)i);
                                }
                            }
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(10);
                        }
                    }
                });   
            }

            internal async Task<Windows.Storage.Streams.IBuffer> ReadAsync(int count)
            {
                byte[] tmp = new byte[count];

                return await mInputStream.ReadAsync(tmp.AsBuffer(), (uint)tmp.Length, Windows.Storage.Streams.InputStreamOptions.Partial);
            }

            public int Read(byte[] buffer, int offset, int count)
            {
                if (mLastError != null) throw mLastError;

                if (mBufferCount == 0) return 0;

                lock (mBuffer)
                {
                    int chunk = Math.Min(count, mBufferCount);

                    Array.Copy(mBuffer, 0, buffer, offset, chunk);

                    Array.Copy(mBuffer, chunk, mBuffer, 0, mBufferCount - chunk);

                    mBufferCount -= chunk;

                    Log("read(" + chunk + "): ", buffer, offset, chunk);

                    return chunk;
                }               
            }
            
        }

        // Helper class that implements output stream 
        private class ConnectionOutputStream : IOutputStream
        {
            private Windows.Storage.Streams.IOutputStream mOutputStream;                                   

            public ConnectionOutputStream(StreamSocket socket)
            {
                this.mOutputStream = socket.OutputStream;
            }

            public int Write(byte[] buffer, int offset, int count)
            {
                Task<int> task = null;

                try
                {
                    task = WriteAsync(buffer, offset, count);
                    task.Wait();                    
                    if (task.Exception != null) throw task.Exception;
                    
                }
                catch (Exception ex)
                {
                    throw new System.IO.IOException("Error sending bytes: " + ex.Message);
                }

                if (!task.IsCompleted)
                {
                    throw new System.IO.IOException("Sending bytes timeout");
                }
                
                Log("write(" + task.Result + "): ", buffer, offset, (int)task.Result);

                return task.Result;                
            }

            internal async Task<int> WriteAsync(byte[] buffer, int offset, int count)
            {
                byte[] tmp = new byte[count];

                Array.Copy(buffer, offset, tmp, 0, count);

                try
                {
                    int x = (int)(await mOutputStream.WriteAsync(tmp.AsBuffer()));                    
                    return x;
                }
                catch (Exception ex)
                {
                    throw new System.IO.IOException("Error sending bytes: " + ex.Message);
                }               
            }
        }

        private List<PeerInformation> mPrinterList = new List<PeerInformation>();
        private StreamSocket mSocket = null;
        private Printer mPrinter = null;

        private void Connect()
        {
            if (_popupBluetooth.PrinterList.SelectedIndex < 0) return;

            PeerInformation pi = mPrinterList[_popupBluetooth.PrinterList.SelectedIndex];

            RunAsync(() =>
            {
                this.mSocket = new StreamSocket();

                try
                {
                    Task task = mSocket.ConnectAsync(pi.HostName, "1").AsTask();
                    task.Wait();                    
                }
                catch (Exception ex)
                {
                    if (ex.HResult != 0) ShowError("F to connect: " + ex.InnerException.Message);
                    else ShowError("Failed to connect: Unknown error");
                    return;
                }

                try
                {
                    IInputStream inStream = new ConnectionInputStream(mSocket);
                    IOutputStream outStream = new ConnectionOutputStream(mSocket);

                    if (ProtocolAdapter.IsProtocolEnabled(inStream, outStream))
                    {
                        ProtocolAdapter adapter = new ProtocolAdapter(inStream, outStream);
                        ProtocolAdapter.LogicalChannel channel = adapter.GetChannel(ProtocolAdapter.Channel.Printer);
                        inStream = channel.GetInputStream();
                        outStream = channel.GetOutputStream();
                    }

                    this.mPrinter = new Printer(inStream, outStream);
                }
                catch (Exception ex)
                {
                    ShowError("Failed to init printer: " + ex.Message);
                    this.mSocket.Dispose();
                    this.mSocket = null;
                    return;
                }

                // Run on UI Thread
                Dispatcher.BeginInvoke(() =>
                {
                    conexaobluetooh = AppResources.ConnectedTo + ": " + pi.DisplayName + " " + pi.HostName.DisplayName;
                    ShowMainDialog();
                }); 
            });      
        }

        private void ShowError(String message)
        {
            Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show(message);
                ShowPrinterSelectionDialog();
                _popup.IsOpen = false;
                ApplicationBar.IsVisible = true;
                Prgs_pagamento.Visibility = Visibility.Collapsed;
                IniciaBluetooth();
            });
        }

        private void RunAsync(Action action)
        {
            _popupBluetooth.ProgressOverlay.Visibility = System.Windows.Visibility.Visible;
            
            Task t = Task.Factory.StartNew(() =>
            {
                try
                {
                    action();                    
                }
                finally
                {                    
                    Dispatcher.BeginInvoke(() =>
                    {
                        _popupBluetooth.ProgressOverlay.Visibility = System.Windows.Visibility.Collapsed;
                    }); 
                }
                    
            });                        
        }
        
        private void ShowPrinterSelectionDialog()
        {
            try
            {
                this.mSocket.Dispose();
                this.mSocket = null;
            }
            catch { }
            
            mPrinterList.Clear();
            _popupBluetooth.PrinterList.Items.Clear();

            // Search for all paired devices
            PeerFinder.AlternateIdentities["Bluetooth:Paired"] = "";

            try
            {
                Task<IReadOnlyList<PeerInformation>> task = PeerFinder.FindAllPeersAsync().AsTask<IReadOnlyList<PeerInformation>>();
                task.Wait();

                if (task.Exception != null) throw task.Exception;

                // Handle the result of the FindAllPeersAsync call
                foreach (PeerInformation peer in task.Result)
                {
                    mPrinterList.Add(peer);
                    _popupBluetooth.PrinterList.Items.Add(peer.DisplayName + " " + peer.HostName.DisplayName);
                }
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x8007048F)
                {
                    MessageBox.Show("Bluetooth is turned off");
                    showBTSettings();
                }
                else
                {
                    MessageBox.Show("Failed to enumarate devices: " + ex.Message);
                    showBTSettings();
                }
            }             
        }

        private void showBTSettings()
        {
            _popup.IsOpen = false;
            var task = new ConnectionSettingsTask();
            task.ConnectionSettingsType = ConnectionSettingsType.Bluetooth;
            task.Show();

        }

        private void ShowMainDialog()
        {
            //MainContent.IsEnabled = true;
            //DialogOverlay.Visibility = System.Windows.Visibility.Collapsed;
            //lblConnectedTo.Visibility = System.Windows.Visibility.Visible;
            _popup.IsOpen = false;
            Dispatcher.BeginInvoke(() => SalvoBanco());
            PrintViaClient();
            
        }        
        
        private void PrintSelfTest()
        {
            RunAsync(() =>
            {
                try
                {
                    mPrinter.PrintSelfTest();
                }
                catch (System.IO.IOException ex)
                {
                    ShowError("Failed to print self test: " + ex.Message);                    
                }
            });
        }

        private void PrintViaClient()
        {
            if (txtb_valorPago.Text == "")
            { txtb_valorPago.Text = "0"; txt_valorRestante.Text = txt_totalPagar.Text; }
            string subtotal = txt_subTotal.Text;
            string debitoanterior = txt_debitoAnterior.Text;
            string totalPagar = txt_totalPagar.Text;
            decimal valorPago = decimal.Parse(txtb_valorPago.Text.Replace(".",","));
            string valorRestante = txt_valorRestante.Text;
            RunAsync(() =>
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("{reset}{CENTER}{H}{w}{B}BILHARES ESTRELA{br}");
                sb.Append("{reset}{CENTER}Fone: 0800 643 6754 | (62) 3242-9419{br}");
                sb.Append("{reset}{CENTER}Via cliente{br}");
                sb.Append("{reset}{br}");

                sb.Append("{reset}N do recibo.: " + Persistencia.NumerodoRecibo.ToString("#00000") + "{br}");
                
                sb.Append("{reset}Regiao......: " + _mesa.id_regiao.ToString("#000") + " " + _mesa.cidade + "{br}");
                sb.Append("{reset}Cod.da Mesa.: " + _mesa.id.ToString() + "{br}");
                sb.Append("{reset}Tipo Mesa...: " + Swifts.TipoMesa(_mesa.id_tipo_mesa) + "{br}");
                sb.Append("{reset}------------------------------------------------{br}");

                sb.Append("{reset}Nome..: {b}" + _mesa.nome.ToString() + "{br}");
                sb.Append("{reset}Ender.: {b}" + _mesa.endereco.ToString() + "{br}");
                sb.Append("{reset}Bairro: {b}" + _mesa.bairro.ToString() + "{br}");
                sb.Append("{reset}Cidade: {b}" + _mesa.cidade.ToString() + "{br}");
                sb.Append("{reset}U.F...: {b}" + _mesa.uf.ToString() + "{br}");
                sb.Append("{reset}CPF...: {b}" + _mesa.cpf.ToString() + "{br}");
                sb.Append("{reset}------------------------------------------------{br}");

                sb.Append("{reset}     Data Ultima Locacao..: {b}" + dataPagamentoAnterior.ToString() + "{br}{br}");
                sb.Append("{reset}       Relogio Atual........: {b}" + regAtual.ToString() + "{br}");
                sb.Append("{reset}     - Relogio Anterior.....: {b}" + _mov.numero_registro_atual.ToString() + "{br}");
                sb.Append("{reset}     = Total de Partidas....: {b}" +  (regAtual - _mov.numero_registro_atual).ToString()+ "{br}");
                sb.Append("{reset}     - Desconto " + _mesa.desconto.ToString("#00") + "%.........: {b}" + desconto + "{br}");
                sb.Append("{reset}       Valor da partida.....: {b}" + _mesa.valor_partida.ToString("c") + "{br}");
                sb.Append("{reset}     = Valor Bruto..........: {b}R$ " + (((regAtual - _mov.numero_registro_atual) - Convert.ToInt16(desconto)) * _mov.valor_partida).ToString("#0.00") + "{br}");
                sb.Append("{reset}       SUB-TOTAL A PAGAR "+_mesa.percentual_recebimento.ToString("#00")+"%: {b}" + subtotal + "{br}");
                sb.Append("{reset}     + DEBITO ANTERIOR......: {b}" + debitoanterior + "{br}");
                sb.Append("{reset}       TOTAL A PAGAR........: {b}{H}{w}" + totalPagar + "{br}");
                sb.Append("{reset}     = VALOR PAGO...........: {b}" + valorPago.ToString("c") + "{br}");
                sb.Append("{reset}     = VALOR RESTANTE.......: {b}{H}{w}" + valorRestante + "{br}");
                sb.Append("{reset}------------------------------------------------{br}");

                sb.Append("{reset}FACA O DEPOSITO EM QUALQUER AGENCIA DO BANCO DO BRASIL OU AGENCIA POSTAL DOS CORREIOS. AGENCIA: 3486-X CONTA: 10.008-0 EM NOME DE: CARLOS SOARES DE SOUZA. AGUARDE O RECIBO DO DEPOSITO COMO\n COMPROVANTE DO PAGAMENTO.{br}{br}");

                sb.Append("{reset}ASS:____________________________________________{br}");
                sb.Append("{reset}{CENTER}"+Persistencia.Representante+"{br}");
                sb.Append("{reset}{CENTER}Representante{br}{br}");
                //sb.Append("{reset}------------------------------------------------{br}{br}");
                sb.Append("{reset}   www.cerrado.mobi      Fone: (62) 3277-1017   {br}{br}");

                try
                {
                    mPrinter.Reset();
                    mPrinter.PrintTaggedText(sb.ToString());
                    mPrinter.FeedPaper(110);
                }
                catch (System.IO.IOException ex)
                {
                    ShowError("Failed to print test: " + ex.Message);
                }
            });

            var result = MessageBox.Show("Imprimir via da empresa", "Bilhar", MessageBoxButton.OK);
            if (result == MessageBoxResult.OK)
                PrintViaEmpresa(); 
        }

        private void PrintViaEmpresa()
        {
            if (txtb_valorPago.Text == "")
            { txtb_valorPago.Text = "0"; txt_valorRestante.Text = txt_totalPagar.Text; }
            string subtotal = txt_subTotal.Text;
            string debitoanterior = txt_debitoAnterior.Text;
            string totalPagar = txt_totalPagar.Text;
            decimal valorPago = decimal.Parse(txtb_valorPago.Text.Replace(".", ","));
            string valorRestante = txt_valorRestante.Text;
            RunAsync(() =>
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("{reset}{CENTER}{H}{w}{B}BILHARES ESTRELA{br}");
                sb.Append("{reset}{CENTER}Fone: 0800 643 6754 | (62) 3242-9419{br}");
                sb.Append("{reset}{CENTER}Via empresa{br}");
                sb.Append("{reset}{br}");

                sb.Append("{reset}N do recibo.: " + Persistencia.NumerodoRecibo.ToString("#00000") + "{br}");

                sb.Append("{reset}Regiao......: " + _mesa.id_regiao.ToString("#000") + " " + _mesa.cidade + "{br}");
                sb.Append("{reset}Cod.da Mesa.: " + _mesa.id.ToString() + "{br}");
                sb.Append("{reset}Tipo Mesa...: " + Swifts.TipoMesa(_mesa.id_tipo_mesa) + "{br}");
                sb.Append("{reset}------------------------------------------------{br}");

                sb.Append("{reset}Nome..: {b}" + _mesa.nome.ToString() + "{br}");
                //sb.Append("{reset}Ender.: {b}" + _mesa.endereco.ToString() + "{br}");
                //sb.Append("{reset}Bairro: {b}" + _mesa.bairro.ToString() + "{br}");
                //sb.Append("{reset}Cidade: {b}" + _mesa.cidade.ToString() + "{br}");
                //sb.Append("{reset}U.F...: {b}" + _mesa.uf.ToString() + "{br}");
                //sb.Append("{reset}CPF...: {b}" + _mesa.cpf.ToString() + "{br}");
                sb.Append("{reset}------------------------------------------------{br}");

                //sb.Append("{reset}     Data Ultima Locacao..: {b}" + dataPagamentoAnterior.ToString() + "{br}{br}");
                sb.Append("{reset}       Relogio Atual........: {b}" + regAtual.ToString() + "{br}");
                sb.Append("{reset}     - Relogio Anterior.....: {b}" + _mov.numero_registro_atual.ToString() + "{br}");
                //sb.Append("{reset}     = Total de Partidas....: {b}" + (regAtual - _mov.numero_registro_atual).ToString() + "{br}");
                //sb.Append("{reset}     - Desconto " + _mesa.desconto.ToString("#00") + "%.........: {b}" + desconto + "{br}");
                //sb.Append("{reset}       Valor da partida.....: {b}" + _mesa.valor_partida.ToString("c") + "{br}");
                //sb.Append("{reset}     = Valor Bruto..........: {b}R$ " + (((regAtual - _mov.numero_registro_atual) - Convert.ToInt16(desconto)) * _mov.valor_partida).ToString("#0.00") + "{br}");
                sb.Append("{reset}       SUB-TOTAL A PAGAR " + _mesa.percentual_recebimento.ToString("#00") + "%: {b}" + subtotal + "{br}");
                sb.Append("{reset}     + DEBITO ANTERIOR......: {b}" + debitoanterior + "{br}");
                sb.Append("{reset}       TOTAL A PAGAR........: {b}{H}{w}" + totalPagar + "{br}");
                sb.Append("{reset}     = VALOR PAGO...........: {b}" + valorPago.ToString("c") + "{br}");
                sb.Append("{reset}     = VALOR RESTANTE.......: {b}{H}{w}" + valorRestante + "{br}");
                sb.Append("{reset}{br}{br}{br}");

                //sb.Append("{reset}FACA O DEPOSITO EM QUALQUER AGENCIA DO BANCO DO BRASIL OU AGENCIA POSTAL DOS CORREIOS. AGENCIA: 3486-X CONTA: 10.008-0 EM NOME DE: CARLOS SOARES DE SOUZA. AGUARDE O RECIBO DO DEPOSITO COMO\n COMPROVANTE DO PAGAMENTO.{br}{br}");

                sb.Append("{reset}ASS:____________________________________________{br}");
                sb.Append("{reset}{CENTER}" + Persistencia.Representante + "{br}");
                sb.Append("{reset}{CENTER}Representante{br}{br}");
                //sb.Append("{reset}------------------------------------------------{br}{br}");
                sb.Append("{reset}   www.cerrado.mobi      Fone: (62) 3277-1017   {br}{br}");

                try
                {
                    mPrinter.Reset();
                    mPrinter.PrintTaggedText(sb.ToString());
                    mPrinter.FeedPaper(110);
                }
                catch (System.IO.IOException ex)
                {
                    ShowError("Failed to print test: " + ex.Message);
                }
            });


        }

        #endregion
    }
}