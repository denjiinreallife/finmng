using OfficeOpenXml; // Namespace của EPPlus
using OfficeOpenXml.Style;
using System.IO;
using System.Collections.ObjectModel;
using System.Globalization;

public class ExcelService
{
    public async Task SaveToExcel(bool isIncome, DateTime date, string category, int value, string note)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        string projectPath = AppContext.BaseDirectory;
        string dataFolderPath = Path.Combine(projectPath, "Data");
        string filePath = Path.Combine(dataFolderPath, "Test.xlsx");
        Console.WriteLine(filePath); 
        if (!Directory.Exists(dataFolderPath))
        {
            Directory.CreateDirectory(dataFolderPath);
        }
        
        FileInfo fileInfo = new FileInfo(filePath);

        using (var package = fileInfo.Exists ? new ExcelPackage(fileInfo) : new ExcelPackage())
        {
            if (package.Workbook.Worksheets.Count == 0)
            {
                package.Workbook.Worksheets.Add("General");
                package.Workbook.Worksheets.Add("Income");
                package.Workbook.Worksheets.Add("Outcome");
            }
            var genreralWorksheet = package.Workbook.Worksheets[0];
            var inoutWorksheet = isIncome 
                        ? package.Workbook.Worksheets[1] 
                        : package.Workbook.Worksheets[2];

            int inoutLastRow = inoutWorksheet.Dimension?.End.Row ?? 0;
            int generalLastRow = genreralWorksheet.Dimension?.End.Row ?? 0;

            if (inoutLastRow == 0)
            {
                inoutWorksheet.Cells[1, 1].Value = "Date";
                inoutWorksheet.Cells[1, 2].Value = "Category";
                inoutWorksheet.Cells[1, 3].Value = "Value";
                inoutWorksheet.Cells[1, 4].Value = "Note";
                inoutWorksheet.Cells[1, 5].Value = "Timestamp"; 
                inoutLastRow = 1;
            }

            if (generalLastRow == 0)
            {
                genreralWorksheet.Cells[1, 1].Value = "Date";
                genreralWorksheet.Cells[1, 2].Value = "Type";
                genreralWorksheet.Cells[1, 3].Value = "Category";
                genreralWorksheet.Cells[1, 4].Value = "Value";
                genreralWorksheet.Cells[1, 5].Value = "Note";
                genreralWorksheet.Cells[1, 6].Value = "Timestamp";
                // genreralWorksheet.Cells[1, 7].Value = "Summary";  
                generalLastRow = 1;
            }

            inoutWorksheet.Cells[inoutLastRow + 1, 1].Value = date;
            inoutWorksheet.Cells[inoutLastRow + 1, 1].Style.Numberformat.Format = "dd/mm/yyyy"; 
            inoutWorksheet.Cells[inoutLastRow + 1, 2].Value = category;
            inoutWorksheet.Cells[inoutLastRow + 1, 3].Value = value;
            inoutWorksheet.Cells[inoutLastRow + 1, 4].Value = note;
            inoutWorksheet.Cells[inoutLastRow + 1, 5].Value = DateTime.Now;
            inoutWorksheet.Cells[inoutLastRow + 1, 5].Style.Numberformat.Format = "dd/mm/yyyy";

            genreralWorksheet.Cells[generalLastRow + 1, 1].Value = date;
            genreralWorksheet.Cells[generalLastRow + 1, 1].Style.Numberformat.Format = "dd/mm/yyyy"; 
            genreralWorksheet.Cells[generalLastRow + 1, 2].Value = isIncome ? "Income" : "Outcome";
            genreralWorksheet.Cells[generalLastRow + 1, 3].Value = category;
            genreralWorksheet.Cells[generalLastRow + 1, 4].Value = value;
            genreralWorksheet.Cells[generalLastRow + 1, 5].Value = note;
            genreralWorksheet.Cells[generalLastRow + 1, 6].Value = DateTime.Now;
            genreralWorksheet.Cells[generalLastRow + 1, 6].Style.Numberformat.Format = "dd/mm/yyyy";
            // var previousValue = generalLastRow == 1 ? 0 : genreralWorksheet.Cells[generalLastRow, 7].Value;
            // genreralWorksheet.Cells[generalLastRow + 1, 7].Value = Convert.ToInt64(previousValue) + (isIncome ? value : -value);

            inoutWorksheet.Cells[inoutWorksheet.Dimension.Address].AutoFitColumns();
            genreralWorksheet.Cells[genreralWorksheet.Dimension.Address].AutoFitColumns();
            File.WriteAllBytes(filePath, package.GetAsByteArray());
        }

        await Application.Current.MainPage.DisplayAlert("Success", "Data saved to Excel!", "OK");
    }
    public ObservableCollection<inoutcomeData> ReadLatestData(string filePath)
    {
        var data = new ObservableCollection<inoutcomeData>();
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            var worksheet = package.Workbook.Worksheets[0]; // Lấy sheet đầu tiên
            int rows = worksheet.Dimension.Rows;

            for (int i = 2; i <= rows; i++) // Bỏ qua header (dòng 1)
            {
                var rowData = new inoutcomeData
                {
                    Date = DateTime.ParseExact(worksheet.Cells[i, 1].Text, "MM/dd/yyyy", CultureInfo.InvariantCulture).Date,
                    Type = worksheet.Cells[i, 2].Text,
                    Category = worksheet.Cells[i, 3].Text,
                    Value = double.Parse(worksheet.Cells[i, 4].Text)

                };
                data.Add(rowData);
            }
        }
        return data;
    }
}

public class inoutcomeData
{
    public DateTime Date { get; set; }
    public string Type { get; set; }
    public string Category { get; set; }
    public double Value { get; set; }
}