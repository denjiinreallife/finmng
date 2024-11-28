using CommunityToolkit.Maui.Views;

namespace FinancialManagement;

public partial class IncomePopup : Popup
{
    public IncomePopup()
    {
        InitializeComponent();
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

        if (category == "Choose income category" || string.IsNullOrWhiteSpace(category))
        {
            Application.Current.MainPage.DisplayAlert("Error", "Please choose category", "OK");
            return;
        }

        // Kiểm tra dữ liệu
        if (string.IsNullOrEmpty(value_text))
        {
            Application.Current.MainPage.DisplayAlert("Error", "Please input income value", "OK");
            return;
        }

        Application.Current.MainPage.DisplayAlert("Income infomation", $"Income: {value}\nCategory: {category}\nDate: {date:dd/MM/yyyy}\nNote: {note}", "OK");

        var excelService = new ExcelService();
        await excelService.SaveToExcel(true, date, category, value, note);

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


    private void OnIncomeCloseClicked(object sender, EventArgs e)
    {
        Close(); // Đóng popup
    }
}
