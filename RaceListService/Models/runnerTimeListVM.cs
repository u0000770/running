using RunningModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RaceListService.Models
{
    public class runnerTimeItemVM
    {
        public int EFKey { get; set; }
        public int RunnerId { get; set; }
        public int EventId { get; set; }
        public int Target { get; set; }
        public int Actual { get; set; }
        //public System.DateTime Date { get; set; }
        public bool Active { get; set; }
        public int PredictedTimeValue { get; set; }

        [Display(Name = "Event Race Distance")]
        public string RaceDistance { get; set; }
        [Display(Name = "Event Race Target")]
        public string RaceTargetTime { get; set; }
        [Display(Name = "Event Race Actual Time")]
        public string RaceActualTime { get; set; }
        [Display(Name = "Event Race Date")]
        [DataType(DataType.Date)]
        public DateTime RaceDate { get; set; }
        [Display(Name = "Time Difference")]
        public string TimeDifference { get; set; }
        [Display(Name = "HC Prediction")]
        public string Predicted { get; set; }





        public static string formatResult(int result)
        {
            string resultString = "No Result";
            if (result > 0)
            {

                TimeSpan t = TimeSpan.FromSeconds(result);

                resultString = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                                t.Hours,
                                t.Minutes,
                                t.Seconds);

            }

            return resultString;
        }
    }


    public class runnerTimeListVM
    {
        public int runnerId { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Median")]
        public string medianTime { get; set; }

        [Display(Name = "Average")]
        public string averageTime { get; set; }

        [Display(Name = "Last Race Prediction")]
        public string lastRaceTime { get; set; }

        [Display(Name = "Recommended Predicted")]
        public string recTime { get; set; }


        public List<int> SelectedRaceIDs { get; set; }

        public List<runnerTimeItemVM> listOfRaces;

        public List<runnerTimeItemVM> selectedRaces;

        private static double CalculateHCPredicion(double oldDistance, int oldtime)
        {
            double distances = 8046.72;
            return (RaceCalc.calcPredictedTime(oldDistance, Convert.ToDouble(distances), oldtime) + RaceCalc.cameron(oldDistance, Convert.ToDouble(distances), oldtime)) / 2;
        }

        public static List<int> buildSelectedList(IEnumerable<runnerTimeItemVM> listofRaces)
        {
            List<int> list = new List<int>();
            foreach (var r in listofRaces)
                if (r.Active)
                {
                    list.Add(r.EFKey);
                }
            return list;

        }

        public static IEnumerable<runnerTimeItemVM> buildVM(IEnumerable<EventRunnerTime> all, RunningModelEntities db)
        {

        

            List<runnerTimeItemVM> vm = new List<runnerTimeItemVM>();

            foreach (var ert in all)
            {
                var item = new runnerTimeItemVM();
                item.Actual = (ert.Actual == null) ? 0 : (int)ert.Actual;
                item.Active = (ert.Active == null) ? false : (bool)ert.Active;
                item.EFKey = ert.EFKey;
                var distance = db.distances.SingleOrDefault(d => d.Code == ert.Event.DistanceCode);
                item.RaceDistance = distance.Name;
                item.RaceActualTime = runnerTimeItemVM.formatResult((int)ert.Actual);
                var date = (DateTime)ert.Date;
                item.RaceDate = date.Date;
                if (ert.Target != null) { 
                item.RaceTargetTime = runnerTimeItemVM.formatResult((int)ert.Target);
                }
                double predicted = CalculateHCPredicion(distance.Value, item.Actual);
                item.PredictedTimeValue = (int)predicted;
                item.Predicted = runnerTimeItemVM.formatResult((int)predicted);
                TimeDifference(ert, item);
                vm.Add(item);
            }

            var result = vm.OrderBy(r => r.PredictedTimeValue);

            return result;

            
            }

        private static void TimeDifference(EventRunnerTime ert, runnerTimeItemVM item)
        {
            if (ert.Target != null)
            { 
                int td = 0;
                if (item.Actual != 0)
                {
                    td = (int)ert.Target - item.Actual;
                }

                if (td > 0)
                {
                    item.TimeDifference = td.ToString();
                }
                else
                {
                    item.TimeDifference = 0.ToString();
                }
            }
        }

        // return listOfRaces;

    }
   
}


