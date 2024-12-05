using CommunityToolkit.Maui.Views;
using System.Text.RegularExpressions;

namespace FinancialManagement;

public partial class OutcomePopup : Popup
{
    private MainViewModel _mainPage;
    public bool IsEditing { get; set; } = false; 
    public IncomeOutcome EditingData { get; set; } 
    string[] readyCategories = { "Debt" };
    private readonly DatabaseService dbService;
    string databasePath = Path.Combine(AppContext.BaseDirectory, "Data", "database.db");
    public OutcomePopup(MainViewModel mainPage)
    {
        InitializeComponent();
        dbService = new DatabaseService(databasePath);
        _mainPage = mainPage;
        OutcomeCategory.SelectedIndex = 0;
    }

    private async void OnOutcomeSubmitClicked(object sender, EventArgs e)
    {
        string value_text = OutcomeValue.Text;
        string note = OutcomeNote.Text;
        string category;
        DateTime date = OutcomeDate.Date;

        if (OutcomeCustomCategory.IsVisible)
        {
            category = OutcomeCustomCategory.Text;
        }
        else
        {
            category = OutcomeCategory.SelectedItem as string;
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
        var selectedValue = picker.SelectedItem as string;

        if (selectedValue == "Other")
        {
            OutcomeCustomCategory.IsVisible = true; 
        }
        else
        {
            OutcomeCustomCategory.IsVisible = false; 
            var selectedCategory = selectedValue;
        }
    }

    public void LoadOutcomeDataForEdit(IncomeOutcome data)
    {
        IsEditing = true;
        OutcomeDeleteBtn.IsVisible = true; 
        EditingData = data;

        OutcomeValue.Text = data.Value.ToString(); 

        if (readyCategories.Contains(data.Category))
        {
            OutcomeCategory.SelectedItem = data.Category;
            OutcomeCustomCategory.IsVisible = false;
        }
        else
        {
            OutcomeCategory.SelectedItem = "Other";
            OutcomeCustomCategory.IsVisible = true;
            OutcomeCustomCategory.Text = data.Category;
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
