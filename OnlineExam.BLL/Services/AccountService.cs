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
    public class AccountService : IAccountService
    {
        IUnitOfwork _unitOfWork;
        ILogger<StudentService> _iLogger; 
        

        public AccountService(IUnitOfwork unitOfWork, ILogger<StudentService> iLogger)
        {
            _unitOfWork = unitOfWork;
            _iLogger = iLogger;
        }

        public bool AddTeacher(UserViewModel vm)
        {
            try
            {
                Users obj = new Users()
                {
                    Name = vm.Name,
                    UserName = vm.UserName,
                    Password = vm.Password,
                    Role = (int)EnumRoles.Tacher

                };
                 _unitOfWork.GenericRepository<Users>().AddAsync(obj);
                _unitOfWork.Save();


            }
            catch (Exception ex)
            {
                 _iLogger.LogError(ex.Message);
                return false;
            }
              return true;
            
        }

        public PagedResult<UserViewModel> GetAllTeacher(int pageNumber, int pageSize)
        {
            var model = new UserViewModel();
            try
            {
                int ExcludeRecords = (pageSize * pageNumber) - pageSize;
                List<UserViewModel> detailList = new List<UserViewModel>();
                var modelList = _unitOfWork.GenericRepository<Users>().GetAll()
                    .Where(x => x.Role == (int)EnumRoles.Tacher).Skip(ExcludeRecords)
                    .Take(pageSize).ToList();
                detailList = ListInfo(modelList);
                if(detailList!=null)
                {
                  model.UserList=detailList;
                    model.TotalCount = _unitOfWork.GenericRepository<Users>().GetAll()
                        .Count(x => x.Role == (int)EnumRoles.Tacher);

                }
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);
            }
            var result = new PagedResult<UserViewModel>
            {
                Data = model.UserList,
                TotalItems = model.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize

            };
            return result;
        }

        private List<UserViewModel> ListInfo(List<Users> modelList)
        {
           return modelList.Select(o => new UserViewModel(o)).ToList();
        }

        public LoginViewModel Login(LoginViewModel vm)
        {
           if(vm.Role==(int)EnumRoles.Admin||vm.Role==(int)EnumRoles.Tacher)
            {
                var user = _unitOfWork.GenericRepository<Users>().GetAll().
                     FirstOrDefault(a => a.UserName == vm.UserName.Trim() &&
                    a.Password == vm.Password.Trim() && a.Role == vm.Role);
                if(user!=null)
                {
                    vm.Id = user.Id;
                    return vm;
                }


            }
           else
            {
                var student = _unitOfWork.GenericRepository<Students>().GetAll().
                      FirstOrDefault(a => a.UserName == vm.UserName.Trim() && a.Password == vm.Password.Trim()
                      && a.Password==vm.Password.Trim());
                if(student!=null)
                {
                   vm.Id = student.Id;

                }
                return vm; 
            }
           return null;
        }
    }
}
