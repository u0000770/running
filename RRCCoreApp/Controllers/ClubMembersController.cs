using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RRCCoreApp.Models;
using RRCCoreApp.ViewModel;

namespace RRCCoreApp.Controllers
{
    public class ClubMembersController : Controller
    {
        private readonly Event_ResultContext _context;

        public ClubMembersController(Event_ResultContext context)
        {
            _context = context;
        }

        // GET: ClubMembers
        public async Task<IActionResult> Index()
        {
            var allMembers = await _context.ClubMember.ToListAsync();
            var vm = ViewModel.ClubMemberListVM.buildVM(allMembers);
            return View(vm);
        }

        // GET: ClubMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           

            var clubMember = await _context.ClubMember.SingleOrDefaultAsync(m => m.AmNumber == id);
            if (clubMember == null)
            {
                return NotFound();
            }
            IEnumerable<RRCCoreApp.Models.RaceMemberResult> results = _context.RaceMemberResult.Where(x => x.Athlete.AmNumber == clubMember.AmNumber).Include(p => p.Event);
            var ViewModel = new ViewModel.MemberRaceListVM(results);
            ViewModel.AmNumber = clubMember.AmNumber;
            return View(ViewModel);
        }

        public async Task<IActionResult> Predict(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


          
            var clubMember = _context.ClubMember.SingleOrDefault(m => m.AmNumber == id);
            if (clubMember == null)
            {
                return NotFound();
            }
            IEnumerable<RRCCoreApp.Models.RaceMemberResult> results = _context.RaceMemberResult.Where(x => x.Athlete.AmNumber == clubMember.AmNumber).Include(p => p.Event).Where(e => e.Event.CompetitionCode == "JR");
            var MRViewModel = new ViewModel.MemberRaceListVM(results);
            var LastRace = MRViewModel.list.OrderByDescending(r => r.Date).First();
            RRCCoreApp.Models.Race races = new Race();
            races.BuildTestList();
            
            RRCCoreApp.ViewModel.PredictTimesVM vm = new ViewModel.PredictTimesVM();
            vm.predictionList = new List<PredictTimeListItemVM>();
            vm.AthleteNumber = clubMember.AmNumber;
            string initialDistanceStr = LastRace.Distance;
            double initialDistanceMtr = RRCCoreApp.Models.Race.GetMetersByName(initialDistanceStr.Trim());
            int initialDistanceSec = LastRace.rawResult;
            foreach (var race in races.list)
            {
                RRCCoreApp.ViewModel.RaceListVM r = new ViewModel.RaceListVM();
               // PredictTimeListItemVM ptvm = new PredictTimeListItemVM();
                //t2 = t1 * (d2 / d1) ^ 1.06
                //where t1 equals the initial time, 
                //d1 equals the initial distance, 
                //d2 equals the new distance being calculated for, and 
                //t2 equals the predicted time for the new distance.
                
               
                double newDistanceMtr = race.meters;
                var predictedTime = calcPredictedTime(initialDistanceMtr, newDistanceMtr, initialDistanceSec);
                var camtime = cameron(initialDistanceMtr, newDistanceMtr, initialDistanceSec);


                RRCCoreApp.ViewModel.PredictTimeListItemVM predictTimeListVM = new PredictTimeListItemVM();
                predictTimeListVM.rawResult = (int)predictedTime;
                predictTimeListVM.raceType = race.title;
                predictTimeListVM.predictedTime = PredictTimesVM.formatResult(predictTimeListVM.rawResult);
                predictTimeListVM.camTime = PredictTimesVM.formatResult((int)camtime);
                vm.predictionList.Add(predictTimeListVM);
            }

            vm.lastRaceRawTime = LastRace.rawResult;
            vm.lastRaceTime = LastRace.result;
            vm.lastRaceType = LastRace.Distance;
            return View("Edit", vm);
        }

        private double calcPredictedTime(double d1, double d2, int t1 )
        {
            double con = 1.06;
            //t2 = t1 * (d2 / d1) ^ 1.06
            // d3 = (d2 / d1)
            double d3 = d2 / d1;
            // w = d3^ 1.06
            double w = Math.Pow(d3, con);
            // t2 = t1 * w
            double t2 = t1 * w;
            return t2;
        }

        private double cameron(double old_dist,double new_distance, double old_time)
        {
            //a = 13.49681 - (0.000030363 * old_dist) + (835.7114 / (old_dist ^ 0.7905))
            //b = 13.49681 - (0.000030363 * new_dist) + (835.7114 / (new_dist ^ 0.7905))
            //new_time = (old_time / old_dist) * (a / b) * new_dist

            // x = (0.000030363 * old_dist)
            double x = (0.000030363 * old_dist);
            // y = (old_dist ^ 0.7905)
            double y = Math.Pow(old_dist, 0.7905);
            // z = (835.7114 / y )
            double z = 835.7114 / y;
            double a = 13.49681 - x + z;

            double w = Math.Pow(new_distance, 0.7905);
            double v = 835.7114 / w;
            double t = 0.000030363 * new_distance;


            double b = 13.49681 - t + v;

            double new_time = (old_time / old_dist) * (a / b) * new_distance;
            return new_time;

        }

        // GET: ClubMembers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClubMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AmNumber,Active")] ClubMember clubMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clubMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clubMember);
        }

        // GET: ClubMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clubMember = await _context.ClubMember.SingleOrDefaultAsync(m => m.Id == id);
            if (clubMember == null)
            {
                return NotFound();
            }
            return View(clubMember);
        }

        // POST: ClubMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AmNumber,Active")] ClubMember clubMember)
        {
            if (id != clubMember.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clubMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubMemberExists(clubMember.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(clubMember);
        }

        // GET: ClubMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clubMember = await _context.ClubMember
                .SingleOrDefaultAsync(m => m.Id == id);
            if (clubMember == null)
            {
                return NotFound();
            }

            return View(clubMember);
        }

        // POST: ClubMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clubMember = await _context.ClubMember.SingleOrDefaultAsync(m => m.Id == id);
            _context.ClubMember.Remove(clubMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClubMemberExists(int id)
        {
            return _context.ClubMember.Any(e => e.Id == id);
        }
    }
}
