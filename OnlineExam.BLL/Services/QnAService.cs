using Microsoft.Extensions.Logging;
using OnilneExa.DataAccessLyer;
using OnilneExa.DataAccessLyer.UnitOfWork;
using OnlineExam.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.BLL.Services
{
    public class QnAService : IQnAService
    {

        IUnitOfwork _uniteofwork;
        ILogger<QnAService> _iLogger;

        public QnAService(IUnitOfwork uniteofwork, ILogger<QnAService> iLogger)
        {
            _uniteofwork = uniteofwork;
            _iLogger = iLogger;
        }

        public async Task<QnAsViewModel> AddAsync(QnAsViewModel QnAVM)
        {
            try
            {
                QnAs objGroup = QnAVM.ConvertViewModel(QnAVM);
                await _uniteofwork.GenericRepository<QnAs>().AddAsync(objGroup);
                _uniteofwork.Save();
            }
            catch (Exception ex)
            {
                return null;
            }
            return QnAVM;
        }

        public PagedResult<QnAsViewModel> GetAll(int pagedNumber, int pageSize)
        {
            var model = new QnAsViewModel();
            try
            {
                int ExcludeRecords = (pageSize * pagedNumber) - pageSize;
                List<QnAsViewModel> detailList = new List<QnAsViewModel>();
                var modelList = _uniteofwork.GenericRepository<QnAs>().GetAll().Skip(ExcludeRecords)
                .Take(pageSize).ToList();
                var totalCount = _uniteofwork.GenericRepository<QnAs>().GetAll().ToList();
                detailList = ExamListInfo(modelList);

                if (detailList != null)
                {
                    model.QnAnsList = detailList;
                    model.TotalCount = totalCount.Count();
                }
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);
            }
            var result = new PagedResult<QnAsViewModel>
            {
                Data = model.QnAnsList,
                TotalItems = model.TotalCount,
                PageNumber = pagedNumber,
                PageSize = pageSize,

            };
            return result;
        }

        private List<QnAsViewModel> ListInfo(List<QnAs> modelList)
        {
            return modelList.Select(o => new QnAsViewModel(o)).ToList();
        }

        public IEnumerable<QnAsViewModel> GetAllQnAByExam(int examId)
        {
            try
            {
                var qnaList = _uniteofwork.GenericRepository<QnAs>().GetAll().Where(x=>x.ExamsId==examId);
                return ListInfo(qnaList.ToList());
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);

            }
            return Enumerable.Empty<QnAsViewModel>();
        }

        public bool IsExamAttended(int examId, int studentId)
        {
            try
            {
                var qnaRecord =_uniteofwork.GenericRepository<ExamResults>().GetAll()
                    .FirstOrDefault(x=>x.ExamsId==examId && x.StudentsId==studentId);
                return qnaRecord ==null?false:true;
            }
            catch(Exception ex)
            {
                _iLogger.LogError(ex.Message);
            }
            return false;
        }
    }
}
