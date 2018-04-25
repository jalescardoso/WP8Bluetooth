using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Swifts
{
    public static string Regiao(int id)
    {
        switch (id)
        {
            case 1:
                return "GOIÂNIA";
            case 2:
                return "NORDESTE GOIANO";
            case 3:
                return "BAHIA";
            case 4:
                return "PIAUÍ";
            case 7:
                return "BAHIA 4";
            case 8:
                return "APARECIDA DE GOIÂNIA";
            case 9:
                return "MESA NO DEPÓSITO PARA LOCAÇÃO";
            case 10:
                return "MESA DESMANCHADA";
            case 20:
                return "PIAUI 2";
            case 22:
                return "BAHIA 5";
            case 23:
                return "MESAS PROCURADAS";
            case 24:
                return "MESA DEPOSITO P/ PAULO AUGUSTO";
            case 25:
                return "NÃO EXISTE NO LOCAL PIAUI";
            case 26:
                return "PIAUI FICHA DEPOSITO CORRENTE";
            case 27:
                return "DEPOSITO CORRENTE POR PAULO";
            default:
                return "";
        }
    }

    public static string TipoMesa(int id)
    {
        switch (id)
        {
            case 2:
                return "GRANDE";
            case 1:
                return "MÉDIA";
            case 243:
                return "MESA DE PEDRA";
            case 3:
                return "PEQUENA";
            default:
                return "";
        }
    }
}

