using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
namespace FinancialManagement;
public partial class MainPage : ContentPage
{
    public ObservableCollection<inoutcomeData> generalData { get; set; } = new ObservableCollection<inoutcomeData>();
    public ObservableCollection<inoutcomeData> incomeData { get; set; } = new ObservableCollection<inoutcomeData>();
    public ObservableCollection<inoutcomeData> outcomeData { get; set; } = new ObservableCollection<inoutcomeData>();

    string filePath = Path.Combine(AppContext.BaseDirectory, "Data", "Test.xlsx");
    ExcelService excelService = new ExcelService();
	public MainPage()
	{
		InitializeComponent();
        generalData = excelService.ReadLatestData(filePath, "General");
        incomeData = excelService.ReadLatestData(filePath, "Income");
        outcomeData = excelService.ReadLatestData(filePath, "Outcome");
		BindingContext = this;
	}

	private void AddIncomeClicked(object sender, EventArgs e)
	{
        var popup = new IncomePopup(this); 
        this.ShowPopup(popup);  
	}
	private void AddOutcomeClicked(object sender, EventArgs e)
	{
        var popup = new OutcomePopup(this); 
        this.ShowPopup(popup);    
	}

	private async void OnGeneralItemSelected(object sender, SelectionChangedEventArgs e)
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
				var outcomePopup = new  OutcomePopup(this);
				outcomePopup.LoadOutcomeDataForEdit(selectedItem); // Truyền dữ liệu vào Popup
				popup = outcomePopup;
			}
			Application.Current.MainPage.ShowPopup(popup);
		}

		// Xóa lựa chọn để người dùng có thể nhấn lại vào cùng một hàng
		((CollectionView)sender).SelectedItem = null;
	}

	private async void OnIncomeItemSelected(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is inoutcomeData selectedItem)
		{
			var popup = new IncomePopup(this);
			selectedItem.Type = "Income";
			selectedItem.ID = selectedItem.ID + "_Income";
			popup.LoadIncomeDataForEdit(selectedItem); 
			Application.Current.MainPage.ShowPopup(popup);
		}
		((CollectionView)sender).SelectedItem = null;
	}

	private async void OnOutcomeItemSelected(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is inoutcomeData selectedItem)
		{
			var popup = new OutcomePopup(this);
			selectedItem.Type = "Outcome";
			selectedItem.ID = selectedItem.ID + "_Outcome";
			popup.LoadOutcomeDataForEdit(selectedItem); 
			Application.Current.MainPage.ShowPopup(popup);
		}
		((CollectionView)sender).SelectedItem = null;
	}


	public void LoadData()
	{
		generalData.Clear(); 
		var latestGeneralData = excelService.ReadLatestData(filePath, "General");
		foreach (var item in latestGeneralData)
		{
			generalData.Add(item);
		}
		incomeData.Clear(); 
		var latestIncomeData = excelService.ReadLatestData(filePath, "Income");
		foreach (var item in latestIncomeData)
		{
			incomeData.Add(item);
		}
		outcomeData.Clear(); 
		var latestOutcomeData = excelService.ReadLatestData(filePath, "Outcome");
		foreach (var item in latestOutcomeData)
		{
			outcomeData.Add(item);
		}
	}
	private void ShowCollectionView(int index)
	{
		GeneralCollectionView.IsVisible = index == 1;
		IncomeCollectionView.IsVisible = index == 2;
		OutcomeCollectionView.IsVisible = index == 3;
	}
	private void ShowGeneralList(object sender, EventArgs e)
	{
		GeneralShowBtn.ScaleTo(0.95, 50); 
		GeneralShowBtn.BackgroundColor = Color.FromArgb("#1c1b1c"); 
		GeneralShowBtn.HasShadow = false; 
    	IncomeShowBtn.ScaleTo(1, 50); 
		IncomeShowBtn.BackgroundColor = Color.FromArgb("#000000"); 
		IncomeShowBtn.HasShadow = true; 
    	OutcomeShowBtn.ScaleTo(1, 50); 
		OutcomeShowBtn.BackgroundColor = Color.FromArgb("#000000"); 
		OutcomeShowBtn.HasShadow = true; 
		ShowCollectionView(1);
	}
	private void ShowIncomeList(object sender, EventArgs e)
	{
		IncomeShowBtn.ScaleTo(0.95, 50); 
		IncomeShowBtn.BackgroundColor = Color.FromArgb("#1c1b1c"); 
		IncomeShowBtn.HasShadow = false; 
    	GeneralShowBtn.ScaleTo(1, 50); 
		GeneralShowBtn.BackgroundColor = Color.FromArgb("#000000"); 
		GeneralShowBtn.HasShadow = true;
    	OutcomeShowBtn.ScaleTo(1, 50);
		OutcomeShowBtn.BackgroundColor = Color.FromArgb("#000000");
		OutcomeShowBtn.HasShadow = true; 
		ShowCollectionView(2);
	}
	private void ShowOutcomeList(object sender, EventArgs e)
	{
		OutcomeShowBtn.ScaleTo(0.95, 50);
		OutcomeShowBtn.BackgroundColor = Color.FromArgb("#1c1b1c"); 
		OutcomeShowBtn.HasShadow = false; 
    	GeneralShowBtn.ScaleTo(1, 50); 
		GeneralShowBtn.BackgroundColor = Color.FromArgb("#000000"); 
		GeneralShowBtn.HasShadow = true; 
    	IncomeShowBtn.ScaleTo(1, 50);
		IncomeShowBtn.BackgroundColor = Color.FromArgb("#000000"); 
		IncomeShowBtn.HasShadow = true;
		ShowCollectionView(3);
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		LoadData(); // Tải lại dữ liệu từ Excel và đồng bộ với giao diện
	}

}

