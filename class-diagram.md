namespace Models;

public class Advertisement
{
    private int _id;
    private string _storeId;
    private string _title;
    private string _imageUrl;
    private string _linkUrl;
    private AdPosition _position;
    private bool _isActive;

    public Advertisement(int id, string storeId, string title, string imageUrl, string linkUrl, AdPosition position)
    {
        _id = id;
        _storeId = storeId;
        _title = title;
        _imageUrl = imageUrl;
        _linkUrl = linkUrl;
        _position = position;
        _isActive = true;
    }

    public int GetId() => _id;
    public string GetStoreId() => _storeId;
    public string GetTitle() => _title;
    public string GetImageUrl() => _imageUrl;
    public string GetLinkUrl() => _linkUrl;
    public AdPosition GetPosition() => _position;
    public bool IsActive() => _isActive;

    public void SetActive(bool active) => _isActive = active;
    public void SetPosition(AdPosition position) => _position = position;
    public void SetTitle(string title) => _title = title;

    public string ToCsv() => $"{_id},{_storeId},{_title},{_imageUrl},{_linkUrl},{_position},{_isActive}";
}

public enum AdPosition { TopBanner, SideBar, HomeHero, BetweenProducts, Footer }
