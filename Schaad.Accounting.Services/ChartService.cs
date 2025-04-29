using System;
using System.Collections.Generic;
using System.Linq;
using Schaad.Accounting.Datasets.Charts;
using Schaad.Accounting.Interfaces;

namespace Schaad.Accounting.Services
{
    public class ChartService : IChartService
    {
        private readonly IAccountRepository accountRepository;
        private readonly ISettingsService settingsService;
        private readonly ISubclassRepository subclassRepository;
        private readonly IViewService viewService;

        public ChartService(
            ISettingsService settingsService,
            IViewService viewService,
            IAccountRepository accountRepository,
            ISubclassRepository subclassRepository)
        {
            this.settingsService = settingsService;
            this.viewService = viewService;
            this.accountRepository = accountRepository;
            this.subclassRepository = subclassRepository;
        }

        public IReadOnlyList<DataSerie> GetExpensesPerMonth()
        {
            var transactions = viewService.GetTransactionViewList().Where(a => a.TargetAccount.Class == ClassIds.Expenses);
            
            // More than one subclass xx
            if (transactions.Select(t => t.TargetAccount.SubClass).Distinct().Count() > 1)
            {
                return GetSubClassExpensesPerMonth();
            }
            
            // only one subclass xx (z.B. Mandant Mannenbach)
            return GetAccountExpensesPerMonth();
        }

        private List<DataSerie> GetSubClassExpensesPerMonth()
        {
            //var subclasses = subclassRepository.GetSubClassList();
            var transactions = viewService.GetTransactionViewList().Where(a => a.TargetAccount.Class == ClassIds.Expenses).ToList();
            var list = new List<DataSerie>();

            var newestTransaction = transactions.OrderByDescending(t => t.ValueDate).FirstOrDefault();
            var maxMonth = newestTransaction?.ValueDate.Month ?? 12;
            var year = settingsService.GetYear();

            // Group by subclass
            foreach (var grp in transactions.GroupBy(a => a.TargetAccount.SubClass).Select(a => new {Key = a.Key, List = a.ToList()}))
            {
                // Sum subclass transactions per month
                var groupedByMonth = grp.List.GroupBy(g => g.ValueDate.Month).ToDictionary(g => g.Key, g => g.ToList().Sum(s => s.Value));
                EnsureEntryForEveryMonth(groupedByMonth, maxMonth);

                //var subClass = subclasses.FirstOrDefault(s => s.Number == grp.List.First().TargetAccount.SubClass);
                list.Add(
                    new DataSerie(
                        Id: grp.List.First().TargetAccount.SubClass.ToString(),
                        Name: grp.List.First().TargetAccount.Name,
                        X: groupedByMonth.OrderBy(g => g.Key).Select(g => new DateOnly(year, g.Key, 1)).ToList(), 
                        Y: groupedByMonth.OrderBy(g => g.Key).Select(g => g.Value).ToList()
                    )
                );
            }
            return list;
        }

        private List<DataSerie> GetAccountExpensesPerMonth()
        {
            var list = new List<DataSerie>();
            var expensesAccounts = viewService.GetAccountViewList().Where(a => a.Class == ClassIds.Expenses);
            foreach (var account in expensesAccounts)
            {
                var serie = GetAccountExpensesPerMonth(account.Id, DateTime.Now.Year);
                if (serie != null)
                {
                    list.Add(serie);
                }
            }
            return list;
        }

        private DataSerie GetAccountExpensesPerMonth(string accountId, int year)
        {
            if (settingsService.TrySetYear(year))
            {
                var accountList = accountRepository.GetAccountList();
                var account = accountList.SingleOrDefault(a => a.Id == accountId);
                // perhaps we dont have the account for last year
                if (account == null)
                {
                    return null;
                }
                var transactions = viewService.GetTransactionViewList().Where(t => t.TargetAccountId == accountId).ToList();

                var newestTransaction = transactions.OrderByDescending(t => t.ValueDate).FirstOrDefault();
                var maxMonth = newestTransaction?.ValueDate.Month ?? 12;

                var groupedByMonth = transactions.GroupBy(g => g.ValueDate.Month).ToDictionary(g => g.Key, g => g.ToList().Sum(s => s.Value));
                EnsureEntryForEveryMonth(groupedByMonth, maxMonth);

                settingsService.SetYear(DateTime.Now.Year);

                return new DataSerie(
                    Id: accountId,
                    Name: account.Name,
                    X: groupedByMonth.OrderBy(g => g.Key).Select(g => new DateOnly(year, g.Key, 1)).ToList(), 
                    Y: groupedByMonth.OrderBy(g => g.Key).Select(g => g.Value).ToList()
                );
            }
            
            return null;
        }

        private void EnsureEntryForEveryMonth(Dictionary<int, decimal> values, int maxMonth = 12)
        {
            for (int i = 1; i <= maxMonth; i++)
            {
                if (values.ContainsKey(i) == false)
                {
                    values.Add(i, 0);
                }
            }
        }
    }
}