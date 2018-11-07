using Microsoft.EntityFrameworkCore;
using Moq;
using SchoolLibrary.Data;
using SchoolLibrary.Data.Models;
using SchoolLibrary.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SchoolLibrary.Tests.Services
{
    public class BookServiceShould
    {
        [Fact]
        public void Add_New_Book()
        {
            var options = new DbContextOptionsBuilder<SchoolLibraryContext>()
                .UseInMemoryDatabase("Add_New_Book")
                .Options;

            using (var context = new SchoolLibraryContext(options))
            {
                var service = new BookService(context);

                service.Add(new Book
                {
                    Id = 123
                });

                Assert.Equal(123, context.Books.Single().Id);
            }
        }

        [Fact]
        public void Get_Book_By_Id()
        {
            var books = new List<Book>
            {
                new Book
                {
                    Name = "Calculus 1",
                    Id = 100
                },

                new Book
                {
                    Name = "Alpha Bravo Charlie",
                    Id = 555
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Book>>();
            mockSet.As<IQueryable<Book>>().Setup(b => b.Provider).Returns(books.Provider);
            mockSet.As<IQueryable<Book>>().Setup(b => b.Expression).Returns(books.Expression);
            mockSet.As<IQueryable<Book>>().Setup(b => b.ElementType).Returns(books.ElementType);
            mockSet.As<IQueryable<Book>>().Setup(b => b.GetEnumerator()).Returns(books.GetEnumerator);

            var mockCtx = new Mock<SchoolLibraryContext>();

            mockCtx.Setup(c => c.Books).Returns(mockSet.Object);

            var sut = new BookService(mockCtx.Object);
            var book = sut.Get(555);

            Assert.Equal("Alpha Bravo Charlie", book.Name);
        }

        [Fact]
        public void Get_All_Books()
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
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Book>>();
            mockSet.As<IQueryable<Book>>().Setup(b => b.Provider).Returns(books.Provider);
            mockSet.As<IQueryable<Book>>().Setup(b => b.Expression).Returns(books.Expression);
            mockSet.As<IQueryable<Book>>().Setup(b => b.ElementType).Returns(books.ElementType);
            mockSet.As<IQueryable<Book>>().Setup(b => b.GetEnumerator()).Returns(books.GetEnumerator);

            var mockCtx = new Mock<SchoolLibraryContext>();
            mockCtx.Setup(c => c.Books).Returns(mockSet.Object);

            var sut = new BookService(mockCtx.Object);
            var queryResult = sut.GetAll().ToList();

            Assert.Equal(2, queryResult.Count);
            Assert.True(queryResult.Exists(b => b.Id == 100));
            Assert.True(queryResult.Exists(b => b.Name == "Introduction to Computer Science"));
        }
    }
}
