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
    public class ExamService : IExamService
    {
        IUnitOfwork _uniteofwork;
        ILogger<ExamService> _iLogger;

        public ExamService(IUnitOfwork uniteofwork, ILogger<ExamService> iLogger)
        {
            _uniteofwork = uniteofwork;
            _iLogger = iLogger;
        }

        public async Task<ExamViewModel> AddAsync(ExamViewModel examVM)
        {
            try
            {
                Exams objGroup = examVM.ConvertViewModel(examVM);
                await _uniteofwork.GenericRepository<Exams>().AddAsync(objGroup);
                _uniteofwork.Save();
            }
            catch (Exception ex)
            {
                return null;
            }
            return examVM;
        }

        public PagedResult<ExamViewModel> GetAll(int pagedNumber, int pageSize)
        {
            var model = new ExamViewModel();
            try
            {
                int ExcludeRecords = (pageSize * pagedNumber) - pageSize;
                List<ExamViewModel> detailList = new List<ExamViewModel>();
                var modelList = _uniteofwork.GenericRepository<Exams>().GetAll().Skip(ExcludeRecords)
                .Take(pageSize).ToList();
                var totalCount = _uniteofwork.GenericRepository<Exams>().GetAll().ToList();
                detailList = ExamListInfo(modelList);

                if (detailList != null)
                {
                    model.ExamsList = detailList;
                    model.TotalCount = totalCount.Count();
                }
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);
            }
            var result = new PagedResult<ExamViewModel>
            {
                Data = model.ExamsList,
                TotalItems = model.TotalCount,
                PageNumber = pagedNumber,
                PageSize = pageSize,

            };
            return result;
        }

        private List<ExamViewModel> ExamListInfo(List<Exams> modelList)
        {
            return modelList.Select(o => new ExamViewModel(o)).ToList();
        }

        public IEnumerable<Exams> GetAllExams()
        {

            try
            {
                var exams = _uniteofwork.GenericRepository<Exams>().GetAll();
                return exams;
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);

            }
            return Enumerable.Empty<Exams>();
        }
    }
}
