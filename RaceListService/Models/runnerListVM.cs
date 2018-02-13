using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RaceListService.Models
{
    public class runnerListVMItem
    {

        [Display(Name = "First Name")]
        public string firstname { get; set; }
        [Display(Name = "Family Name")]
        public string secondname { get; set; }
        [Display(Name = "Select")]
        public bool Active { get; set; }
        public int EFKey { get; set; }
    }

    public class runnerListVM
    {

        public List<int> SelectedRunnerIDs { get; set; }

        public List<runnerListVMItem> listOfRunners;

        public List<runnerListVMItem> selectedRunners;

        public static List<int> buildSelectedList(IEnumerable<runnerListVMItem> listofRunners)
        {
            List<int> list = new List<int>();
            foreach (var r in listofRunners)
                if (r.Active)
                {
                    list.Add(r.EFKey);
                }
            return list;

        }

        public static IEnumerable<runnerListVMItem> buildVM(IEnumerable<runners.runner> all)
        {
            var vm = all.Select(p => new runnerListVMItem
            {
                 firstname  = p.firstname,
                 secondname = p.secondname,
                  Active = (bool)p.Active,
                   EFKey = p.EFKey
                   
            }
            ).AsEnumerable().OrderBy(r => r.firstname).OrderBy(r => r.secondname).OrderByDescending(r => r.Active);

            return vm;

        }

        public static IEnumerable<runnerListVMItem> buildVM(IEnumerable<RunningModel.runner> all)
        {
            var vm = all.Select(p => new runnerListVMItem
            {
                firstname = p.firstname,
                secondname = p.secondname,
                Active = (bool)p.Active,
                EFKey = p.EFKey

            }
            ).AsEnumerable().OrderBy(r => r.firstname).OrderBy(r => r.secondname).OrderByDescending(r => r.Active);

            return vm;

        }

    }
}