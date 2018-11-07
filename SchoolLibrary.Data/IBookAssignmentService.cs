using SchoolLibrary.Data.Models;
using System;
using System.Collections.Generic;

namespace SchoolLibrary.Data
{
    public interface IBookAssignmentService
    {
        string AssignBook(int bookId, int studentId);
        string ExtendDueDate(int bookId, int studentId, DateTime newDueDate);
        BookAssignToStudent GetAssignedBook(int bookId);
        IEnumerable<BookAssignToStudent> GetOverdueBooks();
    }
}
