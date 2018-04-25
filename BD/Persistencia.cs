using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Persistencia
{
    private static IsolatedStorageSettings store;
    private static IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();

    static Persistencia()
    {
        store = IsolatedStorageSettings.ApplicationSettings;
    }

    public enum SettingsKeyNames
    {
        JsonMesas,
        JsonMovimentoMesas,
        JsonSincronismo,
        Representante,
        NumerodoRecibo,
        JsonCaixa
    }

    private static T GetValueOrDefault<T>(SettingsKeyNames keyName, T defaultValue)
    {
        string key = keyName.ToString();
        try
        {
            if (store.Contains(key))
            {
                return (T)store[key];
            }
            else
            {
                return defaultValue;
            }
        }
        catch
        {
            return defaultValue;
        }
    }

    private static void AddOrUpdateSettings(SettingsKeyNames keyName, object value)
    {
        string key = keyName.ToString();
        if (store.Contains(key))
        {
            store[key] = value;
        }
        else
        {
            store.Add(key, value);
        }
        store.Save();
    }

    //private static void AddOrUpdateSettingsList(SettingsKeyNames keyName, object value)
    //{
    //    string key = keyName.ToString();
    //    StreamWriter Writer = new StreamWriter(new IsolatedStorageFileStream(keyName+".txt", FileMode.OpenOrCreate, isf));
    //    Writer.WriteLine(value);
    //    Writer.Close();
    //}

    //private static T GetValueOrDefaultList<T>(SettingsKeyNames keyName, T defaultValue)
    //{
    //    string key = keyName.ToString();
    //    if (isf.FileExists(keyName))
    //    {

    //    }
    //    //try
    //    //{
    //    //    if (store.Contains(key))
    //    //    {
    //    //        return (T)store[key];
    //    //    }
    //    //    else
    //    //    {
    //    //        return defaultValue;
    //    //    }
    //    //}
    //    //catch
    //    //{
    //    //    return defaultValue;
    //    //}
    //}
    public static void ZerarTudo()
    {
        JsonMesas = "";
        JsonMovimentoMesas = "";
        JsonSincronismo = "";
        Representante = "";
        NumerodoRecibo = 0;
        JsonCaixa = "";
    }

    public static string JsonMesas
    {
        get
        {
            return GetValueOrDefault<string>(SettingsKeyNames.JsonMesas, string.Empty);
        }
        set
        {
            AddOrUpdateSettings(SettingsKeyNames.JsonMesas, value);
        }
    }

    public static string JsonMovimentoMesas
    {
        get
        {
            return GetValueOrDefault<string>(SettingsKeyNames.JsonMovimentoMesas, string.Empty);
        }
        set
        {
            AddOrUpdateSettings(SettingsKeyNames.JsonMovimentoMesas, value);
        }
    }

    public static string JsonSincronismo
    {
        get
        {
            return GetValueOrDefault<string>(SettingsKeyNames.JsonSincronismo, string.Empty);
        }
        set
        {
            AddOrUpdateSettings(SettingsKeyNames.JsonSincronismo, value);
        }
    }

    public static string Representante
    {
        get
        {
            return GetValueOrDefault<string>(SettingsKeyNames.Representante, "Bilhares Estrela");
        }
        set
        {
            AddOrUpdateSettings(SettingsKeyNames.Representante, value);
        }
    }

    public static int NumerodoRecibo
    {
        get
        {
            return GetValueOrDefault<int>(SettingsKeyNames.NumerodoRecibo, 1);
        }
        set
        {
            AddOrUpdateSettings(SettingsKeyNames.NumerodoRecibo, value);
        }
    }

    public static string JsonCaixa
    {
        set
        {
            AddOrUpdateSettings(SettingsKeyNames.JsonCaixa, value);
        }
        get
        {
            return GetValueOrDefault<string>(SettingsKeyNames.JsonCaixa, string.Empty);
        }
    }
}

