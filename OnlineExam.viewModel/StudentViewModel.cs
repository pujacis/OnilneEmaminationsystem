using Microsoft.AspNetCore.Http;
using OnilneExa.DataAccessLyer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.viewModel
{
    public class StudentViewModel
    {
        public StudentViewModel()
        {

        }
        public int Id { get; set; }
        [Required]
        [Display(Name ="Student Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Contct no")]
        public string contact { get; set; }
        [Display(Name = "CV")]
        public string CVFileName { get; set; }
        public string PicturefileName { get; set; }
        public int? GroupsId { get; set; }
        public IFormFile PictureFile { get; set; }
        public IFormFile CVFile { get; set; }
        public int TotalCount { get; set; }
        public List<StudentViewModel> StudentList { get; set; }
        public StudentViewModel(Students model)
        {
            Id = model.Id;
            Name = model.Name ?? "";
            UserName = model.UserName;
            Password = model.Password;
            contact = model.contact ?? "";
            CVFileName = model.CVFileName ?? "";
            PicturefileName = model.PicturefileName ?? "";
            GroupsId = model.GroupsId;
        }
        public Students ConvertViewModel(StudentViewModel vm)
        {
            return new Students
            {
                Id = vm.Id,
                Name = vm.Name ?? "",
                UserName = vm.UserName,
                Password = vm.Password,
                contact = vm.contact ?? "",
                CVFileName = vm.CVFileName ?? "",
                PicturefileName = vm.PicturefileName ?? "",
                GroupsId = vm.GroupsId,
            };
        }
               

    }
}
