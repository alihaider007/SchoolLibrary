using SchoolLibrary.Data.Models;

namespace SchoolLibrary.Data
{
    public interface IStudentService
    {
        void Add(Student student);
        Student Get(int id);
    }
}
