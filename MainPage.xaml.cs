using CommunityToolkit.Maui.Views;
namespace FinancialManagement;
public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
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
}

