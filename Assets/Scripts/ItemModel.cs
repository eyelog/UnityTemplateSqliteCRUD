[System.Serializable]
public struct ItemModel
{
    public int id { get; set; }
    public string val { get; set; }

    public ItemModel (
        int id,
        string val
    ) {

        this.id = id;
        this.val = val;
    }
}