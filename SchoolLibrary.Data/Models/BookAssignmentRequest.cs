using System.ComponentModel.DataAnnotations;

namespace SchoolLibrary.Data.Models
{
    public class BookAssignmentRequest
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public int StudentId { get; set; }
    }
}
