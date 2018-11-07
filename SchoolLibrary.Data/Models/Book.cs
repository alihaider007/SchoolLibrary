namespace SchoolLibrary.Data.Models
{
    public class Book : EntityBase
    {
        public string Author { get; set; }
        public bool IsIssued { get; set; }
    }
}
