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
    public class StudentServiceShould
    {
        [Fact]
        public void Add_New_Student()
        {
            var options = new DbContextOptionsBuilder<SchoolLibraryContext>()
                .UseInMemoryDatabase("Add_New_Student")
                .Options;

            using (var context = new SchoolLibraryContext(options))
            {
                var service = new StudentService(context);

                service.Add(new Student
                {
                    Id = 100
                });

                Assert.Equal(100, context.Students.Single().Id);
            }
        }

        [Fact]
        public void Get_Student_By_Id()
        {
            var students = new List<Student>
            {
                new Student
                {
                    Name = "Ali Haider",
                    Id = 1
                },

                new Student
                {
                    Name = "Muhammad Sohail",
                    Id = 2
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Student>>();
            mockSet.As<IQueryable<Student>>().Setup(b => b.Provider).Returns(students.Provider);
            mockSet.As<IQueryable<Student>>().Setup(b => b.Expression).Returns(students.Expression);
            mockSet.As<IQueryable<Student>>().Setup(b => b.ElementType).Returns(students.ElementType);
            mockSet.As<IQueryable<Student>>().Setup(b => b.GetEnumerator()).Returns(students.GetEnumerator);

            var mockCtx = new Mock<SchoolLibraryContext>();

            mockCtx.Setup(c => c.Students).Returns(mockSet.Object);

            var sut = new StudentService(mockCtx.Object);
            var book = sut.Get(1);

            Assert.Equal("Ali Haider", book.Name);
        }
    }
}
