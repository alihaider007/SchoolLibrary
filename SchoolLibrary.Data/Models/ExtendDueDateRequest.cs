using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolLibrary.Data.Models
{
    public class ExtendDueDateRequest : BookAssignmentRequest
    {
        [Required]
        public DateTime NewDueDate { get; set; }
    }
}
