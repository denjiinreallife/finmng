using CommunityToolkit.Maui.Views;

namespace FinancialManagement;

public partial class IncomePopup : Popup
{
    private MainPage _mainPage;
    public bool IsEditing { get; set; } = false; 
    public inoutcomeData EditingData { get; set; } 
    string[] readyCategories = { "Salary", "Debt", "Interest rate", "Freelance jobs" };

    public IncomePopup(MainPage mainPage)
    {
        InitializeComponent();
        _mainPage = mainPage; // Lưu tham chiếu đến MainPage
        IncomeCategory.SelectedIndex = 0;
    }

    private async void OnIncomeSubmitClicked(object sender, EventArgs e)
    {
        string value_text = IncomeValue.Text;
        int.TryParse(value_text, out int value);
        string note = IncomeNote.Text;
        string category;
        DateTime date = IncomeDate.Date;

        if (IncomeCustomCategory.IsVisible)
        {
            category = IncomeCustomCategory.Text;
        }
        else
        {
            category = IncomeCategory.SelectedItem as string;
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

        if (category == "Choose income category" || string.IsNullOrWhiteSpace(category))
        {
            Application.Current.MainPage.DisplayAlert("Error", "Please choose category", "OK");
            return;
        }

        Application.Current.MainPage.DisplayAlert("Income infomation", $"Income: {value}\nCategory: {category}\nDate: {date:dd/MM/yyyy}\nNote: {note}", "OK");

        if (IsEditing)
        {
            var excelService = new ExcelService();
            string projectPath = AppContext.BaseDirectory;
            string dataFolderPath = Path.Combine(projectPath, "Data");
            string filePath = Path.Combine(dataFolderPath, "Test.xlsx");
            int rowIndexGeneral = excelService.FindRowIndex(filePath, "General", "ID", EditingData.ID); 
            if (rowIndexGeneral != -1)
            {
                excelService.UpdateEntry(filePath, "General", rowIndexGeneral, date, category, value, note);
                string[] IDParts = EditingData.ID.Split('_');
                int rowIndexInoutcome = excelService.FindRowIndex(filePath, EditingData.Type, "ID", IDParts[0]); 
                excelService.UpdateEntry(filePath, EditingData.Type, rowIndexInoutcome, date, category, value, note);
                _mainPage.LoadData(); 
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Unable to find entry to update", "OK");
            }
        }
        else
        {
            var excelService = new ExcelService();
            await excelService.SaveToExcel(true, date, category, value, note);
            _mainPage.LoadData(); 
        }

        Close();
    }

    private void OnIncomeCategoryChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedValue = picker.SelectedItem as string;

        if (selectedValue == "Other")
        {
            IncomeCustomCategory.IsVisible = true; 
        }
        else
        {
            IncomeCustomCategory.IsVisible = false; 
            var selectedCategory = selectedValue;
        }
    }

    public void LoadIncomeDataForEdit(inoutcomeData data)
    {
        IsEditing = true;
        EditingData = data;

        IncomeValue.Text = data.Value.ToString(); // Gán giá trị cho ô nhập số tiền

        if (readyCategories.Contains(data.Category))
        {
            IncomeCategory.SelectedItem = data.Category;
            IncomeCustomCategory.IsVisible = false;
        }
        else
        {
            IncomeCategory.SelectedItem = "Other";
            IncomeCustomCategory.IsVisible = true;
            IncomeCustomCategory.Text = data.Category;
        }
        IncomeDate.Date = data.Date; // Gán giá trị ngày tháng
        IncomeNote.Text = data.Note == "None" ? "" : data.Note; // Gán giá trị cho Note (dựa vào Type nếu phù hợp)
    }


    private void OnIncomeCloseClicked(object sender, EventArgs e)
    {
        Close(); // Đóng popup
    }
}
