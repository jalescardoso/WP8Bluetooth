using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ObjCaixa
{
    public decimal valor;
    public bool despesa;
    public string descricao;
    public DateTime datalancamento;
    public ObjCaixa(decimal _valor, bool _despesa, string _descricao, DateTime _datalancamento)
    {
        valor = _valor;
        despesa = _despesa;
        descricao = _descricao;
        datalancamento = _datalancamento;
    }
}