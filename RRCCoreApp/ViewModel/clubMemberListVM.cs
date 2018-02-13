using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RRCCoreApp.ViewModel
{
    public class ClubMemberListVM
    {
        [Display(Name = "EAA Number")]
        public int AmNumber { get; set; }
        [Display(Name = "Name")]
        string Name { get; set; }


        public static IEnumerable<ClubMemberListVM> buildVM(IEnumerable<Models.ClubMember> listIN)
        {
            var vm = listIN.Select(m => new ClubMemberListVM
            {
                AmNumber = m.AmNumber, 
                Name = m.Name 
            }

            ).AsEnumerable();

            return vm;
        }
    }
}
