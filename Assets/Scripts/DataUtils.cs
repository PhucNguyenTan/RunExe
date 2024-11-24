using System;
using System.Collections.Generic;

public static class DataUtils
{
    public static List<T> GetDatas<T>(string fileName)
        where T : BaseData
    {
        var stream = CreateJsonFromCSV.CreateDatas<T>(fileName);
        var inbuild = JsonDataHelper.ReadDatas<T>(fileName);
        if (inbuild == null && stream == null)
        {
            throw new ArgumentException($"Inbuild data {typeof(T).ToString()} is missing");
        }
        return stream == null ? inbuild : stream;
    }

    public static T GetData<T>(string fileName)
        where T : BaseData
    {
        var stream = CreateJsonFromCSV.CreateData<T>(fileName);
        var inbuild = JsonDataHelper.ReadData<T>(fileName);
        if (inbuild == null && stream == null)
        {
            throw new ArgumentException($"Inbuild data {typeof(T).ToString()} is missing");
        }
        return stream == null ? inbuild : stream;
    }

}