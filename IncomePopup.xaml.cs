using CommunityToolkit.Maui.Views;
using System.Text.RegularExpressions;

namespace FinancialManagement;

public partial class IncomePopup : Popup
{
    private IncomeViewModel ViewModel => BindingContext as IncomeViewModel;
    private MainViewModel _mainPage;
    public bool IsEditing { get; set; } = false; 
    public IncomeOutcome EditingData { get; set; } 
    private readonly DatabaseService dbService;
    string databasePath = Path.Combine(AppContext.BaseDirectory, "Data", "database.db");
    public IncomePopup(MainViewModel mainPage)
    {
        InitializeComponent();
        dbService = new DatabaseService(databasePath);
        _mainPage = mainPage;
        IncomeTime.Time = DateTime.Now.TimeOfDay;
        IncomeCategory.SelectedIndex = 0;
        BindingContext = new IncomeViewModel();
    }

    private async void OnIncomeSubmitClicked(object sender, EventArgs e)
    {
        string value_text = IncomeValue.Text;
        string note = IncomeNote.Text;
        var selectedCategory = IncomeCategory.SelectedItem as IncomeCategories;
        string category;
        DateTime date = IncomeDate.Date;

        if (ViewModel.IsNewIncomeCategory)
        {
            category = NewIncomeCategory.Text;
            dbService.AddIncomeCategory(
                new IncomeCategories
                { 
                    ICategories = category
                }
            );
        }
        else
        {
            category = selectedCategory.ICategories as string;
        }

        if (string.IsNullOrWhiteSpace(note))
        {
            note = "None";
        }

        if (string.IsNullOrEmpty(value_text))
        {
            Application.Current.MainPage.DisplayAlert("Error", "Please input income value", "OK");
            return;
        }
        if (!Regex.IsMatch(value_text, @"^-?\d+(\.\d+)?$"))
        {
            Application.Current.MainPage.DisplayAlert("Error", "Invalid income value", "OK");
            return;
        }
        int.TryParse(value_text, out int value);

        if (category == "Choose income category" || string.IsNullOrWhiteSpace(category))
        {
            Application.Current.MainPage.DisplayAlert("Error", "Please choose category", "OK");
            return;
        }

        Application.Current.MainPage.DisplayAlert("Income infomation", $"Income: {value}\nCategory: {category}\nDate: {date:dd/MM/yyyy}\nNote: {note}", "OK");

        if (IsEditing)
        {
            dbService.UpdateInoutcome(
                new IncomeOutcome
                {
                    Id = EditingData.Id,
                    Value = value,
                    Type = "Income",
                    Category = category,
                    Date = date,
                    Note = note,
                    Timestamp = DateTime.Now
                }
            );
        }
        else
        {
            dbService.AddInoutcome(
                new IncomeOutcome
                {
                    Value = value,
                    Type = "Income",
                    Category = category,
                    Date = date,
                    Note = note,
                    Timestamp = DateTime.Now
                }
            );
        }

		if (_mainPage is MainViewModel viewModel)
		{
			viewModel.LoadData();
		}

        Close();
    }

    private void OnIncomeCategoryChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedValue = picker.SelectedItem as IncomeCategories;

        if (selectedValue.ICategories == "Add new income category")
        {
            ViewModel.IsNewIncomeCategory = true; 
        }
        else
        {
            ViewModel.IsNewIncomeCategory = false; 
        }
    }

    public void LoadIncomeDataForEdit(IncomeOutcome data)
    {
        IsEditing = true;
        IncomeDeleteBtn.IsVisible = true; 
        EditingData = data;
        IncomeValue.Text = data.Value.ToString(); 
        var category = IncomeCategory.ItemsSource
        .Cast<IncomeCategories>()
        .FirstOrDefault(c => c.ICategories == data.Category);
        if (category != null)
        {
            IncomeCategory.SelectedItem = category; // Gán đúng đối tượng
        }
        IncomeDate.Date = data.Date; 
        IncomeNote.Text = data.Note == "None" ? "" : data.Note; 
    }

    private void OnIncomeCloseClicked(object sender, EventArgs e)
    {
        Close();
    }
    private async void OnIncomeDeleteClicked(object sender, EventArgs e)
    {
        if (EditingData == null)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "No item to delete.", "OK");
            return;
        }

        dbService.DeleteInoutcome(EditingData.Id);

        _mainPage.LoadData();

        Close();
    }
}
