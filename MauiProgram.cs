using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace FinancialManagement;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        // builder.Services.AddSingleton<IncomePopupViewModel>();
        // builder.Services.AddSingleton<OutcomePopupViewModel>();
        // builder.Services.AddSingleton<AppShellViewModel>();
		// builder.Services.AddTransient<IncomePopupViewModel>(serviceProvider =>
		// {
		// 	var appShellViewModel = serviceProvider.GetRequiredService<AppShellViewModel>();
		// 	return new IncomePopupViewModel(appShellViewModel);
		// });
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
