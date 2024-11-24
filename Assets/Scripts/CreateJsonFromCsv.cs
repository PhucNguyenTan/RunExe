using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public static class CreateJsonFromCSV
{
    private static string[][] ReadCSV(string text)
    {
        List<string[]> rows = new List<string[]>();
        string pattern = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))"; // Regex pattern to split by commas outside quotes
        string[] lines = text.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);

        foreach (string line in lines)
        {
            string[] values = Regex.Split(line, pattern);
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim('\"'); // Remove surrounding quotes
            }
            rows.Add(values);
        }

        return rows.ToArray();
    }

    private static string GetTextAsset(string fileName)
    {
        var path = Path.Combine(Application.streamingAssetsPath, fileName + ".csv");

        if (!File.Exists(path))
        {
            return string.Empty;
        }
        return File.ReadAllText(path);
    }

    public static List<T> CreateDatas<T>(string csvFileName)
        where T : BaseData
    {
        var text = GetTextAsset(csvFileName);
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }
        var maxtrix = ReadCSV(text);
        var datas = new List<T>();
        var colCount = maxtrix[0].Length;

        for (var i = 1; i < maxtrix.Length; i++)
        {
            var data = maxtrix[i];
            if (data.Length != colCount)
            {
                Debug.LogErrorFormat("Seem to be a missmatch between header and line {0}", i);
                continue;
            }
            var instance = (T)Activator.CreateInstance(typeof(T), new object[] { data });
            datas.Add(instance);
        }
#if UNITY_EDITOR
        JsonDataHelper.WriteDatas<T>(datas.ToArray(), csvFileName);
#endif
        return datas;
    }

    public static T CreateData<T>(string csvFileName)
        where T : BaseData
    {
        var text = GetTextAsset(csvFileName);
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }
        var maxtrix = ReadCSV(text);
        var colCount = maxtrix[0].Length;

            var rawData = maxtrix[1];
            if (rawData.Length != colCount)
            {
                //Debug.LogErrorFormat("Seem to be a missmatch between header and line {0}", i);
            }
            var data = (T)Activator.CreateInstance(typeof(T), new object[] { rawData });
#if UNITY_EDITOR
        JsonDataHelper.WriteData<T>(data, csvFileName);
#endif
        return data;
    }
}