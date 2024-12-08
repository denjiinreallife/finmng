using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FinancialManagement;
public class IncomeViewModel : INotifyPropertyChanged
{
    private bool _isNewIncomeCategory;
    public ObservableCollection<IncomeCategories> IncomeCategories { get ; set; } = new ObservableCollection<IncomeCategories>();
	private readonly DatabaseService dbService;
    string databasePath = Path.Combine(AppContext.BaseDirectory, "Data", "database.db");
    public bool IsNewIncomeCategory
    {
        get => _isNewIncomeCategory;
        set
        {
            _isNewIncomeCategory = value;
            OnPropertyChanged(nameof(IsNewIncomeCategory));
        }
    }
    public IncomeViewModel()
    {
		dbService = new DatabaseService(databasePath);
    	LoadIncomeCategory();
        IsNewIncomeCategory = false;
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
}
