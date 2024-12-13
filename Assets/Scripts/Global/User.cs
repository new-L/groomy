using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    private static Player _player;
    private static PlayerCurrency _currency;
    private PlayerRating _rating;

    public static Player Player { get => _player; set => _player = value; }
    public static PlayerCurrency Currency { get => _currency; set => _currency = value; }
    public PlayerRating Rating { get => _rating; set => _rating = value; }
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

[Serializable]
public class PlayerRating
{
    public int rating_count;
}