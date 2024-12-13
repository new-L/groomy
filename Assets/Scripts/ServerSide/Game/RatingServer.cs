using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RatingServer : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private EndGame _endGame;

    #region ServerFields
    private WWWForm _form;
    private UnityWebRequest _www;
    private int _count;
    private bool _isSended;

    public bool IsSended { get => _isSended; }
    #endregion

    public void Counting(int count, RequestType type)
    {
        _isSended = false;
        StartCoroutine(CountingRating(type, count));
    }

    private IEnumerator CountingRating(RequestType request, int count)
    {
        _count = count;
        _form = new WWWForm();
        _form.AddField("user_id", User.Player.user_id);
        if (request == RequestType.Add)
            _form.AddField("request_type", "add");
        else if (request == RequestType.Subtract)
            _form.AddField("request_type", "subtract");
        _form.AddField("count", count);

        _www = UnityWebRequest.Post(URLs.UserRating, _form);//URLs.UserRating, _form);

        _www.timeout = ServerSettings.TimeOut;
        yield return _www.SendWebRequest();
        if (_www.error != null)
        {
            Debug.Log(_www.downloadHandler.text);
            Debug.Log("\n" + _www.error);
            StartCoroutine(Waiter(request));
            yield break;
        }
        _isSended = true;
        _endGame?.OnDatasSended?.Invoke();
    }

    private IEnumerator Waiter(RequestType request)
    {
        Debug.Log("Waiter is working!");
        yield return new WaitForSecondsRealtime(ServerSettings.Cooldown);
        // if (request.Equals("get")) StartCoroutine(SendToServer(request));
        //else if (request.Equals("add")) StartCoroutine(AddCurrency(request, _count));
        StartCoroutine(CountingRating(request, _count));
        Debug.Log("AFTER " + ServerSettings.Cooldown + " seconds waiter is working!");
    }
}
