namespace Models;

public class User
{
    private int _id;
    private string _email;
    private string _passwordHash;
    private string _name;
    private string _address;
    private UserRole _role;

    public User(int id, string email, string passwordHash, string name, string address, UserRole role = UserRole.Buyer)
    {
        _id = id;
        _email = email;
        _passwordHash = passwordHash;
        _name = name;
        _address = address;
        _role = role;
    }

    public int GetId() => _id;
    public string GetEmail() => _email;
    public string GetPasswordHash() => _passwordHash;
    public string GetName() => _name;
    public string GetAddress() => _address;
    public UserRole GetRole() => _role;

    public void SetName(string name) => _name = name;
    public void SetAddress(string address) => _address = address;

    public string ToCsv() => $"{_id},{_email},{_passwordHash},{_name},{_address},{_role}";
}

public enum UserRole { Buyer, Seller, SuperAdmin }
