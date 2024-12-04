using CommunityToolkit.Maui.Views;
using System.Text.RegularExpressions;

namespace FinancialManagement;

public partial class OutcomePopup : Popup
{
    private MainPage _mainPage;
    public bool IsEditing { get; set; } = false; 
    public inoutcomeData EditingData { get; set; } 
    string[] readyCategories = { "Debt" };
    string filePath = Path.Combine(AppContext.BaseDirectory, "Data", "Test.xlsx");
    ExcelService excelService = new ExcelService();
    public OutcomePopup(MainPage mainPage)
    {
        InitializeComponent();
        _mainPage = mainPage; // Lưu tham chiếu đến MainPage
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
            await excelService.SaveToExcel(false, date, category, value, note);
            _mainPage.LoadData();
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

    public void LoadOutcomeDataForEdit(inoutcomeData data)
    {
        IsEditing = true;
        OutcomeDeleteBtn.IsVisible = true; 
        EditingData = data;

        OutcomeValue.Text = data.Value.ToString(); // Gán giá trị cho ô nhập số tiền

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
        OutcomeDate.Date = data.Date; // Gán giá trị ngày tháng
        OutcomeNote.Text = data.Note == "None" ? "" : data.Note; // Gán giá trị cho Note (dựa vào Type nếu phù hợp)
    }

    private void OnOutcomeCloseClicked(object sender, EventArgs e)
    {
        Close(); // Đóng popup
    }
    private async void OnOutcomeDeleteClicked(object sender, EventArgs e)
    {
        if (EditingData == null)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "No item to delete.", "OK");
            return;
        }

        excelService.DeleteItemFromExcel(filePath, EditingData);

        await Application.Current.MainPage.DisplayAlert("Success", "Item deleted successfully.", "OK");

        _mainPage.LoadData();

        Close();
    }
}
