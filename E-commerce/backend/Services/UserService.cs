using ECommerce.Data;
using ECommerce.Models;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Services
{
    /// <summary>
    /// Business logic layer for User operations (register, login, manage).
    ///
    /// SOLID:
    ///   • Single Responsibility — auth and user management only.
    ///   • Dependency Inversion — depends on IRepository&lt;User&gt;.
    /// </summary>
    public class UserService
    {
        // ── Private Attributes ──────────────────────────────────────────────
        private readonly IRepository<User> _repository;

        // ── Constructor ─────────────────────────────────────────────────────
        public UserService(IRepository<User> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // ── Public Methods ──────────────────────────────────────────────────

        public IEnumerable<User> GetAll()           => _repository.GetAll();
        public User?             GetById(string id) => _repository.GetById(id);

        public User? GetByEmail(string email) =>
            _repository.GetAll().FirstOrDefault(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        public User Register(string name, string email, string password, UserRole role = UserRole.Buyer)
        {
            if (string.IsNullOrWhiteSpace(name))     throw new ArgumentException("Name required.");
            if (string.IsNullOrWhiteSpace(email))    throw new ArgumentException("Email required.");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password required.");
            if (GetByEmail(email) != null)            throw new InvalidOperationException("Email already registered.");

            var user = new User(name, email, HashPassword(password), role);
            _repository.Add(user);
            _repository.Save();
            return user;
        }

        /// <summary>Returns user if credentials are valid, otherwise null.</summary>
        public User? Login(string email, string password)
        {
            var user = GetByEmail(email);
            return user?.PasswordHash == HashPassword(password) ? user : null;
        }

        public void DeleteUser(string userId)
        {
            _repository.Delete(userId);
            _repository.Save();
        }

        // ── Private Helpers ─────────────────────────────────────────────────

        /// <summary>SHA-256 password hashing. Passwords are never stored in plain text.</summary>
        private static string HashPassword(string password) =>
            Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password))).ToLower();
    }
}
