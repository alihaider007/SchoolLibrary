using SchoolLibrary.Data.Models;
using System.Collections.Generic;

namespace SchoolLibrary.Data
{
    public interface IBookService
    {
        IEnumerable<Book> GetAll();
        Book Get(int id);
        void Add(Book newBook);
    }
}
