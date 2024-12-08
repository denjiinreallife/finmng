namespace FinancialManagement;

public partial class AppShell : Shell
{
	private readonly DatabaseService dbService;
    string databasePath = Path.Combine(AppContext.BaseDirectory, "Data", "database.db");
	public AppShell()
	{
		InitializeComponent();
		dbService = new DatabaseService(databasePath);
	}

	private void OnDataImport(object sender, EventArgs e)
	{
		// 
	}
	private void OnPDFDataExport(object sender, EventArgs e)
	{
		// 
	}
	private void OnExcelDataExport(object sender, EventArgs e)
	{
		// 
	}
	private void ShowGraphStyle(object sender, EventArgs e)
	{
		// 
	}
	private void ShowJamsStyle(object sender, EventArgs e)
	{
		// 
	}
	private void UseEnglish(object sender, EventArgs e)
	{
		// 
	}
	private void Delete(object sender, EventArgs e)
	{
		// 
		// await dbService.DeleteDatabaseAsync(databasePath);
	}


}
