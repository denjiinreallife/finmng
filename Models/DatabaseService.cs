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
        if (!TableExists("UserConfig"))
        {
            _database.CreateTable<UserConfig>();
            _database.Insert(
                new UserConfig 
                { 
                    UCLanguage  = Language.English,
                    UCVShow     = VisualizationShow.Graph,
                    UCMoneyUnit = MoneyUnit.VND,
                    UCPots      = Pots.Unused  
                }
            );
        }
        if (!TableExists("IncomeOutcome"))
        {
            _database.CreateTable<IncomeOutcome>();
        }
        if (!TableExists("IncomeCategories"))
        {
            _database.CreateTable<IncomeCategories>();
            _database.Insert(new IncomeCategories { ICategories = "Add new income category" });
        }
        if (!TableExists("OutcomeCategories"))
        {
            _database.CreateTable<OutcomeCategories>();
            _database.Insert(new OutcomeCategories { OCategories = "Add new outcome category" });
        }
        if (!TableExists("MoneyPot"))
        {
            _database.CreateTable<MoneyPot>();
            _database.Insert(new MoneyPot { PotName = "Total", PotValue= 0 });
        }
    }
    
    public async Task DeleteDatabaseAsync(string dbPath)
    {
        if (File.Exists(dbPath))
        {
            File.Delete(dbPath);
        }
    }

    /******************* IncomeOutcome TABLE FUNCTION START *******************/
    public List<IncomeOutcome> GetInoutcome(string? type = null)
    {
        var query = _database.Table<IncomeOutcome>().AsQueryable();
        if (!string.IsNullOrEmpty(type))
        {
            query = query.Where(x => x.IOType == type);
        }
        return query.OrderByDescending(x => x.IODate).ToList();
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

    public double GetTotalValue(string? type = null)
    {
        double totalIncome = _database.Table<IncomeOutcome>()
                                    .Where(x => x.IOType == "Income")
                                    .Sum(x => x.IOValue);
        double totalOutcome = _database.Table<IncomeOutcome>()
                                   .Where(x => x.IOType == "Outcome")
                                   .Sum(x => x.IOValue);

        if (!string.IsNullOrEmpty(type))
        {
            return (type == "Income" ? totalIncome : totalOutcome);
        }
        
        return totalIncome - totalOutcome;
    }
    /******************* IncomeOutcome TABLE FUNCTION END *******************/

    /******************* IncomeCategories TABLE FUNCTION START *******************/
    public List<IncomeCategories> GetIncomeCategories()
    {
        var query = _database.Table<IncomeCategories>().AsQueryable();
        return query.OrderByDescending(x => x.IId).ToList();
    }

    public int AddIncomeCategory(IncomeCategories Data)
    {
        return _database.Insert(Data);
    }

    public int DeleteIncomeCategory(string category)
    {
        return _database.Delete<IncomeCategories>(category);
    }

    public int UpdateIncomeCategories(IncomeCategories Data)
    {
        return _database.Update(Data);
    }
    /******************* IncomeCategories TABLE FUNCTION END *******************/

    /******************* OutcomeCategories TABLE FUNCTION START *******************/
    public List<OutcomeCategories> GetOutcomeCategories()
    {
        var query = _database.Table<OutcomeCategories>().AsQueryable();
        return query.OrderByDescending(x => x.OId).ToList();
    }

    public int AddOutcomeCategory(OutcomeCategories Data)
    {
        return _database.Insert(Data);
    }

    public int DeleteOutcomeCategory(string category)
    {
        return _database.Delete<OutcomeCategories>(category);
    }

    public int UpdateOutcomeCategories(OutcomeCategories Data)
    {
        return _database.Update(Data);
    }
    /******************* IncomeCategories TABLE FUNCTION END *******************/

    /******************* MoneyPots TABLE FUNCTION START *******************/
    public List<MoneyPot> GetMoneyPots()
    {
        var query = _database.Table<MoneyPot>().AsQueryable();
        return query.ToList();
    }

    public int AddMoneyPots(MoneyPot Data)
    {
        return _database.Insert(Data);
    }

    public int DeleteMoneyPots(int id)
    {
        return _database.Delete<MoneyPot>(id);
    }

    public int UpdateMoneyPots(MoneyPot Data)
    {
        return _database.Update(Data);
    }

    public double GetPotValue(string? type = null)
    {
        double PotValue = _database.Table<MoneyPot>()
                        .FirstOrDefault(x => x.PotName == "Total").PotValue;
        if (type != null) 
        {
            PotValue = _database.Table<MoneyPot>()
                        .FirstOrDefault(x => x.PotName == type).PotValue;
        }
        return PotValue;
    }
    /******************* MoneyPots TABLE FUNCTION END *******************/

    /******************* UserConfig TABLE FUNCTION START *******************/
    public UserConfig GetUserConfig()
    {
        return _database.Table<UserConfig>().First();
    }
    public int UpdateUserConfig(UserConfig Data)
    {
        return _database.Update(Data);
    }
}
