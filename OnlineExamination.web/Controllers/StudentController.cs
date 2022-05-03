using Microsoft.AspNetCore.Mvc;
using OnlineExam.BLL.Services;
using OnlineExam.viewModel;

namespace OnlineExamination.web.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IExamService _examService;
        private readonly IQnAService __qnAService;

        public StudentController(IStudentService studentService,
            IExamService examService, IQnAService qnAService)
        {
            _studentService = studentService;
            _examService = examService;
            __qnAService = qnAService;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            return View(_studentService.GetAll(pageNumber, pageSize));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(StudentViewModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                await _studentService.AddAsync(studentViewModel);
                return RedirectToAction(nameof(Index));
            }

            return View(studentViewModel);
        }
        public IActionResult AttendExam()
        {
            var model = new AttendExamViewModel();
            LoginViewModel sessionObj = HttpContent.
                Session.Get<LoginViewModel>("loginvm");
            if (sessionObj != null)
            {
                model.StudentId = Convert.ToInt32(sessionObj.Id);
                model.QnAs = new List<QnAsViewModel>();
                var todayExam = _examService.GetAllExams().
                    Where(a => a.StartDate.Date == DateTime.Today.Date).FirstOrDefault();
                if (todayExam == null)
                {
                    model.Message = "No Exam Scheduled today";
                }
                else
                {
                    if (!_qnAService.IsExamAttended(todayExam.Id, model.StudentId))
                    {
                        model.QnAs = __qnAService.GetAllQnAByExam(todayExam.Id).ToList();
                        model.ExamName = todayExam.Title;
                        model.Message = "";
                    }
                    else
                    {
                        model.Message = "You have already attend this exam";
                    }
                }
                return View(model);

            }
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public IActionResult AttendExam(AttendExamViewModel attendExamViewModel)
        {
            bool result = _studentService.SetExamResult(attendExamViewModel);
            return RedirectToAction("AttendExam");
        }
        public IActionResult Result(string studentId)
        {
            var model = _studentService.GetExamResult(Convert.ToInt32(studentId));
            return View(model);

        }
        public IActionResult ViewResult()
        {
            LoginViewModel sessionObj = HttpContent.Session.Get<LoginViewModel>("loginvm");
            if (sessionObj != null)
            {
                var model = _studentService.GetExamResults(Convert.ToInt32(sessionObj.Id));
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }
        public IActionResult Profile()
        {
            LoginViewModel sessionObj = HttpContent.Session.Get<LoginViewModel>("loginvm");
            if (sessionObj != null)
            {
                var model = _studentService.GetStudentDatails(Convert.ToInt32(sessionObj.Id));
                if (model.PicturefileName != null)
                {
                    model.PicturefileName = ConfigurationManager.GetFilepath() + model.PicturefileName;

                }
                model.CVFileName = ConfigurationManager.GetFilepath() + model.CVFileName;
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }
        public IActionResult profile([FromForm] StudentViewModel studentViewModel)
        {
            if (studentViewModel.PictureFile != null)
            {
                studentViewModel.PictureFile = SaveStudentFile(studentViewModel.PictureFile);
                if (studentViewModel.CVFile != null)
                {
                    studentViewModel.CVFileName = SaveStudentFile(studentViewModel.CVFile);
                    _studentService.UpdateAsync(studentViewModel);
                    return RedirectToAction("Profile");
                }
            }
        }

        private string SaveStudentFile(IFormFile pictureFile)
        {
            if (pictureFile != null)
            {
                return String.Empty;
            }
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/file");
            return SaveFile(path, pictureFile);
        }

        private string SaveFile(string path, IFormFile? pictureFile)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

            }
            var filename = Guid.NewGuid().ToString() + "." + pictureFile.FileName.Split('.')
                 [pictureFile.FileName.Split('.').Length - 1];
            path = Path.Combine(path, filename);
            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                pictureFile.CopyTo(stream);
            }
            return filename;
        }
       
      }

    }
}
