using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.BLL.Services;
using OnlineExam.viewModel;

namespace OnlineExamination.web.Controllers
{
    public class AccountCountrol : Controller
    {
        private readonly IAccountService _accountService;

        public AccountCountrol(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public IActionResult Login()
        {
            LoginViewModel sessionObj = HttpContext.Session.Get<LoginViewModel>("loginvm");
            if(sessionObj == null)
            return View();
            else
            {
                return RedirectUser(sessionObj);
            }
            
        }
        public IActionResult Logout()
        {
            HttpContent.Session.Set<LoginViewModel>("loginvm", null);
            return RedirectToAction("login");
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
             if (ModelState.IsValid)
            {
                LoginViewModel loginVM = _accountService.Login(LoginViewModel);
                if (loginVM != null)
                {
                    HttpContent.Session.Set<LoginViewModel>("loginvm",loginVM);
                    return RedirectUser(loginVM);
                }
            }
           
                return View(loginViewModel);
            
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RedirectUser(LoginViewModel loginViewModel)
        {
            if(loginViewModel.Role==(int)EnumRoles.Admin)
            {
                return RedirectToAction("Index", "Users");
            }
            else if (LoginViewModel.Role==(int)EnumRoles.Tacher)
            {
                return RedirectToAction("Index", "Exams");
            }
            return RedirectToAction("profile", "Students");
        }
    }
}
