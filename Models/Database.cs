using SQLite;

public class IncomeOutcome
{
    [PrimaryKey, AutoIncrement]
    public int          IOId { get; set; }
    [NotNull]
    public float        IOValue { get; set; }
    [NotNull]
    public string       IOType { get; set; }
    [MaxLength(20), NotNull]
    public string       IOCategory { get; set; }
    [MaxLength(50)]
    public string       IOPotName { get; set; }
    [MaxLength(100)]
    public string       IONote { get; set; }
    [MaxLength(50)]
    public DateTime     IODate { get; set; }
    public DateTime     IOTimestamp { get; set; }
}

public class IncomeCategories
{
    [PrimaryKey, AutoIncrement]
    public int          IId { get; set; }
    [MaxLength(50), NotNull]
    public string       ICategories { get; set; }
    [MaxLength(50)]
    public string       IPotName { get; set; }
}

public class OutcomeCategories
{
    [PrimaryKey, AutoIncrement]
    public int          OId { get; set; }
    [MaxLength(50), NotNull]
    public string       OCategories { get; set; }
    [MaxLength(50)]
    public string       OPotName { get; set; }
}
public class MoneyPot
{
    [PrimaryKey, AutoIncrement]
    public int          PotId { get; set; }
    [MaxLength(50), NotNull]
    public string       PotName { get; set; }
    public double       PotValue { get; set; }
}

public enum MoneyUnit
{
    VND,
    USD
}
public enum Language
{
    English,
    Vietnamese
}
public enum VisualizationShow
{
    Graph,
    Jams
}
public enum Pots
{
    Unused,
    Used
}

public class UserConfig
{
    [PrimaryKey, AutoIncrement]
    public int                  UCId { get; set; }
    public Language             UCLanguage { get; set; }
    public MoneyUnit            UCMoneyUnit { get; set; }
    public VisualizationShow    UCVShow { get; set; }
    public Pots                 UCPots { get; set; }
}
// for future
public class Account
{
    [PrimaryKey, AutoIncrement]
    public int          Id { get; set; }
    public string       Username { get; set; }
    public string       Password { get; set; }
    public string       Email { get; set; }
}

