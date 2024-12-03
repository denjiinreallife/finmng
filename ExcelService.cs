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
                inoutWorksheet.Cells[1, 1].Value = "ID";
                inoutWorksheet.Cells[1, 2].Value = "Date";
                inoutWorksheet.Cells[1, 3].Value = "Category";
                inoutWorksheet.Cells[1, 4].Value = "Value";
                inoutWorksheet.Cells[1, 5].Value = "Note";
                inoutWorksheet.Cells[1, 6].Value = "Timestamp"; 
                inoutLastRow = 1;
            }

            if (generalLastRow == 0)
            {
                genreralWorksheet.Cells[1, 1].Value = "ID";
                genreralWorksheet.Cells[1, 2].Value = "Date";
                genreralWorksheet.Cells[1, 3].Value = "Category";
                genreralWorksheet.Cells[1, 4].Value = "Value";
                genreralWorksheet.Cells[1, 5].Value = "Note";
                genreralWorksheet.Cells[1, 6].Value = "Timestamp";  
                genreralWorksheet.Cells[1, 7].Value = "Type";
                generalLastRow = 1;
            }
            
            var ID = inoutLastRow;

            inoutWorksheet.Cells[inoutLastRow + 1, 1].Value = ID;
            inoutWorksheet.Cells[inoutLastRow + 1, 2].Value = date;
            inoutWorksheet.Cells[inoutLastRow + 1, 2].Style.Numberformat.Format = "dd/mm/yyyy"; 
            inoutWorksheet.Cells[inoutLastRow + 1, 3].Value = category;
            inoutWorksheet.Cells[inoutLastRow + 1, 4].Value = value;
            inoutWorksheet.Cells[inoutLastRow + 1, 5].Value = note;
            inoutWorksheet.Cells[inoutLastRow + 1, 6].Value = DateTime.Now;
            inoutWorksheet.Cells[inoutLastRow + 1, 6].Style.Numberformat.Format = "dd/mm/yyyy";

            genreralWorksheet.Cells[generalLastRow + 1, 1].Value = ID.ToString() + (isIncome ? "_Income" : "Outcome");
            genreralWorksheet.Cells[generalLastRow + 1, 2].Value = date;
            genreralWorksheet.Cells[generalLastRow + 1, 2].Style.Numberformat.Format = "dd/mm/yyyy"; 
            genreralWorksheet.Cells[generalLastRow + 1, 3].Value = category;
            genreralWorksheet.Cells[generalLastRow + 1, 4].Value = value;
            genreralWorksheet.Cells[generalLastRow + 1, 5].Value = note;
            genreralWorksheet.Cells[generalLastRow + 1, 6].Value = DateTime.Now;
            genreralWorksheet.Cells[generalLastRow + 1, 6].Style.Numberformat.Format = "dd/mm/yyyy";
            genreralWorksheet.Cells[generalLastRow + 1, 7].Value = isIncome ? "Income" : "Outcome";

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

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Excel file does not exist.");
            return data; // Trả về danh sách rỗng
        }

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            var worksheet = package.Workbook.Worksheets[0]; // Lấy sheet đầu tiên
            int rows = worksheet.Dimension.Rows;

            for (int i = 2; i <= rows; i++) // Bỏ qua header (dòng 1)
            {
                var rowData = new inoutcomeData
                {
                    ID = worksheet.Cells[i, 1].Text,
                    Date = DateTime.TryParse(worksheet.Cells[i, 2].Text, out DateTime parsedDate) 
                        ? parsedDate.Date 
                        : DateTime.MinValue,
                    Category = worksheet.Cells[i, 3].Text,
                    Value = double.Parse(worksheet.Cells[i, 4].Text),
                    Note = worksheet.Cells[i, 5].Text,
                    Type = worksheet.Cells[i, 7].Text
                };
                data.Add(rowData);
            }
        }
        return data;
    }

    public int FindRowIndex(string filePath, string sheetName, string columnName, string valueToFind)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        FileInfo fileInfo = new FileInfo(filePath);

        using (var package = new ExcelPackage(fileInfo))
        {
            var worksheet = package.Workbook.Worksheets[sheetName];
            if (worksheet == null)
            {
                throw new Exception($"Sheet '{sheetName}' not found.");
            }

            // Tìm cột cần kiểm tra
            int totalColumns = worksheet.Dimension?.End.Column ?? 0;
            int columnToSearch = -1;

            for (int col = 1; col <= totalColumns; col++)
            {
                if (worksheet.Cells[1, col].Text == columnName)
                {
                    columnToSearch = col;
                    break;
                }
            }

            if (columnToSearch == -1)
            {
                throw new Exception($"Column '{columnName}' not found in sheet '{sheetName}'.");
            }

            // Tìm dòng có giá trị khớp
            int totalRows = worksheet.Dimension?.End.Row ?? 0;
            for (int row = 2; row <= totalRows; row++) // Bỏ qua dòng tiêu đề
            {
                var cellValue = worksheet.Cells[row, columnToSearch].Value;
                if (cellValue != null && cellValue.ToString() == valueToFind)
                {
                    return row; // Trả về số dòng tìm thấy
                }
            }
        }

        return -1; // Không tìm thấy
    }

    public void UpdateEntry(string filePath, string sheetName, int rowIndex, DateTime date, string category, double value, string note)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        FileInfo fileInfo = new FileInfo(filePath);

        using (var package = new ExcelPackage(fileInfo))
        {
            var worksheet = package.Workbook.Worksheets[sheetName];
            if (worksheet == null)
            {
                throw new Exception($"Sheet '{sheetName}' not found in the Excel file.");
            }

            // Cập nhật các ô trong dòng được chọn
            worksheet.Cells[rowIndex, 2].Value = date.ToString("dd/MM/yyyy"); // Cột Date
            worksheet.Cells[rowIndex, 2].Style.Numberformat.Format = "dd/mm/yyyy"; 
            worksheet.Cells[rowIndex, 3].Value = category;                   // Cột Category
            worksheet.Cells[rowIndex, 4].Value = value;                      // Cột Value
            worksheet.Cells[rowIndex, 5].Value = note;                       // Cột Note

            package.Save(); // Lưu lại thay đổi
        }
    }


}

public class inoutcomeData
{
    public string ID { get; set; }
    public DateTime Date { get; set; }
    public string Category { get; set; }
    public double Value { get; set; }
    public string Note { get; set; }
    public string Type { get; set; }
}