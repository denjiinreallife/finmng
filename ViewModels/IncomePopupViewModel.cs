using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FinancialManagement;
public class IncomePopupViewModel : INotifyPropertyChanged
{
    private AppShellViewModel _appShellViewModel = new AppShellViewModel();
    private bool _isNewIncomeCategory;
    public ObservableCollection<IncomeCategories> IncomeCategories { get ; set; } = new ObservableCollection<IncomeCategories>();
	private readonly DatabaseService dbService;
    public ObservableCollection<MoneyPot> MoneyPots { get; set; } = new ObservableCollection<MoneyPot>();
    string databasePath = Path.Combine(AppContext.BaseDirectory, "Data", "database.db");
    public bool IsPotsDivide { get; set; }
    public bool IsNewIncomeCategory
    {
        get => _isNewIncomeCategory;
        set
        {
            _isNewIncomeCategory = value;
            OnPropertyChanged(nameof(IsNewIncomeCategory));
        }
    }
    public IncomePopupViewModel()
    {
		dbService = new DatabaseService(databasePath);
        IsNewIncomeCategory = false;
        if (_appShellViewModel is AppShellViewModel viewModel)
        {
            viewModel.LoadConfig();
            IsPotsDivide = viewModel.IsPotsDivide;
        }
    	LoadIncomeCategory();
        LoadMoneyPots();
    }
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public void LoadIncomeCategory()
    {
        IncomeCategories.Clear(); 
		var incomeCategory = dbService.GetIncomeCategories();
		foreach (var item in incomeCategory)
		{
			IncomeCategories.Add(item);
		}
    }
    public void LoadMoneyPots()
    {
        MoneyPots.Clear(); 
        var moneyPot = dbService.GetMoneyPots();
        foreach (var item in moneyPot)
        {
            MoneyPots.Add(item);
        }
    }
}
