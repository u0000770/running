using ClassLibrary1;
using runners;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaceListService.Models
{

    public class TrophyVM {

        public TrophyVM() { }
        public List<jrListItemVM> jrTrophyList { get; set; }
        public List<jrListItemVM> dbTrophyList { get; set; }
    }
    public class jrListItemVM
    {
        public jrListItemVM()
        {
        }

        public bool admin { get; set; }
        public int RunnerId { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Points")]
        public int jrPoints { get; set; }
        [Display(Name = "Races")]
        public int races { get; set; }
        [Display(Name = "Time Difference")]
        public int timeDiff { get; set; }
        public bool JR { get; set; }

    }

    public class ClubMemberDetailVM
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }


    }


    public class ClubMemberListItemVM
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Race Type")]
        public string Distance { get; set; }
        [Display(Name = "Target Time")]
        public string Time { get; set; }



        public List<SelectListItem> selectRaceList { get; set; }

        public string selectedRace { get; set; }

        public List<SelectListItem> buildSelectRaceList()
        {
            List<SelectListItem> slist = new List<SelectListItem>();
            var races = new RaceDetails();
            races.BuildTestList();
            foreach (var item in races.list)
            {
                var option = new SelectListItem()
                {
                    Text = item.title,
                    Value = item.meters.ToString()
                };
                slist.Add(option);
            }
            return slist;
        }


        public static IEnumerable<ClubMemberListItemVM> buildVM(IEnumerable<ClassLibrary1.memberList> listIN)
        {
            var vm = listIN.Select(m => new ClubMemberListItemVM
            {
                  Name = m.Name,
                   Distance = RaceDetails.GetByRaceNameByMeters((double)m.Distance),
                    Time = RaceDetails.formatResult(m.Time),
                     Id = m.Id
            }

            ).AsEnumerable();

            return vm;
        }
    }
}