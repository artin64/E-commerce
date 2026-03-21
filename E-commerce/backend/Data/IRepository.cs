namespace ECommerce.Data
{
    /// <summary>
    /// Generic Repository Interface — defines the contract for all data access.
    ///
    /// SOLID Principles applied:
    ///   • Interface Segregation Principle (ISP): focused only on CRUD + Save.
    ///   • Dependency Inversion Principle (DIP): Services depend on this
    ///     abstraction, NOT on concrete FileRepository implementations.
    ///
    /// This means storage can be swapped (CSV → SQLite → API) without
    /// touching a single line of business logic in the Services layer.
    /// </summary>
    public interface IRepository<T>
    {
        /// <summary>Returns all entities from the data source.</summary>
        IEnumerable<T> GetAll();

        /// <summary>Returns a single entity by its unique string ID. Null if not found.</summary>
        T? GetById(string id);

        /// <summary>Adds a new entity to the in-memory collection.</summary>
        void Add(T entity);

        /// <summary>Updates an existing entity in the in-memory collection.</summary>
        void Update(T entity);

        /// <summary>Removes an entity by ID from the in-memory collection.</summary>
        void Delete(string id);

        /// <summary>Persists all in-memory state to the underlying CSV file.</summary>
        void Save();
    }
}
