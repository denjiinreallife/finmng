using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
namespace FinancialManagement;
public partial class MainPage : ContentPage
{
    public ObservableCollection<inoutcomeData> Data { get; set; }

	public MainPage()
	{
		InitializeComponent();
        string projectPath = AppContext.BaseDirectory;
        string dataFolderPath = Path.Combine(projectPath, "Data");
        string filePath = Path.Combine(dataFolderPath, "Test.xlsx");
        var excelService = new ExcelService();
        Data = excelService.ReadLatestData(filePath);
		BindingContext = this;
	}

	private void AddIncomeClicked(object sender, EventArgs e)
	{
        var popup = new IncomePopup(); 
        this.ShowPopup(popup);  
	}
	private void AddOutcomeClicked(object sender, EventArgs e)
	{
        var popup = new OutcomePopup(); 
        this.ShowPopup(popup);    
	}
	
	private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is inoutcomeData selectedItem)
		{
			// Mở popup và truyền dữ liệu hàng được chọn vào
			await Navigation.PushModalAsync(new EditPopup(selectedItem));
		}

		// Xóa lựa chọn để người dùng có thể nhấn lại vào cùng một hàng
		((CollectionView)sender).SelectedItem = null;
	}

}

