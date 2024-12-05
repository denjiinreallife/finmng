using SQLite;

public class DatabaseService
{
    private readonly SQLiteConnection _database;

    private bool TableExists(string tableName)
    {
        var query = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}';";
        var result = _database.ExecuteScalar<string>(query);
        return !string.IsNullOrEmpty(result);
    }
    public DatabaseService(string dbPath)
    {
        string dataFolder = Path.GetDirectoryName(dbPath);
        if (!Directory.Exists(dataFolder))
        {
            Directory.CreateDirectory(dataFolder);
        }
        _database = new SQLiteConnection(dbPath);
        if (!TableExists("IncomeOutcome"))
        {
            _database.CreateTable<IncomeOutcome>();
        }
    }

    public List<IncomeOutcome> GetInoutcome(string? type = null)
    {
        var query = _database.Table<IncomeOutcome>().AsQueryable();
        if (!string.IsNullOrEmpty(type))
        {
            query = query.Where(x => x.Type == type);
        }
        return query.OrderByDescending(x => x.Date).ToList();
    }

    public int AddInoutcome(IncomeOutcome Data)
    {
        return _database.Insert(Data);
    }

    public int DeleteInoutcome(int id)
    {
        return _database.Delete<IncomeOutcome>(id);
    }

    public int UpdateInoutcome(IncomeOutcome Data)
    {
        return _database.Update(Data);
    }
}
