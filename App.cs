namespace Models;

public class Store
{
    private string _id;
    private string _name;
    private int _ownerId;
    private string _qrCode;
    private string _theme;
    private string _primaryColor;
    private string _backgroundColor;
    private bool _isActive;

    public Store(string id, string name, int ownerId)
    {
        _id = id;
        _name = name;
        _ownerId = ownerId;
        _qrCode = GenerateQrCodeUrl(id);
        _theme = "default";
        _primaryColor = "#2563EB";
        _backgroundColor = "#FFFFFF";
        _isActive = true;
    }

    public string GetId() => _id;
    public string GetName() => _name;
    public int GetOwnerId() => _ownerId;
    public string GetQrCode() => _qrCode;
    public string GetTheme() => _theme;
    public string GetPrimaryColor() => _primaryColor;
    public string GetBackgroundColor() => _backgroundColor;
    public bool IsActive() => _isActive;

    public void SetTheme(string theme) => _theme = theme;
    public void SetPrimaryColor(string color) => _primaryColor = color;
    public void SetBackgroundColor(string color) => _backgroundColor = color;
    public void SetActive(bool active) => _isActive = active;
    public void SetName(string name) => _name = name;

    private string GenerateQrCodeUrl(string storeId)
        => $"https://api.qrserver.com/v1/create-qr-code/?data=https://platform.com/store/{storeId}";

    public string ToCsv() => $"{_id},{_name},{_ownerId},{_theme},{_primaryColor},{_backgroundColor},{_isActive}";
}
