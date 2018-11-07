using SchoolLibrary.Data;
using SchoolLibrary.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace SchoolLibrary.Services
{
    public class BookService : IBookService
    {
        private readonly SchoolLibraryContext _context;

        public BookService(SchoolLibraryContext context)
        {
            _context = context;
        }

        public void Add(Book newBook)
        {
            var book = Get(newBook.Id);

            if (book == null)
            {
                _context.Add(newBook);
                _context.SaveChanges();
            }
        }

        public Book Get(int id)
        {
            return _context.Books.FirstOrDefault(book => book.Id == id);
        }

        public IEnumerable<Book> GetAll()
        {
            return _context.Books;
        }
    }
}
