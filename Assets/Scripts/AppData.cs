using System;

[Serializable]
public class AppData : BaseData
{
    public string title;
    public string startPath;

    public AppData(string[] strs) : base(strs)
    {
        var setupActions = new Action<string>[]
            {
                value => title      = value,
                value => startPath  = value
            };
        if (strs.Length != setupActions.Length)
        {
            throw new ArgumentException($"Data {typeof(AppData).ToString()} trying to create is not valid");
        }

        for (int i = 0; i < strs.Length; i++)
        {
            setupActions[i](strs[i]);
        }
    }
}
