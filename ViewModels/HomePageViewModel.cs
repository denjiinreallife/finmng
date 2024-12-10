using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FinancialManagement;

public class HomePageViewModel : INotifyPropertyChanged
{
    public ObservableCollection<IncomeOutcome> generalData { get; set; } = new ObservableCollection<IncomeOutcome>();
    public ObservableCollection<IncomeOutcome> incomeData { get; set; } = new ObservableCollection<IncomeOutcome>();
    public ObservableCollection<IncomeOutcome> outcomeData { get; set; } = new ObservableCollection<IncomeOutcome>();
	private readonly DatabaseService dbService;
    string databasePath = Path.Combine(AppContext.BaseDirectory, "Data", "database.db");
    private double _totalGeneral;
    private double _totalIncome;
    private double _totalOutcome;
    public double totalGeneral
    {
        get => _totalGeneral;
        set
        {
            _totalGeneral = value;
            OnPropertyChanged(nameof(totalGeneral));
        }
    }

    public double totalIncome
    {
        get => _totalIncome;
        set
        {
            _totalIncome = value;
            OnPropertyChanged(nameof(totalIncome));
        }
    }
    public double totalOutcome
    {
        get => _totalOutcome;
        set
        {
            _totalOutcome = value;
            OnPropertyChanged(nameof(totalOutcome));
        }
    }

    public HomePageViewModel()
    {
		dbService = new DatabaseService(databasePath);
    	LoadData();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

	public void LoadData()
	{
		generalData.Clear(); 
		var latestGeneralData = dbService.GetInoutcome();
		for (int i = 0; i < Math.Min(latestGeneralData.Count, 5); i++)
		{
			generalData.Add(latestGeneralData[i]);
		}
		totalGeneral = dbService.GetTotalValue();
		incomeData.Clear(); 
		var latestIncomeData = dbService.GetInoutcome("Income");
		for (int i = 0; i < Math.Min(latestIncomeData.Count, 5); i++)
		{
			incomeData.Add(latestIncomeData[i]);
		}
		totalIncome = dbService.GetTotalValue("Income");
		outcomeData.Clear(); 
		var latestOutcomeData = dbService.GetInoutcome("Outcome");
		for (int i = 0; i < Math.Min(latestOutcomeData.Count, 5); i++)
		{
			outcomeData.Add(latestOutcomeData[i]);
		}
		totalOutcome = dbService.GetTotalValue("Outcome");
	}
}
