using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Schaad.Accounting.Models
{
    public class Account
    {
        public string Id { get; set; }

        [Display(Name = "Nummer")]
        [Required]
        public int Number { get; set; }

        [Display(Name = "Name")]
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Display(Name = "Währung")]
        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string Currency { get; set; }

        [XmlIgnore]
        [Display(Name = "Klasse")]
        public int Class
        {
            get { return Number / 1000; }
        }

        [XmlIgnore]
        [Display(Name = "Subklasse")]
        public int SubClass
        {
            get { return Number / 100; }
        }

        /// <summary>
        /// If mapped to a bank account
        /// </summary>
        [Display(Name = "Bankkontonummer")]
        public string BankAccountNumber { get; set; }

        /// <summary>
        /// Start balance for acitvas
        /// </summary>
        [Display(Name = "Start Saldo")]
        public decimal StartBalance { get; set; }

        /// <summary>
        /// Last received balance from bank
        /// </summary>
        [Display(Name = "Letzter Banksaldo")]
        public decimal LastBankBalance { get; set; }

        [XmlIgnore]
        public bool IsFxAccount
        {
            get { return string.IsNullOrEmpty(Currency) == false && Currency != "CHF"; }
        }

        /// <summary>
        /// Makes a copy
        /// </summary>
        public void Copy(Account target)
        {
            target.LastBankBalance = LastBankBalance;
            target.StartBalance = StartBalance;
            target.BankAccountNumber = BankAccountNumber;
            target.Currency = string.IsNullOrEmpty(Currency) ? "CHF" : Currency;
            target.Id = Id;
            target.Name = Name;
            target.Number = Number;
        }
    }
}