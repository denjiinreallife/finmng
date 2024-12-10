using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace FinancialManagement
{
    public class AppShellViewModel
    {
        private readonly DatabaseService dbService;
        string databasePath = Path.Combine(AppContext.BaseDirectory, "Data", "database.db");
        private string _usedStyleGraphText;
        public string UsedStyleGraphText
        {
            get => _usedStyleGraphText;
            set
            {
                if (_usedStyleGraphText != value)
                {
                    _usedStyleGraphText = value;
                    OnPropertyChanged(nameof(UsedStyleGraphText));
                }
            }
        }

        private string _usedStyleJamsText;
        public string UsedStyleJamsText
        {
            get => _usedStyleJamsText;
            set
            {
                if (_usedStyleJamsText != value)
                {
                    _usedStyleJamsText = value;
                    OnPropertyChanged(nameof(UsedStyleJamsText));
                }
            }
        }
        private string _usedUnitVNDText;
        public string UsedUnitVNDText
        {
            get => _usedUnitVNDText;
            set
            {
                if (_usedUnitVNDText != value)
                {
                    _usedUnitVNDText = value;
                    OnPropertyChanged(nameof(UsedUnitVNDText));
                }
            }
        }
        private string _usedUnitUSDText;
        public string UsedUnitUSDText
        {
            get => _usedUnitUSDText;
            set
            {
                if (_usedUnitUSDText != value)
                {
                    _usedUnitUSDText = value;
                    OnPropertyChanged(nameof(UsedUnitUSDText));
                }
            }
        }
        private string _usedLanguageEnglishText;
        public string UsedLanguageEnglishText
        {
            get => _usedLanguageEnglishText;
            set
            {
                if (_usedLanguageEnglishText != value)
                {
                    _usedLanguageEnglishText = value;
                    OnPropertyChanged(nameof(UsedLanguageEnglishText));
                }
            }
        }
        private string _usedLanguageVietnameseText;
        public string UsedLanguageVietnameseText
        {
            get => _usedLanguageVietnameseText;
            set
            {
                if (_usedLanguageVietnameseText != value)
                {
                    _usedLanguageVietnameseText = value;
                    OnPropertyChanged(nameof(UsedLanguageVietnameseText));
                }
            }
        }
        private string _unusedPotsDivideText;
        public string UnusedPotsDivideText
        {
            get => _unusedPotsDivideText;
            set
            {
                if (_unusedPotsDivideText != value)
                {
                    _unusedPotsDivideText = value;
                    OnPropertyChanged(nameof(UnusedPotsDivideText));
                }
            }
        }
        private string _usedPotsDivideText;
        public string UsedPotsDivideText
        {
            get => _usedPotsDivideText;
            set
            {
                if (_usedPotsDivideText != value)
                {
                    _usedPotsDivideText = value;
                    OnPropertyChanged(nameof(UsedPotsDivideText));
                }
            }
        }
        private bool _isPotsDivide;
        public bool IsPotsDivide
        {
            get => _isPotsDivide;
            set
            {
                _isPotsDivide = value;
                OnPropertyChanged(nameof(IsPotsDivide));
            }
        }
        // Commands
        public ICommand DataImportCommand { get; }
        public ICommand PDFDataExportCommand { get; }
        public ICommand ExcelDataExportCommand { get; }
        public ICommand ShowGraphStyleCommand { get; }
        public ICommand ShowJamsStyleCommand { get; }
        public ICommand UsedVNDCommand { get; }
        public ICommand UsedUSDCommand { get; }
        public ICommand UseEnglishCommand { get; }
        public ICommand UseVietnameseCommand { get; }
        public ICommand PotsUsedCommand { get; }
        public ICommand PotsUnusedCommand { get; }
        public ICommand DeleteDatabaseCommand { get; }

        public AppShellViewModel()
        {
            DataImportCommand = new Command(OnDataImport);
            PDFDataExportCommand = new Command(OnPDFDataExport);
            ExcelDataExportCommand = new Command(OnExcelDataExport);
            ShowGraphStyleCommand = new Command(ShowGraphStyle);
            ShowJamsStyleCommand = new Command(ShowJamsStyle);
            UsedVNDCommand = new Command(UsedVND);
            UsedUSDCommand = new Command(UsedUSD);
            UseEnglishCommand = new Command(UseEnglish);
            UseVietnameseCommand = new Command(UseVietnamese);
            PotsUsedCommand = new Command(PotsUsed);
            PotsUnusedCommand = new Command(PotsUnused);
            DeleteDatabaseCommand = new Command(DeleteDatabase);
		    dbService = new DatabaseService(databasePath);
            LoadConfig();
        }
        public void LoadConfig()
        {
            UserConfig currConfig = dbService.GetUserConfig();
            if (currConfig.UCVShow == VisualizationShow.Graph)
            {
                UsedStyleGraphText          = "• Graph";
                UsedStyleJamsText           = "  Jams";
            }
            else if (currConfig.UCVShow == VisualizationShow.Jams)
            {
                UsedStyleGraphText          = "  Graph";
                UsedStyleJamsText           = "• Jams";
            }
            
            if (currConfig.UCMoneyUnit == MoneyUnit.VND)
            {
                UsedUnitVNDText 	        = "• VND";
                UsedUnitUSDText	            = "  USD";
            }
            else if (currConfig.UCMoneyUnit == MoneyUnit.USD)
            {
                UsedUnitVNDText 	        = "  VND";
                UsedUnitUSDText	            = "• USD";
            }

            if (currConfig.UCLanguage == Language.English)
            {
                UsedLanguageEnglishText     = "• English";
                UsedLanguageVietnameseText	= "  Vietnamese";
            }
            else if (currConfig.UCLanguage == Language.Vietnamese)
            {
                UsedLanguageEnglishText 	= "  English";
                UsedLanguageVietnameseText	= "• Vietnamese";
            }
            
            if (currConfig.UCPots == Pots.Unused)
            {
                UnusedPotsDivideText 	    = "• Unuse";
                UsedPotsDivideText      	= "  Use";
                IsPotsDivide = false;
            }
            else if (currConfig.UCPots == Pots.Used)
            {
                UnusedPotsDivideText 	    = "  Unuse";
                UsedPotsDivideText      	= "• Use";
                IsPotsDivide = true;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnDataImport()
        {
            
        }

        private void OnPDFDataExport()
        {
            
        }

        private void OnExcelDataExport()
        {
            
        }

        private void ShowGraphStyle(object parameter)
        {
            UsedStyleGraphText          = "• Graph";
            UsedStyleJamsText           = "  Jams";
            UserConfig userConfig = dbService.GetUserConfig();
            userConfig.UCVShow = VisualizationShow.Graph;
            dbService.UpdateUserConfig(
                new UserConfig
                {
                    UCLanguage = userConfig.UCLanguage,
                    UCMoneyUnit = userConfig.UCMoneyUnit,
                    UCVShow = userConfig.UCVShow,
                    UCPots = userConfig.UCPots,
                }
            );
            dbService.UpdateUserConfig(userConfig);
        }

        private void ShowJamsStyle()
        {
            UsedStyleGraphText          = "  Graph";
            UsedStyleJamsText           = "• Jams";
            UserConfig userConfig = dbService.GetUserConfig();
            userConfig.UCVShow = VisualizationShow.Jams;
            dbService.UpdateUserConfig(
                new UserConfig
                {
                    UCLanguage = userConfig.UCLanguage,
                    UCMoneyUnit = userConfig.UCMoneyUnit,
                    UCVShow = userConfig.UCVShow,
                    UCPots = userConfig.UCPots,
                }
            );
            dbService.UpdateUserConfig(userConfig);
        }

        private void UsedVND()
        {
            UsedUnitVNDText 	        = "• VND";
            UsedUnitUSDText	            = "  USD";
            UserConfig userConfig = dbService.GetUserConfig();
            userConfig.UCMoneyUnit = MoneyUnit.VND;
            dbService.UpdateUserConfig(
                new UserConfig
                {
                    UCLanguage = userConfig.UCLanguage,
                    UCMoneyUnit = userConfig.UCMoneyUnit,
                    UCVShow = userConfig.UCVShow,
                    UCPots = userConfig.UCPots,
                }
            );
            dbService.UpdateUserConfig(userConfig);
        }

        private void UsedUSD()
        {
            UsedUnitVNDText 	        = "  VND";
            UsedUnitUSDText	            = "• USD";
            UserConfig userConfig = dbService.GetUserConfig();
            userConfig.UCMoneyUnit = MoneyUnit.USD;
            dbService.UpdateUserConfig(
                new UserConfig
                {
                    UCLanguage = userConfig.UCLanguage,
                    UCMoneyUnit = userConfig.UCMoneyUnit,
                    UCVShow = userConfig.UCVShow,
                    UCPots = userConfig.UCPots,
                }
            );
            dbService.UpdateUserConfig(userConfig);   
        }

        private void UseEnglish()
        {
            UsedLanguageEnglishText     = "• English";
            UsedLanguageVietnameseText	= "  Vietnamese";
            UserConfig userConfig = dbService.GetUserConfig();
            userConfig.UCLanguage = Language.English;
            dbService.UpdateUserConfig(
                new UserConfig
                {
                    UCLanguage = userConfig.UCLanguage,
                    UCMoneyUnit = userConfig.UCMoneyUnit,
                    UCVShow = userConfig.UCVShow,
                    UCPots = userConfig.UCPots,
                }
            );
            dbService.UpdateUserConfig(userConfig);  
        }

        private void UseVietnamese()
        {
            UsedLanguageEnglishText     = "  English";
            UsedLanguageVietnameseText	= "• Vietnamese";
            UserConfig userConfig = dbService.GetUserConfig();
            userConfig.UCLanguage = Language.Vietnamese;
            dbService.UpdateUserConfig(
                new UserConfig
                {
                    UCLanguage = userConfig.UCLanguage,
                    UCMoneyUnit = userConfig.UCMoneyUnit,
                    UCVShow = userConfig.UCVShow,
                    UCPots = userConfig.UCPots,
                }
            );
            dbService.UpdateUserConfig(userConfig);  
        }

        private void PotsUsed()
        {
            UsedPotsDivideText 	        = "• Use";
            UnusedPotsDivideText 	    = "  Unuse";
            IsPotsDivide = true;
            UserConfig userConfig = dbService.GetUserConfig();
            userConfig.UCPots = Pots.Used;
            dbService.UpdateUserConfig(
                new UserConfig
                {
                    UCLanguage = userConfig.UCLanguage,
                    UCMoneyUnit = userConfig.UCMoneyUnit,
                    UCVShow = userConfig.UCVShow,
                    UCPots = userConfig.UCPots,
                }
            );
            dbService.UpdateUserConfig(userConfig);
        }

        private void PotsUnused()
        {
            UsedPotsDivideText 	        = "  Use";
            UnusedPotsDivideText 	    = "• Unuse";
            IsPotsDivide = false;
            UserConfig userConfig = dbService.GetUserConfig();
            userConfig.UCPots = Pots.Unused;
            dbService.UpdateUserConfig(
                new UserConfig
                {
                    UCLanguage = userConfig.UCLanguage,
                    UCMoneyUnit = userConfig.UCMoneyUnit,
                    UCVShow = userConfig.UCVShow,
                    UCPots = userConfig.UCPots,
                }
            );
            dbService.UpdateUserConfig(userConfig);
        }

        private void DeleteDatabase()
        {
            // Logic for deleting the database
        }
    }
}
