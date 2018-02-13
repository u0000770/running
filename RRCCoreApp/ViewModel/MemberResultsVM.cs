using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RRCCoreApp.ViewModel
{

  

    public class RaceListVM
    {
        [Display(Name = "Race Type")]
        public string Distance { get; set; }
        [Display(Name = "Detail")]
        public string Title { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date{ get; set; }
        [Display(Name = "H:M:S")]
        public string result { get; set; }
        public int rawResult { get; set; }
    }


    public class MemberRaceListVM : ClubMemberListVM
    {
        public List<RaceListVM> list { get; set; }

        public string nextRace { get; set; }

        public MemberRaceListVM(IEnumerable<RRCCoreApp.Models.RaceMemberResult> results)
        {
            List<RaceListVM> list = new List<RaceListVM>();
            foreach (RRCCoreApp.Models.RaceMemberResult r in results)
            {

                RaceListVM rli = new RaceListVM();
                rli.Distance = r.Event.DistanceLength.ToString() + " " + r.Event.DistanceMetric;
                rli.Title = r.Event.Title;
                rli.Date = r.Event.Date;
                rli.rawResult = r.Result;
                rli.result = formatResult(r.Result);
                list.Add(rli);
            }

            this.list = list.OrderBy(c => c.Date).ToList();
        }

        //public calcNext()
        //{
        //    var sortedList = this.list.OrderBy(x => x.Date);
        //    var firstEvent = sortedList.First();

        //}
        

        private static string formatResult(int result)
        {
            TimeSpan t = TimeSpan.FromSeconds(result);

            string resultString = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                            t.Hours,
                            t.Minutes,
                            t.Seconds);


            return resultString;
        }
    }





}
