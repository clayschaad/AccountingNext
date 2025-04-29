using System.ComponentModel.DataAnnotations;

namespace Schaad.Accounting.Models
{
    public class BookingRule
    {
        public string Id { get; set; }

        [Display(Name = "Suchtext")]
        [Required]
        public string LookupText { get; set; }

        [Display(Name = "Suchbetrag")]
        public decimal LookupValue { get; set; }
        
        [Display(Name = "Buchungstext")]
        [Required]
        public string BookingText { get; set; }

        [Display(Name = "Konto")]
        [Required]
        public string AccountId { get; set; }


        /// <summary>
        /// Makes a copy
        /// </summary>
        public void Copy(BookingRule target)
        {
            target.LookupText = LookupText;
            target.LookupValue = LookupValue;
            target.BookingText = BookingText;
            target.Id = Id;
            target.AccountId = AccountId;
        }
    }
}