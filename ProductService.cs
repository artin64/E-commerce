namespace Data;

// OCP + DIP: IRepository është abstraktimi qendror.
// Çdo implementim i ri (DB, XML, API) nuk kërkon ndryshim të Services.
public interface IRepository<T>
{
    List<T> GetAll();
    T? GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
    void Save();
}
