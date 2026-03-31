namespace ECommerce.Models
{
    public enum UserRole { Buyer, Vendor, SuperAdmin }

    /// <summary>
    /// Represents a registered platform user (buyer, vendor, or admin).
    /// </summary>
    public class User
    {
        // ── Private Attributes ──────────────────────────────────────────────
        private string   _userId;
        private string   _name;
        private string   _email;
        private string   _passwordHash;
        private UserRole _role;
        private DateTime _createdAt;

        // ── Public Properties ───────────────────────────────────────────────
        public string   UserId       { get => _userId;       set => _userId       = value ?? throw new ArgumentNullException(nameof(value)); }
        public string   Name         { get => _name;         set => _name         = value ?? throw new ArgumentNullException(nameof(value)); }
        public string   Email        { get => _email;        set => _email        = value ?? throw new ArgumentNullException(nameof(value)); }
        public string   PasswordHash { get => _passwordHash; set => _passwordHash = value ?? throw new ArgumentNullException(nameof(value)); }
        public UserRole Role         { get => _role;         set => _role         = value; }
        public DateTime CreatedAt    { get => _createdAt;    set => _createdAt    = value; }

        // ── Constructors ────────────────────────────────────────────────────
        public User()
        {
            _userId       = Guid.NewGuid().ToString("N")[..8];
            _name         = string.Empty;
            _email        = string.Empty;
            _passwordHash = string.Empty;
            _role         = UserRole.Buyer;
            _createdAt    = DateTime.UtcNow;
        }

        public User(string name, string email, string passwordHash, UserRole role = UserRole.Buyer)
        {
            _userId       = Guid.NewGuid().ToString("N")[..8];
            _name         = name         ?? throw new ArgumentNullException(nameof(name));
            _email        = email        ?? throw new ArgumentNullException(nameof(email));
            _passwordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            _role         = role;
            _createdAt    = DateTime.UtcNow;
        }

        // ── Public Methods ──────────────────────────────────────────────────

        /// <summary>Serializes user to CSV row.</summary>
        public string ToCsv() =>
            $"{_userId},{_name},{_email},{_passwordHash},{_role},{_createdAt:O}";

        /// <summary>Deserializes CSV row to User.</summary>
        public static User FromCsv(string csvLine)
        {
            var p = csvLine.Split(',');
            return new User
            {
                UserId       = p[0], Name  = p[1],
                Email        = p[2], PasswordHash = p[3],
                Role         = Enum.Parse<UserRole>(p[4]),
                CreatedAt    = DateTime.Parse(p[5])
            };
        }

        public override string ToString() =>
            $"[{_userId}] {_name} | {_email} | Role:{_role}";
    }
}
