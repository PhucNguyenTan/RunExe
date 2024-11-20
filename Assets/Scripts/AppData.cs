using System;

[Serializable]
public class AppData
{
    public string title;
    public string testPath;

    public AppData (string title, string testPath)
    {
        this.title = title;
        this.testPath = testPath;
    }
}
