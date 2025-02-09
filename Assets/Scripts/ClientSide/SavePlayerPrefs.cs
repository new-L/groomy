using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SavePlayerPrefs : MonoBehaviour
{

    public void SaveAuthDatas(string login, string pass)
    {
        PlayerPrefs.SetString("Login", login);
        PlayerPrefs.SetString("Password", pass);
        PlayerPrefs.Save();
    }

    public void DeleteAuthDatas()
    {
        PlayerPrefs.DeleteKey("Login");
        PlayerPrefs.DeleteKey("Password");
        PlayerPrefs.Save();
    }

    public bool isPlayerPrefsExist()
    {
        if (PlayerPrefs.HasKey("Login") && PlayerPrefs.HasKey("Password")) return true;
        else return false;
    }
    public string LoadLogin() => PlayerPrefs.GetString("Login");
    public string LoadPassword() => PlayerPrefs.GetString("Password");

}
