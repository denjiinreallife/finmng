using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FinancialManagement;
public class OutcomePopupViewModel : INotifyPropertyChanged
{
    private bool _isNewOutcomeCategory;
    public ObservableCollection<OutcomeCategories> OutcomeCategories { get ; set; } = new ObservableCollection<OutcomeCategories>();
	private readonly DatabaseService dbService;
    string databasePath = Path.Combine(AppContext.BaseDirectory, "Data", "database.db");
    public bool IsNewOutcomeCategory
    {
        get => _isNewOutcomeCategory;
        set
        {
            _isNewOutcomeCategory = value;
            OnPropertyChanged(nameof(IsNewOutcomeCategory));
        }
    }
    public OutcomePopupViewModel()
    {
		dbService = new DatabaseService(databasePath);
    	LoadOutcomeCategory();
        IsNewOutcomeCategory = false;
    }
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public void LoadOutcomeCategory()
    {
        OutcomeCategories.Clear(); 
		var outcomeCategory = dbService.GetOutcomeCategories();
		foreach (var item in outcomeCategory)
		{
			OutcomeCategories.Add(item);
		}
    }
}
