public class ItemData
{
    public int id;
    public string text;
    public string imageName;
    public string exeName;

    public ItemData(int id, string imageName, string exeName, string text)
    {
        this.id = id;
        this.text = text;
        this.imageName = imageName;
        this.exeName = exeName;
    }
}
