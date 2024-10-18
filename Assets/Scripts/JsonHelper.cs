using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Для парса нескольких данных из _json
public static class JsonHelper
{
    public static T[] FromJson<T>(string json, string tableName)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        Loader.LoadedTables[tableName] = true;
        return wrapper.Items;
    }
    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }
    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }
    public static string fixJson(string value)
    {
        value = "{\"Items\": " + value + " }";
        return value;
    }
    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}