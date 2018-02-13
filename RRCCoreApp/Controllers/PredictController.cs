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
    public class PredictController : Controller
    {
        private readonly Event_ResultContext _context;

        public PredictController(Event_ResultContext context)
        {
            _context = context;
        }

        // GET: Predict
        public IActionResult Index()
        {
            // 
            var AllMembers = _context.ClubMember;
            NextRaceListVM vm = new NextRaceListVM();
            AllMembers.Select(m => new PredictTimesVM
            {
                Name = m.Name
            });
            return View(vm);
        }

        // GET: Predict/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Predict/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Predict/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AmNumber,Active,Name")] ClubMember clubMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clubMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clubMember);
        }

        // GET: Predict/Edit/5
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

        // POST: Predict/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AmNumber,Active,Name")] ClubMember clubMember)
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

        // GET: Predict/Delete/5
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

        // POST: Predict/Delete/5
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
