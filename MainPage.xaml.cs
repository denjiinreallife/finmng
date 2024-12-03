using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
namespace FinancialManagement;
public partial class MainPage : ContentPage
{
    public ObservableCollection<inoutcomeData> Data { get; set; } = new ObservableCollection<inoutcomeData>();

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
        var popup = new IncomePopup(this); 
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
			Popup popup;
			// Mở popup và truyền dữ liệu hàng được chọn vào
			if (selectedItem.Type == "Income") 
			{
				var incomePopup = new IncomePopup(this);
				incomePopup.LoadIncomeDataForEdit(selectedItem); // Truyền dữ liệu vào Popup
				popup = incomePopup;
			}
			else 
			{
				var outcomePopup = new  OutcomePopup();
				outcomePopup.LoadOutcomeDataForEdit(selectedItem); // Truyền dữ liệu vào Popup
				popup = outcomePopup;
			}
			Application.Current.MainPage.ShowPopup(popup);
			Console.WriteLine("vo chua vay");
		}

		// Xóa lựa chọn để người dùng có thể nhấn lại vào cùng một hàng
		((CollectionView)sender).SelectedItem = null;
	}

	public void LoadData()
	{
		string filePath = Path.Combine(AppContext.BaseDirectory, "Data", "Test.xlsx");
		var excelService = new ExcelService();

		Data.Clear(); // Xóa dữ liệu cũ
		var latestData = excelService.ReadLatestData(filePath);
		foreach (var item in latestData)
		{
			Data.Add(item); // Thêm dữ liệu mới
		}
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		LoadData(); // Tải lại dữ liệu từ Excel và đồng bộ với giao diện
	}

}

