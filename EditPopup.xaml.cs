namespace FinancialManagement;
public partial class EditPopup : ContentPage
{
    private inoutcomeData CurrentItem;

    public EditPopup(inoutcomeData item)
    {
        InitializeComponent();
        CurrentItem = item;

        // Bind dữ liệu từ item vào các ô Entry
        DateEntry.Text = item.Date.ToString("MM/dd/yyyy");
        TypeEntry.Text = item.Type;
        CategoryEntry.Text = item.Category;
        ValueEntry.Text = item.Value.ToString();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // Cập nhật dữ liệu từ Entry về CurrentItem
        if (DateTime.TryParse(DateEntry.Text, out var date))
            CurrentItem.Date = date;

        CurrentItem.Type = TypeEntry.Text;
        CurrentItem.Category = CategoryEntry.Text;

        if (double.TryParse(ValueEntry.Text, out var value))
            CurrentItem.Value = value;

        await Navigation.PopModalAsync(); // Đóng popup
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync(); // Đóng popup
    }
}
