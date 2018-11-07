using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Data;
using SchoolLibrary.Data.Models;

namespace SchoolLibrary.Services
{
    public class BookAssignmentService : IBookAssignmentService
    {
        private readonly SchoolLibraryContext _context;

        public BookAssignmentService(SchoolLibraryContext context)
        {
            _context = context;
        }

        public string AssignBook(int bookId, int studentId)
        {
            var isValid = ValidateBookAndStudent(bookId, studentId);

            if (!isValid)
            {
                return "Either student or book does not exist!";
            }

            var isBookAlreadyIssued = _context.BookAssignToStudents.Any(a => a.Book.Id == bookId && a.Book.IsIssued == true);

            if (isBookAlreadyIssued)
            {
                return "Book is already issued to the student.";
            }

            var book = _context.Books.First(a => a.Id == bookId);

            book.IsIssued = true;

            _context.Update(book);

            var now = DateTime.Now;

            var student = _context.Students.FirstOrDefault(a => a.Id == studentId);

            var bookAssignToStudent = new BookAssignToStudent
            {
                BookId = bookId,
                Book = book,
                StudentId = studentId,
                Student = student,
                IssueDate = now,
                DueDate = now.AddDays(7)
            };

            _context.Add(bookAssignToStudent);
            _context.SaveChanges();

            return "Success";
        }

        public string ExtendDueDate(int bookId, int studentId, DateTime newDueDate)
        {
            var isValid = ValidateBookAndStudent(bookId, studentId);

            if (!isValid)
            {
                return "Either student or book does not exist!";
            }

            var assignedBook = _context.BookAssignToStudents.SingleOrDefault(a => a.Book.Id == bookId && a.Student.Id == studentId && a.Book.IsIssued == true);

            if (assignedBook != null)
            {
                if(assignedBook.DueDate >= newDueDate)
                {
                    return "New due date should be greater than current due date.";
                }

                assignedBook.DueDate = newDueDate;

                _context.Update(assignedBook);
                _context.SaveChanges();
                return "Success";
            }
            else
            {
                return "Not Record Found! Unable to extend due date.";
            }
        }

        public BookAssignToStudent GetAssignedBook(int bookId)
        {
            var bookAssignToStudent = _context.BookAssignToStudents
                                    .Include(b => b.Book)
                                    .Include(s => s.Student)
                                    .SingleOrDefault(book => book.BookId == bookId);

            return bookAssignToStudent;
        }

        public IEnumerable<BookAssignToStudent> GetOverdueBooks()
        {
            var overdueBooks = _context.BookAssignToStudents
                            .Include(b => b.Book)
                            .Include(s => s.Student)
                            .Where(b => b.DueDate < DateTime.Now);

            return overdueBooks;
        }

        private bool ValidateBookAndStudent(int bookId, int studentId)
        {
            var student = _context.Students.SingleOrDefault(s => s.Id == studentId);
            var book = _context.Books.SingleOrDefault(b => b.Id == bookId);

            if (student == null || book == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
