using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BI_MovimentoMesa
{
    public int id_mesa;
    public string data;
    public int numero_registro_anterior;
    public int numero_registro_atual;
    public int quantidade_partida;
    public decimal desconto;
    public decimal valor_partida;
    public decimal valor_a_pagar;
    public decimal valor_debito_anterior;
    public string data_pagamento;
    public decimal valor_juros;
    public decimal valor_pago;
    public decimal valor_restante;
    public decimal latitude;
    public decimal longitude;
    public int id_regiao;
    public string identidade;
    public string endereco;
    public string bairro;
    public string cidade;
    public string uf;
    public string cep;
    public string telefone;
    public string celular;
    public int id_agendamento;
    public int id_sincronismo;
    public DateTime data_Movimento;

    public BI_MovimentoMesa(int _id_mesa,
     string _data,
     int _numero_registro_anterior,
     int _numero_registro_atual,
     int _quantidade_partida,
     decimal _desconto,
     decimal _valor_partida,
     decimal _valor_a_pagar,
     decimal _valor_debito_anterior,
     string _data_pagamento,
     decimal _valor_juros,
     decimal _valor_pago,
     decimal _valor_restante,
     decimal _latitude,
     decimal _longitude,
     int _id_regiao,
     string _identidade,
     string _endereco,
     string _bairro,
     string _cidade,
     string _uf,
     string _cep,
     string _telefone,
     string _celular,
     int _id_agendamento,
     int _id_sincronismo,
        DateTime _data_Movimento)
    {
        id_mesa = _id_mesa;
        data = _data;
        numero_registro_anterior = _numero_registro_anterior;
        numero_registro_atual = _numero_registro_atual;
        quantidade_partida = _quantidade_partida;
        desconto = _desconto;
        valor_partida = _valor_partida;
        valor_a_pagar = _valor_a_pagar;
        valor_debito_anterior = _valor_debito_anterior;
        data_pagamento = _data_pagamento;
        valor_juros = _valor_juros;
        valor_pago = _valor_pago;
        valor_restante = _valor_restante;
        latitude = _latitude;
        longitude = _longitude;
        id_regiao = _id_regiao;
        identidade = _identidade;
        endereco = _endereco;
        bairro = _bairro;
        cidade = _cidade;
        uf = _uf;
        cep = _cep;
        telefone = _telefone;
        celular = _celular;
        id_agendamento = _id_agendamento;
        id_sincronismo = _id_sincronismo;
        data_Movimento = _data_Movimento;
    }
}

