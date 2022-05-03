using OnilneExa.DataAccessLyer;
using OnlineExam.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.BLL.Services
{
    public interface IStudentService
    {
        PagedResult<StudentViewModel> GetAll(int pageNumber,int pageSize);
        Task<StudentViewModel> AddAsync(StudentViewModel vm);
        IEnumerable<Students> GetAllstudents();
        bool SetGroupIdtoStudents(GroupViewModel vm);
        bool SetExamResult(AttendExamViewModel vm);
        IEnumerable<ResultViewModel> GetExamResult(int studentId);
        StudentViewModel GetStudentDatails(int studentId);
        Task<StudentViewModel> UpdateAsync(StudentViewModel vm);
    }
}
