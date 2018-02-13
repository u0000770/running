using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RRCCoreApp.Models;

namespace RRCCoreApp.Controllers
{
    public class RaceMemberResultsController : Controller
    {
        private readonly Event_ResultContext _context;

        public RaceMemberResultsController(Event_ResultContext context)
        {
            _context = context;
        }

        // GET: RaceMemberResults
        public async Task<IActionResult> Index()
        {
            var event_ResultContext = _context.RaceMemberResult.Include(r => r.Athlete).Include(r => r.Event);
            return View(await event_ResultContext.ToListAsync());
        }

        // GET: RaceMemberResults/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceMemberResult = await _context.RaceMemberResult
                .Include(r => r.Athlete)
                .Include(r => r.Event)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (raceMemberResult == null)
            {
                return NotFound();
            }

            return View(raceMemberResult);
        }

        // GET: RaceMemberResults/Create
        public IActionResult Create()
        {
            ViewData["AthleteId"] = new SelectList(_context.ClubMember, "Id", "Id");
            ViewData["EventId"] = new SelectList(_context.RunEvent, "Id", "CompetitionCode");
            return View();
        }

        // POST: RaceMemberResults/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AthleteId,EventId,Result")] RaceMemberResult raceMemberResult)
        {
            if (ModelState.IsValid)
            {
                _context.Add(raceMemberResult);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AthleteId"] = new SelectList(_context.ClubMember, "Id", "Id", raceMemberResult.AthleteId);
            ViewData["EventId"] = new SelectList(_context.RunEvent, "Id", "CompetitionCode", raceMemberResult.EventId);
            return View(raceMemberResult);
        }

        // GET: RaceMemberResults/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceMemberResult = await _context.RaceMemberResult.SingleOrDefaultAsync(m => m.Id == id);
            if (raceMemberResult == null)
            {
                return NotFound();
            }
            ViewData["AthleteId"] = new SelectList(_context.ClubMember, "Id", "Id", raceMemberResult.AthleteId);
            ViewData["EventId"] = new SelectList(_context.RunEvent, "Id", "CompetitionCode", raceMemberResult.EventId);
            return View(raceMemberResult);
        }

        // POST: RaceMemberResults/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AthleteId,EventId,Result")] RaceMemberResult raceMemberResult)
        {
            if (id != raceMemberResult.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(raceMemberResult);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RaceMemberResultExists(raceMemberResult.Id))
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
            ViewData["AthleteId"] = new SelectList(_context.ClubMember, "Id", "Id", raceMemberResult.AthleteId);
            ViewData["EventId"] = new SelectList(_context.RunEvent, "Id", "CompetitionCode", raceMemberResult.EventId);
            return View(raceMemberResult);
        }

        // GET: RaceMemberResults/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceMemberResult = await _context.RaceMemberResult
                .Include(r => r.Athlete)
                .Include(r => r.Event)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (raceMemberResult == null)
            {
                return NotFound();
            }

            return View(raceMemberResult);
        }

        // POST: RaceMemberResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var raceMemberResult = await _context.RaceMemberResult.SingleOrDefaultAsync(m => m.Id == id);
            _context.RaceMemberResult.Remove(raceMemberResult);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RaceMemberResultExists(int id)
        {
            return _context.RaceMemberResult.Any(e => e.Id == id);
        }
    }
}
