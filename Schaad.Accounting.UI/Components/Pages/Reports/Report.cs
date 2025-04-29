using Schaad.Accounting.Interfaces;

namespace Schaad.Accounting.UI.Components.Pages.Reports;

public static class Report
{
    public static (string header, string footer) GetViewDataTitleAndFooter(string title, ISettingsService settingsService)
    {
        var header = $"{title} {settingsService.GetMandator()} {settingsService.GetYear()}";

        var footer = "Stand: ";
        if (DateTime.Now.Year == settingsService.GetYear())
        {
            footer += DateTime.Now.ToString("dd.MM.yyyy");
        }
        else
        {
            footer += "31.12." + settingsService.GetYear();
        }

        return (header, footer);
    }
}