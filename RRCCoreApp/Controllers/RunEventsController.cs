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
    public class RunEventsController : Controller
    {
        private readonly Event_ResultContext _context;

        public RunEventsController(Event_ResultContext context)
        {
            _context = context;
        }

        // GET: RunEvents
        public async Task<IActionResult> Index()
        {
            return View(await _context.RunEvent.ToListAsync());
        }

        // GET: RunEvents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runEvent = await _context.RunEvent
                .SingleOrDefaultAsync(m => m.Id == id);
            if (runEvent == null)
            {
                return NotFound();
            }

            return View(runEvent);
        }

        // GET: RunEvents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RunEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Location,Date,DistanceLength,DistanceMetric,CompetitionCode,TypeCode")] RunEvent runEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(runEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(runEvent);
        }

        // GET: RunEvents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runEvent = await _context.RunEvent.SingleOrDefaultAsync(m => m.Id == id);
            if (runEvent == null)
            {
                return NotFound();
            }
            return View(runEvent);
        }

        // POST: RunEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Location,Date,DistanceLength,DistanceMetric,CompetitionCode,TypeCode")] RunEvent runEvent)
        {
            if (id != runEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(runEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RunEventExists(runEvent.Id))
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
            return View(runEvent);
        }

        // GET: RunEvents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var runEvent = await _context.RunEvent
                .SingleOrDefaultAsync(m => m.Id == id);
            if (runEvent == null)
            {
                return NotFound();
            }

            return View(runEvent);
        }

        // POST: RunEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var runEvent = await _context.RunEvent.SingleOrDefaultAsync(m => m.Id == id);
            _context.RunEvent.Remove(runEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RunEventExists(int id)
        {
            return _context.RunEvent.Any(e => e.Id == id);
        }
    }
}
