namespace Models;

public class GiftCard
{
    private string _code;
    private double _value;
    private string _storeId;
    private bool _isUsed;
    private DateTime _expiresAt;

    public GiftCard(string code, double value, string storeId, DateTime expiresAt)
    {
        _code = code;
        _value = value;
        _storeId = storeId;
        _isUsed = false;
        _expiresAt = expiresAt;
    }

    public string GetCode() => _code;
    public double GetValue() => _value;
    public string GetStoreId() => _storeId;
    public bool IsUsed() => _isUsed;
    public DateTime GetExpiresAt() => _expiresAt;
    public bool IsValid() => !_isUsed && DateTime.Now < _expiresAt;

    public void MarkAsUsed() => _isUsed = true;

    public string ToCsv() => $"{_code},{_value},{_storeId},{_isUsed},{_expiresAt:yyyy-MM-dd}";
}
