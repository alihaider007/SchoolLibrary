using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Data.Models;

namespace SchoolLibrary.Data
{
    public class SchoolLibraryContext : DbContext
    {
        public SchoolLibraryContext()
        {

        }

        public SchoolLibraryContext (DbContextOptions<SchoolLibraryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Books { get; set; }

        public virtual DbSet<Student> Students { get; set; }

        public virtual DbSet<BookAssignToStudent> BookAssignToStudents { get; set; }
    }
}
