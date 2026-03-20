namespace Data;

// LSP: FileRepository<T> mund të zëvendësohet me DatabaseRepository<T> pa ndikuar sistemin.
public class FileRepository<T> : IRepository<T>
{
    private List<T> _data;
    private readonly string _filePath;
    private readonly Func<string, T> _deserialize;
    private readonly Func<T, string> _serialize;
    private readonly Func<T, int> _getId;

    public FileRepository(string filePath, Func<string, T> deserialize, Func<T, string> serialize, Func<T, int> getId)
    {
        _filePath = filePath;
        _deserialize = deserialize;
        _serialize = serialize;
        _getId = getId;
        _data = new List<T>();
        Load();
    }

    public List<T> GetAll() => new List<T>(_data);

    public T? GetById(int id) => _data.FirstOrDefault(item => _getId(item) == id);

    public void Add(T entity) => _data.Add(entity);

    public void Update(T entity)
    {
        int id = _getId(entity);
        int index = _data.FindIndex(item => _getId(item) == id);
        if (index >= 0) _data[index] = entity;
    }

    public void Delete(int id) => _data.RemoveAll(item => _getId(item) == id);

    public void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath) ?? "data");
        var lines = _data.Select(_serialize);
        File.WriteAllLines(_filePath, lines);
    }

    private void Load()
    {
        if (!File.Exists(_filePath)) return;
        foreach (var line in File.ReadAllLines(_filePath))
        {
            if (!string.IsNullOrWhiteSpace(line))
                _data.Add(_deserialize(line));
        }
    }
}
