#if WINDOWS
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace FinancialManagement;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        BindingContext = new AppShellViewModel();
    }
	
	protected override void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is AppShellViewModel viewModel)
		{
			viewModel.LoadConfig();
		}
	}
}
