﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnilneExa.DataAccessLyer
{
    public class QnAs
    {
        public int Id { get; set; }
        public int ExamsId { get; set; }
        public Exams Exams { get; set; }
        public string Quenstion { get; set; }
        public int Answer { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; } 
        public string Option4 { get; set; }
        public ICollection<ExamResults> ExamResults { get; set; } = new HashSet<ExamResults>();

    }
}
