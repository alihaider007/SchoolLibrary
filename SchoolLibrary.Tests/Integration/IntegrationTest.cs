using Newtonsoft.Json;
using SchoolLibrary.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SchoolLibrary.Tests.Integration
{
    public class IntegrationTest
    {
        private HttpClient client;
        public IntegrationTest()
        {
            client = new TestServerProvider().Client;
        }
        [Fact]
        public async Task Api_Post_New_Book()
        {
            //using (var client = new TestServerProvider().Client)
            //{
                var book = new Book { Id = 123, Author = "ABC", Name = "Test Book", IsIssued = false };
                var stringContent = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"api/Books/", stringContent);

                if (response.Content != null)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseBook = JsonConvert.DeserializeObject<Book>(responseContent);
                    Assert.Equal(book.Id, responseBook.Id);
                }
            //}
        }

        [Fact]
        public async Task Api_Get_All_Books()
        {
            //using (var client = new TestServerProvider().Client)
            //{
                var books = new List<Book>
                {
                    new Book { Id = 123, Author = "ABC", Name = "Test Book", IsIssued = false },
                    new Book { Id = 124, Author = "XYZ", Name = "Test Book 2", IsIssued = false }
                };
                
                foreach(var book in books)
                {
                    var stringContent = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");

                    var res = await client.PostAsync($"api/Books/", stringContent);

                    res.EnsureSuccessStatusCode();
                }

                var response = await client.GetAsync($"api/Books/");

                response.EnsureSuccessStatusCode();

                if (response.Content != null)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseBook = JsonConvert.DeserializeObject<IEnumerable<Book>>(responseContent);
                    Assert.Equal(2, responseBook.Count());
                }
            //}
        }

        [Fact]
        public async Task Api_Assign_Book()
        {
            //using (var client = new TestServerProvider().Client)
            //{
                var books = new List<Book>
                {
                    new Book { Id = 123, Author = "ABC", Name = "Test Book", IsIssued = false },
                    new Book { Id = 124, Author = "XYZ", Name = "Test Book 2", IsIssued = false }
                };

                var students = new List<Student>
                {
                    new Student { Id = 100, Name = "Student 1" }
                };

                await CallAPI($"api/Books/", books, client);
                await CallAPI($"api/Student/", students, client);

                var content = new StringContent(JsonConvert.SerializeObject(new BookAssignmentRequest { BookId = 123, StudentId = 100 }), Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"api/Books/Assign", content);

                response.EnsureSuccessStatusCode();

                if (response.Content != null)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var assignedBook = JsonConvert.DeserializeObject<BookAssignToStudent>(responseContent);
                    Assert.True(assignedBook.Book.IsIssued);
                }
            //}
        }

        [Fact]
        public async Task Api_Extend_Return_Date()
        {
            //using (var client = new TestServerProvider().Client)
            //{
                var books = new List<Book>
                {
                    new Book { Id = 123, Author = "ABC", Name = "Test Book", IsIssued = false }
                };

                var students = new List<Student>
                {
                    new Student { Id = 100, Name = "Student 1" }
                };

                var bookAssignments = new List<BookAssignmentRequest>
                {
                    new BookAssignmentRequest { BookId = 123, StudentId = 100 }
                };

                await CallAPI($"api/Books/", books, client);
                await CallAPI($"api/Student/", students, client);
                await CallAPI($"api/Books/Assign", bookAssignments, client);

                var extendDueDateRequest = new ExtendDueDateRequest
                {
                    BookId = 123,
                    StudentId = 100,
                    NewDueDate = DateTime.Now.AddDays(10)
                };

                var content = new StringContent(JsonConvert.SerializeObject(extendDueDateRequest), Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"api/Books/Extend", content);

                response.EnsureSuccessStatusCode();

                if (response.Content != null)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
                }
            //}
        }

        private async Task<HttpResponseMessage> CallAPI<T>(string apiUri, IEnumerable<T> list, HttpClient client)
        {
            var respose = new HttpResponseMessage();
            foreach (var obj in list)
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

                respose = await client.PostAsync(apiUri, stringContent);

                respose.EnsureSuccessStatusCode();
            }
            return respose;
        }
    }
}
