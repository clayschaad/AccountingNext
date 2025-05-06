using System.IO.Compression;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Schaad.Accounting.Datasets;
using Schaad.Accounting.Interfaces;
using Schaad.Accounting.Models;

namespace Schaad.Accounting.UI.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject]
    private IViewService viewService { get; set; } = null!;
    
    [Inject]
    private IToastService toastService { get; set; } = null!;
    
    [Inject]
    private IMessageService messageService { get; set; } = null!;
    
    [Inject]
    private ISettingsService settingsService { get; set; } = null!;
    
    [Inject]
    private IFileService fileService { get; set; } = null!;
    
    private IQueryable<BankTransaction>? bankTransactionsQueryable;
    
    FluentInputFile? myFileByBuffer;
    int? progressPercent;
    string? progressTitle;
    bool IsCanceled;
    Dictionary<int, string> Files = new();
    
    protected override Task OnInitializedAsync()
    {
        bankTransactionsQueryable = viewService.GetOpenBankTransactionList().AsQueryable();  
        return base.OnInitializedAsync();  
    }
    
    async Task OnProgressChangeAsync(FluentInputFileEventArgs file)
    {
        progressPercent = file.ProgressPercent;
        progressTitle = file.ProgressTitle;

        // To cancel?
        file.IsCancelled = IsCanceled;

        // New file
        if (!Files.ContainsKey(file.Index))
        {
            var localFile = Path.GetTempFileName() + file.Name;
            Files.Add(file.Index, localFile);
        }

        // Write to the FileStream
        await file.Buffer.AppendToFileAsync(Files[file.Index]);
    }

    private async Task OnCompletedAsync(IEnumerable<FluentInputFileEventArgs> files)
    {
        progressPercent = myFileByBuffer!.ProgressPercent;
        progressTitle = myFileByBuffer!.ProgressTitle;

        foreach (var file in Files)
        {
            var extension = Path.GetExtension(file.Value);
            if (extension is ".xml")
            {
                await ImportXmlAndShowResultAsync(file.Value);
            }
            else if (extension is ".zip")
            {
                var unzippedFiles = UnzipAndListFiles(file.Value);
                foreach (var unzippedFile in unzippedFiles)
                {
                    await ImportXmlAndShowResultAsync(unzippedFile);
                    File.Delete(unzippedFile);
                }
            }
            
            File.Delete(file.Value);
        }
        
        bankTransactionsQueryable = viewService.GetOpenBankTransactionList().AsQueryable();
    }

    private async Task ImportXmlAndShowResultAsync(string fileName)
    {
        var messages = fileService.ImportAccountStatementFile(fileName);
        foreach (var message in messages)
        {
            await ShowImportResultAsync(message);
        }
    }
    
    private IReadOnlyList<string> UnzipAndListFiles(string zipFilePath)
    {
        var tempExtractPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));
        Directory.CreateDirectory(tempExtractPath);
        ZipFile.ExtractToDirectory(zipFilePath, tempExtractPath);
        return new List<string>(Directory.GetFiles(tempExtractPath, "*", SearchOption.AllDirectories));
    }

    private async Task ShowImportResultAsync(MessageDataset message)
    {
        var joinedDetails = string.Join(", ", message.Lines);
        var text = $"{message.Title}: {joinedDetails}";
        
        if (message.Status == MessageStatus.Success)
        {
            ShowToast(text, ToastIntent.Success);
        }
        else if (message.Status == MessageStatus.Info)
        {
            ShowToast(text, ToastIntent.Info);
        }
        else if (message.Status == MessageStatus.Warning)
        {
            ShowToast(text, ToastIntent.Warning);
        }
        else if (message.Status == MessageStatus.Error)
        {
            await ShowMessageAsync(text, MessageIntent.Error);
        }
    }

    private async Task ShowMessageAsync(string message, MessageIntent intent)
    {
        await messageService.ShowMessageBarAsync(message, intent, "MESSAGES_TOP");
    }
    
    private void ShowToast(string message, ToastIntent intent)
    {
        toastService.ShowToast(intent, message, 3000);
    }
}
