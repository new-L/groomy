using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ServerUsers : MonoBehaviour
{
    private static ServerUser[] _users;

    [SerializeField] private List<string> _tablesNames;
    [SerializeField] private string _json;

    #region Get/Set
    public static ServerUser[] Users { get => _users; set => _users = value; }
    #endregion

    public void StartLoad()
    {
        Loader.LoadedTables.Clear();
        GetTablesData();
    }

    private void GetTablesData()
    {
        Actions.OnStartLoad?.Invoke();
        foreach (var item in _tablesNames)
        {
            //Loader.LoadedTables.Add(item, false);
            //if (item.Equals(_tablesNames.Last())) GetList(item, true);
            //else GetList(item, false);
        }
    }

    private void Start()
    {
        GetList(DBTablesName.Users, true);
    }

    public void GetList(string tableName, bool isLastTable)
    {
        StartCoroutine(GetUserList(tableName, isLastTable));
    }

    private IEnumerator GetUserList(string tableName, bool isLastTable)
    {
        Loader.IsLoadComplete = false;
        Debug.Log("{GetUserList}: Запрашиваем данные у таблицы: " + tableName);
        WWWForm form = new WWWForm();
        form.AddField("table_name", tableName);
        UnityWebRequest www = UnityWebRequest.Post(URLs.Users, form);

        www.timeout = ServerSettings.TimeOut;

        yield return www.SendWebRequest();
        if (www.error != null) { Debug.Log("Не удалось связаться с сервером!"); yield break; }
        Debug.Log(isLastTable + " " + www.downloadHandler.text);

        _json = JsonHelper.fixJson(www.downloadHandler.text);
        _users = JsonHelper.FromJson<ServerUser>(_json, tableName);
        if (isLastTable)
        {
            Loader.IsLoadComplete = true;
        }
    }
}

[Serializable]
public class ServerUser
{
    public int user_id;
    public string login;
    public string registration_date;
    public string user_type;
    public int gold_count;
}
