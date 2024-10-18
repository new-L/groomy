using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSettings : MonoBehaviour
{

    private static int _timeOut = 15;

    public static int TimeOut { get => _timeOut; set => _timeOut = value; }
}
