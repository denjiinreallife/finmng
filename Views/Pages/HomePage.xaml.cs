using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
namespace FinancialManagement;
public partial class HomePage : ContentPage
{
    public ObservableCollection<IncomeOutcome> generalData { get; set; } = new ObservableCollection<IncomeOutcome>();
    public ObservableCollection<IncomeOutcome> incomeData { get; set; } = new ObservableCollection<IncomeOutcome>();
    public ObservableCollection<IncomeOutcome> outcomeData { get; set; } = new ObservableCollection<IncomeOutcome>();
    public double totalInoutcome = 0;
	public double totalIncome = 0;
	public double totalOutcome = 0;
	private readonly DatabaseService dbService;
    string databasePath = Path.Combine(AppContext.BaseDirectory, "Data", "database.db");
	public HomePage()
	{
		InitializeComponent();
		dbService = new DatabaseService(databasePath);
        BindingContext = new HomePageViewModel();
	}

	private void AddIncomeClicked(object sender, EventArgs e)
	{
		if (BindingContext is HomePageViewModel homePageViewModel)
    	{
			var popup = new IncomePopup(homePageViewModel); 
			this.ShowPopup(popup);  
		}
	}
	private void AddOutcomeClicked(object sender, EventArgs e)
	{
		if (BindingContext is HomePageViewModel homePageViewModel)
    	{
        	var popup = new OutcomePopup(homePageViewModel); 
        	this.ShowPopup(popup);    
		}
	}

	private async void OnGeneralItemSelected(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is IncomeOutcome selectedItem)
		{
			Popup popup = null;
			if (selectedItem.IOType == "Income") 
			{
				if (BindingContext is HomePageViewModel homePageViewModel)
				{
					var incomePopup = new IncomePopup(homePageViewModel);
					incomePopup.LoadIncomeDataForEdit(selectedItem); 
					popup = incomePopup;
				}
			}
			else 
			{
				if (BindingContext is HomePageViewModel homePageViewModel)
				{
					var outcomePopup = new OutcomePopup(homePageViewModel);
					outcomePopup.LoadOutcomeDataForEdit(selectedItem);
					popup = outcomePopup;
				}
			}
			Application.Current.MainPage.ShowPopup(popup);
		}

		// Xóa lựa chọn để người dùng có thể nhấn lại vào cùng một hàng
		((CollectionView)sender).SelectedItem = null;
	}

	private async void OnIncomeItemSelected(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is IncomeOutcome selectedItem)
		{
			if (BindingContext is HomePageViewModel homePageViewModel)
			{
				var popup = new IncomePopup(homePageViewModel);
				popup.LoadIncomeDataForEdit(selectedItem); 
				Application.Current.MainPage.ShowPopup(popup);
			}
		}
		((CollectionView)sender).SelectedItem = null;
	}

	private async void OnOutcomeItemSelected(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is IncomeOutcome selectedItem)
		{
			if (BindingContext is HomePageViewModel homePageViewModel)
			{
				var popup = new OutcomePopup(homePageViewModel);
				popup.LoadOutcomeDataForEdit(selectedItem); 
				Application.Current.MainPage.ShowPopup(popup);
			}
		}
		((CollectionView)sender).SelectedItem = null;
	}
	private void ShowCollectionView(int index)
	{
		GeneralCollectionView.IsVisible = index == 1;
		TotalShow.IsVisible = index == 1;
		IncomeCollectionView.IsVisible = index == 2;
		IncomeShow.IsVisible = index == 2;
		OutcomeCollectionView.IsVisible = index == 3;
		OutcomeShow.IsVisible = index == 3;
		
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
		if (BindingContext is HomePageViewModel viewModel)
		{
			viewModel.LoadData(); // Gọi phương thức LoadData
		}
	}
}

