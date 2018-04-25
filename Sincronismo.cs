using Microsoft.Phone.Info;
using System;



namespace Bilhar
{
    class Sincronismo
    {
        //id - Int Not Null, (auto incremento).
        //public DateTime data_dispositivo_origem;
        public string origem; //identificação do aparelho
        public string tabela;// movimentomesa
        public string operacao;// inclusao
        public int id_mesa; // id da mesa
        public DateTime data_movimento; // data hora do movimento
        public DateTime sincronizado_servidor;

        public Sincronismo(int _id_mesa, DateTime _data_movimento, DateTime _sincronizado_servidor, string _operacao = "INCLUSAO", string _tabela = "MOVMESA")
        {
            //data_dispositivo_origem = _data_dispositivo_origem;
            origem = "Mobile: " + GetDeviceUniqueID();
            tabela = _tabela;
            operacao = _operacao;
            id_mesa = _id_mesa;
            data_movimento = _data_movimento;
            sincronizado_servidor = _sincronizado_servidor;
        }

        private string GetDeviceUniqueID()
        {
            byte[] getresult = null;
            object uniqueId;
            string val = "";
            if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
            {
                getresult = (byte[])uniqueId;
            }
            val = Convert.ToBase64String(getresult);
            return val;
        }
    }
}
