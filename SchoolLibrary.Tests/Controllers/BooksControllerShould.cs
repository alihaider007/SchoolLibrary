using Microsoft.AspNetCore.Mvc;
using Moq;
using SchoolLibrary.Controllers;
using SchoolLibrary.Data;
using SchoolLibrary.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SchoolLibrary.Tests.Controllers
{
    public class BooksControllerShould
    {
        private static IEnumerable<Book> GetBooks()
        {
            var books = new List<Book>
            {
                new Book
                {
                    Id = 100,
                    Name = "Software Engineering",
                    Author = "ABC"
                },

                new Book
                {
                    Id = 101,
                    Name = "Introduction to Computer Science",
                    Author = "XYZ"
                }
            };

            return books;
        }

        private static IEnumerable<Student> GetStudents()
        {
            var students = new List<Student>
            {
                new Student
                {
                    Id = 500,
                    Name = "Ali"
                },

                new Student
                {
                    Id = 501,
                    Name = "Sohail"
                }
            };

            return students;
        }

        private static IEnumerable<BookAssignToStudent> GetOverdueBooks()
        {
            var books = new List<BookAssignToStudent>
            {
                new BookAssignToStudent {
                    Id = 1,
                    BookId = 123,
                    Book = new Book{ Id = 123, Name = "Software Engineering", Author = "ABC", IsIssued = true},
                    IssueDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(-7),
                    StudentId = 100,
                    Student = new Student{ Id = 1,Name = "Ali"}
                },
                new BookAssignToStudent {
                    Id = 2,
                    BookId = 2,
                    Book = new Book{ Id = 124 ,Name = "Mathematics", Author = "XYZ", IsIssued = true},
                    IssueDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(-7),
                    StudentId = 100,
                    Student = new Student{ Id = 1 ,Name = "Ali"}
                }
            };

            return books;
        }

        [Fact]
        public void Get_Book_By_Id()
        {
            var mockBookService = new Mock<IBookService>();
            var mockBookAssignmentService = new Mock<IBookAssignmentService>();

            mockBookService.Setup(r => r.Get(100)).Returns(new Book() { Id = 100 });
            var controller = new BooksController(mockBookService.Object, mockBookAssignmentService.Object);

            var actionResult = controller.GetBook(100);

            var result = Assert.IsType<OkObjectResult>(actionResult);
            var book = Assert.IsType<Book>(result.Value);

            Assert.Equal(100, book.Id);

            mockBookService.Verify(s => s.Get(100), Times.Once());
        }

        [Fact]
        public void Get_All_Books()
        {
            var mockBookService = new Mock<IBookService>();
            var mockBookAssignmentService = new Mock<IBookAssignmentService>();

            mockBookService.Setup(r => r.GetAll()).Returns(GetBooks());
            var controller = new BooksController(mockBookService.Object, mockBookAssignmentService.Object);

            var books = controller.GetBook();

            Assert.Equal(2, books.Count());

            mockBookService.Verify(s => s.GetAll(), Times.Once());
        }

        [Fact]
        public void Assign_Book_To_Student()
        {
            var mockBookService = new Mock<IBookService>();
            var mockBookAssignmentService = new Mock<IBookAssignmentService>();

            mockBookAssignmentService.Setup(r => r.AssignBook(1, 100)).Returns("Success");
            var controller = new BooksController(mockBookService.Object, mockBookAssignmentService.Object);

            var request = new BookAssignmentRequest()
            {
                BookId = 1,
                StudentId = 100
            };

            var actionResult = controller.Assign(request);
            var result = Assert.IsType<CreatedAtActionResult>(actionResult);
            Assert.Equal("Assign", result.ActionName);

            mockBookAssignmentService.Verify(s => s.AssignBook(1, 100), Times.Once());
        }

        [Fact]
        public void Get_Assigned_Book()
        {
            var bookAssignment = new BookAssignToStudent
            {
                Id = 1,
                BookId = 123,
                Book = new Book { Id = 123, Name = "Software Engineering", Author = "ABC", IsIssued = true },
                IssueDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7),
                StudentId = 100,
                Student = new Student { Id = 1, Name = "Ali" }
            };

            var mockBookService = new Mock<IBookService>();
            var mockBookAssignmentService = new Mock<IBookAssignmentService>();

            mockBookAssignmentService.Setup(r => r.GetAssignedBook(123)).Returns(bookAssignment);
            var controller = new BooksController(mockBookService.Object, mockBookAssignmentService.Object);

            var actionResult = controller.Assign(123);
            var result = Assert.IsType<OkObjectResult>(actionResult);
            var assignedBook = Assert.IsType<BookAssignToStudent>(result.Value);
            Assert.True(assignedBook.Book.IsIssued);

            mockBookAssignmentService.Verify(s => s.GetAssignedBook(123), Times.Once());
        }

        [Fact]
        public void Extend_DueDate_Of_Assigned_Book()
        {
            var bookAssignment = new BookAssignToStudent
            {
                Id = 1,
                BookId = 123,
                Book = new Book { Id = 123, Name = "Software Engineering", Author = "ABC", IsIssued = true },
                IssueDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7),
                StudentId = 100,
                Student = new Student { Id = 1, Name = "Ali" }
            };

            var mockBookService = new Mock<IBookService>();
            var mockBookAssignmentService = new Mock<IBookAssignmentService>();
            var newDueDate = DateTime.Now.AddDays(10);

            mockBookAssignmentService.Setup(r => r.ExtendDueDate(123, 100, newDueDate)).Returns("Success");
            var controller = new BooksController(mockBookService.Object, mockBookAssignmentService.Object);

            var request = new ExtendDueDateRequest { BookId = 123, StudentId = 100, NewDueDate = newDueDate};
            var actionResult = controller.Extend(request);
            Assert.IsType<NoContentResult>(actionResult);
            
            mockBookAssignmentService.Verify(s => s.ExtendDueDate(123, 100, newDueDate), Times.Once());
        }

        [Fact]
        public void Get_Overdue_Books()
        {
            var mockBookService = new Mock<IBookService>();
            var mockBookAssignmentService = new Mock<IBookAssignmentService>();
            
            mockBookAssignmentService.Setup(r => r.GetOverdueBooks()).Returns(GetOverdueBooks());
            var controller = new BooksController(mockBookService.Object, mockBookAssignmentService.Object);

            var overdueBooks = controller.Overdue();
            Assert.Equal(2, overdueBooks.Count());

            mockBookAssignmentService.Verify(s => s.GetOverdueBooks(), Times.Once());
        }
    }
}
