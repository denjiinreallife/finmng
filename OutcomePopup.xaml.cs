using CommunityToolkit.Maui.Views;
using System.Text.RegularExpressions;

namespace FinancialManagement;

public partial class OutcomePopup : Popup
{
    private OutcomeViewModel ViewModel => BindingContext as OutcomeViewModel;
    private MainViewModel _mainPage;
    public bool IsEditing { get; set; } = false; 
    public IncomeOutcome EditingData { get; set; } 
    private readonly DatabaseService dbService;
    string databasePath = Path.Combine(AppContext.BaseDirectory, "Data", "database.db");
    public OutcomePopup(MainViewModel mainPage)
    {
        InitializeComponent();
        dbService = new DatabaseService(databasePath);
        _mainPage = mainPage;
        OutcomeTime.Time = DateTime.Now.TimeOfDay;
        OutcomeCategory.SelectedIndex = 0;
        BindingContext = new OutcomeViewModel();
    }

    private async void OnOutcomeSubmitClicked(object sender, EventArgs e)
    {
        string value_text = OutcomeValue.Text;
        string note = OutcomeNote.Text;
        var selectedCategory = OutcomeCategory.SelectedItem as OutcomeCategories;
        string category;
        DateTime date = OutcomeDate.Date;

        if (ViewModel.IsNewOutcomeCategory)
        {
            category = NewOutcomeCategory.Text;
            dbService.AddOutcomeCategory(
                new OutcomeCategories
                { 
                    OCategories = category
                }
            );
        }
        else
        {
            category = selectedCategory.OCategories as string;
        }

        if (string.IsNullOrWhiteSpace(note))
        {
            note = "None";
        }
        
        if (string.IsNullOrEmpty(value_text))
        {
            Application.Current.MainPage.DisplayAlert("Error", "Please input outcome value", "OK");
            return;
        }
        if (!Regex.IsMatch(value_text, @"^-?\d+(\.\d+)?$"))
        {
            Application.Current.MainPage.DisplayAlert("Error", "Invalid outcome value", "OK");
            return;
        }

        int.TryParse(value_text, out int value);

        if (category == "Choose outcome category" || string.IsNullOrWhiteSpace(category))
        {
            Application.Current.MainPage.DisplayAlert("Error", "Please choose category", "OK");
            return;
        }

        Application.Current.MainPage.DisplayAlert("Outcome infomation", $"Outcome: {value}\nCategory: {category}\nDate: {date:dd/MM/yyyy}\nNote: {note}", "OK");
        if (IsEditing)
        {
            dbService.UpdateInoutcome(
                new IncomeOutcome
                {
                    Id = EditingData.Id,
                    Value = value,
                    Type = "Outcome",
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
                    Type = "Outcome",
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
    private void OnOutcomeCategoryChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedValue = picker.SelectedItem as OutcomeCategories;

        if (selectedValue.OCategories == "Add new outcome category")
        {
            ViewModel.IsNewOutcomeCategory = true; 
        }
        else
        {
            ViewModel.IsNewOutcomeCategory = false; 
        }
    }

    public void LoadOutcomeDataForEdit(IncomeOutcome data)
    {
        IsEditing = true;
        OutcomeDeleteBtn.IsVisible = true; 
        EditingData = data;
        OutcomeValue.Text = data.Value.ToString(); 
        var category = OutcomeCategory.ItemsSource
        .Cast<OutcomeCategories>()
        .FirstOrDefault(c => c.OCategories == data.Category);
        if (category != null)
        {
            OutcomeCategory.SelectedItem = category; // Gán đúng đối tượng
        }
        OutcomeDate.Date = data.Date; 
        OutcomeNote.Text = data.Note == "None" ? "" : data.Note; 
    }

    private void OnOutcomeCloseClicked(object sender, EventArgs e)
    {
        Close(); 
    }
    private async void OnOutcomeDeleteClicked(object sender, EventArgs e)
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
