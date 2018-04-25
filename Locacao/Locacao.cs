using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilhar.Locacao
{
    class Locacao
    {
        public int Id_BI_MovLocacao;
        public int IdMesa_BI_MovLocacao;
        public DateTime DataLocacao_BI_MovLocacao;
        public int IdTipoMesa_BI_MovLocacao;
        public decimal Latitude_BI_MovLocacao;
        public decimal Longitude_BI_MovLocacao;
        public int IdRegiao_BI_MovLocacao;
        public string Nome_BI_MovLocacao;
        public string Endereco_BI_MovLocacao;
        public string Bairro_BI_MovLocacao;
        public string Cidade_BI_MovLocacao;
        public string Uf_BI_MovLocacao;
        public string Cep_BI_MovLocacao;
        public string Telefone_BI_MovLocacao;
        public string Celular_BI_MovLocacao;
        public DateTime DataNascimento_BI_MovLocacao;
        public string Identidade_BI_MovLocacao;
        public string Cpf_BI_MovLocacao;
        public string Cnpj_BI_MovLocacao;
        public string InscricaoEstadual_BI_MovLocacao;
        public decimal ValorPartida_BI_MovLocacao;
        public decimal Desconto_BI_MovLocacao;
        public decimal PercentualRecebimento_BI_MovLocacao;
        public int NumeroRelogio_BI_MovLocacao;
        public string Observacao_BI_MovLocacao;
        public int IdSincronismo_BI_MovLocacao;
        public int IdRegiaoAnt_BI_MovLocacao;
        public string NomeAnt_BI_MovLocacao;
        public string EnderecoAnt_BI_MovLocacao;
        public string BairroAnt_BI_MovLocacao;
        public string CidadeAnt_BI_MovLocacao;
        public string UfAnt_BI_MovLocacao;
        public string CepAnt_BI_MovLocacao;
        public string TelefoneAnt_BI_MovLocacao;
        public string CelulaAntr_BI_MovLocacao;
        public DateTime DataNascimentoAnt_BI_MovLocacao;
        public string IdentidadeAnt_BI_MovLocacao;
        public string CpfAnt_BI_MovLocacao;
        public string CnpjAnt_BI_MovLocacao;
        public string InscricaoEstadualAnt_BI_MovLocacao;
        public decimal ValorPartidaAnt_BI_MovLocacao;
        public decimal DescontoAnt_BI_MovLocacao;
        public decimal PercentualRecebimentoAnt_BI_MovLocacao;
        public int NumeroRelogioAnt_BI_MovLocacao;

        public Locacao(int _Id_BI_MovLocacao,
         int _IdMesa_BI_MovLocacao,
         DateTime _DataLocacao_BI_MovLocacao,
         int _IdTipoMesa_BI_MovLocacao,
         decimal _Latitude_BI_MovLocacao,
         decimal _Longitude_BI_MovLocacao,
         int _IdRegiao_BI_MovLocacao,
         string _Nome_BI_MovLocacao,
         string _Endereco_BI_MovLocacao,
         string _Bairro_BI_MovLocacao,
         string _Cidade_BI_MovLocacao,
         string _Uf_BI_MovLocacao,
         string _Cep_BI_MovLocacao,
         string _Telefone_BI_MovLocacao,
         string _Celular_BI_MovLocacao,
         DateTime _DataNascimento_BI_MovLocacao,
         string _Identidade_BI_MovLocacao,
         string _Cpf_BI_MovLocacao,
         string _Cnpj_BI_MovLocacao,
         string _InscricaoEstadual_BI_MovLocacao,
         decimal _ValorPartida_BI_MovLocacao,
         decimal _Desconto_BI_MovLocacao,
         decimal _PercentualRecebimento_BI_MovLocacao,
         int _NumeroRelogio_BI_MovLocacao,
         string _Observacao_BI_MovLocacao,
         int _IdSincronismo_BI_MovLocacao,
         int _IdRegiaoAnt_BI_MovLocacao = 0,
         string _NomeAnt_BI_MovLocacao = "",
         string _EnderecoAnt_BI_MovLocacao = "",
         string _BairroAnt_BI_MovLocacao = "",
         string _CidadeAnt_BI_MovLocacao = "",
         string _UfAnt_BI_MovLocacao = "",
         string _CepAnt_BI_MovLocacao = "",
         string _TelefoneAnt_BI_MovLocacao = "",
         string _CelulaAntr_BI_MovLocacao = "",
         DateTime _DataNascimentoAnt_BI_MovLocacao = default(DateTime),
         string _IdentidadeAnt_BI_MovLocacao = "",
         string _CpfAnt_BI_MovLocacao = "",
         string _CnpjAnt_BI_MovLocacao = "",
         string _InscricaoEstadualAnt_BI_MovLocacao = "",
         decimal _ValorPartidaAnt_BI_MovLocacao = 0,
         decimal _DescontoAnt_BI_MovLocacao = 0,
         decimal _PercentualRecebimentoAnt_BI_MovLocacao = 0,
         int _NumeroRelogioAnt_BI_MovLocacao = 0)
        {
            Id_BI_MovLocacao = _Id_BI_MovLocacao;
            IdMesa_BI_MovLocacao = _IdMesa_BI_MovLocacao;
            DataLocacao_BI_MovLocacao = _DataLocacao_BI_MovLocacao;
            IdTipoMesa_BI_MovLocacao = _IdTipoMesa_BI_MovLocacao;
            Latitude_BI_MovLocacao = _Latitude_BI_MovLocacao;
            Longitude_BI_MovLocacao = _Longitude_BI_MovLocacao;
            IdRegiao_BI_MovLocacao = _IdRegiao_BI_MovLocacao;
            Nome_BI_MovLocacao = _Nome_BI_MovLocacao;
            Endereco_BI_MovLocacao = _Endereco_BI_MovLocacao;
            Bairro_BI_MovLocacao = _Bairro_BI_MovLocacao;
            Cidade_BI_MovLocacao = _Cidade_BI_MovLocacao;
            Uf_BI_MovLocacao = _Uf_BI_MovLocacao;
            Cep_BI_MovLocacao = _Cep_BI_MovLocacao;
            Telefone_BI_MovLocacao = _Telefone_BI_MovLocacao;
            Celular_BI_MovLocacao = _Celular_BI_MovLocacao;
            DataNascimento_BI_MovLocacao = _DataNascimento_BI_MovLocacao;
            Identidade_BI_MovLocacao = _Identidade_BI_MovLocacao;
            Cpf_BI_MovLocacao = _Cpf_BI_MovLocacao;
            Cnpj_BI_MovLocacao = _Cnpj_BI_MovLocacao;
            InscricaoEstadual_BI_MovLocacao = _InscricaoEstadual_BI_MovLocacao;
            ValorPartida_BI_MovLocacao = _ValorPartida_BI_MovLocacao;
            Desconto_BI_MovLocacao = _Desconto_BI_MovLocacao;
            PercentualRecebimento_BI_MovLocacao = _PercentualRecebimento_BI_MovLocacao;
            NumeroRelogio_BI_MovLocacao = _NumeroRelogio_BI_MovLocacao;
            Observacao_BI_MovLocacao = _Observacao_BI_MovLocacao;
            IdSincronismo_BI_MovLocacao = _IdSincronismo_BI_MovLocacao;
            IdRegiaoAnt_BI_MovLocacao = _IdRegiaoAnt_BI_MovLocacao;
            NomeAnt_BI_MovLocacao = _NomeAnt_BI_MovLocacao;
            EnderecoAnt_BI_MovLocacao = _Endereco_BI_MovLocacao;
            BairroAnt_BI_MovLocacao = _BairroAnt_BI_MovLocacao;
            CidadeAnt_BI_MovLocacao = _CidadeAnt_BI_MovLocacao;
            UfAnt_BI_MovLocacao = _UfAnt_BI_MovLocacao;
            CepAnt_BI_MovLocacao = _Cep_BI_MovLocacao;
            TelefoneAnt_BI_MovLocacao = _Telefone_BI_MovLocacao;
            CelulaAntr_BI_MovLocacao = _CelulaAntr_BI_MovLocacao;
            DataNascimentoAnt_BI_MovLocacao = _DataNascimentoAnt_BI_MovLocacao;
            IdentidadeAnt_BI_MovLocacao = _IdentidadeAnt_BI_MovLocacao;
            CpfAnt_BI_MovLocacao = _CpfAnt_BI_MovLocacao;
            CnpjAnt_BI_MovLocacao = _CnpjAnt_BI_MovLocacao;
            InscricaoEstadualAnt_BI_MovLocacao = _InscricaoEstadualAnt_BI_MovLocacao;
            ValorPartidaAnt_BI_MovLocacao = _ValorPartidaAnt_BI_MovLocacao;
            DescontoAnt_BI_MovLocacao = _DescontoAnt_BI_MovLocacao;
            PercentualRecebimentoAnt_BI_MovLocacao = _PercentualRecebimentoAnt_BI_MovLocacao;
            NumeroRelogioAnt_BI_MovLocacao = _NumeroRelogioAnt_BI_MovLocacao;
        }
    }
}
