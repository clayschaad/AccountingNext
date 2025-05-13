using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;
using Schaad.Accounting.Datasets;
using Schaad.Accounting.Interfaces;
using Schaad.Accounting.Models;

namespace Schaad.Accounting.UI.Components.Pages.Dialogs;

public partial class TransactionSplitDialog : ComponentBase
{
    private EditContext editContext = null!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = null!;

    [Parameter]
    public Transaction Content { get; set; } = null!;
    
    [Inject]
    private ITransactionRepository transactionRepository { get; set; } = null!;
    
    [Inject]
    private IBankTransactionRepository bankTransactionRepository { get; set; } = null!;
    
    [Inject]
    private IAccountRepository accountRepository { get; set; } = null!;
    
    [Inject]
    private IBookingTextRepository bookingTextRepository { get; set; } = null!;
    
    [Inject]
    private IViewService viewService { get; set; } = null!;
    
    private List<Transaction> transactionList = new();
    private IReadOnlyList<AccountDataset> accounts = null!;
    private IReadOnlyList<string> bookingTexts = [];
    private decimal openAmount = 0;
    

    protected override void OnInitialized()
    {
        accounts = viewService.GetAccountViewList();
        bookingTexts = bookingTextRepository.GetBookingTextList().Select(b => b.Text).ToArray();
        var accountList = accountRepository.GetAccountList();
        var bankTrx = bankTransactionRepository.GetBankTransaction(Content.BankTransactionId);
        var trx = new Transaction(bankTrx, accountList);
        trx.Value = Math.Abs(bankTrx.Value);
        transactionList.Add(trx);
        editContext = new EditContext(trx);
    }
    
    private async Task AddTransactionAsync()
    {
        transactionList.Add(new Transaction
        {
            ValueDate = Content.ValueDate,
            BookingDate = Content.BookingDate,
            OriginAccountId = Content.OriginAccountId,
            Value = 0
        });
        
        await Task.CompletedTask;
    }

    private void CalculateOpenAmount()
    {
        openAmount = Content.Value + transactionList.Sum(t => t.Value);
    }

    private async Task SaveAsync()
    {
        if (editContext.Validate())
        {
            foreach (var transtaction in transactionList)
            {
                transactionRepository.SaveTransaction(transtaction);
            }
            await Dialog.CloseAsync(Content);
        }
    }

    private async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }
}