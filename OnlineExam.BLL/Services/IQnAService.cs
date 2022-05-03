using OnilneExa.DataAccessLyer;
using OnlineExam.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.BLL.Services
{
    public interface IQnAService
    {
        PagedResult<QnAsViewModel> GetAll(int pagedNumber, int pageSize);
        Task<QnAsViewModel> AddAsync(QnAsViewModel QnAVM);
        IEnumerable<QnAsViewModel> GetAllQnAByExam(int examId);
        bool IsExamAttended(int examId,int studentId);
    }
}
