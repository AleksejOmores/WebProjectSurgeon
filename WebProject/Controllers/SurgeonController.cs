using Microsoft.AspNetCore.Mvc;
using WebProject.Models;
using System.Linq;

namespace WebProject.Controllers
{
    public class SurgeonController : Controller
    {
        private readonly HospitalSurgeryContext _context;

        public SurgeonController(HospitalSurgeryContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Получаем список всех хирургов из базы данных
            var surgeons = _context.Surgeons.ToList();
            return View(surgeons);
        }

        [HttpGet]
        public IActionResult AddSurgeon()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSurgeon(Surgeon surgeon)
        {
            if (ModelState.IsValid)
            {
                _context.Surgeons.Add(surgeon);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(surgeon);
        }

        [HttpGet]
        public IActionResult EditSurgeon(int id)
        {
            var surgeon = _context.Surgeons.FirstOrDefault(s => s.SurgeonId == id);
            if (surgeon == null)
            {
                return NotFound();
            }

            return View(surgeon);
        }

        [HttpPost]
        public IActionResult EditSurgeon(Surgeon surgeon)
        {
            if (ModelState.IsValid)
            {
                _context.Surgeons.Update(surgeon);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(surgeon);
        }
    }
}
