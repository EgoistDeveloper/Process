using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Process.Models.Diet
{
    public class DietLastSelectedFood
    {
        public long Id { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        [Required]
        public long DietFoodId { get; set; }
    }
}
