using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Data;
using SchoolLibrary.Data.Models;
using SchoolLibrary.Services;
using System;
using System.Linq;
using Xunit;

namespace SchoolLibrary.Tests.Services
{
    public class BookAssignmentServiceShould
    {
        [Fact]
        public void Assign_Book_To_Student()
        {
            var options = new DbContextOptionsBuilder<SchoolLibraryContext>()
                .UseInMemoryDatabase("Book_Assignment_Student")
                .Options;

            using (var context = new SchoolLibraryContext(options))
            {
                context.Students.Add(new Student
                {
                    Id = 100,
                    Name = "Ali"
                });

                context.Books.Add(new Book
                {
                    Id = 555,
                    Name = "How to become a millinaire",
                    IsIssued = false,
                    Author = "XYZ"
                });

                context.SaveChanges();
            }

            using (var context = new SchoolLibraryContext(options))
            {
                var service = new BookAssignmentService(context);
                service.AssignBook(555, 100);
                var book = context.Books.Find(555);
                Assert.True(book.IsIssued);
            }
        }

        [Fact]
        public void Not_Assign_Same_Book_To_Multiple_Students()
        {
            var options = new DbContextOptionsBuilder<SchoolLibraryContext>()
                .UseInMemoryDatabase("Not_Assign_Same_Book_To_Multiple_Students")
                .Options;

            using (var context = new SchoolLibraryContext(options))
            {
                context.Students.Add(new Student
                {
                    Id = 100,
                    Name = "Ali"
                });

                context.Students.Add(new Student
                {
                    Id = 200,
                    Name = "Rehmat"
                });

                context.Books.Add(new Book
                {
                    Id = 555,
                    Name = "How to become a millinaire",
                    IsIssued = false,
                    Author = "XYZ"
                });

                context.SaveChanges();
            }

            using (var context = new SchoolLibraryContext(options))
            {
                var service = new BookAssignmentService(context);
                service.AssignBook(555, 100);
                
                var bookAssignedTo100 = context.BookAssignToStudents.SingleOrDefault(a => a.Student.Id == 100);
                Assert.NotNull(bookAssignedTo100);

                service.AssignBook(555, 200);
                var bookAssignedTo200 = context.BookAssignToStudents.SingleOrDefault(a => a.Student.Id == 200);
                Assert.Null(bookAssignedTo200);
            }
        }

        [Fact]
        public void Extend_DueDate_Of_Assigned_Book()
        {
            var options = new DbContextOptionsBuilder<SchoolLibraryContext>()
                .UseInMemoryDatabase("Extend_DueDate_Of_Assigned_Book")
                .Options;

            using (var context = new SchoolLibraryContext(options))
            {
                context.Students.Add(new Student
                {
                    Id = 100,
                    Name = "Ali"
                });

                context.Books.Add(new Book
                {
                    Id = 555,
                    Name = "How to become a millinaire",
                    IsIssued = false,
                    Author = "XYZ"
                });

                context.SaveChanges();
            }

            using (var context = new SchoolLibraryContext(options))
            {
                var service = new BookAssignmentService(context);
                service.AssignBook(555, 100);

                var assignedBook = service.GetAssignedBook(555);
                Assert.True(assignedBook.Book.IsIssued);

                var response = service.ExtendDueDate(assignedBook.BookId, assignedBook.StudentId, assignedBook.DueDate.AddDays(7));

                Assert.Equal("Success", response);
            }
        }

        [Fact]
        public void Get_Overdue_Books()
        {
            var options = new DbContextOptionsBuilder<SchoolLibraryContext>()
                .UseInMemoryDatabase("Get_Overdue_Books")
                .Options;

            using (var context = new SchoolLibraryContext(options))
            {
                context.Students.Add(new Student
                {
                    Id = 100,
                    Name = "Ali"
                });

                context.Books.Add(new Book
                {
                    Id = 555,
                    Name = "How to become a millinaire",
                    IsIssued = false,
                    Author = "XYZ"
                });

                context.Books.Add(new Book
                {
                    Id = 556,
                    Name = "Some New Book 1",
                    IsIssued = false,
                    Author = "XYZ 1"
                });

                context.Books.Add(new Book
                {
                    Id = 557,
                    Name = "Some New Book 2",
                    IsIssued = false,
                    Author = "XYZ 2"
                });

                context.SaveChanges();
            }

            using (var context = new SchoolLibraryContext(options))
            {
                var service = new BookAssignmentService(context);
                service.AssignBook(555, 100);
                service.AssignBook(556, 100);
                service.AssignBook(557, 100);

                var assignedBook1 = context.BookAssignToStudents.Single(b => b.BookId == 555);
                assignedBook1.DueDate = DateTime.Now.AddDays(-5);
                var assignedBook2 = context.BookAssignToStudents.Single(b => b.BookId == 557);
                assignedBook2.DueDate = DateTime.Now.AddDays(-5);

                context.UpdateRange(new object[] { assignedBook1, assignedBook2 });
                context.SaveChanges();

                var response = service.GetOverdueBooks();

                Assert.Equal(2, response.Count());
            }
        }
    }
}
