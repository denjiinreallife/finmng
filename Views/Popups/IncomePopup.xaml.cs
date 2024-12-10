using CommunityToolkit.Maui.Views;
using System.Text.RegularExpressions;

namespace FinancialManagement;

public partial class IncomePopup : Popup
{
    private IncomePopupViewModel ViewModel => BindingContext as IncomePopupViewModel;
    private HomePageViewModel _homePage;
    private AppShellViewModel _appShell;
    public bool IsEditing { get; set; } = false; 
    public IncomeOutcome EditingData { get; set; } 
    private readonly DatabaseService dbService;
    string databasePath = Path.Combine(AppContext.BaseDirectory, "Data", "database.db");
    public IncomePopup(HomePageViewModel homePage)
    {
        InitializeComponent();
        dbService = new DatabaseService(databasePath);
        _homePage = homePage;
        IncomeTime.Time = DateTime.Now.TimeOfDay;
        BindingContext = new IncomePopupViewModel();
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
            if (selectedCategory != null)
            {
                category = selectedCategory.ICategories as string;
            }
            else
            {
                category = null;
            }
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
                    IOId = EditingData.IOId,
                    IOValue = value,
                    IOType = "Income",
                    IOCategory = category,
                    IODate = date,
                    IONote = note,
                    IOTimestamp = DateTime.Now
                }
            );
        }
        else
        {
            dbService.AddInoutcome(
                new IncomeOutcome
                {
                    IOValue = value,
                    IOType = "Income",
                    IOCategory = category,
                    IODate = date,
                    IONote = note,
                    IOTimestamp = DateTime.Now
                }
            );
        }

		if (_homePage is HomePageViewModel viewModel)
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

    private void OnPotChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedValue = picker.SelectedItem as MoneyPot;
        Console.WriteLine(selectedValue.PotName);
    }

    public void LoadIncomeDataForEdit(IncomeOutcome data)
    {
        IsEditing = true;
        IncomeDeleteBtn.IsVisible = true; 
        EditingData = data;
        IncomeValue.Text = data.IOValue.ToString(); 
        var category = IncomeCategory.ItemsSource
        .Cast<IncomeCategories>()
        .FirstOrDefault(c => c.ICategories == data.IOCategory);
        if (category != null)
        {
            IncomeCategory.SelectedItem = category; // Gán đúng đối tượng
        }
        IncomeDate.Date = data.IODate; 
        IncomeNote.Text = data.IONote == "None" ? "" : data.IONote; 
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

        dbService.DeleteInoutcome(EditingData.IOId);

        _homePage.LoadData();

        Close();
    }
}
