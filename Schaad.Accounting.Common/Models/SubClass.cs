using System.ComponentModel.DataAnnotations;

namespace Schaad.Accounting.Models
{
    public class SubClass
    {
        public string Id { get; set; }

        [Display(Name = "Nummer")]
        [Required]
        public int Number { get; set; }

        [Display(Name = "Name")]
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        /// <summary>
        /// Makes a copy
        /// </summary>
        public void Copy(SubClass target)
        {
            target.Id = Id;
            target.Name = Name;
            target.Number = Number;
        }
    }
}