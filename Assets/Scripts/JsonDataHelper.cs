using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class JsonDataHelper
{
    [Serializable]
    public class Wrapper<T>
        where T : class
    {
        public T[] Items;
    }

    private static string JsonPath(string name) => $"JsonData/{name}";
    private static string JsonAbsolutePath(string name) => Application.dataPath + $"/Resources/JsonData/{name}.json";

    public static void WriteData<T>(T data, string name)
        where T : class
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new Exception("Name is not inputted");
        }
        var path = JsonAbsolutePath(name);
        using (var writer = new StreamWriter(path))
        {
            var json = JsonUtility.ToJson(data, true);
            writer.Write(json);
        }
    }

    public static void WriteDatas<T>(T[] datas, string name)
        where T : class
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new Exception("Name is not inputted");
        }
        var path = JsonAbsolutePath(name);
        using (var writer = new StreamWriter(path))
        {
            var wrapper = new Wrapper<T> { Items = datas };
            var json = JsonUtility.ToJson(wrapper, true);
            writer.Write(json);
        }
    }

    public static T ReadData<T>(string name)
        where T : class
    {
        var path = JsonPath(name);
        var json = Resources.Load<TextAsset>(path);

        if (json == null)
            return null;

        return JsonUtility.FromJson<T>(json.ToString());
    }

    public static List<T> ReadDatas<T>(string name)
        where T : class
    {
        var path = JsonPath(name);
        var json = Resources.Load<TextAsset>(path);

        if (json == null)
            return null;

        var wrapper = JsonUtility.FromJson<Wrapper<T>>(json.ToString());
        return wrapper.Items.ToList();
    }

    private static bool ValidatePath(string path)
    {
        return Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute);
    }
}
