using System.Globalization;
using Schaad.Accounting.Interfaces;
using Schaad.Accounting.Repositories;
using Schaad.Accounting.Services;
using Schaad.Finance.Api;
using Schaad.Finance.Services;

namespace Schaad.Accounting.UI
{
    public static class Extensions
    {
        public static string ToFormattedString(this decimal value)
        {
            var culture = new CultureInfo("de-CH");
            culture.NumberFormat.NumberGroupSeparator = "'";
            return value.ToString("#,0.00", culture);
        }
        
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IBankTransactionRepository, BankTransactionRepository>();
            services.AddScoped<IBookingRuleRepository, BookingRuleRepository>();
            services.AddScoped<IBookingTextRepository, BookingTextRepository>();
            services.AddScoped<ISplitPredefinitonRepository, SplitPredefinitonRepository>();
            services.AddScoped<ISubclassRepository, SubclassRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            
            services.AddScoped<IChartService, ChartService>();
            services.AddScoped<IViewService, ViewService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IAccountStatementService, AccountStatementService>();
            services.AddScoped<ICreditCardStatementService, CreditCardStatementService>();
            services.AddSingleton<IFxService, DummyFxService>();
            services.AddSingleton<IPdfParsingService, PdfParsingService>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IChartService, ChartService>();
            services.AddScoped<IViewService, ViewService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IAccountStatementService, AccountStatementService>();
            services.AddScoped<ICreditCardStatementService, CreditCardStatementService>();
            services.AddSingleton<IFxService, DummyFxService>();
            services.AddSingleton<PdfParsingService, PdfParsingService>();

            return services;
        }
    }
}
