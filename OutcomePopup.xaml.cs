using CommunityToolkit.Maui.Views;

namespace FinancialManagement;

public partial class OutcomePopup : Popup
{
    
    public bool IsEditing { get; set; } = false; 
    public inoutcomeData EditingData { get; set; } 
    string[] readyCategories = { "Debt" };

    public OutcomePopup()
    {
        InitializeComponent();
        OutcomeCategory.SelectedIndex = 0;
    }

    private async void OnOutcomeSubmitClicked(object sender, EventArgs e)
    {
        string value_text = OutcomeValue.Text;
        int.TryParse(value_text, out int value);
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

        if (category == "Choose outcome category" || string.IsNullOrWhiteSpace(category))
        {
            Application.Current.MainPage.DisplayAlert("Error", "Please choose category", "OK");
            return;
        }

        Application.Current.MainPage.DisplayAlert("Outcome infomation", $"Outcome: {value}\nCategory: {category}\nDate: {date:dd/MM/yyyy}\nNote: {note}", "OK");
        if (IsEditing)
        {
            Console.WriteLine("edit ne chu ko phai moi");
        }
        else
        {
            var excelService = new ExcelService();
            await excelService.SaveToExcel(false, date, category, value, note);
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
}
