using System;

namespace SchoolLibrary.Data.Models
{
    public class BookAssignToStudent : BookAssignmentRequest
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public Student Student { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
