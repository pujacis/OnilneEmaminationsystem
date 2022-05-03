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
    public class StudentService : IStudentService
    {
        IUnitOfwork _unitOfWork;
        ILogger<StudentService> _iLogger;

        public StudentService(IUnitOfwork unitOfWork, ILogger<StudentService> iLogger)
        {
            _unitOfWork = unitOfWork;
            _iLogger = iLogger;
        }

        public async Task<StudentViewModel> AddAsync(StudentViewModel vm)
        {
            try
            {
                Students obj = vm.ConvertViewModel(vm);
                await _unitOfWork.GenericRepository<Students>().AddAsync(obj);
            }
               catch (Exception ex)
            {
                return null;
            }
            return vm;
           
        }

        public PagedResult<StudentViewModel> GetAll(int pageNumber, int pageSize)
        {
            var model = new StudentViewModel();
            try
            {
                int ExcludeRecords = (pageSize * pageSize) - pageSize;
                List<StudentViewModel> detailList = new List<StudentViewModel>();
                var modelList = _unitOfWork.GenericRepository<Students>().GetAll().Skip(ExcludeRecords).Take(pageSize).ToList();
                var totalCount = _unitOfWork.GenericRepository<Students>().GetAll().ToList();
                detailList = GroupListInfo(modelList);
                if(detailList!=null)
                {
                    model.StudentList = detailList;
                    model.TotalCount = totalCount.Count();
                }

            }
            catch(Exception ex)
            {
                _iLogger.LogError(ex.Message);
            }
            var result = new PagedResult<StudentViewModel>
            {
                Data = model.StudentList,
                TotalItems = model.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,

            };
            return result;
        }

        private List<StudentViewModel> GroupListInfo(List<Students> modelList)
        {
            return modelList.Select(o=>new StudentViewModel(o)).ToList();
        }

        public IEnumerable<Students> GetAllstudents()
        {
            try
            {
              var students = _unitOfWork.GenericRepository<Students>().GetAll();
                return students;

            }
            catch(Exception ex)
            {
                _iLogger.LogError(ex.Message);
            }
            return Enumerable.Empty<Students>();
        }

        public IEnumerable<ResultViewModel> GetExamResult(int studentId)
        {
            try
            {
                var examResults = _unitOfWork.GenericRepository<ExamResults>().GetAll()
                    .Where(a => a.StudentsId == studentId);
                var students = _unitOfWork.GenericRepository<Students>().GetAll();
                var exams = _unitOfWork.GenericRepository<ExamResults>().GetAll();
                var qnas = _unitOfWork.GenericRepository<QnAs>().GetAll();
                var requiredDate = examResults.Join(students, er => er.StudentsId, s => s.Id,
                  (er, st) => new { er, st }).Join(exams, erj => erj.er.ExamsId, ex => ex.Id,
                  (erj, ex) => new { erj, ex }).Join(qnas, exj => exj.erj.er.QnAsId, q => q.Id,
                 (exj, q) => new ResultViewModel()
                 {
                    StudentId = studentId,
                    ExamName = exj.ex.Title,
                     TotalQuestion=examResults.Count(a=>a.StudentsId==studentId 
                     && a.ExamsId==exj.ex.Id), CorrectAnswer=examResults.Count(a=>a.StudentsId==studentId&&
                    a.ExamsId ==exj.ex.Id && a.Answer==q.Answer),
                       WrongAnswer=examResults.Count(a=>a.StudentsId==studentId&&
                       a.ExamsId==exj.ex.Id && a.Answer !=q.Answer)
                     
                 });
                return requiredDate;
                  
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);
            }
            return Enumerable.Empty<ResultViewModel>();
        }

        public StudentViewModel GetStudentDatails(int studentId)
        {
            try
            {
                var student = _unitOfWork.GenericRepository<Students>().GetById(studentId);
                return student != null? new StudentViewModel(student) : null;
            }
            catch (Exception ex)
            {
               _iLogger.LogError(ex.Message);

            }
            return null;
        }

        public bool SetExamResult(AttendExamViewModel vm)
        {
            try
            {
                foreach(var item in vm.QnAs)
                {
                   ExamResults examResults = new ExamResults();
                    examResults.StudentsId = vm.StudentId;
                    examResults.QnAsId = item.Id;
                    examResults.ExamsId = item.ExamsId;
                    examResults.Answer = item.SelectedAnswer;
                  _unitOfWork.GenericRepository<ExamResults>().AddAsync(examResults);

                }
                _unitOfWork.Save();

              return true;
            }
           
            catch(Exception ex)
            {
                _iLogger.LogError(ex.Message);
            }
            return false;
        }

        public bool SetGroupIdtoStudents(GroupViewModel vm)
        {
            try
            {
                foreach(var item in vm.StudentCheckList)
                {
                    var student =_unitOfWork.GenericRepository<Students>().GetById(item.Id);
                    if(item.Selected)
                    {
                        student.GroupsId = vm.Id;
                        _unitOfWork.GenericRepository<Students>().Update(student);
                    }
                    else
                    {
                        if(student.GroupsId==vm.Id)
                        {
                          student.GroupsId = null;
                        }
                    }
                    _unitOfWork.Save();
                    return true;
                }
            }
            catch(Exception ex)
            {
                _iLogger.LogError(ex.Message);
            }
            return false;
        }

        public async Task<StudentViewModel> UpdateAsync(StudentViewModel vm)
        {
            try
            {
                Students obj = _unitOfWork.GenericRepository<Students>().GetById(vm.Id);
                obj.Name = vm.Name;
                obj.UserName = vm.UserName;
                obj.Password = vm.Password;
                obj.PicturefileName = vm.PicturefileName !=null?
               vm.PicturefileName :obj.PicturefileName;
                obj.CVFileName = vm.CVFileName !=null?
                    vm.CVFileName :obj.CVFileName;
                obj.contact = vm.contact;
                await _unitOfWork.GenericRepository<Students>().UpdateAsync(obj);
                _unitOfWork.Save();



            }
            catch(Exception ex)
            {

            }
            return vm;
            
        }
    }
}
