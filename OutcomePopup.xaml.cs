using CommunityToolkit.Maui.Views;

namespace FinancialManagement;

public partial class OutcomePopup : Popup
{
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

        if (category == "Choose Outcome category" || string.IsNullOrWhiteSpace(category))
        {
            Application.Current.MainPage.DisplayAlert("Error", "Please choose category", "OK");
            return;
        }

        // Kiểm tra dữ liệu
        if (string.IsNullOrEmpty(value_text))
        {
            Application.Current.MainPage.DisplayAlert("Error", "Please input Outcome value", "OK");
            return;
        }

        Application.Current.MainPage.DisplayAlert("Outcome infomation", $"Outcome: {value}\nCategory: {category}\nDate: {date:dd/MM/yyyy}\nNote: {note}", "OK");
        
        var excelService = new ExcelService();
        await excelService.SaveToExcel(false, date, category, value, note);

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


    private void OnOutcomeCloseClicked(object sender, EventArgs e)
    {
        Close(); // Đóng popup
    }
}
