using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWCFClient.Models
{
    public class RunnerDTO
    {

        public int EFKey { get; set; }
        public string firstname { get; set; }
        public string secondname { get; set; }
        public string ukan { get; set; }
        public Nullable<System.DateTime> dob { get; set; }
        public string email { get; set; }
        public Nullable<bool> Active { get; set; }
        public string ageGradeCode { get; set; }
    }
}