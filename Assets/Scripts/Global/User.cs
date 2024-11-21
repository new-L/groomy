using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    private static Player _player;
    private static PlayerCurrency _currency;

    public static Player Player { get => _player; set => _player = value; }
    public static PlayerCurrency Currency { get => _currency; set => _currency = value; }
}

[Serializable]
public class Player
{
    public int user_id;
    public string login;
    public string user_type;
}

[Serializable]
public class PlayerCurrency
{
    public int gold_count = 0;
}

