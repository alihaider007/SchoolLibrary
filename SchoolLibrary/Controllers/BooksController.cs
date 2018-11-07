using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Data;
using SchoolLibrary.Data.Models;
using System.Collections.Generic;

namespace SchoolLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IBookAssignmentService _bookAssignmentService;

        public BooksController(IBookService bookService, IBookAssignmentService bookAssignmentService)
        {
            _bookService = bookService;
            _bookAssignmentService = bookAssignmentService;
        }

        // GET: api/Books
        [HttpGet]
        public IEnumerable<Book> GetBook()
        {
            return _bookService.GetAll();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public IActionResult GetBook([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // POST: api/Books
        [HttpPost]
        public IActionResult PostBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _bookService.Add(book);

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // GET: api/Books/Assign/5
        [HttpGet("Assign/{id}")]
        public IActionResult Assign([FromRoute] int id)
        {
            var bookAssignToStudent = _bookAssignmentService.GetAssignedBook(id);

            if (bookAssignToStudent == null)
            {
                return NotFound();
            }

            return Ok(bookAssignToStudent);
        }

        // POST: api/Books/Assign
        [HttpPost("Assign")]
        public IActionResult Assign([FromBody] BookAssignmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookId = request.BookId;
            var studentId = request.StudentId;

            var response = _bookAssignmentService.AssignBook(bookId, studentId);

            if (response.Equals("Success"))
            {
                var bookAssignToStudent = _bookAssignmentService.GetAssignedBook(bookId);
                return CreatedAtAction("Assign", new { id = bookId }, bookAssignToStudent);
            }
            else
            {
                return BadRequest(response);
            }
        }

        // PUT: api/Books/Extend
        [HttpPut("Extend")]
        public IActionResult Extend([FromBody] ExtendDueDateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookId = request.BookId;
            var studentId = request.StudentId;
            var newDueDate = request.NewDueDate;

            var response = _bookAssignmentService.ExtendDueDate(bookId, studentId, newDueDate);

            if (response.Equals("Success"))
            {
                return NoContent();
            }
            else
            {
                return BadRequest(response);
            }
        }

        // GET: api/Books/Overdue
        [HttpGet("Overdue")]
        public IEnumerable<BookAssignToStudent> Overdue()
        {
            var overdueBooks = _bookAssignmentService.GetOverdueBooks();

            return overdueBooks;
        }
    }
}