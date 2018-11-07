using SchoolLibrary.Data;
using SchoolLibrary.Data.Models;
using System.Linq;

namespace SchoolLibrary.Services
{
    public class StudentService : IStudentService
    {
        private readonly SchoolLibraryContext _context;

        public StudentService(SchoolLibraryContext context)
        {
            _context = context;
        }

        public void Add(Student student)
        {
            var newStudent = Get(student.Id);

            if (newStudent == null)
            {
                _context.Add(student);
                _context.SaveChanges();
            }
        }

        public Student Get(int id)
        {
            return _context.Students.FirstOrDefault(student => student.Id == id);
        }
    }
}
