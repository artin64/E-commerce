namespace Models;

public class Category
{
    private int _id;
    private string _name;
    private string _storeId;
    private int _sortOrder;

    public Category(int id, string name, string storeId, int sortOrder = 0)
    {
        _id = id;
        _name = name;
        _storeId = storeId;
        _sortOrder = sortOrder;
    }

    public int GetId() => _id;
    public string GetName() => _name;
    public string GetStoreId() => _storeId;
    public int GetSortOrder() => _sortOrder;

    public void SetName(string name) => _name = name;
    public void SetSortOrder(int order) => _sortOrder = order;

    public string ToCsv() => $"{_id},{_name},{_storeId},{_sortOrder}";
}
