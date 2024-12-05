using SQLite;

public class IncomeOutcome
{
    [PrimaryKey, AutoIncrement]
    public int          Id { get; set; }
    public float        Value { get; set; }
    public string       Type { get; set; }
    [MaxLength(20)]
    public string       Category { get; set; }
    [MaxLength(100)]
    public string       Note { get; set; }
    [MaxLength(50)]
    public DateTime     Date { get; set; }
    public DateTime     Timestamp { get; set; }
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

