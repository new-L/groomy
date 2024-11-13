using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Authorization : MonoBehaviour
{
    [SerializeField] private Notification _notification;

    public void GetAuthorizationStatus(string login, string password)
    {
        StartCoroutine(SendToServer(login, password));
    }

    private IEnumerator SendToServer(string login, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("login", login);
        form.AddField("pass", password);
        UnityWebRequest www = UnityWebRequest.Post(URLs.Authorization, form);

        www.timeout = ServerSettings.TimeOut;

        yield return www.SendWebRequest();
        if (www.error != null) { _notification.NotificationIn(www.error); yield break; }
        if (www.downloadHandler.text.Equals("")) { _notification.NotificationIn("Введен неверный логин или пароль!"); yield break; }
        User.Player = JsonUtility.FromJson<Player>(www.downloadHandler.text);
        
        //Подговнокодил я везде знатно, кнш, но с этим надо что-то делать
        if(User.Player.user_type.Equals("user")) SceneJump.JumpTo((int)ScenesIndex.UserScene);
        else if(User.Player.user_type.Equals("admin")) SceneJump.JumpTo((int)ScenesIndex.AdminScene);
    }
}
