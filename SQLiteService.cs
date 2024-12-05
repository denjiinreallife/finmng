using SQLite;

public class DatabaseService
{
    private readonly SQLiteConnection _database;

    public DatabaseService(string dbPath)
    {
        _database = new SQLiteConnection(dbPath);
        _database.CreateTable<IncomeOutcome>();
    }

    public List<User> GetUsers()
    {
        return _database.Table<IncomeOutcome>().ToList();
    }

    public int AddUser(IncomeOutcome Data)
    {
        return _database.Insert(Data);
    }

    public int DeleteUser(int id)
    {
        return _database.Delete<IncomeOutcome>(id);
    }

    public int UpdateUser(IncomeOutcome Data)
    {
        return _database.Update(Data);
    }
}
