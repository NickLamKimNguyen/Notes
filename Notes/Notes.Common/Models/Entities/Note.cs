using Notes.Common.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Common.Models.Entities
{
    public class Note : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Image"), DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        public DaysOfTheWeek DaysOfWeek { get; set; }
    }
}
